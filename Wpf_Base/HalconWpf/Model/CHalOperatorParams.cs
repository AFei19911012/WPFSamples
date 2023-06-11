using HalconDotNet;
using System;

namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/05 20:41:50
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/05 20:41:50    CoderMan/CoderdMan1012         首次编写         
    ///
    [Serializable]
    public class CHalOperatorParams
    {
        /// <summary>
        /// 算子类型
        /// </summary>
        public EnumHalOperator HalOperator { get; set; }

        /// <summary>
        /// 添加备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 算子名称 解决 EnumHalOperator 中算子顺序问题
        /// </summary>
        public string HalOperatorName { get; set; }


        #region 1. HObject 变量
        public HObject Ho_Image = null;
        public HObject Ho_ImageR = null;
        public HObject Ho_ImageG = null;
        public HObject Ho_ImageB = null;
        public HObject Ho_Region = null;
        #endregion


        #region 2. HTuple 变量
        public HTuple Hv_Rows = new HTuple();
        public HTuple Hv_Cols = new HTuple();
        public HTuple Hv_Values = new HTuple();
        #endregion


        #region 3. 数值变量
        public int IntHTupleLength { get; set; } = 1;
        #endregion


        #region 4. 参数变量
        //********************************************************************************************
        public double GenCircleSector_Row { get; set; } = 200;
        public double GenCircleSector_Col { get; set; } = 200;
        public double GenCircleSector_Radius { get; set; } = 100.5;
        public double GenCircleSector_StartAngle { get; set; } = 0;
        public double GenCircleSector_EndAngle { get; set; } = 3.14159;
        //********************************************************************************************
        public int ScaleImageMax_IndexImage { get; set; } = 1;
        //********************************************************************************************
        public int GrayFeatures_IndexRegion { get; set; } = 1;
        public int GrayFeatures_IndexImage { get; set; } = 1;
        public string GrayFeatures_Features { get; set; } = "mean";
        //********************************************************************************************
        public int Regiongrowing_IndexImage { get; set; } = 1;
        public int Regiongrowing_Row { get; set; } = 3;
        public int Regiongrowing_Col { get; set; } = 3;
        public double Regiongrowing_Tolerance { get; set; } = 6;
        public int Regiongrowing_MinSize { get; set; } = 100;
        //********************************************************************************************
        public int Intersection_IndexRegion1 { get; set; } = 1;
        public int Intersection_IndexRegion2 { get; set; } = 1;
        //********************************************************************************************
        public int SelectShape_Index { get; set; } = 1;
        public string SelectShape_Feature { get; set; } = "area";
        public string SelectShape_Operation { get; set; } = "and";
        public double SelectShape_Min { get; set; } = 150;
        public double SelectShape_Max { get; set; } = 99999;
        //********************************************************************************************
        public int GetContourXld_Index { get; set; } = 1;
        //********************************************************************************************
        public int FillUpShape_Index { get; set; } = 1;
        public string FillUpShape_Feature { get; set; } = "area";
        public double FillUpShape_Min { get; set; } = 1;
        public double FillUpShape_Max { get; set; } = 100;
        //********************************************************************************************
        public int Boundary_IndexRegion { get; set; } = 1;
        public string Boundary_Type { get; set; } = "inner";
        //********************************************************************************************
        public double GenCircle_Row { get; set; } = 200;
        public double GenCircle_Column { get; set; } = 200;
        public double GenCircle_Radius { get; set; } = 150;
        //********************************************************************************************
        public int ThresholdSubPix_IndexImage { get; set; } = 1;
        public int ThresholdSubPix_Threshold { get; set; } = 128;
        //********************************************************************************************
        public int SelectObj_IndexRegion { get; set; } = 1;
        public int SelectObj_Index { get; set; } = 1;
        //********************************************************************************************
        public int ConcatObj_IndexRegion1 { get; set; } = 1;
        public int ConcatObj_IndexRegion2 { get; set; } = 1;
        //********************************************************************************************
        public int GenRegionContourXld_IndexRegion { get; set; } = 1;
        public string GenRegionContourXld_Mode { get; set; } = "filled";
        //********************************************************************************************
        public int PaintRegion_IndexRegion { get; set; } = 1;
        public int PaintRegion_IndexImage { get; set; } = 1;
        public string PaintRegion_Color { get; set; } = "255";
        public string PaintRegion_Type { get; set; } = "fill";
        //********************************************************************************************
        public int PaintGray_IndexImage1 { get; set; } = 1;
        public int PaintGray_IndexImage2 { get; set; } = 1;
        //********************************************************************************************
        public int PaintXld_IndexXld { get; set; } = 1;
        public int PaintXld_IndexImage { get; set; } = 1;
        public string PaintXld_Color { get; set; } = "255";
        //********************************************************************************************
        public int Compose3_IndexImage1 { get; set; } = 1;
        public int Compose3_IndexImage2 { get; set; } = 1;
        public int Compose3_IndexImage3 { get; set; } = 1;
        //********************************************************************************************
        public int GenCrossContourXld_Index { get; set; } = 1;
        public int GenCrossContourXld_Size { get; set; } = 6;
        public int GenCrossContourXld_Angle { get; set; } = 45;
        //********************************************************************************************
        public int AreaCenter_IndexRegion { get; set; } = 1;
        //********************************************************************************************
        public int ShapeTrans_IndexRegion { get; set; } = 1;
        public string ShapeTrans_Type { get; set; } = "convex";
        //********************************************************************************************
        public int Connection_IndexRegion { get; set; } = 1;
        //********************************************************************************************
        public int DilationCircle_IndexRegion { get; set; } = 1;
        public double DilationCircle_Radius { get; set; } = 3.5;
        //********************************************************************************************
        public int Difference_IndexRegion1 { get; set; } = 1;
        public int Difference_IndexRegion2 { get; set; } = 1;
        //********************************************************************************************
        public int Difference2_IndexImage { get; set; } = 1;
        public int Difference2_IndexRegion { get; set; } = 1;
        //********************************************************************************************
        public int GenRectangle2_Row { get; set; } = 300;
        public int GenRectangle2_Col { get; set; } = 200;
        public double GenRectangle2_Phi { get; set; } = 0;
        public int GenRectangle2_Length1 { get; set; } = 100;
        public int GenRectangle2_Length2 { get; set; } = 20;
        //********************************************************************************************
        public int GenRectangle1_Row1 { get; set; } = 30;
        public int GenRectangle1_Col1 { get; set; } = 20;
        public int GenRectangle1_Row2 { get; set; } = 100;
        public int GenRectangle1_Col2 { get; set; } = 200;
        //********************************************************************************************
        public int ErosionCircle_IndexRegion { get; set; } = 1;
        public double ErosionCircle_Radius { get; set; } = 3.5;
        //********************************************************************************************
        public int ClosingRectangle1_IndexRegion { get; set; } = 1;
        public int ClosingRectangle1_Width { get; set; } = 10;
        public int ClosingRectangle1_Height { get; set; } = 10;
        //********************************************************************************************
        public int ClosingCircle_IndexRegion { get; set; } = 1;
        public double ClosingCircle_Radius { get; set; } = 3.5;
        //********************************************************************************************
        public int OpeningRectangle1_IndexRegion { get; set; } = 1;
        public int OpeningRectangle1_Width { get; set; } = 10;
        public int OpeningRectangle1_Height { get; set; } = 10;
        //********************************************************************************************
        public int BinaryThreshold_IndexImage { get; set; } = 1;
        public string BinaryThreshold_Method { get; set; } = "max_separability";
        public string BinaryThreshold_LightDark { get; set; } = "dark";
        //********************************************************************************************
        public int CopyImage_IndexImage { get; set; } = 1;
        //********************************************************************************************
        public int CropPart_IndexImage { get; set; } = 1;
        public int CropPart_Row { get; set; } = 0;
        public int CropPart_Col { get; set; } = 0;
        public int CropPart_Width { get; set; } = 100;
        public int CropPart_Height { get; set; } = 50;
        //********************************************************************************************
        public int Decompose3_IndexImage { get; set; } = 1;
        public int Decompose3_Index { get; set; } = 0;
        //********************************************************************************************
        public int Emphasize_IndexImage { get; set; } = 1;
        public int Emphasize_MaskWidth { get; set; } = 7;
        public int Emphasize_MaskHeight { get; set; } = 7;
        public double Emphasize_Factor { get; set; } = 1;
        //********************************************************************************************
        public int FillUp_IndexRegion { get; set; } = 1;
        //********************************************************************************************
        public int GenImageProto_IndexImage { get; set; } = 1;
        public int GenImageProto_Gray { get; set; } = 0;
        //********************************************************************************************
        public int GetRegionPoints_IndexRegion { get; set; } = 1;
        //********************************************************************************************
        public int MeanImage_IndexImage { get; set; } = 1;
        public int MeanImage_MaskWidth { get; set; } = 5;
        public int MeanImage_MaskHeight { get; set; } = 5;
        //********************************************************************************************
        public int MedianImage_IndexImage { get; set; } = 1;
        public string MedianImage_MaskType { get; set; } = "circle";
        public int MedianImage_Radius { get; set; } = 3;
        public string MedianImage_Margin { get; set; } = "mirrored";
        //********************************************************************************************
        public int MirrorImage_IndexImage { get; set; } = 1;
        public string MirrorImage_Mode { get; set; } = "row";
        //********************************************************************************************
        public string ReadImage_Filepath { get; set; } = "";
        //********************************************************************************************
        public int ReduceDomain_IndexImage { get; set; } = 1;
        public int ReduceDomain_IndexRegion { get; set; } = 1;
        //********************************************************************************************
        public int Rgb1ToGray_IndexImage { get; set; } = 1;
        //********************************************************************************************
        public int RotateImage_IndexImage { get; set; } = 1;
        public double RotateImage_Phi { get; set; } = 90;
        public string RotateImage_Interpolation { get; set; } = "constant";
        //********************************************************************************************
        public int Threshold_IndexImage { get; set; } = 1;
        public int Threshold_MinGray { get; set; } = 0;
        public int Threshold_MaxGray { get; set; } = 100;
        //********************************************************************************************
        public int TransFromRgb_IndexImage { get; set; } = 1;
        public string TransFromRgb_ColorSpace { get; set; } = "hsv";
        public int TransFromRgb_Index { get; set; } = 1;
        //********************************************************************************************
        public int OpeningCircle_IndexRegion { get; set; } = 1;
        public double OpeningCircle_Radius { get; set; } = 3.5;
        //********************************************************************************************
        public int HTupleLength_IndexTuple { get; set; } = 1;
        //********************************************************************************************
        public int TupleGenConst_IndexTuple { get; set; } = 1;
        public int TupleGenConst_Gray { get; set; } = 0;
        //********************************************************************************************
        public int SetGrayval_IndexImage { get; set; } = 1;
        public int SetGrayval_IndexRow { get; set; } = 1;
        public int SetGrayval_IndexCol { get; set; } = 1;
        public int SetGrayval_IndexValue { get; set; } = 1;
        //********************************************************************************************
        public int WriteImage_IndexImage { get; set; } = 1;
        public string WriteImage_Filepath { get; set; } = "";
        //********************************************************************************************
        public int DispImg_IndexImage { get; set; } = 1;
        //********************************************************************************************
        public int DispObj_IndexObject { get; set; } = 1;
        //********************************************************************************************
        public string SetColor_Color { get; set; } = "orange red";
        //********************************************************************************************
        public int SetLineWidth_Width { get; set; } = 1;
        //********************************************************************************************
        public string SetDraw_Draw { get; set; } = "margin";
        #endregion


        public CHalOperatorParams(EnumHalOperator halOperator)
        {
            HalOperator = halOperator;

            // 初始化变量
            HOperatorSet.GenEmptyObj(out Ho_Image);
            HOperatorSet.GenEmptyObj(out Ho_ImageR);
            HOperatorSet.GenEmptyObj(out Ho_ImageG);
            HOperatorSet.GenEmptyObj(out Ho_ImageB);
            HOperatorSet.GenEmptyObj(out Ho_Region);
            Ho_Image.Dispose();
            Ho_ImageR.Dispose();
            Ho_ImageG.Dispose();
            Ho_ImageB.Dispose();
            Ho_Region.Dispose();

            Hv_Rows = new HTuple();
            Hv_Cols = new HTuple();
            Hv_Values = new HTuple();
        }
    }
}