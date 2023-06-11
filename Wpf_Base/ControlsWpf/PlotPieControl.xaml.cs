using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Wpf_Base.ControlsWpf.Model;
using Wpf_Base.LogWpf;
using Wpf_Base.MethodNet;

namespace Wpf_Base.ControlsWpf
{
    /// <summary>
    /// PlotPieControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlotPieControl : UserControl
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string PlotTitle { get; set; } = "Title";

        /// <summary>
        /// 系列
        /// </summary>
        public List<CPlotInfo> PlotSeries { get; set; } = new List<CPlotInfo>();

        /// <summary>
        /// 饼图半径
        /// </summary>
        public double Radius { get; set; } = 150;

        /// <summary>
        /// 角度
        /// </summary>
        private double[] Angles { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        private List<Brush> ColorMap { get; set; } = CPlotConstant.Default;

        /// <summary>
        /// 前一个选中
        /// </summary>
        private int IndexSeries { get; set; } = -1;

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


        public PlotPieControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绘制圆弧：顺时针，以 -x 方向为起点
        /// </summary>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        /// <param name="fill"></param>
        private void DrawArcSegment(double angle1, double angle2, Brush fill)
        {
            // 角度 → 弧度
            double arc1 = angle1 * Math.PI / 180;
            double arc2 = angle2 * Math.PI / 180;

            // 圆心坐标
            Point p0 = new Point(Radius, Radius);

            // 起点、终点坐标
            Point p1 = new Point(Radius - Radius * Math.Cos(arc1), Radius - Radius * Math.Sin(arc1));
            Point p2 = new Point(Radius - Radius * Math.Cos(arc2), Radius - Radius * Math.Sin(arc2));

            // 添加元素：自定义路径
            PathFigure figure = new PathFigure
            {
                StartPoint = p1,
                IsClosed = true,
                IsFilled = true,
            };
            // 圆弧
            ArcSegment segment = new ArcSegment
            {
                // 超过 180° 为大角度
                IsLargeArc = (angle2 - angle1) > 180,
                // 顺时针
                SweepDirection = SweepDirection.Clockwise,
                // 大小
                Size = new Size(Radius, Radius),
                // 圆弧终点
                Point = p2,
            };
            figure.Segments.Add(segment);

            // 增加一个圆心
            figure.Segments.Add(new LineSegment(p0, true));

            // 显示
            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            Path path = new Path
            {
                Fill = fill,
                Data = geometry,
            };

            // 添加事件
            path.MouseLeftButtonDown += Path_MouseLeftButtonDown;

            PlotCanvas.Children.Add(path);

            //// 再添加一个元素
            //geometry = new PathGeometry();
            //figure = new PathFigure
            //{
            //    StartPoint = p2,
            //    IsClosed = true,
            //    IsFilled = true,
            //};
            //RectangleGeometry rectGeometry = new RectangleGeometry
            //{
            //    Rect = new Rect(50, 50, 100, 50),
            //    RadiusX = 0,
            //    RadiusY = 0,
            //};
            //path = new Path
            //{
            //    Fill = Brushes.DodgerBlue,
            //    Data = rectGeometry,
            //};
            //PlotCanvas.Children.Add(path);
        }

        private void Path_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // 判断当前点是否在圆内
            Point curPoint = e.GetPosition(e.Device.Target);
            Point p0 = new Point(Radius, Radius);
            Point p1 = new Point(0, Radius);
            double dist = InkMethod.GetDistancePP(curPoint, new Point(Radius, Radius));
            if (dist < Radius)
            {
                // 计算角度
                double angle = InkMethod.GetPointAngle(p0, p1, curPoint);
                if (!InkMethod.GetRotateDirection(p0, p1, curPoint))
                {
                    angle = 360 - angle;
                }
                //PrintLog("角度：" + angle, EnumLogType.Debug);

                // 判断当前点位于哪个区域
                int idx = 0;
                for (int i = 1; i < Angles.Length; i++)
                {
                    if (angle < Angles[i])
                    {
                        idx = i - 1;
                        break;
                    }
                }

                // 绘制一个大扇形
                if (idx != IndexSeries)
                {
                    IndexSeries = idx;
                    if (PlotCanvas.Children.Count == PlotSeries.Count)
                    {
                        DrawArcSegmentBig(Angles[idx], Angles[idx + 1], 1.05 * Radius, PlotSeries[idx].Fill);
                    }
                    else if (PlotCanvas.Children.Count == PlotSeries.Count + 1)
                    {
                        PlotCanvas.Children.RemoveAt(PlotSeries.Count);
                        DrawArcSegmentBig(Angles[idx], Angles[idx + 1], 1.05 * Radius, PlotSeries[idx].Fill);
                    }
                }
                else
                {
                    if (PlotCanvas.Children.Count == PlotSeries.Count)
                    {
                        DrawArcSegmentBig(Angles[idx], Angles[idx + 1], 1.05 * Radius, PlotSeries[idx].Fill);
                    }
                    else if (PlotCanvas.Children.Count == PlotSeries.Count + 1)
                    {
                        PlotCanvas.Children.RemoveAt(PlotSeries.Count);
                    }
                }
            }
        }

