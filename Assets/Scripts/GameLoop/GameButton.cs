﻿using System.Threading;
using Cysharp.Threading.Tasks;
using GameAudio;
using JetBrains.Annotations;
using ServiceLocator;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace GameLoop
{
    public class GameButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        protected UniTaskCompletionSource _waitForClick;

        private Audio _audio;

        protected virtual void Awake()
        {
            _audio = GlobalServices.Get<Audio>();
        }

        public UniTask WaitForClick(CancellationToken cancellationToken)
        {
            _waitForClick = new UniTaskCompletionSource();
            _waitForClick.AttachCancellationToken(cancellationToken);

            return _waitForClick.Task;
        }

        public void SetEnabled(bool isEnabled)
        {
            _button.interactable = isEnabled;
        }

        [UsedImplicitly]
        public void ClickButton()
        {
            _waitForClick?.TrySetResult();

            _audio.PlayClick();
        }
    }
}