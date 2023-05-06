using Tiles.Containers;
using TMPro;
using UnityEngine;

namespace GameScore
{
    public class ScoreSystem : MonoBehaviour
    {
        public int Score { get; private set; }

        [SerializeField] private ScoreData _data;
        [SerializeField] private TextMeshProUGUI _label;

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
            _label.text = Score.ToString();
        }
    }
}