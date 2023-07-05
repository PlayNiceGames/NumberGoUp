using Cysharp.Threading.Tasks;
using Serialization;
using Tiles.Containers;
using UnityEngine;

namespace GameScore
{
    public class ScoreSystem : MonoBehaviour, IPlayerPrefsSerializable
    {
        private const string HighScoreKey = "high_score"; //TODO temp, move to save/load manager

        public int Score { get; private set; }
        public int HighScore { get; private set; }

        [SerializeField] private ScoreSystemUI _ui;

        [SerializeField] private ScoreData _data;

        private void Start()
        {
            Deserialize();

            _ui.SetScore(Score);
        }

        public int GetScoreForMerge(MergeContainer container, int deltaValue = 1)
        {
            int startingValue = container.GetValue();
            int scoreDelta = _data.GetScoreForMerge(startingValue, deltaValue, container);
            return scoreDelta;
        }

        public UniTask IncrementScore(int scoreDelta)
        {
            int prevScore = Score;

            Score += scoreDelta;

            UpdateHighScore();

            return _ui.UpdateScoreWithAnimation(Score, prevScore);
        }

        private void UpdateHighScore()
        {
            if (Score > HighScore)
            {
                HighScore = Score;

                Serialize(); //TODO temp, move to save/load manager
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