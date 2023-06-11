using System.Windows.Media;

namespace Wpf_Base.MethodNet
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 19:51:21
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:51:21    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class CConstants
    {
        // 绘制对象属性
        public static Color ColorDefault { get; set; } = Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00);
        public static Color ColorMask { get; set; } = Color.FromArgb(0x33, 0x07, 0xAA, 0xE5);
        public static Color ColorMaskStroke { get; set; } = Colors.SpringGreen;
        public static Brush BrushDefault { get; set; } = new SolidColorBrush(ColorDefault);
        public static Brush BrushMask { get; set; } = new SolidColorBrush(ColorMask);
        public static Brush BrushMaskStroke { get; set; } = Brushes.SpringGreen;
        public static double InkStrokeThickness { get; set; } = 10;
        public const double MarkerCrossLenght = 50;
    }
}