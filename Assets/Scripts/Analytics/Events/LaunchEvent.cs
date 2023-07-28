using System.Collections.Generic;
using Analytics.Parameters;
using UnityEngine;

namespace Analytics.Events
{
    public class LaunchEvent : AbstractEvent
    {
        public override string EventName => "launch";
        public override string EventCategory => AnalyticsService.DefaultCategoryName;

        private const string FirstLaunchKey = "launched";

        private bool _isFirstLaunch;

        public LaunchEvent()
        {
            _isFirstLaunch = CheckAndRecordFirstLaunch();
        }

        private bool CheckAndRecordFirstLaunch()
        {
            if (PlayerPrefs.HasKey(FirstLaunchKey))
                return false;

            PlayerPrefs.SetInt(FirstLaunchKey, 1);
            PlayerPrefs.Save();

            return true;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new BooleanEventParameter("is_first_launch", _isFirstLaunch);
        }
    }
}