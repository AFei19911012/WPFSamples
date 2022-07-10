using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Microsoft.Win32;

namespace Demos.Demo
{
    /// <summary>
    /// HelixToolkitDemo.xaml 的交互逻辑
    /// </summary>
    public partial class HelixToolkitDemo : UserControl
    {
        public Point3DCollection TubeVisual3DPath { get; set; }

        private static readonly Random r = new Random();

        private static readonly Brush[] BaseBrush1 = { Brushes.Blue, Brushes.Yellow, Brushes.Red, Brushes.Green };

        private static readonly Brush[] BaseBrush2 = { Brushes.Yellow, Brushes.Blue, Brushes.Green, Brushes.Red };

        private readonly Stopwatch watch = new Stopwatch();
        public Point3DCollection Points { get; set; }
        private int NumberOfPoints { get; set; } = 1000;

        private readonly string SaveFileFilter = "Bitmap Files(*.png;*.jpg;)|*.png;*.jpg;*.bmp|XAML Files(*.xaml)|*.xaml|Wavefront Files(*.obj)|*.obj|" +
                                                 "Wavefront Files zipped(*.ojbz)|*.objz|Extensible 3D Graphics Files(*.x3d)|*.x3d|Collada Fies(*.dae)|*.dae|" +
                                                 "STereoLithograhy(*.stl)|*.stl";

        public HelixToolkitDemo()
        {
            InitializeComponent();

            InitTube();
            AddBases(MV3D, 24, 3, 30);

            DataContext = this;

            //CompositionTarget.Rendering += OnCompositionTargetRendering;
            //Points = new Point3DCollection(GeneratePoints(NumberOfPoints, watch.ElapsedMilliseconds * 0.001));

            // 设置相机
            SetCamera(HView3D);
            SetCamera(HView3D1);
            SetCamera(HView3D2);
        }

        /// <summary>
        /// 初始化弯管路径
        /// </summary>
        private void InitTube()
        {
            // 定义一条路径
            int n = 1800;
            double r = Math.Sqrt(3) / 3;
            TubeVisual3DPath = CreatePath(0, Math.PI * 2, n, u => Math.Cos(u), u => Math.Sin(u) + r, u => Math.Cos(3 * u) / 3);
            //TubeVisual3DPath = new Point3DCollection
            //{
            //    new Point3D(0, 0, 0),
            //    new Point3D(0, 1, 0),
            //    new Point3D(0, 1, 0),
            //    new Point3D(0, 1.5, 0.5),
            //    new Point3D(0, 1.5, 0.5),
            //    new Point3D(-1, 1.5, 0.5),
            //    new Point3D(-1, 1.5, 0.5),
            //    new Point3D(-1, 1.5, 1.5),
            //};
        }

        /// <summary>
        /// 设置相机
        /// </summary>
        /// <param name="view"></param>
        private void SetCamera(HelixViewport3D view)
        {
            (view.Camera as PerspectiveCamera).UpDirection = new Vector3D(0, 0, 1);
            (view.Camera as PerspectiveCamera).LookDirection = new Vector3D(-1, 0, 0);
            (view.Camera as PerspectiveCamera).FieldOfView = 45;
        }

        /// <summary>
        /// DNA 中间的连接棒
        /// </summary>
        /// <param name="model"></param>
        /// <param name="number"></param>
        /// <param name="turns"></param>
        /// <param name="length"></param>
        private void AddBases(ModelVisual3D model, int number, double turns, double length)
        {
            double b = turns * 2 * Math.PI;
            double l = length;
            double p1 = 0d;
            double p2 = 3.14;
            for (int i = 0; i < number; i++)
            {
                double u = (double)i / (number - 1);
                double bu = b * u;
                double x1 = Math.Cos(bu + p1) + Math.Cos(bu + p1);
                double y1 = Math.Sin(bu + p1) + Math.Sin(bu + p1);
                double z = u * l;
                double x2 = Math.Cos(bu + p2) + Math.Cos(bu + p2);
                double y2 = Math.Sin(bu + p2) + Math.Sin(bu + p2);
                Point3D pt1 = new Point3D(x1, y1, z);
                Point3D pt2 = new Point3D(x2, y2, z);
                Point3D pt3 = new Point3D(0, 0, z);

                int j = r.Next(4);
                Brush brush1 = BaseBrush1[j];
                Brush brush2 = BaseBrush2[j];

                PipeVisual3D ts = new PipeVisual3D
                {
                    Point1 = pt1,
                    Point2 = pt3,
                    Diameter = 0.4,
                    Material = MaterialHelper.CreateMaterial(brush1)
                };
                model.Children.Add(ts);

                PipeVisual3D ts2 = new PipeVisual3D
                {
                    Point1 = pt3,
                    Point2 = pt2,
                    Diameter = 0.4,
                    Material = MaterialHelper.CreateMaterial(brush2)
                };
                model.Children.Add(ts2);
            }
        }

