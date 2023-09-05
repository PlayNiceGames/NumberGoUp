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

        private bool _isLoadingScene;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

        public async void ClickPlay()
        {
            if (_isLoadingScene)
                return;

            _isLoadingScene = true;

            _audio.PlayClick();

            GameData currentSave = _gameSaveService.CurrentSave;
            if (currentSave != null)
                await ShowSaveDialog();

            await _background.PlayTransition();

            GameSceneManager.LoadEndlessMode();
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