using Analytics.Providers.Analytics;
using UnityEngine;

namespace Analytics.Events
{
    public class FirstLaunchEvent : LaunchEvent
    {
        private const string FirstLaunchKey = "launched";

        public override string EventName => "FirstLaunch";

        public void Send(AnalyticsProvider provider)
        {
            if (PlayerPrefs.HasKey(FirstLaunchKey))
                return;

            PlayerPrefs.SetInt(FirstLaunchKey, 1);
            PlayerPrefs.Save();
        }
    }
}