using System.Collections.Generic;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using Wpf_Base.HalconWpf.Model;
using Wpf_Base.MethodNet;

namespace Wpf_Base.HalconWpf.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 12:55:29
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 12:55:29    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class EditMethod
    {
        /// <summary>
        /// 形状模板编辑状态
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
                if (!(stroke is CustomRectangle || stroke is CustomEllipse))
                {
                    return mode;
                }
                sps.Clear();
                for (int i = 0; i < 8; i++)
                {
                    sps.Add(stroke.StylusPoints[i]);
                }
                List<double> dists = InkMethod.GetDistancePP(sps, p);
                // 移动
                if (InkMethod.GetPointInPolygon(sps, p))
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
                if (!(stroke is CustomCircle))
                {
                    return mode;
                }
                // 圆心和任意点
                Point point1 = (Point)sps[0];
                double radius = InkMethod.GetDistancePP(point1, (Point)sps[1]);
                Point pN = new Point(point1.X, point1.Y - radius);
                Point pW = new Point(point1.X - radius, point1.Y);
                Point pS = new Point(point1.X, point1.Y + radius);
                Point pE = new Point(point1.X + radius, point1.Y);
                // 模式
                if (InkMethod.GetDistancePP(p, pN) <= threshold || InkMethod.GetDistancePP(p, pW) <= threshold || InkMethod.GetDistancePP(p, pS) <= threshold || InkMethod.GetDistancePP(p, pE) <= threshold)
                {
                    mode = EnumModuleEditType.SizeAll;
                }
                else if (InkMethod.GetDistancePP(p, point1) <= radius)
                {
                    mode = EnumModuleEditType.Move;
                }
            }
            // 多边形模板
            else if (moduleType == EnumModuleType.polygon)
            {
                if (!(stroke is CustomPolygon))
                {
                    return mode;
                }
                // 移动
                if (InkMethod.GetPointInPolygon(sps, p))
                {
                    mode = EnumModuleEditType.Move;
                }
                // 模式
                foreach (StylusPoint sp in sps)
                {
                    // 如果在标记点附近
                    if (InkMethod.GetDistancePP(p, (Point)sp) <= threshold)
                    {
                        mode = EnumModuleEditType.SizeAll;
                        break;
                    }
                }
            }
            return mode;
        }

        /// <summary>
        /// 测量模板编辑状态
        /// </summary>
        /// <param name="stroke"></param>
        /// <param name="p"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static EnumModuleEditType GetMetrologyObjectEditType(this Stroke stroke, Point p, EnumMetrologyObjectType objectType = EnumMetrologyObjectType.rectangle)
        {
            EnumModuleEditType mode = EnumModuleEditType.None;
            // 左上开始，逆时针方向 8 个点坐标
            StylusPointCollection sps = stroke.StylusPoints.Clone();
            double threshold = 5;

            // 矩形模板、椭圆模板
            if (objectType == EnumMetrologyObjectType.rectangle || objectType == EnumMetrologyObjectType.ellipse)
            {
                if (!(stroke is CustomRectangle || stroke is CustomEllipse))
                {
                    return mode;
                }
                sps.Clear();
                for (int i = 0; i < 8; i++)
                {
                    sps.Add(stroke.StylusPoints[i]);
                }
                List<double> dists = InkMethod.GetDistancePP(sps, p);
                // 移动
                if (InkMethod.GetPointInPolygon(sps, p))
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
            else if (objectType == EnumMetrologyObjectType.circle)
            {
                if (!(stroke is CustomCircle))
                {
                    return mode;
                }
                // 圆心和任意点
                Point point1 = (Point)sps[0];
                double radius = InkMethod.GetDistancePP(point1, (Point)sps[1]);
                Point pN = new Point(point1.X, point1.Y - radius);
                Point pW = new Point(point1.X - radius, point1.Y);
                Point pS = new Point(point1.X, point1.Y + radius);
                Point pE = new Point(point1.X + radius, point1.Y);
                // 模式
                if (InkMethod.GetDistancePP(p, pN) <= threshold || InkMethod.GetDistancePP(p, pW) <= threshold || InkMethod.GetDistancePP(p, pS) <= threshold || InkMethod.GetDistancePP(p, pE) <= threshold)
                {
                    mode = EnumModuleEditType.SizeAll;
                }
                else if (InkMethod.GetDistancePP(p, point1) <= radius)
                {
                    mode = EnumModuleEditType.Move;
                }
            }
            // 直线
            else if (objectType == EnumMetrologyObjectType.line)
            {
                if (!(stroke is CustomLine))
                {
                    return mode;
                }
                Point point1 = (Point)sps[0];
                Point point2 = (Point)sps[1];
                double dist1 = InkMethod.GetDistancePP(point1, p);
                double dist2 = InkMethod.GetDistancePP(point2, p);
                // 标记点附近
                if (dist1 <= threshold || dist2 <= threshold)
                {
                    mode = EnumModuleEditType.SizeAll;
                }
                else
                {
                    // 移动
                    double dist3 = InkMethod.GetDistancePL(p, point1, point2);
                    double angle1 = InkMethod.GetPointAngle(point1, p, point2);
                    double angle2 = InkMethod.GetPointAngle(point2, p, point1);
                    if (dist3 <= threshold && angle1 < 90 && angle2 < 90)
                    {
                        mode = EnumModuleEditType.Move;
                    }
                }
            }
            return mode;
        }

        /// <summary>
        /// 卡尺矩形模板当前的编辑状态：四个角改变大小、旋转；中间平移
        /// </summary>
        /// <param name="stroke"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static EnumCaliperEditType GetCaliperEditType(this Stroke stroke, Point p)
        {
            EnumCaliperEditType mode = EnumCaliperEditType.None;
            // 左上开始，逆时针方向 4 个点坐标
            StylusPointCollection sps = stroke.StylusPoints.Clone();
            double threshold = 5;

            // 移动
            if (InkMethod.GetPointInPolygon(sps, p))
            {
                mode = EnumCaliperEditType.Move;
            }

            // 旋转
            for (int i = 0; i < sps.Count; i++)
            {
                if (InkMethod.GetDistancePP(p, (Point)sps[i]) <= 2 * threshold)
                {
                    mode = EnumCaliperEditType.Rotate;
                }
            }

            // 缩放
            if (InkMethod.GetDistancePP(p, (Point)sps[0]) <= threshold || InkMethod.GetDistancePP(p, (Point)sps[1]) <= threshold || InkMethod.GetDistancePP(p, (Point)sps[2]) <= threshold || InkMethod.GetDistancePP(p, (Point)sps[3]) <= threshold)
            {
                mode = EnumCaliperEditType.Resize;
            }

            return mode;
        }
    }
}