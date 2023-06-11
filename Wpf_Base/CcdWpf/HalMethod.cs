using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf_Base.CcdWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 19:36:45
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:36:45    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class HalMethod
    {
        #region Halcon 显示相关
        /// <summary>
        /// 设置显示字体
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_Size"></param>
        /// <param name="hv_Font"></param>
        /// <param name="hv_Bold"></param>
        /// <param name="hv_Slant"></param>
        public static void SetDisplayFont(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font, HTuple hv_Bold, HTuple hv_Slant)
        {
            HTuple hv_OS = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_Style = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_AvailableFonts = new HTuple(), hv_Fdx = new HTuple();
            HTuple hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = new HTuple(hv_Font);
            HTuple hv_Size_COPY_INP_TMP = new HTuple(hv_Size);
            try
            {
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
        /// 设置字体和大小
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="fontsize"></param>
        /// <param name="fontname"></param>
        public static void SetDisplayFont(this HWindow ho_Window, double fontsize = 18, string fontname = "mono")
        {
            SetDisplayFont(ho_Window, fontsize, fontname, "false", "false");
        }

        /// <summary>
        /// 拟合圆
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static void FitCircle(HTuple hv_rows, HTuple hv_cols, out double oX, out double oY, out double radius)
        {
            HOperatorSet.GenEmptyObj(out HObject ho_Contour);
            ho_Contour.Dispose();
            HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_rows, hv_cols);
            hv_rows.Dispose();
            hv_cols.Dispose();
            HOperatorSet.FitCircleContourXld(ho_Contour, "geotukey", -1, 0, 0, 3, 2, out hv_rows, out hv_cols, out HTuple hv_radius, out HTuple hv_startPhi, out HTuple hv_endPhi, out HTuple hv_PointOrder);
            oX = hv_cols.D;
            oY = hv_rows.D;
            radius = hv_radius.D;
            hv_radius.Dispose();
            hv_startPhi.Dispose();
            hv_endPhi.Dispose();
            hv_PointOrder.Dispose();
            hv_rows.Dispose();
            hv_cols.Dispose();
        }

        /// <summary>
        /// 拟合直线 两边延长
        /// </summary>
        /// <param name="contour"></param>
        /// <param name="halfLen"></param>
        /// <param name="row0"></param>
        /// <param name="col0"></param>
        /// <param name="phi"></param>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        public static void FitLine(HObject contour, double halfLen, out double row0, out double col0, out double phi, out double row1, out double col1, out double row2, out double col2,
            string algorithm = "tukey", int maxNumPoints = -1, int clippintEndPoints = 0, int iterations = 5, double clippingFactor = 2)
        {
            HOperatorSet.FitLineContourXld(contour, algorithm, maxNumPoints, clippintEndPoints, iterations, clippingFactor, out HTuple hv_Row11, out HTuple hv_Col11, out HTuple hv_Row12, out HTuple hv_Col12, out HTuple hv_Nr, out HTuple hv_Nc, out HTuple hv_Dist);
            HOperatorSet.LinePosition(hv_Row11, hv_Col11, hv_Row12, hv_Col12, out HTuple hv_RowCenter, out HTuple hv_ColCenter, out HTuple hv_Len, out HTuple hv_Phi);
            phi = Math.Abs(hv_Phi.D);
            row0 = hv_RowCenter.D;
            col0 = hv_ColCenter.D;
            hv_Nr.Dispose();
            hv_Nc.Dispose();
            hv_Dist.Dispose();
            hv_Len.Dispose();
            hv_Phi.Dispose();
            // 先确定角度方向
            double x1 = hv_Col11.D;
            double y1 = hv_Row11.D;
            double x2 = hv_Col12.D;
            double y2 = hv_Row12.D;
            if (x1 > x2)
            {
                y1 = hv_Row12.D;
                y2 = hv_Row11.D;
            }
            // 坐标延长
            if (y1 < y2)
            {
                row1 = hv_RowCenter.D - (halfLen * Math.Sin(phi));
                col1 = hv_ColCenter.D - (halfLen * Math.Cos(phi));
                row2 = hv_RowCenter.D + (halfLen * Math.Sin(phi));
                col2 = hv_ColCenter.D + (halfLen * Math.Cos(phi));
            }
            else
            {
                row1 = hv_RowCenter.D + (halfLen * Math.Sin(phi));
                col1 = hv_ColCenter.D - (halfLen * Math.Cos(phi));
                row2 = hv_RowCenter.D - (halfLen * Math.Sin(phi));
                col2 = hv_ColCenter.D + (halfLen * Math.Cos(phi));
            }
        }

        /// <summary>
        /// 绘制 Marker 点
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="angle"></param>
        public static void DispCrossContour(this HWindow ho_Window, double row, double col, double len = 100, double angle = 0.785398)
        {
            ho_Window.DispCross(row, col, len, angle);
        }
        public static void DispCrossContour(this HWindow ho_Window, HTuple row, HTuple col, HTuple r, double angle = 0.785398)
        {
            ho_Window.DispCross(row, col, r, angle);
        }

        /// <summary>
        /// 绘制矩形框
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <param name="angle"></param>
        public static void DispRectangleContour(this HWindow ho_Window, double row1, double col1, double row2, double col2)
        {
            HOperatorSet.GenRectangle2ContourXld(out HObject ho_Rect, 0.5 * (row1 + row2), 0.5 * (col1 + col2), 0, 0.5 * Math.Abs(col2 - col1), 0.5 * Math.Abs(row2 - row1));
            ho_Window.DispObj(ho_Rect);
            ho_Rect.Dispose();
        }
        public static void DispRectangleContour(this HWindow ho_Window, double row, double col, double len1, double len2, double angle = 0)
        {
            HOperatorSet.GenRectangle2ContourXld(out HObject ho_Rect, row, col, angle, len1, len2);
            ho_Window.DispObj(ho_Rect);
            ho_Rect.Dispose();
        }

        /// <summary>
        /// 绘制椭圆
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="len1"></param>
        /// <param name="len2"></param>
        /// <param name="angle"></param>
        public static void DispEllipse2Contour(this HWindow ho_Window, double row, double col, double len1, double len2, double angle = 0)
        {
            HOperatorSet.GenEllipseContourXld(out HObject ho_Ellipse, row, col, angle, len1, len2, 0, 6.28318, "positive", 1.5);
            ho_Window.DispObj(ho_Ellipse);
            ho_Ellipse.Dispose();
        }

        /// <summary>
        /// 绘制圆
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="r"></param>
        public static void DispCircleContour(this HWindow ho_Window, double row, double col, double r = 40)
        {
            HOperatorSet.GenCircleContourXld(out HObject ho_Circle, row, col, r, 0, 6.28318, "positive", 1);
            ho_Window.DispObj(ho_Circle);
            ho_Circle.Dispose();
        }
        public static void DispCircleContour(this HWindow ho_Window, HTuple row, HTuple col, HTuple r)
        {
            HOperatorSet.GenCircleContourXld(out HObject ho_Circles, row, col, r, 0, 6.28318, "positive", 1);
            ho_Window.DispObj(ho_Circles);
            ho_Circles.Dispose();
        }

        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        public static void DispArrowContour(this HWindow ho_Window, double row1, double col1, double row2, double col2)
        {
            double x1 = col1;
            double y1 = row1;
            double x2 = col2;
            double y2 = row2;
            double arrowLength = Math.Max(0.1 * Math.Sqrt(((row1 - row2) * (row1 - row2)) + ((col1 - col2) * (col1 - col2))), 20);
            double arrowAngle = Math.PI / 12;
            // 起始点线段夹角
            double angleOri = Math.Atan((y2 - y1) / (x2 - x1));
            // 箭头扩张角度
            double angleDown = angleOri - arrowAngle;
            double angleUp = angleOri + arrowAngle;
            // 方向标识
            int directionFlag = (x2 > x1) ? -1 : 1;
            // 箭头两侧点坐标
            double x3 = x2 + (directionFlag * arrowLength * Math.Cos(angleDown));
            double y3 = y2 + (directionFlag * arrowLength * Math.Sin(angleDown));
            double x4 = x2 + (directionFlag * arrowLength * Math.Cos(angleUp));
            double y4 = y2 + (directionFlag * arrowLength * Math.Sin(angleUp));
            // 画线
            ho_Window.DispLine(row1, col1, row2, col2);
            ho_Window.DispLine(row2, col2, y3, x3);
            ho_Window.DispLine(row2, col2, y4, x4);
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="ho_Windwo"></param>
        /// <param name="info"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="color"></param>
        /// <param name="box_color"></param>
        public static void DispText(this HWindow ho_Windwo, string info, double row, double col, string color = "red", string box_color = "black")
        {
            // 背景
            HTuple atts = new HTuple();
            HTuple values = new HTuple();
            atts[0] = "box";
            values[0] = "true";
            atts[1] = "box_color";
            values[1] = box_color;
            atts[2] = "shadow";
            values[2] = "false";
            HOperatorSet.DispText(ho_Windwo, info, "window", row, col, color, atts, values);
        }

        /// <summary>
        /// 获取 Halcon 颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToColorString(this EnumHalColor color)
        {
            return color.ToString().Replace("_", " ");
        }

        #endregion

        #region Halcon 相关的一些方法

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

        /// <summary>
        /// 计算标定 std
        /// </summary>
        /// <param name="pts1"></param>
        /// <param name="pts2"></param>
        /// <returns></returns>
        public static double GetCalibrationStd(HTuple hv_Row1, HTuple hv_Col1, HTuple hv_Row2, HTuple hv_Col2)
        {
            HTuple hv_Distance = new HTuple();
            hv_Distance.Dispose();
            HOperatorSet.DistancePp(hv_Row1, hv_Col1, hv_Row2, hv_Col2, out hv_Distance);
            return hv_Distance.TupleDeviation();
            //double result = 0;
            //for (int i = 0; i < hv_Distance.TupleLength(); i++)
            //{
            //    result += hv_Distance[i] * hv_Distance[i];
            //}
            //hv_Distance.Dispose();

            //return Math.Sqrt(result / (hv_Distance.TupleLength() - 1));
        }

        /// <summary>
        /// Halcon 窗体存图
        /// </summary>
        /// <param name="ho_Window"></param>
        /// <param name="filename"></param>
        /// <param name="ext"></param>
        public static void WriteImage(this HWindow ho_Window, string filename, string ext = "png")
        {
            HImage image = ho_Window.DumpWindowImage();
            image.WriteImage(ext, 0, filename);
            image.Dispose();
        }

        /// <summary>
        /// Ho_Image 存图
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="filename"></param>
        /// <param name="ext"></param>
        public static void WriteImage(this HObject ho_Image, string filename, string ext = "png")
        {
            HOperatorSet.WriteImage(ho_Image, ext, 0, filename);
        }

        #endregion

        #region Halcon 图像采集相关

        /// <summary>
        /// 获取相机列表   将 MVS 目录下的 hAcqMVision.dll（对应 Halcon 版本） 拷贝到 Halcon 根目录
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<string> GetInfoFramegrabber()
        {
            HOperatorSet.InfoFramegrabber("MVision", "device", out _, out HTuple hv_ValueList);
            ObservableCollection<string> infos = new ObservableCollection<string>();
            for (int i = 0; i < hv_ValueList.Length; i++)
            {
                infos.Add(hv_ValueList[i].S);
            }
            return infos;
        }
        public static List<CHalCameraInfo> GetCCDList()
        {
            HOperatorSet.InfoFramegrabber("MVision", "device", out _, out HTuple hv_ValueList);
            List<CHalCameraInfo> infos = new List<CHalCameraInfo>();
            for (int i = 0; i < hv_ValueList.Length; i++)
            {
                CHalCameraInfo info = new CHalCameraInfo
                {
                    CcdName = hv_ValueList[i].S,
                    Hv_AcqHandle = new HTuple(),
                };
                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 启动相机
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        /// <param name="ccd"></param>
        public static void CmdOpenFramegrabber(ref HTuple hv_AcqHandle, string ccd = "default")
        {
            HOperatorSet.OpenFramegrabber("MVision", 1, 1, 0, 0, 0, 0, "progressive", 8, "default", -1, "false", "auto", ccd, 0, -1, out hv_AcqHandle);
        }

        /// <summary>
        /// 获取相机 ID
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        /// <returns></returns>
        public static string GetDeviceID(this HTuple hv_AcqHandle)
        {
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "DeviceID", out HTuple hv_Value);
            return hv_Value.S;
        }

        /// <summary>
        /// 设置连续模式
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        public static void SetCcdContinous(this HTuple hv_AcqHandle)
        {
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "AcquisitionMode", "Continuous");
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        /// <param name="exposure_time"></param>
        public static void SetCcdExposureTime(this HTuple hv_AcqHandle, double exposure_time = 5000)
        {
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureTime", exposure_time);
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        /// <param name="gain"></param>
        public static void SetCcdGain(this HTuple hv_AcqHandle, double gain = 0)
        {
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "Gain", gain);
        }

        /// <summary>
        /// 启动抓图
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        public static void GrabImageStart(this HTuple hv_AcqHandle)
        {
            HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
        }

        /// <summary>
        /// 获取图像
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        /// <param name="ho_Image"></param>
        public static void GrabImageAsync(this HTuple hv_AcqHandle, ref HObject ho_Image)
        {
            ho_Image.Dispose();
            HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        public static void CloseFramegrabber(this HTuple hv_AcqHandle)
        {
            HOperatorSet.CloseFramegrabber(hv_AcqHandle);
            hv_AcqHandle.Dispose();
        }

        /// <summary>
        /// 获取曝光时间
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        /// <returns></returns>
        public static double GetCcdExposureTime(this HTuple hv_AcqHandle)
        {
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "ExposureTime", out HTuple hv_ExposureTime);
            return hv_ExposureTime.D;
        }

        /// <summary>
        /// 获取增益
        /// </summary>
        /// <param name="hv_AcqHandle"></param>
        /// <returns></returns>
        public static double GetCcdGain(this HTuple hv_AcqHandle)
        {
            HOperatorSet.GetFramegrabberParam(hv_AcqHandle, "Gain", out HTuple hv_Gain);
            return hv_Gain.D;
        }

        #endregion
    }
}