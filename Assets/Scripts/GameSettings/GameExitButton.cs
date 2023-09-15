using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace GameSettings
{
    public class GameExitButton : MonoBehaviour
    {
        private UniTaskCompletionSource _waitForClick;

        public UniTask WaitForClick()
        {
            _waitForClick = new UniTaskCompletionSource();

            return _waitForClick.Task;
        }

        [UsedImplicitly]
        public void ClickExitButton()
        {
            _waitForClick?.TrySetResult();
        }
    }
}