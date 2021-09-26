using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Android.Views;

namespace DailyToDo.Droid
{
    [Activity(
        Label = "Daily ToDo",
        Icon ="@mipmap/icon",
        Theme = "@style/MainTheme.Splash",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // https://www.xamboy.com/2018/03/29/sharing-svg-icons-across-platforms-in-xamarin-forms/
            {
                CachedImageRenderer.Init(true);
                SvgCachedImage.Init();
                // ReSharper disable once AssignmentIsFullyDiscarded
                _ = typeof(SvgCachedImage);
            }

            LoadApplication(new App());
            Window.SetSoftInputMode(SoftInput.AdjustResize);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}