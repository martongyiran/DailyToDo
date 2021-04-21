using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace DailyToDo
{
    public class ToDoItemViewModel : BindableBase
    {
        private string _title;
        private DateTime _checkedAt;
        private bool _isEditMode;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DateTime CheckedAt
        {
            get => _checkedAt;
            set
            {
                SetProperty(ref _checkedAt, value);
                RaisePropertyChanged(nameof(IsChecked));
            } 
        }

        [JsonIgnore]
        public bool IsChecked
            => CheckedAt.Year == DateTime.Now.Year
            && CheckedAt.Month == DateTime.Now.Month
            && CheckedAt.Day == DateTime.Now.Day;

        [JsonIgnore]
        public DelegateCommand<object> CheckCommand { get; set; }

        [JsonIgnore]
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }
    }
}
