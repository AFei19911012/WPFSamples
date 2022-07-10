using HelixToolkit.Wpf;
using System.Windows.Controls;
using System.Windows.Input;
using Demos.ViewModel;

namespace Demos.Demo
{
    /// <summary>
    /// HelixToolkitDemo2.xaml 的交互逻辑
    /// </summary>
    public partial class HelixToolkitDemo2 : UserControl
    {
        public HelixToolkitDemo2()
        {
            InitializeComponent();

            HelixToolkitDemo2VM vm = new HelixToolkitDemo2VM()
            {
                HViewPort3D = HView3D,
            };
            vm.RectangleSelectionCommand = new RectangleSelectionCommand(vm.HViewPort3D.Viewport, vm.HandleSelectionModelsEvent, vm.HandleSelectionVisualsEvent);
            vm.PointSelectionCommand = new PointSelectionCommand(vm.HViewPort3D.Viewport, vm.HandleSelectionModelsEvent, vm.HandleSelectionVisualsEvent);
            DataContext = vm;

            // 鼠标按键事件
            HView3D.InputBindings.Add(new MouseBinding(vm.RectangleSelectionCommand, new MouseGesture(MouseAction.RightClick)));
            HView3D.InputBindings.Add(new MouseBinding(vm.PointSelectionCommand, new MouseGesture(MouseAction.LeftClick, ModifierKeys.Control)));
        }
    }
}