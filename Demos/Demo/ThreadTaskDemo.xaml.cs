using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Demos.Demo
{
    /// <summary>
    /// ThreadTaskDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ThreadTaskDemo : UserControl
    {
        private string Thread_String { get; set; }
        private string Thread_Para_String { get; set; }
        private string Task_String { get; set; }
        private string Task_Para_String { get; set; }
        private string Timer_String { get; set; }

        public ThreadTaskDemo()
        {
            InitializeComponent();

            Thread_String = TB_Thread.Text;
            Thread_Para_String = TB_ThreadPara.Text;
            Task_String = TB_Task.Text;
            Task_Para_String = TB_Task_Para.Text;
            Timer_String = TB_Timer.Text;
        }

        /// <summary>
        /// 线程：无参、有参
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnThread_Click(object sender, RoutedEventArgs e)
        {
            // 线程：无参
            Thread thread = new Thread(ThreadEvent)
            {
                IsBackground = true
            };
            thread.Start();

            // 线程：带参
            //Thread thread_para = new Thread(new ThreadStart(delegate { ThreadParaEvent("带参"); }));
            Thread thread_para = new Thread(() => { ThreadParaEvent("带参"); });
            thread_para.Start();

            _ = MessageBox.Show("线程状态：" + thread.IsAlive + "\n" + "线程状态：" + thread_para.IsAlive);
        }

        private void ThreadEvent()
        {
            // 在新的线程里用如下方式更新到界面
            //_ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            //(ThreadStart)delegate ()
            //{
            //    TB_Thread.Text = Thread_String + TB_Timer.Text;
            //    Thread.Sleep(100);
            //}
            //);
            TB_Thread.Text = Thread_String + TB_Timer.Text;
        }

        private void ThreadParaEvent(string para)
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                TB_ThreadPara.Text = Thread_Para_String + para + " " + TB_Timer.Text;
            }
            );
            Thread.Sleep(3000);
        }

        private void BtnTask_Click(object sender, RoutedEventArgs e)
        {
            // 任务：无参
            Task task = new Task(TaskEvent);
            task.Start();

            // 任务：带参
            //Task task_para = new Task(() => { TaskParaEvent("带参"); });
            //task_para.Start();
            Task task_para = Task.Run(() => { TaskParaEvent("带参"); });

            _ = MessageBox.Show("线程状态：" + task.Status + "\n" + "线程状态：" + task_para.Status);
        }

        private void TaskEvent()
        {
            _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                TB_Task.Text = Task_String + TB_Timer.Text;
            }));
        }

        private void TaskParaEvent(string para)
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                TB_Task_Para.Text = Task_Para_String + para + " " + TB_Timer.Text;
            }
            );
            Thread.Sleep(3000);
        }

        private void BtnTimer_Click(object sender, RoutedEventArgs e)
        {
            DispatcherTimer update_timer = new DispatcherTimer
            {
                // 1s 执行一次
                Interval = new TimeSpan(0, 0, 1)
            };
            update_timer.Tick += new EventHandler(TimerEvent);
            update_timer.Start();
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                TB_Timer.Text = Timer_String + string.Format("{0:G}", DateTime.Now);
            }
            );
        }

        private void BtnWaitHandle_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(TaskWaitHandle);
            task.Start();

            // 2022-7-28
            Task task1 = Task.Run(TaskEvent);
            Task task2 = Task.Run(() => { TaskParaEvent("带参"); });
            // 等待任务完成
            Task.WaitAll(task1, task2);
            _ = MessageBox.Show("任务1状态：" + task1.Status + "\n" + "任务2状态：" + task2.Status);
        }

        private void TaskWaitHandle()
        {
            // 2 个任务同步执行
            AutoResetEvent[] watchers = new AutoResetEvent[2];
            for (int i = 0; i < watchers.Length; i++)
            {
                watchers[i] = new AutoResetEvent(false);
            }
            Task[] tasks = new Task[2];
            tasks[0] = new Task(() =>
            {
                TaskEvent();
                // 线程执行完的时候通知
                _ = watchers[0].Set();
            });

            tasks[1] = new Task(() =>
            {
                TaskParaEvent("带参");
                // 线程执行完的时候通知
                _ = watchers[1].Set();
            });

            tasks[0].Start();
            tasks[1].Start();

            bool result = WaitHandle.WaitAll(watchers);
            if (result)
            {
                _ = MessageBox.Show("任务状态：" + tasks[0].Status + "\n" + "任务状态：" + tasks[1].Status);
            }
        }
    }
}