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
    }
}
