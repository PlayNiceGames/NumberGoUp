using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;

namespace GameAnalytics.Events.Game
{
    public class ScoreReachedEvent : AnalyticsEvent
    {
        public override string EventName => "score_reached";
        public override string EventCategory => "game";

        private readonly int _score;

        public ScoreReachedEvent(int score)
        {
            _score = score;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new IntegerEventParameter("score", _score);
        }
    }
}