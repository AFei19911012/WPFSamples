using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace Wpf_Base.LogWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/10/01 16:22:18
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/10/01 16:22:18    CoderMan/CoderdMan1012         首次编写         
    ///
    public class LogToolVM : ViewModelBase
    {
        private ObservableCollection<DataModel> _ListLogs = new ObservableCollection<DataModel>();
        public ObservableCollection<DataModel> ListLogs
        {
            get => _ListLogs;
            set => Set(ref _ListLogs, value);
        }

        public LogToolVM()
        {

        }
    }
}