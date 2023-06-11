using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    [TemplatePart(Name = ProgressBarTemplateName, Type = typeof(ProgressBar))]
    public class WxStepBar : ItemsControl
    {
        static WxStepBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxStepBar), new FrameworkPropertyMetadata(typeof(WxStepBar)));
        }

        private const string ProgressBarTemplateName = "PART_ProgressBar";
        private ProgressBar _progressBar;
        public int StepIndex
        {
            get => (int)GetValue(StepIndexProperty);
            set => SetValue(StepIndexProperty, value);
        }
        public static readonly DependencyProperty StepIndexProperty =
            DependencyProperty.Register("StepIndex", typeof(int), typeof(WxStepBar), new PropertyMetadata(0, OnStepIndexChanged));

        private static void OnStepIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WxStepBar step = (WxStepBar)d;
            int stepIndex = (int)e.NewValue;
            step.UpdateStepItemState(stepIndex);
        }

        private void UpdateStepItemState(int stepIndex)
        {
            int count = Items.Count;
            if (count <= 0)
            {
                return;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is WxStepBarItem stepItem)
                {
                    stepItem.State = i < stepIndex ? StepBarState.Complete : i == stepIndex ? StepBarState.Busy : StepBarState.Default;
                }
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _progressBar = GetTemplateChild(ProgressBarTemplateName) as ProgressBar;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            int count = Items.Count;
            if (_progressBar == null || count <= 0)
            {
                return;
            }

            _progressBar.Maximum = count - 1;
            _progressBar.Value = StepIndex;
            _progressBar.Width = ActualWidth / count * (count - 1);


        }
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is WxStepBarItem;
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new WxStepBarItem();
        }
        public WxStepBar()
        {
            ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        public void Next()
        {
            if (StepIndex >= Items.Count)
            {
                StepIndex = Items.Count - 1;
            }
            StepIndex++;
        }


        public void Prev()
        {
            if (StepIndex < 0)
            {
                StepIndex = -1;
            }
            else
            {
                StepIndex--;
            }
        }


        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                int count = Items.Count;
                if (count <= 0)
                {
                    return;
                }

                UpdateStepItemState(StepIndex);
            }
        }
    }
}