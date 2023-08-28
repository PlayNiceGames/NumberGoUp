using GameAudio;
using ServiceLocator;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        private Audio _audio;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

        public void ClickPlay()
        {
            _audio.PlayClick();

            GameSceneManager.LoadEndlessMode();
        }

        public void ClickTutorial()
        {
            _audio.PlayClick();

            GameSceneManager.LoadTutorial();
        }
    }
}