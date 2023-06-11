using System.Windows.Controls;
using Wpf_Base.HalconWpf.ViewModel;
using Wpf_Base.LogWpf;

namespace Wpf_Base.HalconWpf.Tools
{
    /// <summary>
    /// RoiTool.xaml 的交互逻辑
    /// </summary>
    public partial class RoiTool : UserControl
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

        public RoiTool()
        {
            InitializeComponent();
            // 日志委托
            RoiToolVM vm = DataContext as RoiToolVM;
            vm.LogEvent += PrintLog;
        }
    }
}