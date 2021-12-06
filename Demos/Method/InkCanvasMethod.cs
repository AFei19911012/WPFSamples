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
    /// Created Time: 2021/12/6 11:06:36
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/6 11:06:36    Taosy.W                 
    ///
    public static class InkCanvasMethod
    {
        public static Color ColorDefault = Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00);
        public static Color ColorEidting = Color.FromArgb(0xFF, 0x07, 0xAA, 0xE5);
        public static Brush StrokeBrushDefault = new SolidColorBrush(ColorDefault);
        public static Brush StrokeBrushEdit = new SolidColorBrush(ColorEidting);

        /// <summary>
        /// 设置笔迹属性
        /// </summary>
        public static DrawingAttributes SetInkAttributes(double thickness = 2)
        {
            DrawingAttributes attributes = new DrawingAttributes
            {
                FitToCurve = false,
                Width = thickness,
                Height = thickness,
                Color = ColorDefault,
                StylusTip = StylusTip.Rectangle,
                IsHighlighter = false,
                IgnorePressure = true,
            };
            return attributes;
        }

        /// <summary>
        /// 实线 Pen
        /// </summary>
        /// <returns></returns>
        public static Pen SetPenSolid(int thickness = 2)
        {
            Pen pen = new Pen
            {
                Brush = StrokeBrushDefault,
                Thickness = thickness,
                DashCap = PenLineCap.Round,
                LineJoin = PenLineJoin.Round,
                MiterLimit = 0.0
            };
            return pen;
        }

        /// <summary>
        /// 箭头
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static CustomArrow CreateArrow(Point point1, Point point2)
        {
            StylusPointCollection points = new StylusPointCollection()
            {
                new StylusPoint(point1.X, point1.Y),
                new StylusPoint(point2.X, point2.Y),
            };
            CustomArrow stroke = new CustomArrow(new StylusPointCollection(points))
            {
                DrawingAttributes = SetInkAttributes(),
            };
            return stroke;
        }

        /// <summary>
        /// 计算两点间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistancePP(Point p1, Point p2)
        {
            return Math.Sqrt(((p1.X - p2.X) * (p1.X - p2.X)) + ((p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }
    }
}