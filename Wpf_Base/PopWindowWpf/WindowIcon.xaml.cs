using System;
using System.Windows;
using System.Windows.Media;

namespace Wpf_Base.PopWindowWpf
{
    /// <summary>
    /// WindowIcon.xaml 的交互逻辑
    /// </summary>
    public partial class WindowIcon : Window
    {
        public WindowIcon(EnumWindowType window_type, string info)
        {
            InitializeComponent();

            TB_Info.Text = info;
            TB_Time.Text = DateTime.Now.ToString("G");
            // 资源
            MyPath.Data = (Geometry)FindResource("Icon" + window_type.ToString());
            MyPath.Fill = (Brush)FindResource(window_type.ToString() + "Brush");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}