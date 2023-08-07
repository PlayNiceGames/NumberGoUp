using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;

namespace GameAnalytics.Events.Menu
{
    public class MuteMusicEvent : AnalyticsEvent
    {
        public override string EventName => "mute_music";
        public override string EventCategory => "menu";

        private readonly bool _isMuted;

        public MuteMusicEvent(bool isMuted)
        {
            _isMuted = isMuted;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new BooleanEventParameter("is_muted", _isMuted);
        }
    }
}