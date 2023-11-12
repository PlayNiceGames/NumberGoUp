using System;
using Cysharp.Threading.Tasks;
using GameBoard;
using UnityEngine;

namespace GameAds
{
    public class InterstitialAd : MonoBehaviour
    {
        [SerializeField] private string _placementName;

        private void Start()
        {
            InitializeLoadInterstitialAd();
        }

        private void OnDestroy()
        {
            IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialOnAdLoadFailed;
            IronSourceInterstitialEvents.onAdShowSucceededEvent -= InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent -= InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;
            //IronSource.Agent.destroyBanner();
        }

        private void InitializeLoadInterstitialAd()
        {
            IronSource.Agent.loadInterstitial();
            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
            //IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
            //IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        }

        private void InterstitialOnAdClosedEvent(IronSourceAdInfo obj)
        {
            Debug.Log("Interstitial Closed");
        }

        private void InterstitialOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo arg2)
        {
            Debug.LogError($"Interstitial ad show failed: [{error.getErrorCode()}] {error.getDescription()}");
        }

        private void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo obj)
        {
            IronSource.Agent.loadInterstitial();
        }

        private void InterstitialOnAdLoadFailed(IronSourceError error)
        {
            Debug.LogError($"Interstitial load failed: [{error.getErrorCode()}] {error.getDescription()}");
            IronSource.Agent.loadInterstitial();
        }

        private void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
        {
            Debug.Log("Interstitial loaded and ready");
        }

        public void ShowInterstitialAd()
        {
            Debug.Log("Interstitial ads shown");
            if (IronSource.Agent.isInterstitialReady() && !IronSource.Agent.isInterstitialPlacementCapped(_placementName))
            {
                Debug.Log("Interstitial ads shown");
                IronSource.Agent.showInterstitial(_placementName);
            }
        }
    }
}
