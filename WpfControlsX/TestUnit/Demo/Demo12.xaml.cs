using System.Windows.Controls;
using System.Windows.Documents;
using WpfControlsX.ControlX;

namespace TestUnit.Demo
{
    /// <summary>
    /// Demo12.xaml 的交互逻辑
    /// </summary>
    public partial class Demo12 : UserControl
    {
        public Demo12()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AdornerLayer adorner = AdornerLayer.GetAdornerLayer(this);
            if (adorner != null)
            {
                adorner.Add(new ElementAdorner(border1));
                adorner.Add(new ElementAdorner(border2));
            }
        }
    }
}
