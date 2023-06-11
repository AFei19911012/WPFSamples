
using HalconDotNet;

using MvCamCtrl.NET;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf_Base.CcdWpf;
using Wpf_Base.LogWpf;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// HikCameraDemo.xaml 的交互逻辑
    /// </summary>
    public partial class HikCameraDemo : UserControl
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


        private HObject Ho_Image;
        private DateTime DT { get; set; } = DateTime.Now;

        public HikCameraDemo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 回调函数处理事件
        /// </summary>
        /// <param name="pEventInfo"></param>
        /// <param name="pUser"></param>
        private void EventCallbackFunc(ref MyCamera.MV_EVENT_OUT_INFO pEventInfo, IntPtr pUser)
        {
            HOperatorSet.GenEmptyObj(out Ho_Image);
            Ho_Image.Dispose();
            DateTime dt1 = DateTime.Now;
            CcdManager.Instance.GetHalconImage(CcdManager.Instance.CurrentCamId, ref Ho_Image);
            if (Ho_Image.IsInitialized())
            {
                if (CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].TriggerMode == EnumCaptureMode.Trig)
                {
                    PrintLog("回调函数抓图成功，耗时：" + (DateTime.Now - DT).TotalMilliseconds.ToString(), EnumLogType.Debug);
                }

                HalconWPF.HalconWindow.DispObj(Ho_Image);

                _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    HalconWPF.SetFullImagePart();
                }));
            }
        }

        private void CaptureImageTask()
        {
            while (CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].IsGrabbing)
            {
                if (Ho_Image == null)
                {
                    HOperatorSet.GenEmptyObj(out Ho_Image);
                }
                Ho_Image.Dispose();
                CcdManager.Instance.GetHalconImage(CcdManager.Instance.CurrentCamId, ref Ho_Image, CcdManager.Instance.CurrentCamId);
                if (Ho_Image.IsInitialized())
                {
                    if (CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].TriggerMode == EnumCaptureMode.Trig)
                    {
                        PrintLog("抓图成功，耗时：" + (DateTime.Now - DT).TotalMilliseconds.ToString(), EnumLogType.Debug);
                    }
                    HalconWPF.HalconWindow.DispObj(Ho_Image);

                    _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        HalconWPF.SetFullImagePart();
                    }));
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        /// <summary>
        /// 刷新相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCCD_Click(object sender, RoutedEventArgs e)
        {
            CcdManager.Instance.InitCCD();
            PrintLog("已刷新相机列表", EnumLogType.Debug);
        }

        /// <summary>
        /// 启动相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            if (CcdManager.Instance.NumberCCD > 0)
            {
                bool result = CcdManager.Instance.Open(CcdManager.Instance.CurrentCamId);
                if (result)
                {
                    PrintLog("相机已启动", EnumLogType.Success);
                }
            }
        }

        /// <summary>
        /// 委托事件赋值、注册委托事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDelegate_Click(object sender, RoutedEventArgs e)
        {
            // 给委托事件赋值
            if (CcdManager.Instance.NumberCCD > 0)
            {
                PrintLog("使用委托事件形式抓图", EnumLogType.Info);
                _ = CcdManager.Instance.Stop(CcdManager.Instance.CurrentCamId);
                _ = CcdManager.Instance.Start(CcdManager.Instance.CurrentCamId);

                // 用等号，只有一处委托事件有效
                CcdManager.Instance.ExposureEndEvents[CcdManager.Instance.CurrentCamId] = new MyCamera.cbEventdelegateEx(EventCallbackFunc);
                // 不同工具里使用注意重新注册事件
                bool result = CcdManager.Instance.SetCcdCallbackFunc(CcdManager.Instance.CurrentCamId);
                if (!result)
                {
                    PrintLog("相机注册委托事件失败", EnumLogType.Error);
                    return;
                }
            }
            PrintLog("相机已注册委托事件", EnumLogType.Success);
        }

        /// <summary>
        /// 取消委托事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancle_Click(object sender, RoutedEventArgs e)
        {
            if (CcdManager.Instance.NumberCCD > 0)
            {
                CcdManager.Instance.ExposureEndEvents[CcdManager.Instance.CurrentCamId] = null;
                bool result = CcdManager.Instance.SetCcdCallbackFunc(CcdManager.Instance.CurrentCamId);
                if (!result)
                {
                    PrintLog("相机取消注册委托事件失败", EnumLogType.Error);
                    return;
                }
            }
            PrintLog("相机已取消注册委托事件", EnumLogType.Success);
        }

        /// <summary>
        /// 采集模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMode_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Content.ToString();
            if (name.Contains("连续模式"))
            {
                if (CcdManager.Instance.NumberCCD > 0)
                {
                    _ = CcdManager.Instance.SetContinous(CcdManager.Instance.CurrentCamId);
                    PrintLog("相机设置为连续模式", EnumLogType.Debug);
                }
            }
            else
            {
                if (CcdManager.Instance.NumberCCD > 0)
                {
                    _ = CcdManager.Instance.SetTriggerSoftware(CcdManager.Instance.CurrentCamId);
                    PrintLog("相机设置为软触发模式", EnumLogType.Debug);
                }
            }
        }

        /// <summary>
        /// 开始抓图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (CcdManager.Instance.NumberCCD > 0 && CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].IsOpened)
            {
                if (!CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].IsGrabbing)
                {
                    _ = CcdManager.Instance.Start(CcdManager.Instance.CurrentCamId);
                    // 启动抓图任务
                    Task task = new Task(CaptureImageTask);
                    task.Start();
                    PrintLog("相机启动抓图", EnumLogType.Debug);
                }
            }
        }

        /// <summary>
        /// 停止抓图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            if (CcdManager.Instance.NumberCCD > 0)
            {
                _ = CcdManager.Instance.Stop(CcdManager.Instance.CurrentCamId);
                PrintLog("相机停止抓图", EnumLogType.Debug);
            }
        }

        /// <summary>
        /// 触发抓图指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTrig_Click(object sender, RoutedEventArgs e)
        {
            if (CcdManager.Instance.NumberCCD > 0 && CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].TriggerMode == EnumCaptureMode.Trig)
            {
                _ = CcdManager.Instance.TrigCamBySoft(CcdManager.Instance.CurrentCamId);
                DT = DateTime.Now;
                PrintLog("相机触发抓图指令", EnumLogType.Debug);
            }
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            if (CcdManager.Instance.NumberCCD > 0)
            {
                CcdManager.Instance.Close(CcdManager.Instance.CurrentCamId);
                PrintLog("相机已关闭", EnumLogType.Debug);
            }
        }
    }
}