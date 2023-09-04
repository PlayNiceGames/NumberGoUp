using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace MainMenu
{
    public class MainMenuBackground : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private RectTransform _bottomBackgroundTransform;

        public UniTask PlayTransition()
        {
            Tween moveBackgroundTween = _bottomBackgroundTransform.DOSizeDelta(new Vector2(_bottomBackgroundTransform.sizeDelta.x, 0), _duration);
            moveBackgroundTween.SetEase(_curve);
            return moveBackgroundTween.PlayAsync();
        }
    }
}