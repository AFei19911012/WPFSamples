using Demos.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// ImageDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ImageDemo : UserControl
    {
        public ImageDemo()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Image/ProfilePicture.jpg", UriKind.RelativeOrAbsolute));
            MyImage.Source = bitmapImage;
            Bitmap bmp = ImageHelper.BitmapImageToBitmap(bitmapImage);
            byte[] bytes = ImageHelper.BitmapToBytes(bmp);
            Bitmap bitmap = ImageHelper.BytesToBitmap(bytes);
            ImageHelper.Save(bitmap, "Data/test01.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            ImageHelper.UiSaveToPng(MyGrid, "Data/test02.png");
            MyImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/Data/test02.png", UriKind.RelativeOrAbsolute));
        }
    }
}