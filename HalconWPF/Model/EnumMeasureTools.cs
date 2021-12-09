using System.ComponentModel;

namespace HalconWPF.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/9 19:27:02
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/9 19:27:02    Taosy.W                 
    ///
    public enum EnumMeasureTools
    {
        [Description("测量距离")]
        distance = 0,
        [Description("测量角度")]
        angle,
    }
}