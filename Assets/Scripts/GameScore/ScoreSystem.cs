using Serialization;
using Tiles.Containers;
using TMPro;
using UnityEngine;

namespace GameScore
{
    public class ScoreSystem : MonoBehaviour, IPlayerPrefsSerializable
    {
        private const string HighScoreKey = "high_score"; //TODO temp, move to save/load manager

        public int Score { get; private set; }
        public int HighScore { get; private set; }

        [SerializeField] private ScoreData _data;
        [SerializeField] private TextMeshProUGUI _label;

        private void Start()
        {
            Deserialize();

            UpdateText();
        }

        public void IncrementScoreForMerge(int startingValue, int deltaValue, MergeContainer container)
        {
            int scoreDelta = _data.GetScoreForMerge(startingValue, deltaValue, container);

            Score += scoreDelta;

            UpdateText();
            UpdateHighScore();
        }

        private void UpdateText()
        {
            _label.text = Score.ToString();
        }

        private void UpdateHighScore()
        {
            if (Score > HighScore)
            {
                HighScore = Score;

                Serialize();  //TODO temp, move to save/load manager
            }
        }

        public void Serialize()
        {
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
        }

        public void Deserialize()
        {
            HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        }
    }
}