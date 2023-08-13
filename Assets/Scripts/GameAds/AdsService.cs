using UnityEngine;

namespace GameAds
{
    public class AdsService : MonoBehaviour
    {
        private const string IronSourceAppKey = "1ad55a1fd";

        private void Start()
        {
            IronSource.Agent.init(IronSourceAppKey);
            IronSource.Agent.validateIntegration();
        }

        void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }
    }
}