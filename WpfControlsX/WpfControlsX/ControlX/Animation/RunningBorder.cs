using System;
using System.Windows;
using System.Windows.Controls;
using WpfControlsX.Helper;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/29 17:33:41
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/29 17:33:41                     BigWang         首次编写         
    ///
    public class RunningBorder : Border
    {
        private bool _test;

        protected override Size MeasureOverride(Size constraint)
        {
            if (_test)
            {
                _test = false;
                return constraint;
            }

            UIElement child = Child;
            Thickness borderThickness = BorderThickness;
            Thickness padding = Padding;

            if (UseLayoutRounding)
            {
                double dpiScaleX = DpiHelper.DeviceDpiX;
                double dpiScaleY = DpiHelper.DeviceDpiY;

                borderThickness = new Thickness(
                    DpiHelper.RoundLayoutValue(borderThickness.Left, dpiScaleX),
                    DpiHelper.RoundLayoutValue(borderThickness.Top, dpiScaleY),
                    DpiHelper.RoundLayoutValue(borderThickness.Right, dpiScaleX),
                    DpiHelper.RoundLayoutValue(borderThickness.Bottom, dpiScaleY));
            }

            Size borderSize = ConvertThickness2Size(borderThickness);
            Size paddingSize = ConvertThickness2Size(padding);
            Size mySize = new Size();

            if (child != null)
            {
                Size combined = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);
                Size borderConstraint = new Size(Math.Max(0.0, constraint.Width - combined.Width), Math.Max(0.0, constraint.Height - combined.Height));
                Size childConstraint = new Size(Math.Max(0.0, double.PositiveInfinity - combined.Width), Math.Max(0.0, double.PositiveInfinity - combined.Height));


                child.Measure(borderConstraint);
                Size childSize = child.DesiredSize;

                mySize.Width = childSize.Width + combined.Width;
                mySize.Height = childSize.Height + combined.Height;

                child.Measure(childConstraint);
            }
            else
            {
                mySize = new Size(borderSize.Width + paddingSize.Width, borderSize.Height + paddingSize.Height);
            }

            return mySize;
        }

        private static Size ConvertThickness2Size(Thickness th)
        {
            return new Size(th.Left + th.Right, th.Top + th.Bottom);
        }
    }
}