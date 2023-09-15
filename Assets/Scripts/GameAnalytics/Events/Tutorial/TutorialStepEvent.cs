using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;
using Tutorial.Steps;

namespace GameAnalytics.Events.Tutorial
{
    public class TutorialStepEvent : AnalyticsEvent
    {
        public override string EventName => "tutorial_step";
        public override string EventCategory => "tutorial";

        private readonly string _stepName;
        private readonly int _stepIndex;
        private readonly TutorialStepResult _result;

        public TutorialStepEvent(string stepName, int stepIndex, TutorialStepResult result)
        {
            _stepName = stepName;
            _stepIndex = stepIndex;
            _result = result;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new StringEventParameter("step_name", _stepName);
            yield return new IntegerEventParameter("step_index", _stepIndex);
            yield return new StringEventParameter("step_result", _result.ToString());
        }
    }
}