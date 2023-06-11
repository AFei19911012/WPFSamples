using System.Windows.Controls;
using Wpf_Base.HalconWpf.ViewModel;
using Wpf_Base.LogWpf;

namespace Wpf_Base.HalconWpf.Tools
{
    /// <summary>
    /// CircleCalibrationTool.xaml 的交互逻辑
    /// </summary>
    public partial class CircleCalibrationTool : UserControl
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

        public CircleCalibrationTool()
        {
            InitializeComponent();
            // 日志委托
            (DataContext as CircleCalibrationToolVM).LogEvent += PrintLog;
        }
    }
}