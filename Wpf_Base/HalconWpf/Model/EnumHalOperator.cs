using System;
using System.ComponentModel;

namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/05 20:28:55
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/05 20:28:55    CoderMan/CoderdMan1012         首次编写         
    ///
    [Serializable]
    public enum EnumHalOperator
    {
        [Description("清空窗体内容")]
        ClearWindow = 0,

        [Description("显示图像")]
        DispImg,
        [Description("显示对象")]
        DispObj,
        [Description("设置颜色")]
        SetColor,
        [Description("显示：填充、轮廓")]
        SetDraw,
        [Description("设置线宽")]
        SetLineWidth,

        [Description("加载图像")]
        ReadImage,
        [Description("保存图片")]
        WriteImage,

        [Description("面积和中心点")]
        AreaCenter,

        [Description("阈值分割")]
        BinaryThreshold,
        [Description("限定区域到边界")]
        Boundary,

        [Description("闭运算：填充小孔、平滑边界")]
        ClosingCircle,
        [Description("闭运算：填充小孔、平滑边界")]
        ClosingRectangle1,
        [Description("单通道图组合成三通道图")]
        Compose3,
        [Description("合并对象")]
        ConcatObj,
        [Description("连通域")]
        Connection,
        [Description("图像拷贝")]
        CopyImage,
        [Description("图像裁剪")]
        CropPart,

        [Description("RGB图分解")]
        Decompose3,
        [Description("区域差集")]
        Difference,
        [Description("图像和区域差集")]
        Difference2,
        [Description("图像膨胀：变胖")]
        DilationCircle,

        [Description("图像增强")]
        Emphasize,
        [Description("图像腐蚀：变瘦")]
        ErosionCircle,

        [Description("填充区域")]
        FillUp,
        [Description("根据形状特征填充区域")]
        FillUpShape,

        [Description("生成圆形区域")]
        GenCircle,
        [Description("生成扇形区域")]
        GenCircleSector,
        [Description("生成十字形")]
        GenCrossContourXld,
        [Description("生成灰度值图像")]
        GenImageProto,
        [Description("生成矩形区域")]
        GenRectangle1,
        [Description("生成带角度矩形区域")]
        GenRectangle2,
        [Description("根据XLD生成Region")]
        GenRegionContourXld,
        [Description("获取XLD的坐标")]
        GetContourXld,
        [Description("获取区域的坐标")]
        GetRegionPoints,
        [Description("计算区域的灰度值特征")]
        GrayFeatures,

        [Description("计算变量长度")]
        HTupleLength,

        [Description("两个区域的交集")]
        Intersection,

        [Description("均值滤波")]
        MeanImage,
        [Description("中值滤波")]
        MedianImage,
        [Description("图像镜像")]
        MirrorImage,

        [Description("开运算：去毛刺、平滑边界")]
        OpeningCircle,
        [Description("开运算：去毛刺、平滑边界")]
        OpeningRectangle1,

        [Description("图像叠加")]
        PaintGray,
        [Description("在图像上绘制Region对象")]
        PaintRegion,
        [Description("在图像上绘制XLD对象")]
        PaintXld,

        [Description("限定图像区域")]
        ReduceDomain,
        [Description("区域生长分割图像")]
        Regiongrowing,
        [Description("彩图转灰度图")]
        Rgb1ToGray,
        [Description("图像旋转")]
        RotateImage,

        [Description("图像灰度均衡到0-255")]
        ScaleImageMax,
        [Description("选择目标")]
        SelectObj,
        [Description("根据特征选择区域")]
        SelectShape,
        [Description("设置图像灰度值")]
        SetGrayval,
        [Description("改变区域形状")]
        ShapeTrans,

        [Description("阈值分割")]
        Threshold,
        [Description("提取亚像素边缘")]
        ThresholdSubPix,
        [Description("图像空间转换")]
        TransFromRgb,
        [Description("生成灰度值变量")]
        TupleGenConst,
    }
}