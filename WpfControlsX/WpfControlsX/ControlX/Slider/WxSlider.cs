using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxSlider : Slider
    {
        static WxSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxSlider), new FrameworkPropertyMetadata(typeof(WxSlider)));
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
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxSlider), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxSlider), new PropertyMetadata(10d));


        /// <summary>
        /// 滑块背景颜色
        /// </summary>
        public Brush TrackBackground
        {
            get => (Brush)GetValue(TrackBackgroundProperty);
            set => SetValue(TrackBackgroundProperty, value);
        }
        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.Register("TrackBackground", typeof(Brush), typeof(WxSlider), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 滑动条高度
        /// </summary>
        public double SliderHeight
        {
            get => (double)GetValue(SliderHeightProperty);
            set => SetValue(SliderHeightProperty, value);
        }
        public static readonly DependencyProperty SliderHeightProperty =
            DependencyProperty.Register("SliderHeight", typeof(double), typeof(WxSlider), new PropertyMetadata(5d));

    }
}