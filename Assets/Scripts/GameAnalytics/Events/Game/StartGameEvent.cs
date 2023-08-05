using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;
using GameLoop;

namespace GameAnalytics.Events.Game
{
    public class StartGameEvent : AnalyticsEvent
    {
        public override string EventName => "game_start";
        public override string EventCategory => "game";

        private readonly GameLoopType _gameType;

        public StartGameEvent(GameLoopType gameType)
        {
            _gameType = gameType;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            string gameName = _gameType.ToString();

            yield return new StringEventParameter("game_type", gameName);
            yield return new CounterParameter($"game_counter");
        }
    }
}