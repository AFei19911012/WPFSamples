using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    /// Created Time: 2021/10/13 20:39:12
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/10/13 20:39:12    Taosy.W                 
    ///
    public class TeethDetectionViewModel : ViewModelBase
    {
        private HObject ho_Image;
        private HWindow ho_Window;
        private HSmartWindowControlWPF Halcon;
        private Thread ho_thread;
        private readonly string StrOperationRun = "点我运行";
        private readonly string StrOperationStop = "点我停止";
        private readonly double gap_radius = 10;
        private readonly double gap_distance = 30;

        private string strOperation;
        public string StrOperation
        {
            get => strOperation;
            set => Set(ref strOperation, value);
        }

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public TeethDetectionViewModel()
        {
            StrOperation = StrOperationRun;
        }

        /// <summary>
        /// 关联控件
        /// </summary>
        public RelayCommand<RoutedEventArgs> CmdLoaded => new Lazy<RelayCommand<RoutedEventArgs>>(() => new RelayCommand<RoutedEventArgs>(Loaded)).Value;
        private void Loaded(RoutedEventArgs e)
        {
            Halcon = (e.Source as TeethDetection).HalconWPF;
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


        private void ThreadRun()
        {
            DirectoryInfo folder = new DirectoryInfo(@"D:\MyPrograms\DataSet\halcon\牙模");
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
                        TeethDetect();
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        /// <summary>
        /// 牙模检测
        /// </summary>
        private void TeethDetect()
        {
            // 初始化图像变量
            HOperatorSet.GenEmptyObj(out HObject ho_GrayImage);
            HOperatorSet.GenEmptyObj(out HObject ho_Regions);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionsSkeleton);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionSkeleton);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out HObject ho_Region);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out HObject ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionFrame);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out HObject ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out HObject ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionsTeeth);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionBridge);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionTeeth);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionBridgeTeeth);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionBridgeFrame);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionsBridge);
            HOperatorSet.GenEmptyObj(out HObject ho_ObjectBridge);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionDilationB);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionDilationS);
            HOperatorSet.GenEmptyObj(out HObject ho_Skeleton);
            HOperatorSet.GenEmptyObj(out HObject ho_EndPoints);
            HOperatorSet.GenEmptyObj(out HObject ho_JuncPoints);
            HOperatorSet.GenEmptyObj(out HObject ho_RegionsBridgeTeeth);
            HOperatorSet.GenEmptyObj(out HObject ho_Contour);

            // 初始化控制变量
            HTuple hv_UsedThreshold = new HTuple();
            HTuple hv_Area = new HTuple();
            HTuple hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();
            HTuple hv_MaxArea = new HTuple();
            HTuple hv_MinDistance = new HTuple();
            HTuple hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple();
            HTuple hv_Number = new HTuple();
            HTuple hv_RowStart = new HTuple();
            HTuple hv_ColStart = new HTuple();
            HTuple hv_RowEnd = new HTuple();
            HTuple hv_ColEnd = new HTuple();
            HTuple hv_Rows = new HTuple();
            HTuple hv_Columns = new HTuple();
            HTuple hv_Distance = new HTuple();
            HTuple hv_Indices = new HTuple();
            HTuple hv_Rows1 = new HTuple();
            HTuple hv_Columns1 = new HTuple();
            HTuple hv_Rows2 = new HTuple();
            HTuple hv_Columns2 = new HTuple();
            HTuple hv_Distance1 = new HTuple();
            HTuple hv_Distance2 = new HTuple();

            try
            {
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                // 区域生长分割
                ho_Regions.Dispose();
                HOperatorSet.Regiongrowing(ho_GrayImage, out ho_Regions, 2, 2, 5, 200000);
                // 确定骨架
                ho_RegionsSkeleton.Dispose();
                HOperatorSet.SelectShape(ho_Regions, out ho_RegionsSkeleton, "area", "and", 200000, 2000000);
                ho_RegionSkeleton.Dispose();
                HOperatorSet.Union1(ho_RegionsSkeleton, out ho_RegionSkeleton);
                // 大致确定牙齿区域
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_Regions, out ho_RegionUnion);
                ho_Region.Dispose();
                HOperatorSet.Threshold(ho_GrayImage, out ho_Region, 0, 255);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_Region, ho_RegionUnion, out ho_RegionDifference);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_RegionDifference, out ho_ImageReduced);
                // 定位牙齿
                ho_RegionFrame.Dispose();
                hv_UsedThreshold.Dispose();
                HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_RegionFrame, "max_separability", "light", out hv_UsedThreshold);
                ho_RegionErosion.Dispose();
                HOperatorSet.ErosionCircle(ho_RegionFrame, out ho_RegionErosion, gap_distance);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionErosion, out ho_ConnectedRegions);
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                hv_MaxArea.Dispose();
                HOperatorSet.TupleMax(hv_Area, out hv_MaxArea);
                // 通过 area、circularity、roundness 特征选择牙齿
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_SelectedRegions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, ((new HTuple("area")).TupleConcat("circularity")).TupleConcat("roundness"), "and", ((0.1 * hv_MaxArea)).TupleConcat((new HTuple(0.4)).TupleConcat(0.5)), ((hv_MaxArea + 1)).TupleConcat((new HTuple(1)).TupleConcat(1)));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Regions.Dispose();
                    HOperatorSet.OpeningCircle(ho_SelectedRegions, out ho_Regions, 0.5 * gap_distance);
                }
                ho_RegionsTeeth.Dispose();
                HOperatorSet.DilationCircle(ho_Regions, out ho_RegionsTeeth, gap_distance);

                //
                // 定位连接区域
                // 单独提取牙齿、骨架部分
                ho_RegionDilation.Dispose();
                HOperatorSet.DilationCircle(ho_RegionsTeeth, out ho_RegionDilation, gap_distance);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_RegionDilation, ho_RegionsTeeth, out ho_RegionDifference);
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_RegionDifference, out ho_RegionUnion);
                // 抠图
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_RegionUnion, out ho_ImageReduced);
                ho_Region.Dispose(); hv_UsedThreshold.Dispose();
                HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_Region, "max_separability", "light", out hv_UsedThreshold);
                // 连接区域
                ho_RegionBridge.Dispose();
                HOperatorSet.OpeningCircle(ho_Region, out ho_RegionBridge, 3);
                // 骨架区域
                ho_Region.Dispose();
                HOperatorSet.Union1(ho_RegionDilation, out ho_Region);
                ho_RegionDifference.Dispose();
                HOperatorSet.Difference(ho_RegionFrame, ho_Region, out ho_RegionDifference);
                HOperatorSet.Union2(ho_RegionSkeleton, ho_RegionDifference, out ho_RegionSkeleton);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionSkeleton, out ho_ConnectedRegions);
                hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                hv_MaxArea.Dispose();
                HOperatorSet.TupleMax(hv_Area, out hv_MaxArea);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Regions.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_Regions, "area", "and", 1000, hv_MaxArea + 1);
                }
                ho_RegionSkeleton.Dispose();
                HOperatorSet.Union1(ho_Regions, out ho_RegionSkeleton);
                ho_RegionTeeth.Dispose();
                HOperatorSet.Union1(ho_RegionsTeeth, out ho_RegionTeeth);

                // 精准定位连接区域
                ho_RegionBridgeTeeth.Dispose();
                HOperatorSet.GenEmptyRegion(out ho_RegionBridgeTeeth);
                ho_RegionBridgeFrame.Dispose();
                HOperatorSet.GenEmptyRegion(out ho_RegionBridgeFrame);
                ho_RegionsBridge.Dispose();
                HOperatorSet.Connection(ho_RegionBridge, out ho_RegionsBridge);
                int number_bridge = ho_RegionsBridge.CountObj();
                for (int i = 1; i <= number_bridge; i++)
                {
                    ho_ObjectBridge.Dispose();
                    HOperatorSet.SelectObj(ho_RegionsBridge, out ho_ObjectBridge, i);
                    hv_MinDistance.Dispose();
                    hv_Row1.Dispose();
                    hv_Column1.Dispose();
                    hv_Row2.Dispose();
                    hv_Column2.Dispose();
                    HOperatorSet.DistanceRrMin(ho_ObjectBridge, ho_RegionTeeth, out hv_MinDistance, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);

                    // 条件一：连接到牙齿
                    if (hv_MinDistance < 2)
                    {
                        // 条件二：另一端连接到牙齿、骨架
                        ho_RegionDilation.Dispose();
                        HOperatorSet.DilationCircle(ho_ObjectBridge, out ho_RegionDilation, 5);
                        // 优先判断连接到骨架的情况
                        ho_RegionIntersection.Dispose();
                        HOperatorSet.Intersection(ho_RegionSkeleton, ho_RegionDilation, out ho_RegionIntersection);
                        hv_Area.Dispose();
                        hv_Row.Dispose();
                        hv_Column.Dispose();
                        HOperatorSet.AreaCenter(ho_RegionIntersection, out hv_Area, out hv_Row, out hv_Column);
                        if (hv_Area > 0)
                        {
                            HOperatorSet.Union2(ho_RegionBridgeFrame, ho_ObjectBridge, out ho_RegionBridgeFrame);
                            continue;
                        }

                        //连接到牙齿的情况
                        ho_RegionIntersection.Dispose();
                        HOperatorSet.Intersection(ho_RegionTeeth, ho_RegionDilation, out ho_RegionIntersection);
                        ho_Regions.Dispose();
                        HOperatorSet.Connection(ho_RegionIntersection, out ho_Regions);
                        hv_Number.Dispose();
                        int number = ho_Regions.CountObj();
                        if (number > 1)
                        {
                            HOperatorSet.Union2(ho_RegionBridgeTeeth, ho_ObjectBridge, out ho_RegionBridgeTeeth);
                        }
                    }
                }

                //
                // 确定切割线坐标
                // 牙齿-牙齿、牙齿-骨架，两种情况单独分析
                // 两次膨胀 → 交集 → 差分
                // 牙齿-骨架，一刀切
                // 待优化
                // GapRadius 可以针对每个连接点单独自适应
                // 每个连接区域单独判断，取一个最长的交集
                //
                ho_RegionDilationB.Dispose();
                HOperatorSet.DilationCircle(ho_RegionTeeth, out ho_RegionDilationB, gap_radius);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_RegionDilationS.Dispose();
                    HOperatorSet.DilationCircle(ho_RegionTeeth, out ho_RegionDilationS, gap_radius - 1);
                }
                ho_RegionIntersection.Dispose();
                HOperatorSet.Intersection(ho_RegionDilationB, ho_RegionBridgeFrame, out ho_RegionIntersection);
                ho_Region.Dispose();
                HOperatorSet.Intersection(ho_RegionDilationS, ho_RegionBridgeFrame, out ho_Region);
                ho_RegionBridgeFrame.Dispose();
                HOperatorSet.Difference(ho_RegionIntersection, ho_Region, out ho_RegionBridgeFrame);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionBridgeFrame, out ho_ConnectedRegions);
                number_bridge = ho_ConnectedRegions.CountObj();
                for (int i = 1; i <= number_bridge; i++)
                {
                    ho_ObjectBridge.Dispose();
                    HOperatorSet.SelectObj(ho_ConnectedRegions, out ho_ObjectBridge, i);

                    // 骨架两个端点，即切割点坐标
                    ho_Skeleton.Dispose();
                    HOperatorSet.Skeleton(ho_ObjectBridge, out ho_Skeleton);
                    ho_EndPoints.Dispose();
                    ho_JuncPoints.Dispose();
                    HOperatorSet.JunctionsSkeleton(ho_Skeleton, out ho_EndPoints, out ho_JuncPoints);
                    hv_Rows.Dispose(); hv_Columns.Dispose();
                    HOperatorSet.GetRegionPoints(ho_EndPoints, out hv_Rows, out hv_Columns);
                    if (hv_Rows.TupleLength() > 1)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Distance.Dispose();
                            HOperatorSet.DistancePp(hv_Rows.TupleSelect(0), hv_Columns.TupleSelect(0), hv_Rows.TupleSelect(1), hv_Columns.TupleSelect(1), out hv_Distance);
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_RowStart = hv_RowStart.TupleConcat(hv_Rows.TupleSelect(0));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_ColStart = hv_ColStart.TupleConcat(hv_Columns.TupleSelect(0));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_RowEnd = hv_RowEnd.TupleConcat(hv_Rows.TupleSelect(1));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_ColEnd = hv_ColEnd.TupleConcat(hv_Columns.TupleSelect(1));
                        }
                    }
                }

                //
                // 牙齿-牙齿，切一条
                ho_RegionsBridgeTeeth.Dispose();
                HOperatorSet.Connection(ho_RegionBridgeTeeth, out ho_RegionsBridgeTeeth);
                number_bridge = ho_RegionsBridgeTeeth.CountObj();
                for (int i = 1; i <= number_bridge; i++)
                {
                    ho_ObjectBridge.Dispose();
                    HOperatorSet.SelectObj(ho_RegionsBridgeTeeth, out ho_ObjectBridge, i);
                    ho_RegionDilationB.Dispose();
                    HOperatorSet.DilationCircle(ho_RegionTeeth, out ho_RegionDilationB, gap_radius);
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_RegionDilationS.Dispose();
                        HOperatorSet.DilationCircle(ho_RegionTeeth, out ho_RegionDilationS, gap_radius - 1);
                    }
                    ho_RegionIntersection.Dispose();
                    HOperatorSet.Intersection(ho_RegionDilationB, ho_ObjectBridge, out ho_RegionIntersection);
                    ho_Region.Dispose();
                    HOperatorSet.Intersection(ho_RegionDilationS, ho_ObjectBridge, out ho_Region);
                    ho_RegionDifference.Dispose();
                    HOperatorSet.Difference(ho_RegionIntersection, ho_Region, out ho_RegionDifference);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.Connection(ho_RegionDifference, out ho_ConnectedRegions);
                    hv_Area.Dispose();
                    hv_Row.Dispose();
                    hv_Column.Dispose();
                    HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
                    hv_Indices.Dispose();
                    HOperatorSet.TupleSortIndex(hv_Area, out hv_Indices);

                    if (hv_Indices.TupleLength() == 1)
                    {
                        ho_Region.Dispose();
                        HOperatorSet.ClosingCircle(ho_ConnectedRegions, out ho_Region, 1);
                        ho_Skeleton.Dispose();
                        HOperatorSet.Skeleton(ho_Region, out ho_Skeleton);
                        ho_EndPoints.Dispose();
                        ho_JuncPoints.Dispose();
                        HOperatorSet.JunctionsSkeleton(ho_Skeleton, out ho_EndPoints, out ho_JuncPoints);
                        hv_Rows.Dispose();
                        hv_Columns.Dispose();
                        HOperatorSet.GetRegionPoints(ho_EndPoints, out hv_Rows, out hv_Columns);
                        int len = hv_Rows.TupleLength();
                        if (hv_Rows.TupleLength() > 1)
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Distance.Dispose();
                                HOperatorSet.DistancePp(hv_Rows.TupleSelect(0), hv_Columns.TupleSelect(0), hv_Rows.TupleSelect(1), hv_Columns.TupleSelect(1), out hv_Distance);
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_RowStart = hv_RowStart.TupleConcat(hv_Rows.TupleSelect(0));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ColStart = hv_ColStart.TupleConcat(hv_Columns.TupleSelect(0));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_RowEnd = hv_RowEnd.TupleConcat(hv_Rows.TupleSelect(1));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ColEnd = hv_ColEnd.TupleConcat(hv_Columns.TupleSelect(1));
                            }
                        }
                    }
                    else
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_Regions.Dispose();
                            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_Regions, "area", "and", hv_Area.TupleSelect(hv_Indices.TupleSelect(0)) - 1, hv_Area.TupleSelect(hv_Indices.TupleSelect(1)) + 1);
                        }
                        ho_ObjectBridge.Dispose();
                        HOperatorSet.SelectObj(ho_Regions, out ho_ObjectBridge, 1);
                        ho_Skeleton.Dispose();
                        HOperatorSet.Skeleton(ho_ObjectBridge, out ho_Skeleton);
                        ho_EndPoints.Dispose(); ho_JuncPoints.Dispose();
                        HOperatorSet.JunctionsSkeleton(ho_Skeleton, out ho_EndPoints, out ho_JuncPoints);
                        hv_Rows1.Dispose();
                        hv_Columns1.Dispose();
                        HOperatorSet.GetRegionPoints(ho_EndPoints, out hv_Rows1, out hv_Columns1);
                        //
                        ho_ObjectBridge.Dispose();
                        HOperatorSet.SelectObj(ho_Regions, out ho_ObjectBridge, 2);
                        ho_Skeleton.Dispose();
                        HOperatorSet.Skeleton(ho_ObjectBridge, out ho_Skeleton);
                        ho_EndPoints.Dispose(); ho_JuncPoints.Dispose();
                        HOperatorSet.JunctionsSkeleton(ho_Skeleton, out ho_EndPoints, out ho_JuncPoints);
                        hv_Rows2.Dispose();
                        hv_Columns2.Dispose();
                        HOperatorSet.GetRegionPoints(ho_EndPoints, out hv_Rows2, out hv_Columns2);

                        //
                        // 切短的
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Distance1.Dispose();
                            HOperatorSet.DistancePp(hv_Rows1.TupleSelect(0), hv_Columns1.TupleSelect(0), hv_Rows1.TupleSelect(1), hv_Columns1.TupleSelect(1), out hv_Distance1);
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Distance2.Dispose();
                            HOperatorSet.DistancePp(hv_Rows2.TupleSelect(0), hv_Columns2.TupleSelect(0), hv_Rows2.TupleSelect(1), hv_Columns2.TupleSelect(1), out hv_Distance2);
                        }
                        if (hv_Distance1 > hv_Distance2)
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_RowStart = hv_RowStart.TupleConcat(hv_Rows2.TupleSelect(0));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ColStart = hv_ColStart.TupleConcat(hv_Columns2.TupleSelect(0));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_RowEnd = hv_RowEnd.TupleConcat(hv_Rows2.TupleSelect(1));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ColEnd = hv_ColEnd.TupleConcat(hv_Columns2.TupleSelect(1));
                            }
                        }
                        else
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_RowStart = hv_RowStart.TupleConcat(hv_Rows1.TupleSelect(0));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ColStart = hv_ColStart.TupleConcat(hv_Columns1.TupleSelect(0));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_RowEnd = hv_RowEnd.TupleConcat(hv_Rows1.TupleSelect(1));
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_ColEnd = hv_ColEnd.TupleConcat(hv_Columns1.TupleSelect(1));
                            }
                        }
                    }
                }

                //
                // 进一步优化，把切割线往外延伸 2 个像素点
                // 还要判断切割点是否处在空白区域，针对点做小的偏移
                // pass

                // 把切割线画出来
                for (int i = 0; i < hv_RowStart.TupleLength(); i++)
                {
                    ho_Contour.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_Contour, ((hv_RowStart.TupleSelect(i))).TupleConcat(hv_RowEnd.TupleSelect(i)), (hv_ColStart.TupleSelect(i)).TupleConcat(hv_ColEnd.TupleSelect(i)));
                    HOperatorSet.DispObj(ho_Contour, ho_Window);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.DispText(ho_Window, hv_RowStart.TupleLength(), "image", 12, 12, "black", new HTuple(), new HTuple());
                    HOperatorSet.DispText(ho_Window, hv_RowStart.TupleString(".4d") + " " + hv_ColStart.TupleString(".4d") + ", " + hv_RowEnd.TupleString(".4d") + " " + hv_ColEnd.TupleString(".4d"), "image", 50, 12, "black", new HTuple(), new HTuple());
                }
            }
            catch (Exception)
            {
                ho_Image.Dispose();
                ho_GrayImage.Dispose();
                ho_Regions.Dispose();
                ho_RegionsSkeleton.Dispose();
                ho_RegionSkeleton.Dispose();
                ho_RegionUnion.Dispose();
                ho_Region.Dispose();
                ho_RegionDifference.Dispose();
                ho_ImageReduced.Dispose();
                ho_RegionFrame.Dispose();
                ho_RegionErosion.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionsTeeth.Dispose();
                ho_RegionDilation.Dispose();
                ho_RegionBridge.Dispose();
                ho_RegionTeeth.Dispose();
                ho_RegionBridgeTeeth.Dispose();
                ho_RegionBridgeFrame.Dispose();
                ho_RegionsBridge.Dispose();
                ho_ObjectBridge.Dispose();
                ho_RegionIntersection.Dispose();
                ho_RegionDilationB.Dispose();
                ho_RegionDilationS.Dispose();
                ho_Skeleton.Dispose();
                ho_EndPoints.Dispose();
                ho_JuncPoints.Dispose();
                ho_RegionsBridgeTeeth.Dispose();
                ho_Contour.Dispose();
                hv_UsedThreshold.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_MaxArea.Dispose();
                hv_MinDistance.Dispose();
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Number.Dispose();
                hv_RowStart.Dispose();
                hv_ColStart.Dispose();
                hv_RowEnd.Dispose();
                hv_ColEnd.Dispose();
                hv_Rows.Dispose();
                hv_Columns.Dispose();
                hv_Distance.Dispose();
                hv_Indices.Dispose();
                hv_Rows1.Dispose();
                hv_Columns1.Dispose();
                hv_Rows2.Dispose();
                hv_Columns2.Dispose();
                hv_Distance1.Dispose();
                hv_Distance2.Dispose();
                HandyControl.Controls.Growl.Error("检测失败。");
            }

            // 释放内存
            ho_Image.Dispose();
            ho_GrayImage.Dispose();
            ho_Regions.Dispose();
            ho_RegionsSkeleton.Dispose();
            ho_RegionSkeleton.Dispose();
            ho_RegionUnion.Dispose();
            ho_Region.Dispose();
            ho_RegionDifference.Dispose();
            ho_ImageReduced.Dispose();
            ho_RegionFrame.Dispose();
            ho_RegionErosion.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionsTeeth.Dispose();
            ho_RegionDilation.Dispose();
            ho_RegionBridge.Dispose();
            ho_RegionTeeth.Dispose();
            ho_RegionBridgeTeeth.Dispose();
            ho_RegionBridgeFrame.Dispose();
            ho_RegionsBridge.Dispose();
            ho_ObjectBridge.Dispose();
            ho_RegionIntersection.Dispose();
            ho_RegionDilationB.Dispose();
            ho_RegionDilationS.Dispose();
            ho_Skeleton.Dispose();
            ho_EndPoints.Dispose();
            ho_JuncPoints.Dispose();
            ho_RegionsBridgeTeeth.Dispose();
            ho_Contour.Dispose();
            hv_UsedThreshold.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_MaxArea.Dispose();
            hv_MinDistance.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Number.Dispose();
            hv_RowStart.Dispose();
            hv_ColStart.Dispose();
            hv_RowEnd.Dispose();
            hv_ColEnd.Dispose();
            hv_Rows.Dispose();
            hv_Columns.Dispose();
            hv_Distance.Dispose();
            hv_Indices.Dispose();
            hv_Rows1.Dispose();
            hv_Columns1.Dispose();
            hv_Rows2.Dispose();
            hv_Columns2.Dispose();
            hv_Distance1.Dispose();
            hv_Distance2.Dispose();
        }
    }
}