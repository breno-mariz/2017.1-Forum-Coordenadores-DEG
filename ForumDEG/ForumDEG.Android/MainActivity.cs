using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Android.Content;
using PushNotification.Plugin;

namespace ForumDEG.Droid {
    [Activity(Label = "Fórum DEG", Icon = "@drawable/icon_android", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        public static Context AppContext;

        protected override void OnCreate(Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(this);

            AppContext = this.ApplicationContext;

            CrossPushNotification.Initialize<CrossPushNotificationListener>("<17416083189>");

            StartPushService();

            LoadApplication(new App());
        }

        public static void StartPushService() {
            AppContext.StartService(new Intent(AppContext, typeof(PushNotificationService)));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat) {

                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

        public static void StopPushService() {
            AppContext.StopService(new Intent(AppContext, typeof(PushNotificationService)));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat) {
                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

    }
}

