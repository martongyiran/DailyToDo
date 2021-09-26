using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace DailyToDo.Services.Interfaces
{
    public interface ICommonConfigService
    {
        OSAppTheme AppTheme { get; set; }

        CultureInfo Culture { get; set; }

        List<ToDoItemViewModel> ToDoList { get; set; }
    }
}