        /// <summary>
        /// 生成三维点集
        /// </summary>
        /// <param name="n"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEnumerable<Point3D> GeneratePoints(int n, double time)
        {
            const double R = 2;
            const double Q = 0.5;
            for (int i = 0; i < n; i++)
            {
                double t = Math.PI * 2 * i / (n - 1);
                double u = (t * 24) + (time * 5);
                Point3D pt = new Point3D(Math.Cos(t) * (R + (Q * Math.Cos(u))), Math.Sin(t) * (R + (Q * Math.Cos(u))), Q * Math.Sin(u));
                yield return pt;
                if (i > 0 && i < n - 1)
                {
                    yield return pt;
                }
            }
        }

        /// <summary>
        /// 刷新绘图事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            if (Points == null || Points.Count != NumberOfPoints)
            {
                Points = new Point3DCollection(GeneratePoints(NumberOfPoints, watch.ElapsedMilliseconds * 0.001));
            }
        }

        /// <summary>
        /// 创建弯管路径
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="n"></param>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="fz"></param>
        /// <returns></returns>
        private Point3DCollection CreatePath(double min, double max, int n, Func<double, double> fx, Func<double, double> fy, Func<double, double> fz)
        {
            Point3DCollection list = new Point3DCollection(n);
            for (int i = 0; i < n; i++)
            {
                double u = min + ((max - min) * i / n);
                list.Add(new Point3D(fx(u), fy(u), fz(u)));
            }
            return list;
        }

        /// <summary>
        /// 增加模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            HelixVisual3D helix = new HelixVisual3D
            {
                Origin = new Point3D(10, 0, 0),
                Radius = 2,
                Diameter = 0.5,
                Turns = 10,
                Length = 30,
                Phase = 0,
                Fill = (Brush)FindResource("RainbowBrush"),
            };
            MV3D.Children.Add(helix);
        }

        /// <summary>
        /// 模型大小自适应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonZoom_Click(object sender, RoutedEventArgs e)
        {
            HView3D.ZoomExtents(100);
            HView3D1.ZoomExtents(100);
            HView3D2.ZoomExtents(100);
        }

        /// <summary>
        /// 显示当前位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HView3D_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (HView3D.CursorPosition != null)
            {
                TB_CursorPositionX.Text = HView3D.CursorPosition.Value.X.ToString();
                TB_CursorPositionY.Text = HView3D.CursorPosition.Value.Y.ToString();
                TB_CursorPositionZ.Text = HView3D.CursorPosition.Value.Z.ToString();
            }
            if (HView3D.CursorOnElementPosition != null)
            {
                TB_CursorOnElementPositionX.Text = HView3D.CursorOnElementPosition.Value.X.ToString();
                TB_CursorOnElementPositionY.Text = HView3D.CursorOnElementPosition.Value.Y.ToString();
                TB_CursorOnElementPositionZ.Text = HView3D.CursorOnElementPosition.Value.Z.ToString();
            }

            if (HView3D.CursorOnConstructionPlanePosition != null)
            {
                TB_CursorOnConstructionPlanePositionX.Text = HView3D.CursorOnConstructionPlanePosition.Value.X.ToString();
                TB_CursorOnConstructionPlanePositionY.Text = HView3D.CursorOnConstructionPlanePosition.Value.Y.ToString();
                TB_CursorOnConstructionPlanePositionZ.Text = HView3D.CursorOnConstructionPlanePosition.Value.Z.ToString();
            }
        }

        /// <summary>
        /// 相机关联动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HView3D1_CameraChanged(object sender, RoutedEventArgs e)
        {
            if (HView3D1 != null && HView3D2 != null)
            {
                CameraHelper.Copy(HView3D1.Camera, HView3D2.Camera);
            }
        }
        private void HView3D2_CameraChanged(object sender, RoutedEventArgs e)
        {
            if (HView3D1 != null && HView3D2 != null)
            {
                CameraHelper.Copy(HView3D2.Camera, HView3D1.Camera);
            }
        }

        /// <summary>
        /// 特定方向旋转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRotate_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as RepeatButton).Content.ToString();
            Vector3D axis = new Vector3D(0, 0, 0);
            if (name.Contains("X"))
            {
                axis.X = 1;
            }
            else if (name.Contains("Y"))
            {
                axis.Y = 1;
            }
            else if (name.Contains("Z"))
            {
                axis.Z = 1;
            }
            int angle = 10;
            Matrix3D matrix = MV3D.Transform.Value;
            matrix.Rotate(new Quaternion(axis, angle));
            MV3D.Transform = new MatrixTransform3D(matrix);
        }

        /// <summary>
        /// 保存模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "导出模型",
                Filter = SaveFileFilter,
                InitialDirectory = "data",
                RestoreDirectory = true,
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            string filename = dialog.FileName;
            HView3D1.Export(filename);
        }

        /// <summary>
        /// 设置相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCamera_Click(object sender, RoutedEventArgs e)
        {
            SetCamera(HView3D);
            SetCamera(HView3D1);
            SetCamera(HView3D2);
            // 缩放还原
            ButtonZoom_Click(null, null);
        }
    }
}
