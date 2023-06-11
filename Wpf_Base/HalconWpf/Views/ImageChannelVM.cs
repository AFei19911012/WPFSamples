using GalaSoft.MvvmLight;
using HalconDotNet;
using Wpf_Base.HalconWpf.Model;

namespace Wpf_Base.HalconWpf.Views
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 13:26:10
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 13:26:10    CoderMan/CoderdMan1012         首次编写         
    ///
    public class ImageChannelVM : ViewModelBase
    {
        private EnumImageChannel enumChannel = EnumImageChannel.Gray;
        public EnumImageChannel EnumChannel
        {
            get => enumChannel;
            set => Set(ref enumChannel, value);
        }

        public void TransHImage(ref HObject ho_Image)
        {
            // 判断图像类型
            HOperatorSet.CountChannels(ho_Image, out HTuple hv_Channels);
            if (hv_Channels == 1)
            {
                EnumChannel = EnumImageChannel.Gray;
            }
            else if (hv_Channels == 3)
            {
                // 根据选择的通道显示对应图像
                if (EnumChannel == EnumImageChannel.RGB)
                {
                    return;
                }
                else if (EnumChannel == EnumImageChannel.Gray)
                {
                    HOperatorSet.Rgb1ToGray(ho_Image, out HObject ho_GrayImage);
                    ho_Image.Dispose();
                    HOperatorSet.CopyImage(ho_GrayImage, out ho_Image);
                    ho_GrayImage.Dispose();
                }
                else if ((int)EnumChannel > 1)
                {
                    HOperatorSet.Decompose3(ho_Image, out HObject ho_ImageR, out HObject ho_ImageG, out HObject ho_ImageB);
                    ho_Image.Dispose();
                    if (EnumChannel == EnumImageChannel.R)
                    {
                        HOperatorSet.CopyImage(ho_ImageR, out ho_Image);
                    }
                    else if (EnumChannel == EnumImageChannel.G)
                    {
                        HOperatorSet.CopyImage(ho_ImageG, out ho_Image);
                    }
                    else if (EnumChannel == EnumImageChannel.B)
                    {
                        HOperatorSet.CopyImage(ho_ImageB, out ho_Image);
                    }
                    else
                    {
                        HOperatorSet.TransFromRgb(ho_ImageR, ho_ImageG, ho_ImageB, out HObject ho_ImageH, out HObject ho_ImageS, out HObject ho_ImageV, "hsv");
                        if (EnumChannel == EnumImageChannel.H)
                        {
                            HOperatorSet.CopyImage(ho_ImageH, out ho_Image);
                        }
                        else if (EnumChannel == EnumImageChannel.S)
                        {
                            HOperatorSet.CopyImage(ho_ImageS, out ho_Image);
                        }
                        else if (EnumChannel == EnumImageChannel.V)
                        {
                            HOperatorSet.CopyImage(ho_ImageV, out ho_Image);
                        }
                        ho_ImageH.Dispose();
                        ho_ImageS.Dispose();
                        ho_ImageV.Dispose();
                    }
                    ho_ImageR.Dispose();
                    ho_ImageG.Dispose();
                    ho_ImageB.Dispose();
                }
            }
        }
    }
}