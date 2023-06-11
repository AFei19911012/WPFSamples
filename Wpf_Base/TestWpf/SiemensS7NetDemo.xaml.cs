using HslCommunication;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Wpf_Base.CommunicationWpf;
using Wpf_Base.LogWpf;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// SiemensS7NetDemo.xaml 的交互逻辑
    /// </summary>
    public partial class SiemensS7NetDemo : UserControl
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

        public SiemensS7NetDemo()
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
            if (S7Manager.Instance.S7 == null)
            {
                PrintLog("S7 连接失败", EnumLogType.Error);
                S7Manager.Instance.IsConnected = false;
            }
            else
            {
                // 设置长连接的操作，这样就不需要调用 connectserver 方法了
                S7Manager.Instance.S7.SetPersistentConnection();
                OperateResult<short> connect = S7Manager.Instance.S7.ReadInt16("M100");
                if (connect.IsSuccess)
                {
                    // 进行相关的操作，显示绿灯啥的
                    //PrintLog("S7 已连接", EnumLogType.Success);
                    S7Manager.Instance.IsConnected = true;
                }
                else
                {
                    // 进行相关的操作，显示红灯啥的
                    PrintLog("S7 已断开", EnumLogType.Warning);
                    S7Manager.Instance.IsConnected = false;
                }
            }
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            // 主程序里连接一次 Modbus
            PrintLog("连接 S7", EnumLogType.Debug);
            S7Manager.Instance.Connect();

            // 检查 S7 是否处于连接状态
            StartTimer();
        }

        private void ButtonIsConnected_Click(object sender, RoutedEventArgs e)
        {
            if (S7Manager.Instance.IsConnected)
            {
                PrintLog(string.Format("S7 已连接 {0} {1}", S7Manager.Instance.S7.IpAddress.ToString(), S7Manager.Instance.S7.Port), EnumLogType.Success);
            }
            else
            {
                PrintLog("S7 未连接", EnumLogType.Error);
            }
        }

        private void ButtonReConnect_Click(object sender, RoutedEventArgs e)
        {
            S7Manager.Instance.ReConnect();
            PrintLog("S7 重连", EnumLogType.Debug);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            // 断开连接
            S7Manager.Instance.Close();
            PrintLog("S7 断开连接", EnumLogType.Debug);
            MyTimer?.Stop();
        }

        private void ButtonIP_Click(object sender, RoutedEventArgs e)
        {
            WindowIP window = new WindowIP(EnumCommunicationType.S7);
            _ = window.ShowDialog();
        }

        private void ButtonWriteInt16_Click(object sender, RoutedEventArgs e)
        {
            bool result = S7Manager.Instance.Write("M100", 100);
            if (result)
            {
                PrintLog("S7 M100 写入：100", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 M100 写入失败", EnumLogType.Error);
            };

            result = S7Manager.Instance.Write("I100", 100);
            if (result)
            {
                PrintLog("S7 I100 写入：100", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 I101 写入失败", EnumLogType.Error);
            };

            result = S7Manager.Instance.Write("Q100", 100);
            if (result)
            {
                PrintLog("S7 Q100 写入：100", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 Q100 写入失败", EnumLogType.Error);
            };

            result = S7Manager.Instance.Write("DB100.1.0", 100);
            if (result)
            {
                PrintLog("S7 DB100.1.0 写入：100", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 DB100.1.0 写入失败", EnumLogType.Error);
            };
        }

        private void ButtonReadInt16_Click(object sender, RoutedEventArgs e)
        {
            int M100 = S7Manager.Instance.ReadInt16("M100");
            int I100 = S7Manager.Instance.ReadInt16("I100");
            int Q100 = S7Manager.Instance.ReadInt16("Q100");
            int DB100_1_0 = S7Manager.Instance.ReadInt16("DB100.1.0");
            PrintLog("S7 M100 = " + M100, EnumLogType.Debug);
            PrintLog("S7 I100 = " + I100, EnumLogType.Debug);
            PrintLog("S7 Q100 = " + Q100, EnumLogType.Debug);
            PrintLog("S7 DB100.1.0 = " + DB100_1_0, EnumLogType.Debug);

            //// 规范读取
            //OperateResult<short> readD100 = S7Manager.Instance.S7.ReadInt16("M100");
            //// 判断是否读取成功
            //if (readD100.IsSuccess)
            //{
            //    short value = readD100.Content;
            //    PrintLog("读取成功 M100：" + value, EnumLogType.Success);
            //}
            //else
            //{
            //    short value = readD100.Content;
            //    PrintLog("读取失败 M100：" + value, EnumLogType.Error);
            //}
        }

        private void ButtonWriteFloat_Click(object sender, RoutedEventArgs e)
        {
            bool result = S7Manager.Instance.Write("M100", 100.1f);
            if (result)
            {
                PrintLog("S7 M100 写入：100.1", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 M100 写入失败", EnumLogType.Error);
            };

            result = S7Manager.Instance.Write("I100", 100.1f);
            if (result)
            {
                PrintLog("S7 I100 写入：100.1", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 I100 写入失败", EnumLogType.Error);
            };

            result = S7Manager.Instance.Write("Q100", 100.1f);
            if (result)
            {
                PrintLog("S7 Q100 写入：100.1", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 Q100 写入失败", EnumLogType.Error);
            };

            result = S7Manager.Instance.Write("DB100.1.0", 100.1f);
            if (result)
            {
                PrintLog("S7 DB100.1.0 写入：100.1", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 DB100.1.0 写入失败", EnumLogType.Error);
            };
        }

        private void ButtonReadFloat_Click(object sender, RoutedEventArgs e)
        {
            double M100 = S7Manager.Instance.ReadFloat("M100");
            double I100 = S7Manager.Instance.ReadFloat("I100");
            double Q100 = S7Manager.Instance.ReadFloat("Q100");
            double DB100_1_0 = S7Manager.Instance.ReadFloat("DB100.1.0");
            PrintLog("S7 M100 = " + M100, EnumLogType.Debug);
            PrintLog("S7 I100 = " + I100, EnumLogType.Debug);
            PrintLog("S7 Q100 = " + Q100, EnumLogType.Debug);
            PrintLog("S7 DB100.1.0 = " + DB100_1_0, EnumLogType.Debug);
        }   

        private void ButtonWriteDouble_Click(object sender, RoutedEventArgs e)
        {
            bool result = S7Manager.Instance.Write("DB100.1.0", 100.1);
            if (result)
            {
                PrintLog("S7 DB100.1.0 写入：100.1", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 DB100.1.0 写入失败", EnumLogType.Error);
            };
        }

        private void ButtonReadDouble_Click(object sender, RoutedEventArgs e)
        {
            double DB100_1_0 = S7Manager.Instance.ReadDouble("DB100.1.0");
            PrintLog("S7 DB100.1.0 = " + DB100_1_0, EnumLogType.Debug);
        }

        private void ButtonWriteBool_Click(object sender, RoutedEventArgs e)
        {
            bool result = S7Manager.Instance.Write("DB100.1.0", true);
            if (result)
            {
                PrintLog("S7 DB100.1.0 写入：true", EnumLogType.Debug);
            }
            else
            {
                PrintLog("S7 DB100.1.0 写入失败", EnumLogType.Error);
            };
        }

        private void ButtonReadBool_Click(object sender, RoutedEventArgs e)
        {
            bool DB100_1_0 = S7Manager.Instance.ReadBool("DB100.1.0");
            PrintLog("S7 DB100.1.0 = " + DB100_1_0, EnumLogType.Debug);
        }
    }
}