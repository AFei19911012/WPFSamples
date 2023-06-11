using SimpleTCP;
using System;
using System.Net;
using System.Text;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.CommunicationWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 15:26:54
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 15:26:54    CoderMan/CoderdMan1012         首次编写         
    ///
    public class TcpServerManager
    {
        public SimpleTcpServer SimTcpServer { get; set; }
        public bool IsStarted => SimTcpServer.IsStarted;
        public char[] Delimiter { get; set; } = new char[1] { ';' };
        public string IP { get; set; }
        public int Port { get; set; }
        public EventHandler ClientConnEvent { get; set; }
        public EventHandler MessageRecievedEvent { get; set; }
        public string RecMessage { get; set; } = "null";
        public int ClientCount => SimTcpServer.ConnectedClientsCount;

        /// <summary>
        /// 自身的一个实例
        /// </summary>
        private static TcpServerManager instance = null;
        public static TcpServerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TcpServerManager();
                }
                return instance;
            }
        }

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

        public TcpServerManager()
        {
            SimTcpServer = new SimpleTcpServer
            {
                // 设置编码格式
                StringEncoder = Encoding.ASCII,
                // 设置分隔符，默认是0x13
                Delimiter = Encoding.ASCII.GetBytes(Delimiter)[0],
            };
            // 接收到数据时触发
            SimTcpServer.DataReceived += OnDataRecieved;
            // 客户端连接时触发
            SimTcpServer.ClientConnected += (sender, msg) =>
            {
                RecMessage = "客户端连接";
                ClientConnEvent?.Invoke(null, null);
            };
            // 客户端断开时触发
            SimTcpServer.ClientDisconnected += (sender, msg) =>
            {
                RecMessage = "客户端断开";
                ClientConnEvent?.Invoke(null, null);
            };
        }

        private void OnDataRecieved(object sender, Message msg)
        {
            RecMessage = msg.MessageString;
            // 触发消息处理事件
            MessageRecievedEvent?.Invoke(null, null);
        }

        public void Start()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("TCP", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("TCP", "Port", null, CFileNames.IniFileName));
                Start(ip, port);
            }
            catch (Exception ex)
            {
                PrintLog("Tcp 服务器启动失败：" + ex.Message, EnumLogType.Error);
            }
        }

        public void Start(string ip, int port)
        {
            // 开始监听
            IP = ip;
            Port = port;
            _ = SimTcpServer.Start(IPAddress.Parse(IP), Port);
            if (!IsStarted)
            {
                PrintLog("Tcp 服务器启动失败", EnumLogType.Error);
            }
            else
            {
                PrintLog("Tcp 服务器启动成功", EnumLogType.Success);
            }
        }

        public void ReStart()
        {
            Stop();
            Start();
        }

        public void ReStart(string ip, int port)
        {
            Stop();
            Start(ip, port);
        }

        public void Stop()
        {
            if (IsStarted)
            {
                SimTcpServer.Stop();
                PrintLog("Tcp 服务器已停止", EnumLogType.Success);
            }
        }

        public void Broadcast(string msg)
        {
            SimTcpServer.Broadcast(msg);
        }
    }
}