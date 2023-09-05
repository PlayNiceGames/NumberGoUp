using UnityEngine;
using UnityEngine.UI;

namespace GameOver
{
    public class GameOverContinueButton : MonoBehaviour
    {
        [SerializeField] private float _disabledAlpha;
        [SerializeField] private Button _continueButton;
        [SerializeField] private CanvasGroup _group;

        public void SetInteractable(bool isInteractable)
        {
            _continueButton.interactable = isInteractable;
            _group.alpha = isInteractable ? 1 : _disabledAlpha;
        }
    }
}