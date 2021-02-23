using FFImageLoading.Svg.Forms;
using FFImageLoading.Transformations;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DailyToDo.Views.Controls
{
    public class SvgImage : SvgCachedImage
    {
        protected TintTransformation tintTransformation;
        protected ColorFillTransformation colorFillTransformation;
        protected CircleTransformation circleTransformation;
        public static readonly BindableProperty TintColorProperty
            = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(SvgImage), null, propertyChanged: OnTintColorPropertyChanged);
        public static readonly BindableProperty FillColorProperty
            = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(SvgImage), Color.Transparent, propertyChanged: OnFillColorPropertyChanged);
        public static readonly BindableProperty IsCircledProperty
            = BindableProperty.Create(nameof(IsCircled), typeof(bool), typeof(SvgImage), false, propertyChanged: OnCirclePropertyChanged);
        public static readonly BindableProperty CircleBorderWidthProperty
            = BindableProperty.Create(nameof(CircleBorderWidth), typeof(double), typeof(SvgImage), 0.0, propertyChanged: OnCirclePropertyChanged);
        public static readonly BindableProperty CircleBorderColorProperty
            = BindableProperty.Create(nameof(CircleBorderColor), typeof(Color), typeof(SvgImage), Color.Default, propertyChanged: OnCirclePropertyChanged);
        public Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
        public Color FillColor
        {
            get => (Color)GetValue(FillColorProperty);
            set => SetValue(FillColorProperty, value);
        }
        public bool IsCircled
        {
            get => (bool)GetValue(IsCircledProperty);
            set => SetValue(IsCircledProperty, value);
        }
        public double CircleBorderWidth
        {
            get => (double)GetValue(CircleBorderWidthProperty);
            set => SetValue(CircleBorderWidthProperty, value);
        }
        public Color CircleBorderColor
        {
            get => (Color)GetValue(CircleBorderColorProperty);
            set => SetValue(CircleBorderColorProperty, value);
        }
        public static string ColorToHex(Color color)
            => $"#{(int)(color.A * 255):X2}{(int)(color.R * 255):X2}{(int)(color.G * 255):X2}{(int)(color.B * 255):X2}";
        private static void OnTintColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SvgImage)bindable;
            var color = (Color)newValue;
            control.tintTransformation
                = color != Color.Default
                ? new TintTransformation(ColorToHex(color)) { EnableSolidColor = true }
                : null;
            control.ApplyTransformations();
        }
        private static void OnFillColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SvgImage)bindable;
            var color = (Color)newValue;
            control.colorFillTransformation
                = color != Color.Transparent
                ? new ColorFillTransformation(ColorToHex(color))
                : null;
            control.ApplyTransformations();
        }
        private static void OnCirclePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SvgImage)bindable;
            control.circleTransformation
                = control.IsCircled
                ? new CircleTransformation(control.CircleBorderWidth, ColorToHex(control.CircleBorderColor))
                : null;
            control.ApplyTransformations();
        }
        private void ApplyTransformations()
        {
            Transformations = new List<FFImageLoading.Work.ITransformation>
                {
                    tintTransformation,
                    colorFillTransformation,
                    circleTransformation
                }
                .Where(t => t != null)
                .ToList();
        }
    }
}
