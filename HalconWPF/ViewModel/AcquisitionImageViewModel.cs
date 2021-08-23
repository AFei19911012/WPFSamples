using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using System;
using System.Threading;

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
        private bool IsHalconReleted = false;


        private string strAcqMode;
        public string StrAcqMode
        {
            get => strAcqMode;
            set => Set(ref strAcqMode, value);
        }

        /// <summary>
        /// Halcon 控件关联，写在主窗体的 Loaded 事件里
        /// </summary>
        public RelayCommand<HSmartWindowControlWPF> CmdLoaded => new Lazy<RelayCommand<HSmartWindowControlWPF>>(() => new RelayCommand<HSmartWindowControlWPF>(Loaded)).Value;
        private void Loaded(HSmartWindowControlWPF halcon)
        {
            
        }

        /// <summary>
        /// 触发模式
        /// </summary>
        public RelayCommand<HSmartWindowControlWPF> CmdTrigger => new Lazy<RelayCommand<HSmartWindowControlWPF>>(() => new RelayCommand<HSmartWindowControlWPF>(Trigger)).Value;
        private void Trigger(HSmartWindowControlWPF halcon)
        {
            // 首次运行关联 Halcon 控件
            if (!IsHalconReleted)
            {
                ho_Window = halcon.HalconWindow;
                IsHalconReleted = true;
            }
            HandyControl.Controls.Growl.Error("Trigger");
        }

        /// <summary>
        /// 实时模式
        /// </summary>
        public RelayCommand<HSmartWindowControlWPF> CmdRealTime => new Lazy<RelayCommand<HSmartWindowControlWPF>>(() => new RelayCommand<HSmartWindowControlWPF>(RealTime)).Value;
        private void RealTime(HSmartWindowControlWPF halcon)
        {
            // 首次运行关联 Halcon 控件
            if (!IsHalconReleted)
            {
                ho_Window = halcon.HalconWindow;
                IsHalconReleted = true;
            }
            HandyControl.Controls.Growl.Error("RealTime");
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
