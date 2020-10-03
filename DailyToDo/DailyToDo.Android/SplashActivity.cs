using Android.App;
using Android.OS;

namespace DailyToDo.Droid
{
    [Activity(Label = "DailyToDo", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true, Icon = "@mipmap/icon")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
        }
    }
}