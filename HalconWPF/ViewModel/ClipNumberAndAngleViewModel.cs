using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using HalconWPF.Method;
using HalconWPF.UserControl;
using System;
using System.Windows;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/9/11 22:58:15
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/9/11 22:58:15    Taosy.W                 
    ///
    public class ClipNumberAndAngleViewModel : ViewModelBase
    {
        private HWindow ho_Window;
        private HImage ho_Image;
        private HSmartWindowControlWPF Halcon;

        /// <summary>
        /// Halcon 控件关联，写在 Loaded 事件里
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as ClipNumberAndAngle).HalconWPF;
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
                // 实例化图像变量
                ho_Image = new HImage();
                // 读取图像
                ho_Image.ReadImage("clip.png");
                // 获取图像尺寸
                ho_Image.GetImageSize(out int width, out int height);
                // 设置 Halcon 窗口显示尺寸
                ho_Window.SetPart(0, 0, height - 1, width - 1);
                ho_Window.DispObj(ho_Image);
                // 自适应比例
                Halcon.SetFullImagePart();
            }
            else if (btn == "Calculate")
            {
                if (ho_Image == null)
                {
                    HandyControl.Controls.Growl.Error("请先加载图片。");
                    return;
                }
                // 二值化，使用 Halcon 的灰度直方图工具来定
                HRegion region = ho_Image.Threshold(0.0, 70.0);
                // 连通域
                HRegion region_connected = region.Connection();
                // 按面积选择，使用 Halcon 特征直方图工具来定
                HRegion regions_selected = region_connected.SelectShape("area", "and", 5000, 8000);
                // 填充
                ho_Window.SetDraw("margin");
                // 颜色
                ho_Window.SetColored(12);
                // 显示
                ho_Window.DispObj(regions_selected);
                // 计算方向
                HTuple phis = regions_selected.OrientationRegion();
                // 区域面积和中心点坐标
                HTuple areas = regions_selected.AreaCenter(out HTuple rows, out HTuple columns);
                // 显示箭头
                double len = 80;
                ho_Window.SetColor("blue");
                ho_Window.SetLineWidth(3);
                ho_Window.DispArrow(rows, columns, rows - (len * phis.TupleSin()), columns + (len * phis.TupleCos()), 4);
                // 显示文本 设置为 image 不是 window
                ho_Window.DispText(areas + "\n" + phis.TupleDeg() + " degrees", rows, columns);
            }
            else if (btn == "SaveWindow")
            {
                // 保存窗体，窗体什么样，就保存什么样
                HImage image = ho_Window.DumpWindowImage();
                image.WriteImage("png", 0, @"D:\MyPrograms\DataSet\halcon\clip_image.png");
                HandyControl.Controls.Growl.Info("窗体保存成功。");
            }
        }
    }
}