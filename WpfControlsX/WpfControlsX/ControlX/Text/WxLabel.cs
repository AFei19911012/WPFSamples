using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxLabel : Label
    {
        static WxLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxLabel), new FrameworkPropertyMetadata(typeof(WxLabel)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxLabel), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxLabel), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxLabel), new PropertyMetadata(10d));


        /// <summary>
        /// 标签类型
        /// </summary>
        public LabelType LabelType
        {
            get => (LabelType)GetValue(LabelTypeProperty);
            set => SetValue(LabelTypeProperty, value);
        }

        public static readonly DependencyProperty LabelTypeProperty =
            DependencyProperty.Register("LabelType", typeof(LabelType), typeof(WxLabel), new PropertyMetadata(LabelType.Normal));
    }
}