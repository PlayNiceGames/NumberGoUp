using Tiles.Containers;
using TMPro;
using UnityEngine;

namespace GameScore
{
    public class ScoreSystem : MonoBehaviour
    {
        public int Score { get; private set; }

        [SerializeField] private ScoreData _data;
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            UpdateText();
        }

        public void IncrementScoreForMerge(int startingValue, int deltaValue, IValueTileContainer container)
        {
            int scoreDelta = _data.GetScoreForMerge(startingValue, deltaValue, container);

            Score += scoreDelta;

            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = Score.ToString();
        }
    }
}