using Newtonsoft.Json;
using Prism.Commands;
using System;
using Xamarin.CommunityToolkit.ObjectModel;

namespace DailyToDo
{
    public class ToDoItemViewModel : ObservableObject
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
                if (SetProperty(ref _checkedAt, value))
                {
                    OnPropertyChanged(nameof(IsChecked));
                }
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
        public DelegateCommand<object> DeleteCommand { get; set; }

        [JsonIgnore]
        public AsyncCommand<object> EditCommand { get; set; }
    }
}
