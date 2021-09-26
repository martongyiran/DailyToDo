using DailyToDo.Services.Interfaces;
using DailyToDo.ViewModels.Interfaces;
using Prism.Navigation;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace DailyToDo
{
    public class ViewModelBase : ObservableObject, IViewModel, IDestructible
    {
        private bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public INavigationService NavigationService { get; }

        public IPageDialogService DialogService { get; }

        public ICommonConfigService CommonConfigService { get; }

        public ViewModelBase(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ICommonConfigService commonConfigService)
        {
            NavigationService = navigationService;
            DialogService = dialogService;
            CommonConfigService = commonConfigService;
        }

        public void Destroy()
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
