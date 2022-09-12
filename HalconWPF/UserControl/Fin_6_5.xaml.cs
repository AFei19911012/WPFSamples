using HalconDotNet;
using System.Windows;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// Fin_6_5.xaml 的交互逻辑
    /// </summary>
    public partial class Fin_6_5
    {
        public Fin_6_5()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HOperatorSet.ReadImage(out HObject ho_Image, @"image\fin2.png");
            HOperatorSet.BinaryThreshold(ho_Image, out HObject ho_Region, "max_separability", "dark", out _);
            // 背景
            HOperatorSet.Difference(ho_Image, ho_Region, out HObject ho_RegionBackground);
            ho_Region.Dispose();
            // 闭运算：先膨胀后腐蚀
            HOperatorSet.ClosingCircle(ho_RegionBackground, out HObject ho_RegionClosing, 250);
            // 毛刺区域：带有噪点
            HOperatorSet.Difference(ho_RegionClosing, ho_RegionBackground, out HObject ho_RegionDifference);
            ho_RegionClosing.Dispose();
            ho_RegionBackground.Dispose();
            // 消除小的噪点
            HOperatorSet.OpeningCircle(ho_RegionDifference, out HObject ho_RegionFin, 5);
            ho_RegionDifference.Dispose();
            // 显示结果
            HalconWPF.HalconWindow.SetColor("yellow");
            HalconWPF.HalconWindow.DispObj(ho_Image);
            HalconWPF.HalconWindow.DispObj(ho_RegionFin);
            ho_Image.Dispose();
            ho_RegionFin.Dispose();
        }
    }
}