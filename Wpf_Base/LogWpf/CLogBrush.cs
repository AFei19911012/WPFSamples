using System.Windows.Media;

namespace Wpf_Base.LogWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 11:17:32
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 11:17:32    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class CLogBrush
    {
        // 日志类型颜色
        public static Color ColorLogDebug { get; set; } = Colors.Black;
        public static Color ColorLogInfo { get; set; } = Color.FromArgb(0xFF, 0x00, 0xBC, 0xD4);
        public static Color ColorLogWarning { get; set; } = Color.FromArgb(0xFF, 0xE9, 0xAF, 0x20);
        public static Color ColorLogSuccess { get; set; } = Color.FromArgb(0xFF, 0x2D, 0xB8, 0x4D);
        public static Color ColorLogError { get; set; } = Color.FromArgb(0xFF, 0xDB, 0x33, 0x40);
        public static Brush BrushLogDebug { get; set; } = new SolidColorBrush(ColorLogDebug);
        public static Brush BrushLogInfo { get; set; } = new SolidColorBrush(ColorLogInfo);
        public static Brush BrushLogWarning { get; set; } = new SolidColorBrush(ColorLogWarning);
        public static Brush BrushLogSuccess { get; set; } = new SolidColorBrush(ColorLogSuccess);
        public static Brush BrushLogError { get; set; } = new SolidColorBrush(ColorLogError);
    }
}