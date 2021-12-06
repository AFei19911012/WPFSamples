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
    /// Created Time: 2021/12/7 0:41:43
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/7 0:41:43    Taosy.W                 
    ///
    /// <summary>
    /// 自定义 Circle，继承自 Stroke，重写 DrawCore 事件
    /// 圆心 + 任意点
    /// </summary>
    public class CustomCircle : Stroke
    {
        public CustomCircle(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 圆心和任意点
            Point point1 = (Point)StylusPoints[0];
            Point point2 = (Point)StylusPoints[1];
            double radius = InkCanvasMethod.GetDistancePP(point1, point2);
            // 固定长度
            double len = 2000;

            // Circle
            drawingContext.DrawEllipse(null, InkCanvasMethod.SetPenPoint(2), point1, radius, radius);

            // Cross
            PathGeometry geometry = new PathGeometry();
            // 横线
            PathFigure figure = new PathFigure
            {
                StartPoint = new Point(point1.X - len, point1.Y),
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment(new Point(point1.X + len, point1.Y), true));
            geometry.Figures.Add(figure);
            // 竖线
            figure = new PathFigure
            {
                StartPoint = new Point(point1.X, point1.Y - len),
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment(new Point(point1.X, point1.Y + len), true));
            geometry.Figures.Add(figure);
            // 虚线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenDotted(), geometry);

            // Point 缩放时大小不变
            drawingContext.DrawEllipse(null, InkCanvasMethod.SetPenPoint(), new Point(point1.X, point1.Y - radius), 1, 1);
            drawingContext.DrawEllipse(null, InkCanvasMethod.SetPenPoint(), new Point(point1.X - radius, point1.Y), 1, 1);
            drawingContext.DrawEllipse(null, InkCanvasMethod.SetPenPoint(), new Point(point1.X, point1.Y + radius), 1, 1);
            drawingContext.DrawEllipse(null, InkCanvasMethod.SetPenPoint(), new Point(point1.X + radius, point1.Y), 1, 1);
        }
    }
}