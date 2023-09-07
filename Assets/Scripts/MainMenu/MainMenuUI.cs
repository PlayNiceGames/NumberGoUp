using Cysharp.Threading.Tasks;
using GameAudio;
using GameSave;
using ServiceLocator;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private MainMenuBackground _background;

        private Audio _audio;
        private GameSaveService _gameSaveService;

        private bool _isLoadingScene;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
            _gameSaveService = GlobalServices.Get<GameSaveService>();
        }

        public async void ClickPlay()
        {
            if (_isLoadingScene)
                return;

            _isLoadingScene = true;

            _audio.PlayClick();

            GameData currentSave = _gameSaveService.LastSave;
            if (currentSave != null)
            {
                NewGameDialogResult dialogResult = await ShowSaveDialog();

                if (dialogResult == NewGameDialogResult.NewGame)
                    currentSave = null;
            }

            await _background.PlayTransition(); //TODO create google system for UI transition

            GameSceneManager.LoadEndlessMode(currentSave);
        }

        private async UniTask<NewGameDialogResult> ShowSaveDialog()
        {
            return NewGameDialogResult.Continue;
        }

        public async void ClickTutorial()
        {
            if (_isLoadingScene)
                return;

            _isLoadingScene = true;

            _audio.PlayClick();

            await _background.PlayTransition();

            GameSceneManager.LoadTutorial();
        }
    }
}