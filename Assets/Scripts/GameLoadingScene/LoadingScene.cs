using UnityEngine;

namespace GameLoadingScene
{
    public class LoadingScene : MonoBehaviour
    {
        private const string FirstLaunchHappenedKey = "first_launch_happened";

        private void Start()
        {
            if (IsFirstLaunch())
            {
                RecordFirstLaunch();

                GameSceneManager.LoadTutorial();
            }
            else
            {
                GameSceneManager.LoadMainMenu();
            }
        }

        private bool IsFirstLaunch()
        {
            return !PlayerPrefs.HasKey(FirstLaunchHappenedKey);
        }

        private void RecordFirstLaunch()
        {
            PlayerPrefs.SetInt(FirstLaunchHappenedKey, 1);
            PlayerPrefs.Save();
        }
    }
}