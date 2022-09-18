using HalconDotNet;
using System.Windows;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// Die_5_3.xaml 的交互逻辑
    /// </summary>
    public partial class Die_5_5
    {
        public Die_5_5()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HOperatorSet.ReadImage(out HObject ho_Image, @"Image\die_03.png");
            HalconWPF.HalconWindow.ClearWindow();
            HalconWPF.HalconWindow.SetDraw("fill");
            HalconWPF.HalconWindow.SetLineWidth(2);
            HalconWPF.HalconWindow.DispObj(ho_Image);
            HalconWPF.SetFullImagePart();

            // 阈值分割
            HOperatorSet.Threshold(ho_Image, out HObject ho_RegionBright, 100, 255);
            // 矩形区域
            HOperatorSet.ShapeTrans(ho_RegionBright, out HObject ho_RegionDie, "rectangle2");
            ho_RegionBright.Dispose();
            // 限制区域
            HOperatorSet.ReduceDomain(ho_Image, ho_RegionDie, out HObject ho_ImageDie);
            ho_RegionDie.Dispose();
            // 阈值分割
            HOperatorSet.Threshold(ho_ImageDie, out HObject  ho_Region, 0, 50);
            ho_ImageDie.Dispose();
            // 填充
            HOperatorSet.FillUpShape(ho_Region, out HObject ho_RegionFilled, "area", 1, 100);
            ho_Region.Dispose();
            // 开运算：提取目标区域
            HOperatorSet.OpeningCircle(ho_RegionFilled, out HObject ho_RegionBall, 15);
            ho_RegionFilled.Dispose();
            // 连通
            HOperatorSet.Connection(ho_RegionBall, out HObject ho_Regions);
            ho_RegionBall.Dispose();
            // 特征选择：面积、圆度
            HTuple hv_Features = new HTuple();
            hv_Features[0] = "area";
            hv_Features[1] = "circularity";
            HTuple hv_Mins = new HTuple();
            HTuple hv_Maxs = new HTuple();
            hv_Mins[0] = 500;
            hv_Mins[1] = 0.85;
            hv_Maxs[0] = 1500;
            hv_Maxs[1] = 1;
            HOperatorSet.SelectShape(ho_Regions, out HObject ho_RegionBallTemp, hv_Features, "and", hv_Mins, hv_Maxs);
            ho_Regions.Dispose();
            // 排序
            HOperatorSet.SortRegion(ho_RegionBallTemp, out HObject ho_RegionBalls, "first_point", "true", "column");
            ho_RegionBallTemp.Dispose();
            HalconWPF.HalconWindow.SetColored(12);
            // 外接最小圆
            HOperatorSet.SmallestCircle(ho_RegionBalls, out HTuple hv_Rows, out HTuple hv_Cols, out HTuple hv_Radius);
            ho_RegionBalls.Dispose();

            HalconWPF.HalconWindow.DispCircle(hv_Rows, hv_Cols, hv_Radius);
            HalconWPF.HalconWindow.DispText(hv_Radius.TupleString(".4"), "image", hv_Rows - (2 * hv_Radius), hv_Cols, new HTuple("red"), new HTuple(), new HTuple());
        }
    }
}