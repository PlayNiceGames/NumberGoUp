using UnityEngine;

namespace GameLoadingScene
{
    public class QualitySetter : MonoBehaviour
    {
        private void Awake()
        {
            SetQualitySettings();
        }

        private static void SetQualitySettings()
        {
            if (!Application.isMobilePlatform)
                return;

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}