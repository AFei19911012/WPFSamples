using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfControlsX.Helper;

namespace WpfControlsX.ControlX
{
    internal sealed class WxMessageBox : Window
    {
        static WxMessageBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxMessageBox), new FrameworkPropertyMetadata(typeof(WxMessageBox)));
        }

        /// <summary>
        /// 对话框类型
        /// </summary>
        public MessageBoxType MessageBoxType
        {
            get => (MessageBoxType)GetValue(MessageBoxTypeProperty);
            set => SetValue(MessageBoxTypeProperty, value);
        }

        public static readonly DependencyProperty MessageBoxTypeProperty =
            DependencyProperty.Register("MessageBoxType", typeof(MessageBoxType), typeof(WxMessageBox), new PropertyMetadata(MessageBoxType.Info));


        private string StrMessage { get; set; }
        private string StrTitle { get; set; }
        private string StrTime { get; set; }

        public MessageBoxResult Result { get; set; }

        private Timer MyTimer { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        public WxMessageBox(string message, string title, MessageBoxType type = MessageBoxType.Info, int showingTime = 0)
        {
            StrMessage = message;
            StrTitle = title;
            MessageBoxType = type;
            StrTime = DateTime.Now.ToString();

            // 自动关闭
            if (showingTime > 0)
            {
                MyTimer = new Timer(showingTime);
                MyTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
                MyTimer.Start();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MyTimer.Stop();
            _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
            {
                Close();
            }));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_Title") is TextBlock title && GetTemplateChild("PART_Message") is TextBox msg)
            {
                title.Text = StrTitle;
                msg.Text = StrMessage;
            }
            if (GetTemplateChild("PART_Time") is TextBlock time)
            {
                time.Text = StrTime;
            }
            if (GetTemplateChild("PART_ButtonClose") is WxButton buttonClose)
            {
                buttonClose.Click += (s, e) => { Close(); };
            }
            if (GetTemplateChild("PART_ButtonCancel") is WxButton buttonCancel)
            {
                buttonCancel.Click += (s, e) => { Result = MessageBoxResult.Cancel; Close(); };
            }
            if (GetTemplateChild("PART_ButtonOK") is WxButton buttonOK)
            {
                buttonOK.Click += (s, e) => { Result = MessageBoxResult.OK; Close(); };
            }

            if (Owner == null)
            {
                BorderThickness = new Thickness(1);
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        /// <summary>
        /// 重写关闭事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (Owner != null)
            {
                if (Owner.Content is Grid grid)
                {
                    if (VisualTreeHelper.GetChild(grid, 0) is UIElement original)
                    {
                        grid.Children.Remove(original);
                        Owner.Content = original;
                    }
                }
            }
        }
    }
}