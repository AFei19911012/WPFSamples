using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxGroupBox : GroupBox
    {
        static WxGroupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxGroupBox), new FrameworkPropertyMetadata(typeof(WxGroupBox)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxGroupBox), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxGroupBox), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxGroupBox), new PropertyMetadata(10d));


        /// <summary>
        /// 标题位置
        /// </summary>
        public GroupBoxTitlePosition TitlePosition
        {
            get => (GroupBoxTitlePosition)GetValue(TitlePositionProperty);
            set => SetValue(TitlePositionProperty, value);
        }
        public static readonly DependencyProperty TitlePositionProperty =
            DependencyProperty.Register("TitlePosition", typeof(GroupBoxTitlePosition), typeof(WxGroupBox), new PropertyMetadata(GroupBoxTitlePosition.Top));


    }
}