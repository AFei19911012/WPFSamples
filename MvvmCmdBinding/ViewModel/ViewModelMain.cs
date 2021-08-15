using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmCmdBinding.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MvvmCmdBinding.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/8/6 23:59:18
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/8/6 23:59:18    Taosy.W                 
    ///
    public class ViewModelMain : ViewModelBase
    {
        private string bindingText;
        public string BindingText
        {
            get => bindingText;
            set => Set(ref bindingText, value);
        }

        private double bindingNumber;
        public double BindingNumber
        {
            get => bindingNumber;
            set => Set(ref bindingNumber, value);
        }

        private Gender bindingEnum;
        public Gender BindingEnum
        {
            get => bindingEnum;
            set => Set(ref bindingEnum, value);
        }

        private string showingText;
        public string ShowingText
        {
            get => showingText;
            set => Set(ref showingText, value);
        }

        private ObservableCollection<DataModel> dataList;
        public ObservableCollection<DataModel> DataList
        {
            get => dataList;
            set => Set(ref dataList, value);
        }

        public RelayCommand<MainWindow> CmdLoaded => new Lazy<RelayCommand<MainWindow>>(() => new RelayCommand<MainWindow>(Loaded)).Value;
        private void Loaded(MainWindow window)
        {
            MessageBox.Show("MainWindow Loaded: " + window.ActualWidth + " * " + window.ActualHeight);
        }

        public RelayCommand<MouseEventArgs> CmdMouseMove => new RelayCommand<MouseEventArgs>(MouseMove);
        private void MouseMove(MouseEventArgs e)
        {
            // 显示鼠标所在位置
            System.Windows.Point point = e.GetPosition(e.Device.Target);
            ShowingText = point.X + ", " + point.Y;
        }

        public RelayCommand CmdWithoutParameter => new Lazy<RelayCommand>(() => new RelayCommand(WithoutParameter)).Value;
        private void WithoutParameter()
        {
            MessageBox.Show("Command Binding without parameter");
        }

        public RelayCommand<string> CmdWithParameter => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(WithParameter)).Value;
        private void WithParameter(string info)
        {
            MessageBox.Show("Command Binding without parameter: " + info);
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        public RelayCommand<MouseButtonEventArgs> CmdMouseDown => new Lazy<RelayCommand<MouseButtonEventArgs>>(() => new RelayCommand<MouseButtonEventArgs>(MouseDown)).Value;
        private void MouseDown(MouseButtonEventArgs e)
        {
            // 判断按下的鼠标按键
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MessageBox.Show("Left mouse button down.");
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                MessageBox.Show("Right mouse button down.");
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                MessageBox.Show("Middle mouse button down.");
            }
        }


        public ViewModelMain()
        {
            InitParams();
        }

        private void InitParams()
        {
            BindingText = "This is text";
            BindingNumber = 3.14159;
            BindingEnum = 0;
            DataList = GetDataList();
        }

        private ObservableCollection<DataModel> GetDataList()
        {
            return new ObservableCollection<DataModel>
            {
                new DataModel{ Number = 1, Name = "AAA", Type=Gender.Male, IsChecked = true, 
                               DataList = new ObservableCollection<DataModel>{ new DataModel { Name = "AAA-1"},
                                                                               new DataModel { Name = "AAA-2"} } },
                new DataModel{ Number = 2, Name = "BBB", Type=Gender.Female, IsChecked = false },
                new DataModel{ Number = 3, Name = "CCC", Type=Gender.Female, IsChecked = false,
                               DataList = new ObservableCollection<DataModel>{ new DataModel { Name = "CCC-1"} } },
                new DataModel{ Number = 4, Name = "DDD", Type=Gender.Female, IsChecked = true },
                new DataModel{ Number = 5, Name = "EEE", Type=Gender.Male, IsChecked = true },
                new DataModel{ Number = 6, Name = "FFF", Type=Gender.Male, IsChecked = false },
            };
        }
    }
}
