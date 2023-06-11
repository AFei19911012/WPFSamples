using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 19:11:53
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 19:11:53                     BigWang         首次编写         
    ///
    public class AdornerContainer : Adorner
    {
        private UIElement _child;

        public AdornerContainer(UIElement adornedElement) : base(adornedElement)
        {
        }

        public UIElement Child
        {
            get => _child;
            set
            {
                if (value == null)
                {
                    RemoveVisualChild(_child);
                    // ReSharper disable once ExpressionIsAlwaysNull
                    _child = value;
                    return;
                }
                AddVisualChild(value);
                _child = value;
            }
        }

        protected override int VisualChildrenCount => _child != null ? 1 : 0;

        protected override Size ArrangeOverride(Size finalSize)
        {
            _child?.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return index == 0 && _child != null ? _child : base.GetVisualChild(index);
        }
    }
}