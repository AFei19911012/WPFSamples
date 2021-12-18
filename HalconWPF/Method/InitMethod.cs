using HalconWPF.Model;
using System.Collections.ObjectModel;

namespace HalconWPF.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/12/7 8:58:02
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/12/7 8:58:02    Taosy.W                 
    ///
    public static class InitMethod
    {
        public static ObservableCollection<CDataModel> GetDataList()
        {
            return new ObservableCollection<CDataModel>
            {
                new CDataModel{ Name = "HalconTools", ImgPath="pack://application:,,,/Resource/Image/H.png"},
                new CDataModel{ Name = "AcquisitionImage", ImgPath="pack://application:,,,/Resource/Image/A.png"},
                new CDataModel{ Name = "ImageReadSave", ImgPath="pack://application:,,,/Resource/Image/I.png"},
                new CDataModel{ Name = "ClipNumberAndAngle", ImgPath="pack://application:,,,/Resource/Image/C.png"},
                new CDataModel{ Name = "CircleFitting", ImgPath="pack://application:,,,/Resource/Image/C.png"},
                new CDataModel{ Name = "PcbDefectDetection", ImgPath="pack://application:,,,/Resource/Image/P.png"},
                new CDataModel{ Name = "CalibrationWithPoints", ImgPath="pack://application:,,,/Resource/Image/C.png"},
                new CDataModel{ Name = "BearingDefectDetection", ImgPath="pack://application:,,,/Resource/Image/B.png"},
                new CDataModel{ Name = "TeethDetection", ImgPath="pack://application:,,,/Resource/Image/T.png"},
                new CDataModel{ Name = "BastingDefectDetection", ImgPath="pack://application:,,,/Resource/Image/B.png"},
                new CDataModel{ Name = "DomainModule", ImgPath="pack://application:,,,/Resource/Image/D.png"},
                new CDataModel{ Name = "CaliperCalibration", ImgPath="pack://application:,,,/Resource/Image/C.png"},
                new CDataModel{ Name = "MeasureTools", ImgPath="pack://application:,,,/Resource/Image/M.png"},
            };
        }
    }
}