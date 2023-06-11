using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxMenuItem : MenuItem
    {
        static WxMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxMenuItem), new FrameworkPropertyMetadata(typeof(WxMenuItem)));
        }

        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Iconfont
        {
            get => (Geometry)GetValue(IconfontProperty);
            set => SetValue(IconfontProperty, value);
        }
        public static readonly DependencyProperty IconfontProperty =
            DependencyProperty.Register("Iconfont", typeof(Geometry), typeof(WxMenuItem), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxMenuItem), new PropertyMetadata(10d));


        /// <summary>
        /// Header 宽度
        /// </summary>
        public double HeaderWidth
        {
            get => (double)GetValue(HeaderWidthProperty);
            set => SetValue(HeaderWidthProperty, value);
        }
        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(double), typeof(WxMenuItem), new PropertyMetadata(24d));


    }
}