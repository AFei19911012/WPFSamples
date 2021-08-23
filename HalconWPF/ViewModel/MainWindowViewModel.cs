
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconWPF.Model;
using HalconWPF.UserControl;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/8/18 23:30:04
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/8/18 23:30:04    Taosy.W                 
    ///
    public class MainWindowViewModel : ViewModelBase
    {
        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => Set(ref selectedIndex, value);
        }

        private ObservableCollection<DataModel> dataList;
        public ObservableCollection<DataModel> DataList
        {
            get => dataList;
            set => Set(ref dataList, value);
        }

        /// <summary>
        /// 切换功能页面
        /// </summary>
        public RelayCommand<Grid> CmdSelectionChanged => new Lazy<RelayCommand<Grid>>(() => new RelayCommand<Grid>(SelectionChanged)).Value;
        private void SelectionChanged(Grid mainContent)
        {
            if (SelectedIndex < 0)
            {
                return;
            };
            mainContent.Children.Clear();
            string name = DataList[SelectedIndex].Name;
            if (name == "AcquisitionImage")
            {
                mainContent.Children.Add(new AcquisitionImage());
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel()
        {
            SelectedIndex = -1;
            DataList = GetDataList();
        }

        private ObservableCollection<DataModel> GetDataList()
        {
            return new ObservableCollection<DataModel>
            {
                new DataModel{ Name = "AcquisitionImage", ImgPath="pack://application:,,,/Resource/Image/A.png"},
            };
        }
    }
}
