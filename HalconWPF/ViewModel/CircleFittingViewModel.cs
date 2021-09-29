using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using HalconWPF.Halcon;
using HalconWPF.Method;
using HalconWPF.UserControl;
using System;
using System.Windows;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/9/18 0:27:25
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/9/18 0:27:25    Taosy.W                 
    ///
    public class CircleFittingViewModel : ViewModelBase
    {
        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;

        /// <summary>
        /// Halcon 控件关联，写在 Loaded 事件里
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as CircleFitting).HalconWPF;
            ho_Window = Halcon.HalconWindow;

            // 散点拟合圆案例
            ExecuteCircleFitting();
        }

        private void ExecuteCircleFitting()
        {
            HOperatorSet.GenEmptyObj(out HObject ho_Cross);
            HOperatorSet.GenEmptyObj(out HObject ho_Contour);
            HOperatorSet.GenEmptyObj(out HObject ho_ContCircle);
            HTuple hv_Rows = new HTuple();
            HTuple hv_Cols = new HTuple();
            HTuple hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();
            HTuple hv_Radius = new HTuple();
            HTuple hv_StartPhi = new HTuple();
            HTuple hv_EndPhi = new HTuple();
            HTuple hv_PointOrder = new HTuple();
            ho_Cross.Dispose();
            ho_Contour.Dispose();
            ho_ContCircle.Dispose();
            hv_Rows.Dispose();
            hv_Cols.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Radius.Dispose();
            hv_StartPhi.Dispose();
            hv_EndPhi.Dispose();
            hv_PointOrder.Dispose();

            int number = 10;
            double center_x = 250;
            double center_y = 250;
            double r = 100;
            for (int i = 0; i < number; i++)
            {
                hv_Rows[i] = center_x + (r * Math.Cos(i * 2 * Math.PI / number));
                hv_Cols[i] = center_y + (r * Math.Sin(i * 2 * Math.PI / number));
            }
            HImage ho_Image = new HImage();
            ho_Image.GenEmptyObj();
            ho_Window.DispObj(ho_Image);
            Halcon.SetFullImagePart();
            ho_Window.SetLineWidth(2);
            // 画十字
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Rows, hv_Cols, 30, 0.785398);
            ho_Window.SetColor(HalColor.blue.ToString());
            //ho_Window.SetColored(12);
            ho_Window.DispObj(ho_Cross);
            // 拟合圆
            HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
            HOperatorSet.FitCircleContourXld(ho_Contour, "geotukey", -1, 0, 0, 3, 2, out hv_Row, out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
            ho_Window.DispObj(ho_Cross);
            // 生成圆
            HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_Row, hv_Column, hv_Radius, 0, 6.28318, "positive", 1);
            ho_Window.DispObj(ho_ContCircle);
            ho_Window.DispText(hv_Row + ", " + hv_Column + ", " + hv_Radius, hv_Row, hv_Column);

            ho_Cross.Dispose();
            ho_Contour.Dispose();
            ho_ContCircle.Dispose();
            hv_Rows.Dispose();
            hv_Cols.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Radius.Dispose();
            hv_StartPhi.Dispose();
            hv_EndPhi.Dispose();
            hv_PointOrder.Dispose();
        }
    }
}
