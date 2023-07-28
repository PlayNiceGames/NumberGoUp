namespace Analytics.Events
{
    public class LaunchEvent : AbstractEvent
    {
        public override string EventName => "launch";
        public override string EventCategory => AnalyticsSender.DefaultCategoryName;
    }
}