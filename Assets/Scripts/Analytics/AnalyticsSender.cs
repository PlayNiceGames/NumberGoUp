using System.Collections.Generic;
using Analytics.Events;
using Analytics.Providers;
using Analytics.Providers.Analytics;
using Analytics.Providers.PushNotifications;
using UnityEngine;

namespace Analytics
{
    public class AnalyticsSender : MonoBehaviour
    {
        public bool IsAnalyticsEnabled;
        public bool IsPushNotificationsEnabled;

        public List<AnalyticsProvider> AnalyticsProviders;
        public List<PushNotificationsProvider> PushNotificationsProviders;

        public const string DefaultCategoryName = "general";

        private void Awake()
        {
            Init();

            Send(new LaunchEvent());
            Send(new FirstLaunchEvent());
        }

        private void Init()
        {
            List<Provider> providersToRemove = new List<Provider>();

            foreach (AnalyticsProvider provider in AnalyticsProviders)
            {
                if (provider == null)
                    continue;

                if (IsAnalyticsEnabled && provider.IsEnabledOnCurrentPlatform())
                    provider.Init();
                else
                    providersToRemove.Add(provider);
            }

            foreach (PushNotificationsProvider provider in PushNotificationsProviders)
            {
                if (provider == null)
                    continue;

                if (IsPushNotificationsEnabled && provider.IsEnabledOnCurrentPlatform())
                    provider.Init();
                else
                    providersToRemove.Add(provider);
            }

            foreach (Provider provider in providersToRemove)
            {
                RemoveProvider(provider);
            }
        }

        private void RemoveProvider(Provider provider)
        {
            provider.Disable();

            if (provider is AnalyticsProvider analyticsProvider)
                AnalyticsProviders.Remove(analyticsProvider);
            else if (provider is PushNotificationsProvider pushProvider)
                PushNotificationsProviders.Remove(pushProvider);

            Destroy(provider.gameObject);
        }

        public void Send(AbstractEvent analyticsEvent)
        {
            if (!IsAnalyticsEnabled)
                return;

            foreach (AnalyticsProvider provider in AnalyticsProviders)
            {
                if (!provider)
                    continue;

                if (provider.IsEnabledOnCurrentPlatform())
                    provider.Send(analyticsEvent);
            }
        }
    }
}