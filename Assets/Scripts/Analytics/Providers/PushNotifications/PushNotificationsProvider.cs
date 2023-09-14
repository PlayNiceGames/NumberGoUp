namespace Analytics.Providers.PushNotifications
{
    public abstract class PushNotificationsProvider : Provider
    {
        public bool IsReady { get; protected set; }
    }
}