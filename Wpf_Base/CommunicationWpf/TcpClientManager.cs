using SimpleTCP;
using System;
using System.Text;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.CommunicationWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 11:55:14
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 11:55:14    CoderMan/CoderdMan1012         首次编写         
    ///
    public class TcpClientManager
    {
        public SimpleTcpClient SimTcpClient { get; set; }
        public char[] Delimiter { get; set; } = new char[1] { ';' };
        public string IP { get; set; }
        public int Port { get; set; }
        public EventHandler MessageRecievedEvent { get; set; }
        public string RecMessage { get; set; }
        public bool IsConnected { get; set; } = false;

        /// <summary>
        /// 自身的一个实例
        /// </summary>
        private static TcpClientManager instance = null;
        public static TcpClientManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TcpClientManager();
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

        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpClientManager()
        {
            SimTcpClient = new SimpleTcpClient
            {
                // 设置编码格式
                StringEncoder = Encoding.ASCII,
                // 设置分隔符，默认是0x13
                Delimiter = Encoding.ASCII.GetBytes(Delimiter)[0],
            };
            // 接收到数据时触发
            SimTcpClient.DataReceived += OnDataRecieved;
        }

        private void OnDataRecieved(object sender, Message msg)
        {
            RecMessage = msg.MessageString;
            // 触发消息处理事件
            MessageRecievedEvent?.Invoke(null, null);
        }

        public void Connect()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("TCP", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("TCP", "Port", null, CFileNames.IniFileName));
                Connect(ip, port);
            }
            catch (Exception ex)
            {
                IsConnected = false;
                PrintLog("TCP 客户端连接异常：" + ex.Message, EnumLogType.Error);
            }
        }

        public void Connect(string ip, int port)
        {
            try
            {
                IP = ip;
                Port = port;
                _ = SimTcpClient.Connect(ip, port);
                IsConnected = true;
                PrintLog("TCP 客户端连接成功", EnumLogType.Success);
            }
            catch (Exception ex)
            {
                IsConnected = false;
                PrintLog("TCP 客户端连接失败：" + ex.Message, EnumLogType.Error);
            }
        }

        public void ReConnect()
        {
            Disconnect();
            Connect();
        }

        public void ReConnect(string ip, int port)
        {
            Disconnect();
            Connect(ip, port);
        }

        public void Disconnect()
        {
            if (SimTcpClient.TcpClient != null && SimTcpClient.TcpClient.Connected)
            {
                _ = SimTcpClient.Disconnect();
                IsConnected = false;
                PrintLog("TCP 客户端断开连接", EnumLogType.Success);
            }
        }

        public void Write(string msg)
        {
            if (SimTcpClient.TcpClient != null && SimTcpClient.TcpClient.Connected)
            {
                SimTcpClient?.Write(msg);
            }
        }
    }
}