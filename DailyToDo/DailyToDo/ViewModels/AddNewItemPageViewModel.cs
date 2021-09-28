using DailyToDo.Extensions;
using DailyToDo.Services.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace DailyToDo.ViewModels
{
    public class AddNewItemPageViewModel : ViewModelBase
    {
        private string _item;
        private Guid? _itemId;
        private AsyncCommand _saveCommand;
        private string originalValue;

        public string Item
        {
            get => _item;
            set
            {
                if (SetProperty(ref _item, value))
                {
                    OnPropertyChanged(nameof(Changed));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public Guid? ItemId
        {
            get => _itemId;
            set => SetProperty(ref _itemId, value);
        }

        public bool Changed => originalValue != Item && !string.IsNullOrEmpty(Item);

        public AsyncCommand SaveCommand => _saveCommand ??= new AsyncCommand(this.WrapWithIsBusy(SaveAsync), ()=> Changed, allowsMultipleExecutions: false);

        public AddNewItemPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ICommonConfigService commonConfigService)
            : base(navigationService, dialogService, commonConfigService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            ItemId = parameters.TryGetValue(nameof(ItemId), out Guid id)
                ? id
                : null;

            if (ItemId != null)
            {
                var originalItem = CommonConfigService.ToDoList.First(x => x.Id == ItemId);
                originalValue = originalItem.Title;
                OnPropertyChanged(nameof(originalValue));
                Item = originalItem.Title;
            }
        }

        private async Task SaveAsync()
        {
            var list = CommonConfigService.ToDoList != null
            ? CommonConfigService.ToDoList
            : new List<ToDoItemViewModel>();

            if (ItemId == null)
            {
                list.Add(new ToDoItemViewModel
                {
                    Title = Item
                });
            }
            else
            {
                list.First(x => x.Id == ItemId).Title = Item;
            }

            CommonConfigService.ToDoList = list;

            await NavigationService.GoBackAsync();
        }
    }
}
