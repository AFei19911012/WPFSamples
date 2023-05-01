using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Forms;

namespace Demos.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/5/1 17:45:12
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/5/1 17:45:12                     BigWang         首次编写         
    ///
    public class ImageHelper
    {
        #region Bitmap、BitmapImage、RenderTargetBitmap、ImageSource、byte[] 之间转换
        /// <summary>
        /// Bitmap 转 BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);

                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.StreamSource = ms;
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }


        /// <summary>
        /// BitmapImage 转 Bitmap
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Image/demo.png", UriKind.RelativeOrAbsolute));
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);

                Bitmap bitmap = new Bitmap(stream);
                return new Bitmap(bitmap);
            }
        }


        /// <summary>
        /// RenderTargetBitmap 转 BitmapImage
        /// </summary>
        /// <param name="renderTargetBitmap"></param>
        /// <returns></returns>
        public static BitmapImage RenderTargetBitmapToBitmapImage(RenderTargetBitmap renderTargetBitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                encoder.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);

                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.StreamSource = ms;
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }


        /// <summary>
        /// ImageSource 转 Bitmap
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static Bitmap ImageSourceToBitmap(ImageSource imageSource)
        {
            BitmapSource bitmapSource = (BitmapSource)imageSource;
            Bitmap bitmap = new Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData data = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bitmapSource.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bitmap.UnlockBits(data);
            return bitmap;
        }


        /// <summary>
        /// BitmapImage 转 byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapImageToBtyes(BitmapImage bitmap)
        {
            Stream s = bitmap.StreamSource;
            s.Position = 0;
            using (BinaryReader binaryReader = new BinaryReader(s))
            {
                byte[] bytes = binaryReader.ReadBytes((int)s.Length);
                return bytes;
            }
        }


        /// <summary>
        /// byte[] 转 BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage BytesToBitmapImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.StreamSource = ms;
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }


        /// <summary>
        /// Bitmap 转 byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byte[] bytes = new byte[ms.Length];
                bytes = ms.ToArray();
                return bytes;
            }
        }


        /// <summary>
        /// byte[] 转 Bitmap
        /// </summary>
        /// <param name="bytes"></param>
        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Bitmap bitmap = new Bitmap(ms);
                return bitmap;
            }
        }
        #endregion


        /// <summary>
        /// 图像保存
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void Save(Bitmap bitmap, string filename, ImageFormat format)
        {
            bitmap.Save(filename, format);
        }


        /// <summary>
        /// UI 控件保存成图片
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="filename"></param>
        public static void UiSaveToPng(FrameworkElement ui, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                int width = (int)ui.ActualWidth;
                int height = (int)ui.ActualHeight;
                RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32);
                bmp.Render(ui);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(fs);
                fs.Close();
            }
        }


        /// <summary>
        /// UI 转 BitmapImage
        /// </summary>
        /// <param name="ui"></param>
        public static BitmapImage UiToBitmapImage(FrameworkElement ui)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int width = (int)ui.ActualWidth;
                int height = (int)ui.ActualHeight;
                RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Pbgra32);
                bmp.Render(ui);
                return RenderTargetBitmapToBitmapImage(bmp);
            }
        }


        /// <summary>
        /// 屏幕截图
        /// </summary>
        /// <param name="filename"></param>
        public static void CaptureScreen(string filename)
        {
            System.Drawing.Size size = Screen.PrimaryScreen.Bounds.Size;
            Bitmap bitmap = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(0, 0, 0, 0, size);
            bitmap.Save(filename, ImageFormat.Png);
        }
    }
}