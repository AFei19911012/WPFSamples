using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/2/28 22:08:51
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/2/28 22:08:51                     BigWang         首次编写         
    ///
    public class WxListView : ListView
    {
        static WxListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxListView), new FrameworkPropertyMetadata(typeof(WxListView)));
        }
    }
}