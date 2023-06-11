using System;
using System.Windows;
using System.Windows.Media;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/28 23:57:59
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/28 23:57:59                     BigWang         首次编写         
    ///
    public static class GeometryHelper
    {
        /// <summary>
        /// 获取路径总长度
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static double GetTotalLength(this Geometry geometry)
        {
            if (geometry == null)
            {
                return 0;
            }

            PathGeometry pathGeometry = PathGeometry.CreateFromGeometry(geometry);
            pathGeometry.GetPointAtFractionLength(1e-4, out Point point, out _);
            double length = (pathGeometry.Figures[0].StartPoint - point).Length * 1e+4;

            return length;
        }

        /// <summary>
        /// 获取路径总长度
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="size"></param>
        /// <param name="strokeThickness"></param>
        /// <returns></returns>
        public static double GetTotalLength(this Geometry geometry, Size size, double strokeThickness = 1)
        {
            if (geometry == null)
            {
                return 0;
            }

            if (MathHelper.IsVerySmall(size.Width) || MathHelper.IsVerySmall(size.Height))
            {
                return 0;
            }

            double length = GetTotalLength(geometry);
            double sw = geometry.Bounds.Width / size.Width;
            double sh = geometry.Bounds.Height / size.Height;
            double min = Math.Min(sw, sh);

            if (MathHelper.IsVerySmall(min) || MathHelper.IsVerySmall(strokeThickness))
            {
                return 0;
            }

            length /= min;
            return length / strokeThickness;
        }
    }
}