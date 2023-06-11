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
    /// Created Time: 22/09/05 11:40:37
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/05 11:40:37    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CircleCalibrationToolVM : ViewModelBase
    {
        #region 1. 绑定变量
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

        private EnumMetrologyObjectType enumMetrologyObjectSel = EnumMetrologyObjectType.circle;
        public EnumMetrologyObjectType EnumMetrologyObjectSel
        {
            get => enumMetrologyObjectSel;
            set => Set(ref enumMetrologyObjectSel, value);
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


        /// <summary>
        /// 模式选择：标定、测量
        /// </summary>
        private bool boolIsCalibration = true;
        public bool BoolIsCalibration
        {
            get => boolIsCalibration;
            set => Set(ref boolIsCalibration, value);
        }
        #endregion

        #region 2. 全局变量
        private HSmartWindowControlWPF Halcon { get; set; }
        private MetrologyObjectVM MyMetrologyObjectVM { get; set; }
        private ImageChannelVM MyImageChannelVM { get; set; }
        private InkCanvas DrawingCanvas { get; set; }
        private Border DrawingBorder { get; set; }
        private HWindow Ho_Window { get; set; }
        private HObject Ho_Image = null;


        /// <summary>
        /// 测量模型
        /// </summary>
        private CMetrologyObjectMeasureParams MyMetrologyObjectMeasureParam { get; set; }


        /// <summary>
        /// 绘制模板 编辑
        /// </summary>
        private Point PointMoveOri { get; set; } = new Point();
        private bool CanMove { get; set; } = false;
        private EnumModuleEditType ModuleEditType { get; set; } = EnumModuleEditType.None;
        private bool IsMakingModule { get; set; } = false;
        private bool IsEditingModule { get; set; } = false;
        private bool CanEditModule { get; set; } = false;

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


        public CircleCalibrationToolVM()
        {

        }

        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as CircleCalibrationTool).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            MyImageChannelVM = (e.Source as CircleCalibrationTool).MyImageChannelControl.DataContext as ImageChannelVM;
            MyMetrologyObjectVM = (e.Source as CircleCalibrationTool).MyMetrologyObjectControl.DataContext as MetrologyObjectVM;
            DrawingCanvas = (e.Source as CircleCalibrationTool).DrawingCanvas;
            DrawingBorder = (e.Source as CircleCalibrationTool).DrawingBorder;
            MyPixelValueControl = (e.Source as CircleCalibrationTool).MyPixelValueControl;

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
                    Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                    Ho_Window.SetLineWidth(3);
                    if (IsFirstShow)
                    {
                        IsFirstShow = false;
                        Halcon.SetFullImagePart();
                    }
                    SetDefault();
                    PrintLog("图像加载完成：" + filename, EnumLogType.Info);
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
                        PrintLog("图像保存完成：" + filename, EnumLogType.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }


        /// <summary>
        /// 编辑模板
        /// </summary>
        public RelayCommand CmdEditObject => new Lazy<RelayCommand>(() => new RelayCommand(EditObject)).Value;
        private void EditObject()
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
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 执行命令：标定、测量
        /// </summary>
        public RelayCommand CmdRunCommand => new Lazy<RelayCommand>(() => new RelayCommand(RunCommand)).Value;
        private void RunCommand()
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

                /// 先创建一个模型
                /// 
                CMetrologyObjectParam objParams = new CMetrologyObjectParam(MyMetrologyObjectVM);

                if (MyMetrologyObjectMeasureParam == null)
                {
                    if (DrawingCanvas.Strokes.Count < 1)
                    {
                        PrintLog("未发现绘制对象", EnumLogType.Warning);
                        return;
                    }

                    string modelName = DrawingCanvas.Strokes[0].GetType().Name.ToString();
                    

                    // 坐标转换：控件 → 图像
                    PointCollection points = Halcon.ControlPointToHImagePoint(DrawingCanvas.Strokes[0].StylusPoints);
                    if (modelName == "CustomRectangle" || modelName == "CustomEllipse")
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
                        double len1 = 0.5 * InkMethod.GetDistancePP(points[2], points[4]);
                        double len2 = 0.5 * InkMethod.GetDistancePP(points[0], points[2]);
                        // 中心点
                        double row = 0.5 * (points[0].Y + points[4].Y);
                        double col = 0.5 * (points[0].X + points[4].X);
                        MyMetrologyObjectMeasureParam = new CMetrologyObjectMeasureParams
                        {
                            Row = row,
                            Column = col,
                            Phi = angle,
                            Length1 = len1,
                            Length2 = len2,
                            MeasureLength1 = objParams.measure_length1,
                            MeasureLength2 = objParams.measure_length2,
                            MeasureSigma = objParams.measure_sigma,
                            MeasureThreshold = objParams.measure_threshold,
                        };
                        MyMetrologyObjectMeasureParam.Type = modelName == "CustomRectangle" ? EnumMetrologyObjectType.rectangle : EnumMetrologyObjectType.ellipse;
                    }
                    else if (modelName == "CustomCircle")
                    {
                        double radius = InkMethod.GetDistancePP(points[0], points[1]);
                        MyMetrologyObjectMeasureParam = new CMetrologyObjectMeasureParams
                        {
                            Type = EnumMetrologyObjectType.circle,
                            Row = points[0].Y,
                            Column = points[0].X,
                            Radius = radius,
                            MeasureLength1 = objParams.measure_length1,
                            MeasureLength2 = objParams.measure_length2,
                            MeasureSigma = objParams.measure_sigma,
                            MeasureThreshold = objParams.measure_threshold,
                        };

                    }
                    else if (modelName == "CustomLine")
                    {
                        MyMetrologyObjectMeasureParam = new CMetrologyObjectMeasureParams
                        {
                            Type = EnumMetrologyObjectType.line,
                            RowBegin = points[0].Y,
                            ColumnBegin = points[0].X,
                            RowEnd = points[1].Y,
                            ColumnEnd = points[1].X,
                            MeasureLength1 = objParams.measure_length1,
                            MeasureLength2 = objParams.measure_length2,
                            MeasureSigma = objParams.measure_sigma,
                            MeasureThreshold = objParams.measure_threshold,
                        };
                    }
                    CanEditModule = false;
                    DrawingBorder.Cursor = Cursors.Arrow;
                    DrawingCanvas.Strokes.Clear();
                }

                // 创建测量模型
                HOperatorSet.CreateMetrologyModel(out HTuple hv_MetrologyHandle);
                // 设置模型对象图像大小
                HOperatorSet.GetImageSize(Ho_Image, out HTuple width, out HTuple height);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, width, height);
                // 结果
                if (EnumMetrologyObjectSel == EnumMetrologyObjectType.rectangle)
                {
                    double row = MyMetrologyObjectMeasureParam.Row;
                    double col = MyMetrologyObjectMeasureParam.Column;
                    double len1 = MyMetrologyObjectMeasureParam.Length1;
                    double len2 = MyMetrologyObjectMeasureParam.Length2;
                    double phi = MyMetrologyObjectMeasureParam.Phi;
                    double mlen1 = MyMetrologyObjectMeasureParam.MeasureLength1;
                    double mlen2 = MyMetrologyObjectMeasureParam.MeasureLength2;
                    double msigma = MyMetrologyObjectMeasureParam.MeasureSigma;
                    double mthreshold = MyMetrologyObjectMeasureParam.MeasureThreshold;
                    HOperatorSet.AddMetrologyObjectRectangle2Measure(hv_MetrologyHandle, row, col, phi, len1, len2, mlen1, mlen2, msigma, mthreshold, new HTuple(), new HTuple(), out _);
                }
                else if (EnumMetrologyObjectSel == EnumMetrologyObjectType.ellipse)
                {
                    double row = MyMetrologyObjectMeasureParam.Row;
                    double col = MyMetrologyObjectMeasureParam.Column;
                    double len1 = MyMetrologyObjectMeasureParam.Length1;
                    double len2 = MyMetrologyObjectMeasureParam.Length2;
                    double phi = MyMetrologyObjectMeasureParam.Phi;
                    double mlen1 = MyMetrologyObjectMeasureParam.MeasureLength1;
                    double mlen2 = MyMetrologyObjectMeasureParam.MeasureLength2;
                    double msigma = MyMetrologyObjectMeasureParam.MeasureSigma;
                    double mthreshold = MyMetrologyObjectMeasureParam.MeasureThreshold;
                    HOperatorSet.AddMetrologyObjectEllipseMeasure(hv_MetrologyHandle, row, col, phi, len1, len2, mlen1, mlen2, msigma, mthreshold, new HTuple(), new HTuple(), out _);
                }
                else if (EnumMetrologyObjectSel == EnumMetrologyObjectType.circle)
                {
                    double row = MyMetrologyObjectMeasureParam.Row;
                    double col = MyMetrologyObjectMeasureParam.Column;
                    double r = MyMetrologyObjectMeasureParam.Radius;
                    double mlen1 = MyMetrologyObjectMeasureParam.MeasureLength1;
                    double mlen2 = MyMetrologyObjectMeasureParam.MeasureLength2;
                    double msigma = MyMetrologyObjectMeasureParam.MeasureSigma;
                    double mthreshold = MyMetrologyObjectMeasureParam.MeasureThreshold;
                    HOperatorSet.AddMetrologyObjectCircleMeasure(hv_MetrologyHandle, row, col, r, mlen1, mlen2, msigma, mthreshold, new HTuple(), new HTuple(), out _);
                }
                else if (EnumMetrologyObjectSel == EnumMetrologyObjectType.line)
                {
                    double row1 = MyMetrologyObjectMeasureParam.RowBegin;
                    double col1 = MyMetrologyObjectMeasureParam.ColumnBegin;
                    double row2 = MyMetrologyObjectMeasureParam.RowEnd;
                    double col2 = MyMetrologyObjectMeasureParam.ColumnEnd;
                    double mlen1 = MyMetrologyObjectMeasureParam.MeasureLength1;
                    double mlen2 = MyMetrologyObjectMeasureParam.MeasureLength2;
                    double msigma = MyMetrologyObjectMeasureParam.MeasureSigma;
                    double mthreshold = MyMetrologyObjectMeasureParam.MeasureThreshold;
                    HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, row1, col1, row2, col2, mlen1, mlen2, msigma, mthreshold, new HTuple(), new HTuple(), out _);
                }
                
                // 设置对象参数 统一设置、单独对某个对象设置
                // 遍历属性和值
                PropertyInfo[] properties = objParams.GetType().GetProperties();
                foreach (PropertyInfo item in properties)
                {
                    string key = item.Name;
                    HTuple value = (HTuple)item.GetValue(objParams, null);
                    HOperatorSet.SetMetrologyObjectParam(hv_MetrologyHandle, "all", key, value);
                }

                // 应用测量模型
                HOperatorSet.ApplyMetrologyModel(Ho_Image, hv_MetrologyHandle);
                // 获取所有结果
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, 0, "all", "result_type", "all_param", out HTuple hv_Result);
                if (hv_Result.Length < 1)
                {
                    PrintLog("未发现目标", EnumLogType.Warning);
                    return;
                }
                // 获取检测到的轮廓
                HOperatorSet.GetMetrologyObjectResultContour(out HObject ho_Contour, hv_MetrologyHandle, 0, "all", 1.5);
                Ho_Window.ClearWindow();
                Ho_Window.DispObj(Ho_Image);
                Ho_Window.SetLineWidth(3);
                // 先设置颜色
                Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                Ho_Window.DispObj(ho_Contour);
                ho_Contour.Dispose();
                // 获取所有测量对象的轮廓
                HOperatorSet.GetMetrologyObjectMeasures(out HObject ho_Contours, hv_MetrologyHandle, "all", "all", out _, out _);
                // 显示
                Ho_Window.SetColor(EnumHalColor.spring_green.ToColorString());
                Ho_Window.SetLineWidth(1);
                Ho_Window.DispObj(ho_Contours);
                ho_Contours.Dispose();
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                hv_MetrologyHandle.Dispose();
                PrintLog("检测到目标", EnumLogType.Info);

                /// 下一步做标定、计算
                switch (EnumMetrologyObjectSel)
                {
                    case EnumMetrologyObjectType.rectangle:
                        NumAvePixel = 2 * hv_Result[4].D;
                        break;
                    case EnumMetrologyObjectType.ellipse:
                        NumAvePixel = 2 * hv_Result[3].D;
                        break;
                    case EnumMetrologyObjectType.circle:
                        NumAvePixel = 2 * hv_Result[2].D;
                        break;
                    case EnumMetrologyObjectType.line:
                        NumAvePixel = Math.Sqrt(((hv_Result[0] - hv_Result[2]) * (hv_Result[0] - hv_Result[2])) + ((hv_Result[1] - hv_Result[3]) * (hv_Result[1] - hv_Result[3])));
                        break;
                    default:
                        break;
                }

                if (BoolIsCalibration)
                {
                    NumPixelSize = NumActualLength / NumAvePixel;
                    PrintLog("标定完成", EnumLogType.Success);
                }
                else
                {
                    NumActualLength = NumPixelSize * NumAvePixel;
                    PrintLog("测量完成", EnumLogType.Success);
                }
            }
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
            }
        }


        /// <summary>
        /// 加载标定
        /// </summary>
        public RelayCommand CmdLoadCalibration => new Lazy<RelayCommand>(() => new RelayCommand(LoadCalibration)).Value;
        private void LoadCalibration()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "选择尺寸标定文件",
                    Filter = "尺寸标定文件(*.xml)|*.xml",
                    RestoreDirectory = true,
                    InitialDirectory = Environment.CurrentDirectory + "\\Config",
                    FileName = HalconIoMethod.GenCaliperName(StrRecipeName),
                };
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                string filename = dialog.FileName;
                StrRecipeName = Path.GetFileNameWithoutExtension(filename).Replace("Caliper_", "");
                double pixelsize = 0;
                bool flag = HalconIoMethod.LoadCaliper(filename, ref pixelsize);
                if (flag)
                {
                    NumPixelSize = pixelsize;
                    PrintLog("尺寸标定文件加载完成：" + filename, EnumLogType.Info);
                }
            }
            catch (Exception ex)
            {
                PrintLog("卡尺标定文件读取异常：" + ex.Message, EnumLogType.Error);
            }
        }


        /// <summary>
        /// 保存标定
        /// </summary>
        public RelayCommand CmdSaveCalibration => new Lazy<RelayCommand>(() => new RelayCommand(SaveCalibration)).Value;
        private void SaveCalibration()
        {
            // 保存标定结果
            string filename = HalconIoMethod.GenCaliperName(StrRecipeName);
            bool flag = HalconIoMethod.SaveCaliper(filename, NumPixelSize);
            if (flag)
            {
                PrintLog("像素尺寸标定文件保存完成：" + filename, EnumLogType.Success);
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
            try
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
            catch (Exception ex)
            {
                PrintLog("异常：" + ex.Message, EnumLogType.Error);
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
            MyMetrologyObjectMeasureParam = null;
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
        #endregion
    }
}