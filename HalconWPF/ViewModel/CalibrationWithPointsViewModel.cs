using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
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
        private ObservableCollection<DataModel> dataList;
        public ObservableCollection<DataModel> DataList
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
            ShowingText = "x = " + x.ToString("F2") + ", y = " + y.ToString("F2");
        }

        public CalibrationWithPointsViewModel()
        {
            DataList = GetDataList();
            ShowingText = "";
        }

        private ObservableCollection<DataModel> GetDataList()
        {
            return new ObservableCollection<DataModel>
            {
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = 2.00, MachineY = 2.00 },
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = 0.00, MachineY = 2.00 },
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = -2.0, MachineY = 2.00 },
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = 2.00, MachineY = 0.00 },
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = 0.00, MachineY = 0.00 },
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = -2.0, MachineY = 0.00 },
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = 2.00, MachineY = -2.0 },
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = 0.00, MachineY = -2.0 },
                new DataModel{ ImageX = 0, ImageY = 0, MachineX = -2.0, MachineY = -2.0 },
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
                    InitialDirectory = @"D:\MyPrograms\VisualStudio2019\WPFprograms\WPFSamples\HalconWPF\Resource\Image",
                    FileName = "calibration_circle.bmp"
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
                ho_Window.SetDraw(HalDrawing.margin.ToString());
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
                HDevelopExport.SetDisplayFont(ho_Window, 16, "sans", "true", "false");
                // 计算仿射矩阵
                hv_HomMat2D = new HTuple();
                hv_HomMat2D.Dispose();
                HOperatorSet.VectorToHomMat2d(hv_Row, hv_Column, hv_Machine_x, hv_Machine_y, out hv_HomMat2D);
                for (int i = 0; i < DataList.Count; i++)
                {
                    DataList[i].ImageX = hv_Column[i];
                    DataList[i].ImageY = hv_Row[i];
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
                    InitialDirectory = @"D:\MyPrograms\VisualStudio2019\WPFprograms\WPFSamples\images",
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