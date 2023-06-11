
using System;
using System.Timers;
using System.Windows;
using Wpf_Base.MethodNet;

namespace Wpf_Base.PopWindowWpf
{
    /// <summary>
    /// WindowLoading.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLoading : Window
    {
        private Timer MyTimer { get; set; }

        public WindowLoading(string content = "程序运行中，请稍候 ······", int t = 1000)
        {
            InitializeComponent();

            TB_Content.Text = content;

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
    }
}