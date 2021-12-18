using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace HalconWPF.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/1 15:50:20
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/1 15:50:20    Taosy.W                 
    ///
    /// <summary>
    /// 自定义 Rectangle，继承自 Stroke，重写 DrawCore 事件
    /// 传入 8 个点（左上开始，逆时针方向）即可自动生成形状
    /// </summary>
    public class CustomRectangle : Stroke
    {
        public CustomRectangle(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 左上、右下两个点坐标
            Point point1 = (Point)StylusPoints[0];
            Point point2 = (Point)StylusPoints[4];
            Point point0 = new Point(0.5 * (point1.X + point2.X), 0.5 * (point1.Y + point2.Y));
            // 固定长度
            double radius = 2000;

            // Rectangle
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = point1,
                IsClosed = true,
                IsFilled = true,
            };
            for (int i = 1; i < StylusPoints.Count; i++)
            {
                figure.Segments.Add(new LineSegment((Point)StylusPoints[i], true));
            }
            figure.Segments.Add(new LineSegment(point1, true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);

            // Cross
            geometry = new PathGeometry();
            // 横线
            figure = new PathFigure
            {
                StartPoint = new Point(point0.X - radius, point0.Y),
            };
            figure.Segments.Add(new LineSegment(new Point(point0.X + radius, point0.Y), true));
            geometry.Figures.Add(figure);
            // 竖线
            figure = new PathFigure
            {
                StartPoint = new Point(point0.X, point0.Y - radius),
            };
            figure.Segments.Add(new LineSegment(new Point(point0.X, point0.Y + radius), true));
            geometry.Figures.Add(figure);
            // 虚线 缩放时大小不变
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenDotted(), geometry);

            // Point 缩放时大小不变
            for (int i = 0; i < StylusPoints.Count; i++)
            {
                drawingContext.DrawEllipse(null, InkCanvasMethod.SetPenPoint(), (Point)StylusPoints[i], 1, 1);
            }
        }
    }
}