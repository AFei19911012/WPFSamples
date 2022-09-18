using HalconDotNet;
using System.Windows;


namespace HalconWPF.UserControl
{
    /// <summary>
    /// Pellets_Count_5_4.xaml 的交互逻辑
    /// </summary>
    public partial class Pellets_Count_5_4
    {
        public Pellets_Count_5_4()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HOperatorSet.ReadImage(out HObject ho_Image, @"Image\pellets.png");
            HalconWPF.HalconWindow.ClearWindow();
            HalconWPF.HalconWindow.SetDraw("margin");
            HalconWPF.HalconWindow.SetLineWidth(2);
            HalconWPF.HalconWindow.DispObj(ho_Image);
            HalconWPF.SetFullImagePart();

            // 阈值分割
            HOperatorSet.Threshold(ho_Image, out HObject ho_Region, 100, 255);
            // 连通
            HOperatorSet.Connection(ho_Region, out HObject ho_Regions);
            ho_Region.Dispose();
            // 特征选择
            HOperatorSet.SelectShape(ho_Regions, out HObject ho_SelectedRegions, "area", "and", 156, 9999);
            ho_Regions.Dispose();

            HalconWPF.HalconWindow.SetColored(12);
            HalconWPF.HalconWindow.DispObj(ho_SelectedRegions);

            // 腐蚀 → 连通 → 膨胀
            HOperatorSet.ErosionCircle(ho_SelectedRegions, out HObject ho_RegionErosion, 10);
            HOperatorSet.Connection(ho_RegionErosion, out ho_Regions);
            HOperatorSet.DilationCircle(ho_Regions, out HObject ho_RegionDilation, 10);
            HalconWPF.HalconWindow.DispObj(ho_RegionDilation);

            ho_Image.Dispose();
            ho_Regions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionErosion.Dispose();
            ho_RegionDilation.Dispose();
        }
    }
}