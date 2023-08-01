using System.Collections.Generic;
using Analytics.Parameters;

namespace Analytics.Events
{
    public abstract class AnalyticsEvent
    {
        public abstract string EventName { get; }
        public abstract string EventCategory { get; }

        public virtual IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield break;
        }

        public IEnumerable<AbstractEventParameter> GetBaseParameters()
        {
            yield return new StringEventParameter("name", EventName);
            yield return new StringEventParameter("category", EventCategory);
        }
    }
}