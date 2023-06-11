using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Wpf_Base.HalconWpf.Model;

namespace Wpf_Base.HalconWpf.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 17:00:13
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 17:00:13    CoderMan/CoderdMan1012         首次编写         
    ///
    public class InitMethod
    {
        /// <summary>
        /// 初始化定标点
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<CDataModel> InitMarkers(List<Point> pts, double NumAngle, EnumRotateType rotateType = EnumRotateType.Rotate_3次旋转)
        {
            ObservableCollection<CDataModel> datalist = new ObservableCollection<CDataModel>();
            for (int i = 0; i < 9; i++)
            {
                datalist.Add(new CDataModel { Header = "位置点 " + (i + 1), RobotX = pts[i].X, RobotY = pts[i].Y });
            }
            if (rotateType == EnumRotateType.Rotate_3次旋转)
            {
                datalist.Add(new CDataModel { Header = "旋转点 1", Angle = 0 });
                datalist.Add(new CDataModel { Header = "旋转点 2", Angle = -NumAngle });
                datalist.Add(new CDataModel { Header = "旋转点 3", Angle = 2 * NumAngle });
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    datalist.Add(new CDataModel { Header = "旋转点 " + (i + 1), Angle = NumAngle });
                }
            }
            return datalist;
        }


        /// <summary>
        /// Halcon 算子
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<CDataModel> InitOperators()
        {
            // 获取所有算子
            List<string> names = new List<string>();
            List<string> remarks = new List<string>();
            List<EnumHalOperator> halOperators = new List<EnumHalOperator>();
            foreach (EnumHalOperator item in Enum.GetValues(typeof(EnumHalOperator)))
            {
                halOperators.Add(item);
                DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
                names.Add(item.ToString());
                remarks.Add(attributes.Description);
            }
            ObservableCollection<CDataModel> datalist = new ObservableCollection<CDataModel>();
            for (int i = 0; i < names.Count; i++)
            {
                datalist.Add(new CDataModel { Name = names[i], Remark = remarks[i], HalOperator = halOperators[i] });
            }
            return datalist;
        }
    }
}