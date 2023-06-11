using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfControlsX.ControlX;
using WpfControlsX.Helper;

namespace TestUnit.Demo
{
    /// <summary>
    /// Demo05.xaml 的交互逻辑
    /// </summary>
    public partial class Demo05 : UserControl
    {
        public Demo05()
        {
            InitializeComponent();
        }

        private void WxButton_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as WxButton).Content.ToString();
            if (name.StartsWith("Info"))
            {
                DialogHelper.Info("消息内容");
            }
            else if (name.StartsWith("Warning"))
            {
                DialogHelper.Warning("警告内容");
            }
            else if (name.StartsWith("Error"))
            {
                DialogHelper.Error("错误内容");
            }
            else if (name.StartsWith("Success"))
            {
                DialogHelper.Success("成功内容");
            }
            else if (name.StartsWith("Ask"))
            {
                _ = DialogHelper.Ask("询问内容");
            }
            else if (name.StartsWith("AutoClosed"))
            {
                DialogHelper.Info("2秒后自动关闭", 2000);
            }
            else if (name.StartsWith("Login"))
            {
                List<string> accounts = new List<string> { "操作员", "工程师", "厂家" };
                List<string> passwords = new List<string> { "000", "111", "222" };
                int level = DialogHelper.Login(accounts, passwords);

                DialogHelper.Info("当前用户等级：" + level);
            }
            else if (name.StartsWith("阻塞"))
            {
                bool isBusy = true;
                int i = 0;
                _ = Task.Run(() =>
                {
                    while (isBusy)
                    {
                        i++;
                        Dispatcher.Invoke(new Action(() =>
                        {
                            MessageBoxResult result = DialogHelper.Ask(string.Format("当前第 {0} 次弹出", i));
                            if (result == MessageBoxResult.OK)
                            {
                                isBusy = false;
                            }
                        }));

                        Thread.Sleep(1000);
                    }
                });
            }
            else if (name.StartsWith("非阻塞"))
            {
                bool isBusy = true;
                int i = 0;
                _ = Task.Run(() =>
                {
                    while (isBusy)
                    {
                        i++;
                        _ = Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MessageBoxResult result = DialogHelper.Ask(string.Format("当前第 {0} 次弹出", i));
                            if (result == MessageBoxResult.OK)
                            {
                                isBusy = false;
                            }
                        }));
                        Thread.Sleep(1000);
                    }
                });
            }
            else if (name.StartsWith("一次非阻塞"))
            {
                bool isBusy = true;
                bool isExist = false;
                int i = 0;
                _ = Task.Run(() =>
                {
                    while (isBusy)
                    {
                        i++;
                        _ = Dispatcher.BeginInvoke(new Action(() =>
                        {
                            // 只显示一次弹窗
                            if (!isExist)
                            {
                                isExist = true;
                                MessageBoxResult result = DialogHelper.Ask(string.Format("当前第 {0} 次弹出", i));
                                if (result == MessageBoxResult.OK)
                                {
                                    isBusy = false;
                                }

                                // 关闭弹窗后重置
                                isExist = false;
                            }
                        }));
                        Thread.Sleep(1000);
                    }
                });
            }
            else if (name.StartsWith("IP"))
            {
                DialogHelper.IP();
            }
        }

        private void WxTextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WxTextBox txt = sender as WxTextBox;
            double value = DialogHelper.NumericalKeyboard(double.Parse(txt.Text));
            txt.Text = value.ToString("F2");
        }

        private void WxTextBoxInt_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WxTextBox txt = sender as WxTextBox;
            int value = (int)DialogHelper.NumericalKeyboard(double.Parse(txt.Text), -1000, 1000, false);
            txt.Text = value.ToString();
        }
    }
}