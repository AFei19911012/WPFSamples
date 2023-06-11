using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2023 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2023/2/21 2:26:14
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     2023/2/21 2:26:14    CoderMan/CoderMan1012                 
    ///
    public class WxButton : Button
    {
        static WxButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxButton), new FrameworkPropertyMetadata(typeof(WxButton)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxButton), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxButton), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxButton), new PropertyMetadata(16d));


        /// <summary>
        /// 彩色图标
        /// </summary>
        public DrawingImage CIcon
        {
            get => (DrawingImage)GetValue(CIconProperty);
            set => SetValue(CIconProperty, value);
        }
        public static readonly DependencyProperty CIconProperty =
            DependencyProperty.Register("CIcon", typeof(DrawingImage), typeof(WxButton), new PropertyMetadata(null));


        /// <summary>
        /// 图标和文字排布方式
        /// </summary>
        public bool IsVertical
        {
            get => (bool)GetValue(IsVerticalProperty);
            set => SetValue(IsVerticalProperty, value);
        }

        public static readonly DependencyProperty IsVerticalProperty =
            DependencyProperty.Register("IsVertical", typeof(bool), typeof(WxButton), new PropertyMetadata(false));
    }
}