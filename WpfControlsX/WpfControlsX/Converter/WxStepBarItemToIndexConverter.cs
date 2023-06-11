using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using WpfControlsX.ControlX;

namespace WpfControlsX.Converter
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/4/4 22:50:42
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/4/4 22:50:42                     BigWang         首次编写         
    ///
    public class WxStepBarItemToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            WxStepBarItem item = (WxStepBarItem)value;
            WxStepBar step = ItemsControl.ItemsControlFromItemContainer(item) as WxStepBar;
            int index = step.ItemContainerGenerator.IndexFromContainer(item)
                        + 1;
            item.Index = index;
            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}