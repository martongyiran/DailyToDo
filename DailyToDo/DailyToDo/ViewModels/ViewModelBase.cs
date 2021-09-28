using DailyToDo.Extensions;
using DailyToDo.Services.Interfaces;
using DailyToDo.ViewModels.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace DailyToDo
{
    public class ViewModelBase : ObservableObject, IViewModel, IDestructible
    {
        private bool _isBusy = false;
        private AsyncCommand _goBackCommand;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public INavigationService NavigationService { get; }

        public IPageDialogService DialogService { get; }

        public ICommonConfigService CommonConfigService { get; }

        public AsyncCommand GoBackCommand
            => _goBackCommand ??= new AsyncCommand(this.WrapWithIsBusy(BackAsync), allowsMultipleExecutions: false);

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

        private async Task BackAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
