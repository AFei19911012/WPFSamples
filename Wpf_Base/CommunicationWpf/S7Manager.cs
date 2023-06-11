using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.CommunicationWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 2022/12/20 10:50:04
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     2022/12/20 10:50:04    CoderMan/CoderdMan1012         首次编写         
    ///

    public class S7Manager
    {
        public SiemensS7Net S7 { get; set; }
        public bool IsConnected { get; set; }
        private static S7Manager instance;
        public static S7Manager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new S7Manager();
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

        public S7Manager()
        {
            IsConnected = false;
        }

        public void Connect()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("S7", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("S7", "Port", null, CFileNames.IniFileName));
                ConnectServer(ip, port);
            }
            catch (Exception ex)
            {
                PrintLog("S7 连接异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 端口号 102 无法修改
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void ConnectServer(string ip, int port)
        {
            if (S7 == null)
            {
                IsConnected = false;
                // 指定 PLC 的 ip 地址
                S7 = new SiemensS7Net(SiemensPLCS.S1500, ip)
                {
                    ConnectTimeOut = 1000,
                    ReceiveTimeOut = 1000,
                };
                // 连接
                OperateResult result = S7.ConnectServer();
                if (!result.IsSuccess)
                {
                    IsConnected = false;
                    _ = S7.ConnectClose();
                    S7 = null;
                    PrintLog("S7 连接失败", EnumLogType.Error);
                }
                else
                {
                    IsConnected = true;
                    PrintLog("S7 连接成功", EnumLogType.Success);
                }
            }
        }

        public void ReConnect()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("S7", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("S7", "Port", null, CFileNames.IniFileName));
                ReConnect(ip, port);
            }
            catch (Exception ex)
            {
                PrintLog("S7 重连异常：" + ex.Message, EnumLogType.Error);
            }
        }

        public void ReConnect(string ip, int port)
        {
            if (S7 != null)
            {
                _ = S7.ConnectClose();
                S7 = null;
            }
            ConnectServer(ip, port);
        }

        public void Close()
        {
            if (S7 != null)
            {
                _ = S7.ConnectClose();
                S7 = null;
                PrintLog("S7 已关闭", EnumLogType.Success);
            }
        }

        public int ReadInt16(string address)
        {
            return S7 != null ? S7.ReadInt16(address).Content : 0;
        }

        public double ReadFloat(string address)
        {
            return S7 != null ? S7.ReadFloat(address).Content : 0;
        }

        public double ReadDouble(string address)
        {
            return S7 != null ? S7.ReadDouble(address).Content : 0;
        }

        public bool ReadBool(string address)
        {
            return S7 != null && S7.ReadBool(address).Content;
        }

        public bool Write(string address, short value)
        {
            if (S7 == null)
            {
                return false;
            }
            OperateResult write = S7.Write(address, value);
            return write.IsSuccess;
        }

        public bool Write(string address, float value)
        {
            if (S7 == null)
            {
                return false;
            }
            OperateResult write = S7.Write(address, value);
            return write.IsSuccess;
        }

        public bool Write(string address, double value)
        {
            if (S7 == null)
            {
                return false;
            }
            OperateResult write = S7.Write(address, value);
            return write.IsSuccess;
        }

        public bool Write(string address, bool value)
        {
            if (S7 == null)
            {
                return false;
            }
            OperateResult write = S7.Write(address, value);
            return write.IsSuccess;
        }
    }
}