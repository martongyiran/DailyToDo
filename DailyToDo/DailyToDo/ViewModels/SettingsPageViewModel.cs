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
        public AsyncCommand BackCommand
            => new AsyncCommand(BackAsync, allowsMultipleExecutions: false);

        public SettingsPageViewModel(INavigationService navigationService,
            IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
        }

        private async Task BackAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
