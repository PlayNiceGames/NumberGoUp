using JetBrains.Annotations;
using UnityEngine;

namespace GameSettings
{
    public class GameExitButton : MonoBehaviour
    {
        [UsedImplicitly]
        public void ClickExitButton()
        {
            GameSceneManager.LoadMainMenu();
        }
    }
}