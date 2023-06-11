using GalaSoft.MvvmLight;
using HalconDotNet;

namespace Wpf_Base.HalconWpf.Views
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/20 16:55:51
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/20 16:55:51    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CreateAnisoShapeModelVM : ViewModelBase
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

        private double _NumScaleRMin = 0.9;
        public double NumScaleRMin
        {
            get => _NumScaleRMin;
            set => Set(ref _NumScaleRMin, value);
        }

        private double _NumScaleRMax = 1.1;
        public double NumScaleRMax
        {
            get => _NumScaleRMax;
            set => Set(ref _NumScaleRMax, value);
        }

        private HTuple _StrSelectScaleRStep = "auto";
        public HTuple StrSelectScaleRStep
        {
            get => _StrSelectScaleRStep;
            set => Set(ref _StrSelectScaleRStep, value);
        }

        private double _NumScaleCMin = 0.9;
        public double NumScaleCMin
        {
            get => _NumScaleCMin;
            set => Set(ref _NumScaleCMin, value);
        }

        private double _NumScaleCMax = 1.1;
        public double NumScaleCMax
        {
            get => _NumScaleCMax;
            set => Set(ref _NumScaleCMax, value);
        }

        private HTuple _StrSelectScaleCStep = "auto";
        public HTuple StrSelectScaleCStep
        {
            get => _StrSelectScaleCStep;
            set => Set(ref _StrSelectScaleCStep, value);
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