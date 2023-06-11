using System.ComponentModel;

namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 12:51:52
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 12:51:52    CoderMan/CoderdMan1012         首次编写         
    ///
    public enum EnumMoveType
    {
        [Description("XY运动")]
        move_xy = 0,
        [Description("旋转运动")]
        move_rotation,
        [Description("XY+旋转运动")]
        move_xy_rotation,
    }
}