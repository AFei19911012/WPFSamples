using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using HslCommunication;
using HslCommunication.ModBus;
using SimpleTCP;

namespace Demos.Demo
{
    /// <summary>
    /// ModBusTcpDemo.xaml 的交互逻辑
    /// </summary>
    public partial class TcpDemo : UserControl
    {
        private ModbusTcpNet MBS { get; set; }

        private SimpleTcpClient SimTcpClient { get; set; }
        private char[] Delimiter { get; set; } = new char[1] { ',' };

        private SimpleTcpServer SimTcpServer { get; set; }

        public TcpDemo()
        {
            InitializeComponent();

            SimTcpClient = new SimpleTcpClient
            {
                // 设置编码格式
                StringEncoder = Encoding.ASCII,
                // 设置分隔符，默认是0x13
                Delimiter = Encoding.ASCII.GetBytes(Delimiter)[0]
            };
            // 接收到数据触发的事件
            SimTcpClient.DataReceived += (mysender, msg) => { TcpDataRecieved(msg.MessageString); };

            SimTcpServer = new SimpleTcpServer
            {
                // 设置编码格式
                StringEncoder = Encoding.ASCII,
                // 设置分隔符，默认是0x13
                Delimiter = Encoding.ASCII.GetBytes(Delimiter)[0]
            };
            SimTcpServer.DataReceived += (mysender, msg) => { TcpDataRecieved(msg.MessageString); };
        }

        private void BtnModBus_Click(object sender, RoutedEventArgs e)
        {
            if (MBS == null)
            {
                // 连接
                MBS = new ModbusTcpNet("127.0.0.1");
                OperateResult result = MBS.ConnectServer();
                // 浮点数是这个顺序
                MBS.DataFormat = HslCommunication.Core.DataFormat.CDAB;
                if (!result.IsSuccess)
                {
                    MBS.ConnectClose();
                    MBS = null;
                    return;
                }
            }
            // 读取 100 的值
            TB_value100.Text = MBS.ReadInt16("100").Content.ToString();

            // 写入到 101 和 102
            MBS.Write("101", (short)101);
            MBS.Write("102", (float)102.1);

            TB_value101.Text = MBS.ReadInt16("101").Content.ToString();
            TB_value102.Text = MBS.ReadFloat("102").Content.ToString();
        }


        #region SimpleTCP 客户端
        private void BtnSimTcpClientConnect_Click(object sender, RoutedEventArgs e)
        {
            SimTcpClient.Connect("192.168.1.1", 502);
        }

        private void BtnSimTcpClientWrite_Click(object sender, RoutedEventArgs e)
        {
            SimTcpClient.Write("SetUVar,B,1,0,100;");
            Thread.Sleep(100);
            SimTcpClient.Write("SetUVar,B,1,1,200;");
        }

        private void BtnSimTcpClientClose_Click(object sender, RoutedEventArgs e)
        {
            SimTcpClient.Disconnect();
        }
        #endregion

        #region SimpleTCP 服务器
        private void BtnSimTcpServerStart_Click(object sender, RoutedEventArgs e)
        {
            SimTcpServer.Start(IPAddress.Parse("192.168.1.10"), 502);
        }

        private void BtnSimTcpServerSend_Click(object sender, RoutedEventArgs e)
        {
            SimTcpServer.Broadcast("SetUVar,B,1,0,100;");
        }

        private void BtnSimTcpServerClose_Click(object sender, RoutedEventArgs e)
        {
            SimTcpServer.Stop();
        }
        #endregion

        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="msg"></param>
        private void TcpDataRecieved(string receiveMsg)
        {
            if (receiveMsg == "TCP_Server_OK\n" || receiveMsg == "TCP_Client_OK\n")
            {
                MessageBox.Show("连接成功");
            }
            else if (receiveMsg == "0000\n")
            {
                MessageBox.Show("写入成功");
            }
            else
            {
                MessageBox.Show("接收成功：" + receiveMsg);
            }
        }
    }
}