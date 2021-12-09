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
    /// Created Time: 2021/12/9 16:37:51
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/9 16:37:51    Taosy.W                 
    ///
    public class CustomParallelLines : Stroke
    {
        public CustomParallelLines(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 平行线数量
            for (int i = 0; i < StylusPoints.Count; i += 2)
            {
                // 两点确定一条直线
                Point pt1 = (Point)StylusPoints[i];
                Point pt2 = (Point)StylusPoints[i + 1];
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure
                {
                    StartPoint = pt1,
                    IsClosed = true,
                    IsFilled = true,
                };
                figure.Segments.Add(new LineSegment(pt2, true));
                geometry.Figures.Add(figure);
                // 实线 缩放时大小变化
                drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);
            }
        }
    }
}