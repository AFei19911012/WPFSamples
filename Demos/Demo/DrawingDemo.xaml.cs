using Demos.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace Demos.Demo
{
    /// <summary>
    /// DrawingDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DrawingDemo : UserControl
    {
        public string TitleGDI
        {
            get { return (string)GetValue(TitleGDIProperty); }
            set { SetValue(TitleGDIProperty, value); }
        }
        public static readonly DependencyProperty TitleGDIProperty =
            DependencyProperty.Register("TitleGDI", typeof(string), typeof(DrawingDemo), new PropertyMetadata(""));


        public bool IsCheckedGDI
        {
            get { return (bool)GetValue(IsCheckedGDIProperty); }
            set { SetValue(IsCheckedGDIProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedGDIProperty =
            DependencyProperty.Register("IsCheckedGDI", typeof(bool), typeof(DrawingDemo), new PropertyMetadata(false));



        public string TitlePath
        {
            get { return (string)GetValue(TitlePathProperty); }
            set { SetValue(TitlePathProperty, value); }
        }
        public static readonly DependencyProperty TitlePathProperty =
            DependencyProperty.Register("TitlePath", typeof(string), typeof(DrawingDemo), new PropertyMetadata(""));



        public bool IsCheckedPath
        {
            get { return (bool)GetValue(IsCheckedPathProperty); }
            set { SetValue(IsCheckedPathProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedPathProperty =
            DependencyProperty.Register("IsCheckedPath", typeof(bool), typeof(DrawingDemo), new PropertyMetadata(false));



        public string TitleShape
        {
            get { return (string)GetValue(TitleShapeProperty); }
            set { SetValue(TitleShapeProperty, value); }
        }
        public static readonly DependencyProperty TitleShapeProperty =
            DependencyProperty.Register("TitleShape", typeof(string), typeof(DrawingDemo), new PropertyMetadata(""));



        public bool IsCheckedShape
        {
            get { return (bool)GetValue(IsCheckedShapeProperty); }
            set { SetValue(IsCheckedShapeProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedShapeProperty =
            DependencyProperty.Register("IsCheckedShape", typeof(bool), typeof(DrawingDemo), new PropertyMetadata(false));



        public string TitleDrawingGroup
        {
            get { return (string)GetValue(TitleDrawingGroupProperty); }
            set { SetValue(TitleDrawingGroupProperty, value); }
        }
        public static readonly DependencyProperty TitleDrawingGroupProperty =
            DependencyProperty.Register("TitleDrawingGroup", typeof(string), typeof(DrawingDemo), new PropertyMetadata(""));


        public bool IsCheckedDrawingGroup
        {
            get { return (bool)GetValue(IsCheckedDrawingGroupProperty); }
            set { SetValue(IsCheckedDrawingGroupProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedDrawingGroupProperty =
            DependencyProperty.Register("IsCheckedDrawingGroup", typeof(bool), typeof(DrawingDemo), new PropertyMetadata(false));



        public DrawingDemo()
        {
            InitializeComponent();

            InitDrawing();
        }

        private void InitDrawing()
        {
            int len = 500;
            List<int> dataList = new List<int>();
            Random random = new Random();
            int[] datas = new int[len];
            for (int i = 0; i < len; i++)
            {
                dataList.Add(random.Next(10, 300));
            }

            Task.Run(() =>
            {
                while (true)
                {
                    // GDI 可以刷新很快
                    Thread.Sleep(500);

                    dataList.RemoveAt(0);
                    dataList.Add(random.Next(10, 300));
                    datas = dataList.ToArray();

                    DispatcherHelper.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (IsCheckedGDI)
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            DrawImageGDI(datas);
                            stopwatch.Stop();
                            TitleGDI = $"Using:{stopwatch.ElapsedMilliseconds:D2}ms";
                        }

                        if (IsCheckedDrawingGroup)
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            DrawImageDrawingGroup(datas);
                            stopwatch.Stop();
                            TitleDrawingGroup = $"Using:{stopwatch.ElapsedMilliseconds:D2}ms";
                        }

                        if (IsCheckedPath)
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            DrawingImagePath(datas);
                            stopwatch.Stop();
                            TitlePath = $"Using:{stopwatch.ElapsedMilliseconds:D2}ms";
                        }

                        if (IsCheckedShape)
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            DrawingImageShape(datas);
                            stopwatch.Stop();
                            TitleShape = $"Using:{stopwatch.ElapsedMilliseconds:D2}ms";
                        }
                    }));
                }
            });
        }


        private void DrawImageGDI(int[] datas)
        {
            int width = 500;
            int height = 400;
            var wBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
            MyImage_GDI.Source = wBitmap;
            wBitmap.Lock();

            Bitmap backBitmap = new Bitmap(width, height, wBitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, wBitmap.BackBuffer);
            using (Graphics graphics = Graphics.FromImage(backBitmap))
            {
                // 设置成高质量或者抗锯齿模式更丝滑
                //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                //绘制外框
                //graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Brushes.DarkGray, 1), new System.Drawing.Rectangle(0, 0, height, height));
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Brushes.LightGreen);
                for (int i = 0; i < datas.Length - 1; i++)
                {
                    int x1 = i;
                    int y1 = datas[i];
                    int x2 = i + 1;
                    int y2 = datas[i + 1];
                    graphics.DrawLine(pen, new System.Drawing.Point(x1, y1), new System.Drawing.Point(x2, y2));
                }

                graphics.Flush();
                graphics.Dispose();
                backBitmap.Dispose();
            }
            wBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            wBitmap.Unlock();
        }

        private void DrawImageDrawingGroup(int[] datas)
        {
            DrawingGroup group = new DrawingGroup();
            using (DrawingContext ctx = group.Open())
            {
                System.Windows.Media.Pen pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.LightGreen, 1);

                for (int i = 0; i < datas.Length - 1; i++)
                {
                    int x1 = i;
                    int y1 = datas[i];
                    int x2 = i + 1;
                    int y2 = datas[i + 1];
                    ctx.DrawLine(pen, new System.Windows.Point(x1, y1), new System.Windows.Point(x2, y2));
                }
            }
            group.Freeze();

            MyImage_DrawingGroup.Source = new DrawingImage(group);
        }

        private void DrawingImagePath(int[] datas)
        {
            StreamGeometry geometry = new StreamGeometry()
            {
                FillRule = FillRule.EvenOdd
            };

            using (StreamGeometryContext context = geometry.Open())
            {
                context.BeginFigure(new System.Windows.Point(0, 0), true, false);
                for (int i = 0; i < datas.Length; i++)
                {
                    double x1 = i;
                    double y1 = datas[i];
                    context.LineTo(new System.Windows.Point(x1, y1), true, false);
                }
            }
            geometry.Freeze();

            Path path = new Path()
            {
                Stroke = new SolidColorBrush(Colors.LightGreen),
                StrokeThickness = 1,
                Data = geometry
            };

            CanvasPath.Children.Clear();
            CanvasPath.Children.Add(path);
            

            //PathFigure figure = new PathFigure
            //{
            //    StartPoint = new Point(0, datas[0]),
            //    IsClosed = false,
            //    IsFilled = false,
            //};

            //for (int i = 0; i < datas.Length; i++)
            //{
            //    int x1 = i;
            //    int y1 = datas[i];
            //    LineSegment line = new LineSegment
            //    {
            //        Point = new Point(x1, y1),
            //    };
            //    figure.Segments.Add(line);
            //}
            //PathGeometry geometry = new PathGeometry();
            //geometry.Figures.Add(figure);
            //Path path = new Path
            //{
            //    Fill = new SolidColorBrush(Colors.LightGreen),
            //    Stroke = new SolidColorBrush(Colors.LightGreen),
            //    StrokeThickness = 1,
            //    Data = geometry,
            //};
            //CanvasShape.Children.Clear();
            //CanvasShape.Children.Add(path);
        }

        private void DrawingImageShape(int[] datas)
        {
            CanvasShape.Children.Clear();
            for (int i = 0; i < datas.Length - 1; i++)
            {
                int x1 = i;
                int y1 = datas[i];
                int x2 = i + 1;
                int y2 = datas[i + 1];
                Line line = new Line()
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    StrokeThickness = 1,
                    Stroke = new SolidColorBrush(Colors.LightGreen),
                };

                CanvasShape.Children.Add(line);
            }
        }
    }
}