using HalconDotNet;

namespace Wpf_Base.CcdWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 19:34:43
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:34:43    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CHalCameraInfo
    {
        public string CcdName { get; set; } = "";
        public bool IsOpened { get; set; } = false;
        public HTuple Hv_AcqHandle = new HTuple();
    }
}