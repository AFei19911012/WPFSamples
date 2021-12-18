using System.ComponentModel;

namespace HalconWPF.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/12 0:02:17
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/12 0:02:17    Taosy.W                 
    ///
    public enum EnumModuleEditState
    {
        [Description("null")]
        none = 0,
        [Description("制作ROI")]
        make_roi,
        [Description("编辑ROI")]
        edit_roi,
        [Description("制作模板")]
        make_module,
    }
}