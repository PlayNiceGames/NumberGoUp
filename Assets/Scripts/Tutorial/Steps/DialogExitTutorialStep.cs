using System;
using Cysharp.Threading.Tasks;
using Tutorial.Dialog;
using Tutorial.Steps.Data;

namespace Tutorial.Steps
{
    [Serializable]
    public class DialogExitTutorialStep : TutorialStep
    {
        private DialogExitTutorialStepData _data;

        private TutorialDialogController _dialogController;

        public DialogExitTutorialStep(DialogExitTutorialStepData data, TutorialDialogController dialogController)
        {
            _data = data;
            _dialogController = dialogController;
        }

        public override async UniTask<bool> Run()
        {
            await _dialogController.ShowDialogExit(_data.DialogKey);

            return true;
        }
    }
}