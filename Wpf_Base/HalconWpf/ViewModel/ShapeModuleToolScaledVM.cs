
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Wpf_Base.CcdWpf;
using Wpf_Base.ControlsWpf;
using Wpf_Base.HalconWpf.Method;
using Wpf_Base.HalconWpf.Model;
using Wpf_Base.HalconWpf.Tools;
using Wpf_Base.HalconWpf.Views;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.HalconWpf.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/20 17:55:59
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/20 17:55:59    CoderMan/CoderdMan1012         首次编写         
    ///
    public class ShapeModuleToolScaledVM : ViewModelBase
    {
        #region 1. 绑定变量
        /// <summary>
        /// 文字
        /// </summary>
        private string strGrabContent = "开始采集";
        public string StrGrabContent
        {
            get => strGrabContent;
            set => Set(ref strGrabContent, value);
        }

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

        private EnumModuleType enumModuleSel = EnumModuleType.rectangle;
        public EnumModuleType EnumModuleSel
        {
            get => enumModuleSel;
            set => Set(ref enumModuleSel, value);
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
        #endregion

        #region 2. 全局变量
        private HSmartWindowControlWPF Halcon { get; set; }
        private HWindow Ho_Window { get; set; }
        private InkCanvas DrawingCanvas { get; set; }
        private Border DrawingBorder { get; set; }

        private ImageChannelVM MyImageChannelVM { get; set; }

        /// <summary>
        /// 模板匹配
        /// </summary>
        private CreateScaledShapeModelVM MyCreateScaledShapeModelVM { get; set; }
        private FindScaledShapeModelVM MyFindScaledShapeModelVM { get; set; }

        private PixelValueControl MyPixelValueControl { get; set; }

        /// <summary>
        /// 绘制模板 编辑
        /// </summary>
        private Point PointMoveOri { get; set; } = new Point();
        private EnumModuleEditState ModuleEditState { get; set; } = EnumModuleEditState.none;
        private bool CanMove { get; set; } = false;
        private bool CanEdit { get; set; } = false;
        private EnumModuleEditType ModuleEditType { get; set; } = EnumModuleEditType.None;
        private HTuple Hv_ShapeModelID = new HTuple();
        /// <summary>
        /// 图像
        /// </summary>
        private HObject Ho_Image = null;
        private bool IsFirstShow { get; set; } = true;

        private bool CanRefreshImage { get; set; } = false;

        /// <summary>
        /// 笔记粗细，主要用于涂抹式选择模板
        /// </summary>
        public static double InkStrokeThickness { get; set; } = CConstants.InkStrokeThickness;
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


        public ShapeModuleToolScaledVM()
        {
        }

        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as ScaledShapeModuleTool).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            DrawingCanvas = (e.Source as ScaledShapeModuleTool).DrawingCanvas;
            DrawingBorder = (e.Source as ScaledShapeModuleTool).DrawingBorder;
            MyCreateScaledShapeModelVM = (e.Source as ScaledShapeModuleTool).MyCreateScaledShapeModelControl.DataContext as CreateScaledShapeModelVM;
            MyFindScaledShapeModelVM = (e.Source as ScaledShapeModuleTool).MyFindScaledShapeModelControl.DataContext as FindScaledShapeModelVM;
            MyImageChannelVM = (e.Source as ScaledShapeModuleTool).MyImageChannelControl.DataContext as ImageChannelVM;
            MyPixelValueControl = (e.Source as ScaledShapeModuleTool).MyPixelValueControl;

            // 鼠标事件
            DrawingBorder.MouseWheel += DrawingBorder_MouseWheel;
            DrawingBorder.MouseLeftButtonDown += DrawingBorder_MouseLeftButtonDown;
            DrawingBorder.MouseMove += DrawingBorder_MouseMove;
            DrawingBorder.MouseLeftButtonUp += DrawingBorder_MouseLeftButtonUp;
            DrawingBorder.MouseRightButtonDown += DrawingBorder_MouseRightButtonDown;

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
                    MyImageChannelVM.TransHImage(ref Ho_Image);
                    Ho_Window.DispObj(Ho_Image);
                    Ho_Window.SetColor(EnumHalColor.magenta.ToColorString());
                    Ho_Window.SetLineWidth(3);
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
        /// 保存模板
        /// </summary>
        public RelayCommand CmdSaveModule => new Lazy<RelayCommand>(() => new RelayCommand(SaveModule)).Value;
        private void SaveModule()
        {
            PrintLog("保存模板", EnumLogType.Debug);
            if (Hv_ShapeModelID.ToString().Length > 5)
            {
                string filename = HalconIoMethod.GenShapeModelName(StrRecipeName);
                HOperatorSet.WriteShapeModel(Hv_ShapeModelID, filename);
                PrintLog("模板保存完成：" + filename, EnumLogType.Success);
            }
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        public RelayCommand CmdLoadModule => new Lazy<RelayCommand>(() => new RelayCommand(LoadModule)).Value;
        private void LoadModule()
        {
            try
            {
                PrintLog("加载模板", EnumLogType.Info);
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "加载模板",
                    Filter = "模板文件(*.shm)|*.shm",
                    RestoreDirectory = true,
                    FileName = StrRecipeName + ".shm",
                    InitialDirectory = Environment.CurrentDirectory + "\\Module",
                };
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                string filename = dialog.FileName;
                Hv_ShapeModelID.Dispose();
                HOperatorSet.ReadShapeModel(filename, out Hv_ShapeModelID);
                PrintLog("模板加载完成：" + filename, EnumLogType.Success);
            }
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 选择 ROI
        /// </summary>
        public RelayCommand CmdChooseROI => new Lazy<RelayCommand>(() => new RelayCommand(ChooseROI)).Value;
        private void ChooseROI()
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
                // 切换到选择 ROI 状态
                ModuleEditState = EnumModuleEditState.choose_roi;
            }
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 选择模板轮廓
        /// </summary>
        public RelayCommand CmdChooseModule => new Lazy<RelayCommand>(() => new RelayCommand(ChooseModule)).Value;
        private void ChooseModule()
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

                // 切换到制作模板状态
                ModuleEditState = EnumModuleEditState.choose_module;
                // 存本地 再读取
                HOperatorSet.WriteImage(Ho_Image, "bmp", 0, "module_temp.bmp");
                Ho_Image.Dispose();
                HOperatorSet.ReadImage(out Ho_Image, "module_temp.bmp");
                Ho_Window.DispObj(Ho_Image);
                File.Delete("module_temp.bmp");

                if (DrawingCanvas.Strokes.Count == 1)
                {
                    // 坐标转换：控件 → 图像
                    PointCollection points = Halcon.ControlPointToHImagePoint(DrawingCanvas.Strokes[0].StylusPoints);

                    // ROI 抠图
                    HOperatorSet.GenEmptyObj(out HObject ho_Region);
                    ho_Region.Dispose();
                    // 矩形 椭圆
                    if (DrawingCanvas.Strokes[0] is CustomRectangle || DrawingCanvas.Strokes[0] is CustomEllipse)
                    {
                        // 旋转角度
                        Point px = new Point(points[4].X + InkMethod.GetDistancePP(points[2], points[4]), points[2].Y);
                        // 旋转角度（弧度）
                        double angle = InkMethod.GetPointAngle(points[2], points[4], px) * Math.PI / 180;
                        if (points[4].Y > points[2].Y)
                        {
                            angle = -angle;
                        }
                        // 宽高
                        double len1 = 0.5 * InkMethod.GetDistancePP(points[0], points[2]);
                        double len2 = 0.5 * InkMethod.GetDistancePP(points[2], points[4]);
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
                    // 圆 ROI
                    else if (DrawingCanvas.Strokes[0] is CustomCircle)
                    {
                        // 半径
                        double radius = InkMethod.GetDistancePP(points[0], points[1]);
                        HOperatorSet.GenCircle(out ho_Region, points[0].Y, points[0].X, radius);
                    }
                    // 多边形
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
                    else
                    {
                        return;
                    }
                    // 抠图
                    if (ho_Region.IsInitialized())
                    {
                        // 判断下 Region 是否有效
                        HOperatorSet.AreaCenter(ho_Region, out HTuple hv_Area, out _, out _);
                        if (hv_Area.D < 2)
                        {
                            ho_Region.Dispose();
                            PrintLog("ROI 无效", EnumLogType.Warning);
                            return;
                        }
                        HOperatorSet.ReduceDomain(Ho_Image, ho_Region, out HObject ho_ImageROI);
                        ho_Region.Dispose();
                        // 创建形状模板：缩放
                        PrintLog("创建形状模板", EnumLogType.Debug);
                        Hv_ShapeModelID.Dispose();
                        HOperatorSet.CreateScaledShapeModel(ho_ImageROI, MyCreateScaledShapeModelVM.StrSelectNumLevels, MyCreateScaledShapeModelVM.NumSelectAngleStart, MyCreateScaledShapeModelVM.NumSelectAngleExtent,
                            MyCreateScaledShapeModelVM.StrSelectAngleStep, MyCreateScaledShapeModelVM.NumScaleMin, MyCreateScaledShapeModelVM.NumScaleMax, MyCreateScaledShapeModelVM.StrSelectScaleStep,
                            MyCreateScaledShapeModelVM.StrSelectOptimization, MyCreateScaledShapeModelVM.StrSelectMetric, MyCreateScaledShapeModelVM.StrSelectContrast,
                            MyCreateScaledShapeModelVM.StrSelectMinContrast, out Hv_ShapeModelID);
                        // 寻找模板
                        PrintLog("寻找模板", EnumLogType.Debug);
                        HOperatorSet.FindScaledShapeModel(Ho_Image, Hv_ShapeModelID, MyFindScaledShapeModelVM.NumSelectAngleStart, MyFindScaledShapeModelVM.NumSelectAngleExtent,
                            MyFindScaledShapeModelVM.NumScaleMin, MyFindScaledShapeModelVM.NumScaleMax, MyFindScaledShapeModelVM.NumMinScore, 1, 
                        MyFindScaledShapeModelVM.NumMaxOverlap, MyFindScaledShapeModelVM.StrSelectSubPixel, MyFindScaledShapeModelVM.IntNumLevels, MyFindScaledShapeModelVM.NumGreediness, 
                        out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle, out HTuple hv_Scale, out HTuple hv_Score);
                        ho_ImageROI.Dispose();
                        if (hv_Row.Length > 0)
                        {
                            // 获取模板轮廓
                            HOperatorSet.GetShapeModelContours(out HObject ho_ShapeModel, Hv_ShapeModelID, 1);
                            // 显示模板
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out HTuple hv_HomMat2DRotate);
                            HOperatorSet.AffineTransContourXld(ho_ShapeModel, out HObject ho_ModelTrans, hv_HomMat2DRotate);
                            Ho_Window.SetColor(EnumHalColor.magenta.ToColorString());
                            Ho_Window.SetLineWidth(3);
                            Ho_Window.DispObj(ho_ModelTrans);
                            ho_ShapeModel.Dispose();
                            ho_ModelTrans.Dispose();
                            hv_HomMat2DRotate.Dispose();

                            MyFindScaledShapeModelVM.NumMatchRow = hv_Row.D;
                            MyFindScaledShapeModelVM.NumMatchCol = hv_Column.D;
                            MyFindScaledShapeModelVM.NumMatchAngle = hv_Angle.D;
                            MyFindScaledShapeModelVM.NumMatchScore = hv_Score.D;
                            MyFindScaledShapeModelVM.NumMatchScale = hv_Scale.D;
                            PrintLog("模板匹配完成", EnumLogType.Info);
                        }
                        else
                        {
                            PrintLog("未搜索到模板", EnumLogType.Warning);
                        }
                        DrawingCanvas.Strokes.Clear();
                        DrawingCanvas.Children.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 确定模板
        /// </summary>
        public RelayCommand CmdConfirmModule => new Lazy<RelayCommand>(() => new RelayCommand(ConfirmModule)).Value;
        private void ConfirmModule()
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

                HOperatorSet.SetSystem("border_shape_models", "false");
                // 从涂层里重新制作模板
                HOperatorSet.GenEmptyObj(out HObject ho_RegionROI);
                HOperatorSet.GenEmptyObj(out HObject ho_Region);
                ho_RegionROI.Dispose();
                for (int m = 0; m < DrawingCanvas.Strokes.Count; m++)
                {
                    // 坐标转换：控件 → 图像
                    PointCollection points = Halcon.ControlPointToHImagePoint(DrawingCanvas.Strokes[m].StylusPoints);
                    HTuple hv_rows = new HTuple();
                    HTuple hv_cols = new HTuple();
                    hv_rows.Dispose();
                    hv_cols.Dispose();
                    for (int i = 0; i < points.Count; i++)
                    {
                        hv_rows[i] = points[i].Y;
                        hv_cols[i] = points[i].X;
                    }
                    ho_Region.Dispose();
                    HOperatorSet.GenRegionPolygonFilled(out ho_Region, hv_rows, hv_cols);
                    hv_rows.Dispose();
                    hv_cols.Dispose();
                    if (!ho_RegionROI.IsInitialized())
                    {
                        HOperatorSet.Union1(ho_Region, out ho_RegionROI);
                    }
                    else
                    {
                        HOperatorSet.Union2(ho_RegionROI, ho_Region, out ho_RegionROI);
                    }
                }
                // 抠图
                if (ho_RegionROI.IsInitialized())
                {
                    HOperatorSet.ReduceDomain(Ho_Image, ho_RegionROI, out HObject ho_ImageROI);
                    ho_RegionROI.Dispose();

                    // 创建形状模板：缩放
                    PrintLog("创建形状模板", EnumLogType.Debug);
                    Hv_ShapeModelID.Dispose();
                    HOperatorSet.CreateScaledShapeModel(ho_ImageROI, MyCreateScaledShapeModelVM.StrSelectNumLevels, MyCreateScaledShapeModelVM.NumSelectAngleStart, MyCreateScaledShapeModelVM.NumSelectAngleExtent,
                        MyCreateScaledShapeModelVM.StrSelectAngleStep, MyCreateScaledShapeModelVM.NumScaleMin, MyCreateScaledShapeModelVM.NumScaleMax, MyCreateScaledShapeModelVM.StrSelectScaleStep,
                        MyCreateScaledShapeModelVM.StrSelectOptimization, MyCreateScaledShapeModelVM.StrSelectMetric, MyCreateScaledShapeModelVM.StrSelectContrast,
                        MyCreateScaledShapeModelVM.StrSelectMinContrast, out Hv_ShapeModelID);
                    // 寻找模板
                    PrintLog("寻找模板", EnumLogType.Debug);
                    HOperatorSet.FindScaledShapeModel(Ho_Image, Hv_ShapeModelID, MyFindScaledShapeModelVM.NumSelectAngleStart, MyFindScaledShapeModelVM.NumSelectAngleExtent,
                        MyFindScaledShapeModelVM.NumScaleMin, MyFindScaledShapeModelVM.NumScaleMax, MyFindScaledShapeModelVM.NumMinScore, 1,
                    MyFindScaledShapeModelVM.NumMaxOverlap, MyFindScaledShapeModelVM.StrSelectSubPixel, MyFindScaledShapeModelVM.IntNumLevels, MyFindScaledShapeModelVM.NumGreediness,
                    out HTuple hv_Rows, out HTuple hv_Columns, out HTuple hv_Angles, out HTuple hv_Scales, out HTuple hv_Scores);

                    if (hv_Scores.Length > 0)
                    {
                        Ho_Window.ClearWindow();
                        Ho_Window.DispObj(Ho_Image);
                        Ho_Window.SetColor(EnumHalColor.magenta.ToColorString());
                        Ho_Window.SetLineWidth(3);
                        // 获取模板轮廓
                        HOperatorSet.GetShapeModelContours(out HObject ho_ShapeModel, Hv_ShapeModelID, 1);
                        for (int i = 0; i < hv_Scores.Length; i++)
                        {
                            // 显示模板：单位矩阵 → 平移 → 旋转 → 缩放
                            HOperatorSet.HomMat2dIdentity(out HTuple hv_HomMat2DIdentity);
                            HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Rows[i].D, hv_Columns[i].D, out HTuple hv_HomMat2DTranslate);
                            HOperatorSet.HomMat2dRotate(hv_HomMat2DTranslate, hv_Angles[i].D, hv_Rows[i].D, hv_Columns[i].D, out HTuple hv_HomMat2DRotate);
                            HOperatorSet.HomMat2dScale(hv_HomMat2DRotate, hv_Scales[i].D, hv_Scales[i].D, hv_Rows[i].D, hv_Columns[i].D, out HTuple hv_HomMat2DScale);
                            hv_HomMat2DIdentity.Dispose();
                            hv_HomMat2DTranslate.Dispose();
                            hv_HomMat2DRotate.Dispose();

                            //HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Rows[i].D, hv_Columns[i].D, hv_Angles[i].D, out HTuple hv_HomMat2DRotate);
                            HOperatorSet.AffineTransContourXld(ho_ShapeModel, out HObject ho_ModelTrans, hv_HomMat2DScale);
                            Ho_Window.DispObj(ho_ModelTrans);
                            ho_ModelTrans.Dispose();
                            hv_HomMat2DScale.Dispose();
                            MyFindScaledShapeModelVM.NumMatchRow = hv_Rows[i].D;
                            MyFindScaledShapeModelVM.NumMatchCol = hv_Columns[i].D;
                            MyFindScaledShapeModelVM.NumMatchAngle = hv_Angles[i].D * 180 / Math.PI;
                            MyFindScaledShapeModelVM.NumMatchScore = hv_Scores[i].D;
                            MyFindScaledShapeModelVM.NumMatchScale = hv_Scales[i].D;
                        }
                        ho_ShapeModel.Dispose();
                        PrintLog("模板创建完成", EnumLogType.Success);
                    }
                    else
                    {
                        PrintLog("未搜索到模板", EnumLogType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 模板匹配
        /// </summary>
        public RelayCommand CmdMatchModule => new Lazy<RelayCommand>(() => new RelayCommand(MatchModule)).Value;
        private void MatchModule()
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

                if (Hv_ShapeModelID.ToString().Length < 5)
                {
                    PrintLog("形状模板无效", EnumLogType.Warning);
                    return;
                }
                // 全图匹配
                PrintLog("模板匹配", EnumLogType.Debug);
                HOperatorSet.FindScaledShapeModel(Ho_Image, Hv_ShapeModelID, MyFindScaledShapeModelVM.NumSelectAngleStart, MyFindScaledShapeModelVM.NumSelectAngleExtent,
                    MyFindScaledShapeModelVM.NumScaleMin, MyFindScaledShapeModelVM.NumScaleMax, MyFindScaledShapeModelVM.NumMinScore, MyFindScaledShapeModelVM.IntNumMatches,
                MyFindScaledShapeModelVM.NumMaxOverlap, MyFindScaledShapeModelVM.StrSelectSubPixel, MyFindScaledShapeModelVM.IntNumLevels, MyFindScaledShapeModelVM.NumGreediness,
                out HTuple hv_Rows, out HTuple hv_Columns, out HTuple hv_Angles, out HTuple hv_Scales, out HTuple hv_Scores);

                if (hv_Angles.Length > 0)
                {
                    DrawingCanvas.Strokes.Clear();
                    DrawingCanvas.Children.Clear();
                    Ho_Window.DispObj(Ho_Image);
                    Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                    Ho_Window.SetLineWidth(2);
                    // 获取模板轮廓
                    HOperatorSet.GetShapeModelContours(out HObject ho_ShapeModel, Hv_ShapeModelID, 1);
                    for (int i = 0; i < hv_Scores.Length; i++)
                    {
                        // 显示模板：单位矩阵 → 平移 → 旋转 → 缩放
                        HOperatorSet.HomMat2dIdentity(out HTuple hv_HomMat2DIdentity);
                        HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Rows[i].D, hv_Columns[i].D, out HTuple hv_HomMat2DTranslate);
                        HOperatorSet.HomMat2dRotate(hv_HomMat2DTranslate, hv_Angles[i].D, hv_Rows[i].D, hv_Columns[i].D, out HTuple hv_HomMat2DRotate);
                        HOperatorSet.HomMat2dScale(hv_HomMat2DRotate, hv_Scales[i].D, hv_Scales[i].D, hv_Rows[i].D, hv_Columns[i].D, out HTuple hv_HomMat2DScale);
                        hv_HomMat2DIdentity.Dispose();
                        hv_HomMat2DTranslate.Dispose();
                        hv_HomMat2DRotate.Dispose();

                        HOperatorSet.AffineTransContourXld(ho_ShapeModel, out HObject ho_ModelTrans, hv_HomMat2DScale);
                        Ho_Window.DispObj(ho_ModelTrans);
                        ho_ModelTrans.Dispose();
                        hv_HomMat2DScale.Dispose();
                        MyFindScaledShapeModelVM.NumMatchRow = hv_Rows[i].D;
                        MyFindScaledShapeModelVM.NumMatchCol = hv_Columns[i].D;
                        MyFindScaledShapeModelVM.NumMatchAngle = hv_Angles[i].D * 180 / Math.PI;
                        MyFindScaledShapeModelVM.NumMatchScore = hv_Scores[i].D;
                        MyFindScaledShapeModelVM.NumMatchScale = hv_Scales[i].D;
                    }
                    ho_ShapeModel.Dispose();
                    PrintLog("模板匹配完成", EnumLogType.Info);
                }
                else
                {
                    PrintLog("未找到模板", EnumLogType.Warning);
                }
            }
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }
        #endregion

        #region 4. 内部方法
        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanMove = false;

            // 创建模板
            if (ModuleEditState == EnumModuleEditState.choose_module)
            {
                // 创建涂层 最后一个 Stroke
                int count = DrawingCanvas.Strokes.Count;
                if (count > 0)
                {
                    StylusPointCollection sps = DrawingCanvas.Strokes[count - 1].StylusPoints;
                    // 最少三个点
                    if (sps.Count > 2)
                    {
                        Polygon polygon = new Polygon()
                        {
                            Stroke = CConstants.BrushDefault,
                            StrokeThickness = 1,
                            Points = sps.StylusPointsConverter(),
                            Fill = CConstants.BrushMask,
                        };
                        DrawingCanvas.Children.Add(polygon);
                    }
                    else
                    {
                        DrawingCanvas.Strokes.RemoveAt(count - 1);
                    }
                }
            }
        }
        private void DrawingBorder_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            Point curPoint = e.GetPosition(e.Device.Target);
            //////////////////////////////////////// 当前点信息
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

            // 创建模板 可以多个 Stroke
            if (ModuleEditState == EnumModuleEditState.choose_module && CanMove)
            {
                // 重绘调整笔迹宽度
                //int count = DrawingCanvas.Strokes.Count;
                //StylusPointCollection sps = DrawingCanvas.Strokes[count - 1].StylusPoints.Clone();
                //DrawingCanvas.Strokes.RemoveAt(count - 1);
                //sps.Add(new StylusPoint(curPoint.X, curPoint.Y));
                //DrawingCanvas.Strokes[count - 1].StylusPoints.Add(new StylusPoint(curPoint.X, curPoint.Y));
                //InkStrokeThickness = InkMethod.InkStrokeThicknessDefault * Math.Pow(Halcon.HZoomFactor, WheelScrollValue);
                //DrawingCanvas.Strokes.Add(InkMethod.CreateLineMask(sps));

                // 不用重绘 框选范围
                int count = DrawingCanvas.Strokes.Count;
                // 避免取点密集
                if (InkMethod.GetDistancePP(PointMoveOri, curPoint) > 5)
                {
                    DrawingCanvas.Strokes[count - 1].StylusPoints.Add(new StylusPoint(curPoint.X, curPoint.Y));
                    PointMoveOri = curPoint;
                }
                return;
            }

            //////////////////////////////////////// 编辑 ROI
            if (CanEdit && DrawingCanvas.Children.Count == 0)
            {
                if (DrawingCanvas.Strokes.Count < 1)
                {
                    return;
                }

                if (ModuleEditState != EnumModuleEditState.edit_roi)
                {
                    // 会卡顿 只判断一次 不同位置对应不同鼠标形状
                    ModuleEditType = DrawingCanvas.Strokes[0].GetModuleEditType(curPoint, EnumModuleSel);
                }

                ModuleEditState = EnumModuleEditState.edit_roi;
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
                    ModuleEditState = EnumModuleEditState.none;
                    DrawingBorder.Cursor = Cursors.Arrow;
                }

                // 编辑
                if (CanMove && ModuleEditState == EnumModuleEditState.edit_roi)
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
                    }
                    // 大小
                    else
                    {
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints;
                        // 区分模板
                        if (EnumModuleSel == EnumModuleType.rectangle || EnumModuleSel == EnumModuleType.ellipse)
                        {
                            Point p0 = new Point(0.5 * (sps[0].X + sps[4].X), 0.5 * (sps[0].Y + sps[4].Y));
                            if (InkMethod.GetDistancePP(curPoint, p0) < 5)
                            {
                                return;
                            }
                            Point pt = new Point(curPoint.X, curPoint.Y);
                            // 用 3、5 两个点计算倾角 假设 +x 方向有个点
                            Point px = new Point(DrawingCanvas.Strokes[0].GetBounds().Right + DrawingCanvas.Strokes[0].GetBounds().Width, sps[2].Y);
                            double angle = InkMethod.GetPointAngle((Point)sps[2], (Point)sps[4], px);
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
                                double ratio = InkMethod.GetDistancePP(pt, (Point)sps[idx]) / InkMethod.GetDistancePP(PointMoveOri, (Point)sps[idx]);
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
                            double radius = InkMethod.GetDistancePP(p0, p1);
                            double r = InkMethod.GetDistancePP(p0, curPoint);
                            Matrix matrixResize = new Matrix();
                            matrixResize.ScaleAt(r / radius, r / radius, p0.X, p0.Y);
                            DrawingCanvas.Strokes[0].Transform(matrixResize, false);
                        }
                        else if (EnumModuleSel == EnumModuleType.polygon)
                        {
                            // 找到选中点 鼠标滑动点距离最近的点
                            double r0 = InkMethod.GetDistancePP((Point)sps[0], PointMoveOri);
                            int index = 0;
                            for (int i = 1; i < sps.Count; i++)
                            {
                                double r = InkMethod.GetDistancePP((Point)sps[i], PointMoveOri);
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
                    ModuleEditState = EnumModuleEditState.none;
                    ModuleEditType = EnumModuleEditType.None;
                }
            }

            //////////////////////////////////////// 创建 ROI
            if (ModuleEditState == EnumModuleEditState.choose_roi)
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
                        if (count > 1 && InkMethod.GetDistancePP(PointMoveOri, curPoint) > 1e-6)
                        {
                            sps.RemoveAt(count - 1);
                        }
                        sps.Add(new StylusPoint(curPoint.X, curPoint.Y));

                        // 重绘
                        DrawingCanvas.Strokes.Clear();
                        DrawingCanvas.Children.Clear();
                        DrawingCanvas.Strokes.Add(InkMethod.CreatePolyline(sps));
                    }
                    return;
                }

                if (!CanMove)
                {
                    return;
                }

                DrawingCanvas.Strokes.Clear();
                DrawingCanvas.Children.Clear();
                if (EnumModuleSel == EnumModuleType.rectangle)
                {
                    DrawingCanvas.Strokes.Add(InkMethod.CreateRectangle(PointMoveOri, curPoint));
                }
                else if (EnumModuleSel == EnumModuleType.ellipse)
                {
                    DrawingCanvas.Strokes.Add(InkMethod.CreateEllipse(PointMoveOri, curPoint));
                }
                else if (EnumModuleSel == EnumModuleType.circle)
                {
                    DrawingCanvas.Strokes.Add(InkMethod.CreateCircle(PointMoveOri, curPoint));
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
                // 刷新涂层
                RefreshMask();
                // 更新起点
                PointMoveOri = curPoint;
            }
        }
        private void DrawingBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanMove = true;
            PointMoveOri = e.GetPosition(e.Device.Target);

            // 双击 图像还原
            if (e.ClickCount == 2)
            {
                Halcon.SetFullImagePart();
                // 重置
                SetDefault(false);
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
            else if (e.ClickCount == 1)
            {
                // 创建 PolyLine 添加点 在 MouseMove 中绘制实时效果 
                if (ModuleEditState == EnumModuleEditState.choose_roi && EnumModuleSel == EnumModuleType.polygon)
                {
                    if (DrawingCanvas.Strokes.Count == 0)
                    {
                        DrawingCanvas.Strokes.Add(InkMethod.CreatePolyline(new StylusPointCollection { new StylusPoint(PointMoveOri.X, PointMoveOri.Y) }));
                    }
                    else
                    {
                        DrawingCanvas.Strokes[0].StylusPoints.Add(new StylusPoint(PointMoveOri.X, PointMoveOri.Y));
                    }
                }

                // 创建模板状态
                else if (ModuleEditState == EnumModuleEditState.choose_module)
                {
                    // 每次点击创建一个 Stroke
                    //InkStrokeThickness = InkMethod.InkStrokeThickness * Math.Pow(Halcon.HZoomFactor, WheelScrollValue);
                    DrawingCanvas.Strokes.Add(InkMethod.CreateLineMask(new StylusPointCollection { new StylusPoint(PointMoveOri.X, PointMoveOri.Y) }));
                }
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
                // 缩放时笔迹宽度跟随
                InkStrokeThickness *= Halcon.HZoomFactor;
            }
            else
            {
                matrix.ScaleAt(1 / Halcon.HZoomFactor, 1 / Halcon.HZoomFactor, curPoint.X, curPoint.Y);
                Halcon.HZoomWindowContents(curPoint.X, curPoint.Y, -1);
                // 缩放时笔迹宽度跟随
                InkStrokeThickness /= Halcon.HZoomFactor;
            }
            DrawingCanvas.Strokes.Transform(matrix, false);

            // 重绘涂层
            RefreshMask();
        }
        private void DrawingBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            // 双击 锁定模板
            if (e.ClickCount > 1)
            {
                CanEdit = false;
                ModuleEditState = EnumModuleEditState.none;
                DrawingBorder.Cursor = Cursors.Arrow;
            }
            else
            {
                // 多边形比较特殊 右键完成制作
                if (ModuleEditState == EnumModuleEditState.choose_roi && EnumModuleSel == EnumModuleType.polygon)
                {
                    // 制作过程中是 PolyLine
                    if (DrawingCanvas.Strokes.Count == 1 && DrawingCanvas.Strokes[0].GetType().Name.ToString() == "CustomPolyline")
                    {
                        StylusPointCollection sps = DrawingCanvas.Strokes[0].StylusPoints.Clone();
                        DrawingCanvas.Strokes.Clear();
                        DrawingCanvas.Children.Clear();
                        // 多边形至少 3 个点
                        if (sps.Count > 2)
                        {
                            DrawingCanvas.Strokes.Add(InkMethod.CreatePolygon(sps));
                        }
                    }
                }

                if (ModuleEditState == EnumModuleEditState.choose_module)
                {
                    ModuleEditState = EnumModuleEditState.none;
                    return;
                }
                // 可以编辑 ROI
                CanEdit = true;
                ModuleEditState = EnumModuleEditState.edit_roi;
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
                    MyImageChannelVM.TransHImage(ref Ho_Image);
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
        /// 重置状态
        /// </summary>
        private void SetDefault(bool isRefresh = true)
        {
            DrawingCanvas.Strokes.Clear();
            DrawingCanvas.Children.Clear();
            CanEdit = false;
            ModuleEditState = EnumModuleEditState.none;

            if (isRefresh)
            {
                Ho_Window.ClearWindow();
                if (Ho_Image.IsInitialized())
                {
                    Ho_Window.DispObj(Ho_Image);
                }
            }
        }

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
                            MyImageChannelVM.TransHImage(ref Ho_Image);
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
        /// 刷新涂层
        /// </summary>
        private void RefreshMask()
        {
            // 修改坐标 重绘会闪烁
            for (int i = 0; i < DrawingCanvas.Children.Count; i++)
            {
                Polygon polygon = DrawingCanvas.Children[i] as Polygon;
                // Stroke 和 Children 是一一对应的
                polygon.Points = DrawingCanvas.Strokes[i].StylusPoints.StylusPointsConverter();
            }
        }
        #endregion
    }
}