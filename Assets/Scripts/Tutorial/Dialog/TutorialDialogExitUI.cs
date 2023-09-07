using Cysharp.Threading.Tasks;
using GameAudio;
using JetBrains.Annotations;
using ServiceLocator;
using SimpleTextProvider;
using UnityEngine;

namespace Tutorial.Dialog
{
    public class TutorialDialogExitUI : MonoBehaviour
    {
        [SerializeField] private TextProviderTMP _dialogLabel;

        private Audio _audio;

        private UniTaskCompletionSource _buttonClicked;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
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

        [UsedImplicitly]
        public void ClickExitButton()
        {
            _audio.PlayClick();

            _buttonClicked?.TrySetResult();
        }
    }
}