using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wpf_Base.CcdWpf;
using Wpf_Base.CommunicationWpf;
using Wpf_Base.HalconWpf.Tools;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.HalconWpf.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 12:34:12
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 12:34:12    CoderMan/CoderdMan1012         首次编写         
    ///
    public class QrToolVM : ViewModelBase
    {
        #region 1. 绑定变量
        private string strRecipeName = "default";
        public string StrRecipeName
        {
            get => strRecipeName;
            set => Set(ref strRecipeName, value);
        }

        private string strTcpFormat = "功能码,0,返回状态,结果; (2,0,1,ab123;)";
        public string StrTcpFormat
        {
            get => strTcpFormat;
            set => Set(ref strTcpFormat, value);
        }

        private string strQRContent = "null";
        public string StrQRContent
        {
            get => strQRContent;
            set => Set(ref strQRContent, value);
        }

        private string strDecodeType = "QR Code";
        public string StrDecodeType
        {
            get => strDecodeType;
            set => Set(ref strDecodeType, value);
        }
        #endregion

        #region 2. 全局变量
        private HalconWindowVM MyHalconWindowVM { get; set; }
        #endregion


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


        public QrToolVM()
        {
            // 主程序连接一次
            // TCP 客户端
            Thread.Sleep(10);
            if (!TcpClientManager.Instance.IsConnected)
            {
                TcpClientManager.Instance.Connect();
            }
            // 注册消息事件，可以多个
            TcpClientManager.Instance.MessageRecievedEvent += (sender, e) => { MessageRecievedEvent(); };
        }

        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            MyHalconWindowVM = (e.Source as QrTool).MyHalconWindowControl.DataContext as HalconWindowVM;
        }

        /// <summary>
        /// 二维码识别
        /// </summary>
        public RelayCommand CmdRunQR => new Lazy<RelayCommand>(() => new RelayCommand(RunQR)).Value;
        private void RunQR()
        {
            try
            {
                if (MyHalconWindowVM.CanRefreshImage)
                {
                    GetCurrentImage();

                    if (!MyHalconWindowVM.Ho_Image.IsInitialized())
                    {
                        PrintLog("相机抓图失败", EnumLogType.Error);
                        return;
                    }
                    PrintLog("相机抓图完成", EnumLogType.Success);
                }

                if (!MyHalconWindowVM.Ho_Image.IsInitialized())
                {
                    PrintLog("图像无效", EnumLogType.Warning);
                    return;
                }

                PrintLog("启动二维码识别", EnumLogType.Debug);
                StrQRContent = "null";
                // 二维码识别             
                DoQrOrBarCode();
                // 显示结果
                MyHalconWindowVM.Ho_Window.SetDisplayFont(24);
                MyHalconWindowVM.Ho_Window.DispText(StrQRContent, 10, 10);
                PrintLog("二维码识别完成", EnumLogType.Success);
            }
            catch (Exception ex)
            {
                MyHalconWindowVM.CanRefreshImage = true;
                PrintLog("二维码识别异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 条码识别
        /// </summary>
        public RelayCommand CmdRunBarCode => new Lazy<RelayCommand>(() => new RelayCommand(RunBarCode)).Value;
        private void RunBarCode()
        {
            try
            {
                if (MyHalconWindowVM.CanRefreshImage)
                {
                    GetCurrentImage();

                    if (!MyHalconWindowVM.Ho_Image.IsInitialized())
                    {
                        PrintLog("相机抓图失败", EnumLogType.Error);
                        return;
                    }
                    PrintLog("相机抓图完成", EnumLogType.Success);
                }

                if (!MyHalconWindowVM.Ho_Image.IsInitialized())
                {
                    PrintLog("图像无效", EnumLogType.Warning);
                    return;
                }

                PrintLog("启动条码识别", EnumLogType.Debug);
                StrQRContent = "null";
                // 条码识别
                DoQrOrBarCode();
                // 显示结果
                MyHalconWindowVM.Ho_Window.SetDisplayFont(24);
                MyHalconWindowVM.Ho_Window.DispText(StrQRContent, 10, 10);
                PrintLog("条码识别完成", EnumLogType.Error);
            }
            catch (Exception ex)
            {
                MyHalconWindowVM.CanRefreshImage = true;
                PrintLog("条码识别异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 测试项目
        /// </summary>
        public RelayCommand CmdTestProject => new Lazy<RelayCommand>(() => new RelayCommand(TestProject)).Value;
        private void TestProject()
        {
            Task task = new Task(() =>
            {
                _ = DispatcherHelper.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { GetCodeString(); });
            });
            task.Start();
        }
        #endregion

        #region 4. 内部方法
        /// <summary>
        /// 获取当前图像
        /// </summary>
        private void GetCurrentImage()
        {
            // 禁止刷新图像
            MyHalconWindowVM.CanRefreshImage = false;
            PrintLog("启动相机抓图", EnumLogType.Debug);
            if (!CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].IsOpened)
            {
                _ = CcdManager.Instance.Open(CcdManager.Instance.CurrentCamId);
            }
            if (!CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].IsGrabbing)
            {
                _ = CcdManager.Instance.Start(CcdManager.Instance.CurrentCamId);
            }
            // 尝试多次拍照
            for (int i = 0; i < 5; i++)
            {
                MyHalconWindowVM.Ho_Image.Dispose();
                // 触发模式
                if (CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].TriggerMode == EnumCaptureMode.Trig)
                {
                    PrintLog("触发拍照指令", EnumLogType.Debug);
                    _ = CcdManager.Instance.TrigCamBySoft(CcdManager.Instance.CurrentCamId);
                }
                CcdManager.Instance.GetHalconImage(CcdManager.Instance.CurrentCamId, ref MyHalconWindowVM.Ho_Image);
                if (MyHalconWindowVM.Ho_Image.IsInitialized())
                {
                    break;
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        }

        /// <summary>
        /// 获取 QR 内容
        /// </summary>
        private void GetCodeString()
        {
            try
            {
                StrQRContent = "null";
                try
                {
                    GetCurrentImage();

                    if (!MyHalconWindowVM.Ho_Image.IsInitialized())
                    {
                        PrintLog("相机抓图失败", EnumLogType.Error);
                        return;
                    }
                    PrintLog(" 相机抓图完成", EnumLogType.Debug);
                }
                catch (Exception ex)
                {
                    PrintLog("相机抓图异常：" + ex.Message, EnumLogType.Error);
                    return;
                }

                // 解码
                DoQrOrBarCode();
                // 显示结果
                MyHalconWindowVM.Ho_Window.SetDisplayFont(24);
                MyHalconWindowVM.Ho_Window.DispText(StrQRContent, 10, 10);
                PrintLog("解码完成", EnumLogType.Success);

                if (StrQRContent == "null")
                {
                    PrintLog("解码结果为空", EnumLogType.Warning);
                }
                // 发送检测结果
                if (TcpClientManager.Instance.IsConnected)
                {
                    TcpClientManager.Instance.Write(string.Format("{0},{1},{2},{3};", 2, 0, 1, StrQRContent));
                    PrintLog("解码结果发送完成", EnumLogType.Success);
                }
            }
            catch (Exception ex)
            {
                PrintLog("解码异常：" + ex.Message, EnumLogType.Error);
            }
        }

        private void DoQrOrBarCode()
        {
            if (StrDecodeType == "Bar Code")
            {
                // 创建条码模型
                HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out HTuple hv_BarCodeHandle);
                // 寻找条码并解码
                HOperatorSet.FindBarCode(MyHalconWindowVM.Ho_Image, out HObject ho_SymbolRegions, hv_BarCodeHandle, "auto", out HTuple hv_DecodedDataString);
                // 清空句柄
                HOperatorSet.ClearBarCodeModel(hv_BarCodeHandle);
                // 显示结果
                MyHalconWindowVM.ClearWindow();
                MyHalconWindowVM.DispHImage();
                MyHalconWindowVM.Ho_Window.SetLineWidth(3);
                MyHalconWindowVM.SetDraw("margin");
                MyHalconWindowVM.Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                MyHalconWindowVM.DispHObject(ho_SymbolRegions);
                ho_SymbolRegions.Dispose();
                if (hv_DecodedDataString.Length > 0)
                {
                    StrQRContent = hv_DecodedDataString.S;
                }
            }
            else
            {
                // 创建二维码句柄
                HOperatorSet.CreateDataCode2dModel(StrDecodeType, new HTuple(), new HTuple(), out HTuple hv_DataCodeHandle);
                // 寻找二维码并解码
                HOperatorSet.FindDataCode2d(MyHalconWindowVM.Ho_Image, out HObject ho_SymbolXLDs, hv_DataCodeHandle, new HTuple(), new HTuple(), out _, out HTuple hv_DecodedDataString);
                // 清除句柄
                HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
                // 显示结果
                MyHalconWindowVM.ClearWindow();
                MyHalconWindowVM.DispHImage();
                MyHalconWindowVM.Ho_Window.SetLineWidth(3);
                MyHalconWindowVM.SetDraw("margin");
                MyHalconWindowVM.Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                MyHalconWindowVM.DispHObject(ho_SymbolXLDs);
                if (hv_DecodedDataString.Length > 0)
                {
                    StrQRContent = hv_DecodedDataString.S;
                }
            }

            if (MyHalconWindowVM.IsFirstShow)
            {
                MyHalconWindowVM.IsFirstShow = false;
                _ = DispatcherHelper.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { MyHalconWindowVM.Halcon.SetFullImagePart(); });
            }
        }

        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="msg"></param>
        private void MessageRecievedEvent()
        {
            if (MyHalconWindowVM.Ho_Window == null)
            {
                return;
            }
            string receiveMsg = TcpClientManager.Instance.RecMessage;
            if (receiveMsg.StartsWith("2,0"))
            {
                // 指定相机
                CcdManager.Instance.CurrentCamId = 0;
                TestProject();
            }
        }
        #endregion
    }
}