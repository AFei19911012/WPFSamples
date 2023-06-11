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
using Wpf_Base.CommunicationWpf;
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
    /// Created Time: 22/09/03 16:13:12
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 16:13:12    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CalibrationToolVM : ViewModelBase
    {
        #region 1. 绑定变量
        /// <summary>
        /// Marker 点获取方式
        /// </summary>
        private Dictionary<EnumGetMarkerMode, string> enumGetMarkerCal;
        public Dictionary<EnumGetMarkerMode, string> EnumGetMarkerCal
        {
            get
            {
                Dictionary<EnumGetMarkerMode, string> pairs = new Dictionary<EnumGetMarkerMode, string>();
                foreach (EnumGetMarkerMode item in Enum.GetValues(typeof(EnumGetMarkerMode)))
                {
                    DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                    pairs.Add(item, attributes.Description);
                }
                return pairs;
            }

            set => Set(ref enumGetMarkerCal, value);
        }
        private EnumGetMarkerMode enumGetMarkerSel = EnumGetMarkerMode.module;
        public EnumGetMarkerMode EnumGetMarkerSel
        {
            get => enumGetMarkerSel;
            set => Set(ref enumGetMarkerSel, value);
        }


        private double numScore = 0.8;
        public double NumScore
        {
            get => numScore;
            set => Set(ref numScore, value);
        }


        /// <summary>
        /// 标定点坐标
        /// </summary>
        private ObservableCollection<CDataModel> dataListCalMarker;
        public ObservableCollection<CDataModel> DataListCalMarker
        {
            get => dataListCalMarker;
            set => Set(ref dataListCalMarker, value);
        }
        private int intSelectedIndex = -1;
        public int IntSelectedIndex
        {
            get => intSelectedIndex;
            set => Set(ref intSelectedIndex, value);
        }


        /// <summary>
        /// 标定结果
        /// </summary>
        private double valueA1 = 1;
        public double ValueA1
        {
            get => valueA1;
            set => Set(ref valueA1, value);
        }
        private double valueB1 = 0;
        public double ValueB1
        {
            get => valueB1;
            set => Set(ref valueB1, value);
        }
        private double valueC1 = 0;
        public double ValueC1
        {
            get => valueC1;
            set => Set(ref valueC1, value);
        }
        private double valueA2 = 0;
        public double ValueA2
        {
            get => valueA2;
            set => Set(ref valueA2, value);
        }
        private double valueB2 = 1;
        public double ValueB2
        {
            get => valueB2;
            set => Set(ref valueB2, value);
        }
        private double valueC2 = 0;
        public double ValueC2
        {
            get => valueC2;
            set => Set(ref valueC2, value);
        }
        private double centerX = 0;
        public double CenterX
        {
            get => centerX;
            set => Set(ref centerX, value);
        }
        private double centerY = 0;
        public double CenterY
        {
            get => centerY;
            set => Set(ref centerY, value);
        }

        private double valueX0 = 0;
        public double ValueX0
        {
            get => valueX0;
            set => Set(ref valueX0, value);
        }

        private double valueY0 = 0;
        public double ValueY0
        {
            get => valueY0;
            set => Set(ref valueY0, value);
        }

        private double valueR0 = 0;
        public double ValueR0
        {
            get => valueR0;
            set => Set(ref valueR0, value);
        }


        /// <summary>
        /// 运动点序号
        /// </summary>
        private EnumRobotMovePoint enumRobotPositionNumber;
        public EnumRobotMovePoint EnumRobotPositionNumber
        {
            get => enumRobotPositionNumber;
            set => Set(ref enumRobotPositionNumber, value);
        }


        /// <summary>
        /// 运动类型
        /// </summary>
        private Dictionary<EnumMoveType, string> enumMoveType;
        public Dictionary<EnumMoveType, string> EnumMoveTypeCal
        {
            get
            {
                Dictionary<EnumMoveType, string> pairs = new Dictionary<EnumMoveType, string>();
                foreach (EnumMoveType item in Enum.GetValues(typeof(EnumMoveType)))
                {
                    DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                    pairs.Add(item, attributes.Description);
                }
                return pairs;
            }

            set => Set(ref enumMoveType, value);
        }
        private EnumMoveType enumMoveTypeSel = EnumMoveType.move_xy_rotation;
        public EnumMoveType EnumMoveTypeSel
        {
            get => enumMoveTypeSel;
            set => Set(ref enumMoveTypeSel, value);
        }


        /// <summary>
        /// 标定类型
        /// </summary>
        private Dictionary<EnumCalibrationType, string> enumCalibrationType;
        public Dictionary<EnumCalibrationType, string> EnumCalibrationTypeCal
        {
            get
            {
                Dictionary<EnumCalibrationType, string> pairs = new Dictionary<EnumCalibrationType, string>();
                foreach (EnumCalibrationType item in Enum.GetValues(typeof(EnumCalibrationType)))
                {
                    DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                    pairs.Add(item, attributes.Description);
                }
                return pairs;
            }

            set => Set(ref enumCalibrationType, value);
        }
        private EnumCalibrationType enumCalibrationTypeSel = EnumCalibrationType.point_9;
        public EnumCalibrationType EnumCalibrationTypeSel
        {
            get => enumCalibrationTypeSel;
            set => Set(ref enumCalibrationTypeSel, value);
        }


        /// <summary>
        /// 旋转类型
        /// </summary>
        private EnumRotateType enumRotateTypeSel = EnumRotateType.Rotate_3次旋转;
        public EnumRotateType EnumRotateTypeSel
        {
            get => enumRotateTypeSel;
            set => Set(ref enumRotateTypeSel, value);
        }


        private double numDx = 10;
        public double NumDx
        {
            get => numDx;
            set => Set(ref numDx, value);
        }
        private double numDy = 10;
        public double NumDy
        {
            get => numDy;
            set => Set(ref numDy, value);
        }
        private bool boolSwitchXY;
        public bool BoolSwitchXY
        {
            get => boolSwitchXY;
            set => Set(ref boolSwitchXY, value);
        }
        private double numDr = 30;
        public double NumDr
        {
            get => numDr;
            set => Set(ref numDr, value);
        }


        /// <summary>
        /// XY方向
        /// </summary>
        private Dictionary<EnumDirectionX, string> enumDirectionXCal;
        public Dictionary<EnumDirectionX, string> EnumDirectionXCal
        {
            get
            {
                Dictionary<EnumDirectionX, string> pairs = new Dictionary<EnumDirectionX, string>();
                foreach (EnumDirectionX item in Enum.GetValues(typeof(EnumDirectionX)))
                {
                    DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                    pairs.Add(item, attributes.Description);
                }
                return pairs;
            }

            set => Set(ref enumDirectionXCal, value);
        }
        private EnumDirectionX enumDirectionXSel = EnumDirectionX.positive;
        public EnumDirectionX EnumDirectionXSel
        {
            get => enumDirectionXSel;
            set => Set(ref enumDirectionXSel, value);
        }


        private Dictionary<EnumDirectionY, string> enumDirectionYCal;
        public Dictionary<EnumDirectionY, string> EnumDirectionYCal
        {
            get
            {
                Dictionary<EnumDirectionY, string> pairs = new Dictionary<EnumDirectionY, string>();
                foreach (EnumDirectionY item in Enum.GetValues(typeof(EnumDirectionY)))
                {
                    DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                    pairs.Add(item, attributes.Description);
                }
                return pairs;
            }

            set => Set(ref enumDirectionYCal, value);
        }
        private EnumDirectionY enumDirectionYSel = EnumDirectionY.positive;
        public EnumDirectionY EnumDirectionYSel
        {
            get => enumDirectionYSel;
            set => Set(ref enumDirectionYSel, value);
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
        private FindShapeModelVM MyFindShapeModelVM { get; set; }
        private HWindow Ho_Window { get; set; }
        private Border DrawingBorder { get; set; }
        private HObject Ho_Image = null;
        private bool IsFirstShow { get; set; } = true;
        private bool IsMarkering { get; set; } = false;
        private HTuple Hv_ShapeModelID = new HTuple();
        private bool CanMove { get; set; } = false;
        private Point PointMoveOri { get; set; } = new Point();

        /// <summary>
        /// ModBus 通讯地址
        /// </summary>
        private int AddressMBS { get; set; } = 100;

        private PixelValueControl MyPixelValueControl { get; set; }
        private bool CanRefreshImage { get; set; } = false;
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
        public CalibrationToolVM()
        {
            List<Point> pts = new List<Point>
            {
                new Point(0, 0),
                new Point(NumDx, 0),
                new Point(NumDx, -NumDy),
                new Point(0, -NumDy),
                new Point(-NumDx, -NumDy),
                new Point(-NumDx, 0),
                new Point(-NumDx, NumDy),
                new Point(0, NumDy),
                new Point(NumDx, NumDy),
            };
            DataListCalMarker = InitMethod.InitMarkers(pts, 30);

            // 自动标定
            Task task = new Task(CmdAutoRunCal);
            task.Start();
        }

        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as CalibrationTool).HalconWPF;
            Ho_Window = Halcon.HalconWindow;
            MyFindShapeModelVM = (e.Source as CalibrationTool).MyFindShapeModelControl.DataContext as FindShapeModelVM;
            MyPixelValueControl = (e.Source as CalibrationTool).MyPixelValueControl;
            DrawingBorder = (e.Source as CalibrationTool).DrawingBorder;
            // 初始化不要放在构造函数里 试图加载格式不正确的程序。(Exception from HRESULT: 0x8007000B)
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
            // 载入图像
            if (name == "LoadImage")
            {
                try
                {
                    PrintLog("加载图像", EnumLogType.Debug);
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
                catch (Exception ex)
                {
                    PrintLog("图像加载异常：" + ex.Message, EnumLogType.Error);
                }
            }
            // 保存图像
            else if (name == "SaveImage")
            {
                try
                {
                    PrintLog("保存图像", EnumLogType.Debug);
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
                catch (Exception ex)
                {
                    PrintLog("图像保存异常：" + ex.Message, EnumLogType.Error);
                }
            }
        }

        /// <summary>
        /// 加载形状模板
        /// </summary>
        public RelayCommand CmdLoadShapeModule => new Lazy<RelayCommand>(() => new RelayCommand(LoadShapeModule)).Value;
        private void LoadShapeModule()
        {
            PrintLog("加载模板", EnumLogType.Debug);
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
        /// 设置参数
        /// </summary>
        public RelayCommand CmdConfirmParam => new Lazy<RelayCommand>(() => new RelayCommand(ConfirmParam)).Value;
        private void ConfirmParam()
        {
            PrintLog("设置参数", EnumLogType.Debug);
            if (Ho_Image.IsInitialized())
            {
                Ho_Window.ClearWindow();
                Ho_Window.DispObj(Ho_Image);
            }
            double dx = NumDx;
            double dy = NumDy;
            double dr = NumDr;
            // 判断 x 和 y 的方向
            bool switchXY = BoolSwitchXY;
            EnumDirectionX directionX = EnumDirectionXSel;
            EnumDirectionY directionY = EnumDirectionYSel;
            // 标准方向 → +x  ↑ +y
            List<Point> pts = new List<Point>();
            if (!switchXY)
            {
                if (directionX == EnumDirectionX.negative)
                {
                    dx = -dx;
                }
                if (directionY == EnumDirectionY.negative)
                {
                    dy = -dy;
                }
                pts = new List<Point>
                        {
                            new Point(0, 0),
                            new Point(dx, 0),
                            new Point(dx, -dy),
                            new Point(0, -dy),
                            new Point(-dx, -dy),
                            new Point(-dx, 0),
                            new Point(-dx, dy),
                            new Point(0, dy),
                            new Point(dx, dy),
                        };
            }
            else
            {
                if (directionY == EnumDirectionY.negative)
                {
                    dx = -dx;
                }
                if (directionX == EnumDirectionX.negative)
                {
                    dy = -dy;
                }
                pts = new List<Point>
                        {
                            new Point(0, 0),
                            new Point(0, dy),
                            new Point(-dx, dy),
                            new Point(-dx, 0),
                            new Point(-dx, -dy),
                            new Point(0, -dy),
                            new Point(dx, -dy),
                            new Point(dx, 0),
                            new Point(dx, dy),
                        };
            }
            DataListCalMarker = InitMethod.InitMarkers(pts, dr, EnumRotateTypeSel);
            IntSelectedIndex = -1;
            EnumRobotPositionNumber = EnumRobotMovePoint.point_0;
        }

        /// <summary>
        /// 获取 Marker 点
        /// </summary>
        public RelayCommand CmdGetMarker => new Lazy<RelayCommand>(() => new RelayCommand(GetMarker)).Value;
        private void GetMarker()
        {
            EnumRobotPositionNumber = (EnumRobotMovePoint)IntSelectedIndex + 1;
            if (EnumRobotPositionNumber == EnumRobotMovePoint.point_0)
            {
                PrintLog("未在列表中选中起始点", EnumLogType.Warning);
                return;
            }

            if (CanRefreshImage)
            {
                GetCurrentImage();

                if (!Ho_Image.IsInitialized())
                {
                    PrintLog("相机抓图失败", EnumLogType.Error);
                    return;
                }
                PrintLog("相机抓图完成", EnumLogType.Success);
            }

            if (!Ho_Image.IsInitialized())
            {
                PrintLog("图像无效", EnumLogType.Warning);
                return;
            }
            // 取点模式
            IsMarkering = true;
        }

        /// <summary>
        /// 显示 Marker 点
        /// </summary>
        public RelayCommand CmdShowMarker => new Lazy<RelayCommand>(() => new RelayCommand(ShowMarker)).Value;
        private void ShowMarker()
        {
            // 禁止刷新图像
            CanRefreshImage = false;

            for (int i = 0; i < DataListCalMarker.Count; i++)
            {
                double x = DataListCalMarker[i].ImageX;
                double y = DataListCalMarker[i].ImageY;
                // 只绘制有效数据
                if (DataListCalMarker[i].ImageY * DataListCalMarker[i].ImageX > 0)
                {
                    // 绘制 Marker 点
                    Ho_Window.SetLineWidth(3);
                    if (i < 9)
                    {
                        Ho_Window.SetColor("orange red");
                        Ho_Window.DispCross(y, x, CConstants.MarkerCrossLenght, 0);
                    }
                    else
                    {
                        Ho_Window.SetColor("green");
                        Ho_Window.DispCross(y, x, CConstants.MarkerCrossLenght, 0.785398);
                    }
                }
            }
        }

        /// <summary>
        /// 九点标定
        /// </summary>
        public RelayCommand CmdRunCalibrationXY => new Lazy<RelayCommand>(() => new RelayCommand(RunCalibrationXY)).Value;
        private void RunCalibrationXY()
        {
            try
            {
                PrintLog("执行九点标定", EnumLogType.Debug);
                // 从列表中获取数据
                // 标定
                HTuple hv_image_x = new HTuple();
                HTuple hv_image_y = new HTuple();
                HTuple hv_machine_x = new HTuple();
                HTuple hv_machine_y = new HTuple();
                hv_image_x.Dispose();
                hv_image_y.Dispose();
                hv_machine_x.Dispose();
                hv_machine_y.Dispose();
                for (int i = 0; i < 9; i++)
                {
                    if (DataListCalMarker[i].ImageY * DataListCalMarker[i].ImageX > 0)
                    {
                        hv_image_x[i] = DataListCalMarker[i].ImageX;
                        hv_image_y[i] = DataListCalMarker[i].ImageY;
                        hv_machine_x[i] = DataListCalMarker[i].RobotX;
                        hv_machine_y[i] = DataListCalMarker[i].RobotY;
                    }
                }
                if (hv_image_x.TupleLength() < 4)
                {
                    PrintLog("九点标定至少需要 4 个点", EnumLogType.Warning);
                    hv_image_x.Dispose();
                    hv_image_y.Dispose();
                    hv_machine_x.Dispose();
                    hv_machine_y.Dispose();
                    return;
                }

                // 计算仿射矩阵
                HTuple hv_HomMat2D = new HTuple();
                hv_HomMat2D.Dispose();
                HOperatorSet.VectorToHomMat2d(hv_image_x, hv_image_y, hv_machine_x, hv_machine_y, out hv_HomMat2D);
                ValueA1 = hv_HomMat2D[0].D;
                ValueB1 = hv_HomMat2D[1].D;
                ValueC1 = hv_HomMat2D[2].D;
                ValueA2 = hv_HomMat2D[3].D;
                ValueB2 = hv_HomMat2D[4].D;
                ValueC2 = hv_HomMat2D[5].D;

                // 坐标转换：图像 → 物理
                HOperatorSet.AffineTransPoint2d(hv_HomMat2D, hv_image_x, hv_image_y, out HTuple hv_machine_xfit, out HTuple hv_machine_yfit);
                hv_HomMat2D.Dispose();
                // 计算误差
                double err = HalMethod.GetCalibrationStd(hv_machine_x, hv_machine_y, hv_machine_xfit, hv_machine_yfit);
                hv_machine_x.Dispose();
                hv_machine_y.Dispose();
                hv_machine_xfit.Dispose();
                hv_machine_yfit.Dispose();
                PrintLog(string.Format("九点标定偏差 = {0:F3} mm", err), EnumLogType.Info);
                // 保存标定结果
                SaveCalibration();
            }
            catch (Exception ex)
            {
                PrintLog("标定异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 旋转标定
        /// </summary>
        public RelayCommand CmdRunCalibrationR => new Lazy<RelayCommand>(() => new RelayCommand(RunCalibrationR)).Value;
        private void RunCalibrationR()
        {
            try
            {
                PrintLog("执行旋转标定", EnumLogType.Debug);
                // 从列表中获取数据
                // 拟合圆得到旋转中心
                HTuple hv_rows = new HTuple();
                HTuple hv_cols = new HTuple();
                hv_rows.Dispose();
                hv_cols.Dispose();
                for (int i = 9; i < DataListCalMarker.Count; i++)
                {
                    if (DataListCalMarker[i].ImageY * DataListCalMarker[i].ImageX > 0)
                    {
                        hv_rows[i - 9] = DataListCalMarker[i].ImageY;
                        hv_cols[i - 9] = DataListCalMarker[i].ImageX;
                    }
                }
                if (hv_rows.Length < 3)
                {
                    PrintLog("计算旋转中心至少需要 3 个点", EnumLogType.Warning);
                    return;
                }

                HalMethod.FitCircle(hv_rows, hv_cols, out double oX, out double oY, out _);
                CenterX = oX;
                CenterY = oY;

                // 保存标定结果
                SaveCalibration();
            }
            catch (Exception ex)
            {
                PrintLog("标定异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 自动标定
        /// </summary>
        public RelayCommand CmdRunCalibrationAuto => new Lazy<RelayCommand>(() => new RelayCommand(RunCalibrationAuto)).Value;
        private void RunCalibrationAuto()
        {
            try
            {
                if (CcdManager.Instance.NumberCCD == 0 || CcdManager.Instance.CurrentCamId < 0)
                {
                    PrintLog("未连接到相机", EnumLogType.Warning);
                    return;
                }
                PrintLog("执行自动标定", EnumLogType.Debug);

                if (Hv_ShapeModelID.ToString().Length < 5)
                {
                    PrintLog("未加载模板", EnumLogType.Warning);
                    return;
                }
                // 重置列表
                ConfirmParam();
                // 获取拍照点位置 即原点位置
                ValueX0 = ModbusManager.Instance.ReadFloat(AddressMBS + 6);
                ValueY0 = ModbusManager.Instance.ReadFloat(AddressMBS + 8);
                ValueR0 = ModbusManager.Instance.ReadFloat(AddressMBS + 10);
                /// 先获取坐标
                for (int i = 0; i < DataListCalMarker.Count; i++)
                {
                    PrintLog("拾取点位：" + (i + 1), EnumLogType.Debug);
                    // 九点标定
                    if (EnumMoveTypeSel == EnumMoveType.move_xy)
                    {
                        if (i >= 9)
                        {
                            break;
                        }
                    }
                    // 旋转标定
                    else if (EnumMoveTypeSel == EnumMoveType.move_rotation)
                    {
                        if (i < 9)
                        {
                            continue;
                        }
                    }
                    // 当前点闪烁
                    EnumRobotPositionNumber = i < 12 ? (EnumRobotMovePoint)(i + 1) : EnumRobotMovePoint.rotate_1;
                    IntSelectedIndex = i;

                    // 第一个点是拍照点 跳过
                    if (i > 0)
                    {
                        // 机器人移动到指定位置
                        float x = (float)(ValueX0 + DataListCalMarker[i].RobotX);
                        float y = (float)(ValueY0 + DataListCalMarker[i].RobotY);
                        float angle = (float)DataListCalMarker[i].Angle;
                        _ = ModbusManager.Instance.Write(AddressMBS + 26, x);
                        _ = ModbusManager.Instance.Write(AddressMBS + 28, y);
                        _ = ModbusManager.Instance.Write(AddressMBS + 30, angle);
                        _ = ModbusManager.Instance.Write(AddressMBS + 20, 1);
                        _ = ModbusManager.Instance.Write(AddressMBS + 21, 9);

                        // 等待
                        DateTime t = DateTime.Now.AddMilliseconds(60000);
                        int flag1 = 0;
                        int flag2 = 0;
                        while (DateTime.Now < t)
                        {
                            DispatcherHelper.DoEvents();
                            flag1 = ModbusManager.Instance.ReadInt16(AddressMBS);
                            flag2 = ModbusManager.Instance.ReadInt16(AddressMBS + 1);
                            // 移动到位
                            if (flag1 == 1 && flag2 == 9)
                            {
                                _ = ModbusManager.Instance.Write(AddressMBS, 0);
                                _ = ModbusManager.Instance.Write(AddressMBS + 1, 0);
                                break;
                            }
                        }
                        if (flag1 != 1 || flag2 != 9)
                        {
                            PrintLog("机器人移动超时", EnumLogType.Warning);
                            _ = ModbusManager.Instance.Write(AddressMBS + 20, 1);
                            _ = ModbusManager.Instance.Write(AddressMBS + 21, 4);
                            _ = ModbusManager.Instance.Write(AddressMBS + 22, 0);
                            return;
                        }
                    }

                    try
                    {
                        GetCurrentImage();

                        if (!Ho_Image.IsInitialized())
                        {
                            PrintLog("相机抓图失败", EnumLogType.Error);
                            return;
                        }
                        PrintLog("相机抓图完成", EnumLogType.Debug);
                    }
                    catch (Exception ex)
                    {
                        PrintLog("相机抓图异常：" + ex.Message, EnumLogType.Error);
                        _ = ModbusManager.Instance.Write(AddressMBS + 21, 4);
                        _ = ModbusManager.Instance.Write(AddressMBS + 22, 0);
                        Thread.Sleep(20);
                        _ = ModbusManager.Instance.Write(AddressMBS + 20, 1);
                        return;
                    }

                    // 全图匹配
                    PrintLog("模板匹配 Marker 点", EnumLogType.Debug);
                    HOperatorSet.FindShapeModel(Ho_Image, Hv_ShapeModelID, MyFindShapeModelVM.NumSelectAngleStart, MyFindShapeModelVM.NumSelectAngleExtent,
                        MyFindShapeModelVM.NumMinScore, MyFindShapeModelVM.IntNumMatches, MyFindShapeModelVM.NumMaxOverlap, MyFindShapeModelVM.StrSelectSubPixel,
                        MyFindShapeModelVM.IntNumLevels, MyFindShapeModelVM.NumGreediness, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle, out HTuple hv_Score);
                    if (hv_Score.Length > 0 && hv_Score.D >= NumScore)
                    {
                        Ho_Window.ClearWindow();
                        Ho_Window.DispObj(Ho_Image);
                        Ho_Window.SetLineWidth(3);
                        // 绘制 Marker 点
                        if (i < 9)
                        {
                            Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                            Ho_Window.DispCrossContour(hv_Row.D, hv_Column.D, 100, 0);
                        }
                        else
                        {
                            Ho_Window.SetColor(EnumHalColor.green.ToColorString());
                            Ho_Window.DispCrossContour(hv_Row.D, hv_Column.D, 100, 0.785398);
                        }
                        // 匹配结果
                        MyFindShapeModelVM.NumMatchRow = hv_Row.D;
                        MyFindShapeModelVM.NumMatchCol = hv_Column.D;
                        MyFindShapeModelVM.NumMatchAngle = hv_Angle.D;
                        MyFindShapeModelVM.NumMatchScore = hv_Score.D;
                        // 更新 Marker 点坐标
                        CDataModel model = new CDataModel(DataListCalMarker[i])
                        {
                            ImageX = hv_Column.D,
                            ImageY = hv_Row.D
                        };
                        DataListCalMarker[i] = new CDataModel(model);
                        PrintLog("Marker 点拾取成功", EnumLogType.Success);
                    }
                    else
                    {
                        PrintLog("模板匹配失败", EnumLogType.Error);
                        _ = ModbusManager.Instance.Write(AddressMBS + 20, 1);
                        _ = ModbusManager.Instance.Write(AddressMBS + 21, 4);
                        _ = ModbusManager.Instance.Write(AddressMBS + 22, 0);
                        return;
                    }
                }
                // 所有点走完
                PrintLog("Marker 点拾取完成", EnumLogType.Success);
                _ = ModbusManager.Instance.Write(AddressMBS + 20, 1);
                _ = ModbusManager.Instance.Write(AddressMBS + 21, 4);
                _ = ModbusManager.Instance.Write(AddressMBS + 22, 1);

                IntSelectedIndex = -1;
                EnumRobotPositionNumber = EnumRobotMovePoint.point_0;

                /// 执行标定
                // 九点标定
                if (EnumMoveTypeSel == EnumMoveType.move_xy)
                {
                    RunCalibrationXY();
                }
                // 旋转标定
                else if (EnumMoveTypeSel == EnumMoveType.move_rotation)
                {
                    RunCalibrationR();
                }
                // 九点标定 + 旋转标定
                else
                {
                    PrintLog("执行九点标定+旋转标定", EnumLogType.Debug);
                    /// 九点标定
                    // 从列表中获取数据
                    HTuple hv_image_x = new HTuple();
                    HTuple hv_image_y = new HTuple();
                    HTuple hv_machine_x = new HTuple();
                    HTuple hv_machine_y = new HTuple();
                    hv_image_x.Dispose();
                    hv_image_y.Dispose();
                    hv_machine_x.Dispose();
                    hv_machine_y.Dispose();
                    for (int i = 0; i < 9; i++)
                    {
                        if (DataListCalMarker[i].ImageY * DataListCalMarker[i].ImageX > 0)
                        {
                            hv_image_x[i] = DataListCalMarker[i].ImageX;
                            hv_image_y[i] = DataListCalMarker[i].ImageY;
                            hv_machine_x[i] = DataListCalMarker[i].RobotX;
                            hv_machine_y[i] = DataListCalMarker[i].RobotY;
                        }
                    }
                    if (hv_image_x.TupleLength() < 4)
                    {
                        PrintLog("九点标定至少需要 4 个点", EnumLogType.Warning);
                        hv_image_x.Dispose();
                        hv_image_y.Dispose();
                        hv_machine_x.Dispose();
                        hv_machine_y.Dispose();
                        return;
                    }
                    // 计算仿射矩阵
                    HTuple hv_HomMat2D = new HTuple();
                    hv_HomMat2D.Dispose();
                    HOperatorSet.VectorToHomMat2d(hv_image_x, hv_image_y, hv_machine_x, hv_machine_y, out hv_HomMat2D);
                    ValueA1 = hv_HomMat2D[0].D;
                    ValueB1 = hv_HomMat2D[1].D;
                    ValueC1 = hv_HomMat2D[2].D;
                    ValueA2 = hv_HomMat2D[3].D;
                    ValueB2 = hv_HomMat2D[4].D;
                    ValueC2 = hv_HomMat2D[5].D;

                    // 坐标转换：图像 → 物理
                    HOperatorSet.AffineTransPoint2d(hv_HomMat2D, hv_image_x, hv_image_y, out HTuple hv_machine_xfit, out HTuple hv_machine_yfit);
                    hv_HomMat2D.Dispose();
                    // 计算误差
                    double err = HalMethod.GetCalibrationStd(hv_machine_x, hv_machine_y, hv_machine_xfit, hv_machine_yfit);
                    hv_machine_x.Dispose();
                    hv_machine_y.Dispose();
                    hv_machine_xfit.Dispose();
                    hv_machine_yfit.Dispose();
                    PrintLog(string.Format("九点标定偏差 = {0:F3} mm", err), EnumLogType.Info);

                    /// 拟合圆得到旋转中心
                    // 从列表中获取数据
                    HTuple hv_rows = new HTuple();
                    HTuple hv_cols = new HTuple();
                    hv_rows.Dispose();
                    hv_cols.Dispose();
                    for (int i = 9; i < DataListCalMarker.Count; i++)
                    {
                        if (DataListCalMarker[i].ImageY * DataListCalMarker[i].ImageX > 0)
                        {
                            hv_rows[i - 9] = DataListCalMarker[i].ImageY;
                            hv_cols[i - 9] = DataListCalMarker[i].ImageX;
                        }
                    }
                    if (hv_cols.Length < 3)
                    {
                        PrintLog("计算旋转中心至少需要 3 个点", EnumLogType.Warning);
                        return;
                    }

                    HalMethod.FitCircle(hv_rows, hv_cols, out double oX, out double oY, out _);
                    CenterX = oX;
                    CenterY = oY;

                    // 保存标定结果
                    SaveCalibration();
                }
            }
            catch (Exception ex)
            {
                _ = ModbusManager.Instance.Write(AddressMBS + 21, 4);
                _ = ModbusManager.Instance.Write(AddressMBS + 22, 0);
                Thread.Sleep(20);
                _ = ModbusManager.Instance.Write(AddressMBS + 20, 1);
                PrintLog("标定异常：" + ex.Message, EnumLogType.Error);
            }
        }

        public RelayCommand CmdSaveCalibration => new Lazy<RelayCommand>(() => new RelayCommand(SaveCalibration)).Value;
        private void SaveCalibration()
        {
            PrintLog("保存标定结果", EnumLogType.Debug);
            CDataModel model = new CDataModel()
            {
                A1 = ValueA1,
                B1 = ValueB1,
                C1 = ValueC1,
                A2 = ValueA2,
                B2 = ValueB2,
                C2 = ValueC2,
                CenterX = CenterX,
                CenterY = CenterY,
            };
            string filename = HalconIoMethod.GenCalibrationName(StrRecipeName);
            bool result = model.SaveCalibration(filename);
            if (result)
            {
                PrintLog("标定结果保存完成：" + filename, EnumLogType.Success);
            }
        }
        #endregion

        #region 4. 内部方法
        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            IsMarkering = false;
            CanRefreshImage = true;
        }
        private void DrawingBorder_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            Point curPoint = e.GetPosition(e.Device.Target);
            Point point = Halcon.ControlPointToHImagePoint(curPoint.X, curPoint.Y);
            //////////////////////////////////////// 当前点信息
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

            // 平移
            if (CanMove)
            {
                // 图像窗口平移
                Halcon.HShiftWindowContents(PointMoveOri.X - curPoint.X, PointMoveOri.Y - curPoint.Y);
                // 更新起点
                PointMoveOri = curPoint;
            }
        }
        private void DrawingBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 加这句话 否则执行两次
            e.Handled = true;
            CanMove = true;
            PointMoveOri = e.GetPosition(e.Device.Target);
            Point curPoint = e.GetPosition(e.Device.Target);
            // 双击 图像还原
            if (e.ClickCount == 2)
            {
                Halcon.SetFullImagePart();
                return;
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
                return;
            }

            // 拾取 Marker 点
            if (Ho_Image.IsInitialized() && IsMarkering)
            {
                // 手动拾取 Marker：9 + 3
                int index = IntSelectedIndex;
                if (index < 0)
                {
                    return;
                }

                // 手动取点
                double row = 0;
                double col = 0;
                // 手动取点
                if (EnumGetMarkerSel == EnumGetMarkerMode.hand)
                {
                    Point pt = Halcon.ControlPointToHImagePoint(curPoint.X, curPoint.Y);
                    row = pt.Y;
                    col = pt.X;
                }
                // 模板取点
                else if (EnumGetMarkerSel == EnumGetMarkerMode.module)
                {
                    if (Hv_ShapeModelID.ToString().Length < 5)
                    {
                        PrintLog("未导入模板", EnumLogType.Warning);
                        return;
                    }
                    // 全图搜索 直接匹配
                    PrintLog("模板匹配", EnumLogType.Debug);
                    HOperatorSet.FindShapeModel(Ho_Image, Hv_ShapeModelID, MyFindShapeModelVM.NumSelectAngleStart, MyFindShapeModelVM.NumSelectAngleExtent,
                        MyFindShapeModelVM.NumMinScore, MyFindShapeModelVM.IntNumMatches, MyFindShapeModelVM.NumMaxOverlap, MyFindShapeModelVM.StrSelectSubPixel,
                        MyFindShapeModelVM.IntNumLevels, MyFindShapeModelVM.NumGreediness, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle, out HTuple hv_Score);
                    if (hv_Score.Length > 0 && hv_Score.D >= NumScore)
                    {
                        Ho_Window.ClearWindow();
                        Ho_Window.DispObj(Ho_Image);
                        // 获取模板轮廓
                        HOperatorSet.GetShapeModelContours(out HObject ho_ShapeModel, Hv_ShapeModelID, 1);
                        HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row.D, hv_Column.D, hv_Angle.D, out HTuple hv_HomMat2DRotate);
                        HOperatorSet.AffineTransContourXld(ho_ShapeModel, out HObject ho_ModelTrans, hv_HomMat2DRotate);
                        ho_ShapeModel.Dispose();
                        hv_HomMat2DRotate.Dispose();
                        Ho_Window.SetColor("orange red");
                        Ho_Window.SetLineWidth(3);
                        Ho_Window.DispObj(ho_ModelTrans);
                        ho_ModelTrans.Dispose();

                        // 匹配结果
                        MyFindShapeModelVM.NumMatchRow = hv_Row.D;
                        MyFindShapeModelVM.NumMatchCol = hv_Column.D;
                        MyFindShapeModelVM.NumMatchAngle = hv_Angle.D * 180 / Math.PI;
                        MyFindShapeModelVM.NumMatchScore = hv_Score.D;
                        row = hv_Row.D;
                        col = hv_Column.D;
                    }
                    else
                    {
                        PrintLog("模板匹配失败，建议调整模板参数", EnumLogType.Error);
                        CanMove = false;
                        return;
                    }
                }

                // 更新数据
                if (row * col > 0)
                {
                    if (index < 9)
                    {
                        Ho_Window.SetColor("orange red");
                        Ho_Window.DispCrossContour(row, col, CConstants.MarkerCrossLenght, 0);
                    }
                    else
                    {
                        Ho_Window.SetColor("green");
                        Ho_Window.DispCrossContour(row, col, CConstants.MarkerCrossLenght);
                    }

                    // 更新 Marker 点坐标
                    CDataModel model = new CDataModel(DataListCalMarker[index])
                    {
                        ImageX = col,
                        ImageY = row
                    };
                    DataListCalMarker[index] = new CDataModel(model);

                    // 继续下一个点
                    if (index < DataListCalMarker.Count - 1)
                    {
                        IntSelectedIndex = index + 1;
                        EnumRobotPositionNumber++;
                    }
                    else
                    {
                        IntSelectedIndex = -1;
                        EnumRobotPositionNumber = EnumRobotMovePoint.point_0;
                        IsMarkering = false;
                    }
                    PrintLog("取点完成", EnumLogType.Success);
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
        private void DrawingBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CanMove = false;
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
        }

        /// <summary>
        /// 自动标定
        /// </summary>
        private void CmdAutoRunCal()
        {
            while (true)
            {
                // 通讯
                if (ModbusManager.Instance.IsConnected)
                {
                    int value1000 = ModbusManager.Instance.ReadInt16(AddressMBS);
                    int value1001 = ModbusManager.Instance.ReadInt16(AddressMBS + 1);
                    int value1002 = ModbusManager.Instance.ReadInt16(AddressMBS + 2);
                    // 进入标定流程
                    if (value1000 == 1 && value1001 == 4 && value1002 == 1)
                    {
                        _ = ModbusManager.Instance.Write(AddressMBS, 0);
                        _ = ModbusManager.Instance.Write(AddressMBS + 1, 0);
                        _ = ModbusManager.Instance.Write(AddressMBS + 2, 0);
                        // 自动标定
                        _ = DispatcherHelper.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { RunCalibrationAuto(); });
                    }
                }
                Thread.Sleep(100);
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
        #endregion
    }
}