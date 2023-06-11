using System.ComponentModel;

namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 13:08:50
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 13:08:50    CoderMan/CoderdMan1012         首次编写         
    ///
    public enum EnumModuleEditState
    {
        [Description("null")]
        none = 0,
        [Description("选择ROI区域")]
        choose_roi,
        [Description("编辑ROI区域")]
        edit_roi,
        [Description("选择模板区域")]
        choose_module,
    }
}