using GalaSoft.MvvmLight;

namespace Wpf_Base.HalconWpf.Views
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 14:32:20
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 14:32:20    CoderMan/CoderdMan1012         首次编写         
    ///
    public class VisionOffsetValueVM : ViewModelBase
    {
        private double offsetMaxX = 70;
        public double OffsetMaxX
        {
            get => offsetMaxX;
            set => Set(ref offsetMaxX, value);
        }

        private double offsetMaxY = 70;
        public double OffsetMaxY
        {
            get => offsetMaxY;
            set => Set(ref offsetMaxY, value);
        }

        private double offsetMaxA = 90;
        public double OffsetMaxA
        {
            get => offsetMaxA;
            set => Set(ref offsetMaxA, value);
        }
    }
}