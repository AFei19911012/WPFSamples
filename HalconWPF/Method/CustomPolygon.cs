using System;
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
    /// Created Time: 2021/12/7 0:47:31
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/7 0:47:31    Taosy.W                 
    ///
    /// <summary>
    /// 自定义 Polygon，继承自 Stroke，重写 DrawCore 事件
    /// 需传入多边形所有顶点
    /// </summary>
    public class CustomPolygon : Stroke
    {
        public CustomPolygon(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 起点
            Point point = (Point)StylusPoints[0];
            // 固定长度
            double radius = 2000;

            // Polygon
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = new Point(point.X, point.Y),
                IsClosed = false,
                IsFilled = false,
            };
            for (int i = 1; i < StylusPoints.Count; i++)
            {
                figure.Segments.Add(new LineSegment((Point)StylusPoints[i], true));
            }
            figure.Segments.Add(new LineSegment((Point)StylusPoints[0], true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);

            // Cross
            Point pointCenter = StylusPoints.GetPolygonCenter();
            geometry = new PathGeometry();
            // 横线
            figure = new PathFigure
            {
                StartPoint = new Point(pointCenter.X - radius, pointCenter.Y),
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment(new Point(pointCenter.X + radius, pointCenter.Y), true));
            geometry.Figures.Add(figure);
            // 竖线
            figure = new PathFigure
            {
                StartPoint = new Point(pointCenter.X, pointCenter.Y - radius),
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment(new Point(pointCenter.X, pointCenter.Y + radius), true));
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