namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 13:12:34
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 13:12:34    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CMetrologyObjectMeasureParams
    {
        /// <summary>
        /// 测量模型对象类别
        /// </summary>
        public EnumMetrologyObjectType Type { get; set; }


        /// <summary>
        /// 矩形 椭圆
        /// </summary>
        public double Row { get; set; }
        public double Column { get; set; }
        public double Phi { get; set; }
        public double Length1 { get; set; }
        public double Length2 { get; set; }


        /// <summary>
        /// 圆
        /// </summary>
        public double Radius { get; set; }


        /// <summary>
        /// 直线
        /// </summary>
        public double RowBegin { get; set; }
        public double ColumnBegin { get; set; }
        public double RowEnd { get; set; }
        public double ColumnEnd { get; set; }


        /// <summary>
        /// 通用
        /// </summary>
        public double MeasureLength1 { get; set; }
        public double MeasureLength2 { get; set; }
        public double MeasureSigma { get; set; }
        public double MeasureThreshold { get; set; }
    }
}