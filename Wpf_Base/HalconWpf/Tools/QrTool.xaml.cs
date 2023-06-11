using System.Windows.Controls;
using Wpf_Base.HalconWpf.ViewModel;
using Wpf_Base.LogWpf;

namespace Wpf_Base.HalconWpf.Tools
{
    /// <summary>
    /// BarCodeQRTool.xaml 的交互逻辑
    /// </summary>
    public partial class QrTool : UserControl
    {
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

        public QrTool()
        {
            InitializeComponent();

            // 日志委托
            (DataContext as QrToolVM).LogEvent += PrintLog;
            MyHalconWindowControl.LogEvent += PrintLog;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = (sender as ComboBox).SelectedIndex;
            if (idx == 0)
            {
                if (BT_QR != null)
                {
                    BT_QR.IsEnabled = false;
                    BT_BarCode.IsEnabled = true;
                }
            }
            else
            {
                if (BT_QR != null)
                {
                    BT_QR.IsEnabled = true;
                    BT_BarCode.IsEnabled = false;
                }
            }
        }
    }
}