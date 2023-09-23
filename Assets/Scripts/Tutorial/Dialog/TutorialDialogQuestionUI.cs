using Cysharp.Threading.Tasks;
using GameAudio;
using ServiceLocator;
using SimpleTextProvider;
using Tutorial.Steps;
using UnityEngine;

namespace Tutorial.Dialog
{
    public class TutorialDialogQuestionUI : MonoBehaviour
    {
        [SerializeField] private TextProviderTMP _dialogLabel;

        private Audio _audio;

        private UniTaskCompletionSource<TutorialStepResult> _buttonClicked;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

        public UniTask<TutorialStepResult> ShowWithResult()
        {
            gameObject.SetActive(true);

            _buttonClicked = new UniTaskCompletionSource<TutorialStepResult>();

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
            _buttonClicked?.TrySetResult(TutorialStepResult.StartGame);
            
            _audio.PlayClick();
        }

        public void ClickContinueTutorialButton()
        {
            _buttonClicked?.TrySetResult(TutorialStepResult.Completed);
            
            _audio.PlayClick();
        }
    }
}