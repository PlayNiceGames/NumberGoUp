using UnityEngine;

namespace Analytics.Providers
{
	public abstract class Provider : MonoBehaviour
	{
		public bool IsReady { get; protected set; }
		public abstract bool IsEnabledOnCurrentPlatform();
		public abstract void Init();
		public abstract void Disable();
	}
}
