using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;
using GameLoop;

namespace GameAnalytics.Events.Game
{
    public class GameStartEvent : AnalyticsEvent
    {
        public override string EventName => "game_start";
        public override string EventCategory => "game";

        private readonly GameLoopType _gameType;

        public GameStartEvent(GameLoopType gameType)
        {
            _gameType = gameType;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new StringEventParameter("game_type", _gameType.ToString());
            yield return new CounterParameter("game_start_counter");
        }
    }
}