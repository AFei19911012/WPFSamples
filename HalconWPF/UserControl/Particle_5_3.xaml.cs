using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// Particle_5_3.xaml 的交互逻辑
    /// </summary>
    public partial class Particle_5_3
    {
        public Particle_5_3()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HOperatorSet.ReadImage(out HObject ho_Image, @"image\particle.png");
            // 阈值分割
            HOperatorSet.Threshold(ho_Image, out HObject ho_Region, 120, 255);
            // 连通域
            HOperatorSet.Connection(ho_Region, out HObject ho_Regions);
            ho_Region.Dispose();
            // 面积和位置
            HOperatorSet.AreaCenter(ho_Regions, out HTuple hv_Areas, out HTuple hv_Rows, out HTuple hv_Cols);
            // 十字标记点
            HOperatorSet.GenCrossContourXld(out HObject ho_Crosses, hv_Rows, hv_Cols, 20, 0.785398);
            // 显示结果
            HalconWPF.HalconWindow.SetDraw("margin");
            HalconWPF.HalconWindow.SetLineWidth(3);
            HalconWPF.HalconWindow.SetColored(12);
            HalconWPF.HalconWindow.DispObj(ho_Image);
            HalconWPF.HalconWindow.DispObj(ho_Regions);
            HalconWPF.HalconWindow.SetColor("yellow");
            HalconWPF.HalconWindow.DispObj(ho_Crosses);
            ho_Regions.Dispose();
            ho_Image.Dispose();
            ho_Crosses.Dispose();
        }
    }
}