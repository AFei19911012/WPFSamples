using System;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/28 23:52:26
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/28 23:52:26                     BigWang         首次编写         
    ///
    internal static class MathHelper
    {
        internal const double DBL_EPSILON = 2.2204460492503131e-016;

        public static bool AreClose(double value1, double value2)
        {
            return value1 == value2 || IsVerySmall(value1 - value2);
        }

        public static double Lerp(double x, double y, double alpha)
        {
            return (x * (1.0 - alpha)) + (y * alpha);
        }

        public static bool IsVerySmall(double value)
        {
            return Math.Abs(value) < 1E-06;
        }

        public static bool IsZero(double value)
        {
            return Math.Abs(value) < 10.0 * DBL_EPSILON;
        }

        public static bool IsFiniteDouble(double x)
        {
            return !double.IsInfinity(x) && !double.IsNaN(x);
        }

        public static double DoubleFromMantissaAndExponent(double x, int exp)
        {
            return x * Math.Pow(2.0, exp);
        }

        public static bool GreaterThan(double value1, double value2)
        {
            return value1 > value2 && !AreClose(value1, value2);
        }

        public static bool GreaterThanOrClose(double value1, double value2)
        {
            return value1 > value2 || AreClose(value1, value2);
        }

        public static double Hypotenuse(double x, double y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }

        public static bool LessThan(double value1, double value2)
        {
            return value1 < value2 && !AreClose(value1, value2);
        }

        public static bool LessThanOrClose(double value1, double value2)
        {
            return value1 < value2 || AreClose(value1, value2);
        }

        public static double EnsureRange(double value, double? min, double? max)
        {
            return min.HasValue && value < min.Value ? min.Value : max.HasValue && value > max.Value ? max.Value : value;
        }

        public static double SafeDivide(double lhs, double rhs, double fallback)
        {
            return !IsVerySmall(rhs) ? lhs / rhs : fallback;
        }
    }
}