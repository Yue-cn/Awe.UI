using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Awe.UI.Helper
{
    public class ControlHelper
    {
        public static readonly DependencyProperty IconProperty =
         DependencyProperty.RegisterAttached("Icon", typeof(FrameworkElement), typeof(ControlHelper), new PropertyMetadata());

        public static string GetIcon(DependencyObject obj)
            => (string)obj.GetValue(IconProperty);

        public static void SetIcon(DependencyObject obj, FrameworkElement value)
            => obj.SetValue(IconProperty, value);

        public static readonly DependencyProperty PlaceholderProperty =
 DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(ControlHelper), new PropertyMetadata());

        public static string GetPlaceholder(DependencyObject obj)
            => (string)obj.GetValue(PlaceholderProperty);

        public static void SetPlaceholder(DependencyObject obj, string value)
            => obj.SetValue(PlaceholderProperty, value);

        public static readonly DependencyProperty HeaderProperty =
DependencyProperty.RegisterAttached("Header", typeof(string), typeof(ControlHelper), new PropertyMetadata());

        public static string GetHeader(DependencyObject obj)
            => (string)obj.GetValue(HeaderProperty);

        public static void SetHeader(DependencyObject obj, string value)
            => obj.SetValue(HeaderProperty, value);
    }
}
