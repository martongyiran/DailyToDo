using DailyToDo.Services.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System.ComponentModel;

namespace DailyToDo.ViewModels.Interfaces
{
    public interface IViewModel : INavigationAware, INotifyPropertyChanged
    {
        INavigationService NavigationService { get; }

        IPageDialogService DialogService { get; }

        ICommonConfigService CommonConfigService { get; }

        bool IsBusy { get; set; }
    }
}
