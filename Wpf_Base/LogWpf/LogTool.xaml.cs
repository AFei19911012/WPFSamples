using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_Base.LogWpf
{
    /// <summary>
    /// LogWpf.xaml 的交互逻辑
    /// </summary>
    public partial class LogTool : UserControl
    {
        public bool IsAutoSaved { get; set; } = true;
        public int LogAutoSavedCount { get; set; } = 1000;
        public bool IsAutoDelete { get; set; } = true;
        public int LogDeleteTimeDelay { get; set; } = 5;
        private Timer MyLogTimer { get; set; }

        private LogToolVM VM { get; set; }

        public LogTool()
        {
            InitializeComponent();

            VM = DataContext as LogToolVM;

            // 初始化日志
            InitLog();
        }

        public void InitLog()
        {
            VM.ListLogs = new ObservableCollection<DataModel>
            {
                new DataModel { Type = EnumLogType.Success.ToString(), Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), Content = "程序启动" },
            };
        }

        /// <summary>
        /// 滚动至末尾
        /// </summary>
        public void ScrollToEnd()
        {
            if (MyLogList.Items.Count > 0)
            {
                MyLogList.ScrollIntoView(MyLogList.Items[MyLogList.Items.Count - 1]);
            }
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        public void AddLog(string msg, EnumLogType type)
        {
            DateTime dt = DateTime.Now;
            _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
              {
                  VM.ListLogs.Add(new DataModel
                  {
                      Type = type.ToString(),
                      Time = dt.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                      Content = msg,
                  });

                  // 自动滚动至末尾
                  ScrollToEnd();

                  // 超过最多数量自动保存删除
                  if (IsAutoSaved && VM.ListLogs.Count >= LogAutoSavedCount)
                  {
                      SaveLog();
                      VM.ListLogs.Clear();
                  }
              }));
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        public void SaveLog()
        {
            DirectoryInfo folder = new DirectoryInfo(@"RunLog");
            if (!folder.Exists)
            {
                folder.Create();
            }

            string filename = @"RunLog\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".log";
            using (StreamWriter sw = new StreamWriter(filename))
            {
                for (int i = 0; i < VM.ListLogs.Count; i++)
                {
                    string txt = string.Join(" ", VM.ListLogs[i].Type, VM.ListLogs[i].Time, VM.ListLogs[i].Content);
                    sw.WriteLine(txt);
                }
            }
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        public void ClearLog()
        {
            VM.ListLogs.Clear();
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        public void DeleteLog()
        {
            MyLogTimer = new Timer(DeleteLog, null, 100, 1000);

            // 立即执行一次
            _ = MyLogTimer.Change(0, 1000);
        }

        private void DeleteLog(object obj)
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

        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            ClearLog();
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveLog();
        }
    }
}