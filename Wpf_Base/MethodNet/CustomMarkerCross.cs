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
    /// Created Time: 22/08/29 19:48:08
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:48:08    CoderMan/CoderdMan1012         首次编写         
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
            drawingContext.DrawGeometry(null, InkMethod.SetPenSolid(), geometry);
        }
    }
}