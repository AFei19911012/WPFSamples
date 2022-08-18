using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Demos.Demo
{
    /// <summary>
    /// OpenFileFolderDemo.xaml 的交互逻辑
    /// </summary>
    public partial class OpenFileFolderDemo : UserControl
    {
        public OpenFileFolderDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Content.ToString();
            if (name.EndsWith(" OpenFileDialog"))
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "选择图片",
                    Filter = "图像文件(*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp",
                    RestoreDirectory = true,
                };
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                string filename = dialog.FileName;
                _ = MessageBox.Show(filename);
            }
            else if (name.EndsWith(" SaveFileDialog"))
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Title = "保存图像",
                    Filter = "图像文件(*.bmp)|*.bmp",
                    RestoreDirectory = true,
                    FileName = "000.bmp"
                };
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                string filename = dialog.FileName;
                _ = MessageBox.Show(filename);
            }
            else if (name.EndsWith(" FolderBrowserDialog"))
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                string foldername = dialog.SelectedPath.Trim();
                _ = MessageBox.Show(foldername);
            }
            else if (name.EndsWith(" CommonOpenFileDialog"))
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog
                {
                    IsFolderPicker = true
                };

                if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }
                string folderName = dialog.FileName;
                _ = MessageBox.Show(folderName);
            }
        }
    }
}
