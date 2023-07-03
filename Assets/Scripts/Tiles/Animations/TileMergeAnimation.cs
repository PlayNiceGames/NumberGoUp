using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Tiles.Animations
{
    public class TileMergeAnimation : MonoBehaviour
    {
        [SerializeField] private float _durationSeconds;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private RectTransform _target;

        public UniTask Play(Vector2 position)
        {
            UniTask scaleTask = ScaleDown();
            UniTask moveTask = MoveToPosition(position);

            return UniTask.WhenAll(scaleTask, moveTask);
        }

        private UniTask ScaleDown()
        {
            _target.localScale = Vector3.one;

            var tween = _target.DOScale(Vector3.zero, _durationSeconds);
            tween.SetEase(_scaleCurve);

            return tween.PlayAsync();
        }

        private UniTask MoveToPosition(Vector2 position)
        {
            var tween = _target.DOMove(position, _durationSeconds);
            tween.SetEase(_moveCurve);

            return tween.PlayAsync();
        }
    }
}