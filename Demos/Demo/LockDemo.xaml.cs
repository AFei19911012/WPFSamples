using Demos.Helper;
using Demos.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demos.Demo
{
    /// <summary>
    /// LockDemo.xaml 的交互逻辑
    /// </summary>
    public partial class LockDemo : UserControl
    {
        public ObservableCollection<DataModel> Source
        {
            get { return (ObservableCollection<DataModel>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ObservableCollection<DataModel>), typeof(LockDemo), new PropertyMetadata(null));



        private object Obj { get; } = new object();

        private int NumA { get; set; } = 0;
        private int NumB { get; set; } = 0;



        public LockDemo()
        {
            InitializeComponent();
        }


        private void MissionA()
        {
            NumA++;
            Source.Add(new DataModel { Name = "This is MissionA: NumA = " + NumA });
            Thread.Sleep(100);
            NumB++;
            Source.Add(new DataModel { Name = "This is MissionB: NumB = " + NumB });
        }

        private void MissionB()
        {
            NumA += 10;
            Source.Add(new DataModel { Name = "This is MissionA: NumA = " + NumA });
            Thread.Sleep(300);
            NumB += 10;
            Source.Add(new DataModel { Name = "This is MissionB: NumB = " + NumB });
        }

        private void MissionWithLockA()
        {
            lock (Obj)
            {
                MissionA();
            }
        }

        private void MissionWithLockB()
        {
            lock (Obj)
            {
                MissionB();
            }
        }

        private void MissionC()
        {
            Source.Add(new DataModel { Name = "This is MissionC: CCC" });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NumA = 0;
            NumB = 0;
            Source = new ObservableCollection<DataModel>();

            string name = (sender as Button).Content.ToString();
            if (name.StartsWith("顺序执行"))
            {
                MissionA();
                MissionB();
                MissionC();
            }
            else if (name.StartsWith("多线程"))
            {
                Task.Run(() =>
                {
                    DispatcherHelper.Dispatcher.BeginInvoke(new Action(() => { MissionA(); }));
                });
                Task.Run(() =>
                {
                    DispatcherHelper.Dispatcher.BeginInvoke(new Action(() => { MissionB(); }));
                });
                Task.Run(() =>
                {
                    DispatcherHelper.Dispatcher.BeginInvoke(new Action(() => { MissionC(); }));
                });
            }
            else if (name.StartsWith("线程锁"))
            {
                Task.Run(() =>
                { 
                    DispatcherHelper.Dispatcher.BeginInvoke(new Action(() => { MissionWithLockA(); }));
                });
                Task.Run(() =>
                { 
                    DispatcherHelper.Dispatcher.BeginInvoke(new Action(() => { MissionWithLockB(); }));
                });
                Task.Run(() =>
                { 
                    DispatcherHelper.Dispatcher.BeginInvoke(new Action(() => { MissionC(); }));
                });
            }
        }
    }
}
