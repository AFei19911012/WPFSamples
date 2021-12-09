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
using Microsoft.Win32;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/7 8:43:48
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/7 8:43:48    Taosy.W                 
    ///
    public class DomainModuleViewModel : ViewModelBase
    {
        ////////////////////////////////// 绑定属性 //////////////////////////////////
        /// <summary>
        /// 模板类型
        /// </summary>
        private Dictionary<EnumModuleType, string> enumModuleCal;
        public Dictionary<EnumModuleType, string> EnumModuleCal
        {
            get
            {
                Dictionary<EnumModuleType, string> pairs = new Dictionary<EnumModuleType, string>();
                foreach (EnumModuleType item in Enum.GetValues(typeof(EnumModuleType)))
                {
                    DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                    pairs.Add(item, attributes.Description);
                }
                return pairs;
            }

            set => Set(ref enumModuleCal, value);
        }

        private EnumModuleType enumModuleSel;
        public EnumModuleType EnumModuleSel
        {
            get => enumModuleSel;
            set => Set(ref enumModuleSel, value);
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

        //////////////////////////////////// 变量 ////////////////////////////////////
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
        private bool IsMakingModule { get; set; }
        private bool IsModuleEditing { get; set; }
        private bool CanMove { get; set; }
        private EnumModuleEditType ModuleEditType { get; set; }
        private HTuple Hv_ShapeModelID = new HTuple();

        /// <summary>
        /// 图像
        /// </summary>
        private HObject Ho_Image = null;

        /// <summary>
        /// 首次显示
        /// </summary>
        private bool IsFirstShow { get; set; }

        ////////////////////////////////// 构造函数 //////////////////////////////////
        /// <summary>
        /// 构造函数 初始化参数
        /// </summary>
        public DomainModuleViewModel()
        {
            EnumModuleSel = EnumModuleType.rectangle;
            BoolEditMode = false;
            PointMoveOri = new Point();
            CanMove = false;
            IsMakingModule = false;
            IsModuleEditing = false;
            IsFirstShow = true;

            WheelScrollValue = 0;
            WheelScrollMin = 0;
            WheelScrollMax = 20;

            ModuleEditType = EnumModuleEditType.None;

            StrCurPosition = "X = null, Y = null";
            StrCurGrayValue = "null";

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
            Halcon = (e.Source as DomainModule).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            DrawingCanvas = (e.Source as DomainModule).DrawingCanvas;
            DrawingBorder = (e.Source as DomainModule).DrawingBorder;

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
                // 编辑模板
                if (name == "Edit")
                {
                    HOperatorSet.GenEmptyObj(out Ho_Image);
                    Ho_Image.Dispose();
                    HOperatorSet.ReadImage(out Ho_Image, @"..\HalconWPF\Resource\Image\calibration_circle.bmp");
                    // 显示属性
                    Ho_Window.SetColor("red");
                    Ho_Window.SetLineWidth(1);
                    Ho_Window.DispObj(Ho_Image);
                    Halcon.HZoomFactor = 1.25;
                    if (IsFirstShow)
                    {
                        IsFirstShow = false;
                        Halcon.SetFullImagePart();
                    }
                    // 锁定
                    DrawingCanvas.Strokes.Clear();
                    BoolEditMode = false;
                    IsMakingModule = true;
                }
                // 创建模板
                else if (name == "Create")
                {
                    try
                    {
                        // 绘制了模板
                        if (DrawingCanvas.Strokes.Count == 1 && Ho_Image != null)
                        {
                            // 坐标转换：控件 → 图像
                            PointCollection points = Halcon.ControlPointToHImagePoint(DrawingCanvas.Strokes[0].StylusPoints);
                            // 抠图
                            HOperatorSet.GenEmptyObj(out HObject ho_Region);
                            ho_Region.Dispose();
                            // 矩形模板 椭圆模板
                            if (DrawingCanvas.Strokes[0] is CustomRectangle || DrawingCanvas.Strokes[0] is CustomEllipse)
                            {
                                // 旋转角度
                                Point px = new Point(points[4].X + InkCanvasMethod.GetDistancePP(points[2], points[4]), points[2].Y);
                                // 旋转角度（弧度）
                                double angle = InkCanvasMethod.GetPointAngle(points[2], points[4], px) * Math.PI / 180;
                                if (points[4].Y > points[2].Y)
                                {
                                    angle = -angle;
                                }
                                // 宽高
                                double len1 = 0.5 * InkCanvasMethod.GetDistancePP(points[0], points[2]);
                                double len2 = 0.5 * InkCanvasMethod.GetDistancePP(points[2], points[4]);
                                // 中心点
                                double row = 0.5 * (points[0].Y + points[4].Y);
                                double col = 0.5 * (points[0].X + points[4].X);
                                if (DrawingCanvas.Strokes[0] is CustomRectangle)
                                {
                                    HOperatorSet.GenRectangle2(out ho_Region, row, col, angle, len2, len1);
                                }
                                else
                                {
                                    HOperatorSet.GenEmptyObj(out HObject ho_Contour);
                                    ho_Contour.Dispose();
                                    HOperatorSet.GenEllipseContourXld(out ho_Contour, row, col, angle, len2, len1, 0, 6.28318, "positive", 1);
                                    HOperatorSet.GenRegionContourXld(ho_Contour, out ho_Region, "filled");
                                    ho_Contour.Dispose();
                                }
                            }
                            // 圆模板
                            else if (DrawingCanvas.Strokes[0] is CustomCircle)
                            {
                                // 半径
                                double radius = InkCanvasMethod.GetDistancePP(points[0], points[1]);
                                HOperatorSet.GenCircle(out ho_Region, points[0].Y, points[0].X, radius);
                            }
                            // 多边形模板
                            else if (DrawingCanvas.Strokes[0] is CustomPolygon)
                            {
                                HTuple hv_rows = new HTuple();
                                HTuple hv_cols = new HTuple();
                                hv_rows.Dispose();
                                hv_cols.Dispose();
                                for (int i = 0; i < points.Count; i++)
                                {
                                    hv_rows[i] = points[i].Y;
                                    hv_cols[i] = points[i].X;
                                }
                                HOperatorSet.GenRegionPolygonFilled(out ho_Region, hv_rows, hv_cols);
                                hv_rows.Dispose();
                                hv_cols.Dispose();
                            }
                            // 抠图
                            HOperatorSet.ReduceDomain(Ho_Image, ho_Region, out HObject ho_ImageReduced);
                            //HOperatorSet.WriteImage(ho_ImageReduced, "bmp", 0, @"ho_ImageReduced.bmp");
                            ho_Region.Dispose();
                            // 继续扣 ROI
                            HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_Region, "max_separability", "dark", out HTuple hv_UsedThreshold);
                            HOperatorSet.DilationCircle(ho_Region, out HObject ho_RegionROI, 2);
                            HOperatorSet.ReduceDomain(ho_ImageReduced, ho_RegionROI, out HObject ho_ImageROI);
                            ho_Region.Dispose();
                            ho_RegionROI.Dispose();
                            hv_UsedThreshold.Dispose();
                            // 创建形状模板
                            Hv_ShapeModelID.Dispose();
                            HOperatorSet.CreateShapeModel(ho_ImageROI, "auto", -0.39, 0.79, "auto", "auto", "use_polarity", "auto", "auto", out Hv_ShapeModelID);
                            ho_ImageROI.Dispose();

                            // 显示模板
                            HOperatorSet.FindShapeModel(ho_ImageReduced, Hv_ShapeModelID, -0.39, 0.79, 0.3, 1, 0.5, "least_squares", 0, 0.9, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle, out HTuple hv_Score);
                            // 获取模板轮廓
                            HOperatorSet.GetShapeModelContours(out HObject ho_ShapeModel, Hv_ShapeModelID, 1);
                            // 匹配结果
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out HTuple hv_HomMat2DRotate);
                            HOperatorSet.AffineTransContourXld(ho_ShapeModel, out HObject ho_ModelTrans, hv_HomMat2DRotate);
                            Ho_Window.DispObj(ho_ModelTrans);
                            HandyControl.Controls.Growl.Info("匹配结果：x = " + (int)hv_Column.D + ", y = " + (int)hv_Row.D + "\n, score = " + hv_Score.D.ToString("F2") + ", angle = " + hv_Angle.D.ToString("F2"));
                            ho_ImageReduced.Dispose();
                            ho_ShapeModel.Dispose();
                            hv_HomMat2DRotate.Dispose();
                            ho_ModelTrans.Dispose();
                            hv_Row.Dispose();
                            hv_Column.Dispose();
                            hv_Angle.Dispose();
                            hv_Score.Dispose();
                            HandyControl.Controls.Growl.Success("模板创建成功...\n");
                        }
                    }
                    catch (Exception)
                    {
                        HandyControl.Controls.Growl.Error("模板创建失败...");
                    }
                }

                // 保存模板
                else if (name == "Save")
                {
                    try
                    {
                        if (Hv_ShapeModelID != null && Hv_ShapeModelID.ToString() != "")
                        {
                            SaveFileDialog dialog = new SaveFileDialog
                            {
                                Title = "保存模板",
                                Filter = "模板文件(*.shm)|*.shm",
                                RestoreDirectory = true,
                                FileName = "module.shm",
                                InitialDirectory = Environment.CurrentDirectory + "\\module",
                            };
                            if (dialog.ShowDialog() != true)
                            {
                                return;
                            }
                            string filename = dialog.FileName;
                            HOperatorSet.WriteShapeModel(Hv_ShapeModelID, filename);

                            HandyControl.Controls.Growl.Success("模板保存成功...");
                        }
                    }
                    catch (Exception)
                    {
                        HandyControl.Controls.Growl.Error("模板保存失败...");
                    }
                }

                // 加载模板
                else if (name == "Load")
                {
                    try
                    {
                        OpenFileDialog dialog = new OpenFileDialog
                        {
                            Title = "加载模板",
                            Filter = "模板文件(*.shm)|*.shm",
                            RestoreDirectory = true,
                            FileName = "module.shm",
                            InitialDirectory = Environment.CurrentDirectory + "\\module",
                        };
                        if (dialog.ShowDialog() != true)
                        {
                            return;
                        }
                        string filename = dialog.FileName;
                        Hv_ShapeModelID.Dispose();
                        HOperatorSet.ReadShapeModel(filename, out Hv_ShapeModelID);

                        HandyControl.Controls.Growl.Success("模板加载成功...");
                    }
                    catch (Exception)
                    {
                        HandyControl.Controls.Growl.Error("模板加载失败...");
                    }
                }

                // 模板匹配
                else if (name == "Match")
                {
                    if (Hv_ShapeModelID != null && Hv_ShapeModelID.ToString() != "[]")
                    {
                        // 当前图像视野范围内寻找模板
                        Point pt1 = Halcon.ControlPointToHImagePoint(1, 1);
                        Point pt2 = Halcon.ControlPointToHImagePoint(Halcon.ActualWidth - 1, Halcon.ActualHeight - 1);
                        HOperatorSet.GenRectangle1(out HObject ho_Region, pt1.Y, pt1.X, pt2.Y, pt2.X);
                        HOperatorSet.ReduceDomain(Ho_Image, ho_Region, out HObject ho_ImageReduced);
                        ho_Region.Dispose();
                        //Ho_Window.DispObj(ho_ImageReduced);

                        // 直接匹配 匹配 9 个
                        HOperatorSet.FindShapeModel(ho_ImageReduced, Hv_ShapeModelID, -0.39, 0.79, 0.5, 9, 0.5, "least_squares", 0, 0.9, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle, out HTuple hv_Score);

                        if (hv_Angle.Length > 0)
                        {
                            List<CShapeModelMatchResult> results = new List<CShapeModelMatchResult>();
                            for (int i = 0; i < hv_Angle.TupleLength(); i++)
                            {
                                CShapeModelMatchResult match = new CShapeModelMatchResult
                                {
                                    Row = hv_Row[i].D,
                                    Column = hv_Column[i].D,
                                    Angle = hv_Angle[i].D,
                                    Score = hv_Score[i].D
                                };
                                results.Add(match);
                            }

                            // 保证每次的 Cross 大小一致
                            double r = 10 * Math.Pow(Halcon.HZoomFactor, WheelScrollValue);
                            DrawingCanvas.Strokes.Clear();
                            Ho_Window.DispObj(Ho_Image);
                            foreach (CShapeModelMatchResult item in results)
                            {
                                if (item.Score >= 0.85)
                                {
                                    // 获取模板轮廓
                                    HOperatorSet.GetShapeModelContours(out HObject ho_ShapeModel, Hv_ShapeModelID, 1);
                                    // 匹配结果
                                    HOperatorSet.VectorAngleToRigid(0, 0, 0, item.Row, item.Column, item.Angle, out HTuple hv_HomMat2DRotate);
                                    HOperatorSet.AffineTransContourXld(ho_ShapeModel, out HObject ho_ModelTrans, hv_HomMat2DRotate);
                                    ho_ShapeModel.Dispose();
                                    hv_HomMat2DRotate.Dispose();
                                    Ho_Window.DispObj(ho_ModelTrans);
                                    ho_ModelTrans.Dispose();
                                    // 绘制 Marker 点
                                    DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateMarkerCross(Halcon.ControlPointToHImagePoint(item.Column, item.Row, true), r));
                                    HandyControl.Controls.Growl.Info("匹配结果：x = " + (int)item.Column + ", y = " + (int)item.Row + "\n, score = " + item.Score.ToString("F2") + ", angle = " + item.Angle.ToString("F2"));
                                }
                            }
                        }
                        hv_Row.Dispose();
                        hv_Column.Dispose();
                        hv_Angle.Dispose();
                        hv_Score.Dispose();
                        ho_ImageReduced.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                HandyControl.Controls.Growl.Error("异常操作...设计模板");
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

                if (!IsModuleEditing)
                {
                    // 会卡顿 只判断一次 不同位置对应不同鼠标形状
                    ModuleEditType = DrawingCanvas.Strokes[0].GetModuleEditType(curPoint, EnumModuleSel);
                }

                IsModuleEditing = true;
                // 根据编辑状态设置鼠标形状
                if (ModuleEditType == EnumModuleEditType.Move)
                {
                    DrawingBorder.Cursor = Cursors.SizeAll;
                }
                else if (ModuleEditType == EnumModuleEditType.Rotate)
                {
                    DrawingBorder.Cursor = Cursors.Hand;
                }
                else if (ModuleEditType == EnumModuleEditType.SizeAll)
                {
                    DrawingBorder.Cursor = Cursors.Cross;
                }
                else if (ModuleEditType == EnumModuleEditType.SizeW || ModuleEditType == EnumModuleEditType.SizeE)
                {
                    DrawingBorder.Cursor = Cursors.SizeWE;
                }
                else if (ModuleEditType == EnumModuleEditType.SizeN || ModuleEditType == EnumModuleEditType.SizeS)
                {
                    DrawingBorder.Cursor = Cursors.SizeNS;
                }
                else
                {
                    IsModuleEditing = false;
                    DrawingBorder.Cursor = Cursors.Arrow;
                }

                // 编辑
                if (CanMove && IsModuleEditing)
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
                        // 矩形和椭圆模板支持旋转
                        if (EnumModuleSel == EnumModuleType.rectangle || EnumModuleSel == EnumModuleType.ellipse)
                        {
                            // 获取旋转中心
                            StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                            Point p0 = new Point(0.5 * (sps[0].X + sps[4].X), 0.5 * (sps[0].Y + sps[4].Y));
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
                    }
                    // 缩放
                    else
                    {
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints;
                        // 区分模板
                        if (EnumModuleSel == EnumModuleType.rectangle || EnumModuleSel == EnumModuleType.ellipse)
                        {
                            Point p0 = new Point(0.5 * (sps[0].X + sps[4].X), 0.5 * (sps[0].Y + sps[4].Y));
                            if (InkCanvasMethod.GetDistancePP(curPoint, p0) < 5)
                            {
                                return;
                            }
                            Point pt = new Point(curPoint.X, curPoint.Y);
                            // 用 3、5 两个点计算倾角 假设 +x 方向有个点
                            Point px = new Point(DrawingCanvas.Strokes[0].GetBounds().Right + DrawingCanvas.Strokes[0].GetBounds().Width, sps[2].Y);
                            double angle = InkCanvasMethod.GetPointAngle((Point)sps[2], (Point)sps[4], px);
                            // 判断顺时针、逆时针
                            if (sps[4].Y > px.Y)
                            {
                                angle = -angle;
                            }

                            // 先还原到初始状态 不同方向有不同的旋转中心
                            Matrix matrixResize = new Matrix();
                            if (ModuleEditType == EnumModuleEditType.SizeAll)
                            {
                                matrixResize.RotateAt(angle, p0.X, p0.Y);
                                DrawingCanvas.Strokes[0].Transform(matrixResize, false);
                                // 起始点也做变换                     
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
                            }
                            else
                            {
                                int idx = 1;
                                if (ModuleEditType == EnumModuleEditType.SizeW)
                                {
                                    idx = 5;
                                }
                                else if (ModuleEditType == EnumModuleEditType.SizeN)
                                {
                                    idx = 3;
                                }
                                else if (ModuleEditType == EnumModuleEditType.SizeS)
                                {
                                    idx = 7;
                                }

                                matrixResize.RotateAt(angle, sps[idx].X, sps[idx].Y);
                                DrawingCanvas.Strokes[0].Transform(matrixResize, false);
                                // 起始点也做变换
                                PointMoveOri = matrixResize.Transform(PointMoveOri);
                                pt = matrixResize.Transform(pt);
                                // 缩放
                                double ratio = InkCanvasMethod.GetDistancePP(pt, (Point)sps[idx]) / InkCanvasMethod.GetDistancePP(PointMoveOri, (Point)sps[idx]);
                                matrixResize = new Matrix();
                                if (idx == 3 || idx == 7)
                                {
                                    matrixResize.ScaleAt(1, ratio, sps[idx].X, sps[idx].Y);
                                }
                                else
                                {
                                    matrixResize.ScaleAt(ratio, 1, sps[idx].X, sps[idx].Y);
                                }

                                // 还原
                                matrixResize.RotateAt(-angle, sps[idx].X, sps[idx].Y);
                                DrawingCanvas.Strokes[0].Transform(matrixResize, false);
                            }
                        }
                        else if (EnumModuleSel == EnumModuleType.circle)
                        {
                            Point p0 = (Point)sps[0];
                            Point p1 = (Point)sps[1];
                            double radius = InkCanvasMethod.GetDistancePP(p0, p1);
                            double r = InkCanvasMethod.GetDistancePP(p0, curPoint);
                            Matrix matrixResize = new Matrix();
                            matrixResize.ScaleAt(r / radius, r / radius, p0.X, p0.Y);
                            DrawingCanvas.Strokes[0].Transform(matrixResize, false);
                        }
                        else if (EnumModuleSel == EnumModuleType.polygon)
                        {
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
                        }

                        // 更新初始移动位置
                        PointMoveOri = curPoint;
                    }
                }
                else
                {
                    IsModuleEditing = false;
                    ModuleEditType = EnumModuleEditType.None;
                }
            }

            //////////////////////////////////////// 创建模板
            if (IsMakingModule)
            {
                // 多边形比较特殊
                if (EnumModuleSel == EnumModuleType.polygon)
                {
                    // 创建过程中是 PolyLine
                    if (DrawingCanvas.Strokes.Count == 1 && DrawingCanvas.Strokes[0].GetType().Name.ToString() == "CustomPolyline")
                    {
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                        int count = sps.Count;
                        // 添加 替换
                        if (count > 1 && InkCanvasMethod.GetDistancePP(PointMoveOri, curPoint) > 1e-6)
                        {
                            sps.RemoveAt(count - 1);
                        }
                        sps.Add(new StylusPoint(curPoint.X, curPoint.Y));

                        // 重绘
                        DrawingCanvas.Strokes.Clear();
                        DrawingCanvas.Strokes.Add(InkCanvasMethod.CreatePolyline(sps));
                    }
                    return;
                }

                if (!CanMove)
                {
                    return;
                }
                DrawingCanvas.Strokes.Clear();
                if (EnumModuleSel == EnumModuleType.rectangle)
                {
                    DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateRectangle(PointMoveOri, curPoint));
                }
                else if (EnumModuleSel == EnumModuleType.ellipse)
                {
                    DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateEllipse(PointMoveOri, curPoint));
                }
                else if (EnumModuleSel == EnumModuleType.circle)
                {
                    DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateCircle(PointMoveOri, curPoint));
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
            else
            {
                // 创建 PolyLine 添加点 在 MouseMove 中绘制实时效果 
                if (IsMakingModule && EnumModuleSel == EnumModuleType.polygon)
                {
                    if (DrawingCanvas.Strokes.Count == 0)
                    {
                        DrawingCanvas.Strokes.Add(InkCanvasMethod.CreatePolyline(new StylusPointCollection { new StylusPoint(PointMoveOri.X, PointMoveOri.Y) }));
                    }
                    else
                    {
                        DrawingCanvas.Strokes[0].StylusPoints.Add(new StylusPoint(PointMoveOri.X, PointMoveOri.Y));
                    }
                }
            }
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
                // 多边形比较特殊 右键完成创建
                if (IsMakingModule && EnumModuleSel == EnumModuleType.polygon)
                {
                    // 创建过程中是 PolyLine
                    if (DrawingCanvas.Strokes.Count == 1 && DrawingCanvas.Strokes[0].GetType().Name.ToString() == "CustomPolyline")
                    {
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                        DrawingCanvas.Strokes.Clear();
                        // 多边形至少 3 个点
                        if (sps.Count > 2)
                        {
                            DrawingCanvas.Strokes.Add(InkCanvasMethod.CreatePolygon(sps));
                        }
                    }
                }

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