using DailyToDo.Extensions;
using DailyToDo.Services.Interfaces;
using DailyToDo.Views;
using Prism.Navigation;
using Prism.Services;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DailyToDo.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private AsyncCommand _selectLanguageCommand;
        private CultureInfo _language;
        private bool _isDarkMode;

#if DEBUG
        public string Version => string.Format("{0} ({1})", VersionTracking.CurrentVersion, VersionTracking.CurrentBuild) + " DEV";
#else
        public string Version => string.Format("{0} ({1})", VersionTracking.CurrentVersion, VersionTracking.CurrentBuild);
#endif

        public CultureInfo Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (SetProperty(ref _isDarkMode, value))
                {
                    SetTheme();
                }
            }
        }

        public AsyncCommand SelectLanguageCommand
            => _selectLanguageCommand ??= new AsyncCommand(this.WrapWithIsBusy(SelectLanguageAsnyc), allowsMultipleExecutions: false);

        public SettingsPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ICommonConfigService commonConfigService)
            : base(navigationService, dialogService, commonConfigService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Language = CommonConfigService.Culture;

            _isDarkMode = CommonConfigService.AppTheme == OSAppTheme.Dark;
        }

        private async Task SelectLanguageAsnyc()
        {
            await NavigationService.NavigateAsync(nameof(LanguageSelectorPage));
        }

        private void SetTheme()
        {
            var theme = _isDarkMode ? OSAppTheme.Dark : OSAppTheme.Light;
            CommonConfigService.AppTheme = theme;
            Application.Current.UserAppTheme = theme;
        }
    }
}
