using HalconDotNet;
using System.Windows;
using System.Windows.Controls;

namespace HalconWPF.UserControl
{
    /// <summary>
    /// SvmCharacter_9_4.xaml 的交互逻辑
    /// </summary>
    public partial class SvmCharacter_9_4
    {
        public SvmCharacter_9_4()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Content.ToString();
            if (name == "训练")
            {
                // 字符类别
                HTuple hv_ClassNames = new HTuple();
                hv_ClassNames[0] = "A";
                hv_ClassNames[1] = "B";
                hv_ClassNames[2] = "C";
                hv_ClassNames[3] = "D";
                hv_ClassNames[4] = "E";
                hv_ClassNames[5] = "F";
                hv_ClassNames[6] = "G";

                // OCR 句柄
                HTuple hv_Features = new HTuple();
                hv_Features[0] = "convexity";
                hv_Features[1] = "num_holes";
                hv_Features[2] = "projection_horizontal";
                hv_Features[3] = "projection_vertical";
                HOperatorSet.CreateOcrClassSvm(8, 10, "constant", hv_Features, hv_ClassNames, "rbf", 0.02, 0.05, "one-versus-one", "normalization", 10, out HTuple hv_OCRHandle);

                // 7 张训练图
                for (int i = 0; i < 7; i++)
                {
                    HOperatorSet.ReadImage(out HObject ho_Image, @"Image\ocr\chars_training_" + (i + 1).ToString("D2") + ".png");
                    HalconWPF.HalconWindow.DispObj(ho_Image);
                    // 获取目标区域
                    HOperatorSet.Threshold(ho_Image, out HObject ho_Region, 0, 125);
                    HOperatorSet.Connection(ho_Region, out HObject ho_ConnectedRegions);
                    ho_Region.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions, "area", "and", 50, 999999);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.SortRegion(ho_SelectedRegions, out HObject ho_SortedRegions, "character", "true", "row");
                    ho_SelectedRegions.Dispose();

                    // 字符个数
                    HOperatorSet.CountObj(ho_SortedRegions, out HTuple hv_Count);
                    for (int j = 0; j < hv_Count; j++)
                    {
                        // 选择一个
                        HOperatorSet.SelectObj(ho_SortedRegions, out HObject ho_ObjectSelected, j + 1);
                        if (i == 0 && j == 0)
                        {
                            // 首次创建
                            HOperatorSet.WriteOcrTrainf(ho_ObjectSelected, ho_Image, hv_ClassNames[j], @"Model\A_G_ocr.trf");
                        }
                        else
                        {
                            // 而后追加
                            HOperatorSet.AppendOcrTrainf(ho_ObjectSelected, ho_Image, hv_ClassNames[j], @"Model\A_G_ocr.trf");
                        }
                        ho_ObjectSelected.Dispose();
                    }
                    ho_SortedRegions.Dispose();
                    ho_Image.Dispose();
                }

                // 检查训练文件是否正确
                HOperatorSet.ReadOcrTrainf(out HObject ho_Characters, @"Model\A_G_ocr.trf", out HTuple hv_CharacterNames);
                HOperatorSet.CountObj(ho_Characters, out HTuple hv_NumberCharacters);
                for (int i = 0; i < hv_NumberCharacters; i++)
                {
                    HOperatorSet.SelectObj(ho_Characters, out HObject ho_CharacterSelected, i + 1);
                    HalconWPF.HalconWindow.ClearWindow();
                    HalconWPF.HalconWindow.SetColor("orange red");
                    HalconWPF.HalconWindow.DispObj(ho_CharacterSelected);
                    HalconWPF.SetFullImagePart();
                    HalconWPF.HalconWindow.DispText(hv_CharacterNames[i], "image", 20, 20, "black", new HTuple(), new HTuple());
                    ho_CharacterSelected.Dispose();
                }
                ho_Characters.Dispose();

                // 训练字体，将字体写入到训练文件
                HOperatorSet.TrainfOcrClassSvm(hv_OCRHandle, @"Model\A_G_ocr.trf", 0.01, "default");
                HOperatorSet.ReduceOcrClassSvm(hv_OCRHandle, "bottom_up", 2, 0.001, out HTuple hv_OCRHandleReduced);
                HOperatorSet.WriteOcrClassSvm(hv_OCRHandleReduced, @"Model\A_G_ocr.osc");

                //释放内存
                HOperatorSet.ClearOcrClassSvm(hv_OCRHandle);
                HOperatorSet.ClearOcrClassSvm(hv_OCRHandleReduced);
            }
            else if (name == "测试")
            {
                HOperatorSet.FileExists(@"Model\A_G_ocr.osc", out HTuple hv_FileEites);
                // 训练文件是否存在
                if (hv_FileEites)
                {
                    // 读取训练模型到 OCR 句柄
                    HOperatorSet.ReadOcrClassSvm(@"Model\A_G_ocr.osc", out HTuple hv_OCRHandle);
                    // 读取图像
                    HOperatorSet.ReadImage(out HObject ho_Image, @"Image\ocr\chars_01.png");

                    // 获取目标区域
                    HOperatorSet.Threshold(ho_Image, out HObject ho_Region, 0, 125);
                    HOperatorSet.Connection(ho_Region, out HObject ho_ConnectedRegions);
                    ho_Region.Dispose();
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions, "area", "and", 50, 999999);
                    ho_ConnectedRegions.Dispose();
                    HOperatorSet.SortRegion(ho_SelectedRegions, out HObject ho_SortedRegions, "character", "true", "row");
                    ho_SelectedRegions.Dispose();

                    // 使用 SVM 分类器
                    HOperatorSet.DoOcrMultiClassSvm(ho_SortedRegions, ho_Image, hv_OCRHandle, out HTuple hv_Classes);

                    // 面积和中心位置
                    HOperatorSet.AreaCenter(ho_SortedRegions, out _, out HTuple hv_Rows, out HTuple hv_Cols);

                    // 显示
                    HalconWPF.HalconWindow.ClearWindow();
                    HalconWPF.HalconWindow.DispObj(ho_Image);
                    HalconWPF.HalconWindow.DispObj(ho_SortedRegions);
                    HalconWPF.SetFullImagePart();
                    ho_Image.Dispose();
                    ho_SortedRegions.Dispose();

                    for (int i = 0; i < hv_Classes.Length; i++)
                    {
                        HalconWPF.HalconWindow.DispText(hv_Classes[i].S, "image", hv_Rows[i].D - 80, hv_Cols[i], "black", new HTuple(), new HTuple());
                    }

                    // 清空内存
                    HOperatorSet.ClearOcrClassSvm(hv_OCRHandle);
                }
            }
        }
    }
}