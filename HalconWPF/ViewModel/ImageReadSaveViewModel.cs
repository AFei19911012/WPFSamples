using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using HalconWPF.UserControl;
using System;
using System.Windows;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/9/11 21:50:53
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/9/11 21:50:53    Taosy.W                 
    ///
    public class ImageReadSaveViewModel : ViewModelBase
    {
        private HImage ho_Image = null;
        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;

        /// <summary>
        /// 控件坐标和图像坐标转换
        /// </summary>
        /// <param name="point"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private Point GetImageHalconPoint(double x, double y, bool flag = true)
        {
            // Halcon 控件宽高
            double cHeight = Halcon.ActualHeight;
            double cWidth = Halcon.ActualWidth;
            // Halcon 图像区域
            double x0 = Halcon.HImagePart.X;
            double y0 = Halcon.HImagePart.Y;
            double imHeight = Halcon.HImagePart.Height;
            double imWidth = Halcon.HImagePart.Width;
            double ratio_y = imHeight / cHeight;
            double ratio_x = imWidth / cWidth;
            // 当前点坐标：相对控件或者相对图像
            double x1;
            double y1;
            // Halcon → Image
            if (flag)
            {

                x1 = (ratio_x * x) + x0;
                y1 = (ratio_y * y) + y0;
            }
            else
            // Image → Halcon
            {
                x1 = (x - x0) / ratio_x;
                y1 = (y - y0) / ratio_y;
            }
            return new Point(x1, y1);
        }

        /// <summary>
        /// 让 Halcon 图像等比例缩放到控件尺寸
        /// </summary>
        private void SetHalconScalingZoom(double width, double height)
        {
            double wRatio = Halcon.ActualWidth / width;
            double hRatio = Halcon.ActualHeight / height;
            double ratio = Math.Min(wRatio, hRatio);
            // Halcon 是 WPF 控件对象
            Halcon.HImagePart = wRatio > hRatio
                ? new Rect
                {
                    X = -0.5 * (Halcon.ActualWidth / ratio - width),
                    Y = 0,
                    Width = Halcon.ActualWidth / ratio,
                    Height = Halcon.ActualHeight / ratio
                }
                : new Rect
                {
                    X = 0,
                    Y = -0.5 * (Halcon.ActualHeight / ratio - height),
                    Width = Halcon.ActualWidth / ratio,
                    Height = Halcon.ActualHeight / ratio
                };
        }


        /// <summary>
        /// Halcon 控件关联，写在 Loaded 事件里
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as ImageReadSave).HalconWPF;
            ho_Window = Halcon.HalconWindow;
        }

        /// <summary>
        /// Button 绑定事件
        /// </summary>
        public RelayCommand<string> CmdImageLoadSave => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(ImageLoadSave)).Value;
        private void ImageLoadSave(string btn)
        {
            if (btn == "LoadImage")
            {
                // 初始化
                ho_Image = new HImage();
                // 读取图像
                ho_Image.ReadImage("claudia.png");
                // 获取图像尺寸
                ho_Image.GetImageSize(out int width, out int height);
                // 设置 Halcon 图像显示尺寸，一般来说，图像会铺满 Halcon 控件，因此会有一定程度拉伸
                ho_Window.SetPart(0, 0, height - 1, width - 1);
                // 显示图像
                ho_Window.DispObj(ho_Image);
                // 设置原图像比例缩放，这个效果和双击左键效果一样
                //Halcon.SetFullImagePart();
                // 等价于上面这句话
                SetHalconScalingZoom(width, height);
            }
            else if (btn == "SaveWindow")
            {
                // 保存窗体，窗体什么样，就保存什么样
                HImage image = ho_Window.DumpWindowImage();
                image.WriteImage("png", 0, @"D:\MyPrograms\VisualStudio2019\WPFprograms\WPFSamples\images\window_image.png");
                HandyControl.Controls.Growl.Info("窗体保存成功。");
            }
            else if (btn == "SaveImage")
            {
                if (ho_Image == null)
                {
                    HandyControl.Controls.Growl.Error("请先读取图像。");
                    return;
                }
                // 保存原图
                ho_Image.WriteImage("png", 0, @"D:\MyPrograms\VisualStudio2019\WPFprograms\WPFSamples\images\image_image.png");
                HandyControl.Controls.Growl.Info("图像保存成功。");
            }
        }
    }
}
