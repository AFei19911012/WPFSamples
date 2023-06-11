using System;
using System.Collections.Generic;
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
    /// Created Time: 22/08/29 19:42:06
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:42:06    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class InkMethod
    {
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
                Color = CConstants.ColorDefault,
                StylusTip = StylusTip.Rectangle,
                IsHighlighter = false,
                IgnorePressure = true,
            };
            return attributes;
        }

        /// <summary>
        /// 设置掩膜笔迹属性
        /// </summary>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static DrawingAttributes SetInkAttributesMask()
        {
            DrawingAttributes attributes = new DrawingAttributes
            {
                FitToCurve = false,
                Width = CConstants.InkStrokeThickness,
                Height = CConstants.InkStrokeThickness,
                Color = CConstants.ColorMask,
                StylusTip = StylusTip.Ellipse,
                IsHighlighter = true,
                IgnorePressure = true,
            };
            return attributes;
        }

        /// <summary>
        /// 实线 Pen
        /// </summary>
        /// <returns></returns>
        public static Pen SetPenSolid(double thickness = 2)
        {
            Pen pen = new Pen
            {
                Brush = CConstants.BrushDefault,
                Thickness = thickness,
                DashCap = PenLineCap.Round,
                LineJoin = PenLineJoin.Round,
                MiterLimit = 0.0
            };
            return pen;
        }

        /// <summary>
        /// 掩膜 Pen
        /// </summary>
        /// <returns></returns>
        public static Pen SetPenMask(double thickness = 10)
        {
            Pen pen = new Pen
            {
                Brush = CConstants.BrushMask,
                Thickness = thickness,
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
        public static Pen SetPenDotted(double thickness = 1)
        {
            Pen pen = new Pen
            {
                Brush = CConstants.BrushDefault,
                Thickness = thickness,
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
                Brush = CConstants.BrushDefault,
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
        /// 坐标点类型转换
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static PointCollection StylusPointsConverter(this StylusPointCollection points)
        {
            PointCollection pts = new PointCollection();
            for (int i = 0; i < points.Count; i++)
            {
                pts.Add(new Point(points[i].X, points[i].Y));
            }
            return pts;
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
        /// 点到直线距离
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistancePL(Point p0, Point p1, Point p2)
        {
            double x1 = p1.X;
            double y1 = p1.Y;
            double x2 = p2.X;
            double y2 = p2.Y;
            double dist;
            // 垂直情况
            if (Math.Abs(x1 - x2) < 1e-6)
            {
                dist = Math.Abs(p0.X - x1);
            }
            else
            {
                // 根据点到直线距离公式计算
                // 计算斜率和截距
                double k = (y2 - y1) / (x2 - x1);
                double b = ((x2 * y1) - (x1 * y2)) / (x2 - x1);
                dist = Math.Abs((k * p0.X) - p0.Y + b) / Math.Sqrt((k * k) + 1);

                // 根据海伦公式计算
                //double lenP1P2 = GetDistancePP(p1, p2);
                //double lenP0P1 = GetDistancePP(p0, p1);
                //double lenP0P2 = GetDistancePP(p0, p2);
                //double len = 0.5 * (lenP0P1 + lenP0P2 + lenP1P2);
                //double area = Math.Sqrt(len * (len - lenP0P1) * (len - lenP0P2) * (len - lenP1P2));
                //dist = 2 * area / lenP1P2;
            }
            return dist;
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
        /// 创建 Line
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static CustomLine CreateLine(Point point1, Point point2)
        {
            StylusPointCollection points = new StylusPointCollection()
            {
                new StylusPoint(point1.X, point1.Y),
                new StylusPoint(point2.X, point2.Y),
            };
            CustomLine stroke = new CustomLine(points)
            {
                DrawingAttributes = SetInkAttributes(),
            };
            return stroke;
        }

        /// <summary>
        /// 创建 LineMask
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static CustomLineMask CreateLineMask(StylusPointCollection points)
        {
            CustomLineMask stroke = new CustomLineMask(points)
            {
                DrawingAttributes = SetInkAttributesMask(),
            };
            return stroke;
        }

        /// <summary>
        /// 创建 Marker
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static CustomMarkerCross CreateMarkerCross(Point point, double r = CConstants.MarkerCrossLenght, double angle = 0)
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

        /// <summary>
        /// 左上 → 右下 4 个点生成卡尺矩形
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static CustomCaliperRectangle CreateCaliperRectangle(Point point1, Point point2)
        {
            // 左上开始，逆时针共 4 个点
            StylusPointCollection points = new StylusPointCollection()
            {
                new StylusPoint(point1.X, point1.Y),
                new StylusPoint(point1.X, point2.Y),
                new StylusPoint(point2.X, point2.Y),
                new StylusPoint(point2.X, point1.Y),
            };
            CustomCaliperRectangle stroke = new CustomCaliperRectangle(points)
            {
                DrawingAttributes = SetInkAttributes(),
            };

            return stroke;
        }

        /// <summary>
        /// 平行线
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static CustomParallelLines CreateParallelLines(List<Point> pts)
        {
            CustomParallelLines stroke = new CustomParallelLines(new StylusPointCollection(pts))
            {
                DrawingAttributes = SetInkAttributes(),
            };
            return stroke;
        }
    }
}