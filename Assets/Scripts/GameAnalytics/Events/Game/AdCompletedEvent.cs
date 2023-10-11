using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;
using Firebase.Analytics;

namespace GameAnalytics.Events.Game
{
    public class AdCompletedEvent : AnalyticsEvent
    {
        public override string EventName => "ad_completed";
        public override string EventCategory => "game";

        private double? _revenue;

        public AdCompletedEvent(double? revenue)
        {
            _revenue = revenue;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            if (_revenue == null)
                yield break;

            yield return new FloatEventParameter(FirebaseAnalytics.ParameterValue, (float)_revenue.Value);
            yield return new StringEventParameter(FirebaseAnalytics.ParameterCurrency, "USD");
        }
    }
}