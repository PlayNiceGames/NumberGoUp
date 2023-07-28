namespace Analytics.Providers.Analytics
{
    public abstract class AnalyticsProvider : Provider
    {
        public const string CategoryParameterName = "category";

        public override void Disable()
        {
        }
    }
}