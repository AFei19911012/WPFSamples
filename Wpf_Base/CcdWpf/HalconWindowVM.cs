using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Wpf_Base.ControlsWpf;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.CcdWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/01 08:41:27
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/01 08:41:27    CoderMan/CoderdMan1012         首次编写         
    ///
    public class HalconWindowVM : ViewModelBase
    {
        #region 1. 绑定变量
        /// <summary>
        /// 图标、文字、颜色
        /// </summary>
        private string strGrabContent = "开始采集";
        public string StrGrabContent
        {
            get => strGrabContent;
            set => Set(ref strGrabContent, value);
        }
        #endregion

        #region 2. 全局变量
        public HSmartWindowControlWPF Halcon { get; set; }
        public HWindow Ho_Window { get; set; }

        // 外部也可引用
        public HObject Ho_Image = null;
        public bool IsFirstShow { get; set; } = true;

        /// <summary>
        /// 所有绘制过的对象
        /// </summary>
        private List<HObject> Ho_Objects { get; set; } = new List<HObject>();

        public bool CanRefreshImage { get; set; } = false;

        private PixelValueControl MyPixelValueControl { get; set; }
        #endregion


        #region 委托和事件 打印日志消息
        // 声明一个委托
        public delegate void LogEventHandler(string info, EnumLogType type);
        // 声明一个事件
        public event LogEventHandler LogEvent;
        // 触发事件
        protected virtual void PrintLog(string info, EnumLogType type)
        {
            LogEvent?.Invoke(info, type);
        }
        #endregion


        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as HalconWindowControl).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            MyPixelValueControl = (e.Source as HalconWindowControl).MyPixelValueControl;

            // 鼠标事件
            Halcon.HMouseMove += Halcon_HMouseMove;
            Halcon.MouseDown += Halcon_MouseDown;

            if (Ho_Image == null)
            {
                HOperatorSet.GenEmptyObj(out Ho_Image);
                Ho_Image.Dispose();
            }
        }

        /// <summary>
        /// 开始抓图
        /// </summary>
        public RelayCommand CmdGrabImage => new Lazy<RelayCommand>(() => new RelayCommand(GrabImage)).Value;
        public void GrabImage()
        {
            try
            {
                int camId = CcdManager.Instance.CurrentCamId;
                if (camId < 0 || !CcdManager.Instance.HikCamInfos[camId].IsOpened)
                {
                    PrintLog("当前相机未连接", EnumLogType.Warning);
                    return;
                }

                // 抓图：开启、关闭
                if (CcdManager.Instance.HikCamInfos[camId].IsGrabbing)
                {
                    CanRefreshImage = false;
                    // 等待一段时间 考虑图像刷新为空的情况
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(300);
                        if (Ho_Image.IsInitialized())
                        {
                            break;
                        }
                    }

                    _ = CcdManager.Instance.Stop(camId);
                    StrGrabContent = "开始采集";
                    PrintLog("停止采集：" + CcdManager.Instance.HikCamInfos[camId].UserName, EnumLogType.Info);
                }
                else
                {
                    _ = CcdManager.Instance.Start(camId);
                    StrGrabContent = "停止采集";
                    PrintLog("开始采集：" + CcdManager.Instance.HikCamInfos[camId].UserName, EnumLogType.Info);

                    CanRefreshImage = true;
                    Task task = new Task(CaptureImageTask);
                    task.Start();
                }
            }
            catch (Exception ex)
            {
                PrintLog("相机抓图异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 加载图像 保存图像
        /// </summary>
        public RelayCommand<string> CmdLoadSaveImage => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(LoadSaveImage)).Value;
        private void LoadSaveImage(string name)
        {
            // 载入图像
            if (name == "LoadImage")
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "选择图片",
                    Filter = "图像文件(*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp",
                    RestoreDirectory = true,
                    InitialDirectory = Environment.CurrentDirectory + "\\Image",
                };
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                string filename = dialog.FileName;
                Ho_Image.Dispose();
                HOperatorSet.ReadImage(out Ho_Image, filename);
                Ho_Window.DispObj(Ho_Image);
                Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                Ho_Window.SetLineWidth(2);
                if (IsFirstShow)
                {
                    IsFirstShow = false;
                    Halcon.SetFullImagePart();
                }
                PrintLog("图像加载完成：" + filename, EnumLogType.Success);
            }
            // 保存图像
            else if (name == "SaveImage")
            {
                if (Ho_Image.IsInitialized())
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
                    HOperatorSet.WriteImage(Ho_Image, "bmp", 0, filename);
                    PrintLog("图像保存完成：" + filename, EnumLogType.Success);
                }
            }
        }

        /// <summary>
        /// Halcon 窗体右键菜单事：彩色数量
        /// </summary>
        public RelayCommand<string> CmdSetColored => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(SetColored)).Value;
        /// <summary>
        /// 外部也可调用
        /// </summary>
        /// <param name="info"></param>
        public void SetColored(string info)
        {
            Ho_Window.SetColored(int.Parse(info));
            RefreshHWindow();
        }

        /// <summary>
        /// Halcon 窗体右键菜单事件：颜色
        /// </summary>
        public RelayCommand<string> CmdSetColor => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(SetColor)).Value;
        /// <summary>
        /// 外部也可调用
        /// </summary>
        /// <param name="info"></param>
        public void SetColor(string info)
        {
            Ho_Window.SetColor(info);
            RefreshHWindow();
        }

        /// <summary>
        /// Halcon 窗体右键菜单事件：画
        /// </summary>
        public RelayCommand<string> CmdSetDraw => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(SetDraw)).Value;
        /// <summary>
        /// 外部也可调用
        /// </summary>
        /// <param name="info"></param>
        public void SetDraw(string info)
        {
            Ho_Window.SetDraw(info);
            // 刷新窗体
            RefreshHWindow();
        }

        /// <summary>
        /// Halcon 窗体右键菜单事件：线宽
        /// </summary>
        public RelayCommand<string> CmdSetLineWidth => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(SetLineWidth)).Value;
        private void SetLineWidth(string info)
        {
            Ho_Window.SetLineWidth(int.Parse(info));
            // 刷新窗体
            RefreshHWindow();
        }

        /// <summary>
        /// Halcon 窗体右键菜单事件：线型
        /// </summary>
        public RelayCommand<string> CmdSetLineStyle => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(SetLineStyle)).Value;
        private void SetLineStyle(string info)
        {
            string[] strs = info.Split(',');
            HTuple nums = new HTuple();
            for (int i = 0; i < strs.Length; i++)
            {
                nums[i] = int.Parse(strs[i]);
            }
            Ho_Window.SetLineStyle(nums);
            // 刷新窗体
            RefreshHWindow();
        }
        #endregion

        #region 4. 内部方法
        /// <summary>
        /// 抓图线程
        /// </summary>
        private void CaptureImageTask()
        {
            while (CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].IsGrabbing)
            {
                try
                {
                    if (CanRefreshImage)
                    {
                        Ho_Image.Dispose();

                        CcdManager.Instance.GetHalconImage(CcdManager.Instance.CurrentCamId, ref Ho_Image);

                        if (Ho_Image.IsInitialized())
                        {
                            Ho_Window.DispObj(Ho_Image);
                            if (IsFirstShow)
                            {
                                IsFirstShow = false;
                                _ = DispatcherHelper.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                {
                                    Halcon.SetFullImagePart();
                                    Ho_Window.SetLineWidth(3);
                                    Ho_Window.SetColor("orange red");
                                });
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
                catch (Exception)
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Halcon_HMouseMove(object sender, HSmartWindowControlWPF.HMouseEventArgsWPF e)
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
        private void Halcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.ClickCount == 2)
            {
                Halcon.SetFullImagePart();
            }
            else if (e.ClickCount == 3)
            {
                CanRefreshImage = !CanRefreshImage;

                int camId = CcdManager.Instance.CurrentCamId;
                if (CcdManager.Instance.HikCamInfos[camId].IsGrabbing)
                {
                    if (CanRefreshImage)
                    {
                        PrintLog("图像刷新开启", EnumLogType.Info);
                    }
                    else
                    {
                        PrintLog("图像刷新关闭", EnumLogType.Info);
                    }
                }
            }
        }

        /// <summary>
        /// 更新绘制对象
        /// </summary>
        private void RefreshHWindow()
        {
            Ho_Window.ClearWindow();
            if (Ho_Image != null && Ho_Image.IsInitialized())
            {
                Ho_Window.DispObj(Ho_Image);
            }
            for (int i = 0; i < Ho_Objects.Count; i++)
            {
                Ho_Window.DispObj(Ho_Objects[i]);
            }
        }
        #endregion

        #region 5. 外部方法
        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="ho_Image"></param>
        public void DispHImage(HObject ho_Image)
        {
            if (Ho_Image == null)
            {
                HOperatorSet.GenEmptyObj(out Ho_Image);
            }
            Ho_Image.Dispose();
            Ho_Image = ho_Image.Clone();
            Ho_Window.DispObj(Ho_Image);
        }
        public void DispHImage()
        {
            if (Ho_Image != null && Ho_Image.IsInitialized())
            {
                Ho_Window.DispObj(Ho_Image);
            }
        }

        /// <summary>
        /// 显示目标
        /// </summary>
        public void DispHObject(HObject ho_Object)
        {
            Ho_Window.DispObj(ho_Object);
            Ho_Objects.Add(ho_Object.Clone());
        }

        /// <summary>
        /// 清空窗体
        /// </summary>
        public void ClearWindow()
        {
            Ho_Window.ClearWindow();
            Ho_Objects.Clear();
        }
        #endregion
    }
}