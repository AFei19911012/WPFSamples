using System.Windows;
using System.Windows.Controls;

namespace WpfControlsX.ControlX
{
    public class WxDataGrid : DataGrid
    {
        static WxDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WxDataGrid), new FrameworkPropertyMetadata(typeof(WxDataGrid)));
        }

        /// <summary>
        /// 显示全选按钮
        /// </summary>
        public bool ShowSelectAllIcon
        {
            get => (bool)GetValue(ShowSelectAllIconProperty);
            set => SetValue(ShowSelectAllIconProperty, value);
        }

        public static readonly DependencyProperty ShowSelectAllIconProperty =
            DependencyProperty.Register("ShowSelectAllIcon", typeof(bool), typeof(WxDataGrid), new PropertyMetadata(true, OnPropertyChanged));


        /// <summary>
        /// 全选按钮大小
        /// </summary>
        public double SelectAllIconSize
        {
            get => (double)GetValue(SelectAllIconSizeProperty);
            set => SetValue(SelectAllIconSizeProperty, value);
        }

        public static readonly DependencyProperty SelectAllIconSizeProperty =
            DependencyProperty.Register("SelectAllIconSize", typeof(double), typeof(WxDataGrid), new PropertyMetadata(14.0, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(e.Property, e.NewValue);
        }


        /// <summary>
        /// 显示行号
        /// </summary>
        public bool ShowRowIndex
        {
            get => (bool)GetValue(ShowRowIndexProperty);
            set => SetValue(ShowRowIndexProperty, value);
        }

        public static readonly DependencyProperty ShowRowIndexProperty =
            DependencyProperty.Register("ShowRowIndex", typeof(bool), typeof(WxDataGrid), new PropertyMetadata(false, OnShowRowIndexChanged));

        private static void OnShowRowIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not WxDataGrid grid)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                grid.LoadingRow += (sender, ee) => { RefreshDataGridRowNumbers(sender); };
                grid.UnloadingRow += (sender, ee) => { RefreshDataGridRowNumbers(sender); };
            }
            else
            {
                grid.LoadingRow -= (sender, ee) => { RefreshDataGridRowNumbers(sender); };
                grid.UnloadingRow -= (sender, ee) => { RefreshDataGridRowNumbers(sender); };
            }
        }

        private static void RefreshDataGridRowNumbers(object sender)
        {
            if (sender is not WxDataGrid grid)
            {
                return;
            }

            foreach (object item in grid.Items)
            {
                DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    row.Header = row.GetIndex();
                }
            }
        }


        /// <summary>
        /// 模板中的 Button
        /// </summary>
        private WxButton SelectAllButton { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //ScrollViewer view = GetTemplateChild("DG_ScrollViewer") as ScrollViewer;
            //DependencyObject d = VisualTreeHelper.GetChild(view, 0);
            //SelectAllButton = LogicalTreeHelper.FindLogicalNode(d, "SelectAllButton") as WxButton;

            SelectAllButton = GetTemplateChild("SelectAllButton") as WxButton;
            if (SelectAllButton != null)
            {
                SelectAllButton.Click += (sender, e) => { SelectAll(); };
            }
        }
    }
}