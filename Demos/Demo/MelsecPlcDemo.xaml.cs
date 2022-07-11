using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Demos.Demo
{
    /// <summary>
    /// MelsecPlcDemo.xaml 的交互逻辑
    /// </summary>
    public partial class MelsecPlcDemo : UserControl
    {
        private Timer MyTimer { get; set; }
        private MelsecMcNet MC { get; set; }
        private bool IsConnected { get; set; }

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
            if (MC != null)
            {
                // 设置长连接的操作，这样就不需要调用 connectserver 方法了
                MC.SetPersistentConnection();
                OperateResult<short> connect = MC.ReadInt16("D100");
                if (connect.IsSuccess)
                {
                    // 进行相关的操作，显示绿灯啥的
                    IsConnected = true;
                }
                else
                {
                    // 进行相关的操作，显示红灯啥的
                    IsConnected = false;
                }
            }
        }

        private void ConnectServer(string ip, int port)
        {
            if (MC == null)
            {
                IsConnected = false;
                // 指定 PLC 的 ip 地址和端口号
                MC = new MelsecMcNet("127.0.0.1", 6000)
                {
                    ConnectTimeOut = 1000,
                    ReceiveTimeOut = 1000,
                };
                // 连接
                OperateResult result = MC.ConnectServer();
                if (!result.IsSuccess)
                {
                    IsConnected = false;
                    _ = MC.ConnectClose();
                    MC = null;
                    _ = MessageBox.Show("MelsecPLC 连接失败");
                }
                else
                {
                    IsConnected = true;
                    _ = MessageBox.Show("MelsecPLC 连接成功");
                }
            }
        }

        private void ReConnect(string ip, int port)
        {
            if (MC != null)
            {
                _ = MC.ConnectClose();
                MC = null;
            }
            ConnectServer(ip, port);
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            // 主程序里连接一次 Modbus
            ConnectServer("127.0.0.1", 6000);

            // 检查 MelsecPLC 是否处于连接状态
            StartTimer();
        }

        private void ButtonIsConnected_Click(object sender, RoutedEventArgs e)
        {
            _ = IsConnected
                ? MessageBox.Show(string.Format("MelsecPLC 已连接 {0} {1}", MC.IpAddress.ToString(), MC.Port))
                : MessageBox.Show("MelsecPLC 未连接");
        }

        private void ButtonReConnect_Click(object sender, RoutedEventArgs e)
        {
            ReConnect("127.0.0.1", 6000);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            // 断开连接
            if (MC != null)
            {
                _ = MC.ConnectClose();
                MC = null;
            }
            MyTimer?.Stop();
        }

        private void ButtonReadInt16_Click(object sender, RoutedEventArgs e)
        {
            int value100 = MC.ReadInt16(string.Format("D{0}", 100)).Content;
            _ = MessageBox.Show("MelsecPLC D100 = " + value100);

            //// 规范读取
            //OperateResult<short> readD100 = MC.ReadInt16("D100");
            //// 判断是否读取成功
            //if (readD100.IsSuccess)
            //{
            //    short value = readD100.Content;
            //}
            //else
            //{
            //    short value = readD100.Content;
            //}
        }

        private void ButtonReadFloat_Click(object sender, RoutedEventArgs e)
        {
            double value101 = MC.ReadFloat(string.Format("D{0}", 101)).Content;
            _ = MessageBox.Show("MelsecPLC D101 = " + value101);
        }

        private void ButtonWriteInt16_Click(object sender, RoutedEventArgs e)
        {
            OperateResult write = MC.Write(string.Format("D{0}", 100), 1);
            _ = write.IsSuccess ? MessageBox.Show("MelsecPLC D100 写入：1") : MessageBox.Show("MelsecPLC D100 写入失败"); ;
        }

        private void ButtonWriteFloat_Click(object sender, RoutedEventArgs e)
        {
            OperateResult write = MC.Write(string.Format("D{0}", 101), 1.2f);
            _ = write.IsSuccess ? MessageBox.Show("MelsecPLC D101 写入：1.2") : MessageBox.Show("MelsecPLC D101 写入失败");
        }
    }
}
