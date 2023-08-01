using Analytics.Events;

namespace Analytics.Providers.Analytics
{
    public abstract class AnalyticsProvider : Provider
    {
        public abstract void Send(AnalyticsEvent analyticsEvent);

        public override void Disable()
        {
        }
    }
}