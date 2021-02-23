using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DailyToDo.Views.MarkupExtensions
{
    [ContentProperty("Name")]
    public class IconExtension : IMarkupExtension
    {
        public string Name { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return ImageSourceProvider.GetSvgResourcePath(Name);
        }
    }
}
