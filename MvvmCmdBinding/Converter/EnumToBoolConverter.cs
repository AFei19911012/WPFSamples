using MvvmCmdBinding.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MvvmCmdBinding.Converter
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/8/8 20:31:31
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/8/8 20:31:31    Taosy.W                 
    ///
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Gender mode = (Gender)value;
            return mode == (Gender)int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (!isChecked)
            {
                return null;
            }

            return (Gender)int.Parse(parameter.ToString());
        }
    }
}
