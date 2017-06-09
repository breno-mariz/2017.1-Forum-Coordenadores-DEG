using System;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;
using System.Diagnostics;

namespace ForumDEG.Droid {
    internal class CrossPushNotificationListener : IPushNotificationListener {
        public void OnError(string message, DeviceType deviceType) {
            Debug.WriteLine(" [NotificationListener] OnError: " + message);
        }

        public void OnMessage(JObject values, DeviceType deviceType) {
            Debug.WriteLine(" [NotificationListener] OnMessage");
        }

        public void OnRegistered(string token, DeviceType deviceType) {
            Debug.WriteLine(" [NotificationListener] OnRegistered: " + token);
        }

        public void OnUnregistered(DeviceType deviceType) {
            Debug.WriteLine(" [NotificationListener] OnUnregistered");
        }

        public bool ShouldShowNotification() {
            Debug.WriteLine(" [NotificationListener] ShouldShowNotification");
            return true;
        }
    }
}