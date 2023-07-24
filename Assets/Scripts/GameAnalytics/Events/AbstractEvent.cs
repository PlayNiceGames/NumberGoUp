using Analytics.Providers.Analytics;

namespace Analytics.Events
{
	public abstract class AbstractEvent
	{
		public abstract string EventName { get; }
		public abstract string EventCategory { get; }
		public abstract void Send(AnalyticsProvider provider);

		protected const string CategoryParameterName = "category";
	}
}