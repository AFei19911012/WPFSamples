using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf_Base.MethodNet
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/02 09:50:26
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/02 09:50:26    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class ImgMethod
    {
        /// <summary>
        /// Bitmap --> BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage BitmapToBitmapImage(this Bitmap bitmap)
        {
            BitmapImage image = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                // System.ArgumentException:“参数无效。”
                bitmap.Save(ms, ImageFormat.Png);
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
            }
            return image;
        }

        /// <summary>
        /// BitmapSource --> Bitmap
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static Bitmap BitmapSourceToBitmap(BitmapSource bs)
        {
            Bitmap bmp = new Bitmap(bs.PixelWidth, bs.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            bs.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        /// <summary>
        /// Bitmap --> WriteableBitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static WriteableBitmap BitmapToWriteableBitmap(this Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            WriteableBitmap wb = new WriteableBitmap(640, 480, 96, 96, PixelFormats.Bgra32, null);
            int pixelBytes = width * height * 4;
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            wb.Lock();
            unsafe
            {
                // System.AccessViolationException:“尝试读取或写入受保护的内存。这通常指示其他内存已损坏。”
                Buffer.MemoryCopy(data.Scan0.ToPointer(), wb.BackBuffer.ToPointer(), pixelBytes, pixelBytes);
            }
            wb.AddDirtyRect(new Int32Rect(0, 0, width, height));
            wb.Unlock();
            bitmap.UnlockBits(data);
            return wb;
        }

        /// <summary>
        /// Bitmap --> bytes
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                byte[] bytes = ms.GetBuffer();
                //byte[] bytes = ms.ToArray();
                return bytes;
            }
        }


        /// <summary>
        /// UI 保存成图片
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="fileName"></param>
        public static void SaveToPng(this FrameworkElement ui, string fileName)
        {
            int width = (int)ui.ActualWidth;
            int height = (int)ui.ActualHeight;
            RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(ui);

            BitmapEncoder encoder;
            string ext = Path.GetExtension(fileName);
            encoder = ext == ".bmp" ? new BmpBitmapEncoder() : ext == ".jpg" ? new JpegBitmapEncoder() : (BitmapEncoder)new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (FileStream stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }
    }
}