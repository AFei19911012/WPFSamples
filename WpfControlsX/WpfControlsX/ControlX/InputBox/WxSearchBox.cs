using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfControlsX.Commands;

namespace WpfControlsX.ControlX
{
    public class WxSearchBox : TextBox, ICommandSource
    {
        static WxSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxSearchBox), new FrameworkPropertyMetadata(typeof(WxSearchBox)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxSearchBox), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// 左侧标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxSearchBox), new PropertyMetadata(null));

        /// <summary>
        /// 左侧标题宽度
        /// </summary>
        public double TitleWidth
        {
            get => (double)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }
        public static readonly DependencyProperty TitleWidthProperty =
            DependencyProperty.Register("TitleWidth", typeof(double), typeof(WxSearchBox), new PropertyMetadata(double.NaN));

        /// <summary>
        /// 左侧标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(WxSearchBox), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxSearchBox), new PropertyMetadata(10d));

        /// <summary>
        /// 水印内容
        /// </summary>
        public string WaterMark
        {
            get => (string)GetValue(WaterMarkProperty);
            set => SetValue(WaterMarkProperty, value);
        }
        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(WxSearchBox), new PropertyMetadata(null));

        /// <summary>
        /// 存在文本内容
        /// </summary>
        public bool HasText
        {
            get => (bool)GetValue(HasTextProperty);
            set => SetValue(HasTextProperty, value);
        }
        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.Register("HasText", typeof(bool), typeof(WxSearchBox), new PropertyMetadata());


        public WxSearchBox()
        {
            // 注册事件
            _ = CommandBindings.Add(new CommandBinding(ControlCommands.Search, (s, e) => OnSearchStarted()));
        }

        public static readonly RoutedEvent SearchStartedEvent =
            EventManager.RegisterRoutedEvent("SearchStarted", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<string>>), typeof(WxSearchBox));

        public event EventHandler<FunctionEventArgs<string>> SearchStarted
        {
            add => AddHandler(SearchStartedEvent, value);
            remove => RemoveHandler(SearchStartedEvent, value);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Enter)
            {
                OnSearchStarted();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            HasText = !string.IsNullOrEmpty(Text);

            if (IsRealTime)
            {
                OnSearchStarted();
            }
        }

        private void OnSearchStarted()
        {
            RaiseEvent(new FunctionEventArgs<string>(SearchStartedEvent, this)
            {
                Info = Text
            });

            switch (Command)
            {
                case null:
                    return;
                case RoutedCommand command:
                    command.Execute(CommandParameter, CommandTarget);
                    break;
                default:
                    Command.Execute(CommandParameter);
                    break;
            }
        }

        /// <summary>
        ///     是否实时搜索
        /// </summary>
        public static readonly DependencyProperty IsRealTimeProperty = DependencyProperty.Register(
            nameof(IsRealTime), typeof(bool), typeof(WxSearchBox), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        ///     是否实时搜索
        /// </summary>
        public bool IsRealTime
        {
            get => (bool)GetValue(IsRealTimeProperty);
            set => SetValue(IsRealTimeProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(WxSearchBox), new PropertyMetadata(default(ICommand), OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WxSearchBox ctl = (WxSearchBox)d;
            if (e.OldValue is ICommand oldCommand)
            {
                oldCommand.CanExecuteChanged -= ctl.CanExecuteChanged;
            }
            if (e.NewValue is ICommand newCommand)
            {
                newCommand.CanExecuteChanged += ctl.CanExecuteChanged;
            }
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(WxSearchBox), new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            nameof(CommandTarget), typeof(IInputElement), typeof(WxSearchBox), new PropertyMetadata(default(IInputElement)));

        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if (Command == null)
            {
                return;
            }

            IsEnabled = Command is RoutedCommand command
                ? command.CanExecute(CommandParameter, CommandTarget)
                : Command.CanExecute(CommandParameter);
        }
    }

}