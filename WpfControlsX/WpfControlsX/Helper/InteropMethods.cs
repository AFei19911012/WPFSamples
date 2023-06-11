using System;
using System.Runtime.InteropServices;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/30 4:24:30
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/30 4:24:30                     BigWang         首次编写         
    ///
    internal class InteropMethods
    {
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetDC(IntPtr ptr);

        [DllImport(ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(ExternDll.User32, SetLastError = true)]
        internal static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool AttachThreadInput(in uint currentForegroundWindowThreadId,
        in uint thisWindowThreadId, bool isAttach);


        [DllImport(ExternDll.User32)]
        internal static extern IntPtr GetActiveWindow();


        [DllImport(ExternDll.User32)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        internal static int GetWindowLong(IntPtr hWnd, ExternDll.GWL nIndex)
        {
            return GetWindowLong(hWnd, (int)nIndex);
        }


        [DllImport(ExternDll.User32, CharSet = CharSet.Unicode)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        internal static int SetWindowLong(IntPtr hWnd, ExternDll.GWL nIndex, int dwNewLong)
        {
            return SetWindowLong(hWnd, (int)nIndex, dwNewLong);
        }
    }
}