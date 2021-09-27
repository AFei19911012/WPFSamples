using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using HalconWPF.UserControl;
using System;
using System.Windows;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/9/21 22:24:21
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/9/21 22:24:21    Taosy.W                 
    ///
    public class PcbDefectDetectionViewModel : ViewModelBase
    {
        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;

        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as PcbDefectDetection).HalconWPF;
            ho_Window = Halcon.HalconWindow;

            ExecutePcbDefectDetection();
        }

        private void ExecutePcbDefectDetection()
        {
            HOperatorSet.GenEmptyObj(out HObject ho_Image);
            HOperatorSet.GenEmptyObj(out HObject ho_ImageOpening);
            HOperatorSet.GenEmptyObj(out HObject ho_ImageClosing);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionDynThresh);
            ho_Image.Dispose();
            ho_ImageOpening.Dispose();
            ho_ImageClosing.Dispose();
            ho_RegionDynThresh.Dispose();
            HOperatorSet.ReadImage(out ho_Image, "pcb");
            //闭运算
            HOperatorSet.GrayOpeningShape(ho_Image, out ho_ImageOpening, 7, 7, "octagon");
            //开运算    
            HOperatorSet.GrayClosingShape(ho_Image, out ho_ImageClosing, 7, 7, "octagon");
            //动态阈值     
            HOperatorSet.DynThreshold(ho_ImageOpening, ho_ImageClosing, out ho_RegionDynThresh, 75, "not_equal");
            ho_Window.SetColor("red");
            ho_Window.DispObj(ho_Image);
            ho_Window.DispObj(ho_RegionDynThresh);
            ho_Image.Dispose();
            ho_ImageOpening.Dispose();
            ho_ImageClosing.Dispose();
            ho_RegionDynThresh.Dispose();
            // 图像自适应显示
            Halcon.SetFullImagePart();
        }
    }
}