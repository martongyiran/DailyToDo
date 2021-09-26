using DailyToDo.Services.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace DailyToDo.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private AsyncCommand _backCommand;
        public AsyncCommand BackCommand
            => _backCommand ??= new AsyncCommand(BackAsync, allowsMultipleExecutions: false);

        public SettingsPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ICommonConfigService commonConfigService)
            : base(navigationService, dialogService, commonConfigService)
        {
        }

        private async Task BackAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
