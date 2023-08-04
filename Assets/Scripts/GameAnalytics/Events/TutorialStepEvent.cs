using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;

namespace GameAnalytics.Events
{
    public class TutorialStepEvent : AnalyticsEvent
    {
        public override string EventName => "tutorial_step";
        public override string EventCategory => "tutorial";

        private readonly string _stepName;
        private readonly int _stepIndex;

        public TutorialStepEvent(string stepName, int stepIndex)
        {
            _stepName = stepName;
            _stepIndex = stepIndex;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new StringEventParameter("name", _stepName);
            yield return new IntegerEventParameter("index", _stepIndex);
        }
    }
}