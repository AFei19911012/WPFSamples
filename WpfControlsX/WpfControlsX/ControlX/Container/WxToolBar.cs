using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxToolBar : ToolBar
    {
        static WxToolBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxToolBar), new FrameworkPropertyMetadata(typeof(WxToolBar)));
        }

        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxToolBar), new PropertyMetadata(new CornerRadius(0)));
    }
}