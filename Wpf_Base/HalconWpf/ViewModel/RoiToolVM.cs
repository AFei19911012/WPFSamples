using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Wpf_Base.CcdWpf;
using Wpf_Base.ControlsWpf;
using Wpf_Base.HalconWpf.Method;
using Wpf_Base.HalconWpf.Model;
using Wpf_Base.HalconWpf.Tools;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;


namespace Wpf_Base.HalconWpf.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/05 09:41:19
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/05 09:41:19    CoderMan/CoderdMan1012         首次编写         
    ///
    public class RoiToolVM : ViewModelBase
    {
        #region 1. 绑定变量
        private int intSelectModule = 0;
        public int IntSelectModule
        {
            get => intSelectModule;
            set => Set(ref intSelectModule, value);
        }

        /// <summary>
        /// 配方名称
        /// </summary>
        private string strRecipeName = "default";
        public string StrRecipeName
        {
            get => strRecipeName;
            set => Set(ref strRecipeName, value);
        }


        /// <summary>
        /// 文字
        /// </summary>
        private string strGrabContent = "开始采集";
        public string StrGrabContent
        {
            get => strGrabContent;
            set => Set(ref strGrabContent, value);
        }


        private bool boolMultiModule = false;
        public bool BoolMultiModule
        {
            get => boolMultiModule;
            set => Set(ref boolMultiModule, value);
        }

        private bool boolIsLocked = true;
        public bool BoolIsLocked
        {
            get => boolIsLocked;
            set => Set(ref boolIsLocked, value);
        }

        /// <summary>
        /// 模板参数
        /// </summary>
        private double numRow1 = 50;
        public double NumRow1
        {
            get => numRow1;
            set => Set(ref numRow1, value);
        }

        private double numCol1 = 50;
        public double NumCol1
        {
            get => numCol1;
            set => Set(ref numCol1, value);
        }

        private double numRow2 = 450;
        public double NumRow2
        {
            get => numRow2;
            set => Set(ref numRow2, value);
        }

        private double numCol2 = 450;
        public double NumCol2
        {
            get => numCol2;
            set => Set(ref numCol2, value);
        }

        private double numRow = 50;
        public double NumRow
        {
            get => numRow;
            set => Set(ref numRow, value);
        }

        private double numCol = 50;
        public double NumCol
        {
            get => numCol;
            set => Set(ref numCol, value);
        }

        private double numRadius = 50;
        public double NumRadius
        {
            get => numRadius;
            set => Set(ref numRadius, value);
        }
        #endregion

        #region 2. 全局变量
        private HSmartWindowControlWPF Halcon { get; set; }
        private HWindow Ho_Window { get; set; }
        private Border DrawingBorder { get; set; }
        private HObject Ho_Image = null;
        private bool IsFirstShow { get; set; } = true;

        private Point PointOri { get; set; } = new Point();
        private Point PointEnd { get; set; } = new Point();
        private Point PointOriControl { get; set; } = new Point();
        private Point PointEndControl { get; set; } = new Point();
        private bool CanMove { get; set; } = false;
        private bool CanEdit { get; set; } = false;
        private bool IsMoving { get; set; } = false;

        private bool CanRefreshImage { get; set; } = false;

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


        public RoiToolVM()
        {

        }

        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as RoiTool).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            DrawingBorder = (e.Source as RoiTool).DrawingBorder;
            MyPixelValueControl = (e.Source as RoiTool).MyPixelValueControl;

            // 鼠标事件
            DrawingBorder.MouseWheel += DrawingBorder_MouseWheel;
            DrawingBorder.MouseLeftButtonDown += DrawingBorder_MouseLeftButtonDown;
            DrawingBorder.MouseMove += DrawingBorder_MouseMove;
            DrawingBorder.MouseLeftButtonUp += DrawingBorder_MouseLeftButtonUp;
            DrawingBorder.MouseRightButtonDown += DrawingBorder_MouseRightButtonDown;

            if (Ho_Image == null)
            {
                HOperatorSet.GenEmptyObj(out Ho_Image);
            }
        }

        /// <summary>
        /// 开始抓图
        /// </summary>
        public RelayCommand CmdGrabImage => new Lazy<RelayCommand>(() => new RelayCommand(GrabImage)).Value;
        private void GrabImage()
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
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 加载图像 保存图像
        /// </summary>
        public RelayCommand<string> CmdLoadSaveImage => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(LoadSaveImage)).Value;
        private void LoadSaveImage(string name)
        {
            try
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
                    Ho_Window.ClearWindow();
                    Ho_Window.DispObj(Ho_Image);
                    if (IsFirstShow)
                    {
                        IsFirstShow = false;
                        Halcon.SetFullImagePart();
                        Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                        Ho_Window.SetLineWidth(2);
                    }

                    // 显示模板
                    HOperatorSet.GetImageSize(Ho_Image, out HTuple hv_Width, out HTuple hv_Height);
                    if (IntSelectModule == 0)
                    {
                        NumRow1 = Math.Min(NumRow1, hv_Height.I);
                        NumCol1 = Math.Min(NumCol1, hv_Width.I);
                        NumRow2 = Math.Min(NumRow2, hv_Height.I);
                        NumCol2 = Math.Min(NumCol2, hv_Width.I);
                        Ho_Window.DispRectangleContour(NumRow1, NumCol1, NumRow2, NumCol2);
                    }
                    else
                    {
                        NumRow = Math.Min(NumRow, hv_Height.I);
                        NumCol = Math.Min(NumCol, hv_Width.I);
                        NumRadius = Math.Min(NumRadius, 0.5 * hv_Width.I);
                        Ho_Window.DispCircleContour(NumRow, NumCol, NumRadius);
                    }
                    // 通过委托事件把消息传递出去
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
                // 保存图像
                else if (name == "SaveROIImage")
                {
                    if (Ho_Image.IsInitialized())
                    {
                        SaveFileDialog dialog = new SaveFileDialog
                        {
                            Title = "保存图像",
                            Filter = "图像文件(*.png)|*.png",
                            RestoreDirectory = true,
                            FileName = StrRecipeName + ".png"
                        };
                        if (dialog.ShowDialog() != true)
                        {
                            return;
                        }
                        string filename = dialog.FileName;

                        double row1 = NumRow1;
                        double col1 = NumCol1;
                        double row2 = NumRow2;
                        double col2 = NumCol2;
                        if ((row2 - row1) * (col2 - col1) > 1)
                        {
                            HOperatorSet.CropPart(Ho_Image, out HObject ho_ImageCrop, row1, col1, col2 - col1, row2 - row1);
                            HOperatorSet.WriteImage(ho_ImageCrop, "png", 0, filename);
                            ho_ImageCrop.Dispose();
                            PrintLog("图像保存完成：" + filename, EnumLogType.Success);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintLog("图像存取异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 保存 ROI 参数
        /// </summary>
        public RelayCommand CmdSaveParam => new Lazy<RelayCommand>(() => new RelayCommand(SaveParam)).Value;
        private void SaveParam()
        {
            // 保存 ROI
            string filename = HalconIoMethod.GenROIName(StrRecipeName);
            CROI roi = new CROI();
            // 多模板
            if (BoolMultiModule)
            {
                roi.Row1 = NumRow1;
                roi.Col1 = NumCol1;
                roi.Row2 = NumRow2;
                roi.Col2 = NumCol2;
                roi.Row = NumRow;
                roi.Col = NumCol;
                roi.R = NumRadius;
            }
            // 单模板
            else if (IntSelectModule == 0)
            {
                roi.Row1 = NumRow1;
                roi.Col1 = NumCol1;
                roi.Row2 = NumRow2;
                roi.Col2 = NumCol2;
            }
            else if (IntSelectModule == 1)
            {
                roi.Row = NumRow;
                roi.Col = NumCol;
                roi.R = NumRadius;
            }
            bool result = roi.SaveROI(filename);
            if (result)
            {
                PrintLog("ROI 保存完成：" + filename, EnumLogType.Success);
            }
        }

        /// <summary>
        /// 选择 ROI
        /// </summary>
        public RelayCommand CmdChooseROI => new Lazy<RelayCommand>(() => new RelayCommand(ChooseROI)).Value;
        private void ChooseROI()
        {
            if (CanRefreshImage)
            {
                GetCurrentImage();
            }

            if (!Ho_Image.IsInitialized())
            {
                PrintLog("图像无效", EnumLogType.Warning);
                return;
            }
            CanEdit = true;
        }
        #endregion

        #region 4. 内部方法       
        /// <summary>
        /// 拍图任务
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
                                    Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                                    Ho_Window.SetLineWidth(2);
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
        /// 获取当前图像
        /// </summary>
        private void GetCurrentImage()
        {
            // 禁止刷新图像
            CanRefreshImage = false;
            PrintLog("启动相机抓图", EnumLogType.Debug);
            if (!CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].IsOpened)
            {
                _ = CcdManager.Instance.Open(CcdManager.Instance.CurrentCamId);
            }
            if (!CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].IsGrabbing)
            {
                _ = CcdManager.Instance.Start(CcdManager.Instance.CurrentCamId);
            }
            // 尝试多次拍照
            for (int i = 0; i < 5; i++)
            {
                Ho_Image.Dispose();
                // 触发模式
                if (CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].TriggerMode == EnumCaptureMode.Trig)
                {
                    PrintLog("触发拍照指令", EnumLogType.Debug);
                    _ = CcdManager.Instance.TrigCamBySoft(CcdManager.Instance.CurrentCamId);
                }
                CcdManager.Instance.GetHalconImage(CcdManager.Instance.CurrentCamId, ref Ho_Image);
                if (Ho_Image.IsInitialized())
                {
                    break;
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
            if (Ho_Image.IsInitialized())
            {
                PrintLog("相机抓图完成", EnumLogType.Success);
            }
            else
            {
                PrintLog("相机抓图失败", EnumLogType.Error);
            }
        }

        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanMove = false;
            IsMoving = false;
        }
        private void DrawingBorder_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                e.Handled = true;
                PointEndControl = e.GetPosition(e.Device.Target);
                Point point = Halcon.ControlPointToHImagePoint(PointEndControl.X, PointEndControl.Y);
                // 当前点信息
                if (Ho_Image.IsInitialized())
                {
                    int row = (int)point.Y;
                    int col = (int)point.X;
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

                // 图像不能处于刷新状态
                if (!CanRefreshImage && Ho_Image.IsInitialized() && CanMove && CanEdit)
                {
                    // 锁定 ROI 模式下
                    if (BoolIsLocked)
                    {
                        double dx = point.X - (0.5 * (NumCol1 + NumCol2));
                        double dy = point.Y - (0.5 * (NumRow1 + NumRow2));
                        if (IntSelectModule == 1)
                        {
                            dx = point.X - NumCol;
                            dy = point.Y - NumRow;
                        }
                        // 判断一次即可
                        if (!IsMoving)
                        {
                            IsMoving = Math.Sqrt((dx * dx) + (dy * dy)) > 1;
                        }
                        if (IsMoving)
                        {
                            Ho_Window.ClearWindow();
                            Ho_Window.DispObj(Ho_Image);
                            if (IntSelectModule == 0)
                            {
                                NumRow1 += dy;
                                NumCol1 += dx;
                                NumRow2 += dy;
                                NumCol2 += dx;
                                Ho_Window.DispRectangleContour(NumRow1, NumCol1, NumRow2, NumCol2);
                            }
                            else
                            {
                                NumRow += dy;
                                NumCol += dx;
                                Ho_Window.DispCircleContour(NumRow, NumCol, NumRadius);
                            }
                        }
                        return;
                    }
                    else
                    {
                        PointEnd = point;
                        Ho_Window.ClearWindow();
                        Ho_Window.DispObj(Ho_Image);
                        // 画矩形
                        if (IntSelectModule == 0)
                        {
                            double x0 = 0.5 * (PointOri.X + PointEnd.X);
                            double y0 = 0.5 * (PointOri.Y + PointEnd.Y);
                            double lenX = 0.5 * Math.Abs(PointEnd.X - PointOri.X);
                            double lenY = 0.5 * Math.Abs(PointEnd.Y - PointOri.Y);
                            HTuple hv_Rows = new HTuple();
                            HTuple hv_Cols = new HTuple();
                            hv_Rows.Dispose();
                            hv_Cols.Dispose();
                            hv_Rows[0] = y0 - lenY;
                            hv_Cols[0] = x0 - lenX;
                            hv_Rows[1] = y0 + lenY;
                            hv_Cols[1] = x0 - lenX;
                            hv_Rows[2] = y0 + lenY;
                            hv_Cols[2] = x0 + lenX;
                            hv_Rows[3] = y0 - lenY;
                            hv_Cols[3] = x0 + lenX;
                            hv_Rows[4] = y0 - lenY;
                            hv_Cols[4] = x0 - lenX;
                            HOperatorSet.GenContourPolygonXld(out HObject ho_Polygon, hv_Rows, hv_Cols);
                            Ho_Window.DispObj(ho_Polygon);
                            ho_Polygon.Dispose();
                            hv_Rows.Dispose();
                            hv_Cols.Dispose();
                            // 保存
                            NumRow1 = y0 - lenY;
                            NumCol1 = x0 - lenX;
                            NumRow2 = y0 + lenY;
                            NumCol2 = x0 + lenX;
                        }
                        // 画圆
                        else if (IntSelectModule == 1)
                        {
                            double r = InkMethod.GetDistancePP(PointOri, PointEnd);
                            HOperatorSet.GenCircleContourXld(out HObject ho_Circle, PointOri.Y, PointOri.X, r, 0, 6.28318, "positive", 1);
                            Ho_Window.DispObj(ho_Circle);
                            ho_Circle.Dispose();
                            // 保存
                            NumRow = PointOri.Y;
                            NumCol = PointOri.X;
                            NumRadius = r;
                        }
                        return;
                    }
                }

                // 平移
                if (CanMove)
                {
                    Halcon.HShiftWindowContents(PointOriControl.X - PointEndControl.X, PointOriControl.Y - PointEndControl.Y);
                    PointOriControl = PointEndControl;
                    return;
                }
            }
            catch (Exception ex)
            {
                PrintLog("鼠标移动导致异常：" + ex.Message, EnumLogType.Error);
            }
        }
        private void DrawingBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanMove = true;
            PointOriControl = e.GetPosition(e.Device.Target);
            // 坐标转换
            PointOri = Halcon.ControlPointToHImagePoint(PointOriControl.X, PointOriControl.Y);
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
        private void DrawingBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            // 当前点为中心缩放
            Point curPoint = e.GetPosition(e.Device.Target);
            int flag = e.Delta > 0 ? 1 : -1;
            Halcon.HZoomWindowContents(curPoint.X, curPoint.Y, flag);
        }
        private void DrawingBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanEdit = false;
        }
        #endregion
    }
}