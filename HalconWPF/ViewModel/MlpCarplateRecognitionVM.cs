using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HalconDotNet;
using HalconWPF.UserControl;
using System;
using System.Windows;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2022 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2022/5/3 21:51:28
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/5/3 21:51:28    Taosy.W                 
    ///
    public class MlpCarplateRecognitionVM : ViewModelBase
    {
        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;

        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as MlpCarplateRecognition).HalconWPF;
            ho_Window = Halcon.HalconWindow;

            ExecuteCarplateRecognition();
        }

        private void ExecuteCarplateRecognition()
        {
            HOperatorSet.ReadImage(out HObject ho_Image, "audi2");
            /// 定位车牌位置
            // 阈值分割，可借助灰度直方图
            HOperatorSet.Threshold(ho_Image, out HObject ho_Region, 30, 72);
            // 连通
            HOperatorSet.Connection(ho_Region, out HObject ho_Regions);
            ho_Region.Dispose();
            // 特征选择，借助特征直方图
            HTuple features = new HTuple();
            HTuple min_values = new HTuple();
            HTuple max_values = new HTuple();
            features[0] = "width";
            features[1] = "height";
            features[2] = "area";
            min_values[0] = 33.57;
            min_values[1] = 31.14;
            min_values[2] = 810.02;
            max_values[0] = 54.95;
            max_values[1] = 71.89;
            max_values[2] = 1190.55;
            HOperatorSet.SelectShape(ho_Regions, out HObject ho_SelectedRegions, features, "and", min_values, max_values);
            ho_Regions.Dispose();
            // 按照相对位置排序
            HOperatorSet.SortRegion(ho_SelectedRegions, out HObject ho_SortRegions, "upper_left", "true", "column");
            ho_SelectedRegions.Dispose();
            // mlp 分类器
            HOperatorSet.ReadOcrClassMlp("Industrial_NoRej.omc", out HTuple hv_OCRHandle);
            HOperatorSet.DoOcrMultiClassMlp(ho_SortRegions, ho_Image, hv_OCRHandle, out HTuple hv_Class, out _);
            HOperatorSet.ClearOcrClassMlp(hv_OCRHandle);
            hv_OCRHandle.Dispose();
            string msg = "Carplate: ";
            for (int i = 0; i < hv_Class.TupleLength(); i++)
            {
                msg += hv_Class[i];
            }

            ho_Window.SetColored(12);
            ho_Window.DispObj(ho_Image);
            ho_Window.DispObj(ho_SortRegions);
            ho_Window.DispText(msg, "image", 12, 12, "orange red", new HTuple(), new HTuple());
            ho_Image.Dispose();
            ho_SortRegions.Dispose();
            // 图像自适应显示
            Halcon.SetFullImagePart();
        }
    }
}