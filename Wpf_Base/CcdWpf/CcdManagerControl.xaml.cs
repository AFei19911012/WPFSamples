using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.CcdWpf
{
    /// <summary>
    /// CcdManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class CcdManagerControl : UserControl
    {
        private CcdManagerVM VM { get; set; }

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

        public CcdManagerControl()
        {
            InitializeComponent();

            VM = DataContext as CcdManagerVM;

            VM.LogEvent += PrintLog;

            MyCcdInfoControl.LogEvent += PrintLog;
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonInitCCD_Click(object sender, RoutedEventArgs e)
        {
            BTN_Connect.Content = "连接设备";
            VM.IconConnect = CCcdIcon.IconCcdConnectOn;
        }

        /// <summary>
        /// 连接、断开指定相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSwitch_Click(object sender, RoutedEventArgs e)
        {
            LB_CCD.SelectedIndex = ((sender as Button).DataContext as CHikCameraInfo).CcdOrder;
            int idx = LB_CCD.SelectedIndex;

            if (CcdManager.Instance.HikCamInfos[idx].IsOpened)
            {
                CcdManager.Instance.Close(idx);
                VM.ListCameraInfos[idx].CcdBrush = CCcdIcon.CcdBrushDisConnected;
                VM.ListCameraInfos[idx].CcdStatusIcon = CCcdIcon.IconCcdConnectedOff;
                PrintLog("断开相机：" + (idx + 1), EnumLogType.Info);
            }
            else
            {
                bool result = CcdManager.Instance.Open(idx);
                if (result)
                {
                    VM.ListCameraInfos[idx].CcdBrush = CCcdIcon.CcdBrushConnected;
                    VM.ListCameraInfos[idx].CcdStatusIcon = CCcdIcon.IconCcdConnected;
                    PrintLog("连接相机：" + (idx + 1), EnumLogType.Info);
                }
                else
                {
                    PrintLog("相机被占用：" + (idx + 1), EnumLogType.Warning);
                    return;
                }
            }
            // 是否可修改曝光时间和增益
            MyCcdInfoControl.SetEnabled(CcdManager.Instance.HikCamInfos[idx].IsOpened);
        }

        /// <summary>
        /// 连接、断开所有相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonConnectCCD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VM.ListCameraInfos.Count > 0)
                {
                    if (BTN_Connect.Content.ToString().Contains("连接设备"))
                    {
                        CameraOpen();
                    }
                    else
                    {
                        CameraClose();
                    }
                }
                else
                {
                    PrintLog("当前没有连接相机", EnumLogType.Warning);
                }
            }
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        /// <param name="isExpanded"></param>
        public void SetIsExpanded(bool isExpanded)
        {
            EP_CCD.IsExpanded = isExpanded;
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        public void CameraOpen()
        {
            try
            {
                bool result = CcdManager.Instance.Open();
                if (result)
                {
                    BTN_Connect.Content = "断开设备";
                    VM.IconConnect = CCcdIcon.IconCcdConnectOff;
                    // 开启时相机修改状态和颜色
                    for (int i = 0; i < VM.ListCameraInfos.Count; i++)
                    {
                        VM.ListCameraInfos[i].CcdStatusIcon = CCcdIcon.IconCcdConnected;
                        VM.ListCameraInfos[i].CcdBrush = CCcdIcon.CcdBrushConnected;
                    }
                    PrintLog("相机已全部连接", EnumLogType.Info);
                }
                else
                {
                    PrintLog("未设置相机IP或相机被占用，刷新设备后重新连接", EnumLogType.Warning);
                }
            }
            catch (Exception ex)
            {
                PrintLog("相机启动异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public void CameraClose()
        {
            try
            {
                CcdManager.Instance.Close();
                // 断开后相机修改状态和颜色
                BTN_Connect.Content = "连接设备";
                VM.IconConnect = CCcdIcon.IconCcdConnectOff;
                for (int i = 0; i < VM.ListCameraInfos.Count; i++)
                {
                    VM.ListCameraInfos[i].CcdStatusIcon = CCcdIcon.IconCcdConnectedOff;
                    VM.ListCameraInfos[i].CcdBrush = CCcdIcon.CcdBrushDisConnected;
                }
                PrintLog("相机已全部断开", EnumLogType.Info);
            }
            catch (Exception ex)
            {
                PrintLog("相机关闭异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 双击抓图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                int idx = LB_CCD.SelectedIndex;
                if (idx < 0)
                {
                    return;
                }
                if (!CcdManager.Instance.HikCamInfos[idx].IsOpened)
                {
                    bool result = CcdManager.Instance.Open(idx);
                    if (result)
                    {
                        VM.ListCameraInfos[idx].CcdBrush = CCcdIcon.CcdBrushConnected;
                        VM.ListCameraInfos[idx].CcdStatusIcon = CCcdIcon.IconCcdConnected;
                    }
                    else
                    {
                        PrintLog("未设置相机IP或相机被占用，刷新设备后重新连接", EnumLogType.Warning);
                        return;
                    }
                }
                PrintLog("相机启动连续抓图", EnumLogType.Debug);
                // 设置连续模式
                _ = CcdManager.Instance.SetContinous(idx);
                _ = CcdManager.Instance.Start(idx);
                // 图像显示窗口
                _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
                {
                    WindowHalcon window = new WindowHalcon(idx);
                    _ = window.ShowDialog();
                }));
            }
        }

        /// <summary>
        /// 显示指定相机信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LB_CCD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = LB_CCD.SelectedIndex;
            if (idx > -1)
            {
                LB_CCD.SelectedIndex = idx;
                // 设置参数
                VM.ListCameraInfos[idx].Exposure = CcdManager.Instance.HikCamInfos[idx].Exposure;
                VM.ListCameraInfos[idx].Gain = CcdManager.Instance.HikCamInfos[idx].Gain;
                VM.ListCameraInfos[idx].TriggerMode = CcdManager.Instance.HikCamInfos[idx].TriggerMode;
                MyCcdInfoControl.InitInfo(VM.ListCameraInfos[idx]);
                CcdManager.Instance.CurrentCamId = idx;
                // 是否可修改曝光时间、增益和触发模式
                MyCcdInfoControl.SetEnabled(CcdManager.Instance.HikCamInfos[idx].IsOpened);
            }
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            (((sender as Label).Parent as Grid).Parent as Border).BorderBrush = (Brush)FindResource("AccentBrush");
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            (((sender as Label).Parent as Grid).Parent as Border).BorderBrush = Brushes.Transparent;
        }
    }
}