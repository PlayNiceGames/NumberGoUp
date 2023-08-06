using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;

namespace GameAnalytics.Events.Game
{
    public class GameOverContinueEvent : AnalyticsEvent
    {
        public override string EventName => "game_over_continue";
        public override string EventCategory => "game";

        private int _score;
        private int _highScore;
        private int _highestTileValue;

        public GameOverContinueEvent(int score, int highScore, int highestTileValue)
        {
            _score = score;
            _highScore = highScore;
            _highestTileValue = highestTileValue;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new IntegerEventParameter("score", _score);
            yield return new IntegerEventParameter("high_score", _highScore);
            yield return new IntegerEventParameter("highest_tile_value", _highestTileValue);
        }
    }
}