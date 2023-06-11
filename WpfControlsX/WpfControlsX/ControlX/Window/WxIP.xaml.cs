using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using WpfControlsX.Helper;

namespace WpfControlsX.ControlX
{
    /// <summary>
    /// WxIP.xaml 的交互逻辑
    /// </summary>
    public partial class WxIP : Window
    {
        private string ConfigFileName { get; set; } = @"Config\Config.ini";

        public WxIP(int idx = 0)
        {
            InitializeComponent();

            CBB_Type.SelectedIndex = idx;
        }

        private void WxButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WxButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            bool flag = IPAddress.TryParse(TB_IP.Text, out _);
            if (!flag)
            {
                DialogHelper.Error("IP 设置有误");
                return;
            }

            FileIoHelper.WriteIniFile((CBB_Type.SelectedItem as ComboBoxItem).Content.ToString(), "IP", TB_IP.Text, ConfigFileName);
            FileIoHelper.WriteIniFile((CBB_Type.SelectedItem as ComboBoxItem).Content.ToString(), "Port", TB_Port.Text, ConfigFileName);
            Close();
        }

        private void CBB_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (File.Exists(ConfigFileName))
            {
                if (CBB_Type.SelectedIndex >= 0 && TB_IP != null)
                {
                    TB_IP.Text = FileIoHelper.ReadIniFile((CBB_Type.SelectedItem as ComboBoxItem).Content.ToString(), "IP", ConfigFileName);
                    TB_Port.Text = FileIoHelper.ReadIniFile((CBB_Type.SelectedItem as ComboBoxItem).Content.ToString(), "Port", ConfigFileName);
                }
            }
        }
    }
}