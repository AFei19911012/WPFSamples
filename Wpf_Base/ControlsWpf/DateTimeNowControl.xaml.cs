
using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Wpf_Base.MethodNet;

namespace Wpf_Base.ControlsWpf
{
    /// <summary>
    /// DateTimeNowControl.xaml 的交互逻辑
    /// </summary>
    public partial class DateTimeNowControl : UserControl
    {
        public DateTimeNowControl()
        {
            InitializeComponent();

            InitTimer();
        }

        private void InitTimer()
        {
            DispatcherTimer update_timer = new DispatcherTimer
            {
                // 1s 执行一次
                Interval = new TimeSpan(0, 0, 1)
            };
            update_timer.Tick += new EventHandler(RefreshDateTime);
            update_timer.Start();
        }
        private void RefreshDateTime(object sender, EventArgs e)
        {
            // WinForm 引用用户控件启动时报错
            // Application.Current 始终为 null 
            // 采用以下方式更新
            _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
            {
                TB_Now.Text = string.Format("{0:G}", DateTime.Now);
            }));
        }
    }
}