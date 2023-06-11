using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.CommunicationWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 15:41:26
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 15:41:26    CoderMan/CoderdMan1012         首次编写         
    ///
    public class McManager
    {
        public MelsecMcNet MC { get; set; }
        public bool IsConnected { get; set; }
        private static McManager instance;
        public static McManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new McManager();
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

        public McManager()
        {
            IsConnected = false;
        }

        public void Connect()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("MC", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("MC", "Port", null, CFileNames.IniFileName));
                ConnectServer(ip, port);
            }
            catch (Exception ex)
            {
                PrintLog("MelsecPLC 连接异常：" + ex.Message, EnumLogType.Error);
            }
        }

        public void ConnectServer(string ip, int port)
        {
            if (MC == null)
            {
                IsConnected = false;
                // 指定 PLC 的 ip 地址和端口号
                MC = new MelsecMcNet(ip, port)
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
                    PrintLog("MelsecPLC 连接失败", EnumLogType.Error);
                }
                else
                {
                    IsConnected = true;
                    PrintLog("MelsecPLC 连接成功", EnumLogType.Success);
                }
            }
        }

        public void ReConnect()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("MC", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("MC", "Port", null, CFileNames.IniFileName));
                ReConnect(ip, port);
            }
            catch (Exception ex)
            {
                PrintLog("MelsecPLC 重连异常：" + ex.Message, EnumLogType.Error);
            }
        }

        public void ReConnect(string ip, int port)
        {
            if (MC != null)
            {
                _ = MC.ConnectClose();
                MC = null;
            }
            ConnectServer(ip, port);
        }

        public void Close()
        {
            if (MC != null)
            {
                _ = MC.ConnectClose();
                MC = null;
                PrintLog("MelsecPLC 已关闭", EnumLogType.Success);
            }
        }

        public int ReadInt16(int address)
        {
            return MC != null ? MC.ReadInt16(string.Format("D{0}", address)).Content : 0;
        }

        public double ReadFloat(int address)
        {
            return MC != null ? MC.ReadFloat(string.Format("D{0}", address)).Content : 0;
        }

        public bool Write(int address, float value)
        {
            if (MC == null)
            {
                return false;
            }
            OperateResult write = MC.Write(string.Format("D{0}", address), value);
            return write.IsSuccess;
        }

        public bool Write(int address, short value)
        {
            if (MC == null)
            {
                return false;
            }
            OperateResult write = MC.Write(string.Format("D{0}", address), value);
            return write.IsSuccess;
        }
    }
}