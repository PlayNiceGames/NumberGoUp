#if FIREBASE_MESSAGING
using Firebase.Messaging;	
using UnityEngine;

namespace Analytics.Providers.PushNotifications
{
	public class PushNotificationsProviderFirebase : PushNotificationsProvider
	{
		public override bool IsEnabledOnCurrentPlatform()
		{
			return !Application.isEditor && Application.isMobilePlatform;
		}

		public override void Init()
		{
			FirebaseMessaging.TokenRegistrationOnInitEnabled = true;

			FirebaseMessaging.TokenReceived += OnTokenReceived;
			FirebaseMessaging.MessageReceived += OnMessageReceived;

			IsReady = true;
		}

		public override void Disable()
		{
			if (IsReady)
				FirebaseMessaging.TokenRegistrationOnInitEnabled = false;
		}

		private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
		{
			Debug.Log($"Message received: {e.Message.Notification.Badge}");
		}

		private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
		{
			Debug.Log($"Token received: {e.Token}");
		}
	}
}
#endif