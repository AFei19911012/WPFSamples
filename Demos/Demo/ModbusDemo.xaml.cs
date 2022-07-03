using Demos.Method;
using System.Windows;
using System.Windows.Controls;

namespace Demos.Demo
{
    /// <summary>
    /// ModbusDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ModbusDemo : UserControl
    {
        public ModbusDemo()
        {
            InitializeComponent();
        }

        private void ButtonIsConnected_Click(object sender, RoutedEventArgs e)
        {
            _ = ModbusManager.Instance.IsConnected
                ? MessageBox.Show(string.Format("Modbus 已连接 {0} {1}", ModbusManager.Instance.MBS.IpAddress.ToString(), ModbusManager.Instance.MBS.Port))
                : MessageBox.Show("Modbus 未连接");
        }

        private void ButtonReConnect_Click(object sender, RoutedEventArgs e)
        {
            ModbusManager.Instance.ReConnect("127.0.0.1", 512);
            ButtonIsConnected_Click(null, null);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            ModbusManager.Instance.Close();
        }

        private void ButtonReadInt16_Click(object sender, RoutedEventArgs e)
        {
            int value100 = ModbusManager.Instance.ReadInt16(100);
            _ = MessageBox.Show("Modbus 100 = " + value100);
        }

        private void ButtonReadFloat_Click(object sender, RoutedEventArgs e)
        {
            double value101 = ModbusManager.Instance.ReadFloat(101);
            _ = MessageBox.Show("Modbus 101 = " + value101);
        }

        private void ButtonWriteInt16_Click(object sender, RoutedEventArgs e)
        {
            bool result = ModbusManager.Instance.Write(100, 1);
            _ = result ? MessageBox.Show("Modbus 100 写入：1") : MessageBox.Show("Modbus 100 写入失败");
        }

        private void ButtonWriteFloat_Click(object sender, RoutedEventArgs e)
        {
            bool result = ModbusManager.Instance.Write(101, 1.2f);
            _ = result ? MessageBox.Show("Modbus 101 写入：1.2") : MessageBox.Show("Modbus 101 写入失败");
        }
    }
}
