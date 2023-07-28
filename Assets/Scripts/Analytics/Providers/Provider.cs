using Analytics.Events;
using UnityEngine;

namespace Analytics.Providers
{
    public abstract class Provider : MonoBehaviour
    {
        public abstract void Init();
        public abstract bool IsEnabledOnCurrentPlatform();
        public abstract void Disable();
    }
}