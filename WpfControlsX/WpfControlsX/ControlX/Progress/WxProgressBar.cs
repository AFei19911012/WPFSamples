using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfControlsX.ControlX
{
    public class WxProgressBar : ProgressBar
    {
        static WxProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxProgressBar), new FrameworkPropertyMetadata(typeof(WxProgressBar)));
        }

        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxProgressBar), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 左侧标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxProgressBar), new PropertyMetadata(null));

        /// <summary>
        /// 左侧标题宽度
        /// </summary>
        public double TitleWidth
        {
            get => (double)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }
        public static readonly DependencyProperty TitleWidthProperty =
            DependencyProperty.Register("TitleWidth", typeof(double), typeof(WxProgressBar), new PropertyMetadata(double.NaN));


        /// <summary>
        /// 左侧标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(WxProgressBar), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 样式类型
        /// </summary>
        public ProgressBarType ProgressBarType
        {
            get => (ProgressBarType)GetValue(ProgressBarTypeProperty);
            set => SetValue(ProgressBarTypeProperty, value);
        }

        public static readonly DependencyProperty ProgressBarTypeProperty =
            DependencyProperty.Register("ProgressBarType", typeof(ProgressBarType), typeof(WxProgressBar), new PropertyMetadata(ProgressBarType.Normal));



        /// <summary>
        /// 圆弧厚度
        /// </summary>
        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(WxProgressBar), new PropertyMetadata(10.0, OnThicknessChanged));

        private static void OnThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WxProgressBar obj = d as WxProgressBar;
            obj.Thickness = (double)e.NewValue;
        }


        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(WxProgressBar), new PropertyMetadata(100.0));


        /// <summary>
        /// 显示文字
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WxProgressBar), new PropertyMetadata(null));


        private void WxProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            WxProgressBar obj = sender as WxProgressBar;
            Path_Angle.Data = DrawArcSegment(0, e.NewValue / obj.Maximum * 359.999, Radius);
        }

        private Path Path_Angle { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Path Path_Circle = GetTemplateChild("PART_PathCircle") as Path;
            Path_Angle = GetTemplateChild("PART_PathAngle") as Path;
            if (Path_Circle != null)
            {
                // 绘制背景圆
                // 注意留一点空隙
                Path_Circle.Data = DrawArcSegment(0, 359.999, Radius);
                Path_Angle.Data = DrawArcSegment(0, Math.Min(Value * 360, 359.999), Radius);

                ValueChanged += WxProgressBar_ValueChanged;
            }
        }

        /// <summary>
        /// 画弧形
        /// </summary>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private Geometry DrawArcSegment(double angle1, double angle2, double r)
        {
            // 角度 → 弧度
            double arc1 = angle1 * Math.PI / 180;
            double arc2 = angle2 * Math.PI / 180;

            // 圆心坐标 (r, r)
            // 起点、终点坐标
            Point p1 = new Point(r - r * Math.Cos(arc1), r - r * Math.Sin(arc1));
            Point p2 = new Point(r - r * Math.Cos(arc2), r - r * Math.Sin(arc2));

            // 添加元素：自定义路径
            PathFigure figure = new PathFigure
            {
                StartPoint = p1,
                IsClosed = false,
                IsFilled = false,
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

            // 显示
            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            return geometry;
        }
    }
}