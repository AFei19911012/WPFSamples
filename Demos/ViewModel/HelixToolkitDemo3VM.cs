using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using System;
using System.Windows.Media.Media3D;

namespace Demos.ViewModel
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderMan1012
    /// Created Time: 2022/7/10 19:51:51
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     2022/7/10 19:51:51    Taosy.W                 
    ///
    public class HelixToolkitDemo3VM : ViewModelBase
    {
        private Model3DGroup currentModel = new Model3DGroup();
        public Model3DGroup CurrentModel
        {
            get => currentModel;
            set => Set(ref currentModel, value);
        }

        private object selectedObject;
        public object SelectedObject
        {
            get => selectedObject;

            set => Set(ref selectedObject, value);
        }

        private readonly string OpenFileFilter = "3D 模型文件(*.3ds;*.obj;*.stl;*lwo;*.ply)|*.3ds;*.obj;*.stl;*lwo;*.ply";
        private readonly string SaveFileFilter = "Bitmap Files(*.png;*.jpg;)|*.png;*.jpg|XAML Files(*.xaml)|*.xaml|Wavefront Files(*.obj)|*.obj|" +
                                                 "Wavefront Files zipped(*.ojbz)|*.objz|Extensible 3D Graphics Files(*.x3d)|*.x3d|Collada Fies(*.dae)|*.dae|" +
                                                 "STereoLithograhy(*.stl)|*.stl";
        public IHelixViewport3D HViewPort3D { get; set; }

        public HelixToolkitDemo3VM()
        {
        }

        public RelayCommand CmdLoadModel => new Lazy<RelayCommand>(() => new RelayCommand(LoadModel)).Value;
        private void LoadModel()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "选择 3D 模型文件",
                Filter = OpenFileFilter,
                InitialDirectory = "data",
                RestoreDirectory = true,
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            string filename = dialog.FileName;
            ModelImporter mi = new ModelImporter();
            Model3DGroup model = mi.Load(filename, null, true);
            CurrentModel = model;
            HViewPort3D.ZoomExtents(0);
        }

        public RelayCommand CmdExportModel => new Lazy<RelayCommand>(() => new RelayCommand(ExportModel)).Value;
        private void ExportModel()
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
            HViewPort3D.Export(filename);
        }

        public RelayCommand CmdSetMaterial => new Lazy<RelayCommand>(() => new RelayCommand(SetMaterial)).Value;
        private void SetMaterial()
        {
            Material material = MaterialHelper.CreateMaterial(GradientBrushes.Rainbow);
            // 逐元素着色
            Model3DGroup model = new Model3DGroup();
            for (int i = 0; i < CurrentModel.Children.Count; i++)
            {
                Geometry3D geometry = ((GeometryModel3D)CurrentModel.Children[i]).Geometry;
                model.Children.Add(new GeometryModel3D { Geometry = geometry, Material = material, BackMaterial = material });
            }
            CurrentModel = model;
        }
    }
}