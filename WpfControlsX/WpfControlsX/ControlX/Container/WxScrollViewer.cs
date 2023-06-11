using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxScrollViewer : ScrollViewer
    {
        static WxScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxScrollViewer), new FrameworkPropertyMetadata(typeof(WxScrollViewer)));
        }
    }
}