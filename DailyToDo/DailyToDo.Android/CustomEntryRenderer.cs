using Android.Graphics.Drawables;
using DailyToDo.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace DailyToDo.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
#pragma warning disable CS0618 // Type or member is obsolete
                this.Control.SetBackgroundDrawable(gd);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}