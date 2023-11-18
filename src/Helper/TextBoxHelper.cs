using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Awe.UI.Helper
{
    public class TextBoxHelper
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordBindingProperty =
            DependencyProperty.RegisterAttached("PasswordBinding", typeof(string), typeof(TextBoxHelper), new PropertyMetadata("", OnPasswordBindingChanged));

        public static string GetPasswordBinding(DependencyObject obj)
            => (string)obj.GetValue(PasswordBindingProperty);

        public static void SetPasswordBinding(DependencyObject obj, string value)
            => obj.SetValue(PasswordBindingProperty, value);

        public static void OnPasswordBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pwd)
            {
                pwd.PasswordChanged += delegate
                {
                    SetPasswordBinding(pwd, pwd.Password);
                };
            }
        }
    }
}
