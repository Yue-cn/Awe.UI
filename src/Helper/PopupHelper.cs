using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Awe.UI.Helper
{
    internal class PopupHelper
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FixPopupProperty =
            DependencyProperty.RegisterAttached("FixPopup", typeof(bool), typeof(PopupHelper), new PropertyMetadata(false, OnFixPopupChanged));

        private static void OnFixPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Popup p && e.NewValue is true)
            {
                p.Opened += delegate
                {
                    if (p.Child is Border br)
                    {
                        var pp = br.TranslatePoint(new Point(), (UIElement)p.TemplatedParent);

                        if (pp.Y > 0)
                        {
                            br.BorderThickness = new Thickness(0.5, 0, 0.5, 1);
                        }
                        else
                        {
                            br.BorderThickness = new Thickness(0.5, 1, 0.5, 0);
                        }
                    }
                };
            }
        }

        public static bool GetFixPopup(DependencyObject obj)
            => (bool)obj.GetValue(FixPopupProperty);

        public static void SetFixPopup(DependencyObject obj, bool value)
            => obj.SetValue(FixPopupProperty, value);
    }
}
