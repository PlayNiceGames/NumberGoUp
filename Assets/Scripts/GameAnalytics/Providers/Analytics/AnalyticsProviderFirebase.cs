#if FIREBASE
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

namespace Analytics.Providers.Analytics
{
	public class AnalyticsProviderFirebase : AnalyticsProvider
	{
		public override bool IsEnabledOnCurrentPlatform()
		{
			return !Application.isEditor && Application.isMobilePlatform;
		}

		public override void Init()
		{
			FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
		}

		public override void Send(string name, string category, string label, int? value)
		{
			FirebaseAnalytics.LogEvent(name, GetSimpleEventParameters(category, label, value));
		}

		public void SendFirebaseEvent(string name, Parameter[] parameters = null)
		{
			FirebaseAnalytics.LogEvent(name, parameters);
		}

		private Parameter[] GetSimpleEventParameters(string category, string label, int? value)
		{
			List<Parameter> data = new List<Parameter>();

			if (!string.IsNullOrWhiteSpace(category))
				data.Add(new Parameter("Category", category));

			if (!string.IsNullOrWhiteSpace(label))
				data.Add(new Parameter("Label", label));

			if (value != null)
				data.Add(new Parameter("Value", value.Value));

			return data.ToArray();
		}
	}
}
#endif