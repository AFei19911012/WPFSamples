using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Wpf_Base.ControlsWpf;
using Wpf_Base.ControlsWpf.Model;
using Wpf_Base.LogWpf;

namespace Wpf_Base.TestWpf
{
    /// <summary>
    /// ControlsDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ControlsDemo : UserControl
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

        public ControlsDemo()
        {
            InitializeComponent();

            MyPlotPieControl.LogEvent += PrintLog;
        }

        private void ButtonPlot_Click(object sender, RoutedEventArgs e)
        {
            // 饼图半径
            MyPlotPieControl.Radius = 250;
            MyPlotPieControl.PlotSeries = new List<CPlotInfo>
            {
                new CPlotInfo { Name = "转接片判定", Fill = null, Value = 1/15.0 },
                new CPlotInfo { Name = "贴胶判定", Fill = null, Value = 2/15.0 },
                new CPlotInfo { Name = "转接片焊偏判定", Fill = null, Value = 3/15.0 },
                new CPlotInfo { Name = "电芯外观判定", Fill = null, Value = 4/15.0},
                new CPlotInfo { Name = "来料判定", Fill = null, Value = 5/15.0 },
            };

            MyPlotPieControl.SetColorMap(CPlotConstant.Default);
            MyPlotPieControl.PlotModel();
        }

        private void ButtonSize_Click(object sender, RoutedEventArgs e)
        {
            MyPlotPieControl.Radius = 200;
            MyPlotPieControl.PlotModel();
        }

        private void ButtonData_Click(object sender, RoutedEventArgs e)
        {
            MyPlotPieControl.PlotSeries = new List<CPlotInfo>
            {
                new CPlotInfo { Name = "Legend1", Fill = null, Value = 1 },
                new CPlotInfo { Name = "Legend2", Fill = null, Value = 2 },
                new CPlotInfo { Name = "Legend3", Fill = null, Value = 3 },
                new CPlotInfo { Name = "Legend4", Fill = null, Value = 4},
                new CPlotInfo { Name = "Legend5", Fill = null, Value = 5 },
                new CPlotInfo { Name = "Legend6", Fill = null, Value = 6 },
                new CPlotInfo { Name = "Legend7", Fill = null, Value = 7 },
                new CPlotInfo { Name = "Legend8", Fill = null, Value = 8 },
                new CPlotInfo { Name = "Legend9", Fill = null, Value = 9 },
            };
            MyPlotPieControl.PlotModel();
        }

        private void ButtonColor_Click(object sender, RoutedEventArgs e)
        {
            MyPlotPieControl.SetColorMap(CPlotConstant.RainBow);
            MyPlotPieControl.PlotModel();
        }
    }
}