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
    /// Created Time: 22/09/19 10:13:59
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/19 10:13:59    CoderMan/CoderdMan1012         首次编写         
    ///
    public class ModbusServerManager
    {
        public ModbusTcpServer MBS { get; set; }
        public bool IsStarted => MBS.IsStarted;
        public EventHandler MessageRecievedEvent { get; set; }
        public string RecMessage { get; set; } = "null";


        private static ModbusServerManager instance;
        public static ModbusServerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModbusServerManager();
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
        protected virtual void PringLog(string info, EnumLogType type)
        {
            LogEvent?.Invoke(info, type);
        }
        #endregion


        public ModbusServerManager()
        {
            // 创建
            MBS = new ModbusTcpServer
            {
                DataFormat = HslCommunication.Core.DataFormat.CDAB,
            };

            // 接收到数据时触发
            MBS.OnDataReceived += MBS_OnDataReceived;
        }

        private void MBS_OnDataReceived(object sender, byte[] data)
        {
            // 提取指令信息实现更复杂的功能
            RecMessage = HslCommunication.BasicFramework.SoftBasic.ByteToHexString(data);
            MessageRecievedEvent?.Invoke(null, null);
        }

        public void Start()
        {
            try
            {
                string ip = FileIOMethod.ReadIniFile("Modbus", "IP", null, CFileNames.IniFileName);
                int port = int.Parse(FileIOMethod.ReadIniFile("Modbus", "Port", null, CFileNames.IniFileName));
                Start(port);
            }
            catch (Exception ex)
            {
                PringLog("Modbus 服务器启动异常：" + ex.Message, EnumLogType.Error);
            }
        }

        public void Start(int port)
        {
            MBS.ServerStart(port);
            if (!IsStarted)
            {
                PringLog("ModBus 服务器启动失败", EnumLogType.Error);
            }
            else
            {
                PringLog("ModBus 服务器启动成功", EnumLogType.Success);
            }
        }

        public void ReStart()
        {
            Stop();
            Start();
        }

        public void ReStart(int port)
        {
            Stop();
            Start(port);
        }

        public void Stop()
        {
            if (IsStarted)
            {
                MBS.ServerClose();
                PringLog("ModBus 服务器已关闭", EnumLogType.Warning);
            }
        }

        /// <summary>
        /// 往寄存器写入单个值
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Write(int address, short value)
        {
            OperateResult write = MBS.Write(address.ToString(), value);
            return write.IsSuccess;
        }

        /// <summary>
        /// 往寄存器写入多个值
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool Write(int address, short[] values)
        {
            OperateResult write = MBS.Write(address.ToString(), values);
            return write.IsSuccess;
        }

        /// <summary>
        /// 读取单个值
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int ReadInt16(int address)
        {
            return MBS.ReadInt16(address.ToString()).Content;
        }

        /// <summary>
        /// 读取多个值
        /// </summary>
        /// <param name="address"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public short[] ReadInt16(int address, int count)
        {
            return MBS.ReadInt16(address.ToString(), (ushort)count).Content;
        }
    }
}