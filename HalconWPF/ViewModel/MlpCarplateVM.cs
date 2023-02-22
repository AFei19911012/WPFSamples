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
    /// Created Time: 2022/5/4 0:02:15
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/5/4 0:02:15    Taosy.W                 
    ///
    public class MlpCarplateVM : ViewModelBase
    {
        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;

        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as MlpCarplate).HalconWPF;
            ho_Window = Halcon.HalconWindow;

            ExecuteCarplateRecognition();
        }

        private void ExecuteCarplateRecognition()
        {
            HOperatorSet.ReadImage(out HObject ho_Image, "D:/MyPrograms/DataSet/halcon/carplate.jpg");
            /// 车牌定位和校正
            // 图像通道
            HOperatorSet.Decompose3(ho_Image, out HObject ho_ImageR, out HObject ho_ImageG, out HObject ho_ImageB);
            HOperatorSet.TransFromRgb(ho_ImageR, ho_ImageG, ho_ImageB, out _, out HObject ho_ImageS, out _, "hsv");
            ho_ImageR.Dispose();
            ho_ImageG.Dispose();
            ho_ImageB.Dispose();
            // 阈值分割，可借助灰度直方图
            HOperatorSet.Threshold(ho_ImageS, out HObject ho_Region, 50, 255);
            ho_ImageS.Dispose();
            HOperatorSet.FillUp(ho_Region, out HObject ho_RegionFillUp);
            ho_Region.Dispose();
            // 连通
            HOperatorSet.Connection(ho_RegionFillUp, out HObject ho_Regions);
            ho_RegionFillUp.Dispose();
            // 特征选择，借助特征直方图
            HOperatorSet.SelectShape(ho_Regions, out HObject ho_SelectedRegions, "area", "and", 8000, 10000);
            ho_Regions.Dispose();
            HOperatorSet.ShapeTrans(ho_SelectedRegions, out HObject ho_RegionTrans, "convex");
            ho_SelectedRegions.Dispose();
            // 计算角度
            HOperatorSet.OrientationRegion(ho_RegionTrans, out HTuple hv_Phi);
            // 面积和中心
            HOperatorSet.AreaCenter(ho_RegionTrans, out HTuple hv_Area, out HTuple hv_Row, out HTuple hv_Col);
            // 计算变换矩阵
            HTuple hv_HomMat2D = new HTuple();
            hv_HomMat2D.Dispose();
            if (hv_Phi < 0)
            {
                HOperatorSet.VectorAngleToRigid(hv_Row, hv_Col, hv_Phi, hv_Row, hv_Col, 3.14159, out hv_HomMat2D);
            }
            else
            {
                HOperatorSet.VectorAngleToRigid(hv_Row, hv_Col, hv_Phi, hv_Row, hv_Col, 0, out hv_HomMat2D);
            }
            // 校正
            HOperatorSet.AffineTransRegion(ho_RegionTrans, out HObject ho_RegionAffineTrans, hv_HomMat2D, "nearest_neighbor");
            ho_RegionTrans.Dispose();
            HOperatorSet.AffineTransImage(ho_Image, out HObject ho_ImageAffineTrans, hv_HomMat2D, "constant", "false");
            ho_Image.Dispose();
            // 提取车牌区域
            HOperatorSet.ReduceDomain(ho_ImageAffineTrans, ho_RegionAffineTrans, out HObject ho_ImageReduced);
            ho_RegionAffineTrans.Dispose();

            /// 字符分割
            // 灰度图
            HOperatorSet.Rgb1ToGray(ho_ImageReduced, out HObject ho_GrayImage);
            ho_ImageReduced.Dispose();
            // 反色，分类器车牌是黑色，这里是白色
            HOperatorSet.InvertImage(ho_GrayImage, out HObject ho_ImageInvert);
            ho_GrayImage.Dispose();
            // 阈值分割
            HOperatorSet.Threshold(ho_ImageInvert, out ho_Region, 0, 60);
            // 连通
            HOperatorSet.Connection(ho_Region, out ho_Regions);
            ho_Region.Dispose();
            // 特征选择
            HTuple features = new HTuple();
            HTuple min_values = new HTuple();
            HTuple max_values = new HTuple();
            features[0] = "area";
            features[1] = "height";
            min_values[0] = 82.11;
            min_values[1] = 29.312;
            max_values[0] = 281.19;
            max_values[1] = 35.275;
            HOperatorSet.SelectShape(ho_Regions, out ho_SelectedRegions, features, "and", min_values, max_values);
            ho_Regions.Dispose();
            // 按照相对位置排序
            HOperatorSet.SortRegion(ho_SelectedRegions, out HObject ho_SortRegions, "character", "true", "column");
            ho_SelectedRegions.Dispose();
            // mlp 分类器
            HOperatorSet.ReadOcrClassMlp("Industrial_0-9A-Z_NoRej.omc", out HTuple hv_OCRHandle);
            HOperatorSet.DoOcrMultiClassMlp(ho_SortRegions, ho_ImageInvert, hv_OCRHandle, out HTuple hv_Class, out _);
            ho_ImageInvert.Dispose();
            HOperatorSet.ClearOcrClassMlp(hv_OCRHandle);
            hv_OCRHandle.Dispose();
            string msg = "Carplate: ";
            for (int i = 0; i < hv_Class.TupleLength(); i++)
            {
                msg += hv_Class[i];
            }

            ho_Window.SetColored(12);
            ho_Window.DispObj(ho_ImageAffineTrans);
            ho_Window.DispObj(ho_SortRegions);
            ho_Window.DispText(msg, "image", 12, 12, "orange red", new HTuple(), new HTuple());
            ho_ImageAffineTrans.Dispose();
            ho_SortRegions.Dispose();
            // 图像自适应显示
            Halcon.SetFullImagePart();
        }
    }
}