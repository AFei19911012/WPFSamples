using System.Windows;
using System.Windows.Controls;
using TestUnit.Model;
using TestUnit.ViewModel;
using WpfControlsX.Commands;
using WpfControlsX.ControlX;
using WpfControlsX.Helper;

namespace TestUnit.Demo
{
    /// <summary>
    /// Demo01.xaml 的交互逻辑
    /// </summary>
    public partial class Demo01 : UserControl
    {
        private MainVM VM { get; set; }

        public Demo01()
        {
            InitializeComponent();

            VM = DataContext as MainVM;
        }

        private void SearchBar_OnSearchStarted(object sender, FunctionEventArgs<string> e)
        {
            string key = e.Info;
            if (!(sender is FrameworkElement searchBar && searchBar.Tag is WxListBox listBox))
            {
                return;
            }

            if (string.IsNullOrEmpty(key))
            {
                foreach (DataModel item in listBox.Items)
                {
                    ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                    listBoxItem?.Show(true);
                }
            }
            else
            {
                key = key.ToLower();
                foreach (DataModel item in listBox.Items)
                {
                    string txt = item.Text.ToLower();
                    ListBoxItem listBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                    if (txt.Contains(key))
                    {
                        listBoxItem?.Show(true);
                    }
                    else
                    {
                        listBoxItem?.Show(false);
                    }

                }
            }
        }

        private void WxFolderPathBox_OpenStarted(object sender, FunctionEventArgs<string> e)
        {
            VM.StrFolder = "选择的路径";
        }
    }
}