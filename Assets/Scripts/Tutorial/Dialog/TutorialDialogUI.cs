using SimpleTextProvider;
using UnityEngine;

namespace Tutorial.Dialog
{
    public class TutorialDialogUI : MonoBehaviour
    {
        [SerializeField] private TextProviderTMP _titleLabel;
        [SerializeField] private TextProviderTMP _dialogLabel;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void SetTextKeys(string titleKey, string dialogKey)
        {
            _titleLabel.Key = titleKey;
            _titleLabel.FillText();

            _dialogLabel.Key = dialogKey;
            _dialogLabel.FillText();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}