using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Wpf_Base.ControlsWpf
{
    /// <summary>
    /// RichTextControl.xaml 的交互逻辑
    /// </summary>
    public partial class DispTextControl : UserControl
    {
        public DispTextControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 清空内容
        /// </summary>
        public void ClearText()
        {
            RTB_Logger.Document.Blocks.Clear();
        }

        /// <summary>
        /// 添加显示信息
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="brush"></param>
        /// <param name="fontsize"></param>
        public void AddText(string txt, double fontsize = 14, string flag = "OK")
        {
            _ = Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Run run = new Run
                {
                    Text = txt,
                    FontSize = fontsize,
                };
                run.Foreground = flag == "OK" ? Brushes.SpringGreen : Brushes.OrangeRed;
                Paragraph paragraph = new Paragraph(run)
                {
                    LineHeight = 2,
                };
                RTB_Logger.Document.Blocks.Add(paragraph);

                // 滚动至最后行
                RTB_Logger.ScrollToEnd();
            });
        }
    }
}
