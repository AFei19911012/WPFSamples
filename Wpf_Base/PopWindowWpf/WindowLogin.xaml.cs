using System.Windows;

namespace Wpf_Base.PopWindowWpf
{
    /// <summary>
    /// WindowLogin.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLogin : Window
    {
        private string UserName { get; set; }
        private string UserCode { get; set; }

        public bool IsLogin { get; set; } = false;

        public WindowLogin(string name, string code)
        {
            InitializeComponent();

            UserName = name;
            UserCode = code;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (UserName == PWB_name.Password && UserCode == PWB_code.Password)
            {
                IsLogin = true;
                WinMethod.ShowAutoClosedWindowIcon("登陆成功", 500, EnumWindowType.Success);
            }
            else
            {
                IsLogin = false;
                WinMethod.ShowAutoClosedWindowIcon("登陆失败：用户名或密码有误", 1000, EnumWindowType.Error);
            }
            Close();
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            IsLogin = false;
            WinMethod.ShowAutoClosedWindowIcon("账号登出", 1000, EnumWindowType.Warning);
            Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}