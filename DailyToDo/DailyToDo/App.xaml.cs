using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using FFImageLoading;
using DailyToDo.Views;
using DailyToDo.ViewModels;
using DailyToDo.Services;
using DailyToDo.Services.Interfaces;
using Xamarin.CommunityToolkit.Helpers;
using DailyToDo.Assets.Texts;

[assembly: ExportFont("Roboto-Medium.ttf", Alias = "MediumFont")]
[assembly: ExportFont("Roboto-Regular.ttf", Alias = "RegularFont")]
[assembly: ExportFont("Roboto-Light.ttf", Alias = "LightFont")]
namespace DailyToDo
{
    public partial class App : PrismApplication
    {
        private static ICommonConfigService _commonConfigService;

        private ICommonConfigService CommonConfigService => _commonConfigService ??= Container.Resolve<ICommonConfigService>();

        public App()
        {
            InitializeComponent();
        }

        protected override async void OnInitialized()
        {
            LocalizationResourceManager.Current.PropertyChanged += (sender, e) =>
            {
                Texts.Culture = LocalizationResourceManager.Current.CurrentCulture;
                CommonConfigService.Culture = LocalizationResourceManager.Current.CurrentCulture;
            };

            LocalizationResourceManager.Current.Init(Texts.ResourceManager);
            LocalizationResourceManager.Current.CurrentCulture = CommonConfigService.Culture;

            InitializeComponent();

            ImageService.Instance.Initialize();

            var commonConfig = Container.Resolve<ICommonConfigService>();

            Application.Current.UserAppTheme = commonConfig.AppTheme;

            await NavigationService.NavigateAsync(nameof(MainPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AddNewItemPage, AddNewItemPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<LanguageSelectorPage, LanguageSelectorViewModel>();
            containerRegistry.RegisterSingleton<ICommonConfigService, CommonConfigService>();
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
