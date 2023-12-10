using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Awe.UI.Helper
{
    public class TwiceBindingHelper
    {

        public static readonly DependencyProperty BindingProperty =
                DependencyProperty.RegisterAttached("Binding", typeof(TwiceBindingHelper), typeof(TwiceBindingHelper), new PropertyMetadata(null, OnBindingChanged));

        public static TwiceBindingHelper GetBinding(FrameworkElement obj)
            => (TwiceBindingHelper)obj.GetValue(BindingProperty);

        public static void SetBinding(FrameworkElement obj, TwiceBindingHelper value)
            => obj.SetValue(BindingProperty, value);

        //private static void 

        private static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe && e.NewValue is TwiceBindingHelper tbh)
            {
                fe.SetBinding(tbh.Property, tbh.Binding);
            }
        }

        public DependencyProperty? Property { get; set; }
        public BindingBase? Binding { get; set; }
    }
}
