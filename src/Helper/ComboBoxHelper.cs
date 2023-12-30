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
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Awe.UI.Helper
{
    public static class ComboBoxHelper
    {
        #region UseRewritePopup

        public static readonly DependencyProperty UseRewritePopupProperty =
            DependencyProperty.RegisterAttached("UseRewritePopup", typeof(bool), typeof(ComboBoxHelper), new PropertyMetadata(false, OnUseRewritePopupChanged));

        public static bool GetUseRewritePopup(ComboBox obj)
            => (bool)obj.GetValue(UseRewritePopupProperty);

        public static void SetUseRewritePopup(ComboBox obj, bool value)
            => obj.SetValue(UseRewritePopupProperty, value);

        private static void Loader(Window? wind,bool sw)
        {
            if (wind is null) return;

            if (WindowsHelper.GetDialogOpenned(wind))
            {
                if (wind.Template.FindName("windDialog", wind) is ContentPresenter cp)
                {
                    if (sw)
                    {
                        cp.ClearValue(KeyboardNavigation.TabNavigationProperty);
                        cp.ClearValue(KeyboardNavigation.DirectionalNavigationProperty);
                        cp.ClearValue(KeyboardNavigation.ControlTabNavigationProperty);
                    }
                    else
                    {
                        KeyboardNavigation.SetControlTabNavigation(cp, KeyboardNavigationMode.None);
                        KeyboardNavigation.SetDirectionalNavigation(cp, KeyboardNavigationMode.None);
                        KeyboardNavigation.SetTabNavigation(cp, KeyboardNavigationMode.None);
                    }
                }
            }
        }

        private static void OnUseRewritePopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox cb)
            {
                // 用于重写 ComboBox 的 Popup，使显示效果更好。
                if (e.NewValue is true)
                {
                    var ofm = new ContentControl() { Focusable = false,Cursor = Cursors.Hand };
                    var ev = new System.Windows.Input.MouseButtonEventHandler(delegate { });

                    cb.PreviewKeyUp += (_, e) =>
                    {
                        if (e.Key is System.Windows.Input.Key.Enter or System.Windows.Input.Key.Space)
                        {
                            cb.IsDropDownOpen = true;
                        }
                    };
                    cb.DropDownOpened += delegate
                    {
                        if (cb.Template is null) { global::System.Diagnostics.Debugger.Break(); return; }

                        var menu = (FrameworkElement)cb.Template.FindName("lkc",cb);

                        if (WindowsHelper.MenuDisplayHost is Canvas cv && Application.Current?.MainWindow is var wind)
                        {
                            if (cv.Children.Contains(ofm)) return;

                            //Keyboard.ClearFocus();

                            Loader(wind, false);

                            var flw = 0;
                            var flh = cb.ActualHeight;
                            ev = new System.Windows.Input.MouseButtonEventHandler((_,v) => 
                            {
                                var forMeu = v.GetPosition(ofm);

                                if (forMeu.X > ofm.ActualWidth || forMeu.Y > ofm.ActualHeight || forMeu.X < 0 || forMeu.Y < 0)
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
                                        Loader(wind, true);
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

                            if (wind?.WindowState is WindowState.Maximized)
                            {
                                flw = -4;
                                flh -= 6;
                            }
                            
                            ofm.SetValue(Canvas.LeftProperty, point.X + flw);
                            ofm.SetValue(Canvas.TopProperty, point.Y + flh);

                            cv.Children.Add(ofm);

                            cv.Background = new SolidColorBrush { Color = Colors.Transparent };
                            cv.Dispatcher.Invoke(async () =>
                            {
                                
                                if (cb.SelectionBoxItem is FrameworkElement fb)
                                {
                                    await Task.Delay(20);

                                    System.Windows.Input.Keyboard.Focus(fb);
                                    //ip.Focus();
                                }
                            });
                            if (cb.Template.FindName("listc", cb) is ItemsPresenter ip)
                            {
                                ip.KeyUp += (_, e) =>
                                {
                                    if (e.Key is Key.Enter && Keyboard.FocusedElement is DependencyObject dod)
                                    {
                                        cb.SelectedIndex = cb.ItemContainerGenerator.IndexFromContainer(dod);
                                        cb.IsDropDownOpen = false;
                                    }
                                    
                                };
                            }

                            ofm.TabIndex = cb.TabIndex + 2;
                            cv.PreviewMouseUp += ev;
                        }
                    };
                    cb.SelectionChanged += delegate
                    {
                        if (WindowsHelper.MenuDisplayHost is Canvas cv && Application.Current?.MainWindow is var wind)
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
                            Loader(wind, true);
                            cv.ClearValue(Canvas.BackgroundProperty);
                            cv.PreviewMouseUp -= ev;
                        }
                    };
                    cb.DropDownClosed += delegate
                    {
                        if (WindowsHelper.MenuDisplayHost is Canvas cv && Application.Current?.MainWindow is var wind)
                        {
                            //if (global::System.Diagnostics.Debugger.IsAttached)
                            //{
                            //    return;
                            //}
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
                            Loader(wind, true);
                            cv.PreviewMouseUp -= ev;
                        }
                    };
                    cb.PreviewMouseWheel += (_, ec) =>
                    {
                        if (ofm.Opacity is 1) return;

                        if (cb.SelectedIndex is -1)
                        {
                            cb.SelectedIndex = 0;
                            return;
                        }
                        else if (ec.Delta < 0 && cb.SelectedIndex != cb.Items.Count)
                        {
                            if (cb.Items[cb.SelectedIndex + 1] is ComboBox cab && cab.IsEnabled)
                            {
                                cb.SelectedIndex += 1;
                            }
                        }
                        else if (ec.Delta > 0 && cb.SelectedIndex != 0)
                        {
                            if (cb.Items[cb.SelectedIndex -1] is ComboBox cab && cab.IsEnabled)
                            {
                                cb.SelectedIndex -= 1;
                            }
                            
                        }
                    };
                }
            }
        }

        private static TaskCompletionSource<bool>? taskWaiter;
        private static ContentControl? container;
        private static SizeChangedEventHandler scevent = new SizeChangedEventHandler(delegate { });
        private static EventHandler lseFcevent = new EventHandler(delegate { });
        private static MouseButtonEventHandler eventForHep = new MouseButtonEventHandler(delegate { });
        private static KeyEventHandler eventForSelect = new KeyEventHandler(delegate { });
        public static async Task WaitForClose(ComboBox cb)
        {
            //if (taskWaiter is not null)
            //{
            //    if (taskWaiter.Task.Status is not TaskStatus.RanToCompletion)
            //    {
            //        taskWaiter.SetCanceled();
            //    }
            //}

            container = new ContentControl() { Focusable = false,Opacity = 0 };

            taskWaiter = new TaskCompletionSource<bool>();

            if (cb.Template is null) { global::System.Diagnostics.Debugger.Break(); return; }

            var menu = (FrameworkElement)cb.Template.FindName("lkc", cb);

            if (WindowsHelper.MenuDisplayHost is Canvas cv && Application.Current?.MainWindow is var wind)
            {
                if (cv.Children.Contains(container)) return;

                Loader(wind, false);

                var flw = 0;
                var flh = cb.ActualHeight;

                container.Content = GetMenuHostPopup(menu);

                // 窗口关闭
                eventForHep = new System.Windows.Input.MouseButtonEventHandler((_, v) =>
                {
                    var forMeu = v.GetPosition(container);

                    if (forMeu.X > container.ActualWidth || forMeu.Y > container.ActualHeight || forMeu.X < 0 || forMeu.Y < 0)
                    {
                        container.BeginAnimation(ContentControl.OpacityProperty, new DoubleAnimation
                        {
                            Duration = TimeSpan.FromMilliseconds(200),
                            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                            To = 0,
                        });
                        cv.Dispatcher.Invoke(async () =>
                        {
                            await Task.Delay(150);
                            Loader(wind, true);
                            cv.Children.Remove(container);
                            cv.ClearValue(Canvas.BackgroundProperty);
                        });
                        cv.PreviewMouseUp -= eventForHep;

                        if (taskWaiter.Task.Status is TaskStatus.RanToCompletion)
                        {
                            return;
                        }
                        taskWaiter.SetResult(true);
                    }
                });
                eventForSelect = new KeyEventHandler((_, v) =>
                {
                    if (v.Key is Key.Enter or Key.Space && (cb is Awe.UI.Controls.RwComboBox cv ? cv.IsOpened : true))
                    {
                        if (Keyboard.FocusedElement is DependencyObject dp)
                        {
                            var vi = cb.ItemContainerGenerator.IndexFromContainer(dp);

                            cb.SelectedIndex = vi;
                        }
                        cb.PreviewKeyUp -= eventForSelect;
                    }
                });
                scevent = new SizeChangedEventHandler(delegate
                {
                    Close(cb);
                    wind!.SizeChanged -= scevent;
                });
                lseFcevent = new EventHandler(delegate
                {
                    Close(cb);
                    wind!.Deactivated -= lseFcevent;
                });

                container.SetBinding(ContentControl.WidthProperty, new Binding
                {
                    Source = cb,
                    Path = new PropertyPath(ComboBox.ActualWidthProperty),
                    Mode = BindingMode.OneWay
                });

                var point = cb.TransformToAncestor(wind).Transform(new Point(0, 0));

                if (wind?.WindowState is WindowState.Maximized)
                {
                    flw = -4;
                    flh -= 6;
                }

                container.SetValue(Canvas.LeftProperty, point.X + flw);
                container.SetValue(Canvas.TopProperty, point.Y + flh);

                container.MaxHeight = wind!.ActualHeight - (point.Y + flh);

                cv.Children.Add(container);
                cv.Background = new SolidColorBrush { Color = Colors.Transparent };
                container.BeginAnimation(ContentControl.OpacityProperty, new DoubleAnimation
                {
                    //BeginTime = TimeSpan.FromMinutes(100),
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    To = 1,
                });
                _ = cv.Dispatcher.Invoke(async () =>
                {
                    if (cb.SelectionBoxItem is FrameworkElement fb)
                    {
                        await Task.Delay(20);
                        System.Windows.Input.Keyboard.Focus(fb);
                    }
                });

                if (cb.Template.FindName("listc", cb) is ItemsPresenter ip)
                {
                    ip.KeyUp += (_, e) =>
                    {
                        if (e.Key is Key.Enter && Keyboard.FocusedElement is DependencyObject dod)
                        {
                            cb.SelectedIndex = cb.ItemContainerGenerator.IndexFromContainer(dod);
                            cb.IsDropDownOpen = false;
                        }

                    };
                }

                container.TabIndex = cb.TabIndex + 2;
                cv.PreviewMouseUp += eventForHep;
                cv.PreviewKeyUp += eventForSelect;
                wind!.SizeChanged += scevent;
                wind!.Deactivated += lseFcevent;

                container.CaptureMouse();
                container.ReleaseMouseCapture();


            }

            await taskWaiter.Task;

            taskWaiter = null;
        }

        public static void Close(ComboBox cb)
        {
            if (taskWaiter is null || container is null ||
                taskWaiter.Task.Status is TaskStatus.RanToCompletion) return;

            taskWaiter.SetResult(false);

            if (WindowsHelper.MenuDisplayHost is Canvas cv && Application.Current?.MainWindow is var wind)
            {
                cv.PreviewMouseUp -= eventForHep;

                container.BeginAnimation(ContentControl.OpacityProperty, new DoubleAnimation
                {
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    To = 0,
                });
                cv.Dispatcher.Invoke(async () =>
                {
                    await Task.Delay(150);
                    Loader(wind, true);
                    cv.Children.Remove(container);
                    cv.ClearValue(Canvas.BackgroundProperty);
                });
                cv.PreviewMouseUp -= eventForHep;
            }
        }
        #endregion

        #region MenuHost
        public static readonly DependencyProperty MenuHostPopupProperty = DependencyProperty.RegisterAttached("MenuHostPopup", typeof(FrameworkElement), typeof(ComboBoxHelper), new PropertyMetadata());

        public static FrameworkElement GetMenuHostPopup(FrameworkElement obj)
            => (FrameworkElement)obj.GetValue(MenuHostPopupProperty);

        public static void SetMenuHostPopup(FrameworkElement obj, FrameworkElement value)
            => obj.SetValue(MenuHostPopupProperty, value);
        #endregion

        #region SelectionContentTemplateRewrite
        public static readonly DependencyProperty SelectionContentTemplateRewriteProperty = DependencyProperty.RegisterAttached("SelectionContentTemplateRewrite", typeof(DataTemplate), typeof(ComboBoxHelper), new PropertyMetadata(OnSelectionContentTemplateRewriteChanged));

        public static DataTemplate GetSelectionContentTemplateRewrite(FrameworkElement obj)
            => (DataTemplate)obj.GetValue(SelectionContentTemplateRewriteProperty);

        public static void SetSelectionContentTemplateRewrite(FrameworkElement obj, DataTemplate value)
            => obj.SetValue(SelectionContentTemplateRewriteProperty, value);

        // 重写 ComboBox 的选择框中的 ContentTemplate
        public static void OnSelectionContentTemplateRewriteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox cb)
            {
                cb.Loaded += delegate
                {
                    if (cb.Template.FindName("conhost", cb) is ContentPresenter cp)
                    {
                        cp.SetBinding(ContentPresenter.ContentTemplateProperty, new Binding
                        {
                            Path = new PropertyPath(SelectionContentTemplateRewriteProperty),
                            Source = cb,
                            Mode = BindingMode.OneWay
                        });
                    }
                };
            }
        }
        #endregion


    }
}
