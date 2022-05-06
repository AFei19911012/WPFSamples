using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using HalconWPF.UserControl;
using System;
using System.Windows;
using WSlibs.Method;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2022 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2022/5/6 23:33:20
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/5/6 23:33:20    Taosy.W                 
    ///
    public class MlpNumberRecognitionVM : ViewModelBase
    {
        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;

        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as MlpNumberRecognition).HalconWPF;
            ho_Window = Halcon.HalconWindow;

            ExecuteNumberRecognition();
        }

        private void ExecuteNumberRecognition()
        {
            HOperatorSet.ReadImage(out HObject ho_Image, "D:/MyPrograms/DataSet/halcon/ocr数字识别/Image_01.bmp");
            HOperatorSet.Threshold(ho_Image, out HObject ho_Region, 0, 200);
            HOperatorSet.ClosingCircle(ho_Region, out HObject ho_RegionClosing, 5);
            ho_Region.Dispose();
            HOperatorSet.Connection(ho_RegionClosing, out HObject ho_ConnectedRegions);
            ho_RegionClosing.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions, "area", "and", 2000, 20000);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.SortRegion(ho_SelectedRegions, out HObject ho_SortedRegions, "first_point", "true", "column");
            ho_SelectedRegions.Dispose();
            // 分类器
            HOperatorSet.ReadOcrClassMlp("Industrial_NoRej.omc", out HTuple hv_OCRHandle);
            HOperatorSet.DoOcrMultiClassMlp(ho_SortedRegions, ho_Image, hv_OCRHandle, out HTuple hv_Class, out _);
            string msg = "Number: ";
            for (int i = 0; i < hv_Class.TupleLength(); i++)
            {
                msg += hv_Class[i];
            }
            ho_Window.SetColored(12);
            ho_Window.DispObj(ho_Image);
            ho_Window.DispObj(ho_SortedRegions);
            ho_Window.SetDisplayFont(24);
            ho_Window.DispText(msg, 12, 12);
            ho_Image.Dispose();
            ho_SortedRegions.Dispose();
            // 图像自适应显示
            Halcon.SetFullImagePart();
        }
    }
}