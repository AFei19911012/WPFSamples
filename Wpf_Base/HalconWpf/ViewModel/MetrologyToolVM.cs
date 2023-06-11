using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
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
using Wpf_Base.HalconWpf.Views;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.HalconWpf.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 17:57:30
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 17:57:30    CoderMan/CoderdMan1012         首次编写         
    ///
    public class MetrologyToolVM : ViewModelBase
    {
        #region 1. 绑定变量
        /// <summary>
        /// 模板匹配分数阈值
        /// </summary>
        private double numScoreMin = 0.85;
        public double NumScoreMin
        {
            get => numScoreMin;
            set => Set(ref numScoreMin, value);
        }


        /// <summary>
        /// 模板类型
        /// </summary>
        private Dictionary<EnumMetrologyObjectType, string> enumMetrologyObject;
        public Dictionary<EnumMetrologyObjectType, string> EnumMetrologyObject
        {
            get
            {
                Dictionary<EnumMetrologyObjectType, string> pairs = new Dictionary<EnumMetrologyObjectType, string>();
                foreach (EnumMetrologyObjectType item in Enum.GetValues(typeof(EnumMetrologyObjectType)))
                {
                    DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                    pairs.Add(item, attributes.Description);
                }
                return pairs;
            }

            set => Set(ref enumMetrologyObject, value);
        }

        private EnumMetrologyObjectType enumMetrologyObjectSel = EnumMetrologyObjectType.rectangle;
        public EnumMetrologyObjectType EnumMetrologyObjectSel
        {
            get => enumMetrologyObjectSel;
            set => Set(ref enumMetrologyObjectSel, value);
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


        /// <summary>
        /// 测量对象列表
        /// </summary>
        private ObservableCollection<CDataModel> listModels = new ObservableCollection<CDataModel>();
        public ObservableCollection<CDataModel> ListModels
        {
            get => listModels;
            set => Set(ref listModels, value);
        }

        /// <summary>
        /// 计算偏移量
        /// </summary>
        private double numOffsetX = 0;
        public double NumOffsetX
        {
            get => numOffsetX;
            set => Set(ref numOffsetX, value);
        }
        private double numOffsetY = 0;
        public double NumOffsetY
        {
            get => numOffsetY;
            set => Set(ref numOffsetY, value);
        }
        private double numOffsetA = 0;
        public double NumOffsetA
        {
            get => numOffsetA;
            set => Set(ref numOffsetA, value);
        }
        #endregion

        #region 2. 全局变量
        private HSmartWindowControlWPF Halcon { get; set; }
        private FindShapeModelVM MyFindShapeModelVM { get; set; }
        private MetrologyObjectVM MyMetrologyObjectVM { get; set; }
        private InkCanvas DrawingCanvas { get; set; }
        private Border DrawingBorder { get; set; }

        private ImageChannelVM MyImageChannelVM { get; set; }

        private HWindow Ho_Window { get; set; }
        private HObject Ho_Image = null;
        private bool IsFirstShow { get; set; } = true;


        /// <summary>
        /// 形状模板和测量模型
        /// </summary>
        private HTuple Hv_ShapeModelID = new HTuple();
        private List<CMetrologyObjectMeasureParams> ListMetrologyObjects { get; set; } = new List<CMetrologyObjectMeasureParams>();
        private HTuple Hv_MetrologyHandle = new HTuple();


        /// <summary>
        /// 标定结果
        /// </summary>
        private HTuple Hv_HomMat2D { get; set; } = new HTuple();
        private double RotateCenterX { get; set; }
        private double RotateCenterY { get; set; }


        /// <summary>
        /// 参考点
        /// </summary>
        private CDataModel CalibrationReference { get; set; } = new CDataModel();
        private CDataModel MetrologyReference { get; set; } = new CDataModel();


        /// <summary>
        /// 绘制模板 编辑
        /// </summary>
        private Point PointMoveOri { get; set; } = new Point();
        private bool CanMove { get; set; } = false;
        private EnumModuleEditType ModuleEditType { get; set; } = EnumModuleEditType.None;
        private bool IsMakingModule { get; set; } = false;
        private bool IsEditingModule { get; set; } = false;
        private bool CanEditModule { get; set; } = false;

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


        public MetrologyToolVM()
        {

        }

        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as MetrologyTool).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            MyFindShapeModelVM = (e.Source as MetrologyTool).MyFindShapeModelControl.DataContext as FindShapeModelVM;
            MyMetrologyObjectVM = (e.Source as MetrologyTool).MyMetrologyObjectControl.DataContext as MetrologyObjectVM;
            DrawingCanvas = (e.Source as MetrologyTool).DrawingCanvas;
            DrawingBorder = (e.Source as MetrologyTool).DrawingBorder;
            MyImageChannelVM = (e.Source as MetrologyTool).MyImageChannelControl.DataContext as ImageChannelVM;
            MyPixelValueControl = (e.Source as MetrologyTool).MyPixelValueControl;

            if (Ho_Image == null)
            {
                HOperatorSet.GenEmptyObj(out Ho_Image);
            }

            // 鼠标事件
            DrawingBorder.MouseWheel += DrawingBorder_MouseWheel;
            DrawingBorder.MouseLeftButtonDown += DrawingBorder_MouseLeftButtonDown;
            DrawingBorder.MouseMove += DrawingBorder_MouseMove;
            DrawingBorder.MouseLeftButtonUp += DrawingBorder_MouseLeftButtonUp;
            DrawingBorder.MouseRightButtonDown += DrawingBorder_MouseRightButtonDown;
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
                    MyImageChannelVM.TransHImage(ref Ho_Image);
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
        /// 加载标定文件
        /// </summary>
        public RelayCommand CmdLoadCalibration => new Lazy<RelayCommand>(() => new RelayCommand(LoadCalibration)).Value;
        private void LoadCalibration()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "加载标定文件",
                Filter = "标定文件(*.xml)|*.xml",
                RestoreDirectory = true,
                FileName = "Calibration_" + StrRecipeName + ".xml",
                InitialDirectory = Environment.CurrentDirectory + "\\Config",
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            string filename = dialog.FileName;
            CDataModel model = new CDataModel();
            _ = HalconIoMethod.LoadCalibration(ref model, filename);
            Hv_HomMat2D.Dispose();
            Hv_HomMat2D[0] = model.A1;
            Hv_HomMat2D[1] = model.B1;
            Hv_HomMat2D[2] = model.C1;
            Hv_HomMat2D[3] = model.A2;
            Hv_HomMat2D[4] = model.B2;
            Hv_HomMat2D[5] = model.C2;
            RotateCenterX = model.CenterX;
            RotateCenterY = model.CenterY;
            PrintLog("标定文件加载完成：" + filename, EnumLogType.Success);
        }

        /// <summary>
        /// 加载形状模板
        /// </summary>
        public RelayCommand CmdLoadShapeModule => new Lazy<RelayCommand>(() => new RelayCommand(LoadShapeModule)).Value;
        private void LoadShapeModule()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "加载模板",
                Filter = "模板文件(*.shm)|*.shm",
                RestoreDirectory = true,
                FileName = HalconIoMethod.GenShapeModelName(StrRecipeName),
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

        /// <summary>
        /// 新增测量对象
        /// </summary>
        public RelayCommand CmdNewObject => new Lazy<RelayCommand>(() => new RelayCommand(NewObject)).Value;
        private void NewObject()
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

        /// <summary>
        /// 确定测量对象
        /// </summary>
        public RelayCommand CmdConfirmObject => new Lazy<RelayCommand>(() => new RelayCommand(ConfirmObject)).Value;
        private void ConfirmObject()
        {
            try
            {
                if (DrawingCanvas.Strokes.Count != 1)
                {
                    return;
                }
                string modelName = DrawingCanvas.Strokes[0].GetType().Name.ToString();
                // 坐标转换：控件 → 图像
                PointCollection points = Halcon.ControlPointToHImagePoint(DrawingCanvas.Strokes[0].StylusPoints);
                if (modelName == "CustomRectangle" || modelName == "CustomEllipse")
                {
                    if (modelName == "CustomRectangle")
                    {
                        ListModels.Add(new CDataModel { Name = "矩形对象" });
                    }
                    else
                    {
                        ListModels.Add(new CDataModel { Name = "椭圆对象" });
                    }
                    // 旋转角度
                    Point px = new Point(points[4].X + InkMethod.GetDistancePP(points[2], points[4]), points[2].Y);
                    // 旋转角度（弧度）
                    double angle = InkMethod.GetPointAngle(points[2], points[4], px) * Math.PI / 180;
                    if (points[4].Y > points[2].Y)
                    {
                        angle = -angle;
                    }
                    // 宽高
                    double len1 = 0.5 * InkMethod.GetDistancePP(points[2], points[4]);
                    double len2 = 0.5 * InkMethod.GetDistancePP(points[0], points[2]);
                    // 中心点
                    double row = 0.5 * (points[0].Y + points[4].Y);
                    double col = 0.5 * (points[0].X + points[4].X);
                    CMetrologyObjectMeasureParams param = new CMetrologyObjectMeasureParams
                    {
                        Row = row,
                        Column = col,
                        Phi = angle,
                        Length1 = len1,
                        Length2 = len2,
                        MeasureLength1 = MyMetrologyObjectVM.NumLength1,
                        MeasureLength2 = MyMetrologyObjectVM.NumLength2,
                        MeasureSigma = MyMetrologyObjectVM.NumSigma,
                        MeasureThreshold = MyMetrologyObjectVM.NumThreshold,
                    };
                    param.Type = modelName == "CustomRectangle" ? EnumMetrologyObjectType.rectangle : EnumMetrologyObjectType.ellipse;
                    ListMetrologyObjects.Add(param);
                }
                else if (modelName == "CustomCircle")
                {
                    ListModels.Add(new CDataModel { Name = "圆对象" });
                    double radius = InkMethod.GetDistancePP(points[0], points[1]);
                    CMetrologyObjectMeasureParams param = new CMetrologyObjectMeasureParams
                    {
                        Type = EnumMetrologyObjectType.circle,
                        Row = points[0].Y,
                        Column = points[0].X,
                        Radius = radius,
                        MeasureLength1 = MyMetrologyObjectVM.NumLength1,
                        MeasureLength2 = MyMetrologyObjectVM.NumLength2,
                        MeasureSigma = MyMetrologyObjectVM.NumSigma,
                        MeasureThreshold = MyMetrologyObjectVM.NumThreshold,
                    };
                    ListMetrologyObjects.Add(param);
                }
                else if (modelName == "CustomLine")
                {
                    ListModels.Add(new CDataModel { Name = "直线对象" });
                    CMetrologyObjectMeasureParams param = new CMetrologyObjectMeasureParams
                    {
                        Type = EnumMetrologyObjectType.line,
                        RowBegin = points[0].Y,
                        ColumnBegin = points[0].X,
                        RowEnd = points[1].Y,
                        ColumnEnd = points[1].X,
                        MeasureLength1 = MyMetrologyObjectVM.NumLength1,
                        MeasureLength2 = MyMetrologyObjectVM.NumLength2,
                        MeasureSigma = MyMetrologyObjectVM.NumSigma,
                        MeasureThreshold = MyMetrologyObjectVM.NumThreshold,
                    };
                    ListMetrologyObjects.Add(param);
                }

                CanEditModule = false;
                DrawingBorder.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                PrintLog("确定测量对象异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 删除测量对象
        /// </summary>
        public RelayCommand CmdDeleteObject => new Lazy<RelayCommand>(() => new RelayCommand(DeleteObject)).Value;
        private void DeleteObject()
        {
            ListModels.Clear();
            ListMetrologyObjects.Clear();
            SetDefault();
        }

        /// <summary>
        /// 显示测量对象
        /// </summary>
        public RelayCommand CmdShowObject => new Lazy<RelayCommand>(() => new RelayCommand(ShowObject)).Value;
        private void ShowObject()
        {
            Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
            Ho_Window.SetLineWidth(2);
            SetDefault();
            for (int i = 0; i < ListMetrologyObjects.Count; i++)
            {
                EnumMetrologyObjectType type = ListMetrologyObjects[i].Type;
                if (type == EnumMetrologyObjectType.rectangle)
                {
                    double row = ListMetrologyObjects[i].Row;
                    double col = ListMetrologyObjects[i].Column;
                    double len1 = ListMetrologyObjects[i].Length1;
                    double len2 = ListMetrologyObjects[i].Length2;
                    double phi = ListMetrologyObjects[i].Phi;
                    Ho_Window.DispRectangleContour(row, col, len1, len2, phi);
                }
                else if (type == EnumMetrologyObjectType.ellipse)
                {
                    double row = ListMetrologyObjects[i].Row;
                    double col = ListMetrologyObjects[i].Column;
                    double len1 = ListMetrologyObjects[i].Length1;
                    double len2 = ListMetrologyObjects[i].Length2;
                    double phi = ListMetrologyObjects[i].Phi;
                    Ho_Window.DispEllipse2Contour(row, col, len1, len2, phi);
                }
                else if (type == EnumMetrologyObjectType.circle)
                {
                    double row = ListMetrologyObjects[i].Row;
                    double col = ListMetrologyObjects[i].Column;
                    double r = ListMetrologyObjects[i].Radius;
                    Ho_Window.DispCircleContour(row, col, r);
                }
                else if (type == EnumMetrologyObjectType.line)
                {
                    double row1 = ListMetrologyObjects[i].RowBegin;
                    double col1 = ListMetrologyObjects[i].ColumnBegin;
                    double row2 = ListMetrologyObjects[i].RowEnd;
                    double col2 = ListMetrologyObjects[i].ColumnEnd;
                    Ho_Window.DispArrowContour(row1, col1, row2, col2);
                }
            }
        }

        /// <summary>
        /// 创建模型
        /// </summary>
        public RelayCommand CmdCreateModel => new Lazy<RelayCommand>(() => new RelayCommand(CreateModel)).Value;
        private void CreateModel()
        {
            try
            {
                if (CanRefreshImage)
                {
                    GetCurrentImage();
                }

                DrawingCanvas.Strokes.Clear();
                if (!Ho_Image.IsInitialized())
                {
                    PrintLog("图像无效", EnumLogType.Warning);
                    return;
                }
                DrawingCanvas.Strokes.Clear();
                if (Hv_ShapeModelID.ToString().Length < 5)
                {
                    PrintLog("未加载形状模板", EnumLogType.Warning);
                    return;
                }
                if (ListMetrologyObjects.Count < 1)
                {
                    PrintLog("未创建对象", EnumLogType.Warning);
                    return;
                }

                #region 通用部分
                /// 1. 模板匹配确定测量模型的参考点和参考角度 一般角度为 0
                HOperatorSet.SetSystem("border_shape_models", "false");
                // 获取模板轮廓
                HOperatorSet.GetShapeModelContours(out HObject ho_ShapeModel, Hv_ShapeModelID, 1);
                // 全图匹配
                HOperatorSet.FindShapeModel(Ho_Image, Hv_ShapeModelID, MyFindShapeModelVM.NumSelectAngleStart, MyFindShapeModelVM.NumSelectAngleExtent,
                        MyFindShapeModelVM.NumMinScore, 1, MyFindShapeModelVM.NumMaxOverlap, MyFindShapeModelVM.StrSelectSubPixel,
                        MyFindShapeModelVM.IntNumLevels, MyFindShapeModelVM.NumGreediness, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle, out HTuple hv_Score);
                if (hv_Score.Length < 1)
                {
                    PrintLog("模板匹配失败", EnumLogType.Error);
                    return;
                }
                // 绘制匹配结果
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out HTuple hv_HomMat2DRotate);
                HOperatorSet.AffineTransContourXld(ho_ShapeModel, out HObject ho_ModelTrans, hv_HomMat2DRotate);
                ho_ShapeModel.Dispose();
                hv_HomMat2DRotate.Dispose();
                Ho_Window.ClearWindow();
                Ho_Window.DispObj(Ho_Image);
                Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                Ho_Window.SetLineWidth(3);
                Ho_Window.DispObj(ho_ModelTrans);
                ho_ModelTrans.Dispose();
                // 参考点
                MetrologyReference.RowRef = hv_Row.D;
                MetrologyReference.ColRef = hv_Column.D;
                MetrologyReference.AngRef = hv_Angle.D;

                // 结果显示到界面
                MyFindShapeModelVM.NumMatchRow = hv_Row.D;
                MyFindShapeModelVM.NumMatchCol = hv_Column.D;
                MyFindShapeModelVM.NumMatchAngle = hv_Angle.D * 180 / Math.PI;
                MyFindShapeModelVM.NumMatchScore = hv_Score.D;
                if (Hv_HomMat2D.Length == 6)
                {
                    HOperatorSet.AffineTransPoint2d(Hv_HomMat2D, hv_Column.D, hv_Row.D, out HTuple hv_Qx, out HTuple hv_Qy);
                    MyFindShapeModelVM.NumMatchRow = hv_Qy.D;
                    MyFindShapeModelVM.NumMatchCol = hv_Qx.D;
                }

                /// 2. 创建测量模型
                Hv_MetrologyHandle.Dispose();
                HOperatorSet.CreateMetrologyModel(out Hv_MetrologyHandle);
                // 设置模型对象图像大小
                HOperatorSet.GetImageSize(Ho_Image, out HTuple width, out HTuple height);
                HOperatorSet.SetMetrologyModelImageSize(Hv_MetrologyHandle, width, height);
                // 按顺序添加测量模型
                // 所有结果
                int totalObject = ListMetrologyObjects.Count;
                HTuple[] hv_Indexs = new HTuple[totalObject];
                HTuple[] hv_Results = new HTuple[totalObject];
                for (int i = 0; i < totalObject; i++)
                {
                    EnumMetrologyObjectType type = ListMetrologyObjects[i].Type;
                    if (type == EnumMetrologyObjectType.rectangle)
                    {
                        double row = ListMetrologyObjects[i].Row;
                        double col = ListMetrologyObjects[i].Column;
                        double len1 = ListMetrologyObjects[i].Length1;
                        double len2 = ListMetrologyObjects[i].Length2;
                        double phi = ListMetrologyObjects[i].Phi;
                        double mlen1 = ListMetrologyObjects[i].MeasureLength1;
                        double mlen2 = ListMetrologyObjects[i].MeasureLength2;
                        double msigma = ListMetrologyObjects[i].MeasureSigma;
                        double mthreshold = ListMetrologyObjects[i].MeasureThreshold;
                        HOperatorSet.AddMetrologyObjectRectangle2Measure(Hv_MetrologyHandle, row, col, phi, len1, len2, mlen1, mlen2, msigma, mthreshold, new HTuple(), new HTuple(), out hv_Indexs[i]);
                    }
                    else if (type == EnumMetrologyObjectType.ellipse)
                    {
                        double row = ListMetrologyObjects[i].Row;
                        double col = ListMetrologyObjects[i].Column;
                        double len1 = ListMetrologyObjects[i].Length1;
                        double len2 = ListMetrologyObjects[i].Length2;
                        double phi = ListMetrologyObjects[i].Phi;
                        double mlen1 = ListMetrologyObjects[i].MeasureLength1;
                        double mlen2 = ListMetrologyObjects[i].MeasureLength2;
                        double msigma = ListMetrologyObjects[i].MeasureSigma;
                        double mthreshold = ListMetrologyObjects[i].MeasureThreshold;
                        HOperatorSet.AddMetrologyObjectEllipseMeasure(Hv_MetrologyHandle, row, col, phi, len1, len2, mlen1, mlen2, msigma, mthreshold, new HTuple(), new HTuple(), out hv_Indexs[i]);
                    }
                    else if (type == EnumMetrologyObjectType.circle)
                    {
                        double row = ListMetrologyObjects[i].Row;
                        double col = ListMetrologyObjects[i].Column;
                        double r = ListMetrologyObjects[i].Radius;
                        double mlen1 = ListMetrologyObjects[i].MeasureLength1;
                        double mlen2 = ListMetrologyObjects[i].MeasureLength2;
                        double msigma = ListMetrologyObjects[i].MeasureSigma;
                        double mthreshold = ListMetrologyObjects[i].MeasureThreshold;
                        HOperatorSet.AddMetrologyObjectCircleMeasure(Hv_MetrologyHandle, row, col, r, mlen1, mlen2, msigma, mthreshold, new HTuple(), new HTuple(), out hv_Indexs[i]);
                    }
                    else if (type == EnumMetrologyObjectType.line)
                    {
                        double row1 = ListMetrologyObjects[i].RowBegin;
                        double col1 = ListMetrologyObjects[i].ColumnBegin;
                        double row2 = ListMetrologyObjects[i].RowEnd;
                        double col2 = ListMetrologyObjects[i].ColumnEnd;
                        double mlen1 = ListMetrologyObjects[i].MeasureLength1;
                        double mlen2 = ListMetrologyObjects[i].MeasureLength2;
                        double msigma = ListMetrologyObjects[i].MeasureSigma;
                        double mthreshold = ListMetrologyObjects[i].MeasureThreshold;
                        HOperatorSet.AddMetrologyObjectLineMeasure(Hv_MetrologyHandle, row1, col1, row2, col2, mlen1, mlen2, msigma, mthreshold, new HTuple(), new HTuple(), out hv_Indexs[i]);
                    }
                }

                // 设置对象参数 统一设置、单独对某个对象设置
                // 遍历属性和值
                CMetrologyObjectParam metrologyParam = new CMetrologyObjectParam(MyMetrologyObjectVM);
                PropertyInfo[] properties = metrologyParam.GetType().GetProperties();
                foreach (PropertyInfo item in properties)
                {
                    string key = item.Name;
                    HTuple value = (HTuple)item.GetValue(metrologyParam, null);
                    HOperatorSet.SetMetrologyObjectParam(Hv_MetrologyHandle, "all", key, value);
                }

                // 设置模型参数
                HTuple ref_sys = new HTuple();
                ref_sys[0] = MetrologyReference.RowRef;
                ref_sys[1] = MetrologyReference.ColRef;
                ref_sys[2] = MetrologyReference.AngRef;
                HOperatorSet.SetMetrologyModelParam(Hv_MetrologyHandle, "reference_system", ref_sys);
                // 对齐测量模板
                HOperatorSet.AlignMetrologyModel(Hv_MetrologyHandle, ref_sys[0], ref_sys[1], ref_sys[2]);

                /// 3. 应用测量模型
                HOperatorSet.ApplyMetrologyModel(Ho_Image, Hv_MetrologyHandle);
                // 获取所有结果
                for (int i = 0; i < totalObject; i++)
                {
                    HOperatorSet.GetMetrologyObjectResult(Hv_MetrologyHandle, hv_Indexs[i], "all", "result_type", "all_param", out hv_Results[i]);
                    if (hv_Results[i].Length < 1)
                    {
                        PrintLog("未搜索到对象", EnumLogType.Error);
                        return;
                    }
                }
                // 获取检测到的轮廓
                for (int i = 0; i < totalObject; i++)
                {
                    HOperatorSet.GetMetrologyObjectResultContour(out HObject ho_Contour, Hv_MetrologyHandle, hv_Indexs[i], "all", 1.5);
                    Ho_Window.SetColor(EnumHalColor.spring_green.ToColorString());
                    Ho_Window.DispObj(ho_Contour);
                    ho_Contour.Dispose();
                }
                // 获取所有测量对象的轮廓
                HOperatorSet.GetMetrologyObjectMeasures(out HObject ho_Contours, Hv_MetrologyHandle, "all", "all", out hv_Row, out hv_Column);
                // 显示
                Ho_Window.SetColor(EnumHalColor.slate_blue.ToColorString());
                Ho_Window.SetLineWidth(1);
                Ho_Window.DispObj(ho_Contours);
                ho_Contours.Dispose();
                #endregion

                /// 4. 计算参考点和参考角度
                // 每种应用场景都不一样的 
                // 形状模板比较简单的情况可以使用模板匹配出来的中心点和角度
                // 通常采用具有方向性质的对象来确定角度，比如长一点的直线、两个中心的连线
                CalibrationReference.RowRef = hv_Row.D;
                CalibrationReference.ColRef = hv_Column.D;
                CalibrationReference.AngRef = hv_Angle.D;
                if (hv_Results[0].Length > 0)
                {
                    CalibrationReference.RowRef = hv_Results[0][0].D;
                    CalibrationReference.ColRef = hv_Results[0][1].D;
                    CalibrationReference.AngRef = hv_Angle.D;
                }
                // 绘制参考点
                Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                Ho_Window.SetLineWidth(3);
                Ho_Window.DispCrossContour(CalibrationReference.RowRef, CalibrationReference.ColRef, 50, hv_Angle.D);
                PrintLog("模型创建成功", EnumLogType.Success);
            }
            catch (Exception ex)
            {
                PrintLog("模型创建异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 保存模型
        /// </summary>
        public RelayCommand CmdSaveModel => new Lazy<RelayCommand>(() => new RelayCommand(SaveModel)).Value;
        private void SaveModel()
        {
            if (Hv_MetrologyHandle.ToString().Length < 5)
            {
                return;
            }
            string filename = HalconIoMethod.GenMetrologyReferName(StrRecipeName);
            CalibrationReference.SaveMetrology(filename);
            filename = HalconIoMethod.GenMetrologyModelName(StrRecipeName);
            HOperatorSet.WriteMetrologyModel(Hv_MetrologyHandle, filename);
            // 清空
            //HOperatorSet.ClearMetrologyModel(Hv_MetrologyHandle);
            PrintLog("模型保存完成：" + filename, EnumLogType.Success);
        }

        /// <summary>
        /// 加载模型
        /// </summary>
        public RelayCommand CmdLoadModel => new Lazy<RelayCommand>(() => new RelayCommand(LoadModel)).Value;
        private void LoadModel()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "加载测量模型",
                Filter = "模型文件(*.mem)|*.mem",
                RestoreDirectory = true,
                InitialDirectory = Environment.CurrentDirectory + "\\Module",
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            string filename = dialog.FileName;
            Hv_MetrologyHandle.Dispose();
            HOperatorSet.ReadMetrologyModel(filename, out Hv_MetrologyHandle);
            CDataModel model = new CDataModel();
            HalconIoMethod.LoadMetrology(ref model, filename.Replace(".mem", ".xml"));
            CalibrationReference = new CDataModel(model.RowRef, model.ColRef, model.AngRef);
            PrintLog("模型加载完成：" + filename, EnumLogType.Success);
        }

        /// <summary>
        /// 应用模型
        /// </summary>
        public RelayCommand CmdApplyModel => new Lazy<RelayCommand>(() => new RelayCommand(ApplyModel)).Value;
        private void ApplyModel()
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
                if (Hv_MetrologyHandle.ToString().Length < 5)
                {
                    PrintLog("测量模型无效", EnumLogType.Warning);
                    return;
                }
                //if (!Hv_ShapeModelID.TupleIsValidHandle() || !Hv_MetrologyHandle.TupleIsValidHandle())
                //{
                //    OnNoticed("测量模型无效", EnumLogType.Warning);
                //    return;
                //}

                PrintLog("应用测量模型", EnumLogType.Debug);
                /// 1. 模板匹配 匹配多个目标
                // 获取模板轮廓
                HOperatorSet.GetShapeModelContours(out HObject ho_ShapeModel, Hv_ShapeModelID, 1);
                // 全图匹配
                HOperatorSet.FindShapeModel(Ho_Image, Hv_ShapeModelID, MyFindShapeModelVM.NumSelectAngleStart, MyFindShapeModelVM.NumSelectAngleExtent,
                        MyFindShapeModelVM.NumMinScore, MyFindShapeModelVM.IntNumMatches, MyFindShapeModelVM.NumMaxOverlap, MyFindShapeModelVM.StrSelectSubPixel,
                        MyFindShapeModelVM.IntNumLevels, MyFindShapeModelVM.NumGreediness, out HTuple hv_Rows, out HTuple hv_Columns, out HTuple hv_Angles, out HTuple hv_Scores);
                // 遍历
                if (hv_Scores.Length < 1)
                {
                    PrintLog("未搜索到模板", EnumLogType.Warning);
                    return;
                }

                Ho_Window.ClearWindow();
                Ho_Window.DispObj(Ho_Image);
                Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                Ho_Window.SetLineWidth(3);
                for (int idx = 0; idx < hv_Scores.Length; idx++)
                {
                    // 绘制匹配结果
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Rows[idx], hv_Columns[idx], hv_Angles[idx], out HTuple hv_HomMat2D);
                    HOperatorSet.AffineTransContourXld(ho_ShapeModel, out HObject ho_ModelTrans, hv_HomMat2D);
                    hv_HomMat2D.Dispose();
                    Ho_Window.DispObj(ho_ModelTrans);
                    ho_ModelTrans.Dispose();

                    // 模板匹配结果显示到界面
                    MyFindShapeModelVM.NumMatchRow = hv_Rows[idx].D;
                    MyFindShapeModelVM.NumMatchCol = hv_Columns[idx].D;
                    MyFindShapeModelVM.NumMatchAngle = hv_Angles[idx].D * 180 / Math.PI;
                    MyFindShapeModelVM.NumMatchScore = hv_Scores[idx].D;
                    if (Hv_HomMat2D.Length == 6)
                    {
                        HOperatorSet.AffineTransPoint2d(Hv_HomMat2D, hv_Columns[idx].D, hv_Rows[idx].D, out HTuple hv_Qx, out HTuple hv_Qy);
                        MyFindShapeModelVM.NumMatchRow = hv_Qy.D;
                        MyFindShapeModelVM.NumMatchCol = hv_Qx.D;
                    }

                    /// 2. 应用测量模型
                    PrintLog(string.Format("匹配模板 {0} 测量对象", idx + 1), EnumLogType.Debug);
                    // 对齐测量模型
                    HOperatorSet.AlignMetrologyModel(Hv_MetrologyHandle, hv_Rows[idx], hv_Columns[idx], hv_Angles[idx]);
                    // 应用测量模型
                    HOperatorSet.ApplyMetrologyModel(Ho_Image, Hv_MetrologyHandle);
                    // 所有结果
                    int totalObject = ListMetrologyObjects.Count;
                    if (totalObject == 0)
                    {
                        totalObject = 1;
                    }
                    HTuple[] hv_Results = new HTuple[totalObject];
                    // 获取所有结果
                    for (int i = 0; i < totalObject; i++)
                    {
                        HOperatorSet.GetMetrologyObjectResult(Hv_MetrologyHandle, i, "all", "result_type", "all_param", out hv_Results[i]);
                        if (hv_Results[i].Length < 1)
                        {
                            PrintLog("未搜索到对象", EnumLogType.Error);
                            return;
                        }
                    }
                    // 获取检测到的轮廓
                    for (int i = 0; i < totalObject; i++)
                    {
                        HOperatorSet.GetMetrologyObjectResultContour(out HObject ho_Contour, Hv_MetrologyHandle, i, "all", 1.5);
                        Ho_Window.SetColor(EnumHalColor.spring_green.ToColorString());
                        Ho_Window.DispObj(ho_Contour);
                        ho_Contour.Dispose();
                    }
                    // 获取所有测量对象的轮廓
                    HOperatorSet.GetMetrologyObjectMeasures(out HObject ho_Contours, Hv_MetrologyHandle, "all", "all", out HTuple hv_Row, out HTuple hv_Column);
                    // 显示
                    Ho_Window.SetColor(EnumHalColor.slate_blue.ToColorString());
                    Ho_Window.SetLineWidth(1);
                    Ho_Window.DispObj(ho_Contours);
                    ho_Contours.Dispose();
                    /// 3. 计算当前点和当前角度
                    double row_cur = hv_Results[0][0].D;
                    double col_cur = hv_Results[0][1].D;
                    double ang_cur = hv_Angles[idx].D;
                    // 绘制当前点
                    Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                    Ho_Window.SetLineWidth(3);
                    Ho_Window.DispCrossContour(row_cur, col_cur, 100, ang_cur);
                    if (Hv_HomMat2D.Length < 6)
                    {
                        continue;
                    }
                    /// 4. 计算偏移量
                    // 图像 → 机械
                    double row_ref = CalibrationReference.RowRef;
                    double col_ref = CalibrationReference.ColRef;
                    double ang_ref = CalibrationReference.AngRef;
                    HOperatorSet.AffineTransPoint2d(Hv_HomMat2D, col_ref, row_ref, out HTuple hv_QxRef, out HTuple hv_QyRef);
                    HOperatorSet.AffineTransPoint2d(Hv_HomMat2D, col_cur, row_cur, out HTuple hv_QxCur, out HTuple hv_QyCur);

                    // 计算偏移角度 判断是否需要添加正负号 机械角度的旋转方向（顺时针为负 逆时针为正）
                    //double offset_a = ang_cur - ang_ref;
                    double offset_a = hv_Angles.D;
                    NumOffsetA = offset_a * 180 / Math.PI;
                    // 先做旋转校正
                    //HOperatorSet.AffineTransPoint2d(Hv_HomMat2D, RotateCenterY, RotateCenterX, out HTuple RotateCenterYm, out HTuple RotateCenterXm);
                    //HOperatorSet.VectorAngleToRigid(RotateCenterYm, RotateCenterXm, 0, RotateCenterYm, RotateCenterXm, offset_a, out hv_HomMat2D);
                    //HOperatorSet.AffineTransPoint2d(hv_HomMat2D, hv_QxCur, hv_QyCur, out HTuple hv_QxTra, out HTuple hv_QyTra);
                    // 再做平移校正
                    NumOffsetX = hv_QxCur.D - hv_QxRef.D;
                    NumOffsetY = hv_QyCur.D - hv_QyRef.D;
                }

                PrintLog("模型对象搜索完成", EnumLogType.Success);
            }
            catch (Exception ex)
            {
                PrintLog("模型应用异常：" + ex.Message, EnumLogType.Error);
            }
        }
        #endregion

        #region 4. 内部方法
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
                CanEditModule = false;
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
        private void DrawingBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanMove = false;
        }
        private void DrawingBorder_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            Point curPoint = e.GetPosition(e.Device.Target);
            // 当前点信息
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

            // 编辑测量对象
            if (CanEditModule)
            {
                if (DrawingCanvas.Strokes.Count < 1)
                {
                    return;
                }

                if (!IsEditingModule)
                {
                    // 会卡顿 只判断一次 不同位置对应不同鼠标形状
                    ModuleEditType = DrawingCanvas.Strokes[0].GetMetrologyObjectEditType(curPoint, EnumMetrologyObjectSel);
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
                    DrawingBorder.Cursor = Cursors.Arrow;
                }

                // 编辑
                if (CanMove)
                {
                    if (ModuleEditType == EnumModuleEditType.None)
                    {
                        return;
                    }
                    // 移动
                    else if (ModuleEditType == EnumModuleEditType.Move)
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
                        if (EnumMetrologyObjectSel == EnumMetrologyObjectType.rectangle || EnumMetrologyObjectSel == EnumMetrologyObjectType.ellipse)
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
                        if (EnumMetrologyObjectSel == EnumMetrologyObjectType.rectangle || EnumMetrologyObjectSel == EnumMetrologyObjectType.ellipse)
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
                        else if (EnumMetrologyObjectSel == EnumMetrologyObjectType.circle)
                        {
                            Point p0 = (Point)sps[0];
                            Point p1 = (Point)sps[1];
                            double radius = InkMethod.GetDistancePP(p0, p1);
                            double r = InkMethod.GetDistancePP(p0, curPoint);
                            Matrix matrixResize = new Matrix();
                            matrixResize.ScaleAt(r / radius, r / radius, p0.X, p0.Y);
                            DrawingCanvas.Strokes[0].Transform(matrixResize, false);
                        }
                        else if (EnumMetrologyObjectSel == EnumMetrologyObjectType.line)
                        {
                            // 找到选中点 鼠标滑动点距离最近的点
                            double r1 = InkMethod.GetDistancePP((Point)sps[0], PointMoveOri);
                            double r2 = InkMethod.GetDistancePP((Point)sps[1], PointMoveOri);
                            int index = r1 < r2 ? 0 : 1;
                            // 将选中点的坐标替换为新的坐标
                            sps[index] = new StylusPoint(curPoint.X, curPoint.Y);
                        }
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

            // 创建测量对象
            if (IsMakingModule && CanMove)
            {
                DrawingCanvas.Strokes.Clear();
                if (EnumMetrologyObjectSel == EnumMetrologyObjectType.rectangle)
                {
                    DrawingCanvas.Strokes.Add(InkMethod.CreateRectangle(PointMoveOri, curPoint));
                }
                else if (EnumMetrologyObjectSel == EnumMetrologyObjectType.ellipse)
                {
                    DrawingCanvas.Strokes.Add(InkMethod.CreateEllipse(PointMoveOri, curPoint));
                }
                else if (EnumMetrologyObjectSel == EnumMetrologyObjectType.circle)
                {
                    DrawingCanvas.Strokes.Add(InkMethod.CreateCircle(PointMoveOri, curPoint));
                }
                else if (EnumMetrologyObjectSel == EnumMetrologyObjectType.line)
                {
                    DrawingCanvas.Strokes.Add(InkMethod.CreateLine(PointMoveOri, curPoint));
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
        private void DrawingBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            // 双击 不可编辑
            if (e.ClickCount == 2)
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
        #endregion
    }
}