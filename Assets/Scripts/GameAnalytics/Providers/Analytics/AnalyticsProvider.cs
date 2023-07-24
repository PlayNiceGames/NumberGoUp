namespace Analytics.Providers.Analytics
{
	public abstract class AnalyticsProvider : Provider
	{
		public abstract void Send(string name, string category, string label = null, int? value = null);

		public override void Disable()
		{

		}
	}
}