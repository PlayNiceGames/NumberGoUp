using UnityEngine;

namespace Tiles.Animations
{
    public class TileFadeAnimation : MonoBehaviour
    {
        [SerializeField] private float _fadeAmount;
        [SerializeField] private CanvasGroup _group;

        public void Fade(bool isFaded)
        {
            _group.alpha = isFaded ? _fadeAmount : 1;
        }
    }
}