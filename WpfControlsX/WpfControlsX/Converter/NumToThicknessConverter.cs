using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlsX.Converter
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/25 12:24:44
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/25 12:24:44                     BigWang         首次编写         
    ///
    public class NumToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double flag = parameter == null ? 1: double.Parse(parameter.ToString());
            return new Thickness(flag * (double)value, 0, flag * (double)value, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}