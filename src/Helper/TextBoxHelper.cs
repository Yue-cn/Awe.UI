using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Awe.UI.Helper
{
    public class TextBoxHelper
    {
        #region PasswordBinding
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordBindingProperty =
            DependencyProperty.RegisterAttached("PasswordBinding", typeof(string), typeof(TextBoxHelper), new PropertyMetadata("", OnPasswordBindingChanged));

        public static string GetPasswordBinding(DependencyObject obj)
            => (string)obj.GetValue(PasswordBindingProperty);

        public static void SetPasswordBinding(DependencyObject obj, string value)
            => obj.SetValue(PasswordBindingProperty, value);

        public static void OnPasswordBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is true && d is PasswordBox pwd)
            {
                pwd.PasswordChanged += delegate
                {
                    SetPasswordBinding(pwd, pwd.Password);
                };
            }
        }
        #endregion

        #region IntInputOnly
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntInputOnlyProperty =
            DependencyProperty.RegisterAttached("IntInputOnly", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, OnIntInputOnlyChanged));

        public static string GetIntInputOnly(DependencyObject obj)
            => (string)obj.GetValue(IntInputOnlyProperty);

        public static void SetIntInputOnly(DependencyObject obj, bool value)
            => obj.SetValue(IntInputOnlyProperty, value);

        public static void OnIntInputOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is true && d is TextBox tb)
            {
                int ccval = -1;
                if (!tb.Text.Equals(string.Empty) && int.TryParse(tb.Text,out var i))
                {
                    ccval = i;
                }
                tb.PreviewTextInput += (c, b) =>
                {


                    if (!int.TryParse(b.Text, out var va))
                    {
                        if (va is not 0)
                        {
                            if (tb.Text.Length is 0) return;
                        }
                        b.Handled = true;
                    }

                };
                tb.TextChanged += (c, b) =>
                {
                    if (b.Changes.Count is 1 && b.UndoAction is UndoAction.Create or UndoAction.Merge)
                    {
                        var va = b.Changes.ElementAt(0);
                        if (true)
                        {
                            if (va.AddedLength > 0)
                            {
                                if (!int.TryParse(tb.Text.Substring(va.Offset, va.AddedLength), out _))
                                {
                                    if (ccval is not -1)
                                    {
                                        if (tb.Text.Equals(string.Empty)) { ccval = -1; }
                                        else
                                        {
                                            tb.Text = ccval.ToString();
                                            SetIntInputRewriteBinding(d, ccval);
                                            return;
                                        }
                                    }
                                    tb.Text = "0"; tb.SelectionStart = tb.Text.Length;
                                    SetIntInputRewriteBinding(d, 0);
                                    return;
                                }
                            }
                            if (int.TryParse(tb.Text, out var bl))
                            {
                                SetIntInputRewriteBinding(d, ccval = bl);
                                if (bl > 65535)
                                {
                                    tb.Text = "65535";
                                    SetIntInputRewriteBinding(d, ccval = 65535);
                                    tb.SelectionStart = tb.Text.Length;

                                }
                            }
                            return;
                        }
                    }
                    else if (b.UndoAction is UndoAction.Clear)
                    {
                        if (int.TryParse(tb.Text, out var ii))
                        {
                            ccval = ii;
                        }
                    }
                    //tb.Text = "";

                    //b.Handled = true;
                };
                tb.LostFocus += (c, b) =>
                {
                    if (tb.Text.Equals(string.Empty))
                    {
                        if (ccval != -1)
                        {
                            tb.Text = ccval.ToString();
                        }
                    }
                };
            }
        }
        #endregion

        #region IntInputRewriteBinding
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntInputRewriteBindingProperty =
            DependencyProperty.RegisterAttached("IntInputRewriteBinding", typeof(int), typeof(TextBoxHelper), new PropertyMetadata(0));

        public static string GetIntInputRewriteBinding(DependencyObject obj)
            => (string)obj.GetValue(IntInputRewriteBindingProperty);

        public static void SetIntInputRewriteBinding(DependencyObject obj, int value)
            => obj.SetValue(IntInputRewriteBindingProperty, value);
        #endregion
    }
}
