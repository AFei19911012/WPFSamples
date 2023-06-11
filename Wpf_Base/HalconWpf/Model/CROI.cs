namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 12:39:53
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 12:39:53    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CROI
    {
        /// <summary>
        /// 矩形 ROI
        /// </summary>
        public double Row1 { get; set; }
        public double Col1 { get; set; }
        public double Row2 { get; set; }
        public double Col2 { get; set; }


        /// <summary>
        /// 圆 ROI
        /// </summary>
        public double Row { get; set; }
        public double Col { get; set; }
        public double R { get; set; }
    }
}