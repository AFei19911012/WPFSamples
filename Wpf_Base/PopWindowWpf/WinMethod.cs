
using System;
using Wpf_Base.MethodNet;

namespace Wpf_Base.PopWindowWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 14:34:48
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 14:34:48    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class WinMethod
    {
        /// <summary>
        /// 信息弹窗：线程
        /// </summary>
        /// <param name="info"></param>
        /// <param name="type"></param>
        public static void ShowMessageTask(string info, EnumWindowType type = EnumWindowType.Info)
        {
            _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
            {
                WindowIcon window = new WindowIcon(type, info);
                _ = window.ShowDialog();
            }));
        }

        /// <summary>
        /// 信息弹窗：等待
        /// </summary>
        /// <param name="info"></param>
        /// <param name="type"></param>
        public static void ShowMessage(string info, EnumWindowType type = EnumWindowType.Info)
        {
            WindowIcon window = new WindowIcon(type, info);
            _ = window.ShowDialog();
        }

        /// <summary>
        /// 加载窗体 自动关闭
        /// </summary>
        /// <param name="content"></param>
        /// <param name="t"></param>
        public static void ShowLoadingWindow(string content = "程序加载中，请稍候 ······", int t = 1000)
        {
            _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
            {
                WindowLoading window = new WindowLoading(content, t);
                _ = window.ShowDialog();
            }));
        }

        /// <summary>
        /// 自动关闭窗体
        /// </summary>
        /// <param name="content"></param>
        /// <param name="t"></param>
        public static void ShowAutoClosedWindow(string content = "程序运行中，请稍候 ······", int t = 1000)
        {
            _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
            {
                WindowAutoClosed window = new WindowAutoClosed(content, t);
                _ = window.ShowDialog();
            }));
        }

        /// <summary>
        /// 自动关闭窗体
        /// </summary>
        /// <param name="content"></param>
        /// <param name="t"></param>
        /// <param name="type"></param>
        public static void ShowAutoClosedWindowIcon(string content = "程序运行中，请稍候 ······", int t = 1000, EnumWindowType type = EnumWindowType.Info)
        {
            _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
            {
                WindowAutoClosedIcon window = new WindowAutoClosedIcon(type, content, t);
                _ = window.ShowDialog();
            }));
        }

        /// <summary>
        /// 打开登陆窗口
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool ShowLoginWindow(string name, string code)
        {
            WindowLogin window = new WindowLogin(name, code);
            _ = window.ShowDialog();
            return window.IsLogin;
        }

    }
}