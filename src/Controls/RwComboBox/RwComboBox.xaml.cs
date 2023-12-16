using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Awe.UI.Controls
{
    public partial class RwComboBox : ComboBox
    {

        /// <inheritdoc cref="ComboBox.IsDropDownOpen"/>
        public bool IsOpened
        {
            get { return (bool)GetValue(IsOpenedProperty); }
            set { SetValue(IsOpenedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpened.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenedProperty =
            DependencyProperty.Register("IsOpened", typeof(bool), typeof(RwComboBox), new PropertyMetadata(false,OnIsOpenedChanged));

        public static void OnIsOpenedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is true)
            {
                d.SetValue(IsDropDownOpenProperty, true);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key is Key.Enter or Key.Space)
            {
                if (IsDropDownOpen == false)
                {
                    IsDropDownOpen = true;
                }
                else if (Keyboard.FocusedElement is DependencyObject dp)
                {
                    var vi = this.ItemContainerGenerator.ItemFromContainer(dp);
                }
            }

            e.Handled = true;
        }
        

        protected override async void OnDropDownOpened(EventArgs e)
        {
            IsOpened = true;
            await Awe.UI.Helper.ComboBoxHelper.WaitForClose(this);
            IsOpened = false;

            base.OnDropDownOpened(e);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            Awe.UI.Helper.ComboBoxHelper.Close(this);

            base.OnSelectionChanged(e);
        }
    }
}
