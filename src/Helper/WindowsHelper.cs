using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Awe.UI.Helper
{
    public static class WindowsHelper
    {

        #region VisibilityToFocus

        public static readonly DependencyProperty VisibilityToFocusProperty =
            DependencyProperty.RegisterAttached("VisibilityToFocus", typeof(bool), typeof(WindowsHelper), new PropertyMetadata(false, OnVisibilityToFocusChanged));

        public static bool GetVisibilityToFocus(FrameworkElement obj)
            => (bool)obj.GetValue(VisibilityToFocusProperty);

        public static void SetVisibilityToFocus(FrameworkElement obj, bool value)
            => obj.SetValue(VisibilityToFocusProperty, value);

        private static void OnVisibilityToFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContentPresenter fe)
            {
                IInputElement? bfFocused = null;
                fe.IsVisibleChanged += async delegate
                    {
                    if (fe.Visibility is Visibility.Visible)
                    {
                        bfFocused = Keyboard.FocusedElement;
                         
                        Keyboard.ClearFocus();

                        if (fe.Content is FrameworkElement e)
                        {
                            var ve = FocusManager.GetFocusedElement(e);

                            while (Keyboard.FocusedElement == null)
                            {
                                await Task.Delay(15);

                                Keyboard.Focus(ve);
                            }
                        }
                    }
                    else
                    {
                        //

                        if (bfFocused is not null)
                            Keyboard.Focus(bfFocused);
                    }
                };
            }
        }

        #endregion

        #region MaximumSnap
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumSnapProperty =
            DependencyProperty.RegisterAttached("MaximumSnap", typeof(bool), typeof(WindowsHelper), new PropertyMetadata(false,OnMaximumSnapChanged));

        public static bool GetMaximumSnap(Window obj)
            => (bool)obj.GetValue(MaximumSnapProperty);

        public static void SetMaximumSnap(Window obj, bool value)
            => obj.SetValue(MaximumSnapProperty, value);

        private static async void OnMaximumSnapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button btn)
            {
                if (Environment.OSVersion.Version >= new Version(10, 0, 21996) &&
                    Environment.OSVersion.Platform == PlatformID.Win32NT)

                if (e.NewValue is true)
                {
                    try
                    {
                        _button = btn;

                        HwndSource hwnd = (HwndSource)HwndSource.FromVisual(btn);

                        await Task.Delay(1500);

                        if (hwnd is null) return;
#if NET462_OR_NEWER
                        _dpiScale = VisualTreeHelper.GetDpi(button).DpiScaleX;
#else
                        Matrix transformToDevice = hwnd.CompositionTarget.TransformToDevice;
                        _dpiScale = transformToDevice.M11;
#endif
                        hwnd.AddHook(HwndSourceHook);
                    }
                    catch
                    {

                    } 
                }
            }
        }

        #endregion

        #region Maximun Fix
        private static SolidColorBrush DefaultButtonBackground = new SolidColorBrush
        {
            Color = Colors.Transparent
        };
        private static SolidColorBrush DefaultButtonForeground = new SolidColorBrush
        {
            Color = Colors.White
        };

        private static SolidColorBrush _hoverColor = new SolidColorBrush
        {
            Color = Color.FromArgb(0x1f,0xE6, 0xE6, 0xE6)
        };

        private static SolidColorBrush _pressColor = new SolidColorBrush
        {
            Color = Color.FromArgb(0x0f,0xCC, 0xCC, 0xCC)
        };
        private static SolidColorBrush _pressFeColor = new SolidColorBrush
        {
            Color = Color.FromArgb(0x9f, 0xff, 0xff, 0xff)
        };

        private static bool _isButtonFocused;

        private static bool _isButtonClicked;

        private static double _dpiScale;

        private static Button? _button;

        /// <summary>
        /// Represents the method that handles Win32 window messages.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="uMsg">The message ID.</param>
        /// <param name="wParam">The message's wParam value.</param>
        /// <param name="lParam">The message's lParam value.</param>
        /// <param name="handled">A value that indicates whether the message was handled. Set the value to <see langword="true"/> if the message was handled; otherwise, <see langword="false"/>.</param>
        /// <returns>The appropriate return value depends on the particular message. See the message documentation details for the Win32 message being handled.</returns>
        private static IntPtr HwndSourceHook(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // TODO: This whole class is one big todo

            Win32.User32.WM mouseNotification = (Win32.User32.WM)uMsg;

            //System.Diagnostics.Debug.WriteLine(mouseNotification);

            switch (mouseNotification)
            {
                case Win32.User32.WM.NCLBUTTONDOWN:
                    if (IsOverButton(wParam, lParam))
                    {
                        _isButtonClicked = true;

                        PressedButton();

                        handled = true;
                    }
                    break;
                case Win32.User32.WM.NCMOUSELEAVE:
                    DefocusButton();
                    break;

                case Win32.User32.WM.NCLBUTTONUP:
                    if (_isButtonClicked)
                    {
                        if (IsOverButton(wParam, lParam))
                        {
                            RaiseButtonClick();
                        }
                        _isButtonClicked = false;

                    }
                    else {
                        if (IsOverButton(wParam, lParam))
                        {
                            DepressedButton();

                            if (Application.Current is { } wind && wind.MainWindow is not null)
                            {
                                if (wind.MainWindow.WindowState is WindowState.Maximized)
                                {
                                    SystemCommands.RestoreWindow(Application.Current.MainWindow);
                                }
                                else
                                {
                                    SystemCommands.MaximizeWindow(Application.Current.MainWindow);
                                }
                                
                            }
                            
                        }
                    }
                    break;

                case Win32.User32.WM.NCHITTEST:
                    if (IsOverButton(wParam, lParam))
                    {
                        FocusButton();
                        handled = true;
                    }
                    else
                    {
                        DefocusButton();
                    }
                    return new IntPtr((int)Win32.User32.HT.MAXBUTTON);

                default:
                    handled = false;
                    break;
            }
            return new IntPtr((int)Win32.User32.HT.CLIENT);
        }

        private static void FocusButton()
        {
            if (_isButtonFocused) return;

            if (_button is not null)
                _button.Background = _hoverColor;
            _isButtonFocused = true;
        }

        private static void DefocusButton()
        {
            if (!_isButtonFocused) return;

            if (_button is not null)
            {
                _button.Background = DefaultButtonBackground;
                _button.Foreground = DefaultButtonForeground;
            }
            _isButtonFocused = false;
        }

        private static void PressedButton()
        {
            if (!_isButtonClicked) return;

            if (_button is not null)
            {
                _button.Background = _pressColor;
                _button.Foreground = _pressFeColor;
            }
            

            _isButtonClicked = false;
        }

        private static void DepressedButton()
        {
            if (_button is not null)
            {
                _button.Background = DefaultButtonBackground;
                _button.Foreground = DefaultButtonForeground;
            }
        }

        private static bool IsOverButton(IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (_button is null) return false;

                int positionX = lParam.ToInt32() & 0xffff;
                int positionY = lParam.ToInt32() >> 16;

                Rect rect = new Rect(_button.PointToScreen(new Point()),
                    new Size(_button.Width * _dpiScale, _button.Height * _dpiScale));

                if (rect.Contains(new Point(positionX, positionY)))
                    return true;
            }
            catch (OverflowException)
            {
                return true; // or not to true, that is the question
            }

            return false;
        }

        private static void RaiseButtonClick()
        {
            if (new ButtonAutomationPeer(_button).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProv)
                invokeProv?.Invoke();
        }

        //private void SetHoverColor()
        //{
        //    //_hoverColor = (SolidColorBrush)Application.Current.Resources["SystemControlHighlightListLowBrush"] ?? new SolidColorBrush(Color.FromArgb(21, 255, 255, 255));
        //}

        #endregion

        #region DialongContent
        public static readonly DependencyProperty DialogContentProperty =
    DependencyProperty.RegisterAttached("DialogContent", typeof(FrameworkElement), typeof(WindowsHelper), new PropertyMetadata(null,OnDialogContentChanged));

        public static FrameworkElement GetDialogContent(Window obj)
            => (FrameworkElement)obj.GetValue(DialogContentProperty);

        public static void SetDialogContent(Window obj, FrameworkElement value)
            => obj.SetValue(DialogContentProperty, value);

        public static readonly DependencyProperty DialogOpennedProperty =
            DependencyProperty.RegisterAttached("DialogOpenned", typeof(bool), typeof(WindowsHelper), new PropertyMetadata(false));

        public static bool GetDialogOpenned(Window obj)
            => (bool)obj.GetValue(DialogOpennedProperty);

        public static void SetDialogOpenned(Window obj, bool value)
            => obj.SetValue(DialogOpennedProperty, value);

        private static void OnDialogContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is null && d is Window) { SetDialogOpenned((Window)d, false); }
        }
        #endregion

        #region UseLightMode
        public static readonly DependencyProperty UseLightModeProperty =
           DependencyProperty.RegisterAttached("UseLightMode", typeof(bool), typeof(WindowsHelper), new PropertyMetadata(false, OnUseLightModeChanged));

        public static bool GetUseLightMode(FrameworkElement obj)
            => (bool)obj.GetValue(UseLightModeProperty);

        public static void SetUseLightMode(FrameworkElement obj, bool value)
            => obj.SetValue(UseLightModeProperty, value);

        private static void OnUseLightModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (true)
            {
                DefaultButtonForeground.Color = e.NewValue is true ? Colors.Black : Colors.White;
                _pressFeColor.Color = e.NewValue is true ? Color.FromArgb(0x9f, 0x0,0x0,0x0) : Color.FromArgb(0x9f, 0xff, 0xff, 0xff);
            }
        }


        #endregion

        #region LightModeRebind
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LightModeRebindProperty =
            DependencyProperty.RegisterAttached("LightModeRebind", typeof(bool), typeof(WindowsHelper), new PropertyMetadata(false, OnLightModeRebindChanged));

        public static string GetLightModeRebind(FrameworkElement obj)
            => (string)obj.GetValue(LightModeRebindProperty);

        public static void SetLightModeRebind(FrameworkElement obj, bool value)
            => obj.SetValue(LightModeRebindProperty, value);

        public static void OnLightModeRebindChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is true && d is FrameworkElement cm)
            {
                if (Application.Current?.MainWindow is Window wind)
                {
                    cm.SetBinding(WindowsHelper.UseLightModeProperty, new Binding
                    {
                        Source = wind,
                        Mode = BindingMode.OneWay,
                        Path = new PropertyPath(WindowsHelper.UseLightModeProperty)
                    });
                    if (cm is ContextMenu)
                    {
                        if (!cm.Resources.Contains("ThemeBindingProxy"))
                        {
                            cm.Resources.Add("ThemeBindingProxy", new Converter.BindingProxy
                            {
                                Data = cm
                            });
                        }
                    }
                }
            }
        }
        #endregion

        #region MenuHost

        internal static Canvas? MenuDisplayHost;

        public static readonly DependencyProperty IsMenuHostProperty =
            DependencyProperty.RegisterAttached("IsMenuHost", typeof(bool), typeof(WindowsHelper), new PropertyMetadata(false, OnIsMenuHostChanged));

        public static bool GetIsMenuHost(Canvas obj)
            => (bool)obj.GetValue(IsMenuHostProperty);

        public static void SetIsMenuHost(Canvas obj, bool value)
            => obj.SetValue(IsMenuHostProperty, value);

        private static void OnIsMenuHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas fe)
            {
                if (e.NewValue is true)
                {
                    if (MenuDisplayHost is not null)
                    {
                        throw new ArgumentException("已有 MenuHost 对象。");
                    }
                    MenuDisplayHost = fe;
                }
                else
                {
                    MenuDisplayHost = default;
                }
            }
        }
        #endregion
    }
}
