using TMPro;
using UnityEngine;

namespace GameScore
{
    public class ScoreSystem : MonoBehaviour
    {
        public int Score { get; private set; }

        [SerializeField] private ScoreData _settings;
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            UpdateText();
        }

        public void IncrementScoreForMerge(int startingValue, int deltaValue = 1)
        {
            //Score += scoreDelta;

            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = Score.ToString();
        }
    }
}