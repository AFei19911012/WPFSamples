using System;

namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 13:18:52
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 13:18:52    CoderMan/CoderdMan1012         首次编写         
    ///
    [Serializable]
    public class COcrProjectParams
    {
        // 字符识别参数
        public int IntSelectOCR { get; set; }
        public int MinCharHeight { get; set; }
        public int MinCharWidth { get; set; }
        public int NumRow1 { get; set; }
        public int NumCol1 { get; set; }
        public int NumRow2 { get; set; }
        public int NumCol2 { get; set; }

        // 配方名称
        public string StrRecipeName { get; set; }
    }
}