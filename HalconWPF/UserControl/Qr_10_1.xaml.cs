using HalconDotNet;
using System.Windows;
using System.Windows.Controls;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// Qr_10_1.xaml 的交互逻辑
    /// </summary>
    public partial class Qr_10_1
    {
        public Qr_10_1()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 读取图像
            HOperatorSet.ReadImage(out HObject ho_image, @"Image\qr1.bmp");
            // 创建二维码模型
            HOperatorSet.CreateDataCode2dModel("QR Code", new HTuple(), new HTuple(), out HTuple hv_DataCodeHandle);
            // 寻找二维码并解码
            HOperatorSet.FindDataCode2d(ho_image, out HObject ho_SymbolXLDs, hv_DataCodeHandle, new HTuple(), new HTuple(), out HTuple hv_ResultHandles, out HTuple hv_DecodedDataStrings);
            // 清除模型
            HOperatorSet.ClearDataCode2dModel(hv_DataCodeHandle);
            // 显示结果
            HalconWPF.HalconWindow.ClearWindow();
            HalconWPF.HalconWindow.SetColor("green");
            HalconWPF.HalconWindow.SetLineWidth(3);
            HalconWPF.HalconWindow.DispObj(ho_image);
            HalconWPF.SetFullImagePart();
            HalconWPF.HalconWindow.DispObj(ho_SymbolXLDs);
            HalconWPF.HalconWindow.DispText(hv_DecodedDataStrings.S, "image", 20, 20, "black", new HTuple(), new HTuple());
            ho_image.Dispose();
            ho_SymbolXLDs.Dispose();
        }
    }
}