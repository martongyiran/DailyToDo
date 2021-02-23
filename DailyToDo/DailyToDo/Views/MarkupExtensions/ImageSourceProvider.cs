namespace DailyToDo.Views.MarkupExtensions
{
    public class ImageSourceProvider
    {
        public static string GetSvgResourcePath(string imageName)
        {
            return $"resource://DailyToDo.Assets.Icons.{imageName}.svg?assembly=DailyToDo";
        }
    }
}
