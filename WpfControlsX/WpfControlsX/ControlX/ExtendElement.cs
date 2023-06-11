using System.Windows;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2023 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2023/2/20 11:25:57
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     2023/2/20 11:25:57    CoderMan/CoderMan1012                 
    ///
    public class ExtendElement : DependencyObject
    {
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetAngle(DependencyObject obj)
        {
            return (double)obj.GetValue(AngleProperty);
        }
        public static void SetAngle(DependencyObject obj, double value)
        {
            obj.SetValue(AngleProperty, value);
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.RegisterAttached("Angle", typeof(double), typeof(ExtendElement), new PropertyMetadata(0.0, OnAngleChanged));
        private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                ScaleTransform scaleTrans = new ScaleTransform
                {
                    ScaleY = (double)element.GetValue(ScaleProperty),
                    ScaleX = (double)element.GetValue(ScaleProperty),
                };
                RotateTransform rotateTrans = new RotateTransform
                {
                    Angle = (double)e.NewValue,
                };
                TransformGroup group = new TransformGroup();
                group.Children.Add(scaleTrans);
                group.Children.Add(rotateTrans);

                element.RenderTransformOrigin = (Point)element.GetValue(TransformCenterProperty);
                element.RenderTransform = group;
            }
        }


        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetScale(DependencyObject obj)
        {
            return (double)obj.GetValue(ScaleProperty);
        }
        public static void SetScale(DependencyObject obj, double value)
        {
            obj.SetValue(ScaleProperty, value);
        }
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.RegisterAttached("Scale", typeof(double), typeof(ExtendElement), new PropertyMetadata(1.0, OnScaleChanged));
        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                ScaleTransform scaleTrans = new ScaleTransform
                {
                    ScaleY = (double)e.NewValue,
                    ScaleX = (double)e.NewValue,
                };
                RotateTransform rotateTrans = new RotateTransform
                {
                    Angle = (double)element.GetValue(AngleProperty),
                };
                TransformGroup group = new TransformGroup();
                group.Children.Add(scaleTrans);
                group.Children.Add(rotateTrans);

                element.RenderTransformOrigin = (Point)element.GetValue(TransformCenterProperty);
                element.RenderTransform = group;
            }
        }


        /// <summary>
        /// 旋转中心
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Point GetTransformCenter(DependencyObject obj)
        {
            return (Point)obj.GetValue(TransformCenterProperty);
        }
        public static void SetTransformCenter(DependencyObject obj, Point value)
        {
            obj.SetValue(TransformCenterProperty, value);
        }
        public static readonly DependencyProperty TransformCenterProperty =
            DependencyProperty.RegisterAttached("TransformCenter", typeof(Point), typeof(ExtendElement), new PropertyMetadata(new Point(0.5, 0.5), OnTransformCenterChanged));
        private static void OnTransformCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                ScaleTransform scaleTrans = new ScaleTransform
                {
                    ScaleY = (double)element.GetValue(ScaleProperty),
                    ScaleX = (double)element.GetValue(ScaleProperty),
                };
                RotateTransform rotateTrans = new RotateTransform
                {
                    Angle = (double)element.GetValue(AngleProperty),
                };
                TransformGroup group = new TransformGroup();
                group.Children.Add(scaleTrans);
                group.Children.Add(rotateTrans);

                element.RenderTransformOrigin = (Point)e.NewValue;
                element.RenderTransform = group;
            }
        }


        /// <summary>
        /// 权限等级
        /// </summary>
        public static int GetAuthorityLevel(DependencyObject obj)
        {
            return (int)obj.GetValue(AuthorityLevelProperty);
        }
        public static void SetAuthorityLevel(DependencyObject obj, int value)
        {
            obj.SetValue(AuthorityLevelProperty, value);
        }
        public static readonly DependencyProperty AuthorityLevelProperty =
            DependencyProperty.RegisterAttached("AuthorityLevel", typeof(int), typeof(ExtendElement), new PropertyMetadata(1, OnAuthorityLevelChanged));
        private static void OnAuthorityLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                int value = (int)element.GetValue(AuthorityLevelMinProperty);
                element.IsEnabled = (int)e.NewValue >= value;
            }
        }


        /// <summary>
        /// 最低使用权限
        /// </summary>
        public static int GetAuthorityLevelMin(DependencyObject obj)
        {
            return (int)obj.GetValue(AuthorityLevelMinProperty);
        }
        public static void SetAuthorityLevelMin(DependencyObject obj, int value)
        {
            obj.SetValue(AuthorityLevelMinProperty, value);
        }
        public static readonly DependencyProperty AuthorityLevelMinProperty =
            DependencyProperty.RegisterAttached("AuthorityLevelMin", typeof(int), typeof(ExtendElement), new PropertyMetadata(1, OnAuthorityLevelMinChanged));
        private static void OnAuthorityLevelMinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                int value = (int)element.GetValue(AuthorityLevelProperty);
                element.IsEnabled = value >= (int)e.NewValue;
            }
        }
    }
}