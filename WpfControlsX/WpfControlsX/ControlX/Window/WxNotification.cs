using System;
using System.Windows;
using System.Windows.Threading;

namespace WpfControlsX.ControlX
{
    public class WxNotification : Window
    {
        static WxNotification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxNotification), new FrameworkPropertyMetadata(typeof(WxNotification)));
        }


        /// <summary>
        /// 窗体显示时间
        /// </summary>
        public int WaitTime
        {
            get => (int)GetValue(WaitTimeProperty);
            set => SetValue(WaitTimeProperty, value);
        }
        public static readonly DependencyProperty WaitTimeProperty =
            DependencyProperty.Register("WaitTime", typeof(int), typeof(WxNotification), new PropertyMetadata(5));


        /// <summary>
        ///     计数
        /// </summary>
        private int _tickCount;

        /// <summary>
        ///     关闭计时器
        /// </summary>
        private DispatcherTimer _timerClose;


        public WxNotification()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
        }

        public static WxNotification Show(object content, bool staysOpen = false)
        {
            WxNotification notification = new()
            {
                Content = content,
                Opacity = 0,
            };

            notification.Show();

            Rect desktopWorkingArea = SystemParameters.WorkArea;
            double leftMax = desktopWorkingArea.Width - notification.ActualWidth;
            double topMax = desktopWorkingArea.Height - notification.ActualHeight;

            notification.Opacity = 1;
            notification.Left = leftMax;
            notification.Top = topMax;

            if (!staysOpen)
            {
                notification.StartTimer();
            }

            return notification;
        }

        /// <summary>
        ///     开始计时器
        /// </summary>
        private void StartTimer()
        {
            _timerClose = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timerClose.Tick += delegate
            {
                if (IsMouseOver)
                {
                    _tickCount = 0;
                    return;
                }

                _tickCount++;
                if (_tickCount >= WaitTime)
                {
                    Close();
                }
            };
            _timerClose.Start();
        }
    }
}