using HslCommunication;
using HslCommunication.ModBus;

namespace Demos.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2022 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2022/6/10 9:40:16
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/6/10 9:40:16    Taosy.W                 
    ///
    public class ModbusManager
    {
        public ModbusTcpNet MBS { get; set; }
        public bool IsConnected { get; set; }
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


        public ModbusManager()
        {
            IsConnected = false;
            Connect();
        }

        public void Connect(string ip = "127.0.0.1", int port = 502)
        {
            if (MBS == null)
            {
                // 连接
                MBS = new ModbusTcpNet(ip, port)
                {
                    // 浮点数是这个顺序
                    DataFormat = HslCommunication.Core.DataFormat.CDAB,
                };
                OperateResult result = MBS.ConnectServer();
                if (!result.IsSuccess)
                {
                    IsConnected = false;
                    Close();
                }
                else
                {
                    IsConnected = true;
                }
            }
        }

        public void ReConnect(string ip = "127.0.0.1", int port = 502)
        {
            Close();
            Connect(ip, port);
        }

        public void Close()
        {
            if (MBS != null)
            {
                _ = MBS.ConnectClose();
                MBS = null;
                IsConnected = false;
            }
        }

        public int ReadInt16(int address)
        {
            return MBS != null ? MBS.ReadInt16(address.ToString()).Content : 0;
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
    }
}