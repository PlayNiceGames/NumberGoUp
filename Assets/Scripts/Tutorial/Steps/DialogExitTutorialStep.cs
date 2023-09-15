using System;
using Cysharp.Threading.Tasks;
using GameSettings;
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
        private GameExitButton _exitButton;

        public DialogExitTutorialStep(DialogExitTutorialStepData data, TutorialDialogController dialogController, GameExitButton exitButton)
        {
            _data = data;
            _dialogController = dialogController;
            _exitButton = exitButton;
        }

        public override async UniTask<TutorialStepResult> Run()
        {
            UniTask dialogTask = _dialogController.ShowDialogExit(_data.DialogKey);
            UniTask backButtonClickTask = _exitButton.WaitForClick();

            await UniTask.WhenAny(dialogTask, backButtonClickTask);

            if (backButtonClickTask.IsCompleted())
                return TutorialStepResult.ExitToMenu;

            return TutorialStepResult.StartGame;
        }
    }
}