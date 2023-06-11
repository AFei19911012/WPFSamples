using System.Windows.Media;

namespace Wpf_Base.CcdWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/31 10:46:30
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/31 10:46:30    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class CCcdIcon
    {
        // CCD 一些显示属性
        public static Brush CcdBrushConnected { get; set; } = Brushes.Red;
        public static Brush CcdBrushDisConnected { get; set; } = Brushes.Black;
        public static string IconCcdConnected { get; set; } = "IconConnected";
        public static string IconCcdConnectedOff { get; set; } = "IconConnectedOff";
        public static string IconCcdConnectOn { get; set; } = "IconConnectOn";
        public static string IconCcdConnectOff { get; set; } = "IconConnectOff";
        public static string IconUsb { get; set; } = "IconUsb";
        public static string IconGige { get; set; } = "IconVideo";
    }
}