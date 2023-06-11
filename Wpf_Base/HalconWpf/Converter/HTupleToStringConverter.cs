﻿using HalconDotNet;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wpf_Base.HalconWpf.Converter
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/20 20:50:54
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/20 20:50:54    CoderMan/CoderdMan1012         首次编写         
    ///
    public class HTupleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Replace("\"", "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new HTuple((string)value);
        }
    }
}