using System.Windows;

namespace Demos.Demo
{
    /// <summary>
    /// WindowClosedQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class WindowClosedQuestion : Window
    {
        public bool IsClosing { get; set; } = false;

        public WindowClosedQuestion()
        {
            InitializeComponent();

            Width = SystemParameters.PrimaryScreenWidth;
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = false;
            Close();
        }
    }
}