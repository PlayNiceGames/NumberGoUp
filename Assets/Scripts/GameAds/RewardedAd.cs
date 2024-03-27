using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Game;
using UnityEngine;

namespace GameAds
{
    public class RewardedAd
    {
        private const float AdRewardTimeoutSeconds = 0.5f;

        private readonly string _placementName;
        private bool _adClosed;
        private bool _adRewarded;

        private bool _adShowFailed;

        private readonly AnalyticsService _analytics;

        public RewardedAd(string placementName, AnalyticsService analytics)
        {
            _placementName = placementName;
            _analytics = analytics;

            IronSourceRewardedVideoEvents.onAdClosedEvent += OnAdClosed;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += OnAdShowFailed;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += OnAdRewarded;
        }

        ~RewardedAd()
        {
            IronSourceRewardedVideoEvents.onAdClosedEvent -= OnAdClosed;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent -= OnAdShowFailed;
            IronSourceRewardedVideoEvents.onAdRewardedEvent -= OnAdRewarded;
        }

        public async UniTask<RewardedAdShowResult> Show()
        {
            Debug.Log("ad show method called");
            if (!IronSource.Agent.isRewardedVideoAvailable())
            {
                Debug.Log("rewarded ad is not available");
                IronSource.Agent.loadRewardedVideo();
                return RewardedAdShowResult.Unavailable;
            }

            _adShowFailed = false;
            _adClosed = false;
            _adRewarded = false;

            Debug.Log("rewarded ad show method called 001");
            IronSource.Agent.showRewardedVideo(_placementName);

            if (_adShowFailed)
            {
                IronSource.Agent.loadRewardedVideo();
                return RewardedAdShowResult.Unavailable;
            }

            UniTask adClosedTask = UniTask.WaitUntil(() => _adClosed || _adShowFailed);
            await adClosedTask;

            UniTask adRewardedTask = UniTask.WaitUntil(() => _adRewarded);
            UniTask rewardTimeoutTask = UniTask.WaitForSeconds(AdRewardTimeoutSeconds);

            await UniTask.WhenAny(adRewardedTask, rewardTimeoutTask);

            return _adRewarded ? RewardedAdShowResult.Rewarded : RewardedAdShowResult.Closed;
        }

        private void OnAdShowFailed(IronSourceError error, IronSourceAdInfo info)
        {
            Debug.LogError($"Rewarded ad show failed: [{error.getErrorCode()}] {error.getDescription()}");

            _adShowFailed = true;
        }

        private void OnAdClosed(IronSourceAdInfo info)
        {
            _adClosed = true;
        }

        private void OnAdRewarded(IronSourcePlacement placement, IronSourceAdInfo info)
        {
            Debug.Log("OnAdRewarded");
            _adRewarded = true;

            _analytics.Send(new AdCompletedEvent(info.revenue));
        }
    }
}