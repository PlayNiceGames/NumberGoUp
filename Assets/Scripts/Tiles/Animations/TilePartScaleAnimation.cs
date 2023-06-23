using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Tiles.Animations
{
    public class TilePartScaleAnimation : MonoBehaviour
    {
        [SerializeField] private float _durationSeconds;
        [SerializeField] private AnimationCurve _scaleCurve;
        [Space]
        [SerializeField] private RectTransform _targetTransform;
        [SerializeField] private RectTransform _rootTransform;

        public async UniTask Play()
        {
            float maxHeight = _rootTransform.rect.height;
            Vector2 targetSize = new Vector2(_targetTransform.sizeDelta.x, maxHeight);

            Tween tween = _targetTransform.DOSizeDelta(targetSize, _durationSeconds);
            tween.SetEase(_scaleCurve);
            tween.Play();

            await tween.AsyncWaitForCompletion();
        }
    }
}