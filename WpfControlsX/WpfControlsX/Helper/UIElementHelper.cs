using System.Windows;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 6:15:16
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 6:15:16                     BigWang         首次编写         
    ///
    public static class UIElementHelper
    {
        /// <summary>
        ///     显示元素
        /// </summary>
        /// <param name="element"></param>
        public static void Show(this UIElement element)
        {
            element.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     显示元素
        /// </summary>
        /// <param name="element"></param>
        /// <param name="show"></param>
        public static void Show(this UIElement element, bool show)
        {
            element.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        ///     不现实元素，但保留空间
        /// </summary>
        /// <param name="element"></param>
        public static void Hide(this UIElement element)
        {
            element.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     不显示元素，且不保留空间
        /// </summary>
        /// <param name="element"></param>
        public static void Collapse(this UIElement element)
        {
            element.Visibility = Visibility.Collapsed;
        }
    }
}