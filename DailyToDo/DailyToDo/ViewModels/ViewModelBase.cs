using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace DailyToDo
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        private bool _isBusy = false;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService DialogService { get; private set; }

        public ViewModelBase(
            INavigationService navigationService,
            IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            DialogService = dialogService;
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
