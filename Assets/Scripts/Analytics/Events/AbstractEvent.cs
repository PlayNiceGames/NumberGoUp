namespace Analytics.Events
{
    public abstract class AbstractEvent
    {
        public abstract string EventName { get; }
        public abstract string EventCategory { get; }
    }
}