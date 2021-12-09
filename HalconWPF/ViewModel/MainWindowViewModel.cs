
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

        private int selectedIndex;
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
            string filename = @"..\HalconWPF\Resource\Halcon\" + name + ".txt";
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
            if (name == "AcquisitionImage")
            {
                _ = MainContent.Children.Add(new AcquisitionImage());
            }
            else if (name == "ImageReadSave")
            {
                _ = MainContent.Children.Add(new ImageReadSave());
            }
            else if (name == "ClipNumberAndAngle")
            {
                _ = MainContent.Children.Add(new ClipNumberAndAngle());
            }
            else if (name == "CircleFitting")
            {
                _ = MainContent.Children.Add(new CircleFitting());
            }
            else if (name == "PcbDefectDetection")
            {
                _ = MainContent.Children.Add(new PcbDefectDetection());
            }
            else if (name == "CalibrationWithPoints")
            {
                _ = MainContent.Children.Add(new CalibrationWithPoints());
            }
            else if (name == "BearingDefectDetection")
            {
                _ = MainContent.Children.Add(new BearingDefectDetection());
            }
            else if (name == "TeethDetection")
            {
                _ = MainContent.Children.Add(new TeethDetection());
            }
            else if (name == "BastingDefectDetection")
            {
                _ = MainContent.Children.Add(new BastingDefectDetection());
            }
            else if (name == "DomainModule")
            {
                _ = MainContent.Children.Add(new DomainModule());
            }
            else if (name == "CaliperCalibration")
            {
                _ = MainContent.Children.Add(new CaliperCalibration());
            }
            else if (name == "MeasureTools")
            {
                _ = MainContent.Children.Add(new MeasureTools());
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel()
        {
            SelectedIndex = -1;
            DataList = InitMethod.GetDataList();
        }
    }
}