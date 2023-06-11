using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 19:18:03
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 19:18:03                     BigWang         首次编写         
    ///
    public static class VisualHelper
    {
        internal static VisualStateGroup TryGetVisualStateGroup(DependencyObject d, string groupName)
        {
            FrameworkElement root = GetImplementationRoot(d);
            return root == null
                ? null
                : (VisualStateManager
                .GetVisualStateGroups(root)?
                .OfType<VisualStateGroup>()
                .FirstOrDefault(group => string.CompareOrdinal(groupName, group.Name) == 0));
        }

        internal static FrameworkElement GetImplementationRoot(DependencyObject d)
        {
            return 1 == VisualTreeHelper.GetChildrenCount(d)
                ? VisualTreeHelper.GetChild(d, 0) as FrameworkElement
                : null;
        }

        public static T GetChild<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null)
            {
                return default;
            }

            if (d is T t)
            {
                return t;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(d, i);

                T result = GetChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return default;
        }

        public static T GetParent<T>(DependencyObject d) where T : DependencyObject
        {
            return d switch
            {
                null => default,
                T t => t,
                Window => null,
                _ => GetParent<T>(VisualTreeHelper.GetParent(d))
            };
        }

        public static IntPtr GetHandle(this Visual visual)
        {
            return (PresentationSource.FromVisual(visual) as HwndSource)?.Handle ?? IntPtr.Zero;
        }

        internal static void HitTestVisibleElements(Visual visual, HitTestResultCallback resultCallback, HitTestParameters parameters)
        {
            VisualTreeHelper.HitTest(visual, ExcludeNonVisualElements, resultCallback, parameters);
        }

        private static HitTestFilterBehavior ExcludeNonVisualElements(DependencyObject potentialHitTestTarget)
        {
            return potentialHitTestTarget is not Visual
                ? HitTestFilterBehavior.ContinueSkipSelfAndChildren
                : potentialHitTestTarget is not UIElement uIElement || (uIElement.IsVisible && uIElement.IsEnabled)
                ? HitTestFilterBehavior.Continue
                : HitTestFilterBehavior.ContinueSkipSelfAndChildren;
        }

        internal static bool ModifyStyle(IntPtr hWnd, int styleToRemove, int styleToAdd)
        {
            int windowLong = InteropMethods.GetWindowLong(hWnd, ExternDll.GWL.STYLE);
            int num = (windowLong & ~styleToRemove) | styleToAdd;
            if (num == windowLong)
            {
                return false;
            }

            _ = InteropMethods.SetWindowLong(hWnd, ExternDll.GWL.STYLE, num);
            return true;
        }
    }
}