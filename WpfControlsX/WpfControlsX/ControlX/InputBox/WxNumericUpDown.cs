using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxNumericUpDown : ScrollBar
    {
        static WxNumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxNumericUpDown), new FrameworkPropertyMetadata(typeof(WxNumericUpDown)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxNumericUpDown), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxNumericUpDown), new PropertyMetadata(10d));


        /// <summary>
        /// 左侧标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxNumericUpDown), new PropertyMetadata(null));

        /// <summary>
        /// 左侧标题宽度
        /// </summary>
        public double TitleWidth
        {
            get => (double)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }
        public static readonly DependencyProperty TitleWidthProperty =
            DependencyProperty.Register("TitleWidth", typeof(double), typeof(WxNumericUpDown), new PropertyMetadata(double.NaN));


        /// <summary>
        /// 左侧标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(WxNumericUpDown), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 数据精度
        /// </summary>
        public int Precision
        {
            get => (int)GetValue(PrecisionProperty);
            set => SetValue(PrecisionProperty, value);
        }

        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register("Precision", typeof(int), typeof(WxNumericUpDown), new PropertyMetadata(2));


        /// <summary>
        /// 应用控件模板时调用
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_ButtonUp") is WxRepeatButton btn_up
                && GetTemplateChild("PART_ButtonDown") is WxRepeatButton btn_down
                && GetTemplateChild("PART_Border") is Border border)
            {
                btn_up.Click += Btn_up_Click;
                btn_down.Click += Btn_down_Click;
                border.MouseWheel += Border_MouseWheel;
            }
        }

        private void Border_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Value = e.Delta > 0
                ? Value + LargeChange > Maximum ? Math.Round(Maximum, Precision) : Math.Round(Value + LargeChange, Precision)
                : Value - LargeChange < Minimum ? Math.Round(Minimum, Precision) : Math.Round(Value - LargeChange, Precision);
        }

        private void Btn_down_Click(object sender, RoutedEventArgs e)
        {
            Value = Value - SmallChange < Minimum ? Math.Round(Minimum, Precision) : Math.Round(Value - SmallChange, Precision);
        }

        private void Btn_up_Click(object sender, RoutedEventArgs e)
        {
            Value = Value + SmallChange > Maximum ? Math.Round(Maximum, Precision) : Math.Round(Value + SmallChange, Precision);
        }

        /// <summary>
        /// 禁用自动弹出虚拟键盘
        /// </summary>
        /// <returns></returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FrameworkElementAutomationPeer(this);
        }
    }
}