using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/2 9:59:13
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/2 9:59:13                     BigWang         首次编写         
    ///
    public class WxTextBlock : TextBlock
    {
        static WxTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxTextBlock), new FrameworkPropertyMetadata(typeof(WxTextBlock)));
        }
    }
}