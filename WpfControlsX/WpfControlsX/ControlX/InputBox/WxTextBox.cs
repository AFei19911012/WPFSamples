using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxTextBox : TextBox
    {
        static WxTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxTextBox), new FrameworkPropertyMetadata(typeof(WxTextBox)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxTextBox), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxTextBox), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxTextBox), new PropertyMetadata(10d));

        /// <summary>
        /// 左侧标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxTextBox), new PropertyMetadata(null));

        /// <summary>
        /// 左侧标题宽度
        /// </summary>
        public double TitleWidth
        {
            get => (double)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }
        public static readonly DependencyProperty TitleWidthProperty =
            DependencyProperty.Register("TitleWidth", typeof(double), typeof(WxTextBox), new PropertyMetadata(double.NaN));


        /// <summary>
        /// 左侧标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(WxTextBox), new PropertyMetadata(Brushes.Transparent));


        /// <summary>
        /// 存在文本内容
        /// </summary>
        public bool HasText
        {
            get => (bool)GetValue(HasTextProperty);
            set => SetValue(HasTextProperty, value);
        }
        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.Register("HasText", typeof(bool), typeof(WxTextBox), new PropertyMetadata());


        /// <summary>
        /// 显示清除按钮
        /// </summary>
        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }
        public static readonly DependencyProperty ShowClearButtonProperty =
            DependencyProperty.Register("ShowClearButton", typeof(bool), typeof(WxTextBox), new PropertyMetadata(true));


        /// <summary>
        /// 显示水印
        /// </summary>
        public bool ShowWaterMark
        {
            get => (bool)GetValue(ShowWaterMarkProperty);
            set => SetValue(ShowWaterMarkProperty, value);
        }
        public static readonly DependencyProperty ShowWaterMarkProperty =
            DependencyProperty.Register("ShowWaterMark", typeof(bool), typeof(WxTextBox), new PropertyMetadata(true));

        /// <summary>
        /// 水印内容
        /// </summary>
        public string WaterMark
        {
            get => (string)GetValue(WaterMarkProperty);
            set => SetValue(WaterMarkProperty, value);
        }
        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(WxTextBox), new PropertyMetadata(null));


        private ButtonBase ClearButtonHost { get; set; }

        /// <summary>
        /// 应用控件模板时调用
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 取消订阅 TemplatePart 事件
            if (ClearButtonHost != null)
            {
                ClearButtonHost.Click -= OnClearButtonClick;
            }
            ClearButtonHost = GetTemplateChild("PART_ClearButtonHost") as ButtonBase;
            // 订阅 TemplatePart 事件
            if (ClearButtonHost != null)
            {
                ClearButtonHost.Click += OnClearButtonClick;
            }
        }

        /// <summary>
        /// 清除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            Text = string.Empty;
        }

        /// <summary>
        /// 重写事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            HasText = !string.IsNullOrEmpty(Text);
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