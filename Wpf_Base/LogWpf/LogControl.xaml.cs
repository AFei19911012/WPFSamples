using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Wpf_Base.LogWpf
{
    /// <summary>
    /// LogControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogControl : UserControl
    {
        public bool IsAutoSaved { get; set; } = true;
        public int LogAutoSavedCount { get; set; } = 1000;
        public bool IsAutoDelete { get; set; } = true;
        public int LogDeleteTimeDelay { get; set; } = 5;

        public LogControl()
        {
            InitializeComponent();

            RTB_Logger.Document.Blocks.Clear();
        }

        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            RTB_Logger.Document.Blocks.Clear();
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveLog();
        }

        #region 对外接口：添加日志、保存日志、删除日志
        public void AddLog(string msg, EnumLogType type)
        {
            //_ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //  {

            //  }));

            DateTime dt = DateTime.Now;
            _ = Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Run run = new Run();
                switch (type)
                {
                    case EnumLogType.Debug:
                        run.Foreground = CLogBrush.BrushLogDebug;
                        break;
                    case EnumLogType.Info:
                        run.Foreground = CLogBrush.BrushLogInfo;
                        break;
                    case EnumLogType.Warning:
                        run.Foreground = CLogBrush.BrushLogWarning;
                        break;
                    case EnumLogType.Error:
                        run.Foreground = CLogBrush.BrushLogError;
                        break;
                    case EnumLogType.Success:
                        run.Foreground = CLogBrush.BrushLogSuccess;
                        break;
                    default:
                        break;
                }
                run.Text = string.Format("[{0}] {1} {2}", type.ToString(), dt.ToString("yyyy-MM-dd HH:mm:ss:fff"), msg);
                Paragraph paragraph = new Paragraph(run)
                {
                    LineHeight = 2.5,
                };
                RTB_Logger.Document.Blocks.Add(paragraph);

                // 滚动至最后行
                RTB_Logger.ScrollToEnd();

                // 自动保存并清空
                if (IsAutoSaved && RTB_Logger.Document.Blocks.Count >= LogAutoSavedCount)
                {
                    SaveLog();
                    RTB_Logger.Document.Blocks.Clear();
                }
            });
        }

        public void SaveLog()
        {
            DirectoryInfo folder = new DirectoryInfo(@"RunLog");
            if (!folder.Exists)
            {
                folder.Create();
            }
            FlowDocument doc = RTB_Logger.Document;
            TextRange text = new TextRange(doc.ContentStart, doc.ContentEnd);
            string filename = @"RunLog\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".log";
            using (FileStream file = new FileStream(filename, FileMode.Create))
            {
                text.Save(file, DataFormats.Text);
            }
        }

        public void DeleteLog()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 1, 0, 0)
            };
            timer.Tick += new EventHandler(DeleteLog);
            timer.Start();

            DeleteLog(null, null);
        }
        #endregion

        private void DeleteLog(object sender, EventArgs e)
        {
            DirectoryInfo folder = new DirectoryInfo(@"RunLog");
            if (folder.Exists && IsAutoDelete)
            {
                FileInfo[] files = folder.GetFiles();
                DateTime tt = DateTime.Now;
                foreach (FileInfo item in files)
                {
                    DateTime t0 = item.LastWriteTime;
                    TimeSpan dt = tt - t0;
                    // 删除过期文件
                    if (dt.TotalDays >= LogDeleteTimeDelay)
                    {
                        File.Delete(item.FullName);
                    }
                }
            }
        }
    }
}
