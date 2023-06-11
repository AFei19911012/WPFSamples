using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxNeonText : Label
    {
        static WxNeonText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxNeonText), new FrameworkPropertyMetadata(typeof(WxNeonText)));
        }

        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart
        {
            get => (bool)GetValue(IsStartProperty);
            set => SetValue(IsStartProperty, value);
        }
        public static readonly DependencyProperty IsStartProperty =
            DependencyProperty.Register("IsStart", typeof(bool), typeof(WxNeonText), new PropertyMetadata(true));
    }
}