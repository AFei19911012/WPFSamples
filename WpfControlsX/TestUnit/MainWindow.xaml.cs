using System.Windows;
using WpfControlsX.Helper;

namespace TestUnit
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WxWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = DialogHelper.Ask("是否关闭窗口？");
            if (result == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }
            else
            {
                // 必不可少
                e.Cancel = true;
            }
        }
    }
}