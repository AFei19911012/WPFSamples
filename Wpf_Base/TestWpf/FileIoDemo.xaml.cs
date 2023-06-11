
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Wpf_Base.MethodNet;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// FileIoDemo.xaml 的交互逻辑
    /// </summary>
    public partial class FileIoDemo : UserControl
    {
        private string IniFileName { get; set; } = @"Data\test.ini";
        private string XmlFileName { get; set; } = @"Data\test.xml";

        public FileIoDemo()
        {
            InitializeComponent();
        }

        private void ButtonWriteIni_Click(object sender, RoutedEventArgs e)
        {
            FileIOMethod.WriteIniFile("Section1", "key1", "1", IniFileName);
            FileIOMethod.WriteIniFile("Section1", "key2", "2", IniFileName);
            FileIOMethod.WriteIniFile("Section2", "key1", "1", IniFileName);
            FileIOMethod.WriteIniFile("Section2", "key2", "2", IniFileName);
        }

        private void ButtonReadIni_Click(object sender, RoutedEventArgs e)
        {
            LB_Info.Items.Clear();
            LB_Info.Items.Add("[Section1]");
            string value = FileIOMethod.ReadIniFile("Section1", "key1", null, IniFileName);
            LB_Info.Items.Add("key1=" + value);
            value = FileIOMethod.ReadIniFile("Section1", "key2", null, IniFileName);
            LB_Info.Items.Add("key2=" + value);

            LB_Info.Items.Add("[Section2]");
            value = FileIOMethod.ReadIniFile("Section2", "key1", null, IniFileName);
            LB_Info.Items.Add("key1=" + value);
            value = FileIOMethod.ReadIniFile("Section2", "key2", null, IniFileName);
            LB_Info.Items.Add("key2=" + value);
        }

        private void ButtonWriteXml_Click(object sender, RoutedEventArgs e)
        {
            List<string> nodes = new List<string> { "nodel", "node2", };

            List<Dictionary<string, string>> dicts = new List<Dictionary<string, string>>();
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };
            dicts.Add(dict);

            dict = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };
            dicts.Add(dict);
            FileIOMethod.WriteXml(XmlFileName, "Root", nodes, dicts);
        }

        private void ButtonReadXml_Click(object sender, RoutedEventArgs e)
        {
            List<string> nodes = new List<string>();
            List<Dictionary<string, string>> dicts = new List<Dictionary<string, string>>();
            FileIOMethod.ReadXml(XmlFileName, ref nodes, ref dicts);
            LB_Info.Items.Clear();
            string str = nodes[0];
            foreach (KeyValuePair<string, string> item in dicts[0])
            {
                str = str + "  " + item.Key + "=" + item.Value;
            }
            LB_Info.Items.Add(str);

            str = nodes[1];
            foreach (KeyValuePair<string, string> item in dicts[1])
            {
                str = str + "  " + item.Key + "=" + item.Value;
            }
            _ = LB_Info.Items.Add(str);
        }

        private void ButtonReadXls_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"Data\test.xls";
            List<string> sheet_names = FileIOMethod.GetSheetNames(filename);
            List<List<string>> excel_content = FileIOMethod.ReadSheet(filename, sheet_names[0]);
            LB_Info.Items.Clear();
            for (int i = 0; i < excel_content.Count; i++)
            {
                string info = "";
                for (int j = 0; j < excel_content[i].Count; j++)
                {
                    info += excel_content[i][j];
                }
                _ = LB_Info.Items.Add(info);
            }
        }

        private void ButtonReadXlsx_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"Data\test.xlsx";
            List<string> sheet_names = FileIOMethod.GetSheetNames(filename);
            List<List<string>> excel_content = FileIOMethod.ReadSheet(filename, sheet_names[0]);
            LB_Info.Items.Clear();
            for (int i = 0; i < excel_content.Count; i++)
            {
                string info = "";
                for (int j = 0; j < excel_content[i].Count; j++)
                {
                    info += excel_content[i][j];
                }
                _ = LB_Info.Items.Add(info);
            }
        }
    }
}