using GameAudio;
using ServiceLocator;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private MainMenuBackground _background;

        private Audio _audio;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

        public async void ClickPlay()
        {
            _audio.PlayClick();
            
            await _background.PlayTransition();

            GameSceneManager.LoadEndlessMode();
        }
        
        public async void ClickTutorial()
        {
            _audio.PlayClick();

            await _background.PlayTransition();

            GameSceneManager.LoadTutorial();
        }
    }
}