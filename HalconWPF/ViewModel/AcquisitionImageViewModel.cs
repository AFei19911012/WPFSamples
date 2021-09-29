using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using HalconWPF.Method;
using HalconWPF.UserControl;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/8/23 22:57:37
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/8/23 22:57:37    Taosy.W                 
    ///
    public class AcquisitionImageViewModel : ViewModelBase
    {
        // 定义全局变量
        private HTuple hv_AcqHandle = null;
        private HObject ho_Image = null;
        private HWindow ho_Window;
        private Thread ho_thread;
        private HSmartWindowControlWPF Halcon;

        private string strAcqMode;
        public string StrAcqMode
        {
            get => strAcqMode;
            set => Set(ref strAcqMode, value);
        }

        /// <summary>
        /// Halcon 控件关联，写在 Loaded 事件里
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as AcquisitionImage).HalconWPF;
            ho_Window = Halcon.HalconWindow;
        }

        /// <summary>
        /// 触发模式
        /// </summary>
        public RelayCommand CmdTrigger => new Lazy<RelayCommand>(() => new RelayCommand(Trigger)).Value;
        private void Trigger()
        {
            hv_AcqHandle = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Image);
            hv_AcqHandle.Dispose();
            // 获取相机信息
            string camInfo = HalMethod.GetInfoFramegrabber();
            HandyControl.Controls.Growl.Info(camInfo);
            // 启动相机
            HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default", "default", 0, -1, out hv_AcqHandle);
            // 启动笔记本摄像头
            //HOperatorSet.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb", -1, "false", "default", "[0] ", 0, -1, out hv_AcqHandle);
            HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
            ho_Image.Dispose();
            HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
            HOperatorSet.GetImageSize(ho_Image, out HTuple width, out HTuple height);
            HOperatorSet.SetPart(ho_Window, 0, 0, height - 1, width - 1);
            // Halcon 图像自适应显示
            Halcon.SetFullImagePart();
            ho_Window.DispObj(ho_Image);
            // 关闭摄像头
            HOperatorSet.CloseFramegrabber(hv_AcqHandle);
            ho_Image.Dispose();
            hv_AcqHandle.Dispose();
            HandyControl.Controls.Growl.Info("Image Captured.");
        }

        /// <summary>
        /// 实时模式
        /// </summary>
        public RelayCommand CmdRealTime => new Lazy<RelayCommand>(() => new RelayCommand(RealTime)).Value;
        private void RealTime()
        {
            if (StrAcqMode == "RealTime")
            {
                hv_AcqHandle = new HTuple();
                HOperatorSet.GenEmptyObj(out ho_Image);
                hv_AcqHandle.Dispose();
                // 启动相机
                //HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive", 8, "default", -1, "false", "auto", "U3V:00F86140145 MV-CE050-30UC", 0, -1, out hv_AcqHandle);
                //HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive", 8, "default", -1, "false", "auto", "fefault", 0, -1, out hv_AcqHandle);
                //HOperatorSet.OpenFramegrabber("GenICamTL", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default", "default", 0, -1, out hv_AcqHandle);
                //HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive", -1, "default", -1, "false", "default", "default", 0, -1, out hv_AcqHandle);
                // 启动笔记本自带摄像头
                HOperatorSet.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb", -1, "false", "default", "[0] ", 0, -1, out hv_AcqHandle);
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                StrAcqMode = "Stop";
                // 实时采集线程
                ho_thread = new Thread(ContinuesGrab);
                ho_thread.Start();
                ho_thread.IsBackground = true;
                HandyControl.Controls.Growl.Info("Camera Opening.");
            }
            else
            {
                // 释放
                ho_thread.Abort();
                StrAcqMode = "RealTime";
                HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                ho_Image.Dispose();
                hv_AcqHandle.Dispose();
                HandyControl.Controls.Growl.Info("Camera Stopped.");
            }
        }

        /// <summary>
        /// 保存结果图
        /// </summary>
        public RelayCommand CmdSave => new Lazy<RelayCommand>(() => new RelayCommand(Save)).Value;
        private void Save()
        {
            if (ho_Window == null)
            {
                return;
            }
            ho_Image.Dispose();
            HOperatorSet.DumpWindowImage(out ho_Image, ho_Window);
            HOperatorSet.WriteImage(ho_Image, "png", 0, @"D:\MyPrograms\VisualStudio2019\WPFprograms\WPFSamples\HalconWPF\bin\Debug\capture.png");
            HandyControl.Controls.Growl.Info("Image Saved.");
        }


        /// <summary>
        /// 定义实时采集函数
        /// </summary>
        private void ContinuesGrab()
        {
            while (true)
            {
                // 先释放内存
                ho_Image.Dispose();
                HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
                HOperatorSet.GetImageSize(ho_Image, out HTuple width, out HTuple height);
                //HOperatorSet.SetPart(ho_Window, 0, 0, height - 1, width - 1);
                HOperatorSet.DispObj(ho_Image, ho_Window);
                // Halcon 图像自适应显示
                // 在新的线程里用如下方式更新到界面
                _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    Halcon.SetFullImagePart();
                }
                );
            }
        }

        /// <summary>
        /// 构造函数，初始化参数
        /// </summary>
        public AcquisitionImageViewModel()
        {
            StrAcqMode = "RealTime";
        }
    }
}
