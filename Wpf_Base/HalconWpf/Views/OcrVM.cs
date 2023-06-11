using GalaSoft.MvvmLight;

namespace Wpf_Base.HalconWpf.Views
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 13:34:08
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 13:34:08    CoderMan/CoderdMan1012         首次编写         
    ///
    public class OcrVM : ViewModelBase
    {
        private int minCharHeight = 10;
        public int MinCharHeight
        {
            get => minCharHeight;
            set => Set(ref minCharHeight, value);
        }

        private int minCharWidth = 5;
        public int MinCharWidth
        {
            get => minCharWidth;
            set => Set(ref minCharWidth, value);
        }

        private int numRow1 = 0;
        public int NumRow1
        {
            get => numRow1;
            set => Set(ref numRow1, value);
        }

        private int numRow2 = 10;
        public int NumRow2
        {
            get => numRow2;
            set => Set(ref numRow2, value);
        }

        private int numCol1 = 0;
        public int NumCol1
        {
            get => numCol1;
            set => Set(ref numCol1, value);
        }

        private int numCol2 = 10;
        public int NumCol2
        {
            get => numCol2;
            set => Set(ref numCol2, value);
        }
    }
}