using System;
using Cysharp.Threading.Tasks;
using Tutorial.Data;

namespace Tutorial
{
    [Serializable]
    public class DialogTutorialStep : TutorialStep
    {
        private DialogTutorialStepData _data;

        private TutorialDialogUI _dialogUI;

        public DialogTutorialStep(DialogTutorialStepData data, TutorialDialogUI dialogUI)
        {
            _data = data;
            _dialogUI = dialogUI;
        }

        public override UniTask Run()
        {
            _dialogUI.SetTextKeys(_data.DialogKey);

            return UniTask.CompletedTask;
        }
    }
}