using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Tiles.Animations
{
    public class TileAppearAnimation : MonoBehaviour
    {
        [SerializeField] private float _scaleDurationSeconds;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private RectTransform _target;

        public async UniTask Play()
        {
            _target.localScale = Vector3.zero;

            var tween = _target.DOScale(Vector3.one, _scaleDurationSeconds);
            tween.SetEase(_scaleCurve);
            tween.Play();

            await _target.DOScale(Vector3.one, _scaleDurationSeconds).AsyncWaitForCompletion();
        }
    }
}