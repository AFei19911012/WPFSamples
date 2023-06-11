using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Wpf_Base.LogWpf;

namespace Wpf_Base.CcdWpf
{
    /// <summary>
    /// CcdInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class CcdInfoControl : UserControl
    {
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

        public CcdInfoControl()
        {
            InitializeComponent();
        }

        public void InitInfo(CHikCameraInfo info)
        {
            _ = Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                TB_ModelName.Text = info.ModelName;
                TB_SerialNumber.Text = info.SerialNumber;
                TB_GUID.Text = info.GUID;
                TB_IP.Text = info.IP;
                TB_Version.Text = info.Version;
                TB_ManufactureName.Text = info.ManufacturerName;
                NUD_Exposure.Value = info.Exposure;
                NUD_Gain.Value = info.Gain;
                CBB_TriggerMode.SelectedIndex = info.TriggerMode == EnumCaptureMode.Continous ? 0 : 1;
                SetTriggerMode();
            });
        }

        private void NumericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetExposureGain();
            }
        }

        private void NUD_Exposure_LostFocus(object sender, RoutedEventArgs e)
        {
            SetExposureGain();
        }

        private void SetExposureGain()
        {
            try
            {
                double exposure = NUD_Exposure.Value;
                double gain = NUD_Gain.Value;
                int camId = CcdManager.Instance.CurrentCamId;
                if (camId >= 0 && CcdManager.Instance.HikCamInfos[camId].IsOpened)
                {
                    if (Math.Abs(CcdManager.Instance.HikCamInfos[camId].Exposure - exposure) > 0 || Math.Abs(CcdManager.Instance.HikCamInfos[camId].Gain - gain) > 0.1)
                    {
                        // 设置曝光时间和增益
                        CcdManager.Instance.HikCamInfos[camId].Exposure = exposure;
                        CcdManager.Instance.HikCamInfos[camId].Gain = gain;
                        _ = CcdManager.Instance.SetExposureAndGain(camId);
                        PrintLog("曝光时间、增益设置完成", EnumLogType.Success);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintLog("曝光时间、增益设置异常：" + ex.Message, EnumLogType.Error);
            }
        }

        public void SetEnabled(bool flag = true)
        {
            SP_ExposureGain.IsEnabled = flag;
        }

        private void SetTriggerMode()
        {
            int camId = CcdManager.Instance.CurrentCamId;
            if (camId >= 0 && CcdManager.Instance.HikCamInfos[camId].IsOpened)
            {
                //_ = CBB_TriggerMode.SelectedIndex == 0 ? CcdManager.Instance.SetContinous(camId) : CcdManager.Instance.SetTriggerSoftware(camId);
                if (CBB_TriggerMode.SelectedIndex == 0)
                {
                    if (CcdManager.Instance.HikCamInfos[camId].TriggerMode == EnumCaptureMode.Trig)
                    {
                        _ = CcdManager.Instance.SetContinous(camId);
                        PrintLog("触发模式已关闭", EnumLogType.Info);
                    }
                }
                else
                {
                    if (CcdManager.Instance.HikCamInfos[camId].TriggerMode == EnumCaptureMode.Continous)
                    {
                        _ = CcdManager.Instance.SetTriggerSoftware(camId);
                        PrintLog("触发模式已开启", EnumLogType.Info);
                    }
                }
            }
        }

        private void CBB_TriggerMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = CBB_TriggerMode.SelectedIndex;
            if (idx < 0)
            {
                return;
            }
            SetTriggerMode();
        }
    }
}