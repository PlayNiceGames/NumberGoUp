using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tutorial.Dialog
{
    public class TutorialDialogController : MonoBehaviour
    {
        [SerializeField] private TutorialDialogUI _dialogUI;
        [SerializeField] private TutorialDialogQuestionUI _dialogQuestionUI;
        [SerializeField] private TutorialDialogExitUI _dialogExitUI;

        private void Awake()
        {
            HideAll();
        }

        public void ShowDialog(string titleKey, string dialogKey)
        {
            HideAll();

            _dialogUI.SetTextKeys(titleKey, dialogKey);
            _dialogUI.Show();
        }

        public UniTask<TutorialQuestionAction> ShowDialogQuestion(string dialogKey)
        {
            HideAll();

            _dialogQuestionUI.SetTextKeys(dialogKey);
            return _dialogQuestionUI.ShowWithResult();
        }

        public UniTask ShowDialogExit(string dialogKey)
        {
            HideAll();

            _dialogExitUI.SetTextKeys(dialogKey);
            return _dialogExitUI.ShowWithResult();
        }

        public void HideAll()
        {
            _dialogUI.Hide();
            _dialogQuestionUI.Hide();
            _dialogExitUI.Hide();
        }
    }
}