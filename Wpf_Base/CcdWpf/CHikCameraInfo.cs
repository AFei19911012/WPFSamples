using GalaSoft.MvvmLight;
using MvCamCtrl.NET;
using System.Windows.Media;


namespace Wpf_Base.CcdWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 19:31:12
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:31:12    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CHikCameraInfo : ViewModelBase
    {
        public EnumCameraType CameraType { get; set; } = EnumCameraType.Gige;
        public string UserName { get; set; } = "";
        public string ManufacturerName { get; set; } = "";
        public string ModelName { get; set; } = "";
        public string SerialNumber { get; set; } = "";
        public string GUID { get; set; } = "";
        public string Version { get; set; } = "";
        public string IP { get; set; } = "";

        public double Exposure { get; set; } = 10000;
        public double Gain { get; set; } = 0;
        /// <summary>
        /// 采集模式：连续、触发
        /// </summary>
        public EnumCaptureMode TriggerMode { get; set; } = EnumCaptureMode.Continous;
        public EnumCameraTriggerType TriggerType { get; set; } = EnumCameraTriggerType.Software;

        public bool IsOpened { get; set; } = false;
        public bool IsGrabbing { get; set; } = false;
        public MyCamera Camera { get; set; } = new MyCamera();


        public string CcdTypeIcon { get; set; }

        public string CcdName { get; set; }

        private string ccdStatusIcon;
        public string CcdStatusIcon
        {
            get => ccdStatusIcon;
            set => Set(ref ccdStatusIcon, value);
        }

        private Brush ccdBrush;
        public Brush CcdBrush
        {
            get => ccdBrush;
            set => Set(ref ccdBrush, value);
        }

        /// <summary>
        /// 相机默认搜索序号
        /// </summary>
        public int CcdOrder { get; set; }


        public CHikCameraInfo()
        {

        }

        public CHikCameraInfo(CHikCameraInfo info)
        {
            ModelName = info.ModelName;
            SerialNumber = info.SerialNumber;
            GUID = info.GUID;
            IP = info.IP;
            Version = info.Version;
            ManufacturerName = info.ManufacturerName;
        }
    }
}