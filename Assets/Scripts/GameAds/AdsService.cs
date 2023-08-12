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
            
            ShowBannerAd();
        }

        private void ShowBannerAd()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.LARGE, IronSourceBannerPosition.BOTTOM);
            IronSource.Agent.displayBanner();
        }
    }
}