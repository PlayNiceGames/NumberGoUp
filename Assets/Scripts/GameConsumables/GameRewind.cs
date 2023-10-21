using System.Threading;
using Cysharp.Threading.Tasks;
using GameAds;
using GameLoop;
using GameSave;
using ServiceLocator;
using UnityEngine;

namespace GameConsumables
{
    public class GameRewind : MonoBehaviour
    {
        [SerializeField] private GameButton _button;

        private GameSaveService _saveService;
        private AdsService _adsService;

        private bool IsRewindAvailable => _saveService.LastSavesCount() > 0;

        protected void Awake()
        {
            _saveService = GlobalServices.Get<GameSaveService>();
            _adsService = GlobalServices.Get<AdsService>();

            _saveService.OnSavesChanged += UpdateButton;
        }

        private void OnDestroy()
        {
            _saveService.OnSavesChanged -= UpdateButton;
        }

        private void Start()
        {
            UpdateButton();
        }

        public UniTask WaitForClick(CancellationToken cancellationToken)
        {
            return _button.WaitForClick(cancellationToken);
        }

        public async UniTask<bool> ProcessInput()
        {
            if (!IsRewindAvailable)
                return false;

            if (Application.isEditor)
            {
                await _adsService.ShowAdUnavailableMessage();

                return true;
            }

            RewardedAdShowResult adShowResult = await _adsService.ShowRewardedAd("rewind");

            switch (adShowResult)
            {
                case RewardedAdShowResult.Unavailable:
                    await _adsService.ShowAdUnavailableMessage();
                    return false;
                case RewardedAdShowResult.Rewarded:
                    return true;
            }

            return false;
        }

        private void UpdateButton()
        {
            _button.SetEnabled(IsRewindAvailable);
        }

        public void SetEnabled(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
        }
    }
}