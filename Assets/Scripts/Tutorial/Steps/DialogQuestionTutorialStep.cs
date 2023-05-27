using System;
using Cysharp.Threading.Tasks;
using Tutorial.Dialog;
using Tutorial.Steps.Data;

namespace Tutorial.Steps
{
    [Serializable]
    public class DialogQuestionTutorialStep : TutorialStep
    {
        private DialogTutorialStepData _data;

        private TutorialDialogController _dialogController;

        public DialogQuestionTutorialStep(DialogTutorialStepData data, TutorialDialogController dialogController)
        {
            _data = data;
            _dialogController = dialogController;
        }

        public override async UniTask<bool> Run()
        {
            TutorialQuestionAction action = await _dialogController.ShowDialogQuestion(_data.TitleKey, _data.DialogKey);

            return action switch
            {
                TutorialQuestionAction.Play => true,
                TutorialQuestionAction.ContinueTutorial => false,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}