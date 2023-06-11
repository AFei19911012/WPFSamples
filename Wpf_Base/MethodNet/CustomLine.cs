using System;
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
    /// Created Time: 22/08/29 19:46:46
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:46:46    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CustomLine : Stroke
    {
        public CustomLine(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            Point pt1 = (Point)StylusPoints[0];
            Point pt2 = (Point)StylusPoints[1];
            // 直线
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = pt1,
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment(pt2, true));
            geometry.Figures.Add(figure);
            // 虚线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkMethod.SetPenDotted(), geometry);

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
            double x0 = 0.5 * (x1 + x2);
            double y0 = 0.5 * (y1 + y2);
            double x3 = x0 + (directionFlag * arrowLength * Math.Cos(angleDown));
            double y3 = y0 + (directionFlag * arrowLength * Math.Sin(angleDown));
            double x4 = x0 + (directionFlag * arrowLength * Math.Cos(angleUp));
            double y4 = y0 + (directionFlag * arrowLength * Math.Sin(angleUp));
            Point pt3 = new Point(x3, y3);
            Point pt4 = new Point(x4, y4);

            figure = new PathFigure
            {
                StartPoint = new Point(x0, y0),
                IsClosed = true,
                IsFilled = true,
            };
            figure.Segments.Add(new LineSegment(pt3, true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小不变
            drawingContext.DrawGeometry(null, InkMethod.SetPenSolid(), geometry);

            geometry = new PathGeometry();
            figure = new PathFigure
            {
                StartPoint = new Point(x0, y0),
                IsClosed = true,
                IsFilled = true,
            };
            figure.Segments.Add(new LineSegment(pt4, true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小不变
            drawingContext.DrawGeometry(null, InkMethod.SetPenSolid(), geometry);

            for (int i = 0; i < StylusPoints.Count; i++)
            {
                drawingContext.DrawEllipse(null, InkMethod.SetPenPoint(), (Point)StylusPoints[i], 1, 1);
            }
        }
    }
}