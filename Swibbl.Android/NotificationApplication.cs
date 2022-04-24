using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.FirebasePushNotification;
//using Swibbl.Pages;
using System;


namespace Swibbl.Droid
{
    [Application]
    public class NotificationApplication : Application
    {
        public NotificationApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";

                FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;
            }

            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
            FirebasePushNotificationManager.Initialize(this, false);
#endif
            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Received");
                if (p.Data.ContainsKey("body"))
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        var title = $"{p.Data["title"]}";
                        var body = $"{p.Data["body"]}";
                        //var Main = new MainPageModel();
                    });
                }
            };
        }
    }
}