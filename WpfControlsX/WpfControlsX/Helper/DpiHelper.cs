using System;
using System.Windows;
using System.Windows.Media;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/29 17:34:25
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/29 17:34:25                     BigWang         首次编写         
    ///
    internal static class DpiHelper
    {
        private const double LogicalDpi = 96.0;

        [ThreadStatic]
        private static Matrix _transformToDip;

        static DpiHelper()
        {
            IntPtr dC = InteropMethods.GetDC(IntPtr.Zero);
            if (dC != IntPtr.Zero)
            {
                // 沿着屏幕宽度每逻辑英寸的像素数。在具有多个显示器的系统中，这个值对所有显示器都是相同的
                const int logicPixelsX = 88;
                // 沿着屏幕高度每逻辑英寸的像素数
                const int logicPixelsY = 90;
                DeviceDpiX = InteropMethods.GetDeviceCaps(dC, logicPixelsX);
                DeviceDpiY = InteropMethods.GetDeviceCaps(dC, logicPixelsY);
                _ = InteropMethods.ReleaseDC(IntPtr.Zero, dC);
            }
            else
            {
                DeviceDpiX = LogicalDpi;
                DeviceDpiY = LogicalDpi;
            }

            Matrix identity = Matrix.Identity;
            Matrix identity2 = Matrix.Identity;
            identity.Scale(DeviceDpiX / LogicalDpi, DeviceDpiY / LogicalDpi);
            identity2.Scale(LogicalDpi / DeviceDpiX, LogicalDpi / DeviceDpiY);
            TransformFromDevice = new MatrixTransform(identity2);
            TransformFromDevice.Freeze();
            TransformToDevice = new MatrixTransform(identity);
            TransformToDevice.Freeze();
        }

        public static MatrixTransform TransformFromDevice { get; }

        public static MatrixTransform TransformToDevice { get; }

        public static double DeviceDpiX { get; }

        public static double DeviceDpiY { get; }

        public static double LogicalToDeviceUnitsScalingFactorX => TransformToDevice.Matrix.M11;

        public static double LogicalToDeviceUnitsScalingFactorY => TransformToDevice.Matrix.M22;

        public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
        {
            _transformToDip = Matrix.Identity;
            _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);
            return _transformToDip.Transform(devicePoint);
        }

        public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
        {
            Point pt = DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);
            return new Size(pt.X, pt.Y);
        }

        public static Rect LogicalToDeviceUnits(this Rect logicalRect)
        {
            Rect result = logicalRect;
            result.Transform(TransformToDevice.Matrix);
            return result;
        }

        public static Rect DeviceToLogicalUnits(this Rect deviceRect)
        {
            Rect result = deviceRect;
            result.Transform(TransformFromDevice.Matrix);
            return result;
        }

        public static double RoundLayoutValue(double value, double dpiScale)
        {
            double newValue;

            if (!MathHelper.AreClose(dpiScale, 1.0))
            {
                newValue = Math.Round(value * dpiScale) / dpiScale;
                if (double.IsNaN(newValue) || double.IsInfinity(newValue) || MathHelper.AreClose(newValue, double.MaxValue))
                {
                    newValue = value;
                }
            }
            else
            {
                newValue = Math.Round(value);
            }

            return newValue;
        }
    }
}