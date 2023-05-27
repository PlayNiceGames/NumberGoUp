using Cysharp.Threading.Tasks;

namespace Tutorial.Dialog
{
    public class TutorialDialogQuestionUI : TutorialDialogUI
    {
        private UniTaskCompletionSource<TutorialQuestionAction> _buttonClicked;

        public UniTask<TutorialQuestionAction> ShowWithResult()
        {
            Show();
            
            _buttonClicked = new UniTaskCompletionSource<TutorialQuestionAction>();
            
            return _buttonClicked.Task;
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