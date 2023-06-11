using System.Collections.Generic;
using System.Windows;

namespace WpfControlsX.ControlX
{
    /// <summary>
    /// WxLogin.xaml 的交互逻辑
    /// </summary>
    public partial class WxLogin : Window
    {
        /// <summary>
        /// 用户等级
        /// </summary>
        public int Level { get; set; }

 
        /// <summary>
        /// 密码
        /// </summary>
        private List<string> Passwords { get; set; }

        public WxLogin(List<string> accounts, List<string> passwords)
        {
            InitializeComponent();

            Passwords = passwords;

            foreach (string item in accounts)
            {
                _ = CBB_Account.Items.Add(item);
            }
            CBB_Account.SelectedIndex = 0;
            Level = 0;
        }

        private void WxButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WxButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            // 判断当前输入密码是否匹配
            int idx = CBB_Account.SelectedIndex;
            if (idx >= 0 && Passwords[idx] == PB_Password.Password)
            {
                Level = idx + 1;
            }
            else
            {
                _ = MessageBox.Show("密码输入有误", "错误", MessageBoxType.Error, 1000);
            }
            Close();
        }

        private void PB_Password_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                WxButtonLogin_Click(null, null);
            }
        }
    }
}