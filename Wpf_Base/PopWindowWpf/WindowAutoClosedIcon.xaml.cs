using System;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Wpf_Base.MethodNet;

namespace Wpf_Base.PopWindowWpf
{
    /// <summary>
    /// WindowAutoClosedIcon.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAutoClosedIcon : Window
    {
        private Timer MyTimer { get; set; }

        public WindowAutoClosedIcon(EnumWindowType window_type, string content = "程序运行中，请稍候 ······", int t = 1000)
        {
            InitializeComponent();

            TB_Info.Text = content;
            TB_Time.Text = DateTime.Now.ToString("G");
            // 资源
            MyPath.Data = (Geometry)FindResource("Icon" + window_type.ToString());
            MyPath.Fill = (Brush)FindResource(window_type.ToString() + "Brush");

            MyTimer = new Timer(t);
            MyTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            MyTimer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MyTimer.Stop();
            _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
            {
                Close();
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}