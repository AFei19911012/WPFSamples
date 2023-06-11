using System;
using System.Windows;
using System.Windows.Input;
using WpfControlsX.Helper;

namespace WpfControlsX.Commands
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 4:13:17
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 4:13:17                     BigWang         首次编写         
    ///
    public class PushMainWindow2TopCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (Application.Current.MainWindow != null && Application.Current.MainWindow.Visibility != Visibility.Visible)
            {
                Application.Current.MainWindow.Show();
                WindowHelper.SetWindowToForeground(Application.Current.MainWindow);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public static implicit operator PushMainWindow2TopCommand(RoutedCommand v)
        {
            throw new NotImplementedException();
        }
    }
}