#if FIREBASE
using Analytics.Events;
using Firebase.Analytics;
using UnityEngine;

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

        public override void Send(AbstractEvent analyticsEvent)
        {
            SendFirebaseEvent(analyticsEvent.EventName, new[]
            {
                new Parameter(CategoryParameterName, analyticsEvent.EventCategory)
            });
        }

        private void SendFirebaseEvent(string name, Parameter[] parameters = null)
        {
            FirebaseAnalytics.LogEvent(name, parameters);
        }
    }
}
#endif