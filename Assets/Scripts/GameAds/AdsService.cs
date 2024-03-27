using Analytics;
using Cysharp.Threading.Tasks;
using ServiceLocator;
using UnityEngine;

namespace GameAds
{
    public class AdsService : MonoBehaviour
    {
        private const string IronSourceAppKey = "1ad55a1fd";

        [SerializeField] private AdUnavailableMessageUI _adUnavailableMessagePrefab;

        private AnalyticsService _analytics;

        private void Start()
        {
            _analytics = GlobalServices.Get<AnalyticsService>();

            Initialize();
        }

        private void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        private void Initialize()
        {
            IronSource.Agent.init(IronSourceAppKey);
            IronSource.Agent.validateIntegration();
        }

        public UniTask<RewardedAdShowResult> ShowRewardedAd(string placementName)
        {
            Debug.Log("Rewarded ad show called");
            RewardedAd ad = new RewardedAd(placementName, _analytics);
            return ad.Show();
        }

        public async UniTask ShowAdUnavailableMessage()
        {
            AdUnavailableMessageUI message = InstantiateAdUnavailableMessage();

            await message.Show();
            await message.ShowDelay();
            await message.Hide();

            message.Dispose();
        }

        private AdUnavailableMessageUI InstantiateAdUnavailableMessage()
        {
            return Instantiate(_adUnavailableMessagePrefab);
        }
    }
}