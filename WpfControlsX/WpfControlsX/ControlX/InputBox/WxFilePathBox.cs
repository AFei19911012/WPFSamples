using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfControlsX.Commands;

namespace WpfControlsX.ControlX
{
    public class WxFilePathBox : TextBox
    {
        static WxFilePathBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxFilePathBox), new FrameworkPropertyMetadata(typeof(WxFilePathBox)));
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxFilePathBox), new PropertyMetadata(new CornerRadius(0)));


        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(WxFilePathBox), new PropertyMetadata(null));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxFilePathBox), new PropertyMetadata(10d));

        /// <summary>
        /// 左侧标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxFilePathBox), new PropertyMetadata(null));

        /// <summary>
        /// 左侧标题宽度
        /// </summary>
        public double TitleWidth
        {
            get => (double)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }
        public static readonly DependencyProperty TitleWidthProperty =
            DependencyProperty.Register("TitleWidth", typeof(double), typeof(WxFilePathBox), new PropertyMetadata(double.NaN));


        /// <summary>
        /// 左侧标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(WxFilePathBox), new PropertyMetadata(Brushes.Transparent));


        public WxFilePathBox()
        {
            // 注册事件
            _ = CommandBindings.Add(new CommandBinding(ControlCommands.Open, (s, e) => OnOpenStarted()));
        }

        public static readonly RoutedEvent OpenStartedEvent =
            EventManager.RegisterRoutedEvent("OpenStarted", RoutingStrategy.Bubble, typeof(EventHandler<FunctionEventArgs<string>>), typeof(WxFilePathBox));

        public event EventHandler<FunctionEventArgs<string>> OpenStarted
        {
            add => AddHandler(OpenStartedEvent, value);
            remove => RemoveHandler(OpenStartedEvent, value);
        }

        private void OnOpenStarted()
        {
            RaiseEvent(new FunctionEventArgs<string>(OpenStartedEvent, this)
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


        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(WxFilePathBox), new PropertyMetadata(default(ICommand), OnCommandChanged));


        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(WxFilePathBox), new PropertyMetadata(default(object)));


        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            nameof(CommandTarget), typeof(IInputElement), typeof(WxFilePathBox), new PropertyMetadata(default(IInputElement)));


        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WxFilePathBox ctl = (WxFilePathBox)d;
            if (e.OldValue is ICommand oldCommand)
            {
                oldCommand.CanExecuteChanged -= ctl.CanExecuteChanged;
            }
            if (e.NewValue is ICommand newCommand)
            {
                newCommand.CanExecuteChanged += ctl.CanExecuteChanged;
            }
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