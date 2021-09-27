using GalaSoft.MvvmLight;

namespace HalconWPF.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/8/18 23:36:08
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/8/18 23:36:08    Taosy.W                 
    ///
    public class DataModel : ViewModelBase
    {
        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        public string ImgPath { get; set; }

        public double ImageX { get; set; }
        public double ImageY { get; set; }
        public double MachineX { get; set; }
        public double MachineY { get; set; }
    }
}