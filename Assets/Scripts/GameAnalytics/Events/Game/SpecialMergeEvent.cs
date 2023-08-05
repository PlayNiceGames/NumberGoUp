using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;
using GameBoard.Turns.Merge;

namespace GameAnalytics.Events.Game
{
    public class SpecialMergeEvent : AnalyticsEvent
    {
        public override string EventName => "special_merge";
        public override string EventCategory => "game";

        private readonly MergeType _type;
        private readonly int _score;
        private readonly int _tileValue;

        public SpecialMergeEvent(MergeType type, int score, int tileValue)
        {
            _type = type;
            _score = score;
            _tileValue = tileValue;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new StringEventParameter("type", _type.ToString());
            yield return new IntegerEventParameter("score", _score);
            yield return new IntegerEventParameter("tile_value", _tileValue);
        }
    }
}