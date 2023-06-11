using System.Globalization;
using System.Windows.Controls;

namespace WpfControlsX.Validation
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/2/23 23:40:53
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/2/23 23:40:53                     BigWang         首次编写         
    ///
    public class NotNullRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null
                ? new ValidationResult(false, "不能为空")
                : string.IsNullOrEmpty(value.ToString()) ? new ValidationResult(false, "不能为空字符串") : new ValidationResult(true, null);
        }
    }
}