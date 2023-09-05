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
        [SerializeField] private RectTransform _topBackgroundTransform;
        [SerializeField] private RectTransform _bottomBackgroundTransform;

        public UniTask PlayTransition()
        {
            UniTask topFade = PlayBackgroundFade(_topBackgroundTransform);
            UniTask bottomFade = PlayBackgroundFade(_bottomBackgroundTransform);

            return UniTask.WhenAll(topFade, bottomFade);
        }

        private UniTask PlayBackgroundFade(RectTransform background)
        {
            Tween moveBackgroundTween = background.DOSizeDelta(new Vector2(background.sizeDelta.x, 0), _duration);
            moveBackgroundTween.SetEase(_curve);
            return moveBackgroundTween.PlayAsync();
        }
    }
}