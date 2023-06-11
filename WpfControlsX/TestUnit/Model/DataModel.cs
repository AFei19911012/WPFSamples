using System.Collections.ObjectModel;

namespace TestUnit.Model
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/2/28 2:40:55
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/2/28 2:40:55                     BigWang         首次编写         
    ///
    public class DataModel
    {
        public string Text { get; set; }

        public string ImagePath { get; set; }

        public string Name { get; set; }

        public string TimeStr { get; set; }

        public bool ShowIcon { get; set; }

        public bool IsChecked { get; set; }

        public ObservableCollection<DataModel> Children { get; set; }


        public string Header { get; set; }
        public string Content { get; set; }
        public string Footer { get; set; }

        public DataModel()
        {
            Children = new ObservableCollection<DataModel>();
        }
    }
}