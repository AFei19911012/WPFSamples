using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxToggleButton : ToggleButton
    {
        static WxToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxToggleButton), new FrameworkPropertyMetadata(typeof(WxToggleButton)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxToggleButton), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxToggleButton), new PropertyMetadata(null));


        /// <summary>
        /// 选中时图标
        /// </summary>

        public Geometry IconOn
        {
            get => (Geometry)GetValue(IconOnProperty);
            set => SetValue(IconOnProperty, value);
        }
        public static readonly DependencyProperty IconOnProperty =
            DependencyProperty.Register("IconOn", typeof(Geometry), typeof(WxToggleButton), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxToggleButton), new PropertyMetadata(10d));

        /// <summary>
        /// 中间文本
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WxToggleButton), new PropertyMetadata(null));


        /// <summary>
        /// 选中时文本
        /// </summary>
        public string TextOn
        {
            get => (string)GetValue(TextOnProperty);
            set => SetValue(TextOnProperty, value);
        }
        public static readonly DependencyProperty TextOnProperty =
            DependencyProperty.Register("TextOn", typeof(string), typeof(WxToggleButton), new PropertyMetadata(null));



        /// <summary>
        /// 类型
        /// </summary>
        public ToggleButtonType ToggleButtonType
        {
            get => (ToggleButtonType)GetValue(ToggleButtonTypeProperty);
            set => SetValue(ToggleButtonTypeProperty, value);
        }
        public static readonly DependencyProperty ToggleButtonTypeProperty =
            DependencyProperty.Register("ToggleButtonType", typeof(ToggleButtonType), typeof(WxToggleButton), new PropertyMetadata(ToggleButtonType.Base));


        /// <summary>
        /// 是否垂直
        /// </summary>
        public bool IsVertical
        {
            get => (bool)GetValue(IsVerticalProperty);
            set => SetValue(IsVerticalProperty, value);
        }
        public static readonly DependencyProperty IsVerticalProperty =
            DependencyProperty.Register("IsVertical", typeof(bool), typeof(WxToggleButton), new PropertyMetadata(false));


    }
}