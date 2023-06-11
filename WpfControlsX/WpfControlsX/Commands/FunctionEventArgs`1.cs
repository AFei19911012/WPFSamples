using System.Windows;

namespace WpfControlsX.Commands
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 4:39:28
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 4:39:28                     BigWang         首次编写         
    ///
    public class FunctionEventArgs<T> : RoutedEventArgs
    {
        public FunctionEventArgs(T info)
        {
            Info = info;
        }

        public FunctionEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }

        public T Info { get; set; }
    }
}