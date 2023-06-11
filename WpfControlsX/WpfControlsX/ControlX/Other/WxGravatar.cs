using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/4/18 22:59:06
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/4/18 22:59:06                     BigWang         首次编写         
    ///
    public class WxGravatar : ContentControl
    {
        public static readonly DependencyProperty GeneratorProperty = DependencyProperty.Register(
            nameof(Generator), typeof(IGravatarGenerator), typeof(WxGravatar), new PropertyMetadata(new GithubGravatarGenerator()));

        public IGravatarGenerator Generator
        {
            get => (IGravatarGenerator)GetValue(GeneratorProperty);
            set => SetValue(GeneratorProperty, value);
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            nameof(Id), typeof(string), typeof(WxGravatar), new PropertyMetadata(default(string), OnIdChanged));

        private static void OnIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WxGravatar ctl = (WxGravatar)d;
            if (ctl.Source != null)
            {
                return;
            }

            ctl.Content = ctl.Generator.GetGravatar((string)e.NewValue);
        }

        public string Id
        {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(ImageSource), typeof(WxGravatar), new PropertyMetadata(default(ImageSource), OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WxGravatar ctl = (WxGravatar)d;
            ImageSource v = (ImageSource)e.NewValue;

            if (v != null)
            {
                ctl.Background = new ImageBrush(v)
                {
                    Stretch = Stretch.UniformToFill
                };
            }
        }

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxGravatar), new PropertyMetadata(new CornerRadius(0)));
    }
}