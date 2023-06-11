using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/15 0:51:16
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/15 0:51:16                     BigWang         首次编写         
    ///
    public static class MessageBox
    {
        public static MessageBoxResult Show(string message, string title, MessageBoxType type = MessageBoxType.Info, int showingTime = 0)
        {
            WxMessageBox box = new(message, title, type, showingTime)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            Window win = null;
            if (Application.Current.Windows.Count > 0)
            {
                win = Application.Current.Windows.OfType<Window>().FirstOrDefault(o => o.IsActive);
            }

            if (win == null)
            {
                box.Show();
            }
            else
            {
                Grid layer = new Grid();
                Brush brush = Application.Current.Resources["BrushText"] as Brush;
                // 半透明背景
                _ = layer.Children.Add(new Rectangle { Fill = brush, Opacity = 0.7 });
                UIElement original = win.Content as UIElement;
                win.Content = null;
                Grid container = new Grid();
                if (original != null)
                {
                    _ = container.Children.Add(original);
                }
                _ = container.Children.Add(layer);
                win.Content = container;
                box.Owner = win;
                _ = box.ShowDialog();
            }
            return box.Result;
        }

        public static MessageBoxResult Show(string message)
        {
            return Show(message, "消息");
        }
    }
}