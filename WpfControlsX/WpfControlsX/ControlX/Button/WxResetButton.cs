using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2023 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2023/2/20 2:49:56
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     2023/2/20 2:49:56    CoderMan/CoderMan1012                 
    ///
    public class WxResetButton : TextBox
    {
        static WxResetButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxResetButton), new FrameworkPropertyMetadata(typeof(WxResetButton)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxResetButton), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxResetButton), new PropertyMetadata(null));

        /// <summary>
        /// 彩色图标
        /// </summary>
        public DrawingImage CIcon
        {
            get => (DrawingImage)GetValue(CIconProperty);
            set => SetValue(CIconProperty, value);
        }
        public static readonly DependencyProperty CIconProperty =
            DependencyProperty.Register("CIcon", typeof(DrawingImage), typeof(WxResetButton), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxResetButton), new PropertyMetadata(10d));

        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(WxResetButton), new PropertyMetadata(null));



        public bool IsVertical
        {
            get => (bool)GetValue(IsVerticalProperty);
            set => SetValue(IsVerticalProperty, value);
        }

        public static readonly DependencyProperty IsVerticalProperty =
            DependencyProperty.Register("IsVertical", typeof(bool), typeof(WxResetButton), new PropertyMetadata(false));



        /// <summary>
        /// 重写事件
        /// </summary>
        /// <param name="e"></param>
        /// 
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            // 设置为 false 否则不会触发 MouseDown 事件
            e.Handled = false;
        }


        //private void WxResetButton_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    OnWxMouseDown();
        //}

        //// 声明并注册路由事件
        //public static readonly RoutedEvent WxMouseDownEvent =
        //    EventManager.RegisterRoutedEvent("WxMouseDown", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventArgs<object>), typeof(WxResetButton));

        //// 处理路由事件
        //public event RoutedEventHandler WxMouseDown
        //{
        //    add { AddHandler(WxMouseDownEvent, value); }
        //    remove { RemoveHandler(WxMouseDownEvent, value); }
        //}

        //// 激发路由事件
        //protected virtual void OnWxMouseDown()
        //{
        //    RoutedEventArgs args = new RoutedEventArgs(WxMouseDownEvent);
        //    RaiseEvent(args);
        //}
    }
}