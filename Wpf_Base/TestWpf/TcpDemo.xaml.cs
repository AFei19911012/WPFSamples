using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Wpf_Base.CommunicationWpf;
using Wpf_Base.LogWpf;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// CommunicationDemo.xaml 的交互逻辑
    /// </summary>
    public partial class TcpDemo : UserControl
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

        public TcpDemo()
        {
            InitializeComponent();

            // 注册消息事件，可以多个
            TcpClientManager.Instance.MessageRecievedEvent += (sender, e) => { TcpClientMessageRecievedEvent(); };

            // TCP 服务器
            //if (!TcpServerManager.Instance.IsListening)
            //{
            //    TcpServerManager.Instance.Start();
            //}
            // 注册消息事件，可以多个
            TcpServerManager.Instance.MessageRecievedEvent += (sender, e) => { TcpServerMessageRecievedEvent(); };
        }

        /// <summary>
        /// 数据接收
        /// </summary>
        private void TcpClientMessageRecievedEvent()
        {
            PrintLog("TCP 客户端收到消息：" + TcpClientManager.Instance.RecMessage, EnumLogType.Debug);
        }

        private void TcpServerMessageRecievedEvent()
        {
            PrintLog("TCP 服务器收到消息：" + TcpServerManager.Instance.RecMessage, EnumLogType.Debug);
        }

        private void ButtonClient_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Content.ToString();
            if (name.Contains("：连接"))
            {
                // 主程序连接一次
                // TCP 客户端
                Thread.Sleep(10);
                if (!TcpClientManager.Instance.IsConnected)
                {
                    TcpClientManager.Instance.Connect();
                }
            }
            else if (name.Contains("是否连接上"))
            {
                if (TcpClientManager.Instance.IsConnected)
                {
                    PrintLog(string.Format("TCP 已连接 {0} {1}", TcpClientManager.Instance.IP, TcpClientManager.Instance.Port), EnumLogType.Success);
                }
                else
                {
                    PrintLog("TCP 未连接", EnumLogType.Error);
                }
            }
            else if (name.Contains("重连"))
            {
                TcpClientManager.Instance.ReConnect();
                PrintLog("TCP 重连", EnumLogType.Debug);
            }
            else if (name.Contains("断开"))
            {
                TcpClientManager.Instance.Disconnect();
                PrintLog("TCP 断开", EnumLogType.Debug);
            }
            else if (name.Contains("发送消息"))
            {
                TcpClientManager.Instance.Write("Test: Write Message");
                PrintLog("TCP 发送消息：Test: Write Message", EnumLogType.Debug);
            }
        }

        private void ButtonServer_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Content.ToString();
            if (name.Contains("开启"))
            {
                if (!TcpServerManager.Instance.IsStarted)
                {
                    TcpServerManager.Instance.Start();
                }
                if (TcpServerManager.Instance.IsStarted)
                {
                    PrintLog(string.Format("TCP 服务器已开启 {0} {1}", TcpServerManager.Instance.IP, TcpServerManager.Instance.Port), EnumLogType.Success);
                }
                else
                {
                    PrintLog("TCP 服务器启动失败", EnumLogType.Warning);
                }
            }
            else if (name.Contains("重启"))
            {
                TcpServerManager.Instance.ReStart();
                if (TcpServerManager.Instance.IsStarted)
                {
                    PrintLog(string.Format("TCP 服务器已开启 {0} {1}", TcpServerManager.Instance.IP, TcpServerManager.Instance.Port), EnumLogType.Success);
                }
                else
                {
                    PrintLog("TCP 服务器启动失败", EnumLogType.Warning);
                }
            }
            else if (name.Contains("停止"))
            {
                TcpServerManager.Instance.Stop();
                if (!TcpServerManager.Instance.IsStarted)
                {
                    PrintLog("TCP 服务器已停止", EnumLogType.Debug);
                }
                else
                {
                    PrintLog("TCP 服务器停止失败", EnumLogType.Error);
                }
            }
            else if (name.Contains("发送消息"))
            {
                TcpServerManager.Instance.Broadcast("Test: Cast Message");
                PrintLog("TCP 发送消息：Test: Cast Message", EnumLogType.Debug);
            }
        }

        private void ButtonIP_Click(object sender, RoutedEventArgs e)
        {
            WindowIP window = new WindowIP(EnumCommunicationType.TCP);
            _ = window.ShowDialog();
        }
    }
}