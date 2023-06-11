using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxMenu : Menu
    {
        static WxMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxMenu), new FrameworkPropertyMetadata(typeof(WxMenu)));
        }
    }
}