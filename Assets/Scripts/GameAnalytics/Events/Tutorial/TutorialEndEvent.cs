using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;

namespace GameAnalytics.Events.Tutorial
{
    public class TutorialEndEvent : AnalyticsEvent
    {
        public override string EventName => "tutorial_end";
        public override string EventCategory => "tutorial";

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new CounterParameter("tutorial_end_counter");
        }
    }
}