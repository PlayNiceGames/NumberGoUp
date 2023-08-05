﻿using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;

namespace GameAnalytics.Events.Tutorial
{
    public class TutorialStartEvent : AnalyticsEvent
    {
        public override string EventName => "tutorial_start";
        public override string EventCategory => "tutorial";

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new CounterParameter("tutorial_start_counter");
        }
    }
}