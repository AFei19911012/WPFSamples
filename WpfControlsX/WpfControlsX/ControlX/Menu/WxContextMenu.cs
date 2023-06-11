using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxContextMenu : ContextMenu
    {
        static WxContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxContextMenu), new FrameworkPropertyMetadata(typeof(WxContextMenu)));
        }
    }
}