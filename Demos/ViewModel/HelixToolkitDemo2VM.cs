using GalaSoft.MvvmLight;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Demos.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2022/7/10 19:20:12
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/7/10 19:20:12    Taosy.W                 
    ///
    public class HelixToolkitDemo2VM : ViewModelBase
    {
        /// <summary>
        /// 绑定枚举
        /// </summary>
        public IEnumerable<SelectionHitMode> SelectionModes => Enum.GetValues(typeof(SelectionHitMode)).Cast<SelectionHitMode>();

        /// <summary>
        /// MeshGeometry3D 类型
        /// </summary>
        public MeshGeometry3D GlassGeometry
        {
            get
            {
                MeshBuilder builder = new MeshBuilder(true, true);
                Point[] profile = new[] { new Point(0, 0.4), new Point(0.06, 0.36), new Point(0.1, 0.1), new Point(0.34, 0.1), new Point(0.4, 0.14), new Point(0.5, 0.5), new Point(0.7, 0.56), new Point(1, 0.46) };
                builder.AddRevolvedGeometry(profile, null, new Point3D(0, 0, 0), new Vector3D(0, 0, 1), 100);
                return builder.ToMesh(true);
            }
        }

        /// <summary>
        /// 枚举选中值
        /// </summary>
        public SelectionHitMode SelectionMode
        {
            get => RectangleSelectionCommand != null ? RectangleSelectionCommand.SelectionHitMode : SelectionHitMode.Touch;

            set => RectangleSelectionCommand.SelectionHitMode = value;
        }

        /// <summary>
        /// 选中的目标类型名称
        /// </summary>
        private string strSelectedVisuals = "";
        public string StrSelectedVisuals
        {
            get => strSelectedVisuals;
            set => Set(ref strSelectedVisuals, value);
        }

        private ObservableCollection<Visual3D> hViewObjects = new ObservableCollection<Visual3D>();
        public ObservableCollection<Visual3D> HViewObjects
        {
            get => hViewObjects;
            set => Set(ref hViewObjects, value);
        }

        private IList<Model3D> selectedModels;

        public IHelixViewport3D HViewPort3D { get; set; }

        public RectangleSelectionCommand RectangleSelectionCommand { get; set; }

        public PointSelectionCommand PointSelectionCommand { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HelixToolkitDemo2VM()
        {

        }

        /// <summary>
        /// 增加一个对象
        /// </summary>
        private void Add()
        {
            if (HViewObjects.Count == 0)
            {
                // 光源必备
                HViewObjects.Add(new DefaultLights());
                hViewObjects.Add(new GridLinesVisual3D());
            }
            HViewObjects.Add(new HelixVisual3D { Origin = new Point3D(-3, 0, 0), Length = 10, Radius = 2, Diameter = 0.5, Turns = 6, Fill = GradientBrushes.Hue });
        }

        public void HandleSelectionVisualsEvent(object sender, VisualsSelectedEventArgs args)
        {
            IList<Visual3D> visuals = args.SelectedVisuals;
            StrSelectedVisuals = visuals == null ? "" : string.Join("; ", visuals.Select(x => x.GetType().Name));
        }

        public void HandleSelectionModelsEvent(object sender, ModelsSelectedEventArgs args)
        {
            ChangeMaterial(selectedModels, Materials.Blue);
            selectedModels = args.SelectedModels;
            if (args is ModelsSelectedByRectangleEventArgs rectangleSelectionArgs)
            {
                ChangeMaterial(selectedModels, rectangleSelectionArgs.Rectangle.Size != default ? Materials.Red : Materials.Green);
            }
            else
            {
                ChangeMaterial(selectedModels, Materials.Gold);
            }
        }

        /// <summary>
        /// 着色
        /// </summary>
        /// <param name="models"></param>
        /// <param name="material"></param>
        public void ChangeMaterial(IEnumerable<Model3D> models, Material material)
        {
            if (models == null)
            {
                return;
            }

            foreach (Model3D model in models)
            {
                if (model is GeometryModel3D geometryModel)
                {
                    geometryModel.Material = geometryModel.BackMaterial = material;
                }
            }
        }
    }
}