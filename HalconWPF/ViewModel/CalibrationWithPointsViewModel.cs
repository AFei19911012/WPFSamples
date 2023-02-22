using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HalconDotNet;
using HalconWPF.Method;
using HalconWPF.Model;
using HalconWPF.UserControl;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/9/25 22:40:53
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/9/25 22:40:53    Taosy.W                 
    ///
    public class CalibrationWithPointsViewModel : ViewModelBase
    {
        private ObservableCollection<CDataModel> dataList;
        public ObservableCollection<CDataModel> DataList
        {
            get => dataList;
            set => Set(ref dataList, value);
        }

        private double valueA1;
        public double ValueA1
        {
            get => valueA1;
            set => Set(ref valueA1, value);
        }

        private double valueB1;
        public double ValueB1
        {
            get => valueB1;
            set => Set(ref valueB1, value);
        }

        private double valueC1;
        public double ValueC1
        {
            get => valueC1;
            set => Set(ref valueC1, value);
        }

        private double valueA2;
        public double ValueA2
        {
            get => valueA2;
            set => Set(ref valueA2, value);
        }

        private double valueB2;
        public double ValueB2
        {
            get => valueB2;
            set => Set(ref valueB2, value);
        }

        private double valueC2;
        public double ValueC2
        {
            get => valueC2;
            set => Set(ref valueC2, value);
        }

        private string showingText;
        public string ShowingText
        {
            get => showingText;
            set => Set(ref showingText, value);
        }

        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;
        private HImage ho_Image;
        private HTuple hv_HomMat2D;

        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as CalibrationWithPoints).HalconWPF;
            ho_Window = Halcon.HalconWindow;

            Halcon.HMouseMove += Halcon_HMouseMove;
        }

        private void Halcon_HMouseMove(object sender, HSmartWindowControlWPF.HMouseEventArgsWPF e)
        {
            int x = (int)e.Column;
            int y = (int)e.Row;
            ShowingText = "x = " + x.ToString() + ", y = " + y.ToString();
        }

        public CalibrationWithPointsViewModel()
        {
            DataList = GetDataList();
            ShowingText = "";
        }

        private ObservableCollection<CDataModel> GetDataList()
        {
            return new ObservableCollection<CDataModel>
            {
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = 2.00, MachineY = 2.00 },
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = 0.00, MachineY = 2.00 },
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = -2.0, MachineY = 2.00 },
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = 2.00, MachineY = 0.00 },
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = 0.00, MachineY = 0.00 },
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = -2.0, MachineY = 0.00 },
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = 2.00, MachineY = -2.0 },
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = 0.00, MachineY = -2.0 },
                new CDataModel{ ImageX = 0, ImageY = 0, MachineX = -2.0, MachineY = -2.0 },
            };
        }

        public RelayCommand<string> CmdButtonEvent => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(ButtonEvent)).Value;
        private void ButtonEvent(string btn)
        {
            // 读取图像
            if (btn == "LoadImage")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image Files (*.png)|*.png|Image Files (*.bmp)|*.bmp|Image Files (*.jpg)|*.jpg",
                    RestoreDirectory = true,
                    Title = "Load Image",
                    InitialDirectory = @"D:\MyPrograms\DataSet\halcon",
                    FileName = "calibration_circle.bmp",
                    FilterIndex = 2,
                };
                string filename;
                if (openFileDialog.ShowDialog() != true)
                {
                    return;
                }
                filename = openFileDialog.FileName;
                ho_Image = new HImage();
                ho_Image.Dispose();
                ho_Image.ReadImage(filename);
                ho_Window.DispObj(ho_Image);
                Halcon.SetFullImagePart();
            }
            // 计算变换矩阵
            else if (btn == "Calibration")
            {
                if (ho_Image == null)
                {
                    return;
                }
                // 机械坐标
                HTuple hv_Machine_x = new HTuple();
                HTuple hv_Machine_y = new HTuple();
                hv_Machine_x.Dispose();
                hv_Machine_y.Dispose();
                for (int i = 0; i < DataList.Count; i++)
                {
                    hv_Machine_x[i] = DataList[i].MachineX;
                    hv_Machine_y[i] = DataList[i].MachineY;
                }
                HRegion ho_Regions = ho_Image.LocalThreshold("adapted_std_deviation", "dark", new HTuple(), new HTuple());
                ho_Regions = ho_Regions.FillUp();
                HRegion ho_ConnectedRegions = ho_Regions.Connection();
                HRegion ho_SelectedRegions = ho_ConnectedRegions.SelectShape("roundness", "and", 0.9, 1);
                HRegion ho_SortedRegions = ho_SelectedRegions.SortRegion("first_point", "true", "row");
                // 设置颜色
                ho_Window.SetColored(12);
                ho_Window.SetDraw("margin");
                ho_Window.SetLineWidth(3);
                // 显示结果
                ho_Window.DispObj(ho_SortedRegions);

                // 生成拟合圆需要的变量对象
                HXLDCont ho_Contours = ho_SortedRegions.GenContourRegionXld("center");
                // 变量初始化
                HTuple hv_Row = new HTuple();
                HTuple hv_Column = new HTuple();
                HTuple hv_Radius = new HTuple();
                HTuple hv_StartPhi = new HTuple();
                HTuple hv_EndPhi = new HTuple();
                HTuple hv_PointOrder = new HTuple();
                HObject ho_Cross = new HObject();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Radius.Dispose();
                hv_StartPhi.Dispose();
                hv_EndPhi.Dispose();
                hv_PointOrder.Dispose();
                ho_Cross.Dispose();
                // 拟合圆
                ho_Contours.FitCircleContourXld("geotukey", -1, 0, 0, 3, 2, out hv_Row, out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                // 绘制圆心
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 50, 0.785398);
                ho_Window.DispObj(ho_Cross);
                ho_Window.SetDisplayFont();
                // 计算仿射矩阵
                hv_HomMat2D = new HTuple();
                hv_HomMat2D.Dispose();
                HOperatorSet.VectorToHomMat2d(hv_Row, hv_Column, hv_Machine_x, hv_Machine_y, out hv_HomMat2D);
                for (int i = 0; i < DataList.Count; i++)
                {
                    DataList[i] = new CDataModel { ImageX = hv_Column[i], ImageY = hv_Row[i], MachineX = DataList[i].MachineX, MachineY = DataList[i].MachineY};
                }

                // 显示仿射矩阵
                ValueA1 = hv_HomMat2D[0];
                ValueB1 = hv_HomMat2D[1];
                ValueC1 = hv_HomMat2D[2];
                ValueA2 = hv_HomMat2D[3];
                ValueB2 = hv_HomMat2D[4];
                ValueC2 = hv_HomMat2D[5];

                // 释放内存
                ho_Regions.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_SortedRegions.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();
                hv_Machine_x.Dispose();
                hv_Machine_y.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Radius.Dispose();
                hv_StartPhi.Dispose();
                hv_EndPhi.Dispose();
                hv_PointOrder.Dispose();
            }
            // 验证
            else if (btn == "Validation")
            {
                ho_Image = new HImage();
                ho_Image.Dispose();
                ho_Image.ReadImage(@"D:\MyPrograms\DataSet\halcon\test_circle.bmp");
                ho_Window.DispObj(ho_Image);
                Halcon.SetFullImagePart();
                HRegion ho_Regions = ho_Image.LocalThreshold("adapted_std_deviation", "dark", new HTuple(), new HTuple());
                ho_Regions = ho_Regions.FillUp();
                HRegion ho_ConnectedRegions = ho_Regions.Connection();
                HRegion ho_SelectedRegions = ho_ConnectedRegions.SelectShape("roundness", "and", 0.9, 1);
                ho_Window.DispObj(ho_SelectedRegions);
                HXLDCont ho_Contours = ho_SelectedRegions.GenContourRegionXld("center");
                // 变量初始化
                HTuple hv_Row = new HTuple();
                HTuple hv_Column = new HTuple();
                HTuple hv_Radius = new HTuple();
                HTuple hv_StartPhi = new HTuple();
                HTuple hv_EndPhi = new HTuple();
                HTuple hv_PointOrder = new HTuple();
                HTuple hv_Qx = new HTuple();
                HTuple hv_Qy = new HTuple();
                HOperatorSet.GenEmptyObj(out HObject ho_Cross);
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Radius.Dispose();
                hv_StartPhi.Dispose();
                hv_EndPhi.Dispose();
                hv_PointOrder.Dispose();
                ho_Cross.Dispose();
                hv_Qx.Dispose();
                hv_Qy.Dispose();
                ho_Contours.FitCircleContourXld("geotukey", -1, 0, 0, 3, 2, out hv_Row, out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 50, 0.785398);
                ho_Window.DispObj(ho_Cross);
                // 验证
                HOperatorSet.AffineTransPoint2d(hv_HomMat2D, hv_Row, hv_Column, out hv_Qx, out hv_Qy);
                HObject ho_Contour = new HObject();
                HTuple hv_Distance = new HTuple();
                ho_Contour.Dispose();
                hv_Distance.Dispose();
                // 画横线
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Row.TupleSelect(1).TupleConcat(hv_Row.TupleSelect(2)), hv_Column.TupleSelect(1).TupleConcat(hv_Column.TupleSelect(2)));
                }
                ho_Window.DispObj(ho_Contour);
                // 计算圆心距
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Distance.Dispose();
                    HOperatorSet.DistancePp(hv_Qy.TupleSelect(1), hv_Qx.TupleSelect(1), hv_Qy.TupleSelect(2), hv_Qx.TupleSelect(2), out hv_Distance);
                }
                // 显示文本
                ho_Window.DispText(hv_Distance + "cm", hv_Row.TupleSelect(1) + 10, hv_Column.TupleSelect(1));
                // 画竖线
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Row.TupleSelect(4).TupleConcat(hv_Row.TupleSelect(6)), hv_Column.TupleSelect(4).TupleConcat(hv_Column.TupleSelect(6)));
                }
                ho_Window.DispObj(ho_Contour);
                // 计算圆心距
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Distance.Dispose();
                    HOperatorSet.DistancePp(hv_Qy.TupleSelect(4), hv_Qx.TupleSelect(4), hv_Qy.TupleSelect(6), hv_Qx.TupleSelect(6), out hv_Distance);
                }
                // 显示文本
                ho_Window.DispText(hv_Distance + "cm", hv_Row.TupleSelect(4), hv_Column.TupleSelect(4));

                // 释放内存
                ho_Regions.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_Contours.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Radius.Dispose();
                hv_StartPhi.Dispose();
                hv_EndPhi.Dispose();
                hv_PointOrder.Dispose();
                hv_Qx.Dispose();
                hv_Qy.Dispose();
                hv_Distance.Dispose();
            }
            // 保存图像
            else if (btn == "SaveImage")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Image Files (*.png)|*.png|Image Files" +
                    " (*.bmp)|*.bmp|Image Files (*.jpg)|*.jpg",
                    Title = "Save Image",
                    RestoreDirectory = true,
                    InitialDirectory = @"D:\MyPrograms\DataSet\halcon",
                    FileName = "result.png"
                };
                string filename;
                if (saveFileDialog.ShowDialog() != true)
                {
                    return;
                }
                filename = saveFileDialog.FileName;
                HImage hImage = ho_Window.DumpWindowImage();
                string ext = Path.GetExtension(filename).Replace(".", "");
                hImage.WriteImage(ext, 0, filename);
                hImage.Dispose();
                HandyControl.Controls.Growl.Info("图像保存成功。");
            }
        }
    }
}