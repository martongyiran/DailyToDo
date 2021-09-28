using DailyToDo.Extensions;
using DailyToDo.Services.Interfaces;
using DailyToDo.Views;
using Prism.Navigation;
using Prism.Services;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace DailyToDo.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private AsyncCommand _selectLanguageCommand;

        private CultureInfo _language;

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

        public AsyncCommand SelectLanguageCommand
            => _selectLanguageCommand ??= new AsyncCommand(this.WrapWithIsBusy(SelectLanguageAsnyc), allowsMultipleExecutions: false);

        public SettingsPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ICommonConfigService commonConfigService)
            : base(navigationService, dialogService, commonConfigService)
        {
            if (CommonConfigService.Culture.Name != "en-US" || CommonConfigService.Culture.Name != "hu-HU")
            {
                var en = CultureInfo.GetCultureInfo("en-US");
                CommonConfigService.Culture = en;
            }

            Language = CommonConfigService.Culture;
        }

        private async Task SelectLanguageAsnyc()
        {
            await NavigationService.NavigateAsync(nameof(LanguageSelectorPage));
        }
    }
}
