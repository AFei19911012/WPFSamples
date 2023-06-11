using System;
using System.Windows;
using System.Windows.Input;

namespace WpfControlsX.Commands
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 3:56:24
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 3:56:24                     BigWang         首次编写         
    ///
    public class CloseWindowCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is DependencyObject dependencyObject)
            {
                if (Window.GetWindow(dependencyObject) is { } window)
                {
                    window.Close();
                }
            }
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public static implicit operator CloseWindowCommand(RoutedCommand v)
        {
            throw new NotImplementedException();
        }
    }
}