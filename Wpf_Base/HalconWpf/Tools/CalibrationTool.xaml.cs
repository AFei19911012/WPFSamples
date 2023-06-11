using System.Windows.Controls;
using Wpf_Base.HalconWpf.ViewModel;
using Wpf_Base.LogWpf;

namespace Wpf_Base.HalconWpf.Tools
{
    /// <summary>
    /// CalibrationTool.xaml 的交互逻辑
    /// </summary>
    public partial class CalibrationTool : UserControl
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

        public CalibrationTool()
        {
            InitializeComponent();

            // 日志委托
            (DataContext as CalibrationToolVM).LogEvent += PrintLog;
        }

        /// <summary>
        /// 滚动至当前行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalibrationToolVM vm = DataContext as CalibrationToolVM;
            int idx = vm.IntSelectedIndex;
            if (idx < 0)
            {
                return;
            }
            DataGrid dataGrid = sender as DataGrid;
            dataGrid.ScrollIntoView(dataGrid.Items[idx]);
        }
    }
}
