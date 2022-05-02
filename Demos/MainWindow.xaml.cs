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

        private void Button_Click(object sender, RoutedEventArgs e)
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
    }
}