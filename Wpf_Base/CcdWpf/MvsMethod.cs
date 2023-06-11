using System;
using MvCamCtrl.NET;
using System.Runtime.InteropServices;
using HalconDotNet;
using System.Collections.Generic;
using System.Net;

namespace Wpf_Base.CcdWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 19:35:31
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:35:31    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class MvsMethod
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        /// <summary>
        /// 获取 CCD 列表
        /// </summary>
        /// <returns></returns>
        public static MyCamera.MV_CC_DEVICE_INFO_LIST GetCCDList()
        {
            GC.Collect();
            MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            // 获取设备列表
            _ = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            return m_pDeviceList;
        }

        /// <summary>
        /// 获取 CCD 名称
        /// </summary>
        /// <param name="m_pDeviceList"></param>
        /// <returns></returns>
        public static List<CHikCameraInfo> GetCCDInfoList(this MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList)
        {
            GC.Collect();
            List<CHikCameraInfo> ccdList = new List<CHikCameraInfo>();
            // 设备信息
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                CHikCameraInfo info = new CHikCameraInfo
                {
                    CcdOrder = i
                };
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    info.CameraType = EnumCameraType.Gige;
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    info.ManufacturerName = gigeInfo.chManufacturerName;
                    info.ModelName = gigeInfo.chModelName;
                    info.SerialNumber = gigeInfo.chSerialNumber;
                    info.Version = gigeInfo.chDeviceVersion;
                    info.IP = IPAddress.Parse(gigeInfo.nCurrentIp.ToString()).ToString();
                    if (gigeInfo.chUserDefinedName != "")
                    {
                        info.UserName = "GigE: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        info.UserName = "GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")";
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    info.CameraType = EnumCameraType.Usb;
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    info.ManufacturerName = usbInfo.chManufacturerName;
                    info.ModelName = usbInfo.chModelName;
                    info.SerialNumber = usbInfo.chSerialNumber;
                    info.GUID = usbInfo.chDeviceGUID;
                    info.Version = usbInfo.chDeviceVersion;
                    if (usbInfo.chUserDefinedName != "")
                    {
                        info.UserName = "Usb: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")";
                    }
                    else
                    {
                        info.UserName = "Usb: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")";
                    }
                }
                ccdList.Add(info);
            }
            return ccdList;
        }

        /// <summary>
        /// 创建 CCD
        /// </summary>
        /// <param name="ccd"></param>
        /// <param name="ccdList"></param>
        /// <param name="camIdx"></param>
        /// <returns></returns>
        public static bool Create(this MyCamera ccd, MyCamera.MV_CC_DEVICE_INFO_LIST ccdList, int camIdx)
        {
            MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(ccdList.pDeviceInfo[camIdx], typeof(MyCamera.MV_CC_DEVICE_INFO));
            int nRet = ccd.MV_CC_CreateDevice_NET(ref device);
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 启动 CCD
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static bool Open(this MyCamera ccd)
        {
            int nRet = ccd.MV_CC_OpenDevice_NET();
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 设置网口相机最佳网络包大小
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static bool SetGigeGevSCPSPacketSize(this MyCamera ccd)
        {
            int nPacketSize = ccd.MV_CC_GetOptimalPacketSize_NET();
            if (nPacketSize > 0)
            {
                int nRet = ccd.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                return MyCamera.MV_OK == nRet;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取曝光时间和增益
        /// </summary>
        /// <param name="ccd"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool GetExposureAndGain(this MyCamera ccd, ref CHikCameraInfo info)
        {
            MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
            int nRet = ccd.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            info.Exposure = stParam.fCurValue;
            nRet = ccd.MV_CC_GetFloatValue_NET("Gain", ref stParam);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            info.Gain = stParam.fCurValue;
            return true;
        }

        /// <summary>
        /// 获取触发模式
        /// </summary>
        /// <param name="ccd"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool GetTriggerMode(this MyCamera ccd, ref CHikCameraInfo info)
        {
            MyCamera.MVCC_ENUMVALUE stParam = new MyCamera.MVCC_ENUMVALUE();
            int nRet = ccd.MV_CC_GetEnumValue_NET("TriggerMode", ref stParam);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            info.TriggerMode = (EnumCaptureMode)stParam.nCurValue;
            return true;
        }

        /// <summary>
        /// 设置相机曝光时间和增益
        /// </summary>
        /// <param name="ccd"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool SetExposureAndGain(this MyCamera ccd, CHikCameraInfo info)
        {
            int nRet = ccd.MV_CC_SetEnumValue_NET("ExposureAuto", 0);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            nRet = ccd.MV_CC_SetFloatValue_NET("ExposureTime", (float)info.Exposure);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            nRet = ccd.MV_CC_SetEnumValue_NET("GainAuto", 0);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            nRet = ccd.MV_CC_SetFloatValue_NET("Gain", (float)info.Gain);
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 连续模式
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static bool SetContinous(this MyCamera ccd)
        {
            int nRet = ccd.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            nRet = ccd.MV_CC_SetEnumValue_NET("TriggerMode", 0);
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 软触发模式
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static bool SetTriggerSoftware(this MyCamera ccd)
        {
            int nRet = ccd.MV_CC_SetEnumValue_NET("AcquisitionMode", 2);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            nRet = ccd.MV_CC_SetEnumValue_NET("TriggerMode", 1);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            nRet = ccd.MV_CC_SetEnumValue_NET("TriggerSource", 7);
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 硬触发模式
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static bool SetTriggerLine0(this MyCamera ccd)
        {
            int nRet = ccd.MV_CC_SetEnumValue_NET("AcquisitionMode", 2);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            nRet = ccd.MV_CC_SetEnumValue_NET("TriggerMode", 1);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            nRet = ccd.MV_CC_SetEnumValue_NET("TriggerSource", 0);
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 开始抓图
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static bool Start(this MyCamera ccd)
        {
            int nRet = ccd.MV_CC_StartGrabbing_NET();
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 停止抓图
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static bool Stop(this MyCamera ccd)
        {
            int nRet = ccd.MV_CC_StopGrabbing_NET();
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 关闭 CCD
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static void Close(this MyCamera ccd)
        {
            _ = ccd.MV_CC_CloseDevice_NET();
            _ = ccd.MV_CC_DestroyDevice_NET();
        }

        /// <summary>
        /// 图片格式
        /// </summary>
        /// <param name="enType"></param>
        /// <returns></returns>
        public static bool GetIsMonoPixelFormat(MyCamera.MvGvspPixelType enType)
        {
            switch (enType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;
                default:
                    return false;
            }
        }
        public static bool GetIsColorPixelFormat(MyCamera.MvGvspPixelType enType)
        {
            switch (enType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGBA8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGRA8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 软触发指令
        /// </summary>
        /// <param name="ccd"></param>
        /// <returns></returns>
        public static bool TrigCamBySoft(this MyCamera ccd)
        {
            int nRet = ccd.MV_CC_SetCommandValue_NET("TriggerSoftware");
            return MyCamera.MV_OK == nRet;
        }

        /// <summary>
        /// 抓图：等待超时时间默认 1000
        /// 等待超时时间设置过小可能导致问题：首次抓不到图 后面抓的是前一时刻的图
        /// 可持续刷新抓图 间隔 5ms
        /// </summary>
        /// <param name="ho_Image"></param>
        public static void GetHalconImage(MyCamera Camera, ref HObject ho_Image, int nMsec = 1000, bool isTryAgain = false, int sec = 5)
        {
            MyCamera.MV_FRAME_OUT stFrameOut = new MyCamera.MV_FRAME_OUT();
            IntPtr pImageBuf = IntPtr.Zero;
            int nImageBufSize = 0;
            int nRet = Camera.MV_CC_GetImageBuffer_NET(ref stFrameOut, nMsec);
            // 尝试多次
            if (isTryAgain)
            {
                int iteMax = 100;
                // 多次尝试
                for (int i = 0; i < iteMax; i++)
                {
                    if (MyCamera.MV_OK == nRet)
                    {
                        break;
                    }
                    nRet = Camera.MV_CC_GetImageBuffer_NET(ref stFrameOut, sec);
                }
            }

            if (MyCamera.MV_OK == nRet)
            {
                IntPtr pTemp;
                if (GetIsColorPixelFormat(stFrameOut.stFrameInfo.enPixelType))
                {
                    if (stFrameOut.stFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed)
                    {
                        pTemp = stFrameOut.pBufAddr;
                    }
                    else
                    {
                        if (IntPtr.Zero == pImageBuf || nImageBufSize < (stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight * 3))
                        {
                            if (pImageBuf != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(pImageBuf);
                            }

                            pImageBuf = Marshal.AllocHGlobal(stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight * 3);
                            if (IntPtr.Zero == pImageBuf)
                            {
                                return;
                            }
                            nImageBufSize = stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight * 3;
                        }

                        MyCamera.MV_PIXEL_CONVERT_PARAM stPixelConvertParam = new MyCamera.MV_PIXEL_CONVERT_PARAM
                        {
                            pSrcData = stFrameOut.pBufAddr,
                            nWidth = stFrameOut.stFrameInfo.nWidth,
                            nHeight = stFrameOut.stFrameInfo.nHeight,
                            enSrcPixelType = stFrameOut.stFrameInfo.enPixelType,
                            nSrcDataLen = stFrameOut.stFrameInfo.nFrameLen,
                            nDstBufferSize = (uint)nImageBufSize,
                            pDstBuffer = pImageBuf,
                            enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed
                        };
                        nRet = Camera.MV_CC_ConvertPixelType_NET(ref stPixelConvertParam);
                        if (MyCamera.MV_OK != nRet)
                        {
                            return;
                        }
                        pTemp = pImageBuf;
                    }
                    // 转成 Halcon 图像格式：彩图
                    try
                    {
                        ho_Image.Dispose();
                        HOperatorSet.GenImageInterleaved(out ho_Image, pTemp, "rgb", stFrameOut.stFrameInfo.nWidth, stFrameOut.stFrameInfo.nHeight, -1, "byte", 0, 0, 0, 0, -1, 0);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else if (GetIsMonoPixelFormat(stFrameOut.stFrameInfo.enPixelType))
                {
                    if (stFrameOut.stFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                    {
                        pTemp = stFrameOut.pBufAddr;
                    }
                    else
                    {
                        if (IntPtr.Zero == pImageBuf || nImageBufSize < (stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight))
                        {
                            if (pImageBuf != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(pImageBuf);
                            }

                            pImageBuf = Marshal.AllocHGlobal(stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight);
                            if (IntPtr.Zero == pImageBuf)
                            {
                                return;
                            }
                            nImageBufSize = stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight;
                        }

                        MyCamera.MV_PIXEL_CONVERT_PARAM stPixelConvertParam = new MyCamera.MV_PIXEL_CONVERT_PARAM
                        {
                            pSrcData = stFrameOut.pBufAddr,
                            nWidth = stFrameOut.stFrameInfo.nWidth,
                            nHeight = stFrameOut.stFrameInfo.nHeight,
                            enSrcPixelType = stFrameOut.stFrameInfo.enPixelType,
                            nSrcDataLen = stFrameOut.stFrameInfo.nFrameLen,
                            nDstBufferSize = (uint)nImageBufSize,
                            pDstBuffer = pImageBuf,
                            enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8
                        };
                        nRet = Camera.MV_CC_ConvertPixelType_NET(ref stPixelConvertParam);
                        if (MyCamera.MV_OK != nRet)
                        {
                            return;
                        }
                        pTemp = pImageBuf;
                    }

                    // 转成 Halcon 图像格式：灰度图
                    try
                    {
                        ho_Image.Dispose();
                        // System.AccessViolationException:“尝试读取或写入受保护的内存。这通常指示其他内存已损坏。”
                        //HOperatorSet.GenImage1Extern(out ho_Image, "byte", stFrameOut.stFrameInfo.nWidth, stFrameOut.stFrameInfo.nHeight, pTemp, IntPtr.Zero);
                        HOperatorSet.GenImage1(out ho_Image, "byte", stFrameOut.stFrameInfo.nWidth, stFrameOut.stFrameInfo.nHeight, pTemp);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else
                {
                    _ = Camera.MV_CC_FreeImageBuffer_NET(ref stFrameOut);
                }

                // 释放内存
                _ = Camera.MV_CC_FreeImageBuffer_NET(ref stFrameOut);
            }
            if (pImageBuf != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pImageBuf);
            }
        }

        /// <summary>
        /// 注册回调函数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool SetCameraCallbackFunc(this MyCamera ccd, MyCamera.cbEventdelegateEx EventCallback)
        {
            // 事件触发时间
            int nRet = ccd.MV_CC_SetEnumValueByString_NET("EventSelector", "ExposureEnd");
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            // 开启事件
            nRet = ccd.MV_CC_SetEnumValueByString_NET("EventNotification", "On");
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            // 注册回调函数
            nRet = ccd.MV_CC_RegisterEventCallBackEx_NET("ExposureEnd", EventCallback, IntPtr.Zero);
            return MyCamera.MV_OK == nRet;
        }
    }
}