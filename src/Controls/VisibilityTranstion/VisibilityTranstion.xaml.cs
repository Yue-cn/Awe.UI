using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Awe.UI.Controls
{
    public class VisibilityTranstion : ContentControl
    {
        public VisibilityTranstion()
        {
           
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!IsDisplay)
            {
                if (this.GetTemplateChild("layout") is FrameworkElement gr)
                {
                    gr.Opacity = 0;
                    gr.IsHitTestVisible = false;
                    gr.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool IsDisplay
        {
            get { return (bool)GetValue(IsDisplayProperty); }
            set { SetValue(IsDisplayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDisplay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDisplayProperty =
            DependencyProperty.Register("IsDisplay", typeof(bool), typeof(VisibilityTranstion), new PropertyMetadata(false, OnIsDisplayChanged));

        private static void OnIsDisplayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisibilityTranstion vt && vt.GetTemplateChild("layout") is FrameworkElement gr)
            {
                VisualStateManager.GoToElementState(gr, e.NewValue is true ? "Enabled" : "Disabled",false);

                if (vt.GetTemplateChild("pl") is TranslateTransform ttf &&
                    vt.GetTemplateChild("cp") is ContentPresenter cp)
                {
                    if (e.NewValue is false)
                    {
                        ttf.BeginAnimation(TranslateTransform.YProperty, new DoubleAnimation
                        {
                            To = vt.ActualHeight,
                            From = 0,
                            Duration = TimeSpan.FromMilliseconds(350),
                            FillBehavior = FillBehavior.Stop,
                            EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseOut }
                        });
                        cp.BeginAnimation(MarginProperty, new ThicknessAnimation
                        {
                            To = new Thickness(0, -vt.ActualHeight, 0, 0),
                            From = new Thickness(0),
                            Duration = TimeSpan.FromMilliseconds(250),
                            FillBehavior = FillBehavior.HoldEnd,
                            EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseOut }
                        });
                    }
                    else
                    {
                        cp.BeginAnimation(MarginProperty, null);
                        ttf.BeginAnimation(TranslateTransform.YProperty, null);
                    }
                    
                }
            }
        }


    }
}
