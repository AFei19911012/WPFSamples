using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconWPF.Method;
using HalconWPF.Model;
using HalconWPF.UserControl;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WSlibs.Tools;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/8/18 23:30:04
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/8/18 23:30:04    Taosy.W                 
    ///
    public class MainWindowViewModel : ViewModelBase
    {
        // 控件
        private Grid MainContent;
        private TextEditor TextContainer;

        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get => selectedIndex;
            set => Set(ref selectedIndex, value);
        }

        private ObservableCollection<CDataModel> dataList;
        public ObservableCollection<CDataModel> DataList
        {
            get => dataList;
            set => Set(ref dataList, value);
        }

        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            MainContent = (e.Source as MainWindow).MainContent;
            TextContainer = (e.Source as MainWindow).TextContainer;
        }

        /// <summary>
        /// 加载源代码
        /// </summary>
        public RelayCommand<bool> CmdShowSourceCode => new Lazy<RelayCommand<bool>>(() => new RelayCommand<bool>(ShowSourceCode)).Value;
        private void ShowSourceCode(bool isChecked)
        {
            if (!isChecked || SelectedIndex < 0)
            {
                return;
            }
            string name = DataList[SelectedIndex].Name;
            string filename = @"..\HalconWPF\HalconCode\" + name + ".txt";
            if (File.Exists(filename))
            {
                TextContainer.Load(filename);
            }
            else
            {
                TextContainer.Clear();
            }
        }

        /// <summary>
        /// 切换功能页面
        /// </summary>
        public RelayCommand CmdSelectionChanged => new Lazy<RelayCommand>(() => new RelayCommand(SelectionChanged)).Value;
        private void SelectionChanged()
        {
            if (SelectedIndex < 0)
            {
                return;
            };
            MainContent.Children.Clear();
            string name = DataList[SelectedIndex].Name;
            if (name.Contains("九点标定工具"))
            {
                _ = MainContent.Children.Add(new CamCalibrationTool());
            }
            else if (name.Contains("模板匹配工具"))
            {
                _ = MainContent.Children.Add(new ShapeModuleTool());
            }
            else if (name.Contains("测量模型工具"))
            {
                _ = MainContent.Children.Add(new MetrolobyObjectMeasureTool());
            }
            else if (name.Contains("卡尺标定工具"))
            {
                _ = MainContent.Children.Add(new CaliperCalibrationTool());
            }
            else if (name.Contains("形状标定工具"))
            {
                _ = MainContent.Children.Add(new ShapeCalibrationTool());
            }
            else if (name.Contains("ROI工具"))
            {
                _ = MainContent.Children.Add(new ROITool());
            }
            else if (name.Contains("图像简易编程工具"))
            {
                _ = MainContent.Children.Add(new ImageProcessTool());
            }
            else if (name.Contains("图像裁剪工具"))
            {
                _ = MainContent.Children.Add(new CropImageTool());
            }
            else if (name.Contains("OCR分类工具"))
            {
                _ = MainContent.Children.Add(new OCRToolEpic());
            }
            else if (name.Contains("测量工具：长度、角度"))
            {
                _ = MainContent.Children.Add(new MeasureTools());
            }

            else if (name.Contains("图像采集：调用相机接口"))
            {
                _ = MainContent.Children.Add(new AcquisitionImage());
            }
            else if (name.Contains("读取本地图像、保存图像、保存窗体"))
            {
                _ = MainContent.Children.Add(new ImageReadSave());
            }
            else if (name.Contains("拟合圆"))
            {
                _ = MainContent.Children.Add(new CircleFitting());
            }
            else if (name.Contains("九点标定"))
            {
                _ = MainContent.Children.Add(new CalibrationWithPoints());
            }

            else if (name.Contains("计算别针数量和角度"))
            {
                _ = MainContent.Children.Add(new ClipNumberAndAngle());
            }
            else if (name.Contains("牙模切割"))
            {
                _ = MainContent.Children.Add(new TeethDetection());
            }

            else if (name.Contains("PCB板电路检测"))
            {
                _ = MainContent.Children.Add(new PcbDefectDetection());
            }
            else if (name.Contains("轴承滚子检测"))
            {
                _ = MainContent.Children.Add(new BearingDefectDetection());
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel()
        {
            DataList = InitMethod.GetDataList();
        }
    }
}