using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/4/16 21:13:00
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/4/16 21:13:00                     BigWang         首次编写         
    ///
    [ContentProperty(nameof(Content))]
    public class WxDivider : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content), typeof(object), typeof(WxDivider), new PropertyMetadata(default(object)));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation), typeof(Orientation), typeof(WxDivider), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            nameof(ContentTemplate), typeof(DataTemplate), typeof(WxDivider), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register(
            nameof(ContentStringFormat), typeof(string), typeof(WxDivider), new PropertyMetadata(default(string)));

        public string ContentStringFormat
        {
            get => (string)GetValue(ContentStringFormatProperty);
            set => SetValue(ContentStringFormatProperty, value);
        }

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(WxDivider), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty);
            set => SetValue(ContentTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register(
            nameof(LineStroke), typeof(Brush), typeof(WxDivider), new PropertyMetadata(default(Brush)));

        public Brush LineStroke
        {
            get => (Brush)GetValue(LineStrokeProperty);
            set => SetValue(LineStrokeProperty, value);
        }

        public static readonly DependencyProperty LineStrokeThicknessProperty = DependencyProperty.Register(
            nameof(LineStrokeThickness), typeof(double), typeof(WxDivider), new PropertyMetadata(ValueBoxes.Double1Box));

        public double LineStrokeThickness
        {
            get => (double)GetValue(LineStrokeThicknessProperty);
            set => SetValue(LineStrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty LineStrokeDashArrayProperty = DependencyProperty.Register(
            nameof(LineStrokeDashArray), typeof(DoubleCollection), typeof(WxDivider), new PropertyMetadata(new DoubleCollection()));

        public DoubleCollection LineStrokeDashArray
        {
            get => (DoubleCollection)GetValue(LineStrokeDashArrayProperty);
            set => SetValue(LineStrokeDashArrayProperty, value);
        }
    }
}