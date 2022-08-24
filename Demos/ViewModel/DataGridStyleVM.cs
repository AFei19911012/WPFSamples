using Demos.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;


namespace Demos.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2022/8/24 14:19:00
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/8/24 14:19:00    Taosy.W                 
    ///
    public class DataGridStyleVM : ViewModelBase
    {
        private ObservableCollection<DataModel> _DataGridList;
        public ObservableCollection<DataModel> DataGridList
        {
            get => _DataGridList;
            set => Set(ref _DataGridList, value);
        }

        public DataGridStyleVM()
        {
            DataGridList = new ObservableCollection<DataModel>
            {
                new DataModel{ Name = "1054", Content = "型号1", InspectResult = "NG"},
                new DataModel{ Name = "1245", Content = "型号2", InspectResult = "OK"},
                new DataModel{ Name = "1125", Content = "型号3", InspectResult = "OK"},
            };
        }
    }
}