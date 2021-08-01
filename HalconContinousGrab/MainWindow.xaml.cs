using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace HalconContinousGrab
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 定义 Halcon 窗口对象、图像变量
        private HWindow ho_Window = null;
        private HObject ho_Image = null;
        // 定义摄像头句柄
        private HTuple hv_AcqHandle = null;
        // 实时抓图线程
        private Thread ho_thread;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 关联控件和 Halcon 窗口对象，最好是放在这里
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ho_Window = HalconWPF.HalconWindow;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string btn = ((Button)sender).Content.ToString();
            // 保存 Halcon 窗口图像
            if (btn == "Save")
            {
                HOperatorSet.DumpWindowImage(out HObject image, ho_Window);
                HOperatorSet.WriteImage(image, "png", 0, @"D:\MyPrograms\VisualStudio2019\WPFprograms\WPFSamples\imags\capture.png");
                return;
            }

            // 触发式抓图
            if (btn == "Acqusition")
            {
                hv_AcqHandle = new HTuple();
                HOperatorSet.GenEmptyObj(out ho_Image);
                hv_AcqHandle.Dispose();
                // 启动摄像头
                //HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive", 8, "default", -1, "false", "auto", "U3V:00F86140145 MV-CE050-30UC", 0, -1, out hv_AcqHandle);
                // 启动笔记本摄像头
                HOperatorSet.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb", -1, "false", "default", "[0] ", 0, -1, out hv_AcqHandle);
                HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                ho_Image.Dispose();
                HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
                HOperatorSet.GetImageSize(ho_Image, out HTuple width, out HTuple height);
                HOperatorSet.SetPart(ho_Window, 0, 0, height, width);
                ho_Window.DispObj(ho_Image);
                // 关闭笔记本摄像头
                HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                ho_Image.Dispose();
                hv_AcqHandle.Dispose();
            }
            // 实时采集
            else if (btn == "Realtime" || btn == "Stop")
            {
                if (btn == "Realtime")
                {
                    hv_AcqHandle = new HTuple();
                    HOperatorSet.GenEmptyObj(out ho_Image);
                    hv_AcqHandle.Dispose();
                    //HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive", 8, "default", -1, "false", "auto", "U3V:00F86140145 MV-CE050-30UC", 0, -1, out hv_AcqHandle);
                    // 启动笔记本自带摄像头
                    HOperatorSet.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb", -1, "false", "default", "[0] ", 0, -1, out hv_AcqHandle);
                    HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
                    BtnContinues.Content = "Stop";
                    // 实时采集线程
                    ho_thread = new Thread(ContinuesGrab);
                    ho_thread.Start();
                    ho_thread.IsBackground = true;
                }
                else
                {
                    // 释放
                    ho_thread.Abort();
                    BtnContinues.Content = "Realtime";
                    HOperatorSet.CloseFramegrabber(hv_AcqHandle);
                    ho_Image.Dispose();
                    hv_AcqHandle.Dispose();
                }
            }
        }


        /// <summary>
        /// 定义实时采集函数
        /// </summary>
        private void ContinuesGrab()
        {
            while (true)
            {
                // 先释放内存
                ho_Image.Dispose();
                HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
                HOperatorSet.GetImageSize(ho_Image, out HTuple width, out HTuple height);
                HOperatorSet.SetPart(ho_Window, 0, 0, height, width);
                HOperatorSet.DispObj(ho_Image, ho_Window);
            }
        }
    }
}
