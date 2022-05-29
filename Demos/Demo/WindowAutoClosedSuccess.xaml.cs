using System;
using System.Timers;
using System.Windows;

namespace Demos.Demo
{
    /// <summary>
    /// WindowAutoClosedSuccess.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAutoClosedSuccess : Window
    {
        private Timer MyTimer { get; set; }

        public WindowAutoClosedSuccess(string content = "程序执行完成", int t = 1000)
        {
            InitializeComponent();

            TB_Info.Text = content;
            TB_Time.Text = DateTime.Now.ToString("G");

            MyTimer = new Timer(t);
            MyTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            MyTimer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MyTimer.Stop();
            _ = Dispatcher.BeginInvoke(new Action(() =>
            {
                Close();
            }));
        }
    }
}
