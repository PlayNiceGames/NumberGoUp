using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameLoop;
using Tutorial.Dialog;
using Tutorial.Steps.Data;
using Utils;

namespace Tutorial.Steps
{
    [Serializable]
    public class DialogExitTutorialStep : TutorialStep
    {
        private DialogExitTutorialStepData _data;

        private TutorialDialogController _dialogController;
        private GameButton _exitButton;

        public DialogExitTutorialStep(DialogExitTutorialStepData data, TutorialDialogController dialogController, GameButton exitButton)
        {
            _data = data;
            _dialogController = dialogController;
            _exitButton = exitButton;
        }

        public override async UniTask<TutorialStepResult> Run()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            
            UniTask dialogTask = _dialogController.ShowDialogExit(_data.DialogKey);
            UniTask backButtonClickTask = _exitButton.WaitForClick(cancellationToken);

            await UniTask.WhenAny(dialogTask, backButtonClickTask);
            
            cancellationTokenSource.Cancel();

            if (backButtonClickTask.IsCompleted())
                return TutorialStepResult.ExitToMenu;

            return TutorialStepResult.StartGame;
        }
    }
}