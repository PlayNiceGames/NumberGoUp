using Analytics;
using Cysharp.Threading.Tasks;
using GameAnalytics.Events.Game;
using Serialization;
using ServiceLocator;
using Tiles.Containers;
using UnityEngine;

namespace GameScore
{
    public class ScoreSystem : MonoBehaviour, IPlayerPrefsSerializable, IDataSerializable<int>
    {
        private const string HighScoreKey = "high_score"; //TODO temp, move to save/load manager

        [SerializeField] private ScoreSystemUI _ui;

        [SerializeField] private ScoreDatabase _data;

        private AnalyticsService _analytics;

        private bool _isEnabled = true;
        private int _prevScoreReachedEventScore;

        public int Score { get; private set; }
        public int HighScore { get; private set; }

        private void Awake()
        {
            _analytics = GlobalServices.Get<AnalyticsService>();
        }

        private void Start()
        {
            Deserialize();

            _ui.SetScore(Score);
        }

        public void Serialize()
        {
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
        }

        public void Deserialize()
        {
            HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        public void SetEnabled(bool isEnabled)
        {
            _ui.gameObject.SetActive(isEnabled);

            _isEnabled = isEnabled;
        }

        public int GetScoreForMerge(MergeContainer container, int deltaValue = 1)
        {
            int startingValue = container.GetValue();
            int scoreDelta = _data.GetScoreForMerge(startingValue, deltaValue, container);
            return scoreDelta;
        }

        public UniTask IncrementScore(int scoreDelta)
        {
            if (!_isEnabled)
                return UniTask.CompletedTask;

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

        private void TrySendScoreReachedEvent()
        {
            int? range = _data.GetScoreReachedEventScoreRange(Score, out int rangeScore);

            if (range == null)
                return;

            int? prevRange = _data.GetScoreReachedEventScoreRange(_prevScoreReachedEventScore, out _);

            if (prevRange == null || range > prevRange)
            {
                _prevScoreReachedEventScore = Score;

                _analytics.Send(new ScoreReachedEvent(rangeScore, Score));
            }
        }

        public int GetData()
        {
            return Score;
        }

        public void SetData(int score)
        {
            Score = score;

            _ui.SetScore(Score);
        }
    }
}