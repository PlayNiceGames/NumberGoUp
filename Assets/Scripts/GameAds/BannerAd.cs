using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameAds
{
    public class BannerAd : MonoBehaviour
    {
        [SerializeField] private string _placementName;
        [SerializeField] private BannerAdSize _size;
        [SerializeField] private IronSourceBannerPosition _position;

        private void Start()
        {
            InitializeAndShowBanner();
        }

        private void OnDestroy()
        {
            IronSourceBannerEvents.onAdLoadedEvent -= OnBannerLoaded;
            IronSourceBannerEvents.onAdLoadFailedEvent -= OnBannerLoadFailed;

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
            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: destroyCancellationToken);

            LoadBanner();
        }
    }
}