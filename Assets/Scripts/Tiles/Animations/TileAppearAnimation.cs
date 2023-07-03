using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Tiles.Animations
{
    public class TileAppearAnimation : MonoBehaviour
    {
        [SerializeField] private float _scaleDurationSeconds;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private RectTransform _target;
        [Space]
        [SerializeField] private float _tilesAppearDelaySeconds;

        public UniTask Play()
        {
            _target.localScale = Vector3.zero;

            var tween = _target.DOScale(Vector3.one, _scaleDurationSeconds);
            tween.SetEase(_scaleCurve);

            return tween.PlayAsync();
        }

        public UniTask WaitForTilesAppearDelay()
        {
            return UniTask.Delay(TimeSpan.FromSeconds(_tilesAppearDelaySeconds));
        }
    }
}