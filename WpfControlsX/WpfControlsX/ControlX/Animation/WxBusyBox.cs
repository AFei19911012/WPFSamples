using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfControlsX.ControlX
{
    public class WxBusyBox : ProgressBar
    {
        static WxBusyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxBusyBox), new FrameworkPropertyMetadata(typeof(WxBusyBox)));
        }

        /// <summary>
        /// 类型
        /// </summary>
        public BusyBoxType BusyBoxType
        {
            get => (BusyBoxType)GetValue(BusyBoxTypeProperty);
            set => SetValue(BusyBoxTypeProperty, value);
        }

        public static readonly DependencyProperty BusyBoxTypeProperty =
            DependencyProperty.Register("BusyBoxType", typeof(BusyBoxType), typeof(WxBusyBox), new PropertyMetadata(BusyBoxType.Bar));


        /// <summary>
        /// 显示文字
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WxBusyBox), new PropertyMetadata(null));


        /// <summary>
        /// 状态
        /// </summary>
        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(WxBusyBox), new PropertyMetadata(true));


        /// <summary>
        /// 应用控件模板时调用
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_Canvas") is Canvas cs)
            {
                double r = 100;
                double da = 2 * Math.PI / 9;

                for (int i = 0; i < cs.Children.Count; i++)
                {
                    double left = r + (r * Math.Sin(i * da));
                    double top = r - (r * Math.Cos(i * da));
                    Ellipse ellipse = cs.Children[i] as Ellipse;
                    ellipse.SetValue(Canvas.LeftProperty, left);
                    ellipse.SetValue(Canvas.TopProperty, top);
                }
            }
        }
    }
}