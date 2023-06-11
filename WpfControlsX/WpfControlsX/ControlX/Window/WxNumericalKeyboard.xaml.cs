using System.Windows;

namespace WpfControlsX.ControlX
{
    /// <summary>
    /// WxNumericalKeyboard.xaml 的交互逻辑
    /// </summary>
    public partial class WxNumericalKeyboard : Window
    {
        /// <summary>
        /// 当前值
        /// </summary>
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(WxNumericalKeyboard), new PropertyMetadata(null));

        /// <summary>
        /// 最小值
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(WxNumericalKeyboard), new PropertyMetadata(0d));

        /// <summary>
        /// 最大值
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(WxNumericalKeyboard), new PropertyMetadata(100d));

        /// <summary>
        /// 数据类型：浮点数、整数
        /// </summary>
        public bool IsDouble
        {
            get { return (bool)GetValue(IsDoubleProperty); }
            set { SetValue(IsDoubleProperty, value); }
        }
        public static readonly DependencyProperty IsDoubleProperty =
            DependencyProperty.Register("IsDouble", typeof(bool), typeof(WxNumericalKeyboard), new PropertyMetadata(true));


        public double Result { get; set; } = 0;

        public WxNumericalKeyboard(double value)
        {
            InitializeComponent();

            Value = value.ToString();
            Result = value;
        }

        private void WxButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WxButtonBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (Value.Length > 1)
            {
                Value = Value.Substring(0, Value.Length - 1);
            }
            else
            {
                Value = "";
            }
        }

        private void WxButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Value = "";
        }

        private void WxButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Value, out double result))
            {
                Result = result;
            }
            Close();
        }

        private void WxButton_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as WxButton).Content.ToString();
            name = name.Replace("·", ".");
            Value += name;

            // 浮点数
            if (IsDouble)
            {
                // 首部不能连续两个 0
                if (Value.StartsWith("00"))
                {
                    WxButtonBackspace_Click(null, null);
                }

                // 首部不能单独 0
                if (Value.StartsWith("0") && !Value.StartsWith("0."))
                {
                    Value = Value.Substring(1, Value.Length - 1);
                }

                // 只能一个小数点
                int idx1 = Value.IndexOf(".");
                int idx2 = Value.LastIndexOf(".");
                if (idx1 != idx2)
                {
                    WxButtonBackspace_Click(null, null);
                }

                // 保留两位小数
                int idx = Value.IndexOf(".");
                if (idx > 0 && idx < Value.Length - 3)
                {
                    WxButtonBackspace_Click(null, null);
                }
            }
            // 整数
            else
            {
                // 整数不能 0 和 . 开头
                if (Value.StartsWith("0") || Value.StartsWith("."))
                {
                    Value = Value.Substring(1, Value.Length - 1);
                }

                // 整数不能有小数点
                if (Value.Contains("."))
                {
                    WxButtonBackspace_Click(null, null);
                }
            }

            // 尾部不能有 -
            if (Value.Length > 1 && Value.EndsWith("-"))
            {
                WxButtonBackspace_Click(null, null);
            }

            // 限定范围
            if (double.TryParse(Value, out double result))
            {
                if (result < Minimum || result > Maximum)
                {
                    WxButtonBackspace_Click(null, null);
                }
            }
        }
    }
}