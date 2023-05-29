﻿using Cysharp.Threading.Tasks;
using SimpleTextProvider;
using UnityEngine;

namespace Tutorial.Dialog
{
    public class TutorialDialogQuestionUI : MonoBehaviour
    {
        [SerializeField] private TextProviderTMP _dialogLabel;

        private UniTaskCompletionSource<TutorialQuestionAction> _buttonClicked;

        public void Setup()
        {
            _dialogLabel.Setup();

            Hide();
        }

        public UniTask<TutorialQuestionAction> ShowWithResult()
        {
            gameObject.SetActive(true);

            _buttonClicked = new UniTaskCompletionSource<TutorialQuestionAction>();

            return _buttonClicked.Task;
        }

        public void SetTextKeys(string dialogKey)
        {
            _dialogLabel.Key = dialogKey;
            _dialogLabel.FillText();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ClickPlayButton()
        {
            _buttonClicked?.TrySetResult(TutorialQuestionAction.Play);
        }

        public void ClickContinueTutorialButton()
        {
            _buttonClicked?.TrySetResult(TutorialQuestionAction.ContinueTutorial);
        }
    }
}