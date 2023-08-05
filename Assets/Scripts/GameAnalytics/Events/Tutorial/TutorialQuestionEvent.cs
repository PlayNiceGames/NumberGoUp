using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;
using Tutorial.Dialog;

namespace GameAnalytics.Events.Tutorial
{
    public class TutorialQuestionEvent : AnalyticsEvent
    {
        public override string EventName => "tutorial_question";
        public override string EventCategory => "tutorial";

        private readonly TutorialQuestionAction _action;

        public TutorialQuestionEvent(TutorialQuestionAction action)
        {
            _action = action;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new StringEventParameter("action", _action.ToString());
        }
    }
}