namespace Wpf_Base.LogWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/10/01 16:10:57
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/10/01 16:10:57    CoderMan/CoderdMan1012         首次编写         
    ///
    public class DataModel
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }
    }
}