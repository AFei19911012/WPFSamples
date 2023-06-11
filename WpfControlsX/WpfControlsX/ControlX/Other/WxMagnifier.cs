using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WpfControlsX.Helper;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/4/19 23:10:33
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/4/19 23:10:33                     BigWang         首次编写         
    ///
    [TemplatePart(Name = ElementVisualBrush, Type = typeof(VisualBrush))]
    public class WxMagnifier : AdornerElement
    {
        private AdornerContainer _adornerContainer;

        private Size _viewboxSize;

        private const string ElementVisualBrush = "PART_VisualBrush";

        private VisualBrush _visualBrush = new();

        private readonly TranslateTransform _translateTransform;

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            nameof(HorizontalOffset), typeof(double), typeof(WxMagnifier), new PropertyMetadata(ValueBoxes.Double0Box));

        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            nameof(VerticalOffset), typeof(double), typeof(WxMagnifier), new PropertyMetadata(ValueBoxes.Double0Box));

        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }


        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxMagnifier), new PropertyMetadata(new CornerRadius(0)));


        public static WxMagnifier Default => new();

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            nameof(Scale), typeof(double), typeof(WxMagnifier), new PropertyMetadata(5.0, OnScaleChanged), ValidateHelper.IsInRangeOfPosDouble);

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WxMagnifier)d).UpdateViewboxSize();
        }

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public WxMagnifier()
        {
            _translateTransform = new TranslateTransform();
            RenderTransform = _translateTransform;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var visualBrush = GetTemplateChild(ElementVisualBrush) as VisualBrush ?? new VisualBrush();
            visualBrush.Viewbox = _visualBrush.Viewbox;
            _visualBrush = visualBrush;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateViewboxSize();
        }

        private void UpdateViewboxSize()
        {
            _viewboxSize = new Size(ActualWidth / Scale, ActualHeight / Scale);
        }

        protected sealed override void OnTargetChanged(FrameworkElement element, bool isNew)
        {
            base.OnTargetChanged(element, isNew);

            if (element == null) return;

            if (!isNew)
            {
                element.MouseEnter -= Element_MouseEnter;
                element.MouseLeave -= Element_MouseLeave;
                element.MouseMove -= Element_MouseMove;
                ElementTarget = null;
            }
            else
            {
                element.MouseEnter += Element_MouseEnter;
                element.MouseLeave += Element_MouseLeave;
                element.MouseMove += Element_MouseMove;
                ElementTarget = element;
            }
        }

        protected override void Dispose() => HideAdornerElement();

        private void UpdateLocation()
        {
            var targetPoint = Mouse.GetPosition(Target);
            var subX = targetPoint.X - _visualBrush.Viewbox.Width / 2;
            var subY = targetPoint.Y - _visualBrush.Viewbox.Height / 2;

            var targetVector = VisualTreeHelper.GetOffset(Target);
            _visualBrush.Viewbox = new Rect(new Point(subX + targetVector.X, subY + targetVector.Y), _viewboxSize);

            var adornerPoint = Mouse.GetPosition(_adornerContainer);
            _translateTransform.X = adornerPoint.X + HorizontalOffset;
            _translateTransform.Y = adornerPoint.Y + VerticalOffset;
        }

        private void Element_MouseMove(object sender, MouseEventArgs e) => UpdateLocation();

        private void Element_MouseLeave(object sender, MouseEventArgs e) => HideAdornerElement();

        private void Element_MouseEnter(object sender, MouseEventArgs e) => ShowAdornerElement();

        private void HideAdornerElement()
        {
            if (_adornerContainer == null) return;
            var layer = AdornerLayer.GetAdornerLayer(Target);

            if (layer != null)
            {
                layer.Remove(_adornerContainer);
            }
            else if (_adornerContainer is { Parent: AdornerLayer parent })
            {
                parent.Remove(_adornerContainer);
            }

            if (_adornerContainer != null)
            {
                _adornerContainer.Child = null;
                _adornerContainer = null;
            }
        }

        private void ShowAdornerElement()
        {
            if (_adornerContainer == null)
            {
                var layer = AdornerLayer.GetAdornerLayer(Target);
                if (layer == null) return;

                _adornerContainer = new AdornerContainer(layer)
                {
                    Child = this
                };
                layer.Add(_adornerContainer);
            }
            this.Show();
        }
    }
}
