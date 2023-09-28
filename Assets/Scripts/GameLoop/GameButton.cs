using Cysharp.Threading.Tasks;
using GameAudio;
using JetBrains.Annotations;
using ServiceLocator;
using UnityEngine;

namespace GameLoop
{
    public class GameButton : MonoBehaviour
    {
        protected UniTaskCompletionSource _waitForClick;

        private Audio _audio;

        protected virtual void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

        public UniTask WaitForClick()
        {
            _waitForClick = new UniTaskCompletionSource();
            
            return _waitForClick.Task;
        }

        [UsedImplicitly]
        public void ClickButton()
        {
            _waitForClick?.TrySetResult();

            _audio.PlayClick();
        }
    }
}