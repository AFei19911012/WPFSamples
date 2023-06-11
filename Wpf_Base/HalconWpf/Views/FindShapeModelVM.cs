using GalaSoft.MvvmLight;

namespace Wpf_Base.HalconWpf.Views
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 16:21:16
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 16:21:16    CoderMan/CoderdMan1012         首次编写         
    ///
    public class FindShapeModelVM : ViewModelBase
    {
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

        private double numMinScore = 0.2;
        public double NumMinScore
        {
            get => numMinScore;
            set => Set(ref numMinScore, value);
        }

        private int intNumMatches = 1;
        public int IntNumMatches
        {
            get => intNumMatches;
            set => Set(ref intNumMatches, value);
        }

        private double numMaxOverlap = 0;
        public double NumMaxOverlap
        {
            get => numMaxOverlap;
            set => Set(ref numMaxOverlap, value);
        }


        private string _StrSelectSubPixel = "least_squares";
        public string StrSelectSubPixel
        {
            get => _StrSelectSubPixel;
            set => Set(ref _StrSelectSubPixel, value);
        }

        private int intNumLevels = 0;
        public int IntNumLevels
        {
            get => intNumLevels;
            set => Set(ref intNumLevels, value);
        }

        private double numGreediness = 0.1;
        public double NumGreediness
        {
            get => numGreediness;
            set => Set(ref numGreediness, value);
        }

        private double numMatchRow = 0;
        public double NumMatchRow
        {
            get => numMatchRow;
            set => Set(ref numMatchRow, value);
        }

        private double numMatchCol = 0;
        public double NumMatchCol
        {
            get => numMatchCol;
            set => Set(ref numMatchCol, value);
        }

        private double numMatchAngle = 0;
        public double NumMatchAngle
        {
            get => numMatchAngle;
            set => Set(ref numMatchAngle, value);
        }

        private double numMatchScore = 0;
        public double NumMatchScore
        {
            get => numMatchScore;
            set => Set(ref numMatchScore, value);
        }
    }
}