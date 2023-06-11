using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfControlsX.ControlX
{
    public class WxPath : Shape
    {
        static WxPath()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxPath), new FrameworkPropertyMetadata(typeof(WxPath)));
        }

        protected override Geometry DefiningGeometry => Icon ?? Geometry.Empty;

        /// </// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxPath), new PropertyMetadata(null));

        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxPath), new PropertyMetadata(16d));
    }
}