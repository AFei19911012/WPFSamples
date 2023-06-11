using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WpfControlsX.Helper;

namespace WpfControlsX.ControlX
{
    public class WxPathAnimation : Shape
    {
        static WxPathAnimation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxPathAnimation), new FrameworkPropertyMetadata(typeof(WxPathAnimation)));

            _ = StretchProperty.AddOwner(typeof(WxPathAnimation), new FrameworkPropertyMetadata(Stretch.Uniform, OnPropertiesChanged));

            _ = StrokeThicknessProperty.AddOwner(typeof(WxPathAnimation), new FrameworkPropertyMetadata(ValueBoxes.Double1Box, OnPropertiesChanged));
        }

        protected override Geometry DefiningGeometry => Data ?? Geometry.Empty;

        private Storyboard _storyboard;

        private double _pathLength;

        /// <summary>
        /// 路径数据
        /// </summary>
        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data),
            typeof(Geometry), typeof(WxPathAnimation), new FrameworkPropertyMetadata(null, OnPropertiesChanged));

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WxPathAnimation path)
            {
                path.UpdatePath();
            }
        }


        /// <summary>
        /// 路径长度
        /// </summary>
        public double PathLength
        {
            get => (double)GetValue(PathLengthProperty);
            set => SetValue(PathLengthProperty, value);
        }
        public static readonly DependencyProperty PathLengthProperty = DependencyProperty.Register(
            nameof(PathLength), typeof(double), typeof(WxPathAnimation), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, OnPropertiesChanged));


        /// <summary>
        /// 动画持续时间
        /// </summary>
        public Duration Duration
        {
            get => (Duration)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            nameof(Duration), typeof(Duration), typeof(WxPathAnimation), new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(2)), OnPropertiesChanged));


        /// <summary>
        /// 是否执行
        /// </summary>
        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, ValueBoxes.BooleanBox(value));
        }
        public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(
            nameof(IsPlaying), typeof(bool), typeof(WxPathAnimation), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, (o, args) =>
            {
                WxPathAnimation ctl = (WxPathAnimation)o;
                bool v = (bool)args.NewValue;
                if (v)
                {
                    ctl.UpdatePath();
                }
                else
                {
                    ctl._storyboard?.Pause();
                }
            }));


        /// <summary>
        /// 循环模式
        /// </summary>
        public RepeatBehavior RepeatBehavior
        {
            get => (RepeatBehavior)GetValue(RepeatBehaviorProperty);
            set => SetValue(RepeatBehaviorProperty, value);
        }
        public static readonly DependencyProperty RepeatBehaviorProperty =
            Timeline.RepeatBehaviorProperty.AddOwner(typeof(WxPathAnimation), new PropertyMetadata(RepeatBehavior.Forever));


        /// <summary>
        /// 填充模式
        /// </summary>
        public FillBehavior FillBehavior
        {
            get => (FillBehavior)GetValue(FillBehaviorProperty);
            set => SetValue(FillBehaviorProperty, value);
        }
        public static readonly DependencyProperty FillBehaviorProperty =
            Timeline.FillBehaviorProperty.AddOwner(typeof(WxPathAnimation), new PropertyMetadata(FillBehavior.Stop));


        /// <summary>
        /// 构造函数
        /// </summary>
        public WxPathAnimation()
        {
            Loaded += (s, e) => UpdatePath();
        }

        public static readonly RoutedEvent CompletedEvent =
            EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(WxPathAnimation));

        public event EventHandler Completed
        {
            add => AddHandler(CompletedEvent, value);
            remove => RemoveHandler(CompletedEvent, value);
        }

        private void UpdatePath()
        {
            if (!Duration.HasTimeSpan || !IsPlaying)
            {
                return;
            }

            _pathLength = PathLength > 0 ? PathLength : Data.GetTotalLength(new Size(ActualWidth, ActualHeight), StrokeThickness);

            if (MathHelper.IsVerySmall(_pathLength))
            {
                return;
            }

            StrokeDashOffset = _pathLength;
            StrokeDashArray = new DoubleCollection(new List<double>
                {
                    _pathLength,
                    _pathLength
                });

            if (_storyboard != null)
            {
                _storyboard.Stop();
                _storyboard.Completed -= Storyboard_Completed;
            }
            _storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior,
                FillBehavior = FillBehavior
            };
            _storyboard.Completed += Storyboard_Completed;

            DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames();

            LinearDoubleKeyFrame frameIn = new LinearDoubleKeyFrame
            {
                Value = _pathLength,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
            };
            _ = frames.KeyFrames.Add(frameIn);

            LinearDoubleKeyFrame frameOut = new LinearDoubleKeyFrame
            {
                Value = FillBehavior == FillBehavior.Stop ? -_pathLength : 0,
                KeyTime = KeyTime.FromTimeSpan(Duration.TimeSpan)
            };
            _ = frames.KeyFrames.Add(frameOut);

            Storyboard.SetTarget(frames, this);
            Storyboard.SetTargetProperty(frames, new PropertyPath(StrokeDashOffsetProperty));
            _storyboard.Children.Add(frames);

            _storyboard.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CompletedEvent));
        }
    }
}