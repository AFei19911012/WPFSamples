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
                new CDataModel{ Name = "2.11 测量工具：长度、角度", ImgPath="pack://application:,,,/Resource/Image/B.png"},

                new CDataModel{ Name = "4.1 图像采集：调用相机接口", ImgPath="pack://application:,,,/Resource/Image/D.png"},
                new CDataModel{ Name = "4.2 读取本地图像、保存图像、保存窗体", ImgPath="pack://application:,,,/Resource/Image/D.png"},
                new CDataModel{ Name = "4.3 拟合圆", ImgPath="pack://application:,,,/Resource/Image/D.png"},
                new CDataModel{ Name = "4.4 九点标定", ImgPath="pack://application:,,,/Resource/Image/D.png"},

                new CDataModel{ Name = "5.1 计算别针数量和角度", ImgPath="pack://application:,,,/Resource/Image/E.png"},
                new CDataModel{ Name = "5.2 牙模切割", ImgPath="pack://application:,,,/Resource/Image/E.png"},

                new CDataModel{ Name = "6.1 PCB板电路检测", ImgPath="pack://application:,,,/Resource/Image/F.png"},
                new CDataModel{ Name = "6.2 轴承滚子检测", ImgPath="pack://application:,,,/Resource/Image/F.png"},
                new CDataModel{ Name = "6.3 LED灯珠检测", ImgPath="pack://application:,,,/Resource/Image/F.png"},

                new CDataModel{ Name = "8.2 测量模型：定位圆", ImgPath="pack://application:,,,/Resource/Image/H.png"},

                new CDataModel{ Name = "9.1 MLP应用：简单车牌识别", ImgPath="pack://application:,,,/Resource/Image/I.png"},
                new CDataModel{ Name = "9.2 MLP应用：车牌识别", ImgPath="pack://application:,,,/Resource/Image/I.png"},
                new CDataModel{ Name = "9.3 MLP应用：数字识别", ImgPath="pack://application:,,,/Resource/Image/I.png"},
            };
        }
    }
}