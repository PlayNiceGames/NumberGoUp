using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Game;
using Serialization;
using ServiceLocator;
using Tiles.Containers;
using UnityEngine;

namespace GameScore
{
    public class ScoreSystem : MonoBehaviour, IPlayerPrefsSerializable
    {
        private const string HighScoreKey = "high_score"; //TODO temp, move to save/load manager

        public int Score { get; private set; }
        public int HighScore { get; private set; }

        [SerializeField] private ScoreData _data;
        [SerializeField] private ScoreSystemUI _ui;

        private AnalyticsService _analytics;

        private int _prevScoreReachedEventScore;

        private void Awake()
        {
            _analytics = GlobalServices.Get<AnalyticsService>();
        }

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

            TrySendScoreReachedEvent();

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

        private void TrySendScoreReachedEvent()
        {
            int rangeScore = 0;
            int? range = _data.GetScoreReachedEventScoreRange(Score, out rangeScore);

            if (range == null)
                return;

            int? prevRange = _data.GetScoreReachedEventScoreRange(_prevScoreReachedEventScore, out _);

            if (prevRange == null || range > prevRange)
            {
                _prevScoreReachedEventScore = Score;

                _analytics.Send(new ScoreReachedEvent(rangeScore, Score));
            }
        }
    }
}