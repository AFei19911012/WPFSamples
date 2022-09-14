using Demos.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Demos.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2022/9/14 21:45:06
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     2022/9/14 21:45:06    CoderMan/CoderMan1012                 
    ///
    public class ProgressBarDemoVM : ViewModelBase
    {
        private ObservableCollection<DataModel> _DataGridList;
        public ObservableCollection<DataModel> DataGridList
        {
            get => _DataGridList;
            set => Set(ref _DataGridList, value);
        }

        private int _IntSelectCommand = -1;
        public int IntSelectCommand
        {
            get => _IntSelectCommand;
            set => Set(ref _IntSelectCommand, value);
        }

        private double _NumCurrentCount = 0;
        public double NumCurrentCount
        {
            get => _NumCurrentCount;
            set => Set(ref _NumCurrentCount, value);
        }

        private int _IntExecuteCount = 1;
        public int IntExecuteCount
        {
            get => _IntExecuteCount;
            set => Set(ref _IntExecuteCount, value);
        }


        public ProgressBarDemoVM()
        {
            DataGridList = new ObservableCollection<DataModel>
            {
                new DataModel{ Name = "1054", Content = "型号1", InspectResult = "NG"},
                new DataModel{ Name = "1245", Content = "型号2", InspectResult = "OK"},
                new DataModel{ Name = "1125", Content = "型号3", InspectResult = "OK"},
            };
        }

        public RelayCommand CmdRun => new Lazy<RelayCommand>(() => new RelayCommand(Run)).Value;
        private void Run()
        {
            int count = DataGridList.Count;

            // 新开一个线程
            Task task = Task.Run(() =>
            {
                // 执行多次
                for (int k = 0; k < IntExecuteCount; k++)
                {
                    NumCurrentCount = k + 1;
                    for (int i = 0; i < count; i++)
                    {
                        // 执行当前行
                        IntSelectCommand = i;
                        // do something
                        Thread.Sleep(20);
                    }
                }
            });
        }
    }
}