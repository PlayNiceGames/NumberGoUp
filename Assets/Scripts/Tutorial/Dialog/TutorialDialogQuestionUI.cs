using Cysharp.Threading.Tasks;
using GameAudio;
using ServiceLocator;
using SimpleTextProvider;
using UnityEngine;

namespace Tutorial.Dialog
{
    public class TutorialDialogQuestionUI : MonoBehaviour
    {
        [SerializeField] private TextProviderTMP _dialogLabel;

        private Audio _audio;

        private UniTaskCompletionSource<TutorialQuestionAction> _buttonClicked;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
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
            _audio.PlayClick();

            _buttonClicked?.TrySetResult(TutorialQuestionAction.Play);
        }

        public void ClickContinueTutorialButton()
        {
            _audio.PlayClick();

            _buttonClicked?.TrySetResult(TutorialQuestionAction.ContinueTutorial);
        }
    }
}