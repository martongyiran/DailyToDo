using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace DailyToDo
{
    public class ToDoItemViewModel : BindableBase
    {
        private string _title;
        private DateTime _checkedAt;

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
    }
}
