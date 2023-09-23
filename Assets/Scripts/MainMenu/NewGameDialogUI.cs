using System;
using Cysharp.Threading.Tasks;
using GameAudio;
using ServiceLocator;
using UnityEngine;

namespace MainMenu
{
    public class NewGameDialogUI : MonoBehaviour
    {
        private UniTaskCompletionSource<NewGameDialogResult> _clickResult;

        private Audio _audio;

        private void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

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
            
            _audio.PlayClick();
        }

        public void ClickContinue()
        {
            _clickResult.TrySetResult(NewGameDialogResult.Continue);
            
            _audio.PlayClick();
        }
    }
}