using Cysharp.Threading.Tasks;
using GameAudio;
using JetBrains.Annotations;
using ServiceLocator;
using UnityEngine;

namespace GameSettings
{
    public class GameExitButton : MonoBehaviour
    {
        private UniTaskCompletionSource _waitForClick;

        private Audio _audio;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

        public UniTask WaitForClick()
        {
            _waitForClick = new UniTaskCompletionSource();

            return _waitForClick.Task;
        }

        [UsedImplicitly]
        public void ClickExitButton()
        {
            _waitForClick?.TrySetResult();

            _audio.PlayClick();
        }
    }
}