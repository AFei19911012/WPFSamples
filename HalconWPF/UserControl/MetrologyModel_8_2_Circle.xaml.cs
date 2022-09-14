using HalconDotNet;
using System.Windows;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// MetrologyModel_8_2_Circle.xaml 的交互逻辑
    /// </summary>
    public partial class MetrologyModel_8_2_Circle
    {
        public MetrologyModel_8_2_Circle()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HOperatorSet.ReadImage(out HObject ho_Image, @"Image\LED.tif");
            // 动态阈值
            HOperatorSet.MedianImage(ho_Image, out HObject ho_ImageMedian, "circle", 3, "mirrored");
            HOperatorSet.DynThreshold(ho_Image, ho_ImageMedian, out HObject ho_Region, 3, "light");
            // 开运算：去掉孤立点
            HOperatorSet.OpeningCircle(ho_Region, out HObject ho_RegionOpening, 2);
            ho_Region.Dispose();
            // 连通
            HOperatorSet.Connection(ho_RegionOpening, out HObject ho_Regions);
            ho_RegionOpening.Dispose();
            // 通过灰度值来区分不亮的
            HOperatorSet.AreaCenter(ho_Regions, out _, out HTuple hv_Row, out HTuple hv_Col);
            HOperatorSet.GetGrayval(ho_ImageMedian, hv_Row, hv_Col, out HTuple hv_Grayvals);
            ho_ImageMedian.Dispose();
            int threshold_gray = 100;
            HOperatorSet.GenEmptyObj(out HObject ho_DarkRegion);
            ho_DarkRegion.Dispose();
            for (int i = 0; i < hv_Grayvals.Length; i++)
            {
                HOperatorSet.SelectObj(ho_Regions, out HObject ho_RegionSelected, i + 1);
                if (hv_Grayvals[i] < threshold_gray)
                {
                    if (!ho_DarkRegion.IsInitialized())
                    {
                        HOperatorSet.Union1(ho_RegionSelected, out ho_DarkRegion);
                    }
                    else
                    {
                        HOperatorSet.Union2(ho_DarkRegion, ho_RegionSelected, out ho_DarkRegion);
                    }
                }
                ho_RegionSelected.Dispose();
            }
            // 统计异常灯珠个数
            ho_Regions.Dispose();
            HOperatorSet.Connection(ho_DarkRegion, out ho_Regions);
            ho_DarkRegion.Dispose();
            hv_Row.Dispose();
            hv_Col.Dispose();
            HOperatorSet.AreaCenter(ho_Regions, out _, out hv_Row, out hv_Col);
            HOperatorSet.GenCrossContourXld(out HObject ho_Cross, hv_Row, hv_Col, 6, 0.785398);
            HalconWPF.HalconWindow.SetDraw("margin");
            HalconWPF.HalconWindow.SetLineWidth(2);
            HalconWPF.HalconWindow.SetColored(12);
            HalconWPF.HalconWindow.DispObj(ho_Image);
            HalconWPF.HalconWindow.DispObj(ho_Cross);
            HalconWPF.HalconWindow.DispObj(ho_Regions);
            HalconWPF.SetFullImagePart();
            ho_Image.Dispose();
            ho_Regions.Dispose();
            ho_Cross.Dispose();
        }
    }
}