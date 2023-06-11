using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/4/21 0:50:57
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/4/21 0:50:57                     BigWang         首次编写         
    ///
    public class ElementAdorner : Adorner
    {
        private const double ThumbSize = 10, ElementMiniSize = 20;
        private readonly Thumb tLeftUp;
        private readonly Thumb tRightUp;
        private readonly Thumb tLeftBottom;
        private readonly Thumb tRightBottom;
        private readonly Thumb tMove;
        private readonly Thumb tLeft;
        private readonly Thumb tRight;
        private readonly Thumb tUp;
        private readonly Thumb tBottom;
        private readonly VisualCollection visualCollection;

        public ElementAdorner(UIElement adornedElement) : base(adornedElement)
        {
            visualCollection = new VisualCollection(this)
            {
                (tMove = CreateMoveThumb()),
                (tLeftUp = CreateThumb(Cursors.SizeNWSE, HorizontalAlignment.Left, VerticalAlignment.Top)),
                (tRightUp = CreateThumb(Cursors.SizeNESW, HorizontalAlignment.Right, VerticalAlignment.Top)),
                (tLeftBottom = CreateThumb(Cursors.SizeNESW, HorizontalAlignment.Left, VerticalAlignment.Bottom)),
                (tRightBottom = CreateThumb(Cursors.SizeNWSE, HorizontalAlignment.Right, VerticalAlignment.Bottom)),

                (tLeft = CreateThumb(Cursors.SizeWE, HorizontalAlignment.Left, VerticalAlignment.Stretch)),
                (tUp = CreateThumb(Cursors.SizeNS, HorizontalAlignment.Stretch, VerticalAlignment.Top)),
                (tRight = CreateThumb(Cursors.SizeWE, HorizontalAlignment.Right, VerticalAlignment.Stretch)),
                (tBottom = CreateThumb(Cursors.SizeNS, HorizontalAlignment.Stretch, VerticalAlignment.Bottom)),
            };
        }

        protected override int VisualChildrenCount => visualCollection.Count;

        protected override void OnRender(DrawingContext drawingContext)
        {
            double offset = ThumbSize / 2;
            Size sz = new(ThumbSize, ThumbSize);
            Pen pen = new(new SolidColorBrush(Colors.White), 2.0);
            Point pt1 = new(AdornedElement.RenderSize.Width / 2, 0);
            Point pt2 = new(AdornedElement.RenderSize.Width / 2, -3 * ThumbSize);
            drawingContext.DrawLine(pen, pt1, pt2);

            tMove.Arrange(new Rect(new Point(0, 0), new Size(RenderSize.Width, RenderSize.Height)));
            tLeftUp.Arrange(new Rect(new Point(-offset, -offset), sz));
            tRightUp.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, -offset), sz));
            tLeftBottom.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height - offset), sz));
            tRightBottom.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height - offset), sz));

            tLeft.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height / 2 - offset), sz));
            tUp.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width / 2 - offset, -offset), sz));
            tRight.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height / 2 - offset), sz));
            tBottom.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width / 2 - offset, AdornedElement.RenderSize.Height - offset), sz));
        }

        private Thumb CreateMoveThumb()
        {
            Thumb thumb = new()
            {
                Cursor = Cursors.SizeAll,
                Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(Brushes.Transparent)
                }
            };
            thumb.DragDelta += (s, e) =>
            {
                if (AdornedElement is not FrameworkElement element)
                {
                    return;
                }

                Canvas.SetLeft(element, Canvas.GetLeft(element) + e.HorizontalChange);
                Canvas.SetTop(element, Canvas.GetTop(element) + e.VerticalChange);
            };
            return thumb;
        }

        /// <summary>
        ///     创建Thumb
        /// </summary>
        /// <param name="cursor">鼠标</param>
        /// <param name="horizontal">水平</param>
        /// <param name="vertical">垂直</param>
        /// <returns></returns>
        private Thumb CreateThumb(Cursor cursor, HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            Thumb thumb = new()
            {
                Cursor = cursor,
                Width = ThumbSize,
                Height = ThumbSize,
                HorizontalAlignment = horizontal,
                VerticalAlignment = vertical,
                Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(Brushes.White),
                }
            };

            thumb.DragDelta += (s, e) =>
            {
                if (AdornedElement is not FrameworkElement element)
                {
                    return;
                }

                Resize(element);
                if (thumb.VerticalAlignment == VerticalAlignment.Bottom)
                {
                    if (element.Height + e.VerticalChange > ElementMiniSize)
                    {
                        element.Height += e.VerticalChange;
                    }
                }
                else if (thumb.VerticalAlignment == VerticalAlignment.Top)
                {
                    if (element.Height - e.VerticalChange > ElementMiniSize)
                    {
                        element.Height -= e.VerticalChange;
                        Canvas.SetTop(element, Canvas.GetTop(element) + e.VerticalChange);
                    }
                }

                if (thumb.HorizontalAlignment == HorizontalAlignment.Left)
                {
                    if (element.Width - e.HorizontalChange > ElementMiniSize)
                    {
                        element.Width -= e.HorizontalChange;
                        Canvas.SetLeft(element, Canvas.GetLeft(element) + e.HorizontalChange);
                    }
                }
                else if (thumb.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    if (element.Width + e.HorizontalChange > ElementMiniSize)
                    {
                        element.Width += e.HorizontalChange;
                    }
                }
                e.Handled = true;
            };

            return thumb;
        }


        private void Resize(FrameworkElement fElement)
        {
            if (double.IsNaN(fElement.Width))
            {
                fElement.Width = fElement.RenderSize.Width;
            }

            if (double.IsNaN(fElement.Height))
            {
                fElement.Height = fElement.RenderSize.Height;
            }
        }

        private FrameworkElementFactory GetFactory(Brush back)
        {
            FrameworkElementFactory elementFactory = new(typeof(Ellipse));
            elementFactory.SetValue(Shape.FillProperty, back);
            return elementFactory;
        }


        protected override Visual GetVisualChild(int index)
        {
            return visualCollection[index];
        }
    }
}