using TMPro;
using UnityEngine;

namespace GameScore
{
    public class ScoreSystem : MonoBehaviour
    {
        public int Score { get; private set; }

        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            UpdateText();
        }

        public void IncrementScore(int scoreDelta)
        {
            Score += scoreDelta;

            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = Score.ToString();
        }
    }
}