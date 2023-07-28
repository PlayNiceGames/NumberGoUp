using Analytics.Events;

namespace Analytics.Providers.Analytics
{
    public abstract class AnalyticsProvider : Provider
    {
        public abstract void Send(AbstractEvent analyticsEvent);

        public override void Disable()
        {
        }
    }
}