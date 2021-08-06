using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmCmdBinding.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/8/6 23:41:07
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/8/6 23:41:07    Taosy.W                 
    ///
    public class DataModel : ViewModelBase
    {
        private int number;
        public int Number
        {
            get => number;
            set => Set(ref number, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private Gender type;
        public Gender Type
        {
            get => type;
            set => Set(ref type, value);
        }

        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set => Set(ref isChecked, value);
        }
    }
}
