using HalconDotNet;
using MvCamCtrl.NET;
using System.Collections.Generic;

namespace Wpf_Base.CcdWpf
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/08/29 19:30:01
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/08/29 19:30:01    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CcdManager
    {
        /// <summary>
        /// 所有相机信息
        /// </summary>
        public MyCamera.MV_CC_DEVICE_INFO_LIST CcdList { get; set; }
        public List<CHikCameraInfo> HikCamInfos { get; set; }

        public List<CHalCameraInfo> HalCamInfos { get; set; }

        /// <summary>
        /// 相机数量
        /// </summary>
        public int NumberCCD => HikCamInfos.Count;


        /// <summary>
        /// 当前选中的相机序号
        /// </summary>
        public int CurrentCamId { get; set; } = -1;


        /// <summary>
        /// 自身的一个实例
        /// 在不同工具中可以使用同一个相机来管理
        /// </summary>
        private static CcdManager instance;
        public static CcdManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CcdManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// 注册事件：一个相机对应一个
        /// 软触发、硬触发均可使用
        /// </summary>
        public MyCamera.cbEventdelegateEx[] ExposureEndEvents { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public CcdManager()
        {
            InitCCD();
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        public void InitCCD()
        {
            CcdList = MvsMethod.GetCCDList();
            HikCamInfos = CcdList.GetCCDInfoList();
            ExposureEndEvents = new MyCamera.cbEventdelegateEx[NumberCCD];
            if (NumberCCD > 0)
            {
                CurrentCamId = 0;
            }

            //HalCamInfos = HalMethod.GetCCDList();
        }

        /// <summary>
        /// 创建相机 启动相机
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                if (!HikCamInfos[i].IsOpened)
                {
                    bool result = HikCamInfos[i].Camera.Create(CcdList, i);
                    if (!result)
                    {
                        return false;
                    }
                    result = HikCamInfos[i].Camera.Open();
                    if (!result)
                    {
                        return false;
                    }
                    else
                    {
                        HikCamInfos[i].IsOpened = true;
                        _ = GetExposureAndGain(i);
                        _ = GetTriggerMode(i);
                        // 探测网络最佳包大小
                        if (HikCamInfos[i].CameraType == EnumCameraType.Gige)
                        {
                            _ = HikCamInfos[i].Camera.SetGigeGevSCPSPacketSize();
                        }
                    }
                }
            }
            return true;
        }
        public bool Open(int camId)
        {
            // 判断相机打开状态
            if (!HikCamInfos[camId].IsOpened)
            {
                bool result = HikCamInfos[camId].Camera.Create(CcdList, camId);
                if (!result)
                {
                    return false;
                }
                result = HikCamInfos[camId].Camera.Open();
                if (result)
                {
                    HikCamInfos[camId].IsOpened = true;
                    _ = GetExposureAndGain(camId);
                    result = GetTriggerMode(camId);
                    // 探测网络最佳包大小
                    if (HikCamInfos[camId].CameraType == EnumCameraType.Gige)
                    {
                        result = HikCamInfos[camId].Camera.SetGigeGevSCPSPacketSize();
                    }
                }
                return result;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 开始抓图
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                if (!HikCamInfos[i].IsGrabbing)
                {
                    bool result = HikCamInfos[i].Camera.Start();
                    if (!result)
                    {
                        return false;
                    }
                    else
                    {
                        HikCamInfos[i].IsGrabbing = true;
                    }
                }
            }
            return true;
        }
        public bool Start(int camId)
        {
            // 判断相机是否在抓图
            if (!HikCamInfos[camId].IsGrabbing)
            {
                bool result = HikCamInfos[camId].Camera.Start();
                if (result)
                {
                    HikCamInfos[camId].IsGrabbing = true;
                }
                return result;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 暂停相机
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                bool result = HikCamInfos[i].Camera.Stop();
                if (!result)
                {
                    return false;
                }
                else
                {
                    HikCamInfos[i].IsGrabbing = false;
                }
            }
            return true;
        }
        public bool Stop(int camId)
        {
            bool result = HikCamInfos[camId].Camera.Stop();
            if (result)
            {
                HikCamInfos[camId].IsGrabbing = false;
            }
            return result;
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            _ = Stop();
            for (int i = 0; i < NumberCCD; i++)
            {
                HikCamInfos[i].Camera.Close();
                HikCamInfos[i].IsOpened = false;
                HikCamInfos[i].IsGrabbing = false;
            }
        }
        public void Close(int camId)
        {
            _ = Stop(camId);
            HikCamInfos[camId].IsOpened = false;
            HikCamInfos[camId].IsGrabbing = false;
            HikCamInfos[camId].Camera.Close();
        }

        /// <summary>
        /// 设置连续模式
        /// </summary>
        /// <returns></returns>
        public bool SetContinous()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                bool result = HikCamInfos[i].Camera.SetContinous();
                if (!result)
                {
                    return false;
                }
                HikCamInfos[i].TriggerMode = EnumCaptureMode.Continous;
            }
            return true;
        }
        public bool SetContinous(int camId)
        {
            bool result = HikCamInfos[camId].Camera.SetContinous();
            HikCamInfos[camId].TriggerMode = EnumCaptureMode.Continous;
            return result;
        }

        /// <summary>
        /// 设置软触发模式
        /// </summary>
        /// <returns></returns>
        public bool SetTriggerSoftware()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                bool result = HikCamInfos[i].Camera.SetTriggerSoftware();
                if (!result)
                {
                    return false;
                }
                HikCamInfos[i].TriggerMode = EnumCaptureMode.Trig;
            }
            return true;
        }
        public bool SetTriggerSoftware(int camId)
        {
            bool result = HikCamInfos[camId].Camera.SetTriggerSoftware();
            HikCamInfos[camId].TriggerMode = EnumCaptureMode.Trig;
            return result;
        }

        /// <summary>
        /// 设置硬触发模式
        /// </summary>
        /// <returns></returns>
        public bool SetTriggerLine0()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                bool result = HikCamInfos[i].Camera.SetTriggerLine0();
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public bool SetTriggerLine0(int camId)
        {
            bool result = HikCamInfos[camId].Camera.SetTriggerLine0();
            return result;
        }

        /// <summary>
        /// 软触发指令
        /// </summary>
        /// <returns></returns>
        public bool TrigCamBySoft()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                bool result = HikCamInfos[i].Camera.TrigCamBySoft();
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public bool TrigCamBySoft(int camId)
        {
            bool result = HikCamInfos[camId].Camera.TrigCamBySoft();
            return result;
        }

        /// <summary>
        /// 开启注册事件
        /// </summary>
        /// <returns></returns>
        public bool SetCcdCallbackFunc()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                bool result = MvsMethod.SetCameraCallbackFunc(HikCamInfos[i].Camera, ExposureEndEvents[i]);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public bool SetCcdCallbackFunc(int camId)
        {
            bool result = MvsMethod.SetCameraCallbackFunc(HikCamInfos[camId].Camera, ExposureEndEvents[camId]);
            return result;
        }

        /// <summary>
        /// 获取曝光时间和增益
        /// </summary>
        /// <returns></returns>
        public bool GetExposureAndGain()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                CHikCameraInfo info = new CHikCameraInfo();
                bool result = MvsMethod.GetExposureAndGain(HikCamInfos[i].Camera, ref info);
                HikCamInfos[i].Exposure = info.Exposure;
                HikCamInfos[i].Gain = info.Gain;
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public bool GetExposureAndGain(int camId)
        {
            CHikCameraInfo info = new CHikCameraInfo();
            bool result = MvsMethod.GetExposureAndGain(HikCamInfos[camId].Camera, ref info);
            HikCamInfos[camId].Exposure = info.Exposure;
            HikCamInfos[camId].Gain = info.Gain;
            return result;
        }

        /// <summary>
        /// 获取触发模式
        /// </summary>
        /// <returns></returns>
        public bool GetTriggerMode()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                CHikCameraInfo info = new CHikCameraInfo();
                bool result = MvsMethod.GetTriggerMode(HikCamInfos[i].Camera, ref info);
                HikCamInfos[i].TriggerMode = info.TriggerMode;
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public bool GetTriggerMode(int camId)
        {
            CHikCameraInfo info = new CHikCameraInfo();
            bool result = MvsMethod.GetTriggerMode(HikCamInfos[camId].Camera, ref info);
            HikCamInfos[camId].TriggerMode = info.TriggerMode;
            return result;
        }

        /// <summary>
        /// 设置曝光时间和增益
        /// </summary>
        /// <returns></returns>
        public bool SetExposureAndGain()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                bool result = MvsMethod.SetExposureAndGain(HikCamInfos[i].Camera, HikCamInfos[i]);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public bool SetExposureAndGain(int camId)
        {
            bool result = MvsMethod.SetExposureAndGain(HikCamInfos[camId].Camera, HikCamInfos[camId]);
            return result;
        }

        /// <summary>
        /// 获取相机图像
        /// </summary>
        /// <param name="ccd"></param>
        /// <param name="ho_Image"></param>
        public void GetHalconImage(int camId, ref HObject ho_Image, int nMsec = 1000, bool isTryAgain = false, int sec = 5)
        {
            MvsMethod.GetHalconImage(HikCamInfos[camId].Camera, ref ho_Image, nMsec, isTryAgain, sec);
        }

        #region  // ******************** Halcon 接口 ******************** //
        /// <summary>
        /// 初始化相机
        /// </summary>
        public void HInitCCD()
        {
            HalCamInfos = HalMethod.GetCCDList();
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        public void HOpen()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                HOpen(i);
            }
        }
        public void HOpen(int camId)
        {
            if (!HalCamInfos[camId].IsOpened)
            {
                HalCamInfos[camId].Hv_AcqHandle.Dispose();
                HalMethod.CmdOpenFramegrabber(ref HalCamInfos[camId].Hv_AcqHandle, HalCamInfos[camId].CcdName);
                HalCamInfos[camId].IsOpened = true;
            }
        }

        /// <summary>
        /// 设置连续模式
        /// </summary>
        public void HSetContinous()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                HalCamInfos[i].Hv_AcqHandle.SetCcdContinous();
            }
        }
        public void HSetContinous(int camId)
        {
            HalCamInfos[camId].Hv_AcqHandle.SetCcdContinous();
        }

        /// <summary>
        /// 开始抓图
        /// </summary>
        public void HStart()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                HalCamInfos[i].Hv_AcqHandle.GrabImageStart();
            }
        }
        public void HStart(int camId)
        {
            HalCamInfos[camId].Hv_AcqHandle.GrabImageStart();
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public void HClose()
        {
            for (int i = 0; i < NumberCCD; i++)
            {
                HalCamInfos[i].Hv_AcqHandle.CloseFramegrabber();
                HalCamInfos[i].IsOpened = false;
            }
        }
        public void HClose(int camId)
        {
            HalCamInfos[camId].Hv_AcqHandle.CloseFramegrabber();
            HalCamInfos[camId].IsOpened = false;
        }

        /// <summary>
        /// 抓图图像
        /// </summary>
        /// <param name="camId"></param>
        /// <param name="ho_Image"></param>
        public void HGrabImage(int camId, ref HObject ho_Image)
        {
            HalCamInfos[camId].Hv_AcqHandle.GrabImageAsync(ref ho_Image);
        }
        #endregion
    }
}