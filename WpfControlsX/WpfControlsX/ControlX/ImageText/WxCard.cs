using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxCard : ContentControl
    {
        static WxCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxCard), new FrameworkPropertyMetadata(typeof(WxCard)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxCard), new PropertyMetadata(new CornerRadius(0)));

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(WxCard), new PropertyMetadata(default(object)));


        [Bindable(true), Category("Content")]
        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(WxCard), new PropertyMetadata(default(DataTemplate)));


        [Bindable(true), Category("Content")]
        public DataTemplateSelector HeaderTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty);
            set => SetValue(HeaderTemplateSelectorProperty, value);
        }
        public static readonly DependencyProperty HeaderTemplateSelectorProperty =
            DependencyProperty.Register(nameof(HeaderTemplateSelector), typeof(DataTemplateSelector), typeof(WxCard), new PropertyMetadata(default(DataTemplateSelector)));


        [Bindable(true), Category("Content")]
        public string HeaderStringFormat
        {
            get => (string)GetValue(HeaderStringFormatProperty);
            set => SetValue(HeaderStringFormatProperty, value);
        }
        public static readonly DependencyProperty HeaderStringFormatProperty =
            DependencyProperty.Register(nameof(HeaderStringFormat), typeof(string), typeof(WxCard), new PropertyMetadata(default(string)));


        public object Footer
        {
            get => GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register(nameof(Footer), typeof(object), typeof(WxCard), new PropertyMetadata(default(object)));


        [Bindable(true), Category("Content")]
        public DataTemplate FooterTemplate
        {
            get => (DataTemplate)GetValue(FooterTemplateProperty);
            set => SetValue(FooterTemplateProperty, value);
        }
        public static readonly DependencyProperty FooterTemplateProperty =
            DependencyProperty.Register(nameof(FooterTemplate), typeof(DataTemplate), typeof(WxCard), new PropertyMetadata(default(DataTemplate)));


        [Bindable(true), Category("Content")]
        public DataTemplateSelector FooterTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(FooterTemplateSelectorProperty);
            set => SetValue(FooterTemplateSelectorProperty, value);
        }
        public static readonly DependencyProperty FooterTemplateSelectorProperty =
            DependencyProperty.Register(nameof(FooterTemplateSelector), typeof(DataTemplateSelector), typeof(WxCard), new PropertyMetadata(default(DataTemplateSelector)));


        [Bindable(true), Category("Content")]
        public string FooterStringFormat
        {
            get => (string)GetValue(FooterStringFormatProperty);
            set => SetValue(FooterStringFormatProperty, value);
        }
        public static readonly DependencyProperty FooterStringFormatProperty =
            DependencyProperty.Register(nameof(FooterStringFormat), typeof(string), typeof(WxCard), new PropertyMetadata(default(string)));
    }
}