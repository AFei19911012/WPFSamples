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
    /// Created Time: 2021/12/9 16:37:04
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/9 16:37:04    Taosy.W                 
    ///
    public class CustomCaliperRectangle : Stroke
    {
        public CustomCaliperRectangle(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 左上、右下两个点坐标
            Point point1 = (Point)StylusPoints[0];
            Point point2 = (Point)StylusPoints[1];
            Point point3 = (Point)StylusPoints[2];
            Point point4 = (Point)StylusPoints[3];

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

            // 参考线
            Point pt1 = new Point(0.5 * (point1.X + point2.X), 0.5 * (point1.Y + point2.Y));
            Point pt2 = new Point(0.5 * (point3.X + point4.X), 0.5 * (point3.Y + point4.Y));
            geometry = new PathGeometry();
            figure = new PathFigure
            {
                StartPoint = pt1,
                IsClosed = true,
                IsFilled = true,
            };
            figure.Segments.Add(new LineSegment(pt2, true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);

            // 箭头 -->
            double x1 = pt1.X;
            double y1 = pt1.Y;
            double x2 = pt2.X;
            double y2 = pt2.Y;
            double arrowLength = 20;
            double arrowAngle = Math.PI / 12;
            // 起始点线段夹角
            double angleOri = Math.Atan((y2 - y1) / (x2 - x1));
            // 箭头扩张角度
            double angleDown = angleOri - arrowAngle;
            double angleUp = angleOri + arrowAngle;
            // 方向标识
            int directionFlag = (x2 > x1) ? -1 : 1;
            // 箭头两侧点坐标
            double x3 = x2 + (directionFlag * arrowLength * Math.Cos(angleDown));
            double y3 = y2 + (directionFlag * arrowLength * Math.Sin(angleDown));
            double x4 = x2 + (directionFlag * arrowLength * Math.Cos(angleUp));
            double y4 = y2 + (directionFlag * arrowLength * Math.Sin(angleUp));
            Point pt3 = new Point(x3, y3);
            Point pt4 = new Point(x4, y4);

            geometry = new PathGeometry();
            figure = new PathFigure
            {
                StartPoint = pt2,
                IsClosed = true,
                IsFilled = true,
            };
            figure.Segments.Add(new LineSegment(pt3, true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小不变
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);

            geometry = new PathGeometry();
            figure = new PathFigure
            {
                StartPoint = pt2,
                IsClosed = true,
                IsFilled = true,
            };
            figure.Segments.Add(new LineSegment(pt4, true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小不变
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);
        }
    }
}