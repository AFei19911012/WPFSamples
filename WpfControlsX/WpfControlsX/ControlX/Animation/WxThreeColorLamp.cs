using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxThreeColorLamp : ContentControl
    {
        static WxThreeColorLamp()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxThreeColorLamp), new FrameworkPropertyMetadata(typeof(WxThreeColorLamp)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxThreeColorLamp), new PropertyMetadata(new CornerRadius(60d)));


        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart
        {
            get => (bool)GetValue(IsStartProperty);
            set => SetValue(IsStartProperty, value);
        }
        public static readonly DependencyProperty IsStartProperty =
            DependencyProperty.Register("IsStart", typeof(bool), typeof(WxThreeColorLamp), new PropertyMetadata(true));


        /// <summary>
        /// 闪灯类型
        /// </summary>
        public ThreeColorLampType ThreeColorLampType
        {
            get => (ThreeColorLampType)GetValue(ThreeColorLampTypeProperty);
            set => SetValue(ThreeColorLampTypeProperty, value);
        }
        public static readonly DependencyProperty ThreeColorLampTypeProperty =
            DependencyProperty.Register("ThreeColorLampType", typeof(ThreeColorLampType), typeof(WxThreeColorLamp), new PropertyMetadata(ThreeColorLampType.Warning));


        /// <summary>
        /// 闪灯颜色
        /// </summary>
        public Color LampColor
        {
            get => (Color)GetValue(LampColorProperty);
            set => SetValue(LampColorProperty, value);
        }
        public static readonly DependencyProperty LampColorProperty =
            DependencyProperty.Register("LampColor", typeof(Color), typeof(WxThreeColorLamp), new PropertyMetadata(Colors.Transparent));


    }
}