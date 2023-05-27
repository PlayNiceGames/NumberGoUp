﻿using System;
using Cysharp.Threading.Tasks;
using Tutorial.Dialog;
using Tutorial.Steps.Data;

namespace Tutorial.Steps
{
    [Serializable]
    public class DialogTutorialStep : TutorialStep
    {
        private DialogTutorialStepData _data;

        private TutorialDialogController _dialogController;

        public DialogTutorialStep(DialogTutorialStepData data, TutorialDialogController dialogController)
        {
            _data = data;
            _dialogController = dialogController;
        }

        public override UniTask Run()
        {
            _dialogController.ShowDialog(_data.TitleKey, _data.DialogKey);

            return UniTask.CompletedTask;
        }
    }
}