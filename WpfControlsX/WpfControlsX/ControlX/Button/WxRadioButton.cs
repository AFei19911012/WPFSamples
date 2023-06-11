using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxRadioButton : RadioButton
    {
        static WxRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxRadioButton), new FrameworkPropertyMetadata(typeof(WxRadioButton)));
        }

        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxRadioButton), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxRadioButton), new PropertyMetadata(10d));

        /// <summary>
        /// 类型
        /// </summary>
        public RadioButtonType RadioButtonType
        {
            get => (RadioButtonType)GetValue(RadioButtonTypeProperty);
            set => SetValue(RadioButtonTypeProperty, value);
        }

        public static readonly DependencyProperty RadioButtonTypeProperty =
            DependencyProperty.Register("RadioButtonType", typeof(RadioButtonType), typeof(WxRadioButton), new PropertyMetadata(RadioButtonType.Normal));
    }
}