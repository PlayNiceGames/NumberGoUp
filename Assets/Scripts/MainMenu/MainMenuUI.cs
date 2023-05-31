using UnityEngine;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public void ClickPlay()
        {
            GameSceneManager.LoadEndlessMode();
        }

        public void ClickTutorial()
        {
            GameSceneManager.LoadTutorial();
        }
    }
}