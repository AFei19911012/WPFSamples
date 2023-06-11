using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfControlsX.ControlX
{
    public class WxRangeSlider : Control
    {
        static WxRangeSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxRangeSlider), new FrameworkPropertyMetadata(typeof(WxRangeSlider)));
        }

        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxRangeSlider), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxRangeSlider), new PropertyMetadata(10d));


        /// <summary>
        /// 滑动条高度
        /// </summary>
        public double SliderHeight
        {
            get => (double)GetValue(SliderHeightProperty);
            set => SetValue(SliderHeightProperty, value);
        }
        public static readonly DependencyProperty SliderHeightProperty =
            DependencyProperty.Register("SliderHeight", typeof(double), typeof(WxRangeSlider), new PropertyMetadata(5d));


        /// <summary>
        /// 最小值
        /// </summary>
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(WxRangeSlider), new PropertyMetadata(0d));


        /// <summary>
        /// 最大值
        /// </summary>
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(WxRangeSlider), new PropertyMetadata(100d));



        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue
        {
            get => (double)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(WxRangeSlider), new PropertyMetadata(25d, OnMinValueChanged));
        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WxRangeSlider element)
            {
                element.MinValuePropertyChanged();
            }
        }


        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(WxRangeSlider), new PropertyMetadata(75d, OnMaxValueChanged));

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WxRangeSlider element)
            {
                element.MaxValuePropertyChanged();
            }
        }

        /// <summary>
        /// 小步长
        /// </summary>
        public double SmallChange
        {
            get => (double)GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }
        public static readonly DependencyProperty SmallChangeProperty =
            DependencyProperty.Register("SmallChange", typeof(double), typeof(WxRangeSlider), new PropertyMetadata(1d));


        /// <summary>
        /// 大步长
        /// </summary>
        public double LargeChange
        {
            get => (double)GetValue(LargeChangeProperty);
            set => SetValue(LargeChangeProperty, value);
        }
        public static readonly DependencyProperty LargeChangeProperty =
            DependencyProperty.Register("LargeChange", typeof(double), typeof(WxRangeSlider), new PropertyMetadata(10d));



        private double OffsetFrom { get; set; } = -1;
        private double OffsetTo { get; set; } = -1;
        private bool PART_FromPressed { get; set; } = false;
        private bool PART_ToPressed { get; set; } = false;
        private Path PART_From { get; set; }
        private Path PART_To { get; set; }
        private Rectangle PART_RectBackground { get; set; }
        private Rectangle PART_RectFill { get; set; }
        private double OriValue { get; set; }
        private bool PART_RectFillPressed { get; set; } = false;
        private double OriMinValue { get; set; }
        private double OriMaxValue { get; set; }


        /// <summary>
        /// 应用控件模板时调用
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_RectBackground = GetTemplateChild("PART_RectBackground") as Rectangle;
            PART_RectFill = GetTemplateChild("PART_RectFill") as Rectangle;
            PART_From = GetTemplateChild("PART_From") as Path;
            PART_To = GetTemplateChild("PART_To") as Path;

            // 注册事件
            PART_From.PreviewMouseLeftButtonDown += PART_From_PreviewMouseLeftButtonDown;
            PART_From.MouseLeftButtonUp += PART_From_MouseLeftButtonUp;
            PART_To.PreviewMouseLeftButtonDown += PART_To_PreviewMouseLeftButtonDown;
            PART_To.MouseLeftButtonUp += PART_To_MouseLeftButtonUp;
            PART_From.MouseMove += PART_Path_MouseMove;
            PART_To.MouseMove += PART_Path_MouseMove;

            // 初始化状态
            PART_RectBackground.Loaded += PART_RectBackground_Loaded;
            // 大步长
            PART_RectBackground.MouseLeftButtonDown += PART_RectBackground_MouseLeftButtonDown;

            PART_RectFill.PreviewMouseLeftButtonDown += PART_RectFill_PreviewMouseLeftButtonDown;
            PART_RectFill.MouseMove += PART_RectFill_MouseMove;
            PART_RectFill.MouseLeftButtonUp += PART_RectFill_MouseLeftButtonUp;
        }

        private void PART_RectFill_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PART_RectFill.ReleaseMouseCapture();
            PART_RectFillPressed = false;
        }

        private void PART_RectFill_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            double percentage = e.GetPosition(PART_RectBackground).X / PART_RectBackground.ActualWidth;
            double value = (Maximum - Minimum) * percentage + Minimum;
            if (PART_RectFillPressed)
            {
                if (value - OriValue >= SmallChange)
                {
                    MinValue = OriMinValue + SmallChange;
                    MaxValue = OriMaxValue + SmallChange;
                    OriMinValue = MinValue;
                    OriMaxValue = MaxValue;
                    OriValue = value;
                }
                if (OriValue - value >= SmallChange)
                {
                    MinValue = OriMinValue - SmallChange;
                    MaxValue = OriMaxValue - SmallChange;
                    OriMinValue = MinValue;
                    OriMaxValue = MaxValue;
                    OriValue = value;
                }
            }
        }

        private void PART_RectFill_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PART_RectFill.CaptureMouse();
            PART_RectFillPressed = true;

            double percentage = e.GetPosition(PART_RectBackground).X / PART_RectBackground.ActualWidth;
            OriValue = (Maximum - Minimum) * percentage + Minimum;
            OriMinValue = MinValue;
            OriMaxValue = MaxValue;
        }

        private void PART_RectBackground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x1 = e.GetPosition(PART_From).X;
            double x2 = e.GetPosition(PART_To).X;
            // 左侧还是右侧
            if (x1 < 0)
            {
                MinValue = Math.Max(Minimum, MinValue - LargeChange);
            }
            else if (x2 > 0)
            {
                MaxValue = Math.Min(Maximum, MaxValue + LargeChange);
            }
        }

        private void PART_RectBackground_Loaded(object sender, RoutedEventArgs e)
        {
            MinValuePropertyChanged();
            MaxValuePropertyChanged();
        }


        private void PART_From_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PART_From.CaptureMouse();
            PART_FromPressed = true;

            OriValue = MinValue;
        }

        private void PART_To_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PART_To.CaptureMouse();
            PART_ToPressed = true;

            OriValue = MaxValue;
        }

        private void PART_From_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PART_From.ReleaseMouseCapture();
            PART_FromPressed = false;
        }

        private void PART_To_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PART_To.ReleaseMouseCapture();
            PART_ToPressed = false;
        }

        private void PART_Path_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            double percentage = e.GetPosition(PART_RectBackground).X / PART_RectBackground.ActualWidth;
            double value = (Maximum - Minimum) * percentage + Minimum;
            // 判断方向 按照小步长计算
            if (PART_FromPressed)
            {
                if (value - MinValue >= SmallChange)
                {
                    MinValue = OriValue + SmallChange;
                    OriValue = MinValue;
                }
                if (MinValue - value >= SmallChange)
                {
                    MinValue = OriValue - SmallChange;
                    OriValue = MinValue;
                }
            }
            if (PART_ToPressed)
            {
                if (value - MaxValue >= SmallChange)
                {
                    MaxValue = OriValue + SmallChange;
                    OriValue = MaxValue;
                }
                if (MaxValue - value >= SmallChange)
                {
                    MaxValue = OriValue - SmallChange;
                    OriValue = MaxValue;
                }
            }
        }

        //最小值变化
        private void MinValuePropertyChanged()
        {
            if (MinValue < Minimum)
            {
                MinValue = Minimum;
                return;
            }
            if (MinValue > MaxValue)
            {
                MaxValue = MinValue;
            }
            OffsetFrom = (MinValue - Minimum) * PART_RectBackground.ActualWidth / (Maximum - Minimum);
            Canvas.SetLeft(PART_From, OffsetFrom);

            if (OffsetTo > -1)
            {
                double diff = OffsetTo - OffsetFrom;
                if (diff >= 0)
                {
                    PART_RectFill.Width = diff;
                    Canvas.SetLeft(PART_RectFill, OffsetFrom);
                }
            }
        }

        //最大值变化
        private void MaxValuePropertyChanged()
        {
            if (MaxValue > Maximum)
            {
                MaxValue = Maximum;
                return;
            }
            if (MaxValue < MinValue)
            {
                MinValue = MaxValue;
            }
            OffsetTo = (MaxValue - Minimum) * PART_RectBackground.ActualWidth / (Maximum - Minimum);
            Canvas.SetLeft(PART_To, OffsetTo);

            if (OffsetFrom > -1)
            {
                double diff = OffsetTo - OffsetFrom;
                if (diff >= 0)
                {
                    PART_RectFill.Width = diff;
                    Canvas.SetLeft(PART_RectFill, OffsetFrom);
                }
            }
        }
    }
}