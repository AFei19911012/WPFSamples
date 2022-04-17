using HalconDotNet;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace HalconWPF.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/9/29 15:47:10
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/9/29 15:47:10    Taosy.W                 
    ///
    public static class HalMethod
    {
        // Chapter: Graphics / Text
        // Short Description: Set font independent of OS 
        public static void Set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font, HTuple hv_Bold, HTuple hv_Slant)
        {
            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_Style = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_AvailableFonts = new HTuple(), hv_Fdx = new HTuple();
            HTuple hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = new HTuple(hv_Font);
            HTuple hv_Size_COPY_INP_TMP = new HTuple(hv_Size);

            // Initialize local and output iconic variables 
            try
            {
                //This procedure sets the text font of the current window with
                //the specified attributes.
                //
                //Input parameters:
                //WindowHandle: The graphics window for which the font will be set
                //Size: The font size. If Size=-1, the default of 16 is used.
                //Bold: If set to 'true', a bold font is used
                //Slant: If set to 'true', a slanted font is used
                //
                hv_OS.Dispose();
                HOperatorSet.GetSystem("operating_system", out hv_OS);
                if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                    new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                {
                    hv_Size_COPY_INP_TMP.Dispose();
                    hv_Size_COPY_INP_TMP = 16;
                }
                if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
                {
                    //Restore previous behaviour
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Size = ((1.13677 * hv_Size_COPY_INP_TMP)).TupleInt()
                                ;
                            hv_Size_COPY_INP_TMP.Dispose();
                            hv_Size_COPY_INP_TMP = ExpTmpLocalVar_Size;
                        }
                    }
                }
                else
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Size = hv_Size_COPY_INP_TMP.TupleInt()
                                ;
                            hv_Size_COPY_INP_TMP.Dispose();
                            hv_Size_COPY_INP_TMP = ExpTmpLocalVar_Size;
                        }
                    }
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Courier";
                    hv_Fonts[1] = "Courier 10 Pitch";
                    hv_Fonts[2] = "Courier New";
                    hv_Fonts[3] = "CourierNew";
                    hv_Fonts[4] = "Liberation Mono";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Consolas";
                    hv_Fonts[1] = "Menlo";
                    hv_Fonts[2] = "Courier";
                    hv_Fonts[3] = "Courier 10 Pitch";
                    hv_Fonts[4] = "FreeMono";
                    hv_Fonts[5] = "Liberation Mono";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Luxi Sans";
                    hv_Fonts[1] = "DejaVu Sans";
                    hv_Fonts[2] = "FreeSans";
                    hv_Fonts[3] = "Arial";
                    hv_Fonts[4] = "Liberation Sans";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Times New Roman";
                    hv_Fonts[1] = "Luxi Serif";
                    hv_Fonts[2] = "DejaVu Serif";
                    hv_Fonts[3] = "FreeSerif";
                    hv_Fonts[4] = "Utopia";
                    hv_Fonts[5] = "Liberation Serif";
                }
                else
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple(hv_Font_COPY_INP_TMP);
                }
                hv_Style.Dispose();
                hv_Style = "";
                if ((int)(new HTuple(hv_Bold.TupleEqual("true"))) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Style = hv_Style + "Bold";
                            hv_Style.Dispose();
                            hv_Style = ExpTmpLocalVar_Style;
                        }
                    }
                }
                else if ((int)(new HTuple(hv_Bold.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception.Dispose();
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant.TupleEqual("true"))) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Style = hv_Style + "Italic";
                            hv_Style.Dispose();
                            hv_Style = ExpTmpLocalVar_Style;
                        }
                    }
                }
                else if ((int)(new HTuple(hv_Slant.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception.Dispose();
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Style.TupleEqual(""))) != 0)
                {
                    hv_Style.Dispose();
                    hv_Style = "Normal";
                }
                hv_AvailableFonts.Dispose();
                HOperatorSet.QueryFont(hv_WindowHandle, out hv_AvailableFonts);
                hv_Font_COPY_INP_TMP.Dispose();
                hv_Font_COPY_INP_TMP = "";
                for (hv_Fdx = 0; (int)hv_Fdx <= (int)((new HTuple(hv_Fonts.TupleLength())) - 1); hv_Fdx = (int)hv_Fdx + 1)
                {
                    hv_Indices.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Indices = hv_AvailableFonts.TupleFind(
                            hv_Fonts.TupleSelect(hv_Fdx));
                    }
                    if ((int)(new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        if ((int)(new HTuple(((hv_Indices.TupleSelect(0))).TupleGreaterEqual(0))) != 0)
                        {
                            hv_Font_COPY_INP_TMP.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(
                                    hv_Fdx);
                            }
                            break;
                        }
                    }
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(""))) != 0)
                {
                    throw new HalconException("Wrong value of control parameter Font");
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    {
                        HTuple
                          ExpTmpLocalVar_Font = (((hv_Font_COPY_INP_TMP + "-") + hv_Style) + "-") + hv_Size_COPY_INP_TMP;
                        hv_Font_COPY_INP_TMP.Dispose();
                        hv_Font_COPY_INP_TMP = ExpTmpLocalVar_Font;
                    }
                }
                HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP);

                hv_Font_COPY_INP_TMP.Dispose();
                hv_Size_COPY_INP_TMP.Dispose();
                hv_OS.Dispose();
                hv_Fonts.Dispose();
                hv_Style.Dispose();
                hv_Exception.Dispose();
                hv_AvailableFonts.Dispose();
                hv_Fdx.Dispose();
                hv_Indices.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_Font_COPY_INP_TMP.Dispose();
                hv_Size_COPY_INP_TMP.Dispose();
                hv_OS.Dispose();
                hv_Fonts.Dispose();
                hv_Style.Dispose();
                hv_Exception.Dispose();
                hv_AvailableFonts.Dispose();
                hv_Fdx.Dispose();
                hv_Indices.Dispose();

                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 设置 Halcon 窗口显示字体
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="fontsize"></param>
        /// <param name="fontfamily"></param>
        /// <param name="fontbold"></param>
        /// <param name="fontslant"></param>
        public static void SetDisplayFont(this HWindow ho_Window, int fontsize = 16, string fontfamily = "sans", string fontbold = "true", string fontslant = "false")
        {
            Set_display_font(ho_Window, fontsize, fontfamily, fontbold, fontslant);
        }

        /// <summary>
        /// 显示文本到 Halcon 窗口
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="text"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="fontcolor"></param>
        public static void DispText(this HWindow ho_Window, string text, HTuple row, HTuple col, string co = "image", string fontcolor = "black")
        {
            HOperatorSet.DispText(ho_Window, text, co, row, col, fontcolor, new HTuple(), new HTuple());
        }

        /// <summary>
        /// 单通道 Halcon 图像 → 灰度值
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <returns></returns>
        public static void GetImageGrayValue(HImage ho_Image, out byte[] grayValue)
        {
            HOperatorSet.GetImagePointer1(ho_Image, out HTuple hv_Pointer, out HTuple hv_Type, out HTuple hv_Width, out HTuple hv_Height);
            int len = hv_Width * hv_Height;
            grayValue = new byte[len];
            Marshal.Copy(hv_Pointer, grayValue, 0, len);
            return;
        }

        /// <summary>
        /// 三通道 Halcon 图像 → R、G、B 值
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <returns></returns>
        public static void GetImageMultiValue(HImage ho_Image, out byte[] R, out byte[] G, out byte[] B)
        {
            HOperatorSet.GetImagePointer3(ho_Image, out HTuple hv_PointerRed, out HTuple hv_PointerGreen, out HTuple hv_PointerBlue, out HTuple hv_Type, out HTuple hv_Width, out HTuple hv_Height);
            int len = hv_Width * hv_Height;
            R = new byte[len];
            G = new byte[len];
            B = new byte[len];
            Marshal.Copy(hv_PointerRed, R, 0, len);
            Marshal.Copy(hv_PointerGreen, G, 0, len);
            Marshal.Copy(hv_PointerBlue, B, 0, len);
            return;
        }

        /// <summary>
        /// 获取相机信息
        /// </summary>
        /// <returns></returns>
        public static string GetInfoFramegrabber()
        {
            HTuple hv_Information = new HTuple();
            HTuple hv_ValueList = new HTuple();
            hv_Information.Dispose();
            hv_ValueList.Dispose();
            HOperatorSet.InfoFramegrabber("GigEVision2", "device", out hv_Information, out hv_ValueList);
            //HOperatorSet.InfoFramegrabber("GenICamTL", "device", out hv_Information, out hv_ValueList);
            //HOperatorSet.InfoFramegrabber("MVision", "device", out hv_Information, out hv_ValueList);
            hv_Information.Dispose();
            string camInfo = "";
            if (hv_ValueList.Length > 0)
            {
                // MVision
                camInfo = hv_ValueList;
                // GigEVision2
                if (camInfo.Contains("device"))
                {
                    string[] strs = camInfo.Split('|');
                    camInfo = strs[1].Trim().Split(':')[1];
                }
            }
            return camInfo;
        }

        /// <summary>
        /// 坐标转换：Control ↔ Halcon
        /// </summary>
        /// <param name="Halcon"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isReversed"></param>
        /// <returns></returns>
        public static Point ControlPointToHImagePoint(this HSmartWindowControlWPF Halcon, double x, double y, bool isReversed = false)
        {
            // Halcon 控件宽高
            double cHeight = Halcon.ActualHeight;
            double cWidth = Halcon.ActualWidth;
            // Halcon 图像区域
            double x0 = Halcon.HImagePart.X;
            double y0 = Halcon.HImagePart.Y;
            double imHeight = Halcon.HImagePart.Height;
            double imWidth = Halcon.HImagePart.Width;
            // 缩放系数
            double ratio_y = imHeight / cHeight;
            double ratio_x = imWidth / cWidth;
            double x1;
            double y1;
            if (isReversed)
            {
                x1 = (x - x0) / ratio_x;
                y1 = (y - y0) / ratio_y;
            }
            else
            {
                x1 = (ratio_x * x) + x0;
                y1 = (ratio_y * y) + y0;
            }
            return new Point(x1, y1);
        }
        public static PointCollection ControlPointToHImagePoint(this HSmartWindowControlWPF Halcon, StylusPointCollection sps, bool isReversed = false)
        {
            PointCollection pts = new PointCollection();
            for (int i = 0; i < sps.Count; i++)
            {
                pts.Add(ControlPointToHImagePoint(Halcon, sps[i].X, sps[i].Y, isReversed));
            }
            return pts;
        }
        public static PointCollection ControlPointToHImagePoint(this HSmartWindowControlWPF Halcon, PointCollection points, bool isReversed = false)
        {
            PointCollection pts = new PointCollection();
            for (int i = 0; i < points.Count; i++)
            {
                pts.Add(ControlPointToHImagePoint(Halcon, points[i].X, points[i].Y, isReversed));
            }
            return pts;
        }
    }
}