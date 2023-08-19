using Cysharp.Threading.Tasks;
using GameAds;
using GameAudio;
using ServiceLocator;
using SimpleTextProvider;
using Tiles;
using Tiles.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameOver
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextProviderTMP _currentScoreLabel;
        [SerializeField] private TextProviderTMP _highScoreLabel;
        [SerializeField] private RectTransform _biggestTileRoot;
        [SerializeField] private Button _continueButton;
        [SerializeField] private TileFactory _tileFactory;

        private AdsService _adsService;
        private Audio _audio;

        private Tile _biggestTile;
        private UniTaskCompletionSource<GameOverAction> _buttonClicked;

        public void Setup()
        {
            _adsService = GlobalServices.Get<AdsService>();
            _audio = GlobalServices.Get<Audio>();

            _currentScoreLabel.Setup();
            _highScoreLabel.Setup();

            Hide();
        }

        public async UniTask<GameOverAction> ShowWithResult(int currentScore, int highScore, ValueTileData biggestTileData, bool isEnabledContinueButton)
        {
            SetData(currentScore, highScore, biggestTileData, isEnabledContinueButton);

            gameObject.SetActive(true);

            _audio.PlayGameOver();

            GameOverAction finalAction = await ProcessButtonClicksUntilResult();

            Hide();

            return finalAction;
        }

        private async UniTask<GameOverAction> ProcessButtonClicksUntilResult()
        {
            while (true)
            {
                _buttonClicked = new UniTaskCompletionSource<GameOverAction>();
                GameOverAction buttonClickResult = await _buttonClicked.Task;

                if (buttonClickResult == GameOverAction.Continue)
                {
                    RewardedAdShowResult adShowResult = await _adsService.ShowRewardedAd("game_over");

                    switch (adShowResult)
                    {
                        case RewardedAdShowResult.Unavailable:
                            await _adsService.ShowAdUnavailableMessage();
                            break;
                        case RewardedAdShowResult.Rewarded:
                            return GameOverAction.Continue;
                    }
                }
                else
                {
                    return buttonClickResult;
                }
            }
        }

        private void SetData(int currentScore, int highScore, ValueTileData biggestTileData, bool isEnabledContinueButton)
        {
            _currentScoreLabel.FillText(currentScore);
            _highScoreLabel.FillText(highScore);

            _biggestTile = _tileFactory.InstantiateTile(biggestTileData);

            _biggestTile.SetParent(_biggestTileRoot);
            _biggestTile.transform.localPosition = Vector3.zero;

            _continueButton.interactable = isEnabledContinueButton;
        }

        public void Hide()
        {
            if (_biggestTile != null)
                _biggestTile.Dispose();

            gameObject.SetActive(false);
        }

        public void OnContinueButtonClicked()
        {
            _buttonClicked?.TrySetResult(GameOverAction.Continue);
        }

        public void OnExitButtonClicked()
        {
            _buttonClicked?.TrySetResult(GameOverAction.Exit);
        }
    }
}