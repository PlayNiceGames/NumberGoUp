using System;
using System.Threading;
using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Tutorial;
using GameLoop;
using ServiceLocator;
using Tutorial.Dialog;
using Tutorial.Steps.Data;
using Utils;

namespace Tutorial.Steps
{
    [Serializable]
    public class DialogQuestionTutorialStep : TutorialStep
    {
        private DialogQuestionTutorialStepData _data;

        private TutorialDialogController _dialogController;
        private GameButton _exitButton;

        private AnalyticsService _analytics;

        public DialogQuestionTutorialStep(DialogQuestionTutorialStepData data, TutorialDialogController dialogController, GameButton exitButton)
        {
            _data = data;
            _dialogController = dialogController;
            _exitButton = exitButton;

            _analytics = GlobalServices.Get<AnalyticsService>();
        }

        public override async UniTask<TutorialStepResult> Run()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            UniTask<TutorialStepResult> dialogTask = _dialogController.ShowDialogQuestion(_data.DialogKey);
            UniTask backButtonClickTask = _exitButton.WaitForClick(cancellationToken);

            await UniTask.WhenAny(dialogTask, backButtonClickTask);

            cancellationTokenSource.Cancel();

            TutorialStepResult result;

            if (backButtonClickTask.IsCompleted())
                result = TutorialStepResult.ExitToMenu;
            else
                result = await dialogTask;

            _analytics.Send(new TutorialQuestionEvent(result));

            return result;
        }
    }
}