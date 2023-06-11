using System.Globalization;
using System.Windows.Controls;

namespace WpfControlsX.Validation
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/2/23 23:45:11
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/2/23 23:45:11                     BigWang         首次编写         
    ///
    public class GreaterRule : ValidationRule
    {
        public double? Number { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null || Number == null
                ? new ValidationResult(false, "丢失相比较的数值")
                : !double.TryParse(value.ToString(), out double v)
                ? new ValidationResult(false, "输入数值")
                : v > Number ? new ValidationResult(true, null) : new ValidationResult(false, string.Format("输入值必需大于{0}", Number));
        }
    }
}