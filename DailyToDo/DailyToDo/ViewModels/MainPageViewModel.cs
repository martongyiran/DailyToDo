using DailyToDo.Assets.Texts;
using DailyToDo.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace DailyToDo
{
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<ToDoItemViewModel> _items;
        private string _newTitle;
        private ToDoItemViewModel _selectedItem;

        public EventHandler<bool> EditModeOpened;
        public EventHandler<bool> AddNewItemVisibility;

        public ObservableCollection<ToDoItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public string NewTitle
        {
            get => _newTitle;
            set => SetProperty(ref _newTitle, value);
        }

        public ToDoItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string ApplicationData
        {
            get => Preferences.Get(nameof(ApplicationData), null);
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    Preferences.Set(nameof(ApplicationData), value);
                }
                else if (Preferences.ContainsKey(nameof(ApplicationData)))
                {
                    Preferences.Remove(nameof(ApplicationData));
                }
            }
        }

        public bool CenterAddIsVisible => Items == null || Items.Count == 0;

        public AsyncCommand DeleteItemCommand
            => new AsyncCommand(DeleteItemAsync, allowsMultipleExecutions: false);

        public DelegateCommand<object> EditCommand
            => new DelegateCommand<object>((item) => Edit(item));

        public DelegateCommand CloseEditCommand
            => new DelegateCommand(CloseEditModeAsync);

        public AsyncCommand SaveNewItemCommand
            => new AsyncCommand(SaveNewItem, allowsMultipleExecutions: false);

        public AsyncCommand NavigateToSettingsCommand
            => new AsyncCommand(NavigateToSettingsAsync, allowsMultipleExecutions: false);

        public MainPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Task.Run(() => Initialize());
        }

        private void Initialize()
        {
            if (ApplicationData != null)
            {
                Items = JsonConvert.DeserializeObject<ObservableCollection<ToDoItemViewModel>>(ApplicationData);
            }
            else
            {
                Items = new ObservableCollection<ToDoItemViewModel>();
            }

            foreach (var item in Items)
            {
                item.CheckCommand = new DelegateCommand<object>((item) => CheckItem(item));
            }

            RaisePropertyChanged(nameof(CenterAddIsVisible));
        }

        private async Task SaveNewItem()
        {
            if (string.IsNullOrEmpty(NewTitle))
            {
                await DialogService.DisplayAlertAsync(null, Texts.SaveError, Texts.Ok);
                return;
            }

            Items.Add(new ToDoItemViewModel
            {
                Title = NewTitle,
                CheckCommand = new DelegateCommand<object>((item) => CheckItem(item))
            });

            PersistDatabase();

            NewTitle = string.Empty;

            RaisePropertyChanged(nameof(CenterAddIsVisible));

            AddNewItemVisibility?.Invoke(null, true);
        }

        private void CheckItem(object item)
        {
            if (item == null && !(item is ToDoItemViewModel))
            {
                return;
            }

            (item as ToDoItemViewModel).CheckedAt = (item as ToDoItemViewModel).IsEditMode
                ? new DateTime()
                : DateTime.Now;

            PersistDatabase();
        }

        private async Task DeleteItemAsync()
        {
            var result = await DialogService.DisplayAlertAsync(null, "Do you want to delete this item?", "yes", "no");

            if (!result)
            {
                return;
            }

            if (SelectedItem is ToDoItemViewModel)
            {
                Items.Remove(SelectedItem);
                SelectedItem = null;
                PersistDatabase();

                RaisePropertyChanged(nameof(CenterAddIsVisible));

                EditModeOpened?.Invoke(null, false);
            }
        }

        private void Edit(object item)
        {
            if(SelectedItem != null)
            {
                return;
            }


            if (item is ToDoItemViewModel)
            {
                SelectedItem = item as ToDoItemViewModel;
                SelectedItem.IsEditMode = true;
            }

            if (Items == null || Items.Count == 0)
            {
                return;
            }
            else
            {
                EditModeOpened?.Invoke(null, true);
            }
        }

        private async void CloseEditModeAsync()
        {
            if (SelectedItem != null)
            {
                if (string.IsNullOrEmpty(SelectedItem.Title))
                {
                    await DeleteItemAsync();
                    return;
                }

                foreach (var item in Items)
                {
                    item.IsEditMode = false;
                }
                SelectedItem = null;
            }

            PersistDatabase();

            EditModeOpened?.Invoke(null, false);
        }

        private async Task NavigateToSettingsAsync()
        {
            await NavigationService.NavigateAsync(nameof(SettingsPage));
        }

        private void PersistDatabase()
        {
            ApplicationData = JsonConvert.SerializeObject(Items);
        }
    }
}
