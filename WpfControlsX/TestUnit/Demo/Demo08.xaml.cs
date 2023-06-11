using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfControlsX.ControlX;

namespace TestUnit.Demo
{
    /// <summary>
    /// Demo08.xaml 的交互逻辑
    /// </summary>
    public partial class Demo08 : UserControl
    {
        private List<Uri> UriList { get; set; } = new List<Uri>()
        {
            new Uri("../Demo/Demo01.xaml", UriKind.Relative),
            new Uri("../Demo/Demo03.xaml", UriKind.Relative),
        };

        public Demo08()
        {
            InitializeComponent();

            _ = MyFrame.Navigate(UriList[0]);
        }

        private void WxDrawerMenuItem_Selected(object sender, RoutedEventArgs e)
        {
            int idx = MyMenu.Content.IndexOf(sender as WxDrawerMenuItem);
            _ = MyFrame.Navigate(UriList[idx]);
        }
    }
}
