using System;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace Demos.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/6 11:04:10
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/6 11:04:10    Taosy.W                 
    ///
    public class CustomArrow : Stroke
    {
        public CustomArrow(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 箭头 -->
            double x1 = StylusPoints[0].X;
            double y1 = StylusPoints[0].Y;
            double x2 = StylusPoints[1].X;
            double y2 = StylusPoints[1].Y;
            double dist = Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));
            double arrowLength = Math.Min(20, dist);
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

            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = (Point)StylusPoints[0],
                IsClosed = true,
                IsFilled = true,
            };
            figure.Segments.Add(new LineSegment((Point)StylusPoints[1], true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);

            geometry = new PathGeometry();
            figure = new PathFigure
            {
                StartPoint = (Point)StylusPoints[1],
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
                StartPoint = (Point)StylusPoints[1],
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