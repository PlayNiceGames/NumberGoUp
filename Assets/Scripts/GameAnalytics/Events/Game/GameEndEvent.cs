using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;

namespace GameAnalytics.Events.Game
{
    public class GameEndEvent : AnalyticsEvent
    {
        public override string EventName => "game_end";
        public override string EventCategory => "game";

        private int _score;
        private int _highScore;
        private int _highestTileValue;
        private bool _canContinue;

        public GameEndEvent(int score, int highScore, int highestTileValue)
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
            yield return new BooleanEventParameter("can_continue", _canContinue);
            yield return new CounterParameter("game_end_counter");
        }
    }
}