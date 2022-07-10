using HelixToolkit.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Demos.Converter
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2022/7/7 22:23:03
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/7/7 22:23:03    Taosy.W                 
    ///
    [ValueConversion(typeof(Visual3D), typeof(Rect3D))]
    public class BoundsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visual = value as Visual3D;
            return visual != null ? visual.FindBounds(Transform3D.Identity) : Rect3D.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}