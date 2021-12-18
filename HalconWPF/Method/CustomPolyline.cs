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
    /// Created Time: 2021/12/7 0:48:16
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/7 0:48:16    Taosy.W                 
    ///
    /// <summary>
    /// 自定义 Polyline，继承自 Stroke，重写 DrawCore 事件
    /// 所有点
    /// </summary>
    public class CustomPolyline : Stroke
    {
        public CustomPolyline(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 起始点
            Point point1 = (Point)StylusPoints[0];
            Point point2 = (Point)StylusPoints[StylusPoints.Count - 1];
            // 固定长度
            double radius = 2000;

            // Polyline
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = new Point(point1.X, point1.Y),
                IsClosed = true,
                IsFilled = true,
            };
            for (int i = 0; i < StylusPoints.Count; i++)
            {
                figure.Segments.Add(new LineSegment((Point)StylusPoints[i], true));
            }
            geometry.Figures.Add(figure);
            // 实线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);

            // Cross
            geometry = new PathGeometry();
            // 横线
            figure = new PathFigure
            {
                StartPoint = new Point(point2.X - radius, point2.Y),
            };
            figure.Segments.Add(new LineSegment(new Point(point2.X + radius, point2.Y), true));
            geometry.Figures.Add(figure);
            // 竖线
            figure = new PathFigure
            {
                StartPoint = new Point(point2.X, point2.Y - radius),
            };
            figure.Segments.Add(new LineSegment(new Point(point2.X, point2.Y + radius), true));
            geometry.Figures.Add(figure);
            // 虚线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenDotted(), geometry);

            // Point 缩放时大小不变
            for (int i = 0; i < StylusPoints.Count; i++)
            {
                drawingContext.DrawEllipse(null, InkCanvasMethod.SetPenPoint(), (Point)StylusPoints[i], 1, 1);
            }
        }
    }
}