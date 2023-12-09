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
    public partial class NavigationButton : ButtonBase
    {
        public NavigationButton()
        {
            
        }

        public string NavigateUrl
        {
            get { return (string)GetValue(NavigateUrlProperty); }
            set { SetValue(NavigateUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavigateUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigateUrlProperty =
            DependencyProperty.Register("NavigateUrl", typeof(string), typeof(NavigationButton), new PropertyMetadata());

        protected override void OnClick()
        {
            if (!string.IsNullOrEmpty(NavigateUrl))
            {
                if (Uri.TryCreate(NavigateUrl, UriKind.RelativeOrAbsolute, out _))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(NavigateUrl);

                        return;
                    }
                    catch
                    {

                    }
                    try
                    {
                        System.Diagnostics.Process.Start("cmd", $"/c start {NavigateUrl}");
                    }
                    catch
                    {

                    }
                }
            }

            base.OnClick();
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("__TranLayout") is FrameworkElement fe)
            {
                if (!fe.IsEnabled)
                {
                    VisualStateManager.GoToState(this, "DfDisabledEnd", false);
                }
                fe.IsEnabledChanged += delegate
                {
                    VisualStateManager.GoToState(this, fe.IsEnabled ? "DfEnabled" : "DfDisabled", false);
                };
            }

            base.OnApplyTemplate();
        }


        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new NavigationButtonAutomationPeer(this);
        }
    }
}
