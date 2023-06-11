using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wpf_Base.CcdWpf;
using Wpf_Base.CommunicationWpf;
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
    /// Created Time: 22/09/03 18:28:21
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 18:28:21    CoderMan/CoderdMan1012         首次编写         
    ///
    public class OcrToolVM : ViewModelBase
    {
        #region 1. 绑定变量
        private int intSelectOCR = 0;
        public int IntSelectOCR
        {
            get => intSelectOCR;
            set => Set(ref intSelectOCR, value);
        }

        private int intSelectImage = 0;
        public int IntSelectImage
        {
            get => intSelectImage;
            set => Set(ref intSelectImage, value);
        }

        private ObservableCollection<CDataModel> listImageFiles = new ObservableCollection<CDataModel>();
        public ObservableCollection<CDataModel> ListImageFiles
        {
            get => listImageFiles;
            set => Set(ref listImageFiles, value);
        }

        private string strRecipeName = "default";
        public string StrRecipeName
        {
            get => strRecipeName;
            set => Set(ref strRecipeName, value);
        }

        private string strTcpFormat = "功能码,相机编号,返回状态,结果; (1,0,1,01234;)";
        public string StrTcpFormat
        {
            get => strTcpFormat;
            set => Set(ref strTcpFormat, value);
        }

        private string strOCRContent = "null";
        public string StrOCRContent
        {
            get => strOCRContent;
            set => Set(ref strOCRContent, value);
        }
        #endregion

        #region 2. 全局变量
        private HalconWindowVM MyHalconWindowVM { get; set; }
        private OcrVM MyOcrVM { get; set; }
        private HTuple Hv_OCRHandle = new HTuple();

        /// <summary>
        /// 项目参数
        /// </summary>
        private COcrProjectParams ProjectParams { get; set; }
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


        public OcrToolVM()
        {
            // 主程序连接一次
            // TCP 客户端
            Thread.Sleep(10);
            if (!TcpClientManager.Instance.IsConnected)
            {
                TcpClientManager.Instance.Connect();
            }
            // 注册消息事件，可以多个
            TcpClientManager.Instance.MessageRecievedEvent += (sender, e) => { MessageRecievedEvent(); };
        }

        #region 3. 绑定命令
        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            MyOcrVM = (e.Source as OcrTool).MyOcrControl.DataContext as OcrVM;
            MyHalconWindowVM = (e.Source as OcrTool).MyHalconWindowControl.DataContext as HalconWindowVM;
        }

        /// <summary>
        /// 加载训练集
        /// </summary>
        public RelayCommand CmdLoadTrainImages => new Lazy<RelayCommand>(() => new RelayCommand(LoadTrainImages)).Value;
        private void LoadTrainImages()
        {
            try
            {
                _ = DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
                {
                    PrintLog("加载训练图像数据集", EnumLogType.Debug);
                    OpenFileDialog dialog = new OpenFileDialog
                    {
                        Title = "选择图片",
                        Filter = "图像文件(*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp",
                        RestoreDirectory = true,
                        Multiselect = true,
                    };
                    if (dialog.ShowDialog() != true)
                    {
                        PrintLog("取消加载训练图像数据集", EnumLogType.Warning);
                        return;
                    }
                    string[] filenames = dialog.FileNames;
                    // 显示到列表
                    ListImageFiles.Clear();
                    for (int i = 0; i < filenames.Length; i++)
                    {
                        string train_char = Path.GetFileName(filenames[i]).Split('_')[0];
                        ListImageFiles.Add(new CDataModel { Name = train_char, Remark = filenames[i] });
                    }
                    // 图像处理
                    PrintLog("图像预处理", EnumLogType.Debug);
                    string training_filename = HalconIoMethod.GenOcrTrainingFileName(StrRecipeName);
                    for (int i = 0; i < filenames.Length; i++)
                    {
                        // 预处理
                        MyHalconWindowVM.Ho_Image.Dispose();
                        HOperatorSet.ReadImage(out MyHalconWindowVM.Ho_Image, filenames[i]);
                        HOperatorSet.MedianImage(MyHalconWindowVM.Ho_Image, out HObject ho_ImageMedian, "circle", 1, "mirrored");
                        HOperatorSet.BinaryThreshold(ho_ImageMedian, out HObject ho_Region, "max_separability", "light", out _);
                        ho_ImageMedian.Dispose();
                        HOperatorSet.FillUpShape(ho_Region, out HObject ho_RegionFillUp, "area", 1, 5);
                        ho_Region.Dispose();
                        HOperatorSet.Connection(ho_RegionFillUp, out ho_Region);
                        ho_RegionFillUp.Dispose();
                        // 筛选
                        HTuple hv_Features = new HTuple();
                        HTuple hv_Min = new HTuple();
                        HTuple hv_Max = new HTuple();
                        hv_Features[0] = "height";
                        hv_Features[1] = "width";
                        // 获取参数
                        hv_Min[0] = MyOcrVM.MinCharHeight;
                        hv_Min[1] = MyOcrVM.MinCharWidth;
                        hv_Max[0] = 9999;
                        hv_Max[1] = 9999;
                        HOperatorSet.SelectShape(ho_Region, out HObject ho_SelectedRegions, hv_Features, "and", hv_Min, hv_Max);
                        ho_Region.Dispose();
                        HOperatorSet.Union1(ho_SelectedRegions, out HObject ho_Number);
                        ho_SelectedRegions.Dispose();

                        // 保存到训练文件里
                        if (i == 0)
                        {
                            HOperatorSet.WriteOcrTrainf(ho_Number, MyHalconWindowVM.Ho_Image, ListImageFiles[i].Name, training_filename);
                        }
                        else
                        {
                            HOperatorSet.AppendOcrTrainf(ho_Number, MyHalconWindowVM.Ho_Image, ListImageFiles[i].Name, training_filename);
                        }
                        ho_Number.Dispose();

                        // 显示
                        MyHalconWindowVM.ClearWindow();
                        MyHalconWindowVM.DispHImage();
                        if (MyHalconWindowVM.IsFirstShow)
                        {
                            MyHalconWindowVM.IsFirstShow = false;
                            MyHalconWindowVM.Halcon.SetFullImagePart();
                            MyHalconWindowVM.Ho_Window.SetLineWidth(2);
                            MyHalconWindowVM.Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                        }
                        Thread.Sleep(10);
                    }
                    PrintLog("图像数据集加载完成", EnumLogType.Success);
                }));
            }
            catch (Exception ex)
            {
                PrintLog("字符检测异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 查看训练图像
        /// </summary>
        public RelayCommand CmdViewImage => new Lazy<RelayCommand>(() => new RelayCommand(ViewImage)).Value;
        public void ViewImage()
        {
            try
            {
                if (IntSelectImage < 0)
                {
                    return;
                }
                string training_filename = HalconIoMethod.GenOcrTrainingFileName(StrRecipeName);
                if (File.Exists(training_filename))
                {
                    HOperatorSet.ReadOcrTrainf(out HObject ho_Characters, training_filename, out HTuple hv_CharacterNames);
                    int count = ho_Characters.CountObj();
                    if (count == ListImageFiles.Count)
                    {
                        HOperatorSet.SelectObj(ho_Characters, out HObject ho_CharacterSelected, IntSelectImage + 1);
                        MyHalconWindowVM.ClearWindow();
                        MyHalconWindowVM.DispHObject(ho_CharacterSelected);
                        MyHalconWindowVM.Ho_Window.SetDisplayFont(24);
                        MyHalconWindowVM.Ho_Window.DispText(hv_CharacterNames[IntSelectImage], 10, 10);
                    }
                    else
                    {
                        return;
                    }
                    ho_Characters.Dispose();
                }
                else
                {
                    PrintLog("训练集文件丢失：" + training_filename, EnumLogType.Warning);
                }
            }
            catch (Exception ex)
            {
                PrintLog("字符检测异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 执行训练
        /// </summary>
        public RelayCommand CmdRunTrain => new Lazy<RelayCommand>(() => new RelayCommand(RunTrain)).Value;
        private void RunTrain()
        {
            try
            {
                PrintLog("开始 OCR 训练", EnumLogType.Debug);
                string training_filename = HalconIoMethod.GenOcrTrainingFileName(StrRecipeName);
                if (File.Exists(training_filename))
                {
                    HOperatorSet.ReadOcrTrainfNames(training_filename, out HTuple hv_CharacterNames, out HTuple hv_CharacterCount);
                    // MLP 多层感知器
                    Hv_OCRHandle.Dispose();
                    if (IntSelectOCR == 0)
                    {
                        HOperatorSet.CreateOcrClassMlp(8, 10, "constant", "default", hv_CharacterNames, 80, "none", 10, 42, out Hv_OCRHandle);
                        HOperatorSet.TrainfOcrClassMlp(Hv_OCRHandle, training_filename, 200, 1, 0.01, out _, out _);
                    }
                    // SVM 支持向量机
                    else if (IntSelectOCR == 1)
                    {
                        HOperatorSet.CreateOcrClassSvm(8, 10, "constant", "default", hv_CharacterNames, "rbf", 0.02, 0.05, "one-versus-one", "normalization", 10, out Hv_OCRHandle);
                        HOperatorSet.TrainfOcrClassSvm(Hv_OCRHandle, training_filename, 0.001, "default");
                    }
                    // KNN k-近邻
                    else if (IntSelectOCR == 2)
                    {
                        HOperatorSet.CreateOcrClassKnn(8, 10, "constant", "default", hv_CharacterNames, new HTuple(), new HTuple(), out Hv_OCRHandle);
                        HOperatorSet.TrainfOcrClassKnn(Hv_OCRHandle, training_filename, new HTuple(), new HTuple());
                    }
                    WriteOCRHandleToFile();
                    PrintLog("OCR 训练完成", EnumLogType.Success);
                }
                else
                {
                    PrintLog("训练集文件丢失：" + training_filename, EnumLogType.Warning);
                }
            }
            catch (Exception ex)
            {
                PrintLog("字符检测异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 加载 OCR
        /// </summary>
        public RelayCommand CmdLoadOCR => new Lazy<RelayCommand>(() => new RelayCommand(LoadOCR)).Value;
        private void LoadOCR()
        {
            try
            {
                PrintLog("加载 OCR 模型", EnumLogType.Debug);
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "加载 OCR 模型",
                    //Filter = "OCR 模型文件(*.omc;*.osc;*.onc)|*.omc;*.osc;*.onc",
                    RestoreDirectory = true,
                    InitialDirectory = Environment.CurrentDirectory + "\\Module",
                };
                if (IntSelectOCR == 0)
                {
                    dialog.Filter = "OCR 模型文件(*.omc)|*.omc";
                }
                else if (IntSelectOCR == 1)
                {
                    dialog.Filter = "OCR 模型文件(*.osc)|*.osc";
                }
                else if (IntSelectOCR == 2)
                {
                    dialog.Filter = "OCR 模型文件(*.onc)|*.onc";
                }

                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                string filename = dialog.FileName;
                if (File.Exists(filename))
                {
                    StrRecipeName = Path.GetFileNameWithoutExtension(filename).Remove(0, 4);
                    ReadOCRHandleFromFile(filename);
                }
                else
                {
                    PrintLog("OCR 模型文件缺失：" + filename, EnumLogType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                PrintLog("字符检测异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// OCR 字符识别
        /// </summary>
        public RelayCommand CmdRunOCR => new Lazy<RelayCommand>(() => new RelayCommand(RunOCR)).Value;
        private void RunOCR()
        {
            try
            {
                if (MyHalconWindowVM.CanRefreshImage)
                {
                    GetCurrentImage();

                    if (!MyHalconWindowVM.Ho_Image.IsInitialized())
                    {
                        PrintLog("相机抓图失败", EnumLogType.Error);
                        return;
                    }
                    PrintLog("相机抓图完成", EnumLogType.Success);
                }

                if (!MyHalconWindowVM.Ho_Image.IsInitialized())
                {
                    PrintLog("图像无效", EnumLogType.Warning);
                    return;
                }
                PrintLog("启动 OCR", EnumLogType.Debug);
                // 加载 OCR 模型
                string filename = HalconIoMethod.GenOcrModelFileName(StrRecipeName, IntSelectOCR);
                ReadOCRHandleFromFile(filename);
                if (Hv_OCRHandle.ToString().Length < 5)
                {
                    PrintLog("必要的文件缺失: " + filename, EnumLogType.Warning);
                    return;
                }
                // 字符识别
                OCR();
                // 显示结果
                MyHalconWindowVM.Ho_Window.ClearWindow();
                MyHalconWindowVM.DispHImage();
                MyHalconWindowVM.Ho_Window.DispRectangleContour(MyOcrVM.NumRow1, MyOcrVM.NumCol1, MyOcrVM.NumRow2, MyOcrVM.NumCol2);
                MyHalconWindowVM.Ho_Window.SetDisplayFont(24);
                MyHalconWindowVM.Ho_Window.DispText(StrOCRContent, 10, 10);
                PrintLog("字符识别完成", EnumLogType.Success);
            }
            catch (Exception ex)
            {
                PrintLog("字符检测异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 加载项目
        /// </summary>
        public RelayCommand CmdLoadProject => new Lazy<RelayCommand>(() => new RelayCommand(LoadProject)).Value;
        private void LoadProject()
        {
            PrintLog("加载项目", EnumLogType.Debug);
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "加载项目文件",
                Filter = "项目文件(*.ocrProj)|*.ocrProj",
                RestoreDirectory = true,
                InitialDirectory = Environment.CurrentDirectory + "\\Task",
            };
            if (dialog.ShowDialog() != true)
            {
                PrintLog("取消加载项目", EnumLogType.Warning);
                return;
            }
            string filenameProject = dialog.FileName;

            Task task = Task.Run(() =>
            {
                // 项目参数
                ProjectParams = new COcrProjectParams();
                using (FileStream stream = new FileStream(filenameProject, FileMode.Open))
                {
                    BinaryFormatter format = new BinaryFormatter();
                    ProjectParams = format.Deserialize(stream) as COcrProjectParams;
                    stream.Close();
                }
                // 设置参数
                StrRecipeName = ProjectParams.StrRecipeName;
                IntSelectOCR = ProjectParams.IntSelectOCR;
                MyOcrVM.MinCharHeight = ProjectParams.MinCharHeight;
                MyOcrVM.MinCharWidth = ProjectParams.MinCharWidth;
                MyOcrVM.NumRow1 = ProjectParams.NumRow1;
                MyOcrVM.NumRow2 = ProjectParams.NumRow2;
                MyOcrVM.NumCol1 = ProjectParams.NumCol1;
                MyOcrVM.NumCol2 = ProjectParams.NumCol2;

                // 加载模板、模型、OCR
                try
                {
                    // 加载 OCR 模型
                    string filename = HalconIoMethod.GenOcrModelFileName(StrRecipeName, IntSelectOCR);
                    ReadOCRHandleFromFile(filename);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    PrintLog("必要文件读取异常", EnumLogType.Error);
                    return;
                }
                PrintLog("项目加载完成：" + filenameProject, EnumLogType.Success);
            });
            Task.WaitAll(task);
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        public RelayCommand CmdSaveProject => new Lazy<RelayCommand>(() => new RelayCommand(SaveProject)).Value;
        private void SaveProject()
        {
            PrintLog("保存项目", EnumLogType.Debug);
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "保存项目",
                Filter = "项目文件(*.ocrProj)|*.ocrProj",
                RestoreDirectory = true,
                FileName = StrRecipeName + ".ocrProj",
                InitialDirectory = Environment.CurrentDirectory + "\\Task",
            };
            if (dialog.ShowDialog() != true)
            {
                PrintLog("取消保存项目", EnumLogType.Warning);
                return;
            }
            string filename = dialog.FileName;

            Task task = Task.Run(() =>
            {
                // 项目参数保存
                ProjectParams = new COcrProjectParams
                {
                    IntSelectOCR = IntSelectOCR,
                    MinCharHeight = MyOcrVM.MinCharHeight,
                    MinCharWidth = MyOcrVM.MinCharWidth,
                    NumRow1 = MyOcrVM.NumRow1,
                    NumRow2 = MyOcrVM.NumRow2,
                    NumCol1 = MyOcrVM.NumCol1,
                    NumCol2 = MyOcrVM.NumCol2,
                    StrRecipeName = StrRecipeName,
                };

                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    BinaryFormatter format = new BinaryFormatter();
                    format.Serialize(stream, ProjectParams);
                    stream.Close();
                }
                PrintLog("项目保存完成：" + filename, EnumLogType.Success);
            });
            Task.WaitAll(task);
        }
        #endregion

        #region 4. 内部方法     
        /// <summary>
        /// 获取当前图像
        /// </summary>
        private void GetCurrentImage()
        {
            // 禁止刷新图像
            MyHalconWindowVM.CanRefreshImage = false;
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
                MyHalconWindowVM.Ho_Image.Dispose();
                // 触发模式
                if (CcdManager.Instance.HikCamInfos[CcdManager.Instance.CurrentCamId].TriggerMode == EnumCaptureMode.Trig)
                {
                    PrintLog("触发拍照指令", EnumLogType.Debug);
                    _ = CcdManager.Instance.TrigCamBySoft(CcdManager.Instance.CurrentCamId);
                }
                CcdManager.Instance.GetHalconImage(CcdManager.Instance.CurrentCamId, ref MyHalconWindowVM.Ho_Image);
                if (MyHalconWindowVM.Ho_Image.IsInitialized())
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
        /// 保存 OCR 模型
        /// </summary>
        private void WriteOCRHandleToFile()
        {
            string ocr_filename = HalconIoMethod.GenOcrModelFileName(StrRecipeName, IntSelectOCR);
            if (Hv_OCRHandle.TupleIsValidHandle())
            {
                if (ocr_filename.Contains(".omc"))
                {
                    HOperatorSet.WriteOcrClassMlp(Hv_OCRHandle, ocr_filename);
                }
                else if (ocr_filename.Contains(".osc"))
                {
                    HOperatorSet.WriteOcrClassSvm(Hv_OCRHandle, ocr_filename);
                }
                else if (ocr_filename.Contains(".onc"))
                {
                    HOperatorSet.WriteOcrClassKnn(Hv_OCRHandle, ocr_filename);
                }
                PrintLog("OCR 模型保存完成：" + ocr_filename, EnumLogType.Success);
            }
            else
            {
                PrintLog("无效 OCR 句柄", EnumLogType.Error);
            }
        }

        /// <summary>
        /// 加载 OCR 模型
        /// </summary>
        /// <param name="ocr_filename"></param>
        private void ReadOCRHandleFromFile(string ocr_filename)
        {
            if (File.Exists(ocr_filename))
            {
                Hv_OCRHandle.Dispose();
                if (ocr_filename.Contains(".omc"))
                {
                    HOperatorSet.ReadOcrClassMlp(ocr_filename, out Hv_OCRHandle);
                }
                else if (ocr_filename.Contains(".osc"))
                {
                    HOperatorSet.ReadOcrClassSvm(ocr_filename, out Hv_OCRHandle);
                }
                else if (ocr_filename.Contains(".onc"))
                {
                    HOperatorSet.ReadOcrClassKnn(ocr_filename, out Hv_OCRHandle);
                }
                PrintLog("OCR 模型加载完成：" + ocr_filename, EnumLogType.Success);
            }
            else
            {
                PrintLog("文件缺失：" + ocr_filename, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 字符识别
        /// </summary>
        /// <param name="ho_Image"></param>
        private void OCR()
        {
            PrintLog("确定字符 ROI", EnumLogType.Debug);
            HOperatorSet.GenRectangle1(out HObject hv_Rectangle, MyOcrVM.NumRow1, MyOcrVM.NumCol1, MyOcrVM.NumRow2, MyOcrVM.NumCol2);
            HOperatorSet.ReduceDomain(MyHalconWindowVM.Ho_Image, hv_Rectangle, out HObject ho_ImageReduced);
            hv_Rectangle.Dispose();
            PrintLog("字符识别", EnumLogType.Debug);
            if (Hv_OCRHandle.ToString().Length > 5)
            {
                HOperatorSet.BinaryThreshold(ho_ImageReduced, out HObject ho_Region, "max_separability", "light", out _);
                HOperatorSet.FillUpShape(ho_Region, out HObject ho_RegionFillUp, "area", 1, 5);
                ho_Region.Dispose();
                // 处理上下分离
                HOperatorSet.ClosingRectangle1(ho_RegionFillUp, out HObject ho_RegionClosing, 1, 3);
                ho_RegionFillUp.Dispose();
                // 处理一下连字符
                HOperatorSet.OpeningRectangle1(ho_RegionClosing, out ho_Region, 3, 1);
                ho_RegionClosing.Dispose();
                HOperatorSet.Connection(ho_Region, out HObject ho_Regions);
                ho_Region.Dispose();
                // 筛选
                HTuple hv_Features = new HTuple();
                HTuple hv_Min = new HTuple();
                HTuple hv_Max = new HTuple();
                hv_Features[0] = "height";
                hv_Features[1] = "width";
                hv_Min[0] = MyOcrVM.MinCharHeight;
                hv_Min[1] = MyOcrVM.MinCharWidth;
                hv_Max[0] = 9999;
                hv_Max[1] = 9999;
                HOperatorSet.SelectShape(ho_Regions, out HObject ho_SelectedRegions, hv_Features, "and", hv_Min, hv_Max);
                ho_Regions.Dispose();
                HOperatorSet.SortRegion(ho_SelectedRegions, out ho_Region, "character", "true", "row");
                ho_SelectedRegions.Dispose();
                HTuple hv_Class = new HTuple();
                if (IntSelectOCR == 0)
                {
                    HOperatorSet.DoOcrMultiClassMlp(ho_Region, ho_ImageReduced, Hv_OCRHandle, out hv_Class, out _);
                }
                else if (IntSelectOCR == 1)
                {
                    HOperatorSet.DoOcrMultiClassSvm(ho_Region, ho_ImageReduced, Hv_OCRHandle, out hv_Class);
                }
                else if (IntSelectOCR == 2)
                {
                    HOperatorSet.DoOcrMultiClassKnn(ho_Region, ho_ImageReduced, Hv_OCRHandle, out hv_Class, out _);
                }
                ho_Region.Dispose();
                // 结果
                StrOCRContent = "";
                for (int i = 0; i < hv_Class.Length; i++)
                {
                    StrOCRContent += hv_Class[i];
                }
            }
            else
            {
                PrintLog("未加载 OCR 模型", EnumLogType.Warning);
            }
            ho_ImageReduced.Dispose();
        }

        private void GetOCR()
        {
            try
            {
                StrOCRContent = "";
                try
                {
                    GetCurrentImage();

                    if (!MyHalconWindowVM.Ho_Image.IsInitialized())
                    {
                        PrintLog("相机抓图失败", EnumLogType.Error);
                        return;
                    }
                    PrintLog("相机抓图完成", EnumLogType.Debug);
                }
                catch (Exception ex)
                {
                    PrintLog("相机抓图异常：" + ex.Message, EnumLogType.Error);
                    return;
                }
                RunOCR();
                // 显示结果
                MyHalconWindowVM.ClearWindow();
                MyHalconWindowVM.DispHImage();
                if (MyHalconWindowVM.IsFirstShow)
                {
                    MyHalconWindowVM.IsFirstShow = false;
                    MyHalconWindowVM.Ho_Window.SetColor(EnumHalColor.orange_red.ToColorString());
                    MyHalconWindowVM.Ho_Window.SetLineWidth(3);
                    _ = DispatcherHelper.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { MyHalconWindowVM.Halcon.SetFullImagePart(); });
                }

                PrintLog("字符识别完成", EnumLogType.Success);
                if (StrOCRContent == "")
                {
                    StrOCRContent = "null";
                    PrintLog("未检测到字符", EnumLogType.Warning);
                }
                MyHalconWindowVM.Ho_Window.SetDisplayFont(24);
                MyHalconWindowVM.Ho_Window.DispText(StrOCRContent, 10, 10);

                // 发送检测结果
                if (TcpClientManager.Instance.IsConnected)
                {
                    TcpClientManager.Instance.Write(StrTcpFormat.Replace("ocr", StrOCRContent));
                    PrintLog("字符发送完成", EnumLogType.Success);
                }
            }
            catch (Exception ex)
            {
                PrintLog("OCR 异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="msg"></param>
        private void MessageRecievedEvent()
        {
            if (MyHalconWindowVM.Ho_Window == null)
            {
                return;
            }
            string receiveMsg = TcpClientManager.Instance.RecMessage;
            if (receiveMsg.StartsWith("1,0") || receiveMsg.StartsWith("1,1") || receiveMsg.StartsWith("1,2") || receiveMsg.StartsWith("1,3"))
            {
                try
                {
                    int camId = int.Parse(receiveMsg.Split(';')[0].Split(',')[1]);
                    CcdManager.Instance.CurrentCamId = camId;
                    Task task = new Task(() =>
                    {
                        _ = DispatcherHelper.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { GetOCR(); });
                    });
                    task.Start();
                }
                catch (Exception ex)
                {
                    PrintLog("异常：" + ex.Message, EnumLogType.Error);
                }
            }
        }
        #endregion
    }
}