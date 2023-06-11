using System.Globalization;
using System.Windows.Controls;

namespace WpfControlsX.Validation
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/2/23 23:51:54
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/2/23 23:51:54                     BigWang         首次编写         
    ///
    public class NumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null
                ? new ValidationResult(false, "不能为空")
                : !double.TryParse(value.ToString(), out _) ? new ValidationResult(false, "输入数值") : new ValidationResult(true, null);
        }
    }
}