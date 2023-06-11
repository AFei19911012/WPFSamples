using System;
using System.Globalization;
using System.Windows.Data;
using Wpf_Base.HalconWpf.Model;

namespace Wpf_Base.HalconWpf.Converter
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 12:26:41
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 12:26:41    CoderMan/CoderdMan1012         首次编写         
    ///
    public class RobotMovePointToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EnumRobotMovePoint mode = (EnumRobotMovePoint)value;
            return mode == (EnumRobotMovePoint)int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            return !isChecked ? null : (object)(EnumRobotMovePoint)int.Parse(parameter.ToString());
        }
    }
}