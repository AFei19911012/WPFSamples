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
    /// Created Time: 22/08/29 19:46:08
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:46:08    CoderMan/CoderdMan1012         首次编写         
    ///
    /// <summary>
    /// 自定义 Ellipse，继承自 Stroke，重写 DrawCore 事件
    /// 传入 8 个点（左上开始，逆时针方向）和椭圆点即可自动生成形状
    /// </summary>
    public class CustomEllipse : Stroke
    {
        public CustomEllipse(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 左上、右下两个点坐标
            Point point1 = (Point)StylusPoints[0];
            Point point2 = (Point)StylusPoints[4];
            Point point0 = new Point(0.5 * (point1.X + point2.X), 0.5 * (point1.Y + point2.Y));
            double radius = 2000;

            // Rectangle
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = point1,
                IsClosed = false,
                IsFilled = false,
            };
            for (int i = 0; i < 8; i++)
            {
                figure.Segments.Add(new LineSegment((Point)StylusPoints[i], true));
            }
            figure.Segments.Add(new LineSegment((Point)StylusPoints[0], true));
            geometry.Figures.Add(figure);
            // 虚线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkMethod.SetPenDotted(), geometry);

            // Ellipse
            geometry = new PathGeometry();
            figure = new PathFigure
            {
                StartPoint = (Point)StylusPoints[8],
                IsClosed = false,
                IsFilled = false,
            };
            for (int i = 9; i < StylusPoints.Count; i++)
            {
                figure.Segments.Add(new LineSegment((Point)StylusPoints[i], true));
            }
            geometry.Figures.Add(figure);
            // 实线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkMethod.SetPenSolid(), geometry);

            // Cross
            geometry = new PathGeometry();
            // 横线
            figure = new PathFigure
            {
                StartPoint = new Point(point0.X - radius, point0.Y),
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment(new Point(point0.X + radius, point0.Y), true));
            geometry.Figures.Add(figure);
            // 竖线
            figure = new PathFigure
            {
                StartPoint = new Point(point0.X, point0.Y - radius),
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment(new Point(point0.X, point0.Y + radius), true));
            geometry.Figures.Add(figure);
            // 虚线 缩放时大小不变
            drawingContext.DrawGeometry(null, InkMethod.SetPenDotted(), geometry);

            // Point 缩放时大小不变
            for (int i = 0; i < 8; i++)
            {
                drawingContext.DrawEllipse(null, InkMethod.SetPenPoint(), (Point)StylusPoints[i], 1, 1);
            }
        }
    }
}