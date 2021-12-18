using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HalconWPF.Method;
using HalconWPF.Model;
using HalconWPF.UserControl;
using System.Threading;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/9 15:39:25
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/9 15:39:25    Taosy.W                 
    ///
    public class CaliperCalibrationViewModel : ViewModelBase
    {
        ////////////////////////////////// 绑定属性 //////////////////////////////////
        #region
        /// <summary>
        /// 当前点位置和灰度值
        /// </summary>
        private string strCurPosition;
        public string StrCurPosition
        {
            get => strCurPosition;
            set => Set(ref strCurPosition, value);
        }

        private string strCurGrayValue;
        public string StrCurGrayValue
        {
            get => strCurGrayValue;
            set => Set(ref strCurGrayValue, value);
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        private string strCurTime;
        public string StrCurTime
        {
            get => strCurTime;
            set => Set(ref strCurTime, value);
        }

        /// <summary>
        /// 最小边缘幅度
        /// </summary>
        private double numMinAmp;
        public double NumMinAmp
        {
            get => numMinAmp;
            set => Set(ref numMinAmp, value);
        }

        /// <summary>
        /// 平滑
        /// </summary>
        private double numSmooth;
        public double NumSmooth
        {
            get => numSmooth;
            set => Set(ref numSmooth, value);
        }

        /// <summary>
        /// 平均像素
        /// </summary>
        private double numAvePixel;
        public double NumAvePixel
        {
            get => numAvePixel;
            set => Set(ref numAvePixel, value);
        }

        /// <summary>
        /// 实际长度
        /// </summary>
        private double numActualLength;
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
        #endregion
        //////////////////////////////////// 变量 ////////////////////////////////////
        #region
        private HSmartWindowControlWPF Halcon { get; set; }
        private HWindow Ho_Window { get; set; }
        private InkCanvas DrawingCanvas { get; set; }
        private Border DrawingBorder { get; set; }

        /// <summary>
        /// 缩放参数
        /// </summary>
        private int WheelScrollValue { get; set; }
        private int WheelScrollMin { get; set; }
        private int WheelScrollMax { get; set; }

        /// <summary>
        /// 绘制模板 编辑
        /// </summary>
        private Point PointMoveOri { get; set; }
        private bool CanMove { get; set; }
        private bool IsMakingModule { get; set; }
        private bool IsEditingModule { get; set; }
        private bool CanEditModule { get; set; }
        private EnumModuleEditType ModuleEditType { get; set; }
        private List<Point> PointsMeasured { get; set; }
        private HObject Ho_Image = null;
        #endregion
        ////////////////////////////////// 构造函数 //////////////////////////////////
        /// <summary>
        /// 构造函数 初始化参数
        /// </summary>
        public CaliperCalibrationViewModel()
        {
            PointsMeasured = new List<Point>();
            NumMinAmp = 20;
            NumSmooth = 3;
            NumAvePixel = 30.00;
            NumActualLength = 1;
            NumPixelSize = NumActualLength / NumAvePixel;

            StrCurPosition = "X = null, Y = null";
            StrCurGrayValue = "null";

            PointMoveOri = new Point();
            CanMove = false;
            IsMakingModule = false;
            IsEditingModule = false;
            CanEditModule = false;
            WheelScrollValue = 0;
            WheelScrollMin = 0;
            WheelScrollMax = 20;
            ModuleEditType = EnumModuleEditType.None;

            // 计时器更新时间
            InitTimer();
        }

        ////////////////////////////////// 绑定命令//////////////////////////////////
        #region
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as CaliperCalibration).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            DrawingCanvas = (e.Source as CaliperCalibration).DrawingCanvas;
            DrawingBorder = (e.Source as CaliperCalibration).DrawingBorder;

            // 鼠标事件
            DrawingBorder.MouseWheel += DrawingBorder_MouseWheel;
            DrawingBorder.MouseLeftButtonDown += DrawingBorder_MouseLeftButtonDown;
            DrawingBorder.MouseMove += DrawingBorder_MouseMove;
            DrawingBorder.MouseLeftButtonUp += DrawingBorder_MouseLeftButtonUp;
            DrawingBorder.MouseRightButtonDown += DrawingBorder_MouseRightButtonDown;
        }

        /// <summary>
        /// 窗体尺寸变化后 图像会自动自适应
        /// </summary>
        public RelayCommand CmdSizeChanged => new Lazy<RelayCommand>(() => new RelayCommand(SizeChanged)).Value;
        private void SizeChanged()
        {
            WheelScrollValue = 0;
            // 清空 Strokes
            DrawingCanvas.Strokes.Clear();
        }

        /// <summary>
        /// 按钮事件
        /// </summary>
        public RelayCommand<string> CmdButtonEvent => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(ButtonEvent)).Value;
        private void ButtonEvent(string name)
        {
            try
            {
                // 创建模板
                if (name == "Create")
                {
                    HOperatorSet.GenEmptyObj(out Ho_Image);
                    Ho_Image.Dispose();
                    HOperatorSet.ReadImage(out Ho_Image, @"image\caliper.bmp");
                    Ho_Window.DispObj(Ho_Image);
                    Halcon.SetFullImagePart();
                    Halcon.HZoomFactor = 1.25;
                    Ho_Window.SetColor("red");
                    Ho_Window.SetLineWidth(1);
                    WheelScrollValue = 0;
                    // 锁定
                    DrawingCanvas.Strokes.Clear();
                    IsMakingModule = true;
                    IsEditingModule = false;
                    CanEditModule = false;
                }

                // 标定
                else if (name == "Calibration")
                {
                    if (Ho_Image == null || DrawingCanvas.Strokes.Count < 1)
                    {
                        return;
                    }

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

                    // 标定过程
                    try
                    {
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

                            // 画线 只保留矩形框
                            while (DrawingCanvas.Strokes.Count > 1)
                            {
                                DrawingCanvas.Strokes.RemoveAt(1);
                            }
                            DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateParallelLines(PointsMeasured));
                        }

                        HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);
                        hv_Distance_Measure.Dispose();
                        hv_Row_Measure.Dispose();
                        hv_Column_Measure.Dispose();
                    }
                    catch (Exception)
                    {
                        HandyControl.Controls.Growl.Error("异常操作...卡尺标定");
                    }
                }
            }
            catch (Exception)
            {
                HandyControl.Controls.Growl.Error("异常操作...卡尺标定");
            }
        }
        #endregion
        ////////////////////////////////// 方法 //////////////////////////////////
        #region
        private void DrawingBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanMove = true;
            PointMoveOri = e.GetPosition(e.Device.Target);

            // 双击 图像还原
            if (e.ClickCount > 1)
            {
                Halcon.SetFullImagePart();
                WheelScrollValue = 0;
                // 清空 Strokes
                DrawingCanvas.Strokes.Clear();
            }
        }

        private void DrawingBorder_MouseMove(object sender, MouseEventArgs e)
        {
            Point curPoint = e.GetPosition(e.Device.Target);

            //////////////////////////////////////// 当前点信息 ////////////////////////////////////////
            if (Ho_Image != null)
            {
                Point point = Halcon.ControlPointToHImagePoint(curPoint.X, curPoint.Y);
                int row = (int)point.Y;
                int column = (int)point.X;
                StrCurPosition = "X = " + column + ", " + "Y = " + row;
                try
                {
                    HOperatorSet.GetGrayval(Ho_Image, point.Y, point.X, out HTuple ho_Grayval);
                    StrCurGrayValue = ho_Grayval.ToString();
                    ho_Grayval.Dispose();
                }
                catch (Exception)
                {
                    StrCurGrayValue = "null";
                }
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
                if (ModuleEditType == EnumModuleEditType.Move)
                {
                    DrawingBorder.Cursor = Cursors.SizeAll;
                }
                else if (ModuleEditType == EnumModuleEditType.Rotate)
                {
                    DrawingBorder.Cursor = Cursors.Hand;
                }
                else if (ModuleEditType == EnumModuleEditType.Resize)
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
                    if (ModuleEditType == EnumModuleEditType.Move)
                    {
                        Matrix matrixMove = new Matrix();
                        matrixMove.Translate(curPoint.X - PointMoveOri.X, curPoint.Y - PointMoveOri.Y);
                        DrawingCanvas.Strokes.Transform(matrixMove, false);
                        // 更新初始移动位置
                        PointMoveOri = curPoint;
                        return;
                    }
                    // 旋转
                    else if (ModuleEditType == EnumModuleEditType.Rotate)
                    {
                        // 获取旋转中心
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                        Point p0 = new Point(0.5 * (sps[0].X + sps[2].X), 0.5 * (sps[0].Y + sps[2].Y));
                        // 旋转角度，顺时针为正，逆时针为负
                        double angle = InkCanvasMethod.GetPointAngle(p0, PointMoveOri, curPoint);
                        Matrix matrixRotate = new Matrix();
                        if (InkCanvasMethod.GetRotateDirection(p0, PointMoveOri, curPoint))
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
                        if (InkCanvasMethod.GetDistancePP(curPoint, p0) < 5)
                        {
                            return;
                        }

                        // 计算倾角 假设 +x 方向有个点
                        Point px = new Point(DrawingCanvas.Strokes[0].GetBounds().Right + DrawingCanvas.Strokes[0].GetBounds().Width, sps[1].Y);
                        double angle = InkCanvasMethod.GetPointAngle((Point)sps[1], (Point)sps[2], px);
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
                    ModuleEditType = EnumModuleEditType.None;
                }
            }

            if (!CanMove)
            {
                return;
            }

            //////////////////////////////////////// 自由绘制矩形模板 ////////////////////////////////////////
            if (IsMakingModule)
            {
                DrawingCanvas.Strokes.Clear();
                DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateCaliperRectangle(PointMoveOri, curPoint));
                return;
            }

            //////////////////////////////////////// 图像平移 ////////////////////////////////////////
            // 图像窗口平移
            Halcon.HShiftWindowContents(PointMoveOri.X - curPoint.X, PointMoveOri.Y - curPoint.Y);
            // Strokes 平移
            Matrix matrix = new Matrix();
            matrix.Translate(curPoint.X - PointMoveOri.X, curPoint.Y - PointMoveOri.Y);
            DrawingCanvas.Strokes.Transform(matrix, false);
            // 更新起点
            PointMoveOri = curPoint;
        }

        private void DrawingBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CanMove = false;
        }

        private void DrawingBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
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
            // 当前点为中心缩放
            Point curPoint = e.GetPosition(e.Device.Target);
            Matrix matrix = new Matrix();
            if (e.Delta > 0)
            {
                WheelScrollValue += 1;
                // 控制缩放比例
                if (WheelScrollValue > WheelScrollMax)
                {
                    WheelScrollValue = WheelScrollMax;
                    // 退出 不继续执行
                    return;
                }
                matrix.ScaleAt(Halcon.HZoomFactor, Halcon.HZoomFactor, curPoint.X, curPoint.Y);
                Halcon.HZoomWindowContents(curPoint.X, curPoint.Y, 1);
            }
            else
            {
                WheelScrollValue -= 1;
                // 控制缩放比例
                if (WheelScrollValue < WheelScrollMin)
                {
                    WheelScrollValue = WheelScrollMin;
                    // 退出 不继续执行
                    return;
                }
                matrix.ScaleAt(1 / Halcon.HZoomFactor, 1 / Halcon.HZoomFactor, curPoint.X, curPoint.Y);
                Halcon.HZoomWindowContents(curPoint.X, curPoint.Y, -1);
            }
            DrawingCanvas.Strokes.Transform(matrix, false);
        }

        /// <summary>
        /// 计时器
        /// </summary>
        private void InitTimer()
        {
            DispatcherTimer update_timer = new DispatcherTimer
            {
                // 1s 执行一次
                Interval = new TimeSpan(0, 0, 1)
            };
            update_timer.Tick += new EventHandler(RefreshDateTime);
            update_timer.Start();
        }
        private void RefreshDateTime(object sender, EventArgs e)
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                StrCurTime = string.Format("{0:G}", DateTime.Now);
            });

            // WinForm 引用用户控件启动时报错
            // Application.Current 始终为 null 
            // 采用以下方式更新
            //_ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    StrCurTime = string.Format("{0:G}", DateTime.Now);
            //}));
        }
        #endregion
    }
}