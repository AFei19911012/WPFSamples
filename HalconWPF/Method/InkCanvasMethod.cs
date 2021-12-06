using HalconWPF.Model;
using System;
using System.Collections.Generic;
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
    /// Created Time: 2021/12/7 0:42:17
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/7 0:42:17    Taosy.W                 
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
        public static Pen SetPenSolid()
        {
            Pen pen = new Pen
            {
                Brush = StrokeBrushDefault,
                Thickness = 2,
                DashCap = PenLineCap.Round,
                LineJoin = PenLineJoin.Round,
                MiterLimit = 0.0
            };
            return pen;
        }

        /// <summary>
        /// 细虚线 Pen
        /// </summary>
        /// <returns></returns>
        public static Pen SetPenDotted()
        {
            Pen pen = new Pen
            {
                Brush = StrokeBrushDefault,
                Thickness = 1,
                DashStyle = new DashStyle(new double[] { 2.0, 2.0 }, 0.0),
                DashCap = PenLineCap.Round,
                LineJoin = PenLineJoin.Round,
                MiterLimit = 0.0
            };
            return pen;
        }

        /// <summary>
        /// Point 笔迹
        /// </summary>
        /// <returns></returns>
        public static Pen SetPenPoint(int thickness = 6)
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
        /// 计算两点间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistancePP(Point p1, Point p2)
        {
            return Math.Sqrt(((p1.X - p2.X) * (p1.X - p2.X)) + ((p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }
        public static List<double> GetDistancePP(StylusPointCollection sps, Point p)
        {
            List<double> dists = new List<double>();
            for (int i = 0; i < sps.Count; i++)
            {
                dists.Add(GetDistancePP((Point)sps[i], p));
            }
            return dists;
        }

        /// <summary>
        /// 光线投影算法判断点是否在多边形内部
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="points"></param>
        /// <param name="noneZeroMode"></param>
        /// <returns></returns>
        public static bool GetPointInPolygon(this StylusPointCollection points, Point pt, bool noneZeroMode = false)
        {
            int ptNum = points.Count;
            if (ptNum < 3)
            {
                return false;
            }
            int j = ptNum - 1;
            bool oddNodes = false;
            int zeroState = 0;
            for (int k = 0; k < ptNum; k++)
            {
                Point ptK = (Point)points[k];
                Point ptJ = (Point)points[j];
                if (((ptK.Y > pt.Y) != (ptJ.Y > pt.Y)) && (pt.X < (ptJ.X - ptK.X) * (pt.Y - ptK.Y) / (ptJ.Y - ptK.Y) + ptK.X))
                {
                    oddNodes = !oddNodes;
                    if (ptK.Y > ptJ.Y)
                    {
                        zeroState++;
                    }
                    else
                    {
                        zeroState--;
                    }
                }
                j = k;
            }
            return noneZeroMode ? zeroState != 0 : oddNodes;
        }

        /// <summary>
        /// 计算多边形的质心
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point GetPolygonCenter(this StylusPointCollection points)
        {
            Point point = new Point(0, 0);
            double area = 0;
            double x0;
            double y0;
            double x1;
            double y1;
            double temp;
            for (int i = 0; i < points.Count - 1; i++)
            {
                x0 = points[i].X;
                y0 = points[i].Y;
                x1 = points[i + 1].X;
                y1 = points[i + 1].Y;
                temp = (x0 * y1) - (x1 * y0);
                area += temp;
                point.X += (x0 + x1) * temp;
                point.Y += (y0 + y1) * temp;
            }
            x0 = points[points.Count - 1].X;
            y0 = points[points.Count - 1].Y;
            x1 = points[0].X;
            y1 = points[0].Y;
            temp = (x0 * y1) - (x1 * y0);
            area += temp;
            point.X += (x0 + x1) * temp;
            point.Y += (y0 + y1) * temp;
            point.X /= 3 * area;
            point.Y /= 3 * area;
            return point;
        }

        /// <summary>
        /// 计算夹角
        /// </summary>
        /// <param name="C"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double GetPointAngle(Point C, Point A, Point B)
        {
            double a = GetDistancePP(B, C);
            double b = GetDistancePP(A, C);
            double c = GetDistancePP(A, B);
            double cTheta = ((a * a) + (b * b) - (c * c)) / (2 * a * b);
            cTheta = Math.Min(1, cTheta);
            return Math.Acos(cTheta) * 180 / Math.PI;
        }

        /// <summary>
        /// 计算旋转方向：顺时针、逆时针
        /// </summary>
        /// <param name="p0"> 旋转中心 </param>
        /// <param name="p1"> 起点 </param>
        /// <param name="p2"> 终点 </param>
        /// <returns></returns>
        public static bool GetRotateDirection(Point p0, Point p1, Point p2)
        {
            Vector vector1 = p1 - p0;
            Vector vector2 = p2 - p0;
            // 向量叉乘判断
            bool direction = (vector1.X * vector2.Y) - (vector1.Y * vector2.X) > 0;
            return direction;
        }

        /// <summary>
        /// 编辑状态
        /// </summary>
        /// <param name="stroke"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static EnumModuleEditType GetModuleEditType(this Stroke stroke, Point p, EnumModuleType moduleType = EnumModuleType.rectangle)
        {
            EnumModuleEditType mode = EnumModuleEditType.None;
            // 左上开始，逆时针方向 8 个点坐标
            StylusPointCollection sps = stroke.StylusPoints.Clone();
            double threshold = 5;

            // 矩形模板、椭圆模板
            if (moduleType == EnumModuleType.rectangle || moduleType == EnumModuleType.ellipse)
            {
                sps.Clear();
                for (int i = 0; i < 8; i++)
                {
                    sps.Add(stroke.StylusPoints[i]);
                }
                List<double> dists = GetDistancePP(sps, p);
                // 移动
                if (GetPointInPolygon(sps, p))
                {
                    mode = EnumModuleEditType.Move;
                }

                // 旋转
                for (int i = 0; i < sps.Count; i += 2)
                {
                    if (dists[i] <= 2 * threshold)
                    {
                        mode = EnumModuleEditType.Rotate;
                    }
                }

                // 缩放
                if (dists[0] <= threshold || dists[2] <= threshold || dists[4] <= threshold || dists[6] <= threshold)
                {
                    mode = EnumModuleEditType.SizeAll;
                }
                else if (dists[1] <= threshold)
                {
                    mode = EnumModuleEditType.SizeW;
                }
                else if (dists[3] <= threshold)
                {
                    mode = EnumModuleEditType.SizeS;
                }
                else if (dists[5] <= threshold)
                {
                    mode = EnumModuleEditType.SizeE;
                }
                else if (dists[7] <= threshold)
                {
                    mode = EnumModuleEditType.SizeN;
                }
            }
            // 圆模板
            else if (moduleType == EnumModuleType.circle)
            {
                // 圆心和任意点
                Point point1 = (Point)sps[0];
                double radius = GetDistancePP(point1, (Point)sps[1]);
                Point pN = new Point(point1.X, point1.Y - radius);
                Point pW = new Point(point1.X - radius, point1.Y);
                Point pS = new Point(point1.X, point1.Y + radius);
                Point pE = new Point(point1.X + radius, point1.Y);
                // 模式
                if (GetDistancePP(p, pN) <= threshold || GetDistancePP(p, pW) <= threshold || GetDistancePP(p, pS) <= threshold || GetDistancePP(p, pE) <= threshold)
                {
                    mode = EnumModuleEditType.SizeAll;
                }
                else if (GetDistancePP(p, point1) <= radius)
                {
                    mode = EnumModuleEditType.Move;
                }
            }
            // 多边形模板
            else if (moduleType == EnumModuleType.polygon)
            {
                // 移动
                if (GetPointInPolygon(sps, p))
                {
                    mode = EnumModuleEditType.Move;
                }
                // 模式
                foreach (StylusPoint sp in sps)
                {
                    // 如果在标记点附近
                    if (GetDistancePP(p, (Point)sp) <= threshold)
                    {
                        mode = EnumModuleEditType.SizeAll;
                        break;
                    }
                }
            }

            return mode;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////// 创建模板

        /// <summary>
        /// 左上 → 右下 8 个点生成矩形
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static CustomRectangle CreateRectangle(Point point1, Point point2)
        {
            // 左上开始，逆时针共 8 个点
            StylusPointCollection points = new StylusPointCollection()
            {
                new StylusPoint(point1.X, point1.Y),
                new StylusPoint(point1.X, 0.5 * (point1.Y + point2.Y)),
                new StylusPoint(point1.X, point2.Y),
                new StylusPoint(0.5 * (point1.X + point2.X), point2.Y),
                new StylusPoint(point2.X, point2.Y),
                new StylusPoint(point2.X, 0.5 * (point1.Y + point2.Y)),
                new StylusPoint(point2.X, point1.Y),
                new StylusPoint(0.5 * (point1.X + point2.X), point1.Y),
            };
            CustomRectangle stroke = new CustomRectangle(points)
            {
                DrawingAttributes = SetInkAttributes(),
            };

            return stroke;
        }

        /// <summary>
        /// 左上 → 右下 8 个点 + 椭圆点 生成 Ellipse
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static CustomEllipse CreateEllipse(Point point1, Point point2)
        {
            // 左上开始，逆时针共 8 个点
            StylusPointCollection points = new StylusPointCollection()
            {
                new StylusPoint(point1.X, point1.Y),
                new StylusPoint(point1.X, 0.5 * (point1.Y + point2.Y)),
                new StylusPoint(point1.X, point2.Y),
                new StylusPoint(0.5 * (point1.X + point2.X), point2.Y),
                new StylusPoint(point2.X, point2.Y),
                new StylusPoint(point2.X, 0.5 * (point1.Y + point2.Y)),
                new StylusPoint(point2.X, point1.Y),
                new StylusPoint(0.5 * (point1.X + point2.X), point1.Y),
            };
            // 椭圆的点，从右边开始
            Point point = new Point(0.5 * (point1.X + point2.X), 0.5 * (point1.Y + point2.Y));
            double ra = 0.5 * Math.Abs(point2.X - point1.X);
            double rb = 0.5 * Math.Abs(point2.Y - point1.Y);
            for (int i = 0; i < 100; i++)
            {
                double theta = Math.PI * 2 * i / (100 - 1);
                points.Add(new StylusPoint(point.X + (ra * Math.Cos(theta)), point.Y + (rb * Math.Sin(theta))));
            }
            CustomEllipse stroke = new CustomEllipse(points)
            {
                DrawingAttributes = SetInkAttributes(),
            };
            return stroke;
        }

        /// <summary>
        /// 圆心 + 任意点 生成圆
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static CustomCircle CreateCircle(Point point1, Point point2)
        {
            StylusPointCollection points = new StylusPointCollection()
            {
                new StylusPoint(point1.X, point1.Y),
                new StylusPoint(point2.X, point2.Y),
            };
            CustomCircle stroke = new CustomCircle(points)
            {
                DrawingAttributes = SetInkAttributes(),
            };
            return stroke;
        }

        /// <summary>
        /// 创建 Polygon
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static CustomPolygon CreatePolygon(StylusPointCollection points)
        {
            CustomPolygon stroke = new CustomPolygon(points)
            {
                DrawingAttributes = SetInkAttributes(),
            };
            return stroke;
        }

        /// <summary>
        /// 创建 PolyLine
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static CustomPolyline CreatePolyline(StylusPointCollection points)
        {
            CustomPolyline stroke = new CustomPolyline(points)
            {
                DrawingAttributes = SetInkAttributes(),
            };
            return stroke;
        }

        /// <summary>
        /// 创建 Marker
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static CustomMarkerCross CreateMarkerCross(Point point, double r = 10, double angle = 0)
        {
            StylusPointCollection points = new StylusPointCollection()
            {
                new StylusPoint(point.X - r, point.Y),
                new StylusPoint(point.X + r, point.Y),
                new StylusPoint(point.X, point.Y - r),
                new StylusPoint(point.X, point.Y + r),
            };
            CustomMarkerCross stroke = new CustomMarkerCross(points)
            {
                DrawingAttributes = SetInkAttributes(),
            };

            // 旋转
            if (angle > 0)
            {
                Matrix matrix = new Matrix();
                matrix.RotateAt(angle, point.X, point.Y);
                stroke.Transform(matrix, false);
            }
            return stroke;
        }
    }
}