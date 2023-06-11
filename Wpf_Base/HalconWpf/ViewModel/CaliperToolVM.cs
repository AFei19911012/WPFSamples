using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Created Time: 22/09/03 14:30:36
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 14:30:36    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CaliperToolVM : ViewModelBase
    {
        #region 1. 绑定变量
        /// <summary>
        /// 最小边缘幅度
        /// </summary>
        private double numMinAmp = 10;
        public double NumMinAmp
        {
            get => numMinAmp;
            set => Set(ref numMinAmp, value);
        }

        /// <summary>
        /// 平滑
        /// </summary>
        private double numSmooth = 2;
        public double NumSmooth
        {
            get => numSmooth;
            set => Set(ref numSmooth, value);
        }

        /// <summary>
        /// 平均像素
        /// </summary>
        private double numAvePixel = 50;
        public double NumAvePixel
        {
            get => numAvePixel;
            set => Set(ref numAvePixel, value);
        }

        /// <summary>
        /// 实际长度
        /// </summary>
        private double numActualLength = 1;
        public double NumActualLength
        {
            get => numActualLength;
            set => Set(ref numActualLength, value);
        }

        /// <summary>
        /// 像素尺寸
        /// </summary>
        private double numPixelSize;
        public double NumPixelSize
        {
            get => numPixelSize;
            set => Set(ref numPixelSize, value);
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
        #endregion

        #region 2. 全局变量
        private HSmartWindowControlWPF Halcon { get; set; }
        private HWindow Ho_Window { get; set; }
        private InkCanvas DrawingCanvas { get; set; }
        private Border DrawingBorder { get; set; }

        /// <summary>
        /// 绘制模板 编辑
        /// </summary>
        private Point PointMoveOri { get; set; } = new Point();
        private bool CanMove { get; set; } = false;
        private bool IsMakingModule { get; set; } = false;
        private bool IsEditingModule { get; set; } = false;
        private bool CanEditModule { get; set; } = false;
        private EnumCaliperEditType ModuleEditType { get; set; } = EnumCaliperEditType.None;
        private List<Point> PointsMeasured { get; set; } = new List<Point>();

        /// <summary>
        /// 图像
        /// </summary>
        private HObject Ho_Image = null;

        private bool IsFirstShow { get; set; } = true;

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


        /// <summary>
        /// 构造函数 初始化参数
        /// </summary>
        public CaliperToolVM()
        {
            NumPixelSize = NumActualLength / NumAvePixel;
        }

        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as CaliperTool).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            DrawingCanvas = (e.Source as CaliperTool).DrawingCanvas;
            DrawingBorder = (e.Source as CaliperTool).DrawingBorder;
            MyPixelValueControl = (e.Source as CaliperTool).MyPixelValueControl;

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
                PrintLog("抓图异常：" + ex.Message, EnumLogType.Error);
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
                    Ho_Window.DispObj(Ho_Image);
                    Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                    Ho_Window.SetLineWidth(2);
                    if (IsFirstShow)
                    {
                        IsFirstShow = false;
                        Halcon.SetFullImagePart();
                    }
                    SetDefault();
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
            catch (Exception ex)
            {
                PrintLog("图像存取异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 保存标定结果
        /// </summary>
        public RelayCommand CmdSaveCaliper => new Lazy<RelayCommand>(() => new RelayCommand(SaveCaliper)).Value;
        private void SaveCaliper()
        {
            // 保存标定结果
            string filename = HalconIoMethod.GenCaliperName(StrRecipeName);
            bool flag = HalconIoMethod.SaveCaliper(filename, NumPixelSize);
            if (flag)
            {
                PrintLog("尺寸标定文件保存成功：" + filename, EnumLogType.Success);
            }
            else
            {
                PrintLog("尺寸标定文件保存失败", EnumLogType.Error);
            }
        }

        /// <summary>
        /// 选择参考线
        /// </summary>
        public RelayCommand CmdChooseLine => new Lazy<RelayCommand>(() => new RelayCommand(ChooseLine)).Value;
        private void ChooseLine()
        {
            try
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

                SetDefault();
                IsMakingModule = true;
            }
            catch (Exception ex)
            {
                PrintLog("参考线绘制异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 执行标定
        /// </summary>
        public RelayCommand CmdExecuteCaliper => new Lazy<RelayCommand>(() => new RelayCommand(ExecuteCaliper)).Value;
        private void ExecuteCaliper()
        {
            try
            {
                if (DrawingCanvas.Strokes.Count < 1)
                {
                    PrintLog("未发现绘制对象", EnumLogType.Warning);
                    return;
                }
                IsMakingModule = false;
                CanEditModule = false;
                // 获取矩形坐标
                StylusPointCollection pts = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                // 转换为 HImage 坐标
                Point pt1 = Halcon.ControlPointToHImagePoint(pts[0].X, pts[0].Y);
                Point pt2 = Halcon.ControlPointToHImagePoint(pts[1].X, pts[1].Y);
                Point pt3 = Halcon.ControlPointToHImagePoint(pts[2].X, pts[2].Y);
                Point pt4 = Halcon.ControlPointToHImagePoint(pts[3].X, pts[3].Y);

                // 参考直线坐标
                double line_row1 = 0.5 * (pt1.Y + pt2.Y);
                double line_col1 = 0.5 * (pt1.X + pt2.X);
                double line_row2 = 0.5 * (pt3.Y + pt4.Y);
                double line_col2 = 0.5 * (pt3.X + pt4.X);
                double row0 = 0.5 * (line_row1 + line_row2);
                double col0 = 0.5 * (line_col1 + line_col2);
                double dr = line_row1 - line_row2;
                double dc = line_col2 - line_col1;
                double phi = Math.Atan2(dr, dc);
                double len1 = 0.5 * Math.Sqrt((dr * dr) + (dc * dc));
                double len2 = 0.5 * Math.Abs(pt1.Y - pt3.Y);

                // 只保留矩形框
                while (DrawingCanvas.Strokes.Count > 1)
                {
                    DrawingCanvas.Strokes.RemoveAt(1);
                }

                // 标定过程
                // 找平行线
                HOperatorSet.GetImageSize(Ho_Image, out HTuple hv_Width, out HTuple hv_Height);
                HOperatorSet.GenMeasureRectangle2(row0, col0, phi, len1, len2, hv_Width, hv_Height, "nearest_neighbor", out HTuple hv_MsrHandle_Measure);
                HOperatorSet.MeasurePos(Ho_Image, hv_MsrHandle_Measure, NumSmooth, NumMinAmp, "negative", "all", out HTuple hv_Row_Measure, out HTuple hv_Column_Measure, out HTuple hv_Amplitude_Measure, out HTuple hv_Distance_Measure);
                hv_Width.Dispose();
                hv_Height.Dispose();
                hv_MsrHandle_Measure.Dispose();
                hv_Amplitude_Measure.Dispose();

                // 计算平均距离
                if (hv_Distance_Measure.Length > 0)
                {
                    NumAvePixel = hv_Distance_Measure.TupleSum() / hv_Distance_Measure.Length;
                    NumPixelSize = NumActualLength / NumAvePixel;

                    // 换算坐标点
                    PointsMeasured.Clear();
                    double len = 75;
                    for (int i = 0; i < hv_Row_Measure.Length; i++)
                    {
                        double row1 = hv_Row_Measure[i].D - (len * Math.Cos(phi));
                        double col1 = hv_Column_Measure[i].D - (len * Math.Sin(phi));
                        double row2 = hv_Row_Measure[i].D + (len * Math.Cos(phi));
                        double col2 = hv_Column_Measure[i].D + (len * Math.Sin(phi));
                        pt1 = Halcon.ControlPointToHImagePoint(col1, row1, true);
                        pt2 = Halcon.ControlPointToHImagePoint(col2, row2, true);
                        PointsMeasured.Add(pt1);
                        PointsMeasured.Add(pt2);
                    }

                    // 画线
                    DrawingCanvas.Strokes.Add(InkMethod.CreateParallelLines(PointsMeasured));
                }

                HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);
                hv_Distance_Measure.Dispose();
                hv_Row_Measure.Dispose();
                hv_Column_Measure.Dispose();
            }
            catch (Exception ex)
            {
                PrintLog("执行标定异常：" + ex.Message, EnumLogType.Error);
            }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 重置状态
        /// </summary>
        private void SetDefault()
        {
            DrawingCanvas.Strokes.Clear();
            CanEditModule = false;
            IsEditingModule = false;
            IsMakingModule = false;
            Ho_Window.ClearWindow();
            if (Ho_Image.IsInitialized())
            {
                Ho_Window.DispObj(Ho_Image);
            }
        }

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
        private void DrawingBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanMove = true;
            PointMoveOri = e.GetPosition(e.Device.Target);

            // 双击 图像还原
            if (e.ClickCount == 2)
            {
                Halcon.SetFullImagePart();
                // 清空 Strokes
                DrawingCanvas.Strokes.Clear();
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
        private void DrawingBorder_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            Point curPoint = e.GetPosition(e.Device.Target);

            //////////////////////////////////////// 当前点信息 ////////////////////////////////////////
            if (Ho_Image.IsInitialized())
            {
                Point point = Halcon.ControlPointToHImagePoint(curPoint.X, curPoint.Y);
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

            //////////////////////////////////////// 编辑模板 ////////////////////////////////////////
            if (CanEditModule)
            {
                if (DrawingCanvas.Strokes.Count < 1)
                {
                    return;
                }

                if (!IsEditingModule)
                {
                    // 会卡顿 只判断一次 不同位置对应不同鼠标形状
                    ModuleEditType = DrawingCanvas.Strokes[0].GetCaliperEditType(curPoint);
                }

                IsEditingModule = true;
                // 根据编辑状态设置鼠标形状
                if (ModuleEditType == EnumCaliperEditType.Move)
                {
                    DrawingBorder.Cursor = Cursors.SizeAll;
                }
                else if (ModuleEditType == EnumCaliperEditType.Rotate)
                {
                    DrawingBorder.Cursor = Cursors.Hand;
                }
                else if (ModuleEditType == EnumCaliperEditType.Resize)
                {
                    DrawingBorder.Cursor = Cursors.Cross;
                }
                else
                {
                    IsEditingModule = false;
                    DrawingBorder.Cursor = Cursors.Arrow;
                }

                // 编辑
                if (CanMove && IsEditingModule)
                {
                    // 移动
                    if (ModuleEditType == EnumCaliperEditType.Move)
                    {
                        Matrix matrixMove = new Matrix();
                        matrixMove.Translate(curPoint.X - PointMoveOri.X, curPoint.Y - PointMoveOri.Y);
                        DrawingCanvas.Strokes.Transform(matrixMove, false);
                        // 更新初始移动位置
                        PointMoveOri = curPoint;
                        return;
                    }
                    // 旋转
                    else if (ModuleEditType == EnumCaliperEditType.Rotate)
                    {
                        // 获取旋转中心
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                        Point p0 = new Point(0.5 * (sps[0].X + sps[2].X), 0.5 * (sps[0].Y + sps[2].Y));
                        // 旋转角度，顺时针为正，逆时针为负
                        double angle = InkMethod.GetPointAngle(p0, PointMoveOri, curPoint);
                        Matrix matrixRotate = new Matrix();
                        if (InkMethod.GetRotateDirection(p0, PointMoveOri, curPoint))
                        {
                            matrixRotate.RotateAt(angle, p0.X, p0.Y);
                        }
                        else
                        {
                            matrixRotate.RotateAt(-angle, p0.X, p0.Y);
                        }
                        DrawingCanvas.Strokes.Transform(matrixRotate, false);
                        // 更新初始移动位置
                        PointMoveOri = curPoint;
                        return;
                    }
                    // 缩放
                    else
                    {
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                        Point p0 = new Point(0.5 * (sps[0].X + sps[2].X), 0.5 * (sps[0].Y + sps[2].Y));
                        if (InkMethod.GetDistancePP(curPoint, p0) < 5)
                        {
                            return;
                        }

                        // 计算倾角 假设 +x 方向有个点
                        Point px = new Point(DrawingCanvas.Strokes[0].GetBounds().Right + DrawingCanvas.Strokes[0].GetBounds().Width, sps[1].Y);
                        double angle = InkMethod.GetPointAngle((Point)sps[1], (Point)sps[2], px);
                        // 判断顺时针、逆时针
                        if (sps[2].Y > px.Y)
                        {
                            angle = -angle;
                        }

                        // 先还原到初始状态 旋转中心为 p0
                        Matrix matrixResize = new Matrix();
                        matrixResize.RotateAt(angle, p0.X, p0.Y);
                        DrawingCanvas.Strokes[0].Transform(matrixResize, false);
                        // 起始点也做变换
                        Point pt = new Point(curPoint.X, curPoint.Y);
                        PointMoveOri = matrixResize.Transform(PointMoveOri);
                        pt = matrixResize.Transform(pt);
                        // 缩放
                        double ratiox = Math.Abs(pt.X - p0.X) / Math.Abs(PointMoveOri.X - p0.X);
                        double ratioy = Math.Abs(pt.Y - p0.Y) / Math.Abs(PointMoveOri.Y - p0.Y);
                        matrixResize = new Matrix();
                        matrixResize.ScaleAt(ratiox, ratioy, p0.X, p0.Y);
                        // 还原
                        matrixResize.RotateAt(-angle, p0.X, p0.Y);
                        DrawingCanvas.Strokes[0].Transform(matrixResize, false);
                        // 更新初始移动位置
                        PointMoveOri = curPoint;
                    }
                }
                else
                {
                    IsEditingModule = false;
                    ModuleEditType = EnumCaliperEditType.None;
                }
            }

            //////////////////////////////////////// 自由绘制参考线 ////////////////////////////////////////
            if (IsMakingModule && CanMove)
            {
                DrawingCanvas.Strokes.Clear();
                DrawingCanvas.Strokes.Add(InkMethod.CreateCaliperRectangle(PointMoveOri, curPoint));
                return;
            }

            //////////////////////////////////////// 图像平移 ////////////////////////////////////////
            if (CanMove)
            {
                // 图像窗口平移
                Halcon.HShiftWindowContents(PointMoveOri.X - curPoint.X, PointMoveOri.Y - curPoint.Y);
                // Strokes 平移
                Matrix matrix = new Matrix();
                matrix.Translate(curPoint.X - PointMoveOri.X, curPoint.Y - PointMoveOri.Y);
                DrawingCanvas.Strokes.Transform(matrix, false);
                // 更新起点
                PointMoveOri = curPoint;
            }
        }
        private void DrawingBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanMove = false;
        }
        private void DrawingBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            // 双击 不可编辑
            if (e.ClickCount > 1)
            {
                CanEditModule = false;
                DrawingBorder.Cursor = Cursors.Arrow;
            }
            else
            {
                IsMakingModule = false;
                CanEditModule = true;
            }
        }
        private void DrawingBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            // 当前点为中心缩放
            Point curPoint = e.GetPosition(e.Device.Target);
            Matrix matrix = new Matrix();
            if (e.Delta > 0)
            {
                matrix.ScaleAt(Halcon.HZoomFactor, Halcon.HZoomFactor, curPoint.X, curPoint.Y);
                Halcon.HZoomWindowContents(curPoint.X, curPoint.Y, 1);
            }
            else
            {
                matrix.ScaleAt(1 / Halcon.HZoomFactor, 1 / Halcon.HZoomFactor, curPoint.X, curPoint.Y);
                Halcon.HZoomWindowContents(curPoint.X, curPoint.Y, -1);
            }
            DrawingCanvas.Strokes.Transform(matrix, false);
        }
        #endregion
    }
}