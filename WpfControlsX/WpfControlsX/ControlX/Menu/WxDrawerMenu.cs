using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxDrawerMenu : ContentControl
    {
        static WxDrawerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxDrawerMenu), new FrameworkPropertyMetadata(typeof(WxDrawerMenu)));
        }



        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxDrawerMenu), new PropertyMetadata(null));



        /// <summary>
        /// 菜单项
        /// </summary>
        public new List<WxDrawerMenuItem> Content
        {
            get => (List<WxDrawerMenuItem>)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        public static new readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(List<WxDrawerMenuItem>), typeof(WxDrawerMenu), new FrameworkPropertyMetadata(null));



        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(WxDrawerMenu), new PropertyMetadata(true));


        public override void BeginInit()
        {
            Content = new List<WxDrawerMenuItem>();
            base.BeginInit();
        }
    }
}