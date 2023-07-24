using System.Collections.Generic;
using System.Linq;
using Analytics.Events;
using Analytics.Providers;
using Analytics.Providers.Analytics;
using Analytics.Providers.PushNotifications;
using UnityEngine;

namespace Analytics
{
	public class AnalyticsSender : MonoBehaviour
	{
		public static AnalyticsSender Instance;

		public bool IsAnalyticsEnabled;
		public bool IsPushNotificationsEnabled;

		public List<AnalyticsProvider> AnalyticsProviders;
		public List<PushNotificationsProvider> PushNotificationsProviders;

		public const string DefaultCategoryName = "General";

		private bool _isInitialized = false;

		private void Awake()
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}

		private void Start()
		{
			Init();

			Send(new LaunchEvent());
			Send(new FirstLaunchEvent());
		}

		public void Init()
		{
			if (Instance == null) Instance = this;

			if (_isInitialized)
				return;

			List<Provider> providersToRemove = new List<Provider>();

			foreach (AnalyticsProvider provider in AnalyticsProviders)
			{
				if (!provider)
					continue;

				if (Instance.IsAnalyticsEnabled && provider.IsEnabledOnCurrentPlatform())
					provider.Init();
				else
					providersToRemove.Add(provider);
			}

			foreach (PushNotificationsProvider provider in PushNotificationsProviders)
			{
				if (!provider)
					continue;

				if (Instance.IsPushNotificationsEnabled && provider.IsEnabledOnCurrentPlatform())
					provider.Init();
				else
					providersToRemove.Add(provider);
			}

			foreach (Provider provider in providersToRemove)
			{
				RemoveProvider(provider);
			}

			_isInitialized = true;
		}

		private void RemoveProvider(Provider provider)
		{
			provider.Disable();

			if (provider is AnalyticsProvider analyticsProvider)
				AnalyticsProviders.Remove(analyticsProvider);
			if (provider is PushNotificationsProvider pushProvider)
				PushNotificationsProviders.Remove(pushProvider);

			Destroy(provider.gameObject);
		}

		public static void Send(string name, string category = DefaultCategoryName, string label = null, int? value = null)
		{
			if (!CheckInstance() || !Instance.IsAnalyticsEnabled)
				return;

			foreach (AnalyticsProvider provider in Instance.AnalyticsProviders)
			{
				if (!provider)
					continue;

				if (provider.IsEnabledOnCurrentPlatform())
					provider.Send(name, category, label, value);
			}
		}

		public static void Send(AbstractEvent analyticsEvent)
		{
			if (!CheckInstance() || !Instance.IsAnalyticsEnabled)
				return;

			foreach (AnalyticsProvider provider in Instance.AnalyticsProviders)
			{
				if(!provider)
					continue;

				if (provider.IsEnabledOnCurrentPlatform())
				{
					analyticsEvent.Send(provider);
				}
			}
		}

		private static bool CheckInstance()
		{
			if (Instance == null)
			{
				Debug.LogWarning("AnalyticsSender == null. Add analytics prefab to your starting scene and configure it");
				return false;
			}

			if (!Instance._isInitialized)
			{
				Instance.Init();
			}

			return Instance._isInitialized;
		}
	}
}