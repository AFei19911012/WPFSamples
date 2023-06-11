using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    public class WxPasswordBox : Control
    {
        static WxPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxPasswordBox), new FrameworkPropertyMetadata(typeof(WxPasswordBox)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxPasswordBox), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxPasswordBox), new PropertyMetadata(10d));

        /// <summary>
        /// 左侧标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxPasswordBox), new PropertyMetadata(null));

        /// <summary>
        /// 左侧标题宽度
        /// </summary>
        public double TitleWidth
        {
            get => (double)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }
        public static readonly DependencyProperty TitleWidthProperty =
            DependencyProperty.Register("TitleWidth", typeof(double), typeof(WxPasswordBox), new PropertyMetadata(double.NaN));


        /// <summary>
        /// 左侧标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(WxPasswordBox), new PropertyMetadata(Brushes.Transparent));


        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(WxPasswordBox), new PropertyMetadata(null, OnPasswordChanged));

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WxPasswordBox obj = d as WxPasswordBox;
            if (obj.GetTemplateChild("PART_PasswordText") is TextBox txt && obj.GetTemplateChild("PART_Password") is PasswordBox pb)
            {
                txt.Text = (string)obj.GetValue(PasswordProperty);
                pb.Password = (string)obj.GetValue(PasswordProperty);
            }
        }


        /// <summary>
        /// 水印
        /// </summary>
        public string WaterMark
        {
            get => (string)GetValue(WaterMarkProperty);
            set => SetValue(WaterMarkProperty, value);
        }

        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(WxPasswordBox), new PropertyMetadata(null));

        /// <summary>
        /// 显示水印
        /// </summary>
        public bool ShowWaterMark
        {
            get => (bool)GetValue(ShowWaterMarkProperty);
            set => SetValue(ShowWaterMarkProperty, value);
        }

        public static readonly DependencyProperty ShowWaterMarkProperty =
            DependencyProperty.Register("ShowWaterMark", typeof(bool), typeof(WxPasswordBox), new PropertyMetadata(false));

        /// <summary>
        /// 存在文本内容
        /// </summary>
        public bool HasText
        {
            get => (bool)GetValue(HasTextProperty);
            set => SetValue(HasTextProperty, value);
        }
        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.Register("HasText", typeof(bool), typeof(WxPasswordBox), new PropertyMetadata(false));


        /// <summary>
        /// 密码字符
        /// </summary>
        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(WxPasswordBox), new PropertyMetadata('●'));


        private TextBox PART_PasswordText { get; set; }
        private PasswordBox PART_Password { get; set; }

        /// <summary>
        /// 应用控件模板时调用
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_PasswordText = GetTemplateChild("PART_PasswordText") as TextBox;
            PART_Password = GetTemplateChild("PART_Password") as PasswordBox;

            if (PART_PasswordText != null && PART_Password != null)
            {
                PART_PasswordText.TextChanged += PArt_Password_TextChanged;
                PART_Password.PasswordChanged += PART_Password_PasswordChanged;

                PART_Password.Password = Password;
                // 是否为空
                HasText = !string.IsNullOrEmpty(Password);
            }
        }

        private void PART_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PART_Password.Password;
            PART_PasswordText.Text = Password;

            // 光标移动至最后
            PART_Password.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(PART_Password, new object[] { PART_Password.Password.Length, 0 });
        }

        private void PArt_Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password = PART_PasswordText.Text;
            PART_Password.Password = Password;

            // 是否为空
            HasText = !string.IsNullOrEmpty(Password);
        }
    }
}