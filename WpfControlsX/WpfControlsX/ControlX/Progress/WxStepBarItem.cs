using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/4/4 22:32:06
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/4/4 22:32:06                     BigWang         首次编写         
    ///
    public class WxStepBarItem : ContentControl
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get => (int)GetValue(IndexProperty);
            internal set => SetValue(IndexProperty, value);
        }
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(WxStepBarItem), new PropertyMetadata(-1));


        /// <summary>
        /// 当前状态
        /// </summary>

        public StepBarState State
        {
            get => (StepBarState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(StepBarState), typeof(WxStepBarItem), new PropertyMetadata(StepBarState.Default));


        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxStepBarItem), new PropertyMetadata(12d));

        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WxStepBarItem), new PropertyMetadata(new CornerRadius(0)));
    }
}