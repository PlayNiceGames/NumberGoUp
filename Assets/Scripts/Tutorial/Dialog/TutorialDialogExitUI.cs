using Cysharp.Threading.Tasks;
using SimpleTextProvider;
using UnityEngine;

namespace Tutorial.Dialog
{
    public class TutorialDialogExitUI : MonoBehaviour
    {
        [SerializeField] private TextProviderTMP _dialogLabel;

        private UniTaskCompletionSource _buttonClicked;

        public void Setup()
        {
            _dialogLabel.Setup();

            Hide();
        }

        public UniTask ShowWithResult()
        {
            gameObject.SetActive(true);

            _buttonClicked = new UniTaskCompletionSource();

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

        public void ClickExitButton()
        {
            _buttonClicked?.TrySetResult();
        }
    }
}