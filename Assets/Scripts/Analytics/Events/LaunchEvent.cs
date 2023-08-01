using System.Collections.Generic;
using Analytics.Parameters;

namespace Analytics.Events
{
    public class LaunchEvent : AnalyticsEvent
    {
        public override string EventName => "launch";
        public override string EventCategory => AnalyticsService.DefaultCategoryName;

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new CounterParameter("launch_counter");
        }
    }
}