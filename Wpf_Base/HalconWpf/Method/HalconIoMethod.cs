using System;
using System.Xml.Linq;
using Wpf_Base.HalconWpf.Model;

namespace Wpf_Base.HalconWpf.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 12:32:55
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 12:32:55    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class HalconIoMethod
    {
        /// <summary>
        /// 卡尺标定
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="NumPixelSize"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public static bool SaveCaliper(string filename, double pixelsize)
        {
            try
            {
                // 创建文档
                XDocument xDoc = new XDocument();
                // 根节点
                XElement root = new XElement("Caliper");
                // 添加根节点 必须只有一个根节点
                xDoc.Add(root);
                // 节点
                XElement ele = new XElement("Property");
                ele.Add(new XAttribute("PixelSize", pixelsize));
                // 添加节点
                root.Add(ele);
                xDoc.Save(filename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool LoadCaliper(string filename, ref double pixelsize)
        {
            try
            {
                // 加载 xml 文件
                XDocument xDoc = XDocument.Load(filename);
                // 获取根
                XElement root = xDoc.Root;
                // 获取值
                pixelsize = double.Parse(root.Element("Property").Attribute("PixelSize").Value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        

        /// <summary>
        /// 九点标定
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool SaveCalibration(this CDataModel model, string filename)
        {
            try
            {
                XDocument xDoc = new XDocument();
                XElement root = new XElement("Calibration");
                xDoc.Add(root);
                // 九点标定
                XElement matrix = new XElement("Matrix");
                XAttribute A1 = new XAttribute("A1", model.A1);
                XAttribute B1 = new XAttribute("B1", model.B1);
                XAttribute C1 = new XAttribute("C1", model.C1);
                XAttribute A2 = new XAttribute("A2", model.A2);
                XAttribute B2 = new XAttribute("B2", model.B2);
                XAttribute C2 = new XAttribute("C2", model.C2);
                matrix.Add(A1, B1, C1, A2, B2, C2);
                root.Add(matrix);

                // 旋转标定
                XElement rotate = new XElement("Rotate");
                XAttribute centerX = new XAttribute("CenterX", model.CenterX);
                XAttribute centerY = new XAttribute("CenterY", model.CenterY);
                rotate.Add(centerX, centerY);
                root.Add(rotate);

                // 保存
                xDoc.Save(filename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool LoadCalibration(ref CDataModel model, string filename)
        {
            try
            {
                XDocument xDoc = XDocument.Load(filename);
                XElement roots = xDoc.Root;
                XElement element = roots.Element("Matrix");
                // 获取值
                model.A1 = double.Parse(element.Attribute("A1").Value);
                model.B1 = double.Parse(element.Attribute("B1").Value);
                model.C1 = double.Parse(element.Attribute("C1").Value);
                model.A2 = double.Parse(element.Attribute("A2").Value);
                model.B2 = double.Parse(element.Attribute("B2").Value);
                model.C2 = double.Parse(element.Attribute("C2").Value);
                element = roots.Element("Rotate");
                model.CenterX = double.Parse(element.Attribute("CenterX").Value);
                model.CenterY = double.Parse(element.Attribute("CenterY").Value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        

        /// <summary>
        /// ROI 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="roi"></param>
        /// <returns></returns>
        public static bool SaveROI(this CROI roi, string filename)
        {
            try
            {
                XDocument xDoc = new XDocument();
                XElement root = new XElement("ROI");
                xDoc.Add(root);
                XElement element = new XElement("Rectangle");
                XAttribute att1 = new XAttribute("Row1", roi.Row1);
                XAttribute att2 = new XAttribute("Col1", roi.Col1);
                XAttribute att3 = new XAttribute("Row2", roi.Row2);
                XAttribute att4 = new XAttribute("Col2", roi.Col2);
                element.Add(att1, att2, att3, att4);
                root.Add(element);

                element = new XElement("Circle");
                XAttribute att5 = new XAttribute("Row", roi.Row);
                XAttribute att6 = new XAttribute("Col", roi.Col);
                XAttribute att7 = new XAttribute("R", roi.R);
                element.Add(att5, att6, att7);
                root.Add(element);
                xDoc.Save(filename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool LoadROI(ref CROI roi, string filename)
        {
            try
            {
                XDocument xDoc = XDocument.Load(filename);
                XElement roots = xDoc.Root;
                XElement element = roots.Element("Rectangle");
                roi.Row1 = double.Parse(element.Attribute("Row1").Value);
                roi.Col1 = double.Parse(element.Attribute("Col1").Value);
                roi.Row2 = double.Parse(element.Attribute("Row2").Value);
                roi.Col2 = double.Parse(element.Attribute("Col2").Value);

                element = roots.Element("Circle");
                roi.Row = double.Parse(element.Attribute("Row").Value);
                roi.Col = double.Parse(element.Attribute("Col").Value);
                roi.R = double.Parse(element.Attribute("R").Value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        

        /// <summary>
        /// 测量对象
        /// </summary>
        /// <param name="match"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool SaveMetrology(this CDataModel model, string filename)
        {
            try
            {
                XDocument xDoc = new XDocument();
                XElement root = new XElement("Metrology");
                xDoc.Add(root);
                XElement element = new XElement("Reference");
                XAttribute att1 = new XAttribute("Row", model.RowRef);
                XAttribute att2 = new XAttribute("Col", model.ColRef);
                XAttribute att3 = new XAttribute("Ang", model.AngRef);
                element.Add(att1, att2, att3);
                root.Add(element);
                xDoc.Save(filename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool LoadMetrology(ref CDataModel model, string filename)
        {
            try
            {
                XDocument xDoc = XDocument.Load(filename);
                XElement roots = xDoc.Root;
                XElement element = roots.Element("Reference");
                model.RowRef = double.Parse(element.Attribute("Row").Value);
                model.ColRef = double.Parse(element.Attribute("Col").Value);
                model.AngRef = double.Parse(element.Attribute("Ang").Value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool SaveShapeModelRefer(this CDataModel model, string filename)
        {
            try
            {
                XDocument xDoc = new XDocument();
                XElement root = new XElement("ShapeModel");
                xDoc.Add(root);
                XElement element = new XElement("Reference");
                XAttribute att1 = new XAttribute("Row", model.RowRef);
                XAttribute att2 = new XAttribute("Col", model.ColRef);
                XAttribute att3 = new XAttribute("Ang", model.AngRef);
                element.Add(att1, att2, att3);
                root.Add(element);
                xDoc.Save(filename);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool LoadShapeModelRefer(ref CDataModel model, string filename)
        {
            try
            {
                XDocument xDoc = XDocument.Load(filename);
                XElement roots = xDoc.Root;
                XElement element = roots.Element("Reference");
                model.RowRef = double.Parse(element.Attribute("Row").Value);
                model.ColRef = double.Parse(element.Attribute("Col").Value);
                model.AngRef = double.Parse(element.Attribute("Ang").Value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region 生成指定文件名
        /// <summary>
        /// 生成检测参数文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenDetectionParamsName(string name = "default")
        {
            return "Config\\DetectionParams_" + name + ".xml";
        }

        /// <summary>
        /// 生成像素尺寸标定（卡尺、菲林片）文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenCaliperName(string name = "default")
        {
            return "Config\\Caliper_" + name + ".xml";
        }

        /// <summary>
        /// 生成九点标定文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenCalibrationName(string name = "default")
        {
            return "Config\\Calibration_" + name + ".xml";
        }

        /// <summary>
        /// 生成 ROI 文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenROIName(string name = "default")
        {
            return "Config\\ROI_" + name + ".xml";
        }

        /// <summary>
        /// 生成 OCR 训练文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenOcrTrainingFileName(string name = "default")
        {
            return "Module\\Ocr_" + name + ".trf";
        }

        /// <summary>
        /// 生成 OCR 模型名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GenOcrModelFileName(string name = "default", int flag = 0)
        {
            string filename = "Module\\Ocr_" + name;
            string ext = ".omc";
            if (flag == 1)
            {
                ext = ".osc";
            }
            else if (flag == 2)
            {
                ext = ".onc";
            }
            return filename + ext;
        }

        /// <summary>
        /// 生成形状模板名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenShapeModelName(string name = "default")
        {
            return "Module\\ShapeModel_" + name + ".shm";
        }

        /// <summary>
        /// 生成测量模型名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenMetrologyModelName(string name = "default")
        {
            return "Module\\Metrology_" + name + ".mem";
        }

        /// <summary>
        /// 生成测量模型参考名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GenMetrologyReferName(string name = "default")
        {
            return "Module\\Metrology_" + name + ".xml";
        }
        #endregion
    }
}