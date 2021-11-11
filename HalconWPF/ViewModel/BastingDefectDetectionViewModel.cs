using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HalconDotNet;
using HalconWPF.UserControl;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace HalconWPF.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/11/11 14:52:03
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/11/11 14:52:03    Taosy.W                 
    ///
    public class BastingDefectDetectionViewModel : ViewModelBase
    {
        private HObject ho_Image;
        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;
        private Thread ho_thread;
        private readonly string StrOperationRun = "点我运行";
        private readonly string StrOperationStop = "点我停止";

        private string strOperation;
        public string StrOperation
        {
            get => strOperation;
            set => Set(ref strOperation, value);
        }

        private string strMethod;
        public string StrMethod
        {
            get => strMethod;
            set => Set(ref strMethod, value);
        }

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public BastingDefectDetectionViewModel()
        {
            StrOperation = StrOperationRun;
            StrMethod = "01";
        }

        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as BastingDefectDetection).HalconWPF;
            ho_Window = Halcon.HalconWindow;
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
                HOperatorSet.GenEmptyObj(out ho_Image);
                ho_thread = new Thread(ThreadRun)
                {
                    IsBackground = true
                };
                ho_thread.Start();
                // 设置颜色，必备
                HOperatorSet.SetColor(ho_Window, "red");
                HOperatorSet.SetLineWidth(ho_Window, 3);
            }
            // 关闭
            else
            {
                StrOperation = StrOperationRun;
                ho_thread.Abort();
            }
        }

        public RelayCommand CmdChangeMethod => new Lazy<RelayCommand>(() => new RelayCommand(ChangeMethod)).Value;
        private void ChangeMethod()
        {
            StrMethod = StrMethod == "01" ? "02" : "01";
        }

        private void ThreadRun()
        {
            DirectoryInfo folder = new DirectoryInfo(@"D:\MyPrograms\DataSet\halcon\针脚\" + StrMethod);
            if (folder.Exists)
            {
                FileInfo[] files = folder.GetFiles();
                while (true)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        ho_Image.Dispose();
                        HOperatorSet.ReadImage(out ho_Image, files[i].FullName);
                        // 显示结果
                        HOperatorSet.DispObj(ho_Image, ho_Window);

                        _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            (ThreadStart)delegate ()
                            {
                                // 图像自适应显示
                                Halcon.SetFullImagePart();
                            }
                            );
                        BastingDetect(StrMethod);
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        /// <summary>
        /// 针脚检测
        /// </summary>
        private void BastingDetect(string method = "01")
        {
            // 初始化控制变量
            HTuple hv_Area = new HTuple();
            HTuple hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();
            HTuple hv_Min = new HTuple();
            HTuple hv_Max = new HTuple();
            HTuple hv_Range = new HTuple();
            HTuple hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple();
            HTuple hv_Indices = new HTuple();

            // 初始化图像变量
            HOperatorSet.GenEmptyObj(out HObject ho_ImageMean);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionDynThresh);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out HObject ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out HObject ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out HObject ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out HObject ho_Cross);
            HOperatorSet.GenEmptyObj(out HObject ho_GrayImage);
            HOperatorSet.GenEmptyObj(out HObject ho_Rectangle);
            HOperatorSet.GenEmptyObj(out HObject ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out HObject ho_Region);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out HObject ho_ROIRegion);
            HOperatorSet.GenEmptyObj(out HObject ho_ObjectSelected);
            HOperatorSet.GenEmptyObj(out HObject ho_ImageSelected);
            HOperatorSet.GenEmptyObj(out HObject ho_Regions);

            ho_Window.SetLineWidth(3);
            ho_Window.SetDraw("margin");
            try
            {
                // 第一种打光方式
                if (method == "01")
                {
                    // 局部阈值，突出轮廓
                    ho_ImageMean.Dispose();
                    HOperatorSet.MeanImage(ho_Image, out ho_ImageMean, 5, 5);
                    ho_RegionDynThresh.Dispose();
                    HOperatorSet.DynThreshold(ho_Image, ho_ImageMean, out ho_RegionDynThresh, 30, "light");
                    // 饱满
                    ho_RegionClosing.Dispose();
                    HOperatorSet.ClosingCircle(ho_RegionDynThresh, out ho_RegionClosing, 30);
                    // 连通域
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionClosing, out ho_ConnectedRegions);
                    // 特征选择
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", 100, 99999);
                    // 饱满
                    ho_RegionTrans.Dispose();
                    HOperatorSet.ShapeTrans(ho_SelectedRegions, out ho_RegionTrans, "convex");
                    // 排序
                    ho_SortedRegions.Dispose();
                    HOperatorSet.SortRegion(ho_RegionTrans, out ho_SortedRegions, "first_point", "true", "column");
                    // 面积
                    hv_Area.Dispose();
                    hv_Row.Dispose();
                    hv_Column.Dispose();
                    HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row, out hv_Column);
                    // 中心点
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 100, 0.785398);
                    // 显示结果
                    ho_Window.DispObj(ho_Cross);
                    //HOperatorSet.WaitSeconds(0.5);
                }
                // 第二种打光方式
                else
                {
                    ho_GrayImage.Dispose();
                    HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                    // ROI
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 500, 2048, 2500);
                    // 抠图
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_GrayImage, ho_Rectangle, out ho_ImageReduced);
                    // 获取每个针脚大致区域
                    hv_Min.Dispose();
                    hv_Max.Dispose();
                    hv_Range.Dispose();
                    HOperatorSet.MinMaxGray(ho_ImageReduced, ho_ImageReduced, 0, out hv_Min, out hv_Max, out hv_Range);
                    // 阈值分割
                    ho_Region.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 0.8 * hv_Max, hv_Max);
                    // 膨胀
                    ho_RegionDilation.Dispose();
                    HOperatorSet.DilationCircle(ho_Region, out ho_RegionDilation, 30);
                    // 连通域
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionDilation, out ho_ConnectedRegions);
                    // 数量
                    int number = ho_ConnectedRegions.CountObj();
                    // 大致区域里单独分割
                    ho_ROIRegion.Dispose();
                    // 生成一个空的区域
                    HOperatorSet.GenEmptyRegion(out ho_ROIRegion);
                    // 遍历
                    for (int i = 1; i <= number; i++)
                    {
                        // 选择一个区域
                        ho_ObjectSelected.Dispose();
                        HOperatorSet.SelectObj(ho_ConnectedRegions, out ho_ObjectSelected, i);
                        // 抠图
                        ho_ImageSelected.Dispose();
                        HOperatorSet.ReduceDomain(ho_ImageReduced, ho_ObjectSelected, out ho_ImageSelected);
                        // 灰度范围
                        hv_Min.Dispose();
                        hv_Max.Dispose();
                        hv_Range.Dispose();
                        HOperatorSet.MinMaxGray(ho_ImageSelected, ho_ImageSelected, 0, out hv_Min, out hv_Max, out hv_Range);
                        // 粗略阈值
                        ho_Region.Dispose();
                        HOperatorSet.Threshold(ho_ImageSelected, out ho_Region, 0.9 * hv_Max, hv_Max);
                        // 闭运算
                        ho_RegionClosing.Dispose();
                        HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 30);
                        // 最小外接矩形
                        hv_Row1.Dispose();
                        hv_Column1.Dispose();
                        hv_Row2.Dispose();
                        hv_Column2.Dispose();
                        HOperatorSet.SmallestRectangle1(ho_RegionClosing, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
                        double min = Math.Min(hv_Row2 - hv_Row1, hv_Column2 - hv_Column1);
                        double max = Math.Max(hv_Row2 - hv_Row1, hv_Column2 - hv_Column1);
                        // 光斑太小
                        if (min < 10)
                        {
                            // 局部阈值
                            ho_Region.Dispose();
                            HOperatorSet.LocalThreshold(ho_ImageSelected, out ho_Region, "adapted_std_deviation", "light", new HTuple(), new HTuple());
                            // 闭运算
                            ho_RegionClosing.Dispose();
                            HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 30);
                            // 最小外接矩形
                            hv_Row1.Dispose();
                            hv_Column1.Dispose();
                            hv_Row2.Dispose();
                            hv_Column2.Dispose();
                            HOperatorSet.SmallestRectangle1(ho_RegionClosing, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
                        }
                        // 光斑太大，阈值根据针脚的实际尺寸来定
                        if (max > 42)
                        {
                            // 闭操作减小半径
                            ho_RegionClosing.Dispose();
                            HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 5);
                            // 选择最大面积
                            ho_Regions.Dispose();
                            HOperatorSet.Connection(ho_RegionClosing, out ho_Regions);
                            hv_Area.Dispose();
                            hv_Row.Dispose();
                            hv_Column.Dispose();
                            HOperatorSet.AreaCenter(ho_Regions, out hv_Area, out hv_Row, out hv_Column);
                            hv_Indices.Dispose();
                            HOperatorSet.TupleSortIndex(hv_Area, out hv_Indices);
                            int len = ho_Regions.CountObj();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                ho_ObjectSelected.Dispose();
                                HOperatorSet.SelectObj(ho_Regions, out ho_ObjectSelected, (hv_Indices.TupleSelect(len - 1)) + 1);
                            }
                            // 最小外接矩形
                            hv_Row1.Dispose();
                            hv_Column1.Dispose();
                            hv_Row2.Dispose();
                            hv_Column2.Dispose();
                            HOperatorSet.SmallestRectangle1(ho_ObjectSelected, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);
                        }
                        // 生成矩形
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
                        // 合并
                        HOperatorSet.Union2(ho_ROIRegion, ho_Rectangle, out ho_ROIRegion);
                    }
                    // 连通域
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_ROIRegion, out ho_ConnectedRegions);
                    // 面积
                    hv_Area.Dispose();
                    hv_Row.Dispose();
                    hv_Column.Dispose();
                    HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                    // 生成十字
                    ho_Cross.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 100, 0.785398);
                    // 显示结果
                    ho_Window.DispObj(ho_ConnectedRegions);
                    ho_Window.DispObj(ho_Cross);
                    //HOperatorSet.WaitSeconds(0.5);
                }
            }
            catch (Exception)
            {
                // 释放内存
                ho_Image.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Min.Dispose();
                hv_Max.Dispose();
                hv_Range.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Indices.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionTrans.Dispose();
                ho_SortedRegions.Dispose();
                ho_Cross.Dispose();
                ho_GrayImage.Dispose();
                ho_Rectangle.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionDilation.Dispose();
                ho_ROIRegion.Dispose();
                ho_ObjectSelected.Dispose();
                ho_ImageSelected.Dispose();
                ho_Regions.Dispose();
                HandyControl.Controls.Growl.Error("检测失败。");
            }

            // 释放内存
            ho_Image.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Min.Dispose();
            hv_Max.Dispose();
            hv_Range.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Indices.Dispose();
            ho_ImageMean.Dispose();
            ho_RegionDynThresh.Dispose();
            ho_RegionClosing.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionTrans.Dispose();
            ho_SortedRegions.Dispose();
            ho_Cross.Dispose();
            ho_GrayImage.Dispose();
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_RegionDilation.Dispose();
            ho_ROIRegion.Dispose();
            ho_ObjectSelected.Dispose();
            ho_ImageSelected.Dispose();
            ho_Regions.Dispose();
        }
    }
}
