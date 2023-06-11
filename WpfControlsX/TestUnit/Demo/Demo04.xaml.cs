using System.Windows.Controls;
using System.Windows.Input;
using TestUnit.ViewModel;

namespace TestUnit.Demo
{
    /// <summary>
    /// Demo03.xaml 的交互逻辑
    /// </summary>
    public partial class Demo04 : UserControl
    {
        public Demo04()
        {
            InitializeComponent();
        }

        private void WxTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainVM vm = DataContext as MainVM;
            if (vm.ListDataModel.Count > 10)
            {
                (DataContext as MainVM).ListDataModel.RemoveAt(9);
            }
        }
    }
}