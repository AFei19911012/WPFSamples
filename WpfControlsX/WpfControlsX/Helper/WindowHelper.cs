using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 4:16:10
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 4:16:10                     BigWang         首次编写         
    ///
    public static class WindowHelper
    {
        /// <summary>
        ///     获取当前应用中处于激活的一个窗口
        /// </summary>
        /// <returns></returns>
        public static Window GetActiveWindow()
        {
            IntPtr activeWindow = InteropMethods.GetActiveWindow();
            return Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.GetHandle() == activeWindow);
        }

        /// <summary>
        ///     让窗口激活作为前台最上层窗口
        /// </summary>
        /// <param name="window"></param>
        public static void SetWindowToForeground(Window window)
        {
            WindowInteropHelper interopHelper = new WindowInteropHelper(window);
            uint thisWindowThreadId = InteropMethods.GetWindowThreadProcessId(interopHelper.Handle, out _);
            IntPtr currentForegroundWindow = InteropMethods.GetForegroundWindow();
            uint currentForegroundWindowThreadId = InteropMethods.GetWindowThreadProcessId(currentForegroundWindow, out _);

            _ = InteropMethods.AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, true);

            window.Show();
            _ = window.Activate();
            // 去掉和其他线程的输入链接
            _ = InteropMethods.AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, false);

            // 用于踢掉其他的在上层的窗口
            if (window.Topmost != true)
            {
                window.Topmost = true;
                window.Topmost = false;
            }
        }
    }
}