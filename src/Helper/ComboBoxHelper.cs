using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Data;

namespace Awe.UI.Helper
{
    public class ComboBoxHelper
    {
        #region UseRewritePopup

        public static readonly DependencyProperty UseRewritePopupProperty =
            DependencyProperty.RegisterAttached("UseRewritePopup", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(false, OnUseRewritePopupChanged));

        public static bool GetUseRewritePopup(ComboBox obj)
            => (bool)obj.GetValue(UseRewritePopupProperty);

        public static void SetUseRewritePopup(ComboBox obj, bool value)
            => obj.SetValue(UseRewritePopupProperty, value);

        private static void OnUseRewritePopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox cb)
            {
                // 用于重写 ComboBox 的 Popup，使显示效果更好。
                if (e.NewValue is true)
                {
                    var ofm = new ContentControl() { Focusable = false };
                    var ev = new System.Windows.Input.MouseButtonEventHandler(delegate { });

                    cb.DropDownOpened += delegate
                    {
                        var menu = (FrameworkElement)cb.Template.FindName("lkc",cb);

                        if (WindowsHelper.MenuDisplayHost is Canvas cv && Application.Current?.MainWindow is var wind)
                        {
                            ev = new System.Windows.Input.MouseButtonEventHandler((_,v) => 
                            {
                                var forMeu = v.GetPosition(menu);

                                if (forMeu.X > menu.ActualWidth || forMeu.Y > menu.ActualHeight || forMeu.X < 0 || forMeu.Y < 0)
                                {
                                    ofm.BeginAnimation(ContentControl.OpacityProperty, new DoubleAnimation
                                    {
                                        Duration = TimeSpan.FromMilliseconds(200),
                                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut},
                                        To = 0,
                                    });
                                    cv.Dispatcher.Invoke(async () =>
                                    {
                                        await Task.Delay(150);
                                        cv.Children.Remove(ofm);
                                        cv.ClearValue(Canvas.BackgroundProperty);
                                    });
                                    cv.PreviewMouseUp -= ev;
                                }
                            });

                            ofm.BeginAnimation(ContentControl.OpacityProperty, new DoubleAnimation
                            {
                                Duration = TimeSpan.FromMilliseconds(200),
                                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                                To = 1,
                            });
                            ofm.Content = GetMenuHostPopup(menu);
                            ofm.SetBinding(ContentControl.WidthProperty, new Binding
                            {
                                Source = cb,
                                Path = new PropertyPath(ComboBox.ActualWidthProperty),
                                Mode = BindingMode.OneWay
                            });

                            var point = cb.TransformToAncestor(wind).Transform(new Point(0, 0));

                            ofm.SetValue(Canvas.LeftProperty, point.X);
                            ofm.SetValue(Canvas.TopProperty, point.Y + cb.ActualHeight);

                            cv.Children.Add(ofm);

                            cv.Background = new SolidColorBrush { Color = Colors.Transparent };

                            cv.PreviewMouseUp += ev;
                        }
                    };
                    cb.SelectionChanged += delegate
                    {
                        if (WindowsHelper.MenuDisplayHost is Canvas cv)
                        {
                            ofm.BeginAnimation(ContentControl.OpacityProperty, new DoubleAnimation
                            {
                                Duration = TimeSpan.FromMilliseconds(200),
                                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                                To = 0,
                            });
                            cv.Dispatcher.Invoke(async () =>
                            {
                                await Task.Delay(150);

                                cv.Children.Remove(ofm);
                                cv.ClearValue(Canvas.BackgroundProperty);
                            });
                            cv.ClearValue(Canvas.BackgroundProperty);
                            cv.PreviewMouseUp -= ev;
                        }
                    };
                    cb.DropDownClosed += delegate
                    {
                        if (WindowsHelper.MenuDisplayHost is Canvas cv)
                        {
                            ofm.BeginAnimation(ContentControl.OpacityProperty, new DoubleAnimation
                            {
                                Duration = TimeSpan.FromMilliseconds(200),
                                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                                To = 0,
                            });
                            cv.Dispatcher.Invoke(async () =>
                            {
                                await Task.Delay(150);

                                cv.Children.Remove(ofm);
                                cv.ClearValue(Canvas.BackgroundProperty);
                            });
                            cv.ClearValue(Canvas.BackgroundProperty);
                            cv.PreviewMouseUp -= ev;
                        }
                    };
                    cb.PreviewMouseWheel += (_, ec) =>
                    {
                        if (cb.SelectedIndex is -1)
                        {
                            cb.SelectedIndex = 0;
                            return;
                        }
                        else if (ec.Delta < 0 && cb.SelectedIndex != cb.Items.Count)
                        {
                            cb.SelectedIndex += 1;
                        }
                        else if (ec.Delta > 0 && cb.SelectedIndex != 0)
                        {
                            cb.SelectedIndex -= 1;
                        }
                    };
                }
            }
        }
        #endregion

        #region MenuHost
        public static readonly DependencyProperty MenuHostPopupProperty =
    DependencyProperty.RegisterAttached("MenuHostPopup", typeof(FrameworkElement), typeof(ComboBoxHelper), new PropertyMetadata());

        public static FrameworkElement GetMenuHostPopup(FrameworkElement obj)
            => (FrameworkElement)obj.GetValue(MenuHostPopupProperty);

        public static void SetMenuHostPopup(FrameworkElement obj, FrameworkElement value)
            => obj.SetValue(MenuHostPopupProperty, value);
        #endregion
    }
}
