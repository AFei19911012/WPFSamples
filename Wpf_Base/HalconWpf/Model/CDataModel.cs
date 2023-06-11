using GalaSoft.MvvmLight;

namespace Wpf_Base.HalconWpf.Model
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/03 12:34:12
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/03 12:34:12    CoderMan/CoderdMan1012         首次编写         
    ///
    public class CDataModel : ViewModelBase
    {
        /// <summary>
        /// 机器人移动点位
        /// </summary>
        public string Header { get; set; }

        private double robotX;
        public double RobotX
        {
            get => robotX;
            set => Set(ref robotX, value);
        }

        private double robotY;
        public double RobotY
        {
            get => robotY;
            set => Set(ref robotY, value);
        }

        private double imageX;
        public double ImageX
        {
            get => imageX;
            set => Set(ref imageX, value);
        }

        private double imageY;
        public double ImageY
        {
            get => imageY;
            set => Set(ref imageY, value);
        }

        private double angle;
        public double Angle
        {
            get => angle;
            set => Set(ref angle, value);
        }


        /// <summary>
        /// 九点标定结果
        /// </summary>
        public double A1 { get; set; }
        public double B1 { get; set; }
        public double C1 { get; set; }
        public double A2 { get; set; }
        public double B2 { get; set; }
        public double C2 { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }


        /// <summary>
        /// 测量列表
        /// </summary>
        public int Index { get; set; }
        public string Name { get; set; }


        /// <summary>
        /// 标注
        /// </summary>
        private string remark = "";
        public string Remark
        {
            get => remark;
            set => Set(ref remark, value);
        }


        /// <summary>
        /// Halcon 算子
        /// </summary>
        public EnumHalOperator HalOperator { get; set; }


        /// <summary>
        /// 参考位置和角度
        /// </summary>
        public double RowRef { get; set; }
        public double ColRef { get; set; }
        public double AngRef { get; set; }


        public CDataModel()
        {

        }


        public CDataModel(double row, double col, double ang)
        {
            RowRef = row;
            ColRef = col;
            AngRef = ang;
        }


        public CDataModel(CDataModel model)
        {
            Header = model.Header;
            RobotX = model.RobotX;
            RobotY = model.RobotY;
            ImageX = model.ImageX;
            ImageY = model.ImageY;
            Angle = model.Angle;
        }
    }
}