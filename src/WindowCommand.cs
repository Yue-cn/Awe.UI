using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Awe.UI
{
    public class WindowCommand : ICommand
    {
        private string _handle = "";

        private WindowCommand(string handle) { _handle = handle; }

        event EventHandler ICommand.CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is Window wind)
            {
                switch (_handle)
                {
                    case "close":
                        {
                            wind.Close();
                            break;
                        }
                    case "minimize":
                        {
                            wind.WindowState = WindowState.Minimized;
                            break;
                        }
                    case "maximize":
                        {
                            wind.WindowState = WindowState.Maximized;
                            break;
                        }
                    case "restore":
                        {
                            wind.WindowState = WindowState.Normal;
                            break;
                        }
                    case "cdf":
                        {
                            wind.Dispatcher.Invoke(async () =>
                            {
                                wind.SetValue(Awe.UI.Helper.WindowsHelper.DialogOpennedProperty, false);
                                await Task.Delay(250);
                                wind.SetCurrentValue(Awe.UI.Helper.WindowsHelper.DialogContentProperty, DependencyProperty.UnsetValue);
                            });
                            break;
                        }
                }
            }
        }


        public static ICommand CloseWindowCommand = new WindowCommand("close");
        public static ICommand MinimizeWindowCommand = new WindowCommand("minimize");
        public static ICommand MaximizeWindowCommand = new WindowCommand("maximize");
        public static ICommand RestoreWindowCommand = new WindowCommand("restore");
        public static ICommand CloseDialogForWindowCommand = new WindowCommand("cdf");
    }
}
