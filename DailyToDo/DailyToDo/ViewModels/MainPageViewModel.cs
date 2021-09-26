using DailyToDo.Extensions;
using DailyToDo.Services.Interfaces;
using DailyToDo.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace DailyToDo
{
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<ToDoItemViewModel> _items;
        private AsyncCommand _navigateToSettingsCommand;
        private AsyncCommand _addNewItemCommand;

        public ObservableCollection<ToDoItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

#if DEBUG
        public string Version => string.Format("{0} ({1})", VersionTracking.CurrentVersion, VersionTracking.CurrentBuild) + " DEV";
#else
        public string Version => string.Format("{0} ({1})", VersionTracking.CurrentVersion, VersionTracking.CurrentBuild);
#endif

        public AsyncCommand NavigateToSettingsCommand => _navigateToSettingsCommand ??= new AsyncCommand(this.WrapWithIsBusy(NavigateToSettingsAsync), allowsMultipleExecutions: false);

        public AsyncCommand AddNewItemCommand => _addNewItemCommand ??= new AsyncCommand(this.WrapWithIsBusy(AddNewItemAsync), allowsMultipleExecutions: false);

        public MainPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ICommonConfigService commonConfigService)
            : base(navigationService, dialogService, commonConfigService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.InvokeWithIsBusy(Initialize);
        }

        private void Initialize()
        {
            Items = CommonConfigService.ToDoList != null
                ? new ObservableCollection<ToDoItemViewModel>(CommonConfigService.ToDoList)
                : new ObservableCollection<ToDoItemViewModel>();

            foreach (var item in Items)
            {
                item.CheckCommand = new DelegateCommand<object>((item) => CheckItem(item));
                item.DeleteCommand = new DelegateCommand<object>((item) => DeleteItem(item));
                item.EditCommand = new AsyncCommand<object>(this.WrapWithIsBusy<object>(EditItemAsync), allowsMultipleExecutions: false);
            }
        }

        private void CheckItem(object selected)
        {
            if (selected != null && (selected is ToDoItemViewModel item))
            {
                item.CheckedAt = item.IsChecked
                    ? DateTime.Now.AddDays(-1)
                    : DateTime.Now;

                CommonConfigService.ToDoList = new List<ToDoItemViewModel>(Items);
            }
        }

        private void DeleteItem(object selected)
        {
            if (selected != null && (selected is ToDoItemViewModel item))
            {
                Items.Remove(item);

                CommonConfigService.ToDoList = new List<ToDoItemViewModel>(Items);

                OnPropertyChanged(nameof(Items));
            }
        }

        private async Task EditItemAsync(object selected)
        {
            await NavigationService.NavigateAsync(nameof(AddNewItemPage));
        }
             

        private async Task AddNewItemAsync()
        {
            await NavigationService.NavigateAsync(nameof(AddNewItemPage));
        }

        private async Task NavigateToSettingsAsync()
        {
            await NavigationService.NavigateAsync(nameof(SettingsPage));
        }
    }
}
