using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf_Base.MethodNet
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 19:45:11
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:45:11    CoderMan/CoderdMan1012         首次编写         
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
            double radius = InkMethod.GetDistancePP(point1, point2);
            // 固定长度
            double len = 2000;

            // Circle
            drawingContext.DrawEllipse(null, InkMethod.SetPenPoint(2), point1, radius, radius);

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
            drawingContext.DrawGeometry(null, InkMethod.SetPenDotted(), geometry);

            // Point 缩放时大小不变
            drawingContext.DrawEllipse(null, InkMethod.SetPenPoint(), new Point(point1.X, point1.Y - radius), 1, 1);
            drawingContext.DrawEllipse(null, InkMethod.SetPenPoint(), new Point(point1.X - radius, point1.Y), 1, 1);
            drawingContext.DrawEllipse(null, InkMethod.SetPenPoint(), new Point(point1.X, point1.Y + radius), 1, 1);
            drawingContext.DrawEllipse(null, InkMethod.SetPenPoint(), new Point(point1.X + radius, point1.Y), 1, 1);
        }
    }
}