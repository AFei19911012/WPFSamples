using Demos.Demo;
using Demos.Method;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Demos
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// NPOI 读写 Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNPOI_Click(object sender, RoutedEventArgs e)
        {
            NpoiMethod.NpoiDemo();
        }

        private void BtnLogger_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(AddLogger);
            task.Start();
        }

        private void AddLogger()
        {
            for (int i = 0; i < 1005; i++)
            {
                if (i % 5 == 0)
                {
                    MyLogger.AddLogger("测试测试", Demo.EnumLoggerType.Error);
                }
                else if (i % 4 == 0)
                {
                    MyLogger.AddLogger("测试测试", Demo.EnumLoggerType.Warning);
                }
                else if (i % 3 == 0)
                {
                    MyLogger.AddLogger("测试测试", Demo.EnumLoggerType.Success);
                }
                else
                {
                    MyLogger.AddLogger("测试测试", Demo.EnumLoggerType.Info);
                }
                Thread.Sleep(1);
            }
        }

        private void BtnAutoClosedWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowAutoClosedSuccess window = new WindowAutoClosedSuccess("测试窗体，一秒后自动关闭");
            _ = window.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowClosedQuestion window = new WindowClosedQuestion();
            _ = window.ShowDialog();

            if (window.IsClosing)
            {
                // 可以做一些事情

                // 关闭窗体
                Application.Current.Shutdown();
            }
            else
            {
                // 必不可少
                e.Cancel = true;
                return;
            }
        }
    }
}