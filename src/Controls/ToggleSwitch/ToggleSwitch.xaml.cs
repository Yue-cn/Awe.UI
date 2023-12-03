using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;

namespace Awe.UI.Controls
{
    public class ToggleSwitch : ToggleButton
    {
        public ToggleSwitch()
        {
            
        }

        private FrameworkElement? __fe { get; set; }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("xr") is FrameworkElement fe)
            {
                __fe = fe;
            }
            if (IsChecked is true && __fe is not null)
            {
                VisualStateManager.GoToElementState(__fe, "SwitchDefaultOn", false);
            }

            base.OnApplyTemplate();
        }

        public static readonly RoutedEvent ToggledEvent = EventManager.RegisterRoutedEvent(nameof(Toggled), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleSwitch));

        public event RoutedEventHandler Toggled
        {
            add { AddHandler(ToggledEvent, value); }
            remove { RemoveHandler(ToggledEvent, value); }
        }

        protected virtual void OnToggled()
        {
            RaiseEvent(new RoutedEventArgs(ToggledEvent));
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            OnToggled();
            if (__fe is not null)
            {
                VisualStateManager.GoToElementState(__fe, "SwitchOn", false);
            }
            base.OnChecked(e);
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            OnToggled();
            if (__fe is not null)
            {
                VisualStateManager.GoToElementState(__fe, "SwitchOff", false);
            }
            base.OnUnchecked(e);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ToggleSwitchAutomationPeer(this);
        }
    }
}
