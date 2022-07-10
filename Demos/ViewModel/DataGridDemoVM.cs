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
    /// Created Time: 2022/7/5 21:44:25
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/7/5 21:44:25    Taosy.W                 
    ///
    public class DataGridDemoVM : ViewModelBase
    {
        private ObservableCollection<DataModel> _DataGridList;
        public ObservableCollection<DataModel> DataGridList
        {
            get => _DataGridList;
            set => Set(ref _DataGridList, value);
        }

        public DataGridDemoVM()
        {
            DataGridList = new ObservableCollection<DataModel>
            {
                new DataModel{ Name = "TextBox", Content = "This is a TextBox"},
                new DataModel{ Name = "ComboBox", Content = "This is a ComboBox"},
                new DataModel{ Name = "TextBoxBinding", Content = "This is a TextBox"},
            };
        }

        public RelayCommand CmdGetNewValues => new Lazy<RelayCommand>(() => new RelayCommand(GetNewValues)).Value;
        private void GetNewValues()
        {
            _ = MessageBox.Show(string.Format("第一行：Name = {0}, Content = {1}\n第三行：Name = {2}, Content = {3}",
                DataGridList[0].Name, DataGridList[0].Content, DataGridList[2].Name, DataGridList[2].Content));
        }
    }

    public class DataModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
}