using System.ComponentModel;

namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 19:54:41
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:54:41    CoderMan/CoderdMan1012         首次编写         
    ///
    public enum EnumMetrologyObjectType
    {
        [Description("矩形")]
        rectangle = 0,
        [Description("椭圆")]
        ellipse,
        [Description("圆")]
        circle,
        [Description("直线")]
        line,
    }
}