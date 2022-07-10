using HelixToolkit.Wpf;
using System.Linq;
using System.Windows.Controls;
using Demos.ViewModel;

namespace Demos.Demo
{
    /// <summary>
    /// HelixToolkitDemo3.xaml 的交互逻辑
    /// </summary>
    public partial class HelixToolkitDemo3 : UserControl
    {
        private HelixToolkitDemo3VM MyModelViewrVM { get; set; }

        public HelixToolkitDemo3()
        {
            InitializeComponent();

            MyModelViewrVM = new HelixToolkitDemo3VM()
            {
                HViewPort3D = HView3D,
            };
            DataContext = MyModelViewrVM;
        }

        private void MenuItemSize_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            HView3D.ZoomExtents(500);
        }

        private void HView3D_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Viewport3DHelper.HitResult firstHit = HView3D.Viewport.FindHits(e.GetPosition(HView3D)).FirstOrDefault();
            MyModelViewrVM.SelectedObject = firstHit?.Visual;
        }
    }
}
