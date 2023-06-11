using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxComboBox : ComboBox
    {
        static WxComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxComboBox), new FrameworkPropertyMetadata(typeof(WxComboBox)));
        }


        /// <summary>
        /// 左侧标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxComboBox), new PropertyMetadata(null));

        /// <summary>
        /// 左侧标题宽度
        /// </summary>
        public double TitleWidth
        {
            get => (double)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }
        public static readonly DependencyProperty TitleWidthProperty =
            DependencyProperty.Register("TitleWidth", typeof(double), typeof(WxComboBox), new PropertyMetadata(double.NaN));


        /// <summary>
        /// 左侧标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(WxComboBox), new PropertyMetadata(Brushes.Transparent));
    }
}