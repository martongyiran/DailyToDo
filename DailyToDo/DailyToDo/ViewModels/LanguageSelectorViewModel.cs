﻿using DailyToDo.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Globalization;
using Xamarin.CommunityToolkit.Helpers;

namespace DailyToDo.ViewModels
{
    public class LanguageSelectorViewModel : ViewModelBase
    {
        private DelegateCommand<string> _selectLanguageCommand;

        public DelegateCommand<string> SelectLanguageCommand
            => _selectLanguageCommand ??= new DelegateCommand<string>((x) => SetLanguage(x));

        public bool IsHungarianSelected => CommonConfigService.Culture.Name == "hu-HU";

        public LanguageSelectorViewModel(
            INavigationService navigationService,
            IPageDialogService dialogService,
            ICommonConfigService commonConfigService)
            : base(navigationService, dialogService, commonConfigService)
        {
        }

        private void SetLanguage(string lang)
        {
            var selected = CultureInfo.GetCultureInfo(lang);
            CommonConfigService.Culture = selected;
            LocalizationResourceManager.Current.CurrentCulture = selected;
            OnPropertyChanged(nameof(IsHungarianSelected));
        }
    }
}
