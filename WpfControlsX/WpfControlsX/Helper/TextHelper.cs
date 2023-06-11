using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/29 19:26:36
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/29 19:26:36                     BigWang         首次编写         
    ///
    public class TextHelper
    {
        public static FormattedText CreateFormattedText(string text, FlowDirection flowDirection, Typeface typeface, double fontSize)
        {
            FormattedText formattedText = new FormattedText(
                text,
                CultureInfo.CurrentUICulture,
                flowDirection,
                typeface,
                fontSize, Brushes.Black, DpiHelper.DeviceDpiX);
            return formattedText;
        }
    }
}