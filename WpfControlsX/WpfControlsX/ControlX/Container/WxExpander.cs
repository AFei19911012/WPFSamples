using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxExpander : Expander
    {
        static WxExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxExpander), new FrameworkPropertyMetadata(typeof(WxExpander)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxExpander), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxExpander), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxExpander), new PropertyMetadata(10d));
    }
}