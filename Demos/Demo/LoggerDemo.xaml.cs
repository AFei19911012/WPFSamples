using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Demos.Demo
{
    /// <summary>
    /// LoggerDemo.xaml 的交互逻辑
    /// </summary>
    public partial class LoggerDemo : UserControl
    {
        public LoggerDemo()
        {
            InitializeComponent();

            RTB_Logger.Document.Blocks.Clear();
            AddLogger("测试测试", EnumLoggerType.Error);
            AddLogger("测试测试", EnumLoggerType.Info);
        }

        public void AddLogger(string msg, EnumLoggerType type)
        {
            _ = Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                string t = string.Format("{0:G}", DateTime.Now);
                Run run = new Run();
                switch (type)
                {
                    case EnumLoggerType.Info:
                        run.Text = t + " 信息：" + msg;
                        run.Foreground = Brushes.Black;
                        break;
                    case EnumLoggerType.Success:
                        run.Text = t + " 成功：" + msg;
                        run.Foreground = Brushes.Green;
                        break;
                    case EnumLoggerType.Warning:
                        run.Text = t + " 警告：" + msg;
                        run.Foreground = Brushes.BlueViolet;
                        break;
                    case EnumLoggerType.Error:
                        run.Text = t + " 错误：" + msg;
                        run.Foreground = Brushes.OrangeRed;
                        break;
                    default:
                        break;
                }
                Paragraph paragraph = new Paragraph(run)
                {
                    LineHeight = 2,
                };
                RTB_Logger.Document.Blocks.Add(paragraph);

                // 滚动至最后行
                RTB_Logger.ScrollToEnd();

                // 删除
                if (RTB_Logger.Document.Blocks.Count > 1000)
                {
                    for (int i = 1000; i < RTB_Logger.Document.Blocks.Count; i++)
                    {
                        _ = RTB_Logger.Document.Blocks.Remove(RTB_Logger.Document.Blocks.FirstBlock);
                    }
                }
            });
        }
    }

    public enum EnumLoggerType
    {
        Info = 0,
        Success,
        Warning,
        Error,
    }
}