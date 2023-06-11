
using System.Windows;
using System.Windows.Controls;
using Wpf_Base.LogWpf;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// LogDemo.xaml 的交互逻辑
    /// </summary>
    public partial class LogDemo : UserControl
    {
        private int Index { get; set; } = 1;

        public LogDemo()
        {
            InitializeComponent();

            // 自动删除日志
            MyLog.DeleteLog();

            // 初始化
            MyLog.InitLog();
            //MyLog.ClearLog();
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (Index % 10 == 1)
            {
                MyLog.AddLog(string.Format("第{0:D3}条日志", Index), EnumLogType.Info);
            }
            else if (Index % 10 == 2)
            {
                MyLog.AddLog(string.Format("第{0:D3}条日志", Index), EnumLogType.Warning);
            }
            else if (Index % 10 == 3)
            {
                MyLog.AddLog(string.Format("第{0:D3}条日志", Index), EnumLogType.Success);
            }
            else if (Index % 10 == 4)
            {
                MyLog.AddLog(string.Format("第{0:D3}条日志", Index), EnumLogType.Error);
            }
            else
            {
                MyLog.AddLog(string.Format("第{0:D3}条日志", Index), EnumLogType.Debug);
            }
            Index++;
        }
    }
}
