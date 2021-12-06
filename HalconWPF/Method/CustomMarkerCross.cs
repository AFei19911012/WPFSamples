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
    /// Created Time: 2021/12/7 0:46:54
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/7 0:46:54    Taosy.W                 
    ///
    /// <summary>
    /// 自定义 Cross，继承自 Stroke，重写 DrawCore 事件
    /// 当前点为中心
    /// </summary>
    public class CustomMarkerCross : Stroke
    {
        public CustomMarkerCross(StylusPointCollection points) : base(points)
        {
            StylusPoints = points.Clone();
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            // Cross
            PathGeometry geometry = new PathGeometry();
            // 横线
            PathFigure figure = new PathFigure
            {
                StartPoint = (Point)StylusPoints[0],
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment((Point)StylusPoints[1], true));
            geometry.Figures.Add(figure);
            // 竖线
            figure = new PathFigure
            {
                StartPoint = (Point)StylusPoints[2],
                IsClosed = false
            };
            figure.Segments.Add(new LineSegment((Point)StylusPoints[3], true));
            geometry.Figures.Add(figure);
            // 实线 缩放时大小变化
            drawingContext.DrawGeometry(null, InkCanvasMethod.SetPenSolid(), geometry);
        }
    }
}