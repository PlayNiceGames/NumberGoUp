using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;
using UnityEngine;

namespace GameAnalytics.Events.Game
{
    public class ScoreReachedEvent : AnalyticsEvent
    {
        public override string EventName => "score_reached";
        public override string EventCategory => "game";

        private readonly int _scoreRange;
        private readonly int _score;

        public ScoreReachedEvent(int scoreRange, int score)
        {
            _scoreRange = scoreRange;
            _score = score;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new IntegerEventParameter("score_range", _scoreRange);
            yield return new IntegerEventParameter("score", _score);
        }
    }
}