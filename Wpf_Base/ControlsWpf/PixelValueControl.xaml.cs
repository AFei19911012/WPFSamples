using System.Windows.Controls;

namespace Wpf_Base.ControlsWpf
{
    /// <summary>
    /// PixelValueControl.xaml 的交互逻辑
    /// </summary>
    public partial class PixelValueControl : UserControl
    {
        public PixelValueControl()
        {
            InitializeComponent();
        }

        public void RefreshInfo(int x, int y, string value)
        {
            TB_Position.Text = string.Format("X = {0}, Y = {1}", x, y);
            TB_PixelValue.Text = "Pixel = " + value;
        }
    }
}