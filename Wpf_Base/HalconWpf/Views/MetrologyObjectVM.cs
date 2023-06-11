using GalaSoft.MvvmLight;

namespace Wpf_Base.HalconWpf.Views
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 17:42:26
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 17:42:26    CoderMan/CoderdMan1012         首次编写         
    ///
    public class MetrologyObjectVM : ViewModelBase
    {
        private double numLength1 = 20;
        public double NumLength1
        {
            get => numLength1;
            set => Set(ref numLength1, value);
        }

        private double numLength2 = 5;
        public double NumLength2
        {
            get => numLength2;
            set => Set(ref numLength2, value);
        }

        private double numSigma = 2;
        public double NumSigma
        {
            get => numSigma;
            set => Set(ref numSigma, value);
        }

        private double numThreshold = 30;
        public double NumThreshold
        {
            get => numThreshold;
            set => Set(ref numThreshold, value);
        }

        private string _StrSelectSelect = "all";
        public string StrSelectSelect
        {
            get => _StrSelectSelect;
            set => Set(ref _StrSelectSelect, value);
        }

        private string _StrSelectTransition = "all";
        public string StrSelectTransition
        {
            get => _StrSelectTransition;
            set => Set(ref _StrSelectTransition, value);
        }

        private string _StrSelectInterpolation = "nearest_neighbor";
        public string StrSelectInterpolation
        {
            get => _StrSelectInterpolation;
            set => Set(ref _StrSelectInterpolation, value);
        }

        private double numMinScore = 0.5;
        public double NumMinScore
        {
            get => numMinScore;
            set => Set(ref numMinScore, value);
        }

        private int intMinInstances = 1;
        public int IntMinInstances
        {
            get => intMinInstances;
            set => Set(ref intMinInstances, value);
        }
    }
}