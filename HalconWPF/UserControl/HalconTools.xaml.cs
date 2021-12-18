using System.Windows;
using WSlibs.PopWindow;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// HalconTools.xaml 的交互逻辑
    /// </summary>
    public partial class HalconTools
    {
        public HalconTools()
        {
            InitializeComponent();
        }

        private void ButtonShapeModel_Click(object sender, RoutedEventArgs e)
        {
            WindowShapeModule window = new WindowShapeModule();
            window.Show();
        }
        private void ButtonCaliper_Click(object sender, RoutedEventArgs e)
        {
            WindowCaliperCalibration window = new WindowCaliperCalibration();
            window.Show();
        }
    }
}