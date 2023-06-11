using System.Diagnostics;
using System.IO;
using Encoder;
using System;
using System.Management;

namespace Wpf_Base.MethodNet
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/31 13:39:52
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/31 13:39:52    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class SysMethod
    {

        /// <summary>
        /// 禁止软件多次运行
        /// </summary>
        public static bool GetIsRunningOnce()
        {
            // 获取当前活动进程的模块名称
            string moduleName = Process.GetCurrentProcess().MainModule.ModuleName;
            // 返回指定路径字符串的文件名
            string processName = Path.GetFileNameWithoutExtension(moduleName);
            // 根据文件名创建进程资源数组
            Process[] processes = Process.GetProcessesByName(processName);
            // 如果该数组长度大于1，说明多次运行
            return processes.Length <= 1;
        }


        /// <summary>
        /// 系统默认方式打开文件
        /// </summary>
        /// <param name="filename"></param>
        public static void SystemOpenFile(string filename)
        {
            if (File.Exists(filename))
            {
                _ = Process.Start(filename);
            }
        }


        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="name"></param>
        public static void RunningExe(string folder, string name)
        {
            string path = folder + "\\" + name + ".exe";
            if (File.Exists(path))
            {
                _ = Process.Start(path);
            }
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="name"></param>
        public static void CloseExe(string name)
        {
            Process[] process = Process.GetProcessesByName(name);
            foreach (Process p in process)
            {
                p.Kill();
            }
        }


        /// <summary>
        /// 获取机器码
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCode()
        {
            string code = Register.f_GetMachineCode();
            return code;
        }

        /// <summary>
        /// 获取机器名称
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }

        /// <summary>
        /// 获取机器名称
        /// </summary>
        /// <returns></returns>
        public static string GetMachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// 获取 CPU 编号
        /// </summary>
        /// <returns></returns>
        public static string GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                string strCpuID = null;
                foreach (ManagementObject obj in moc)
                {
                    strCpuID = obj.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取硬盘编号
        /// </summary>
        /// <returns></returns>
        public static string GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string str = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    str = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取网卡序列号（MAC 地址）
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string str = "";
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    str = mo["MacAddress"].ToString();
                }
            }
            return str;
        }
    }
}