using GalaSoft.MvvmLight;
using HalconDotNet;

namespace Wpf_Base.HalconWpf.Views
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/20 17:18:52
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/20 17:18:52    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CreateScaledShapeModelVM : ViewModelBase
    {
        private HTuple _StrSelectNumLevels = 6;
        public HTuple StrSelectNumLevels
        {
            get => _StrSelectNumLevels;
            set => Set(ref _StrSelectNumLevels, value);
        }

        private double _NumSelectAngleStart = -3.14;
        public double NumSelectAngleStart
        {
            get => _NumSelectAngleStart;
            set => Set(ref _NumSelectAngleStart, value);
        }

        private double _NumSelectAngleExtent = 6.29;
        public double NumSelectAngleExtent
        {
            get => _NumSelectAngleExtent;
            set => Set(ref _NumSelectAngleExtent, value);
        }

        private HTuple _StrSelectAngleStep = "auto";
        public HTuple StrSelectAngleStep
        {
            get => _StrSelectAngleStep;
            set => Set(ref _StrSelectAngleStep, value);
        }

        private double _NumScaleMin = 0.9;
        public double NumScaleMin
        {
            get => _NumScaleMin;
            set => Set(ref _NumScaleMin, value);
        }

        private double _NumScaleMax = 1.1;
        public double NumScaleMax
        {
            get => _NumScaleMax;
            set => Set(ref _NumScaleMax, value);
        }

        private HTuple _StrSelectScaleStep = "auto";
        public HTuple StrSelectScaleStep
        {
            get => _StrSelectScaleStep;
            set => Set(ref _StrSelectScaleStep, value);
        }


        private string _StrSelectOptimization = "auto";
        public string StrSelectOptimization
        {
            get => _StrSelectOptimization;
            set => Set(ref _StrSelectOptimization, value);
        }

        private string _StrSelectMetric = "use_polarity";
        public string StrSelectMetric
        {
            get => _StrSelectMetric;
            set => Set(ref _StrSelectMetric, value);
        }

        private HTuple _StrSelectContrast = "auto";
        public HTuple StrSelectContrast
        {
            get => _StrSelectContrast;
            set => Set(ref _StrSelectContrast, value);
        }

        private HTuple _StrSelectMinContrast = "auto";
        public HTuple StrSelectMinContrast
        {
            get => _StrSelectMinContrast;
            set => Set(ref _StrSelectMinContrast, value);
        }
    }
}