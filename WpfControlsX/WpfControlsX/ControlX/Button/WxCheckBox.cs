using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxCheckBox : CheckBox
    {
        static WxCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxCheckBox), new FrameworkPropertyMetadata(typeof(WxCheckBox)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxCheckBox), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxCheckBox), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxCheckBox), new PropertyMetadata(10d));


        /// <summary>
        /// 类型
        /// </summary>
        public CheckBoxType CheckBoxType
        {
            get => (CheckBoxType)GetValue(CheckBoxTypeProperty);
            set => SetValue(CheckBoxTypeProperty, value);
        }

        public static readonly DependencyProperty CheckBoxTypeProperty =
            DependencyProperty.Register("CheckBoxType", typeof(CheckBoxType), typeof(WxCheckBox), new PropertyMetadata(CheckBoxType.Fill));
    }
}