using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Awe.UI.Helper
{
    public static class ControlHelper
    {
        public static readonly DependencyProperty IconProperty =
         DependencyProperty.RegisterAttached("Icon", typeof(FrameworkElement), typeof(ControlHelper), new PropertyMetadata());

        public static FrameworkElement GetIcon(DependencyObject obj)
            => (FrameworkElement)obj.GetValue(IconProperty);

        public static void SetIcon(DependencyObject obj, FrameworkElement value)
            => obj.SetValue(IconProperty, value);

        public static readonly DependencyProperty PlaceholderProperty =
 DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(ControlHelper), new PropertyMetadata());

        public static string GetPlaceholder(FrameworkElement obj)
            => (string)obj.GetValue(PlaceholderProperty);

        public static void SetPlaceholder(FrameworkElement obj, string value)
            => obj.SetValue(PlaceholderProperty, value);

        public static readonly DependencyProperty HeaderProperty =
DependencyProperty.RegisterAttached("Header", typeof(string), typeof(ControlHelper), new PropertyMetadata());

        public static string GetHeader(FrameworkElement obj)
            => (string)obj.GetValue(HeaderProperty);

        public static void SetHeader(FrameworkElement obj, string value)
            => obj.SetValue(HeaderProperty, value);
    }
}
