using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;

namespace Wpf_Base.TestWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2022/10/30 10:53:13
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     2022/10/30 10:53:13    CoderMan/CoderMan1012                 
    ///
    public class ControlsDemoVM : ViewModelBase
    {

        private string _BtnName = "ddd";
        public string BtnName
        {
            get => _BtnName;
            set => Set(ref _BtnName, value);
        }


        public RelayCommand CmdRun => new Lazy<RelayCommand>(() => new RelayCommand(Run)).Value;
        private void Run()
        {
            BtnName = "new " + DateTime.Now.ToString();
        }
    }
}