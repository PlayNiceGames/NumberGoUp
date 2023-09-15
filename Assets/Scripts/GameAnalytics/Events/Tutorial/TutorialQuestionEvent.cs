using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;
using Tutorial.Dialog;
using Tutorial.Steps;

namespace GameAnalytics.Events.Tutorial
{
    public class TutorialQuestionEvent : AnalyticsEvent
    {
        public override string EventName => "tutorial_question";
        public override string EventCategory => "tutorial";

        private readonly TutorialStepResult _action;

        public TutorialQuestionEvent(TutorialStepResult action)
        {
            _action = action;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new StringEventParameter("action", _action.ToString());
        }
    }
}