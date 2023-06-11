using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Wpf_Base.CcdWpf.Converter
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2022/10/30 18:08:39
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     2022/10/30 18:08:39    CoderMan/CoderMan1012                 
    ///
    public class StringToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ResourceDictionary geometry = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/../Theme/Geometry.xaml")
            };

            List<string> names = new List<string>();
            foreach (object item in geometry.Keys)
            {
                names.Add((string)item);
            }

            List<Geometry> paths = new List<Geometry>();
            foreach (object item in geometry.Values)
            {
                paths.Add((Geometry)item);
            }

            int idx = names.IndexOf((string)value);
            PathGeometry path = new PathGeometry();
            path.AddGeometry(paths[idx]);
            // 设置填充规则 很重要
            path.FillRule = FillRule.Nonzero;
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}