        private void DrawArcSegmentBig(double angle1, double angle2, double r, Brush fill)
        {
            // 角度 → 弧度
            double arc1 = angle1 * Math.PI / 180;
            double arc2 = angle2 * Math.PI / 180;

            // 圆心坐标
            Point p0 = new Point(Radius, Radius);

            // 起点、终点坐标
            Point p1 = new Point(Radius - r * Math.Cos(arc1), Radius - r * Math.Sin(arc1));
            Point p2 = new Point(Radius - r * Math.Cos(arc2), Radius - r * Math.Sin(arc2));

            // 添加元素：自定义路径
            PathFigure figure = new PathFigure
            {
                StartPoint = p1,
                IsClosed = true,
                IsFilled = true,
            };
            // 圆弧
            ArcSegment segment = new ArcSegment
            {
                // 超过 180° 为大角度
                IsLargeArc = (angle2 - angle1) > 180,
                // 顺时针
                SweepDirection = SweepDirection.Clockwise,
                // 大小
                Size = new Size(r, r),
                // 圆弧终点
                Point = p2,
            };
            figure.Segments.Add(segment);

            // 增加一个圆心
            figure.Segments.Add(new LineSegment(p0, true));

            // 显示
            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            Path path = new Path
            {
                Fill = fill,
                Data = geometry,
            };

            // 添加事件
            path.MouseLeftButtonDown += Path_MouseLeftButtonDown;

            PlotCanvas.Children.Add(path);
        }

        #region 对外接口
        /// <summary>
        /// 绘图
        /// </summary>
        public void PlotModel()
        {
            try
            {
                if (PlotSeries.Count < 0)
                {
                    PrintLog("绘图数据为空", EnumLogType.Warning);
                    return;
                }
                int count = PlotSeries.Count;

                // 初始化
                PlotCanvas.Children.Clear();
                Angles = new double[count + 1];

                // 累计求和
                double[] cumsum = new double[count];
                cumsum[0] = PlotSeries[0].Value;
                for (int i = 1; i < count; i++)
                {
                    cumsum[i] = cumsum[i - 1] + PlotSeries[i].Value;
                }

                // 换算成角度
                double total = cumsum[count - 1];
                Angles[0] = 0;
                for (int i = 0; i < count; i++)
                {
                    Angles[i + 1] = cumsum[i] / total * 360;
                }

                // 逐个绘图
                PlotCanvas.Children.Clear();
                for (int i = 0; i < count; i++)
                {
                    // 如果没有设置颜色 选择模板颜色
                    if (PlotSeries[i].Fill == null)
                    {
                        PlotSeries[i].Fill = ColorMap[i % ColorMap.Count];
                    }
                    DrawArcSegment(Angles[i], Angles[i + 1], PlotSeries[i].Fill);
                }

                // 图例
                LB_Legend.ItemsSource = null;
                LB_Legend.ItemsSource = PlotSeries;
            }
            catch (Exception ex)
            {
                PrintLog("绘图异常：" + ex.Message, EnumLogType.Error);
            }
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="colors"></param>
        public void SetColorMap(List<Brush> colors)
        {
            ColorMap = colors;
            for (int i = 0; i < PlotSeries.Count; i++)
            {
                PlotSeries[i].Fill = ColorMap[i % ColorMap.Count];
            }
        }
        #endregion
    }
}