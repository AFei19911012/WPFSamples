using System.Windows;
using Wpf_Base.MethodNet;

namespace Wpf_Base.CommunicationWpf
{
    /// <summary>
    /// WindowIpSetting.xaml 的交互逻辑
    /// </summary>
    public partial class WindowIP : Window
    {
        private EnumCommunicationType EnumType { get; set; }

        public WindowIP(EnumCommunicationType flag)
        {
            InitializeComponent();

            EnumType = flag;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TB_IP.Text = FileIOMethod.ReadIniFile(EnumType.ToString(), "IP", null, CFileNames.IniFileName);
            TB_Port.Text = FileIOMethod.ReadIniFile(EnumType.ToString(), "Port", null, CFileNames.IniFileName);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            FileIOMethod.WriteIniFile(EnumType.ToString(), "IP", TB_IP.Text, CFileNames.IniFileName);
            FileIOMethod.WriteIniFile(EnumType.ToString(), "Port", TB_Port.Text, CFileNames.IniFileName);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}