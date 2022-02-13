using Demos.Method;
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
    }
}