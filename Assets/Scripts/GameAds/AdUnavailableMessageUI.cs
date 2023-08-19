using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace GameAds
{
    public class AdUnavailableMessageUI : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _fadeInDurationSeconds;
        [SerializeField] private float _delayDurationSeconds;
        [SerializeField] private float _fadeOutDurationSeconds;
        [SerializeField] private CanvasGroup _group;

        public void Dispose()
        {
            Destroy(gameObject);
        }

        public UniTask Show()
        {
            Tween fadeTween = _group.DOFade(1, _fadeInDurationSeconds);

            return fadeTween.PlayAsync();
        }

        public UniTask ShowDelay()
        {
            return UniTask.Delay(TimeSpan.FromSeconds(_delayDurationSeconds));
        }

        public UniTask Hide()
        {
            Tween fadeTween = _group.DOFade(0, _fadeOutDurationSeconds);

            return fadeTween.PlayAsync();
        }
    }
}