using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsX.ControlX
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/3/4 19:03:02
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/3/4 19:03:02                     BigWang         首次编写         
    ///
    public class WxCheckComboBox : ComboBox
    {
        static WxCheckComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxCheckComboBox), new FrameworkPropertyMetadata(typeof(WxCheckComboBox)));
        }

        /// <summary>
        /// 图标尺寸
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(WxCheckComboBox), new PropertyMetadata(0d));


        /// <summary>
        /// 左侧标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WxCheckComboBox), new PropertyMetadata(null));

        /// <summary>
        /// 左侧标题宽度
        /// </summary>
        public double TitleWidth
        {
            get => (double)GetValue(TitleWidthProperty);
            set => SetValue(TitleWidthProperty, value);
        }
        public static readonly DependencyProperty TitleWidthProperty =
            DependencyProperty.Register("TitleWidth", typeof(double), typeof(WxCheckComboBox), new PropertyMetadata(double.NaN));


        /// <summary>
        /// 左侧标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get => (Brush)GetValue(TitleBackgroundProperty);
            set => SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(WxCheckComboBox), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 选中项列表
        /// </summary>
        public ObservableCollection<WxComboBoxBaseData> ChekedItems = new ObservableCollection<WxComboBoxBaseData>();

        /// <summary>
        /// ListBox 竖向列表
        /// </summary>
        private ListBox ListBoxV { get; set; }

        /// <summary>
        /// ListBox 横向列表
        /// </summary>
        private ListBox ListBoxH { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ListBoxV = Template.FindName("PART_ListBox", this) as ListBox;
            ListBoxH = Template.FindName("PART_ListBoxChecked", this) as ListBox;
            if (ListBoxV != null)
            {
                ListBoxV.SelectionChanged += ListBoxV_SelectionChanged;
            }
            if (ListBoxH != null)
            {
                ListBoxH.ItemsSource = ChekedItems;
                ListBoxH.SelectionChanged += ListBoxH_SelectionChanged;
            }

            if (ItemsSource != null)
            {
                foreach (object item in ItemsSource)
                {
                    WxComboBoxBaseData data = item as WxComboBoxBaseData;
                    if (data.IsChecked)
                    {
                        ListBoxV.SelectedItems.Add(data);
                    }
                }
            }
        }

        private void ListBoxH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object item in e.RemovedItems)
            {
                WxComboBoxBaseData datachk = item as WxComboBoxBaseData;

                for (int i = 0; i < ListBoxV.SelectedItems.Count; i++)
                {
                    WxComboBoxBaseData datachklist = ListBoxV.SelectedItems[i] as WxComboBoxBaseData;
                    if (datachklist.ID == datachk.ID)
                    {
                        ListBoxV.SelectedItems.Remove(ListBoxV.SelectedItems[i]);
                    }
                }
            }
        }

        private void ListBoxV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object item in e.AddedItems)
            {
                WxComboBoxBaseData data = item as WxComboBoxBaseData;
                data.IsChecked = true;
                if (ChekedItems.IndexOf(data) < 0)
                {
                    ChekedItems.Add(data);
                }
            }

            foreach (object item in e.RemovedItems)
            {
                WxComboBoxBaseData data = item as WxComboBoxBaseData;
                data.IsChecked = false;
                ChekedItems.Remove(data);
            }
        }
    }
}
