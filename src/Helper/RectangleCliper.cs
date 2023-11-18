using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Awe.UI.Helper
{
    public static class RectangleCliper
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(RectangleCliper), new PropertyMetadata(new CornerRadius(0),OnCornerRadiusChanged));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        => element.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(DependencyObject element)
            => (CornerRadius)element.GetValue(CornerRadiusProperty);

        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (element.Clip != null)
                {
                    return;
                }
                
                

                double Radius = GetCornerRadius(element).TopRight;
                RectangleGeometry geometry = new RectangleGeometry { RadiusX = Radius, RadiusY = Radius };

                MultiBinding binding = new MultiBinding { Converter = new SizeToRectConverter() };
                binding.Bindings.Add(new Binding { Source = 0, });
                binding.Bindings.Add(new Binding { Source = 0, });
                binding.Bindings.Add(new Binding
                {
                    Source = element,
                    Path = new PropertyPath(nameof(element.ActualWidth))
                });
                binding.Bindings.Add(new Binding
                {
                    Source = element,
                    Path = new PropertyPath(nameof(element.ActualHeight))
                });
                BindingOperations.SetBinding(geometry, RectangleGeometry.RectProperty, binding);

                element.Clip = geometry;
            }
        }

        public class SizeToRectConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                double a = values[0] is double ? (double)values[0] : 0;
                double b = values[1] is double ? (double)values[1] : 0;
                double c = values[2] is double ? (double)values[2] : 0;
                double d = values[3] is double ? (double)values[3] : 0;
                return new Rect(a, b, c, d);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                Rect rect = value is Rect ? (Rect)value : new Rect(0, 0, 0, 0);
                return new object[] { rect.TopLeft.X, rect.TopLeft.Y, rect.BottomRight.X, rect.BottomRight.Y };
            }
        }
    }
}
