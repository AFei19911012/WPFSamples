using HslCommunication;
using HslCommunication.ModBus;
using System;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.CommunicationWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 15:35:01
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 15:35:01    CoderMan/CoderdMan1012         首次编写         
    ///
    public class ModbusManager
    {
        public ModbusTcpNet MBS { get; set; }

        public int HeartBeatAdress { get; set; } = 100;
        public bool IsConnected
        {
            get
            {
                if (MBS != null)
                {
                    OperateResult<short> connect = MBS.ReadInt16(HeartBeatAdress.ToString());
                    return connect.IsSuccess;
                }
                return false;
            }
        }

        private static ModbusManager instance;
        public static ModbusManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModbusManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// 连接多个服务器
        /// </summary>
        public static ModbusManager[] Instances { get; set; }


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


        public ModbusManager()
        {
            //IsConnected = false;
        }

        public void Connect()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("Modbus", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("Modbus", "Port", null, CFileNames.IniFileName));
                ConnectServer(ip, port);
            }
            catch (Exception ex)
            {
                PrintLog("Modbus 连接异常：" + ex.Message, EnumLogType.Error);
            }
        }

        public void ConnectServer(string ip, int port)
        {
            if (MBS == null)
            {
                //IsConnected = false;
                // 连接
                MBS = new ModbusTcpNet(ip, port)
                {
                    // 浮点数是这个顺序
                    DataFormat = HslCommunication.Core.DataFormat.CDAB,
                    ConnectTimeOut = 2000,
                    ReceiveTimeOut = 2000,
                };
                OperateResult result = MBS.ConnectServer();
                if (!result.IsSuccess)
                {
                    //IsConnected = false;
                    _ = MBS.ConnectClose();
                    MBS = null;
                    PrintLog("ModBus 连接失败", EnumLogType.Error);
                }
                else
                {
                    //IsConnected = true;
                    // 设置长连接的操作，这样就不需要调用 connectserver 方法了
                    MBS.SetPersistentConnection();
                    PrintLog("ModBus 连接成功", EnumLogType.Success);
                }
            }
        }

        public void ReConnect()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("Modbus", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("Modbus", "Port", null, CFileNames.IniFileName));
                ReConnect(ip, port);
            }
            catch (Exception ex)
            {
                PrintLog("ModBus 重连失败：" + ex.Message, EnumLogType.Error);
            }
        }

        public void ReConnect(string ip, int port)
        {
            if (MBS != null)
            {
                _ = MBS.ConnectClose();
                MBS = null;
            }
            ConnectServer(ip, port);
        }

        public void Close()
        {
            if (MBS != null)
            {
                _ = MBS.ConnectClose();
                //IsConnected = false;
                MBS = null;
                PrintLog("ModBus 已关闭", EnumLogType.Success);
            }
        }

        public int ReadInt16(int address)
        {
            return MBS != null ? MBS.ReadInt16(address.ToString()).Content : 0;
        }

        public short[] ReadInt16(int address, int count)
        {
            short[] values = new short[count];
            if (MBS != null)
            {
                values = MBS.ReadInt16(address.ToString(), (ushort)count).Content;
            }
            return values;
        }

        public double ReadFloat(int address)
        {
            return MBS != null ? MBS.ReadFloat(address.ToString()).Content : 0;
        }

        public bool Write(int address, float value)
        {
            if (MBS == null)
            {
                return false;
            }
            OperateResult write = MBS.Write(address.ToString(), value);
            return write.IsSuccess;
        }

        public bool Write(int address, short value)
        {
            if (MBS == null)
            {
                return false;
            }
            OperateResult write = MBS.Write(address.ToString(), value);
            return write.IsSuccess;
        }

        public bool Write(int address, short[] values)
        {
            OperateResult write = MBS.Write(address.ToString(), values);
            return write.IsSuccess;
        }
    }
}