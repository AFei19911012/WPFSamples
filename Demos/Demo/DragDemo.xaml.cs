using Demos.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demos.Demo
{
    /// <summary>
    /// DragDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DragDemo : UserControl
    {
        public ObservableCollection<DataModel> Source
        {
            get { return (ObservableCollection<DataModel>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ObservableCollection<DataModel>), typeof(DragDemo), new PropertyMetadata(null));


        public DragDemo()
        {
            InitializeComponent();

            InitSource();
        }

        private void InitSource()
        {
            Source = new ObservableCollection<DataModel>()
            {
                new DataModel() { Name = "ProfilePicture.jpg", Image = "pack://application:,,,/Image/ProfilePicture.jpg" },
                new DataModel() { Name = "Earth.ico", Image = "pack://application:,,,/Image/Earth.ico" },
            };

        }

        private void TB_DragSrc_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                TB_DragDst.Text = "";
                DragDrop.DoDragDrop(TB_DragSrc, TB_DragSrc.Text, DragDropEffects.Copy);
            }
        }

        private void TB_DragDst_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(string)) is string)
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void TB_DragDst_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(string)) is string txt)
            {
                TB_DragDst.Text = txt;
            }
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(DataModel)) is DataModel)
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(DataModel)) is DataModel data)
            {
                Img_Dst.Source = new BitmapImage(new Uri(data.Image, UriKind.RelativeOrAbsolute));
                TB_Dst.Text = data.Name;
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is Border border && border.DataContext != null)
            {
                DragDrop.DoDragDrop(border, border.DataContext, DragDropEffects.Copy);
            }
        }
    }
}
