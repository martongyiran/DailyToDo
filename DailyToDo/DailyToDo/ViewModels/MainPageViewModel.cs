using DailyToDo.Values;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace DailyToDo
{
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<ToDoItemViewModel> _items;
        private bool _isEditMode;
        private bool _addNewItemIsVisible;
        private string _newTitle;

        public ObservableCollection<ToDoItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public bool AddNewItemIsVisible
        {
            get => _addNewItemIsVisible;
            set => SetProperty(ref _addNewItemIsVisible, value);
        }

        public string NewTitle
        {
            get => _newTitle;
            set => SetProperty(ref _newTitle, value);
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

        public DelegateCommand ShowItemCreatorCommand
            => new DelegateCommand(ShowItemCreator);

        public DelegateCommand<object> CheckCommand
            => new DelegateCommand<object>((item) => CheckItem(item));

        public DelegateCommand<object> DeleteItemCommand
            => new DelegateCommand<object>((item) => DeleteItem(item));

        public DelegateCommand EditCommand
            => new DelegateCommand(Edit);

        public DelegateCommand SaveNewItemCommand
            => new DelegateCommand(SaveNewItem);
        
        public MainPageViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Initialize();
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
        }

        private void ShowItemCreator()
        {
            NewTitle = string.Empty;
            AddNewItemIsVisible = !AddNewItemIsVisible;
        }

        private async void SaveNewItem()
        {
            if (string.IsNullOrEmpty(NewTitle))
            {
                await DialogService.DisplayAlertAsync(null, Texts.SaveError, Texts.Ok);
                return;
            }

            Items.Add(new ToDoItemViewModel
            {
                Title = NewTitle
            });

            PersistDatabase();

            AddNewItemIsVisible = !AddNewItemIsVisible;
        }

        private void CheckItem(object item)
        {
            if (item == null && !(item is ToDoItemViewModel))
            {
                return;
            }

            (item as ToDoItemViewModel).CheckedAt = IsEditMode
                ? new DateTime()
                : DateTime.Now;

            PersistDatabase();
        }

        private void DeleteItem(object item)
        {
            if (item == null && !(item is ToDoItemViewModel))
            {
                return;
            }

            Items.Remove(item as ToDoItemViewModel);
        }

        private void Edit()
        {
            if (IsEditMode)
            {
                PersistDatabase();
            }
            else if (Items == null || Items.Count == 0)
            {
                return;
            }

            IsEditMode = !IsEditMode;
        }

        private void PersistDatabase()
        {
            ApplicationData = JsonConvert.SerializeObject(Items);
        }
    }
}
