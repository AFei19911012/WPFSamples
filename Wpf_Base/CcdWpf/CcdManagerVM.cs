using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf_Base.LogWpf;

namespace Wpf_Base.CcdWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 20:05:08
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 20:05:08    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CcdManagerVM : ViewModelBase
    {
        private ObservableCollection<CHikCameraInfo> listCameraInfos = new ObservableCollection<CHikCameraInfo>();
        public ObservableCollection<CHikCameraInfo> ListCameraInfos
        {
            get => listCameraInfos;
            set => Set(ref listCameraInfos, value);
        }

        private string _IconConnect = CCcdIcon.IconCcdConnectOn;
        public string IconConnect
        {
            get => _IconConnect;
            set => Set(ref _IconConnect, value);
        }



        #region 委托和事件 打印日志消息
        // 声明一个委托
        public delegate void LogEventHandler(string info, EnumLogType type);
        // 声明一个事件
        public event LogEventHandler LogEvent;
        // 触发事件
        protected virtual void PrintLog(string info, EnumLogType type)
        {
            LogEvent?.Invoke(info, type);
        }
        #endregion


        public CcdManagerVM()
        {
            GetCCD();
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        private void GetCCD()
        {
            ListCameraInfos.Clear();
            for (int i = 0; i < CcdManager.Instance.NumberCCD; i++)
            {
                ListCameraInfos.Add(new CHikCameraInfo(CcdManager.Instance.HikCamInfos[i])
                {
                    CcdTypeIcon = CcdManager.Instance.HikCamInfos[i].CameraType == EnumCameraType.Gige ? CCcdIcon.IconGige : CCcdIcon.IconUsb,
                    CcdName = CcdManager.Instance.HikCamInfos[i].UserName,
                    CcdStatusIcon = CCcdIcon.IconCcdConnectedOff,
                    CcdBrush = CCcdIcon.CcdBrushDisConnected,
                    CcdOrder = CcdManager.Instance.HikCamInfos[i].CcdOrder,
                });
            }
            if (CcdManager.Instance.NumberCCD > 0)
            {
                CcdManager.Instance.CurrentCamId = 0;
            }
        }

        /// <summary>
        /// 重置相机
        /// </summary>
        public RelayCommand CmdInitCCD => new Lazy<RelayCommand>(() => new RelayCommand(InitCCD)).Value;
        private void InitCCD()
        {
            try
            {
                // 先关闭
                CcdManager.Instance.Close();
                // 再刷新
                CcdManager.Instance.InitCCD();
                PrintLog("初始化相机", EnumLogType.Debug);
                GetCCD();
                PrintLog("扫描到相机数量为：" + ListCameraInfos.Count, EnumLogType.Debug);
            }
            catch (Exception ex)
            {
                PrintLog("相机扫描异常：" + ex.Message, EnumLogType.Error);
            }
        }
    }
}