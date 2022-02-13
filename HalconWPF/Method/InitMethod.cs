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
                new CDataModel{ Name = "A图像采集", ImgPath="pack://application:,,,/Resource/Image/A.png"},
                new CDataModel{ Name = "A图像读取和保存", ImgPath="pack://application:,,,/Resource/Image/A.png"},
                new CDataModel{ Name = "B计算别针数量和角度", ImgPath="pack://application:,,,/Resource/Image/B.png"},
                new CDataModel{ Name = "C拟合圆", ImgPath="pack://application:,,,/Resource/Image/C.png"},
                new CDataModel{ Name = "D缺陷检测-PCB", ImgPath="pack://application:,,,/Resource/Image/D.png"},
                new CDataModel{ Name = "E九点标定1", ImgPath="pack://application:,,,/Resource/Image/E.png"},
                new CDataModel{ Name = "D缺陷检测-轴承", ImgPath="pack://application:,,,/Resource/Image/D.png"},
                new CDataModel{ Name = "D缺陷检测-牙模", ImgPath="pack://application:,,,/Resource/Image/D.png"},
                new CDataModel{ Name = "D缺陷检测-针脚", ImgPath="pack://application:,,,/Resource/Image/D.png"},
                new CDataModel{ Name = "MeasureTools", ImgPath="pack://application:,,,/Resource/Image/M.png"},
            };
        }
    }
}