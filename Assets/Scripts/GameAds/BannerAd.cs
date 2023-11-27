using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameAds
{
    public class BannerAd : MonoBehaviour
    {
        private const float BannerLoadRetryTime = 5.0f;

        [SerializeField] private string _placementName;
        [SerializeField] private BannerAdSize _size;
        [SerializeField] private IronSourceBannerPosition _position;

        private void Start()
        {
            if (PlayerPrefs.GetInt("No_Ads", 0) == 0) InitializeAndShowBanner();
            IAPManager.OnPurchaseDone += HideBanner;
        }

        private void OnDestroy()
        {
            IronSourceBannerEvents.onAdLoadedEvent -= OnBannerLoaded;
            IronSourceBannerEvents.onAdLoadFailedEvent -= OnBannerLoadFailed;
            IAPManager.OnPurchaseDone -= HideBanner;
            IronSource.Agent.destroyBanner();
        }

        private void InitializeAndShowBanner()
        {
            LoadBanner();

            IronSourceBannerEvents.onAdLoadedEvent += OnBannerLoaded;
            IronSourceBannerEvents.onAdLoadFailedEvent += OnBannerLoadFailed;
        }

        private void OnBannerLoaded(IronSourceAdInfo info)
        {
            Debug.Log("Banner loaded");
        }

        private void OnBannerLoadFailed(IronSourceError error)
        {
            Debug.LogError($"Banner load failed: [{error.getErrorCode()}] {error.getDescription()}");

            RetryBannerLoading().Forget();
        }

        private void LoadBanner()
        {
            IronSourceBannerSize ironSourceBannerSize = GetBannerSize(_size);

            IronSource.Agent.loadBanner(ironSourceBannerSize, _position, _placementName);
        }

        private IronSourceBannerSize GetBannerSize(BannerAdSize size)
        {
            return size switch
            {
                BannerAdSize.Small => IronSourceBannerSize.BANNER,
                BannerAdSize.Large => IronSourceBannerSize.LARGE,
                _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
            };
        }

        private async UniTask RetryBannerLoading()
        {
            await UniTask.WaitForSeconds(BannerLoadRetryTime, cancellationToken: destroyCancellationToken);

            LoadBanner();
        }

        void HideBanner()
        {
            Debug.Log("Its here to hide banner");
            IronSource.Agent.hideBanner();
            IronSource.Agent.destroyBanner();
        }
    }
}