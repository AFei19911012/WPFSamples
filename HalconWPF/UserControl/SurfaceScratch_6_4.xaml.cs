using HalconDotNet;
using System.Windows;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// SurfaceScratch_6_4.xaml 的交互逻辑
    /// </summary>
    public partial class SurfaceScratch_6_4
    {
        public SurfaceScratch_6_4()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HOperatorSet.ReadImage(out HObject ho_Image, @"Image\surface_scratch_01.png");
            HOperatorSet.GetImageSize(ho_Image, out HTuple hv_Width, out HTuple hv_Height);
            // 反色
            HOperatorSet.InvertImage(ho_Image, out HObject ho_ImageInvert);
            // 频域滤波 增强划痕
            HOperatorSet.GenSinBandpass(out HObject ho_ImageBandpass, 0.4, "none", "rft", hv_Width, hv_Height);
            HOperatorSet.RftGeneric(ho_ImageInvert, out HObject ho_ImageFFT, "to_freq", "none", "complex", hv_Width);
            HOperatorSet.ConvolFft(ho_ImageFFT, ho_ImageBandpass, out HObject ho_ImageConvol);
            HOperatorSet.RftGeneric(ho_ImageConvol, out HObject ho_Lines, "from_freq", "n", "byte", hv_Width);
            ho_ImageInvert.Dispose();
            ho_ImageBandpass.Dispose();
            ho_ImageFFT.Dispose();
            ho_ImageConvol.Dispose();
            // 识别划痕
            HOperatorSet.Threshold(ho_Lines, out HObject ho_Region, 5, 255);
            HOperatorSet.Connection(ho_Region, out HObject ho_ConnectedRegions);
            HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions, "area", "and", 5, 5000);
            ho_Lines.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            // 膨胀
            HOperatorSet.DilationCircle(ho_SelectedRegions, out HObject ho_RegionDilation, 5.5);
            HOperatorSet.Union1(ho_RegionDilation, out HObject ho_RegionUnion);
            ho_SelectedRegions.Dispose();
            ho_RegionDilation.Dispose();
            // 抠图 ROI
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionUnion, out HObject ho_ImageReduced);
            ho_RegionUnion.Dispose();
            // 寻找直线
            HOperatorSet.LinesGauss(ho_ImageReduced, out HObject ho_LinesXLD, 0.8, 3, 5, "dark", "false", "bar-shaped", "false");
            ho_ImageReduced.Dispose();
            // 合并近似共线的轮廓
            HOperatorSet.UnionCollinearContoursXld(ho_LinesXLD, out HObject ho_UnionContours, 40, 3, 3, 0.2, "attr_keep");
            HOperatorSet.SelectShapeXld(ho_UnionContours, out HObject ho_SelectedXLD, "contlength", "and", 15, 1000);
            ho_LinesXLD.Dispose();
            ho_UnionContours.Dispose();
            // 显示划痕位置
            HOperatorSet.GenRegionContourXld(ho_SelectedXLD, out HObject ho_RegionXLD, "filled");
            HOperatorSet.Union1(ho_RegionXLD, out ho_RegionUnion);
            HOperatorSet.DilationCircle(ho_RegionUnion, out HObject ho_RegionScratches, 10.5);
            HalconWPF.HalconWindow.ClearWindow();
            HalconWPF.HalconWindow.SetColored(12);
            HalconWPF.HalconWindow.SetDraw("margin");
            HalconWPF.HalconWindow.SetLineWidth(2);
            HalconWPF.HalconWindow.DispObj(ho_Image);
            HalconWPF.HalconWindow.DispObj(ho_RegionScratches);

            ho_SelectedXLD.Dispose();
            ho_RegionXLD.Dispose();
            ho_RegionUnion.Dispose();
            ho_RegionScratches.Dispose();
        }
    }
}
