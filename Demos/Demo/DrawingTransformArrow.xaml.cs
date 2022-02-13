using Demos.Method;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Demos.Demo
{
    /// <summary>
    /// DrawingTransformArrow.xaml 的交互逻辑
    /// </summary>
    public partial class DrawingTransformArrow : UserControl
    {
        private bool CanMove { get; set; } = false;
        private Point PointMoveOri { get; set; } = new Point();
        private bool IsPanning { get; set; } = false;

        public DrawingTransformArrow()
        {
            InitializeComponent();
        }

        private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanMove = true;
            PointMoveOri = e.GetPosition(e.Device.Target);
        }
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point curPoint = e.GetPosition(e.Device.Target);
            // 创建箭头 每次刷新
            if (CanMove && !IsPanning)
            {
                DrawingCanvas.Strokes.Clear();
                DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateArrow(PointMoveOri, curPoint));
            }

            // 平移
            if (CanMove && IsPanning)
            {
                Matrix matrixMove = new Matrix();
                matrixMove.Translate(curPoint.X - PointMoveOri.X, curPoint.Y - PointMoveOri.Y);
                DrawingCanvas.Strokes.Transform(matrixMove, false);
                // 更新初始移动位置
                PointMoveOri = curPoint;
            }
        }
        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CanMove = false;
        }
        private void DrawingCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // 当前点为中心缩放
            Point curPoint = e.GetPosition(e.Device.Target);
            Matrix matrix = new Matrix();
            if (e.Delta > 0)
            {
                matrix.ScaleAt(1.25, 1.25, curPoint.X, curPoint.Y);
            }
            else
            {
                matrix.ScaleAt(0.8, 0.8, curPoint.X, curPoint.Y);
            }
            DrawingCanvas.Strokes.Transform(matrix, false);
        }
        private void DrawingCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPanning = true;
            if (e.ClickCount > 1)
            {
                DrawingCanvas.Strokes.Clear();
                IsPanning = false;
            }
        }
    }
}