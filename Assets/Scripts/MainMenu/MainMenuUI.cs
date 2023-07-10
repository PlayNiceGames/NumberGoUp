using GameAudio;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public void ClickPlay()
        {
            GameSounds.PlayClick();
            
            GameSceneManager.LoadEndlessMode();
        }

        public void ClickTutorial()
        {
            GameSounds.PlayClick();
            
            GameSceneManager.LoadTutorial();
        }
    }
}