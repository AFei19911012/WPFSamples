using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/2/28 1:48:02
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/2/28 1:48:02                     BigWang         首次编写         
    ///
    public class WxListBox : ListBox
    {
        static WxListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxListBox), new FrameworkPropertyMetadata(typeof(WxListBox)));
        }
    }
}