using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxRepeatButton : RepeatButton
    {
        static WxRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxRepeatButton), new FrameworkPropertyMetadata(typeof(WxRepeatButton)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxRepeatButton), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxRepeatButton), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxRepeatButton), new PropertyMetadata(10d));

        /// <summary>
        /// 按钮类型
        /// </summary>
        public ButtonType ButtonType
        {
            get => (ButtonType)GetValue(ButtonTypeProperty);
            set => SetValue(ButtonTypeProperty, value);
        }

        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType", typeof(ButtonType), typeof(WxRepeatButton), new PropertyMetadata(ButtonType.IconText));


        public bool IsVertical
        {
            get => (bool)GetValue(IsVerticalProperty);
            set => SetValue(IsVerticalProperty, value);
        }

        public static readonly DependencyProperty IsVerticalProperty =
            DependencyProperty.Register("IsVertical", typeof(bool), typeof(WxRepeatButton), new PropertyMetadata(false));
    }
}