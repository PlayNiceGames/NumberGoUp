using GameAudio;
using GameSave;
using ServiceLocator;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private NewGameDialogUI _newGameDialog;
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

            GameData currentSave = _gameSaveService.CurrentSave;
            if (currentSave != null)
            {
                NewGameDialogResult dialogResult = await _newGameDialog.Show();

                if (dialogResult == NewGameDialogResult.NewGame)
                {
                    currentSave = null;
                    
                    _gameSaveService.DeleteCurrentSave();
                }
            }

            await _background.PlayTransition(); //TODO create good system for UI transition

            GameSceneManager.LoadEndlessMode(currentSave);
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