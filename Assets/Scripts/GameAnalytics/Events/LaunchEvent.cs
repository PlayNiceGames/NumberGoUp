using Analytics.Providers.Analytics;
using Firebase.Analytics;
#if FIREBASE
#endif

namespace Analytics.Events
{
    public class LaunchEvent : AbstractEvent
    {
        public override string EventName => "Launch";
        public override string EventCategory => AnalyticsSender.DefaultCategoryName;

        public const string SourceAppParameter = "source_app";

        public override void Send(AnalyticsProvider provider)
        {
            if (provider is AnalyticsProviderFirebase firebaseProvider)
            {
                firebaseProvider.SendFirebaseEvent(EventName, new[]
                {
                    new Parameter(CategoryParameterName, EventCategory)
                });
            }
        }
    }
}