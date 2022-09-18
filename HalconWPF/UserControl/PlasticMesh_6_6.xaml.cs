using HalconDotNet;
using HalconWPF.Method;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// PlasticMesh_6_6.xaml 的交互逻辑
    /// </summary>
    public partial class PlasticMesh_6_6
    {
        private bool IsRunning { get; set; } = false;

        public PlasticMesh_6_6()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsRunning = !IsRunning;
            _ = Task.Run(() =>
              {
                  while (true)
                  {
                      for (int i = 1; i < 15; i++)
                      {
                          if (!IsRunning)
                          {
                              return;
                          }
                          HOperatorSet.ReadImage(out HObject ho_Image, @"Image\plastic_mesh\plastic_mesh_" + i.ToString("D2") + ".png");
                          HalconWPF.HalconWindow.ClearWindow();
                          HalconWPF.HalconWindow.SetDraw("fill");
                          HalconWPF.HalconWindow.SetLineWidth(2);
                          HalconWPF.HalconWindow.SetColor("orange red");
                          HalconWPF.HalconWindow.SetDisplayFont(16);
                          HalconWPF.HalconWindow.DispObj(ho_Image);
                          _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { HalconWPF.SetFullImagePart(); });

                          // 均值滤波
                          HOperatorSet.MeanImage(ho_Image, out HObject ho_ImageMean, 50, 50);
                          // 阈值分割
                          HOperatorSet.DynThreshold(ho_Image, ho_ImageMean, out HObject ho_Region, 5, "dark");
                          ho_Image.Dispose();
                          ho_ImageMean.Dispose();
                          // 连通
                          HOperatorSet.Connection(ho_Region, out HObject ho_Regions);
                          ho_Region.Dispose();
                          // 特征选择
                          HOperatorSet.SelectShape(ho_Regions, out HObject ho_RegionsDefeact, "area", "and", 500, 99999);
                          ho_Regions.Dispose();
                          // 计数
                          int count = ho_RegionsDefeact.CountObj();
                          if (count > 0)
                          {
                              HalconWPF.HalconWindow.DispText("NG", "image", 10, 10, "orange red", new HTuple(), new HTuple());
                              HalconWPF.HalconWindow.DispObj(ho_RegionsDefeact);
                          }
                          else
                          {
                              HalconWPF.HalconWindow.DispText("OK", "image", 10, 10, "green", new HTuple(), new HTuple());
                          }
                          ho_RegionsDefeact.Dispose();

                          Thread.Sleep(200);
                      }
                  }
              });
        }
    }
}