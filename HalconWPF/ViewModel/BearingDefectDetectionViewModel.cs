using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HalconDotNet;
using HalconWPF.UserControl;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using HalconWPF.Method;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/10/13 10:26:36
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/10/13 10:26:36    Taosy.W                 
    ///
    public class BearingDefectDetectionViewModel : ViewModelBase
    {
        private string strOperation;
        public string StrOperation
        {
            get => strOperation;
            set => Set(ref strOperation, value);
        }

        private bool boolGood;
        public bool BoolGood
        {
            get => boolGood;
            set => Set(ref boolGood, value);
        }

        private HObject ho_Image = null;
        private HWindow ho_Window;
        private HWindow ho_WindowImage;
        private HSmartWindowControlWPF Halcon;
        private HSmartWindowControlWPF HalconImage;
        private Thread ho_thread;
        private readonly string StrOperationRun = "点我运行";
        private readonly string StrOperationStop = "点我停止";

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public BearingDefectDetectionViewModel()
        {
            StrOperation = StrOperationRun;
            BoolGood = false;
        }

        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as BearingDefectDetection).HalconWPF;
            HalconImage = (e.Source as BearingDefectDetection).HalconImage;
            ho_Window = Halcon.HalconWindow;
            ho_WindowImage = HalconImage.HalconWindow;
        }

        /// <summary>
        /// 运行检测
        /// </summary>
        public RelayCommand CmdRun => new Lazy<RelayCommand>(() => new RelayCommand(Run)).Value;
        private void Run()
        {
            // 运行
            if (StrOperation == StrOperationRun)
            {
                StrOperation = StrOperationStop;
                ho_thread = new Thread(ThreadRun)
                {
                    IsBackground = true
                };
                ho_thread.Start();
            }
            // 关闭
            else
            {
                StrOperation = StrOperationRun;
                ho_thread.Abort();
            }
        }


        private void ThreadRun()
        {
            DirectoryInfo folder = new DirectoryInfo(@"D:\MyPrograms\DataSet\halcon\轴承");
            if (folder.Exists)
            {
                FileInfo[] files = folder.GetFiles();
                while (true)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        HOperatorSet.ReadImage(out ho_Image, files[i].FullName);
                        BearingDetect();
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        /// <summary>
        /// 轴承检测
        /// </summary>
        private void BearingDetect()
        {
            // 初始化变量
            HOperatorSet.GenEmptyObj(out HObject ho_ImageMedian);
            HOperatorSet.GenEmptyObj(out HObject ho_GrayImage);
            HOperatorSet.GenEmptyObj(out HObject ho_Regions);
            HOperatorSet.GenEmptyObj(out HObject ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionsDifference);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionsRollers);
            HTuple hv_Area = new HTuple();
            HTuple hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();
            HTuple hv_Indices = new HTuple();

            // 检测
            try
            {
                // 第一步，识别轴承  尽量减少控制参数，自动计算
                ho_ImageMedian.Dispose();
                ho_GrayImage.Dispose();
                ho_Regions.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Indices.Dispose();
                ho_RegionFillUp.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionsDifference.Dispose();
                ho_Regions.Dispose();

                // 显示实时图像
                HOperatorSet.DispObj(ho_Image, ho_Window);
                // 在新的线程里用如下方式更新到界面
                _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    // 图像自适应显示
                    Halcon.SetFullImagePart();
                    BoolGood = false;
                }
                );

                // 清空
                HOperatorSet.ClearWindow(ho_WindowImage);
                HOperatorSet.MedianImage(ho_Image, out ho_ImageMedian, "circle", 10, "mirrored");
                HOperatorSet.Rgb1ToGray(ho_ImageMedian, out ho_GrayImage);
                HOperatorSet.AutoThreshold(ho_GrayImage, out ho_Regions, 10);
                HOperatorSet.AreaCenter(ho_Regions, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.TupleSortIndex(hv_Area, out hv_Indices);
                if (hv_Area.TupleLength() < 2)
                {
                    ho_ImageMedian.Dispose();
                    ho_GrayImage.Dispose();
                    ho_Regions.Dispose();
                    hv_Area.Dispose();
                    hv_Row.Dispose();
                    hv_Column.Dispose();
                    hv_Indices.Dispose();
                    return;
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SelectObj(ho_Regions, out ho_ObjectSelected, hv_Indices.TupleSelect(new HTuple(hv_Area.TupleLength()) - 2) + 1);
                }
                HOperatorSet.FillUp(ho_ObjectSelected, out ho_RegionFillUp);
                HOperatorSet.Difference(ho_RegionFillUp, ho_ObjectSelected, out ho_RegionsDifference);
                ho_Regions.Dispose();
                HOperatorSet.Connection(ho_RegionsDifference, out ho_Regions);

                // 第二步，识别滚子
                int NumberBearing = ho_Regions.CountObj();
                int[] NumberRoller = new int[NumberBearing];
                for (int i = 1; i <= NumberBearing; i++)
                {
                    ho_ObjectSelected.Dispose();
                    ho_RegionTrans.Dispose();
                    ho_RegionDifference.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_RegionsRollers.Dispose();
                    ho_RegionTrans.Dispose();
                    hv_Area.Dispose();
                    hv_Row.Dispose();
                    hv_Column.Dispose();
                    HOperatorSet.SelectObj(ho_Regions, out ho_ObjectSelected, i);
                    HOperatorSet.ShapeTrans(ho_ObjectSelected, out ho_RegionTrans, "convex");
                    HOperatorSet.Difference(ho_RegionTrans, ho_ObjectSelected, out ho_RegionDifference);
                    HOperatorSet.OpeningCircle(ho_RegionDifference, out ho_RegionOpening, 3);
                    HOperatorSet.Connection(ho_RegionOpening, out ho_RegionsRollers);
                    HOperatorSet.ShapeTrans(ho_RegionsRollers, out ho_RegionTrans, "outer_circle");
                    HOperatorSet.AreaCenter(ho_RegionTrans, out hv_Area, out hv_Row, out hv_Column);
                    double MaxArea = hv_Area.TupleMax();
                    ho_RegionsRollers.Dispose();
                    HOperatorSet.SelectShape(ho_RegionTrans, out ho_RegionsRollers, "area", "and", 0.3 * MaxArea, MaxArea + 1);
                    //// 显示滚子位置
                    //HOperatorSet.SetColored(ho_WindowImage, 12);
                    //HOperatorSet.DispObj(ho_RegionsRollers, ho_WindowImage);
                    NumberRoller[i - 1] = ho_RegionsRollers.CountObj();
                }

                // 第三步，判断异常
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                HOperatorSet.AreaCenter(ho_Regions, out hv_Area, out hv_Row, out hv_Column);
                double MeanNumber = NumberRoller.Mean();
                int numNG = 0;
                bool[] flagNG = new bool[NumberBearing];
                for (int i = 0; i < NumberBearing; i++)
                {
                    if (NumberRoller[i] < MeanNumber)
                    {
                        flagNG[i] = true;
                        numNG += 1;
                    }
                }

                // 第四步，显示异常
                if (numNG > 0)
                {
                    // 必须设置颜色
                    HOperatorSet.SetColor(ho_WindowImage, "red");
                    HOperatorSet.SetLineWidth(ho_WindowImage, 3);
                    HOperatorSet.DispObj(ho_Image, ho_WindowImage);

                    _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate ()
                    {
                        // 图像自适应显示
                        HalconImage.SetFullImagePart();
                    }
                    );

                    // 具体 NG 位置
                    for (int i = 0; i < NumberBearing; i++)
                    {
                        if (flagNG[i])
                        {
                            HOperatorSet.GenCrossContourXld(out HObject ho_Cross, hv_Row.TupleSelect(i), hv_Column.TupleSelect(i), 300, 0.785398);
                            HOperatorSet.DispObj(ho_Cross, ho_WindowImage);
                            ho_ObjectSelected.Dispose();
                            ho_Cross.Dispose();
                        }
                    }

                    // 报警器发声
                    for (int i = 0; i < 3; i++)
                    {
                        _ = BeepMethod.Beep(1000, 200);
                    }
                }
                else
                {
                    BoolGood = true;
                    // 报警器发声
                    _ = BeepMethod.Beep(1000, 500);
                }
            }
            catch (HalconException)
            {
                // 释放内存
                ho_Image.Dispose();
                ho_ImageMedian.Dispose();
                ho_GrayImage.Dispose();
                ho_Regions.Dispose();
                ho_ImageMedian.Dispose();
                ho_ObjectSelected.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionsDifference.Dispose();
                ho_RegionTrans.Dispose();
                ho_RegionDifference.Dispose();
                ho_RegionOpening.Dispose();
                ho_RegionsRollers.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Indices.Dispose();
                HandyControl.Controls.Growl.Error("检测失败。");
            }

            // 释放内存
            ho_Image.Dispose();
            ho_ImageMedian.Dispose();
            ho_GrayImage.Dispose();
            ho_Regions.Dispose();
            ho_ImageMedian.Dispose();
            ho_ObjectSelected.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionsDifference.Dispose();
            ho_RegionTrans.Dispose();
            ho_RegionDifference.Dispose();
            ho_RegionOpening.Dispose();
            ho_RegionsRollers.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Indices.Dispose();
        }
    }
}