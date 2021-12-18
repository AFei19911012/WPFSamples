using HalconWPF.ViewModel;
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
    /// Created Time: 2021/12/12 11:11:29
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/12 11:11:29    Taosy.W                 
    ///
    public class CustomLineMask : Stroke
    {
        public CustomLineMask(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // 所有的点组成一条路径
            Point pt1 = (Point)StylusPoints[0];
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = pt1,
                IsClosed = true,
                IsFilled = true,
            };
            // 头部圆滑
            //if (StylusPoints.Count > 1)
            //{
            //    figure.Segments.Add(new LineSegment((Point)StylusPoints[1], true));
            //    figure.Segments.Add(new LineSegment((Point)StylusPoints[0], true));
            //}
            for (int i = 1; i < StylusPoints.Count; i++)
            {
                figure.Segments.Add(new LineSegment((Point)StylusPoints[i], true));
            }
            // 尾部圆滑
            //if (StylusPoints.Count > 1)
            //{
            //    figure.Segments.Add(new LineSegment((Point)StylusPoints[StylusPoints.Count - 2], true));
            //    figure.Segments.Add(new LineSegment((Point)StylusPoints[StylusPoints.Count - 1], true));
            //}
            geometry.Figures.Add(figure);
            // 缩放时宽度改变
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);
            // 缩放时宽度改变
            //drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenMask(DomainModuleViewModel.InkStrokeThickness), geometry);
        }
    }
}