using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Threading.Tasks;

namespace Swibbl.Droid
{
    [Activity(Label = "Swibbl", MainLauncher = true, Theme = "@style/MainTheme.Splash", NoHistory = true, Icon = "@mipmap/ic_launcher")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startup = new Task(() => { SimulateStartupAsync(); });
            startup.Start();
        }

        private async Task SimulateStartupAsync()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            throw new NotImplementedException();
        }
    }
}