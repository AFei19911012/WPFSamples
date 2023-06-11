using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/21 17:00:26
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/21 17:00:26                     BigWang         首次编写         
    ///
    public static class FileIoHelper
    {
        #region 时间 → 字符串（年月日时分秒毫秒） 时间 → 字符串（年月日）
        /// <summary>
        /// 时间 → 字符串（年月日时分秒毫秒）
        /// </summary>
        /// <returns></returns>
        public static string DateTimeNowToString()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        public static string DateTimeToString(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// 时间 → 字符串（年月日）
        /// </summary>
        /// <returns></returns>
        public static string DateTimeNowToFolder()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        public static string DateTimeNowToFolder(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }
        #endregion

        #region 读写 ini 文件
        /// <summary>
        /// 读取 ini 文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 写入 ini 文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// 读取 ini 文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ReadIniFile(string section, string key, string filename)
        {
            StringBuilder sb = new StringBuilder(1024);
            _ = GetPrivateProfileString(section, key, null, sb, 1024, filename);
            return sb.ToString();
        }

        /// <summary>
        /// 写入 ini 文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static void WriteIniFile(string section, string key, string val, string filename)
        {
            _ = WritePrivateProfileString(section, key, val, filename);
        }
        #endregion

        #region 读写 xml 文件
        /// <summary>
        /// 读取 xml 文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="nodes"></param>
        /// <param name="dicts"></param>
        public static void ReadXml(string filename, ref List<string> nodes, ref List<Dictionary<string, string>> dicts)
        {
            nodes.Clear();
            dicts.Clear();
            XDocument xDoc = XDocument.Load(filename);
            XElement root = xDoc.Root;
            // 遍历所有节点
            foreach (XElement element in xDoc.Descendants())
            {
                string node_name = element.Name.ToString();
                XElement node = root.Element(node_name);
                // 判断非 null
                if (node != null)
                {
                    // 根
                    nodes.Add(node_name);
                    // 属性
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    foreach (XAttribute item in root.Elements(node_name).Attributes())
                    {
                        string key = item.Name.ToString();
                        string value = item.Value.ToString();
                        dict.Add(key, value);
                    }
                    dicts.Add(dict);
                }
            }
        }

        /// <summary>
        /// 写入 xml 文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="rootName"></param>
        /// <param name="nodes"></param>
        /// <param name="dicts"></param>
        public static void WriteXml(string filename, string rootName, List<string> nodes, List<Dictionary<string, string>> dicts)
        {
            // 创建文档
            XDocument xDoc = new XDocument();
            // 根节点 只有一个
            XElement root = new XElement(rootName);
            // 添加根节点
            xDoc.Add(root);
            for (int i = 0; i < nodes.Count; i++)
            {
                // 节点
                XElement node = new XElement(nodes[i]);
                // 添加属性
                foreach (KeyValuePair<string, string> item in dicts[i])
                {
                    string key = item.Key;
                    string value = item.Value;
                    XAttribute att = new XAttribute(key, value);
                    node.Add(att);
                }
                // 添加节点
                root.Add(node);
            }
            // 保存
            xDoc.Save(filename);
        }
        #endregion
    }
}