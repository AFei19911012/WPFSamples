using System.ComponentModel;

namespace HalconWPF.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/7 0:45:35
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/7 0:45:35    Taosy.W                 
    ///
    public enum EnumModuleType
    {
        [Description("矩形")]
        rectangle = 0,
        [Description("椭圆")]
        ellipse,
        [Description("圆")]
        circle,
        [Description("多边形")]
        polygon,
    }
}