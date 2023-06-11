using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using TestUnit.Demo;
using TestUnit.Model;
using WpfControlsX.ControlX;
using MessageBox = WpfControlsX.ControlX.MessageBox;

namespace TestUnit.ViewModel
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/2/22 17:03:34
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/2/22 17:03:34                     BigWang         首次编写         
    ///
    public class MainVM : ViewModelBase
    {

        #region 绑定变量区

        private string _SomeText = "TextBlock Click Test";
        public string SomeText
        {
            get => _SomeText;
            set => Set(ref _SomeText, value);
        }

        private string _StrTitle = "Title On Left";
        public string StrTitle
        {
            get => _StrTitle;
            set => Set(ref _StrTitle, value);
        }

        private double _NumWidth = double.NaN;
        public double NumWidth
        {
            get => _NumWidth;
            set => Set(ref _NumWidth, value);
        }

        private string _StrText;
        public string StrText
        {
            get => _StrText;
            set => Set(ref _StrText, value);
        }

        private int _IntAuthority = 1;
        public int IntAuthority
        {
            get => _IntAuthority;
            set => Set(ref _IntAuthority, value);
        }

        private int _IntAuthorityMin = 1;
        public int IntAuthorityMin
        {
            get => _IntAuthorityMin;
            set => Set(ref _IntAuthorityMin, value);
        }


        private bool _BoolShowClearButton = true;
        public bool BoolShowClearButton
        {
            get => _BoolShowClearButton;
            set => Set(ref _BoolShowClearButton, value);
        }

        private bool _BoolShowWaterMark = true;
        public bool BoolShowWaterMark
        {
            get => _BoolShowWaterMark;
            set => Set(ref _BoolShowWaterMark, value);
        }

        private string _StrWaterMark = "This is WaterMark";
        public string StrWaterMark
        {
            get => _StrWaterMark;
            set => Set(ref _StrWaterMark, value);
        }


        private int _IntBorderThickness = 1;
        public int IntBorderThickness
        {
            get => _IntBorderThickness;
            set
            {
                _ = Set(ref _IntBorderThickness, value);

                int flag = IntBorderThickness % 2;
                ThicknessBorder = flag == 1 ? new Thickness(0, 0, 0, 1) : new Thickness(1);
            }
        }

        private Thickness _ThicknessBorder = new Thickness(0, 0, 0, 1);
        public Thickness ThicknessBorder
        {
            get => _ThicknessBorder;
            set => Set(ref _ThicknessBorder, value);
        }

        private ObservableCollection<DataModel> _ListDataModel;
        public ObservableCollection<DataModel> ListDataModel
        {
            get => _ListDataModel;
            set => Set(ref _ListDataModel, value);
        }

        private ObservableCollection<WxComboBoxBaseData> _WxMultiComboBoxListData;
        public ObservableCollection<WxComboBoxBaseData> WxMultiComboBoxListData
        {
            get => _WxMultiComboBoxListData;
            set => Set(ref _WxMultiComboBoxListData, value);
        }


        private double _Percent = 0.25;
        public double Percent
        {
            get => _Percent;
            set => Set(ref _Percent, value);
        }


        private string _StrFolder;
        public string StrFolder
        {
            get => _StrFolder;
            set => Set(ref _StrFolder, value);
        }

        private bool _StaysOpen;
        public bool StaysOpen
        {
            get => _StaysOpen;
            set => Set(ref _StaysOpen, value);
        }


        private int _IntProgress = 1;
        public int IntProgress
        {
            get => _IntProgress;
            set => Set(ref _IntProgress, value);
        }

        private ObservableCollection<string> _ListSteps;
        public ObservableCollection<string> ListSteps
        {
            get => _ListSteps;
            set => Set(ref _ListSteps, value);
        }

        private ObservableCollection<DataModel> dataList;
        public ObservableCollection<DataModel> DataList
        {
            get => dataList;
            set => Set(ref dataList, value);
        }

        private ObservableCollection<DataModel> dataListCoverView;
        public ObservableCollection<DataModel> DataListCoverView
        {
            get => dataListCoverView;
            set => Set(ref dataListCoverView, value);
        }

        private string _ImgPath;
        public string ImgPath
        {
            get => _ImgPath;
            set => Set(ref _ImgPath, value);
        }

        private double _ScaleXY = 5;
        public double ScaleXY
        {
            get => _ScaleXY;
            set => Set(ref _ScaleXY, value);
        }

        #endregion

        #region 定义绑定命令区
        public RelayCommand<string> SomeCommand => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(DoSomething)).Value;

        public RelayCommand OpenCmd => new Lazy<RelayCommand>(() => new RelayCommand(OpenNotification)).Value;

        public RelayCommand<object> PrevCommand => new Lazy<RelayCommand<object>>(() => new RelayCommand<object>(Prev)).Value;

        public RelayCommand<object> NextCommand => new Lazy<RelayCommand<object>>(() => new RelayCommand<object>(Next)).Value;

        public RelayCommand<string> DrawMenuCommand => new Lazy<RelayCommand<string>>(() => new RelayCommand<string>(DrawMenu)).Value;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainVM()
        {
            ListDataModel = new ObservableCollection<DataModel>
            {
                new DataModel { ImagePath = "pack://application:,,,/Image/ProfilePicture.jpg", Name = "WpfControlsX", Text = "Badminton",
                    TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), Children = new ObservableCollection<DataModel>{ new DataModel { Name = "Node 1.1"} } },

                new DataModel { ImagePath = "pack://application:,,,/Image/Archery.ico", Name = "WpfControlsX", Text = "Archery", 
                    TimeStr = DateTime.Now.ToString("HH:mm"), ShowIcon = true, IsChecked = true },

                new DataModel { ImagePath = "pack://application:,,,/Image/Artistic Gymnastics.ico", Name = "WpfControlsX", Text = "Artistic Gymnastics",
                    TimeStr = "昨天", IsChecked = true, Children = new ObservableCollection<DataModel>{ new DataModel { Name = "Node 2.1"}, new DataModel { Name = "Node 2.2"} } },

                new DataModel { ImagePath = "pack://application:,,,/Image/Athletics.ico", Name = "WpfControlsX", Text = "Athletics", TimeStr = "星期天", ShowIcon = true, },
                new DataModel { ImagePath = "pack://application:,,,/Image/Badminton.ico", Name = "WpfControlsX", Text = "Badminton", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Baseball.ico", Name = "WpfControlsX", Text = "Baseball", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Basketball.ico", Name = "WpfControlsX", Text = "Basketball", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Beach Volleyball.ico", Name = "WpfControlsX", Text = "Beach Volleyball", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Boxing.ico", Name = "WpfControlsX", Text = "Boxing", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Canoe Kayak Flatwater.ico", Name = "WpfControlsX", Text = "Canoe Kayak Flatwater", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Canoe Kayak Slalom.ico", Name = "WpfControlsX", Text = "Canoe Kayak Slalom", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Cycling.ico", Name = "WpfControlsX", Text = "Cycling", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Diving.ico", Name = "WpfControlsX", Text = "Diving", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Equestrian.ico", Name = "WpfControlsX", Text = "Equestrian", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Fencing.ico", Name = "WpfControlsX", Text = "Fencing", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Football.ico", Name = "WpfControlsX", Text = "Football", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Handball.ico", Name = "WpfControlsX", Text = "Handball", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Hockey.ico", Name = "WpfControlsX", Text = "Hockey", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Judo.ico", Name = "WpfControlsX", Text = "Judo", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Modern Pentathlon.ico", Name = "WpfControlsX", Text = "Modern Pentathlon", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Rhythmic Gymnastics.ico", Name = "WpfControlsX", Text = "Rhythmic Gymnastics", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Rowing.ico", Name = "WpfControlsX", Text = "Rowing", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
                new DataModel { ImagePath = "pack://application:,,,/Image/Sailing.ico", Name = "WpfControlsX", Text = "Sailing", TimeStr = DateTime.Now.ToString("yyyy/MM/dd"), },
            };

            WxMultiComboBoxListData = new ObservableCollection<WxComboBoxBaseData>()
            {
                new WxComboBoxBaseData{ ID=0, Name="包包", IsChecked=true },
                new WxComboBoxBaseData{ ID=1, Name="馒头" },
                new WxComboBoxBaseData{ ID=2, Name="贝儿" },
                new WxComboBoxBaseData{ ID=3, Name="熊熊" },
                new WxComboBoxBaseData{ ID=4, Name="羊羊" },
            };

            _ = Task.Run(() =>
            {
                while (true)
                {
                    Percent += 0.01;
                    if (Percent > 0.99)
                    {
                        Percent = 0;
                    }
                    Thread.Sleep(10);
                }
            });

            ListSteps = new ObservableCollection<string>
            {
                "Step 1",
                "Step 2",
                "Step 3",
                "Step 4"
            };

            DataList = new ObservableCollection<DataModel>
            {
                new DataModel{ Header = "Header1", Content = "pack://application:,,,/Image/1.jpg", Footer = "information" },
                new DataModel{ Header = "Header2", Content = "pack://application:,,,/Image/2.jpg", Footer = "information" },
                new DataModel{ Header = "Header3", Content = "pack://application:,,,/Image/3.jpg", Footer = "information" },
                new DataModel{ Header = "Header4", Content = "pack://application:,,,/Image/4.jpg", Footer = "information" },
                new DataModel{ Header = "Header5", Content = "pack://application:,,,/Image/5.jpg", Footer = "information" },
            };

            DataListCoverView = new ObservableCollection<DataModel>
            {
                new DataModel{ ImagePath = "pack://application:,,,/Image/01.jpg", Name = "This is Image 1" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/02.jpg", Name = "This is Image 2" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/03.jpg", Name = "This is Image 3" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/04.jpg", Name = "This is Image 4" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/05.jpg", Name = "This is Image 5" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/06.jpg", Name = "This is Image 6" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/07.jpg", Name = "This is Image 7" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/08.jpg", Name = "This is Image 8" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/09.jpg", Name = "This is Image 9" },
                new DataModel{ ImagePath = "pack://application:,,,/Image/10.jpg", Name = "This is Image 10" },
            };

            ImgPath = "pack://application:,,,/Image/1.jpg";
        }

        #region 绑定命令的具体实现区

        private void DoSomething(string txt)
        {
            SomeText = "MouseEvent: " + txt;
        }

        private void OpenNotification()
        {
            _ = WxNotification.Show(new AppNotification(), StaysOpen);
        }

        private void Prev(object sender)
        {
            if (sender is UniformGrid uniformGrid)
            {
                foreach (WxStepBar step in uniformGrid.Children.OfType<WxStepBar>())
                {
                    step.Prev();
                }
            }
        }

        private void Next(object sender)
        {
            if (sender is UniformGrid uniformGrid)
            {
                foreach (WxStepBar step in uniformGrid.Children.OfType<WxStepBar>())
                {
                    step.Next();
                }
            }
        }

        private void DrawMenu(string para)
        {
            _ = MessageBox.Show("点击页面：" + para);
        }
        #endregion
    }
}