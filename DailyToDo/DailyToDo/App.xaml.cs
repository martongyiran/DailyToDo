using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using FFImageLoading;
using DailyToDo.Views;
using DailyToDo.ViewModels;

[assembly: ExportFont("Roboto-Medium.ttf", Alias = "MediumFont")]
[assembly: ExportFont("Roboto-Regular.ttf", Alias = "RegularFont")]
[assembly: ExportFont("Roboto-Light.ttf", Alias = "LightFont")]
namespace DailyToDo
{
    public partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            ImageService.Instance.Initialize();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
        }

        protected override void OnStart()
        {
#if PRD
            AppCenter.Start("android=c3a568d3-0800-4b3d-8a01-05e8cbb729fa;", typeof(Analytics), typeof(Crashes));
#endif
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
