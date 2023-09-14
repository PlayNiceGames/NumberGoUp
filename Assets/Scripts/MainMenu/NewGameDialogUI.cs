using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MainMenu
{
    public class NewGameDialogUI : MonoBehaviour
    {
        private UniTaskCompletionSource<NewGameDialogResult> _clickResult;

        public async UniTask<NewGameDialogResult> Show()
        {
            gameObject.SetActive(true);

            _clickResult = new UniTaskCompletionSource<NewGameDialogResult>();

            NewGameDialogResult result = await _clickResult.Task;

            Hide();

            return result;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ClickNewGame()
        {
            _clickResult.TrySetResult(NewGameDialogResult.NewGame);
        }

        public void ClickContinue()
        {
            _clickResult.TrySetResult(NewGameDialogResult.Continue);
        }
    }
}