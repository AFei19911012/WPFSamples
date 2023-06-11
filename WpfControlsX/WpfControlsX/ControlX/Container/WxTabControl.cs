using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxTabControl : TabControl
    {
        static WxTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxTabControl), new FrameworkPropertyMetadata(typeof(WxTabControl)));
        }
    }
}