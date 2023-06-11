using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Wpf_Base.CommunicationWpf;
using Wpf_Base.LogWpf;
using Wpf_Base.PopWindowWpf;

namespace Wpf_Base
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // 绑定日志事件
            MyCcdManagerControl.LogEvent += PrintLog;
            MyControlsDemo.LogEvent += PrintLog;
            MyTcpDemo.LogEvent += PrintLog;
            MyMelsecPlcDemo.LogEvent += PrintLog;
            MyModbusDemo.LogEvent += PrintLog;
            MyHikCameraDemo.LogEvent += PrintLog;
            MyQrTool.LogEvent += PrintLog;
            MyCaliperTool.LogEvent += PrintLog;
            MyCalibrationTool.LogEvent += PrintLog;
            MyMetrologyTool.LogEvent += PrintLog;
            MyOcrTool.LogEvent += PrintLog;
            MyRoiTool.LogEvent += PrintLog;
            MyShapeModuleTool.LogEvent += PrintLog;
            MyScaledShapeModuleTool.LogEvent += PrintLog;
            MyAnisoShapeModuleTool.LogEvent += PrintLog;
            MyCircleCalibrationTool.LogEvent += PrintLog;
            MySiemensS7NetDemo.LogEvent += PrintLog;

            McManager.Instance.LogEvent += PrintLog;
            ModbusManager.Instance.LogEvent += PrintLog;
            TcpClientManager.Instance.LogEvent += PrintLog;
            TcpServerManager.Instance.LogEvent += PrintLog;
            S7Manager.Instance.LogEvent += PrintLog;

            InitGeometry();
        }

        private void PrintLog(string info, EnumLogType type)
        {
            MyLog?.AddLog(info, type);
        }

        private void InitGeometry()
        {
            ResourceDictionary geometry = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Theme/Geometry.xaml")
            };

            List<string> names = new List<string>();
            foreach (object item in geometry.Keys)
            {
                names.Add((string)item);
            }

            List<Geometry> paths = new List<Geometry>();
            foreach (object item in geometry.Values)
            {
                paths.Add((Geometry)item);
            }

            for (int i = 0; i < names.Count; i++)
            {
                Path path = new Path
                {
                    Margin = new Thickness(10),
                    Fill = (Brush)FindResource("PrimaryBrush"),
                    Stretch = Stretch.Uniform,
                    ToolTip = names[i],
                    Data = paths[i],
                    Width = 48,
                    Height = 48,
                };
                _ = MyGeometryContainer.Children.Add(path);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowClosedQuestion window = new WindowClosedQuestion();
            _ = window.ShowDialog();

            if (window.IsClosing)
            {
                Application.Current.Shutdown();
                // Environment.Exit(0);
            }
            else
            {
                // 必不可少
                e.Cancel = true;
                return;
            }
        }
    }
}
