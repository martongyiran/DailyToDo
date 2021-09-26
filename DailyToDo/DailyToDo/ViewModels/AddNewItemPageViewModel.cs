using DailyToDo.Extensions;
using DailyToDo.Services.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace DailyToDo.ViewModels
{
    public class AddNewItemPageViewModel : ViewModelBase
    {
        private string _item;
        private AsyncCommand _saveCommand;

        public string Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        public AsyncCommand SaveCommand => _saveCommand ??= new AsyncCommand(this.WrapWithIsBusy(SaveAsync), allowsMultipleExecutions: false);

        public AddNewItemPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ICommonConfigService commonConfigService)
            : base(navigationService, dialogService, commonConfigService)
        {
        }

        private async Task SaveAsync()
        {
            var list = CommonConfigService.ToDoList != null
            ? CommonConfigService.ToDoList
            : new List<ToDoItemViewModel>();

            list.Add(new ToDoItemViewModel
            {
                Title = Item
            });
            CommonConfigService.ToDoList = list;

            await NavigationService.GoBackAsync();
        }
    }
}
