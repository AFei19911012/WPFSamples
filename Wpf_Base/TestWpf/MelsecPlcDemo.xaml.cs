using HslCommunication;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Wpf_Base.CommunicationWpf;
using Wpf_Base.LogWpf;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// MelsecPlcDemo.xaml 的交互逻辑
    /// </summary>
    public partial class MelsecPlcDemo : UserControl
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


        private Timer MyTimer;

        public MelsecPlcDemo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动定时器
        /// </summary>
        public void StartTimer()
        {
            MyTimer = new Timer
            {
                Interval = 5000, //单位毫秒
            };
            MyTimer.Elapsed += new ElapsedEventHandler(ThreadCheck);
            MyTimer.Start();
        }

        public void ThreadCheck(object sender, ElapsedEventArgs e)
        {
            if (McManager.Instance.MC == null)
            {
                PrintLog("MelsecPLC 连接失败", EnumLogType.Error);
                McManager.Instance.IsConnected = false;
            }
            else
            {
                // 设置长连接的操作，这样就不需要调用 connectserver 方法了
                McManager.Instance.MC.SetPersistentConnection();
                OperateResult<short> connect = McManager.Instance.MC.ReadInt16("D100");
                if (connect.IsSuccess)
                {
                    // 进行相关的操作，显示绿灯啥的
                    PrintLog("MelsecPLC 已连接", EnumLogType.Success);
                    McManager.Instance.IsConnected = true;
                }
                else
                {
                    // 进行相关的操作，显示红灯啥的
                    PrintLog("MelsecPLC 已断开", EnumLogType.Warning);
                    McManager.Instance.IsConnected = false;
                }
            }
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            // 主程序里连接一次 Modbus
            PrintLog("连接 MelsecPLC", EnumLogType.Debug);
            McManager.Instance.Connect();

            // 检查 MelsecPLC 是否处于连接状态
            StartTimer();
        }

        private void ButtonIsConnected_Click(object sender, RoutedEventArgs e)
        {
            if (McManager.Instance.IsConnected)
            {
                PrintLog(string.Format("MelsecPLC 已连接 {0} {1}", McManager.Instance.MC.IpAddress.ToString(), McManager.Instance.MC.Port), EnumLogType.Success);
            }
            else
            {
                PrintLog("MelsecPLC 未连接", EnumLogType.Error);
            }
        }

        private void ButtonReConnect_Click(object sender, RoutedEventArgs e)
        {
            McManager.Instance.ReConnect();
            PrintLog("MelsecPLC 重连", EnumLogType.Debug);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            // 断开连接
            McManager.Instance.Close();
            PrintLog("MelsecPLC 断开连接", EnumLogType.Debug);
            MyTimer?.Stop();
        }

        private void ButtonReadInt16_Click(object sender, RoutedEventArgs e)
        {
            int value100 = McManager.Instance.ReadInt16(100);
            PrintLog("MelsecPLC D100 = " + value100, EnumLogType.Debug);

            //// 规范读取
            //OperateResult<short> readD100 = McManager.Instance.MC.ReadInt16("D100");
            //// 判断是否读取成功
            //if (readD100.IsSuccess)
            //{
            //    short value = readD100.Content;
            //    PrintLog("读取成功 D100：" + value, EnumLogType.Success);
            //}
            //else
            //{
            //    short value = readD100.Content;
            //    PrintLog("读取失败 D100：" + value, EnumLogType.Error);
            //}
        }

        private void ButtonReadFloat_Click(object sender, RoutedEventArgs e)
        {
            double value101 = McManager.Instance.ReadFloat(101);
            PrintLog("MelsecPLC D101 = " + value101, EnumLogType.Debug);
        }

        private void ButtonWriteInt16_Click(object sender, RoutedEventArgs e)
        {
            bool result = McManager.Instance.Write(100, 1);
            if (result)
            {
                PrintLog("MelsecPLC D100 写入：1", EnumLogType.Debug);
            }
            else
            {
                PrintLog("MelsecPLC D100 写入失败", EnumLogType.Error);
            };
        }

        private void ButtonWriteFloat_Click(object sender, RoutedEventArgs e)
        {
            bool result = McManager.Instance.Write(101, 1.2f);
            if (result)
            {
                PrintLog("MelsecPLC D101 写入：1.2", EnumLogType.Debug);
            }
            else
            {
                PrintLog("MelsecPLC D101 写入失败", EnumLogType.Error);
            }
        }

        private void ButtonIP_Click(object sender, RoutedEventArgs e)
        {
            WindowIP window = new WindowIP(EnumCommunicationType.MC);
            _ = window.ShowDialog();
        }
    }
}
