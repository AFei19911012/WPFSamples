using HalconDotNet;
using System.Windows;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// HomMat2D_4_7.xaml 的交互逻辑
    /// </summary>
    public partial class HomMat2D_4_7
    {
        public HomMat2D_4_7()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 生成一副图像
            HOperatorSet.GenImageConst(out HObject ho_Image, "byte", 1000, 1000);
            HalconWPF.HalconWindow.ClearWindow();
            HalconWPF.HalconWindow.SetDraw("margin");
            HalconWPF.HalconWindow.SetLineWidth(2);
            HalconWPF.HalconWindow.DispObj(ho_Image);
            HalconWPF.SetFullImagePart();

            // 矩形 1 号
            HOperatorSet.GenRectangle1(out HObject ho_Rectangle1, 700, 300, 900, 500);
            // Marker 点
            HOperatorSet.GenCrossContourXld(out HObject ho_Cross1, 700, 500, 50, 0.785398);
            HalconWPF.HalconWindow.SetColor("red");
            HalconWPF.HalconWindow.DispObj(ho_Rectangle1);
            HalconWPF.HalconWindow.DispObj(ho_Cross1);

            // 变换矩阵
            HTuple hv_Angle = new HTuple(5);
            HOperatorSet.VectorAngleToRigid(700, 300, 0, 400, 300, hv_Angle.TupleRad(), out HTuple hv_HomMat2D);

            // 矩形 2 号：矩形 1 号通过平移、旋转得到
            HOperatorSet.AffineTransRegion(ho_Rectangle1, out HObject ho_Rectangle2, hv_HomMat2D, "nearest_neighbor");
            HOperatorSet.AffineTransContourXld(ho_Cross1, out HObject ho_Cross2, hv_HomMat2D);
            HalconWPF.HalconWindow.SetColor("blue");
            HalconWPF.HalconWindow.DispObj(ho_Rectangle2);
            HalconWPF.HalconWindow.DispObj(ho_Cross2);

            // 矩形 3 号：矩形 1 号通过平移得到
            HOperatorSet.MoveRegion(ho_Rectangle1, out HObject ho_Rectangle3, -300, 400);
            HalconWPF.HalconWindow.SetColor("green");
            HalconWPF.HalconWindow.DispObj(ho_Rectangle3);

            // 3号到2号：（400,700,0）→（400,300,5）
            HOperatorSet.VectorAngleToRigid(400, 700, 0, 400, 300, hv_Angle, out HTuple hv_HomMat2D_3_2);
            HOperatorSet.AffineTransRegion(ho_Rectangle3, out HObject ho_Region_3_2, hv_HomMat2D_3_2, "nearest_neighbor");

            // 2号到1号：（400,300,5）→（700,300,0）
            HOperatorSet.VectorAngleToRigid(400, 300, hv_Angle, 700, 300, 0, out HTuple hv_HomMat2D_2_1);
            HOperatorSet.AffineTransRegion(ho_Region_3_2, out HObject ho_Region_2_1, hv_HomMat2D_2_1, "nearest_neighbor");
            HalconWPF.HalconWindow.SetColor("magenta");
            HalconWPF.HalconWindow.DispObj(ho_Region_2_1);

            // 3号到1号
            HOperatorSet.HomMat2dCompose(hv_HomMat2D_2_1, hv_HomMat2D_3_2, out HTuple hv_HomMat2D_3_1);
            HOperatorSet.AffineTransRegion(ho_Rectangle3, out HObject ho_Region_3_1, hv_HomMat2D_3_1, "nearest_neighbor");
            HalconWPF.HalconWindow.DispObj(ho_Region_3_1);

            ho_Image.Dispose();
            ho_Rectangle1.Dispose();
            ho_Rectangle2.Dispose();
            ho_Rectangle3.Dispose();
            ho_Cross1.Dispose();
            ho_Cross2.Dispose();
            ho_Region_3_2.Dispose();
            ho_Region_2_1.Dispose();
            ho_Region_3_1.Dispose();
        }
    }
}