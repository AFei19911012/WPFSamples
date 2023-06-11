using HslCommunication;

using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Wpf_Base.CommunicationWpf;
using Wpf_Base.LogWpf;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// ModbusDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ModbusDemo : UserControl
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

        public ModbusDemo()
        {
            InitializeComponent();

            // 注册消息事件
            ModbusServerManager.Instance.MessageRecievedEvent += (sender, e) => { ModbusServerMessageRecievedEvent(); };
        }

        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="msg"></param>
        private void ModbusServerMessageRecievedEvent()
        {
            string msg = ModbusServerManager.Instance.RecMessage;
            LogEvent("Modbus 服务器收到消息：" + msg, EnumLogType.Debug);
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
            if (ModbusManager.Instance.MBS == null)
            {
                PrintLog("Modbus 连接失败", EnumLogType.Error);
                //ModbusManager.Instance.IsConnected = false;
            }
            else
            {
                // 设置长连接的操作，这样就不需要调用 connectserver 方法了
                ModbusManager.Instance.MBS.SetPersistentConnection();
                OperateResult<short> connect = ModbusManager.Instance.MBS.ReadInt16("100");
                if (connect.IsSuccess)
                {
                    // 进行相关的操作，显示绿灯啥的
                    PrintLog("Modbus 已连接", EnumLogType.Success);
                    //ModbusManager.Instance.IsConnected = true;
                }
                else
                {
                    // 进行相关的操作，显示红灯啥的
                    PrintLog("Modbus 已断开", EnumLogType.Error);
                    //ModbusManager.Instance.IsConnected = false;
                }
            }
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            // 主程序里连接一次 Modbus
            PrintLog("连接 Modbus", EnumLogType.Debug);
            ModbusManager.Instance.Connect();

            // 检查程序是否处于连接状态
            StartTimer();
        }

        private void ButtonIsConnected_Click(object sender, RoutedEventArgs e)
        {
            if (ModbusManager.Instance.IsConnected)
            {
                PrintLog(string.Format("Modbus 已连接 {0} {1}", ModbusManager.Instance.MBS.IpAddress.ToString(), ModbusManager.Instance.MBS.Port), EnumLogType.Success);
            }
            else
            {
                PrintLog("Modbus 未连接", EnumLogType.Error);
            }
        }

        private void ButtonReConnect_Click(object sender, RoutedEventArgs e)
        {
            ModbusManager.Instance.ReConnect();
            PrintLog("Modbus 重连", EnumLogType.Debug);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            ModbusManager.Instance.Close();
            PrintLog("Modbus 断开连接", EnumLogType.Debug);
            MyTimer?.Stop();
        }

        private void ButtonReadInt16_Click(object sender, RoutedEventArgs e)
        {
            int value100 = ModbusManager.Instance.ReadInt16(100);
            PrintLog("Modbus 100 = " + value100, EnumLogType.Debug);
        }

        private void ButtonReadFloat_Click(object sender, RoutedEventArgs e)
        {
            double value101 = ModbusManager.Instance.ReadFloat(101);
            PrintLog("Modbus 101 = " + value101, EnumLogType.Debug);
        }

        private void ButtonWriteInt16_Click(object sender, RoutedEventArgs e)
        {
            bool result = ModbusManager.Instance.Write(100, 1);
            if (result)
            {
                PrintLog("Modbus 100 写入：1", EnumLogType.Debug);
            }
            else
            {
                PrintLog("Modbus 100 写入失败", EnumLogType.Error);
            };
        }

        private void ButtonWriteFloat_Click(object sender, RoutedEventArgs e)
        {
            bool result = ModbusManager.Instance.Write(101, 1.2f);
            if (result)
            {
                PrintLog("Modbus 101 写入：1.2", EnumLogType.Debug);
            }
            else
            {
                PrintLog("Modbus 101 写入失败", EnumLogType.Error);
            }
        }

        private void ButtonIP_Click(object sender, RoutedEventArgs e)
        {
            WindowIP window = new WindowIP(EnumCommunicationType.Modbus);
            _ = window.ShowDialog();
        }

        /////////////////////////////////////////////////// 服务器
        private void ButtonServer_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Content.ToString();
            if (name.Contains("开启"))
            {
                if (!ModbusServerManager.Instance.IsStarted)
                {
                    ModbusServerManager.Instance.Start();
                }
                if (ModbusServerManager.Instance.IsStarted)
                {
                    LogEvent("Modbus 服务器已开启", EnumLogType.Success);
                }
                else
                {
                    LogEvent("Modbus 服务器启动失败", EnumLogType.Warning);
                }
            }
            else if (name.Contains("重启"))
            {
                ModbusServerManager.Instance.ReStart();
                if (ModbusServerManager.Instance.IsStarted)
                {
                    LogEvent("Modbus 服务器已开启", EnumLogType.Success);
                }
                else
                {
                    LogEvent("Modbus 服务器启动失败", EnumLogType.Warning);
                }
            }
            else if (name.Contains("停止"))
            {
                ModbusServerManager.Instance.Stop();
                if (!ModbusServerManager.Instance.IsStarted)
                {
                    LogEvent("Modbus 服务器已停止", EnumLogType.Success);
                }
                else
                {
                    LogEvent("Modbus 服务器停止失败", EnumLogType.Error);
                }
            }
            else if (name.Contains("写 short 类型"))
            {
                _ = ModbusServerManager.Instance.Write(100, 100);

                _ = ModbusServerManager.Instance.Write(110, new short[] { 110, 111, 112 });

                LogEvent("Modbus 写入 100: 100", EnumLogType.Debug);
                LogEvent("Modbus 写入 110: 110", EnumLogType.Debug);
                LogEvent("Modbus 写入 111: 111", EnumLogType.Debug);
                LogEvent("Modbus 写入 112: 112", EnumLogType.Debug);
            }
            else if (name.Contains("读 short 类型"))
            {
                int value = ModbusServerManager.Instance.ReadInt16(100);
                short[] values = ModbusServerManager.Instance.ReadInt16(110, 3);
                LogEvent("Modbus 读取 100: " + value, EnumLogType.Debug);
                LogEvent("Modbus 读取 110: " + values[0], EnumLogType.Debug);
                LogEvent("Modbus 读取 111: " + values[1], EnumLogType.Debug);
                LogEvent("Modbus 读取 112: " + values[2], EnumLogType.Debug);
            }
        }
    }
}