using SimpleTextProvider;
using UnityEngine;
using UnityEngine.UI;

namespace GameOver
{
    public class GameOverContinueButton : MonoBehaviour
    {
        [SerializeField] private float _disabledAlpha;
        [SerializeField] private Button _continueButton;
        [SerializeField] private TextProviderTMP _text;
        [SerializeField] private CanvasGroup _group;

        public void SetData(int continueCount, int maxContinueCount)
        {
            int remainingContinues = maxContinueCount - continueCount;
            _text.FillText(remainingContinues, maxContinueCount);

            bool canContinue = continueCount < maxContinueCount;

            SetInteractable(canContinue);
        }

        private void SetInteractable(bool isInteractable)
        {
            _continueButton.interactable = isInteractable;
            _group.alpha = isInteractable ? 1 : _disabledAlpha;
        }
    }
}