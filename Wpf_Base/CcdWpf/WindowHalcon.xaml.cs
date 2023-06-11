using HalconDotNet;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_Base.CcdWpf
{
    /// <summary>
    /// WindowHalcon.xaml 的交互逻辑
    /// </summary>
    public partial class WindowHalcon : Window
    {
        private int CamId { get; set; } = -1;
        private bool IsGrabbing { get; set; } = true;
        private bool IsFirstShow { get; set; } = true;

        private HObject Ho_Image = null;

        public WindowHalcon(int camId)
        {
            InitializeComponent();

            CamId = camId;
            TB_Cam.Text = CcdManager.Instance.HikCamInfos[camId].UserName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HOperatorSet.GenEmptyObj(out Ho_Image);
            Task task = new Task(CaptureImageTask);
            task.Start();
        }

        private void HalconWPF_HMouseMove(object sender, HSmartWindowControlWPF.HMouseEventArgsWPF e)
        {
            if (Ho_Image != null && Ho_Image.IsInitialized())
            {
                int row = (int)e.Row;
                int col = (int)e.Column;
                // 实时容易出问题
                string value;
                try
                {
                    HOperatorSet.GetGrayval(Ho_Image, row, col, out HTuple ho_Grayval);
                    value = ho_Grayval.ToString();
                }
                catch (Exception)
                {
                    value = "null";
                }
                MyPixelValueControl.RefreshInfo(col, row, value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsGrabbing = false;
            if (CamId > -1)
            {
                _ = CcdManager.Instance.Stop(CamId);
            }
            Close();
        }

        /// <summary>
        /// 抓图线程
        /// </summary>
        private void CaptureImageTask()
        {
            while (IsGrabbing)
            {
                if (CamId > -1)
                {
                    try
                    {
                        Ho_Image.Dispose();
                        CcdManager.Instance.GetHalconImage(CamId, ref Ho_Image);
                        if (Ho_Image.IsInitialized())
                        {
                            HalconWPF.HalconWindow.DispObj(Ho_Image);
                            if (IsFirstShow)
                            {
                                IsFirstShow = false;
                                _ = Dispatcher.BeginInvoke(new Action(() => { HalconWPF.SetFullImagePart(); }));
                            }
                        }
                        Thread.Sleep(10);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}