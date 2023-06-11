using System.Windows.Media;

namespace Wpf_Base.ControlsWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/07 23:07:35
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/07 23:07:35    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CPlotInfo
    {
        /// <summary>
        /// 图例名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public Brush Fill { get; set; }

        /// <summary>
        /// 图例颜色标记宽度 防止字体过大导致显示不全
        /// </summary>
        public int Width { get; set; } = 50;
    }
}