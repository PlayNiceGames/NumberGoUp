#if FIREBASE_MESSAGING
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Messaging;
using UnityEngine;

namespace Analytics.Providers.PushNotifications
{
    public class PushNotificationsProviderFirebase : PushNotificationsProvider
    {
        private FirebaseApp _firebase;

        public override bool IsEnabledOnCurrentPlatform()
        {
            return !Application.isEditor && Application.isMobilePlatform;
        }

        public override void Init()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            bool isFirebaseReady = await CheckAndFixDependencies();

            if (!isFirebaseReady)
                return;

            await FirebaseMessaging.RequestPermissionAsync();

            FirebaseMessaging.TokenRegistrationOnInitEnabled = true;

            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;

            IsReady = true;
        }

        private async UniTask<bool> CheckAndFixDependencies()
        {
            DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (status == DependencyStatus.Available)
            {
                Debug.Log("Firebase initialized");

                _firebase = FirebaseApp.DefaultInstance;

                return true;
            }

            Debug.LogError($"Could not resolve all Firebase dependencies: {status}");

            return false;
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