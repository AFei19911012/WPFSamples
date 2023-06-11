using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfControlsX.ControlX;
using MessageBox = WpfControlsX.ControlX.MessageBox;

namespace WpfControlsX.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/16 15:13:57
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/16 15:13:57                     BigWang         首次编写         
    ///
    public static class DialogHelper
    {
        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="showingTime"></param>
        /// <returns></returns>
        public static void Info(string message, int showingTime = 0)
        {
            _ = MessageBox.Show(message, "消息", MessageBoxType.Info, showingTime);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="showingTime"></param>
        /// <returns></returns>
        public static void Warning(string message, int showingTime = 0)
        {
            _ = MessageBox.Show(message, "警告", MessageBoxType.Warning, showingTime);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="showingTime"></param>
        /// <returns></returns>
        public static void Error(string message, int showingTime = 0)
        {
            _ = MessageBox.Show(message, "错误", MessageBoxType.Error, showingTime);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="showingTime"></param>
        /// <returns></returns>
        public static void Success(string message, int showingTime = 0)
        {
            _ = MessageBox.Show(message, "成功", MessageBoxType.Success, showingTime);
        }

        /// <summary>
        /// 询问
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="showingTime"></param>
        /// <returns></returns>
        public static MessageBoxResult Ask(string message, int showingTime = 0)
        {
            return MessageBox.Show(message, "询问", MessageBoxType.Ask, showingTime);
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="passwords"></param>
        /// <returns></returns>
        public static int Login(List<string> accounts, List<string> passwords)
        {
            WxLogin login = new WxLogin(accounts, passwords);

            Window win = null;
            if (Application.Current.Windows.Count > 0)
            {
                win = Application.Current.Windows.OfType<Window>().FirstOrDefault(o => o.IsActive);
            }

            if (win == null)
            {
                login.Show();
            }
            else
            {
                login.Owner = win;
                _ = login.ShowDialog();
            }
            return login.Level;
        }

        /// <summary>
        /// 设置 IP
        /// idx = 0  ModBus
        /// idx = 1  TCP
        /// idx = 2  MC
        /// idx = 3  S7
        /// </summary>
        /// <param name="idx"></param>
        public static void IP(int idx = 0)
        {
            WxIP login = new WxIP(idx);

            Window win = null;
            if (Application.Current.Windows.Count > 0)
            {
                win = Application.Current.Windows.OfType<Window>().FirstOrDefault(o => o.IsActive);
            }

            if (win == null)
            {
                login.Show();
            }
            else
            {
                login.Owner = win;
                _ = login.ShowDialog();
            }
        }

        /// <summary>
        /// 数值输入
        /// </summary>
        /// <param name="idx"></param>
        public static double NumericalKeyboard(double value, double min = 0, double max = 1000, bool isDouble = true)
        {
            WxNumericalKeyboard keyboard = new WxNumericalKeyboard(value)
            {
                Minimum = min,
                Maximum = max,
                IsDouble = isDouble,
            };

            Window win = null;
            if (Application.Current.Windows.Count > 0)
            {
                win = Application.Current.Windows.OfType<Window>().FirstOrDefault(o => o.IsActive);
            }

            if (win == null)
            {
                keyboard.Show();
            }
            else
            {
                keyboard.Owner = win;
                _ = keyboard.ShowDialog();
            }
            return keyboard.Result;
        }
    }
}