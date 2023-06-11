using System.Windows;
using System.Windows.Controls;
using Wpf_Base.PopWindowWpf;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// PopWindowDemo.xaml 的交互逻辑
    /// </summary>
    public partial class PopWindowDemo : UserControl
    {
        public PopWindowDemo()
        {
            InitializeComponent();
        }

        private void ButtonAutoClosed_Click(object sender, RoutedEventArgs e)
        {
            WinMethod.ShowAutoClosedWindow("演示窗体：1s后自动关闭");
        }

        private void ButtonAutoClosedIcon_Click(object sender, RoutedEventArgs e)
        {
            WinMethod.ShowAutoClosedWindowIcon("演示窗体：1s后自动关闭");
        }

        private void ButtonLoading_Click(object sender, RoutedEventArgs e)
        {
            WinMethod.ShowLoadingWindow("演示窗体：程序加载中");
        }

        private void ButtonError_Click(object sender, RoutedEventArgs e)
        {
            WinMethod.ShowMessage("演示窗体：错误弹窗", EnumWindowType.Error);
        }

        private void ButtonQuestion_Click(object sender, RoutedEventArgs e)
        {
            WindowQuestion window = new WindowQuestion("演示窗体：询问");
            window.ShowDialog();
            if (window.IsOnYes)
            {
                WinMethod.ShowAutoClosedWindowIcon("选择了：是");
            }
            else
            {
                WinMethod.ShowAutoClosedWindowIcon("选择了：否");
            }
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            WinMethod.ShowLoginWindow("Admin", "123");
        }
    }
}