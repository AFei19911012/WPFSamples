using System.Collections.Generic;
using System.Windows.Media;

namespace Wpf_Base.ControlsWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/08 14:05:15
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/08 14:05:15    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CPlotConstant
    {
        /// <summary>
        /// 默认颜色模板：9 种颜色
        /// </summary>
        public static List<Brush> Default { get; set; } = new List<Brush>
        {
             new SolidColorBrush(Color.FromArgb(0xFF, 0x54, 0x70, 0xC6)),
             new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0x66, 0x66)),
             new SolidColorBrush(Color.FromArgb(0xFF, 0x91, 0xCC, 0x75)),
             new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xC8, 0x58)),
             new SolidColorBrush(Color.FromArgb(0xFF, 0x73, 0xC0, 0xDE)),
             new SolidColorBrush(Color.FromArgb(0xFF, 0x3B, 0xA2, 0x72)),
             new SolidColorBrush(Color.FromArgb(0xFF, 0xFC, 0x84, 0x52)),
             new SolidColorBrush(Color.FromArgb(0xFF, 0x9A, 0x60, 0xB4)),
             new SolidColorBrush(Color.FromArgb(0xFF, 0xEA, 0x7C, 0xCC)),
        };


        /// <summary>
        /// 彩虹色
        /// </summary>
        public static List<Brush> RainBow { get; } = new List<Brush>
        {
             Brushes.DodgerBlue,
             Brushes.OrangeRed,
             Brushes.Orange,
             Brushes.LawnGreen,
             Brushes.Cyan,
             Brushes.Magenta,
             Brushes.Yellow,
        };
    }
}