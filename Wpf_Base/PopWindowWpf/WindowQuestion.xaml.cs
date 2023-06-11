using System.Windows;

namespace Wpf_Base.PopWindowWpf
{
    /// <summary>
    /// WindowQuestion.xaml 的交互逻辑
    /// </summary>
    public partial class WindowQuestion : Window
    {
        public bool IsOnYes { get; set; } = false;

        public WindowQuestion(string content)
        {
            InitializeComponent();

            TB_Content.Text = content;
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            IsOnYes = true;
            Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            IsOnYes = false;
            Close();
        }
    }
}