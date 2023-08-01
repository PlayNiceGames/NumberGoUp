using Analytics.Events;

namespace GameAnalytics.Events
{
    public class GameOverEvent : AnalyticsEvent
    {
        public override string EventName => "game_over";
        public override string EventCategory => "game";

        private int _score;
        private int _highestTileValue;
        private bool _canContinue;
    }
}