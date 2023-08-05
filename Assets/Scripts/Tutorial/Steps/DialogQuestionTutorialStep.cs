using System;
using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Tutorial;
using ServiceLocator;
using Tutorial.Dialog;
using Tutorial.Steps.Data;

namespace Tutorial.Steps
{
    [Serializable]
    public class DialogQuestionTutorialStep : TutorialStep
    {
        private DialogQuestionTutorialStepData _data;

        private TutorialDialogController _dialogController;

        private AnalyticsService _analytics;

        public DialogQuestionTutorialStep(DialogQuestionTutorialStepData data, TutorialDialogController dialogController)
        {
            _data = data;
            _dialogController = dialogController;

            _analytics = GlobalServices.Get<AnalyticsService>();
        }

        public override async UniTask<bool> Run()
        {
            TutorialQuestionAction action = await _dialogController.ShowDialogQuestion(_data.DialogKey);

            _analytics.Send(new TutorialQuestionEvent(action));

            return action switch
            {
                TutorialQuestionAction.Play => true,
                TutorialQuestionAction.ContinueTutorial => false,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}