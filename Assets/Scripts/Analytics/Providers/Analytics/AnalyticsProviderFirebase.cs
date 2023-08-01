#if FIREBASE
using System;
using System.Collections.Generic;
using System.Linq;
using Analytics.Events;
using Analytics.Parameters;
using Firebase.Analytics;
using UnityEngine;

namespace Analytics.Providers.Analytics
{
    public class AnalyticsProviderFirebase : AnalyticsProvider
    {
        public override bool IsEnabledOnCurrentPlatform()
        {
            return !Application.isEditor && Application.isMobilePlatform;
        }

        public override void Init()
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        }

        public override void Send(AnalyticsEvent analyticsEvent)
        {
            Parameter[] firebaseParameters = GetFirebaseParameters(analyticsEvent);

            SendFirebaseEvent(analyticsEvent.EventName, firebaseParameters);
        }

        private Parameter[] GetFirebaseParameters(AnalyticsEvent analyticsEvent)
        {
            IEnumerable<AbstractEventParameter> baseParameters = analyticsEvent.GetBaseParameters();
            IEnumerable<AbstractEventParameter> parameters = analyticsEvent.GetParameters();
            IEnumerable<AbstractEventParameter> allParameters = baseParameters.Concat(parameters);

            IEnumerable<Parameter> firebaseParameters = allParameters.Select(GetFirebaseParameter);

            return firebaseParameters.ToArray();
        }

        private Parameter GetFirebaseParameter(AbstractEventParameter parameter)
        {
            switch (parameter)
            {
                case StringEventParameter stringParameter:
                    return new Parameter(stringParameter.Name, stringParameter.Value);
                case BooleanEventParameter boolParameter:
                    return new Parameter(boolParameter.Name, Convert.ToInt32(boolParameter.Value));
                case FloatEventParameter floatParameter:
                    return new Parameter(floatParameter.Name, floatParameter.Value);
                case IntegerEventParameter intParameter:
                    return new Parameter(intParameter.Name, intParameter.Value);
                case CounterParameter counterParameter:
                    return new Parameter(counterParameter.Name, counterParameter.Value);
            }

            throw new ArgumentException("Invalid analytics parameter type in Firebase analytics provider");
        }

        private void SendFirebaseEvent(string eventName, Parameter[] parameters = null)
        {
            FirebaseAnalytics.LogEvent(eventName, parameters);
        }
    }
}
#endif