using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HalconWPF.Method;
using HalconWPF.Model;
using HalconWPF.UserControl;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/9 19:29:55
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/9 19:29:55    Taosy.W                 
    ///
    public class MeasureToolsViewModel : ViewModelBase
    {
        ////////////////////////////////// 绑定属性 //////////////////////////////////
        /// <summary>
        /// 模板类型
        /// </summary>
        private Dictionary<EnumMeasureTools, string> enumToolsCal;
        public Dictionary<EnumMeasureTools, string> EnumToolsCal
        {
            get
            {
                Dictionary<EnumMeasureTools, string> pairs = new Dictionary<EnumMeasureTools, string>();
                foreach (EnumMeasureTools item in Enum.GetValues(typeof(EnumMeasureTools)))
                {
                    DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                    pairs.Add(item, attributes.Description);
                }
                return pairs;
            }

            set => Set(ref enumToolsCal, value);
        }

        private EnumMeasureTools enumToolsSel;
        public EnumMeasureTools EnumToolsSel
        {
            get => enumToolsSel;
            set => Set(ref enumToolsSel, value);
        }

        /// <summary>
        /// 编辑模式
        /// </summary>
        private bool boolEditMode;
        public bool BoolEditMode
        {
            get => boolEditMode;
            set => Set(ref boolEditMode, value);
        }

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

        // 显示信息
        private string strInfo;
        public string StrInfo
        {
            get => strInfo;
            set => Set(ref strInfo, value);
        }

        //////////////////////////////////// 变量 ////////////////////////////////////
        private HSmartWindowControlWPF Halcon { get; set; }
        private HWindow Ho_Window { get; set; }
        private InkCanvas DrawingCanvas { get; set; }
        private Border DrawingBorder { get; set; }

        /// <summary>
        /// 缩放参数
        /// </summary>
        private int WheelScrollValue { get; set; } = 0;
        private int WheelScrollMin { get; set; } = -5;
        private int WheelScrollMax { get; set; } = -15;

        /// <summary>
        /// 绘制模板 编辑
        /// </summary>
        private Point PointMoveOri { get; set; } = new Point();
        private bool IsMakingModule { get; set; } = false;
        private bool IsEditingModule { get; set; } = false;
        private bool CanMove { get; set; } = false;
        private EnumModuleEditType ModuleEditType { get; set; }
        private HTuple Hv_ShapeModelID = new HTuple();

        /// <summary>
        /// 图像
        /// </summary>
        private HObject Ho_Image = null;

        ////////////////////////////////// 构造函数 //////////////////////////////////
        /// <summary>
        /// 构造函数 初始化参数
        /// </summary>
        public MeasureToolsViewModel()
        {
            EnumToolsSel = EnumMeasureTools.distance;
            BoolEditMode = false;
            ModuleEditType = EnumModuleEditType.None;

            StrCurPosition = "X = null, Y = null";
            StrCurGrayValue = "null";
            StrInfo = "null";

            // 计时器更新时间
            InitTimer();
        }

        ////////////////////////////////// 绑定命令//////////////////////////////////
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as MeasureTools).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            DrawingCanvas = (e.Source as MeasureTools).DrawingCanvas;
            DrawingBorder = (e.Source as MeasureTools).DrawingBorder;

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
            // 创建模板
            if (name == "Create")
            {
                HOperatorSet.GenEmptyObj(out Ho_Image);
                Ho_Image.Dispose();
                HOperatorSet.ReadImage(out Ho_Image, @"Image\calibration_circle.bmp");
                // 显示属性
                Ho_Window.SetColor("red");
                Ho_Window.SetLineWidth(1);
                Ho_Window.DispObj(Ho_Image);
                Halcon.SetFullImagePart();
                // 锁定
                DrawingCanvas.Strokes.Clear();
                BoolEditMode = false;
                IsMakingModule = true;
                StrInfo = "null";
            }
        }

        ////////////////////////////////// 方法 //////////////////////////////////
        /// <summary>
        /// 鼠标左键弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CanMove = false;
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBorder_MouseMove(object sender, MouseEventArgs e)
        {
            Point curPoint = e.GetPosition(e.Device.Target);

            //////////////////////////////////////// 当前点信息
            if (Ho_Image != null)
            {
                Point point = Halcon.ControlPointToHImagePoint(curPoint.X, curPoint.Y);
                int row = (int)point.Y;
                int column = (int)point.X;
                StrCurPosition = "X = " + column + ", " + "Y = " + row;
                // 实时容易出问题
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

            //////////////////////////////////////// 编辑模板
            if (BoolEditMode)
            {
                if (DrawingCanvas.Strokes.Count < 1)
                {
                    return;
                }

                if (!IsEditingModule)
                {
                    // 会卡顿 只判断一次 不同位置对应不同鼠标形状
                    ModuleEditType = DrawingCanvas.Strokes[0].GetMeasureTools(curPoint, EnumToolsSel);
                }

                IsEditingModule = true;
                // 根据编辑状态设置鼠标形状
                if (ModuleEditType == EnumModuleEditType.Move)
                {
                    DrawingBorder.Cursor = Cursors.SizeAll;
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
                    // 缩放
                    else if (ModuleEditType == EnumModuleEditType.Resize)
                    {
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints;
                        // 找到选中点 鼠标滑动点距离最近的点
                        double r0 = InkCanvasMethod.GetDistancePP((Point)sps[0], PointMoveOri);
                        int index = 0;
                        for (int i = 1; i < sps.Count; i++)
                        {
                            double r = InkCanvasMethod.GetDistancePP((Point)sps[i], PointMoveOri);
                            if (r0 > r)
                            {
                                r0 = r;
                                index = i;
                            }
                        }
                        // 将选中点的坐标替换为新的坐标
                        sps[index] = new StylusPoint(curPoint.X, curPoint.Y);
                        // 更新初始移动位置
                        PointMoveOri = curPoint;

                        // 显示信息
                        if (EnumToolsSel == EnumMeasureTools.distance)
                        {
                            PointCollection pts = Halcon.ControlPointToHImagePoint(DrawingCanvas.Strokes[0].StylusPoints);
                            StrInfo = "当前距离为：" + InkCanvasMethod.GetDistancePP(pts[0], pts[1]).ToString("F2") + " pixel";
                        }
                    }
                }
                else
                {
                    IsEditingModule = false;
                    ModuleEditType = EnumModuleEditType.None;
                }
            }

            //////////////////////////////////////// 创建模板
            if (IsMakingModule && CanMove)
            {
                DrawingCanvas.Strokes.Clear();
                if (EnumToolsSel == EnumMeasureTools.distance)
                {
                    DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateArrowDistance(PointMoveOri, curPoint));
                    PointCollection pts = Halcon.ControlPointToHImagePoint(DrawingCanvas.Strokes[0].StylusPoints);
                    StrInfo = "当前距离为：" + InkCanvasMethod.GetDistancePP(pts[0], pts[1]).ToString("F2") + " pixel";
                }
                else if (EnumToolsSel == EnumMeasureTools.angle)
                {
                }
                return;
            }

            //////////////////////////////////////// 图像平移
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
        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanMove = true;
            PointMoveOri = e.GetPosition(e.Device.Target);

            // 双击 图像还原
            if (e.ClickCount > 1)
            {
                Halcon.SetFullImagePart();
                WheelScrollValue = 0;
                DrawingCanvas.Strokes.Clear();
                BoolEditMode = false;
            }
            //else
            //{
            //    // 创建 PolyLine 添加点 在 MouseMove 中绘制实时效果 
            //    if (IsMakingModule && EnumModuleSel == EnumModuleType.polygon)
            //    {
            //        if (DrawingCanvas.Strokes.Count == 0)
            //        {
            //            DrawingCanvas.Strokes.Add(InkCanvasMethod.CreatePolyline(new StylusPointCollection { new StylusPoint(PointMoveOri.X, PointMoveOri.Y) }));
            //        }
            //        else
            //        {
            //            DrawingCanvas.Strokes[0].StylusPoints.Add(new StylusPoint(PointMoveOri.X, PointMoveOri.Y));
            //        }
            //    }
            //}
        }
        ///鼠标滚轮滚动
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 右键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 双击 锁定模板
            if (e.ClickCount > 1)
            {
                BoolEditMode = false;
                DrawingBorder.Cursor = Cursors.Arrow;
            }
            else
            {
                //// 多边形比较特殊 右键完成创建
                //if (IsMakingModule && EnumModuleSel == EnumModuleType.polygon)
                //{
                //    // 创建过程中是 PolyLine
                //    if (DrawingCanvas.Strokes.Count == 1 && DrawingCanvas.Strokes[0].GetType().Name.ToString() == "CustomPolyline")
                //    {
                //        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                //        DrawingCanvas.Strokes.Clear();
                //        // 多边形至少 3 个点
                //        if (sps.Count > 2)
                //        {
                //            DrawingCanvas.Strokes.Add(InkCanvasMethod.CreatePolygon(sps));
                //        }
                //    }
                //}

                // 创建模板完成 开启编辑状态
                IsMakingModule = false;
                BoolEditMode = true;
            }
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

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshDateTime(object sender, EventArgs e)
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                StrCurTime = string.Format("{0:G}", DateTime.Now);
            }
            );
        }
    }
}