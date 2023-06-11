using System;
using System.Windows;
using System.Windows.Input;

namespace WpfControlsX.Commands
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 3:55:26
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 3:55:26                     BigWang         首次编写         
    ///
    public class ShutdownAppCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public static implicit operator ShutdownAppCommand(RoutedCommand v)
        {
            throw new NotImplementedException();
        }
    }
}