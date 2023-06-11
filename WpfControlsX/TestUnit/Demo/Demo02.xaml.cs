using System.Windows.Controls;
using System.Windows.Input;
using WpfControlsX.ControlX;

namespace TestUnit.Demo
{
    /// <summary>
    /// Demo02.xaml 的交互逻辑
    /// </summary>
    public partial class Demo02 : UserControl
    {
        private int Index { get; set; } = 0;

        public Demo02()
        {
            InitializeComponent();
        }

        private void WxResetButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as WxResetButton).SetValue(ExtendElement.AngleProperty, 30.0);
        }

        private void WxResetButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as WxResetButton).SetValue(ExtendElement.AngleProperty, 0.0);
        }

        private void WxRepeatButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Index = (Index + 1) % 10;
            (sender as WxRepeatButton).Content = Index.ToString();
        }
    }
}