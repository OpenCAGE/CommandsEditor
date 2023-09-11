using CathodeLib;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Numerics;
using CATHODE;
using AlienPAK;
using CommandsEditor.DockPanels;
using CATHODE.Scripting;
using System;
using CATHODE.LEGACY;
using HelixToolkit.Wpf;
using static CATHODE.Materials.Material;
using System.Security.Cryptography;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Resources;
using HelixToolkit.Wpf.SharpDX;
using Media3D = System.Windows.Media.Media3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using System.IO;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SharpDX;

namespace CommandsEditor.Popups.UserControls
{
    /// <summary>
    /// Interaction logic for GUI_ModelViewer.xaml
    /// </summary>
    public partial class GUI_ModelViewer : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string info = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        protected bool Set<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        protected LevelContent _content;

        public GroupModel3D GroupModel { get; } = new GroupModel3D();
        public EffectsManager EffectsManager { get; }
        public Camera Camera { get; }

        public GUI_ModelViewer(LevelContent content, List<Model> models)
        {
            EffectsManager = new DefaultEffectsManager();
            Camera = new PerspectiveCamera();
            /*
            EffectsManager = new DefaultEffectsManager();
            Camera = new OrthographicCamera()
            {
                LookDirection = new Vector3D(0, -10, -10),
                Position = new Media3D.Point3D(0, 10, 10),
                UpDirection = new Vector3D(0, 1, 0),
                FarPlaneDistance = 5000,
                NearPlaneDistance = 0.1f
            };
            */

            _content = content;

            if (models != null)
                ShowModel(models);

            InitializeComponent();
        }

        public void ShowModel(List<Model> models)
        {
            for (int i = 0; i < models.Count; i++)
            {
                viewport.Items.Add(new MeshGeometryModel3D()
                {
                    Geometry = OffsetModel(models[i].modelIndex, models[i].position, models[i].rotation, out PBRMaterial material, out Media3D.Transform3DGroup transform),
                    Transform = transform,
                    Material = material
                });
            }
            //    GroupModel.AddNode();
            //view.ZoomExtents();
        }

        //https://github.com/MontagueM/Charm/blob/f64df305aff330eca5dee895b33a7192c05715c9/Charm/MainViewModel.cs#L234
        private Geometry3D OffsetModel(int modelIndex, Vector3D position, Vector3D rotation, out PBRMaterial material, out Media3D.Transform3DGroup transform, int materialIndex = -1)
        {
            //Get mesh data
            Models.CS2.Component.LOD.Submesh submesh = _content.resource.models.GetAtWriteIndex(modelIndex);
            MeshGeometry3D mesh = submesh.ToMeshGeometry3D();

            //Get material & texture data
            material = new PBRMaterial();
            try
            {
                ShadersPAK.ShaderMaterialMetadata mdlMeta = _content.resource.shaders.GetMaterialMetadataFromShader(_content.resource.materials.GetAtWriteIndex(materialIndex == -1 ? submesh.MaterialLibraryIndex : materialIndex), _content.resource.shadersIDX);

                ShadersPAK.MaterialTextureContext diffuseMap = mdlMeta.textures.FirstOrDefault(o => o.Type == ShadersPAK.ShaderSlot.DIFFUSE_MAP);
                if (diffuseMap != null)
                    material.AlbedoMap = new TextureModel(new MemoryStream(GetTexDB(diffuseMap.TextureInfo.Source).GetAtWriteIndex(diffuseMap.TextureInfo.BinIndex)?.ToDDS()));

                ShadersPAK.MaterialTextureContext normalMap = mdlMeta.textures.FirstOrDefault(o => o.Type == ShadersPAK.ShaderSlot.NORMAL_MAP);
                if (normalMap != null)
                    material.NormalMap = new TextureModel(new MemoryStream(GetTexDB(normalMap.TextureInfo.Source).GetAtWriteIndex(normalMap.TextureInfo.BinIndex)?.ToDDS()));
            }
            catch (Exception ex2)
            {
                Console.WriteLine("ERROR: " + ex2.ToString());
            }

            //Get transform data
            transform = new Media3D.Transform3DGroup();
            transform.Children.Add(new Media3D.ScaleTransform3D(submesh.ScaleFactor, submesh.ScaleFactor, submesh.ScaleFactor));
            System.Numerics.Quaternion q = System.Numerics.Quaternion.CreateFromYawPitchRoll((float)(rotation.Y * Math.PI / 180.0f), (float)(rotation.X * Math.PI / 180.0f), (float)(rotation.Z * Math.PI / 180.0f));
            transform.Children.Add(new Media3D.RotateTransform3D(new Media3D.QuaternionRotation3D(new System.Windows.Media.Media3D.Quaternion(q.X, q.Y, q.Z, q.W))));
            transform.Children.Add(new Media3D.TranslateTransform3D(position.X, position.Y, position.Z));

            return mesh;
        }

        private Textures GetTexDB(Texture.TextureSource source)
        {
            return source == Texture.TextureSource.GLOBAL ? _content.resource.textures_global : _content.resource.textures;
        }

        public class Model
        {
            public Model(int modelIndex, int materialIndex = -1)
            {
                Create(modelIndex, materialIndex);
            }
            public Model(int modelIndex, Vector3D position, Vector3D rotation, int materialIndex = -1)
            {
                Create(modelIndex, position, rotation, materialIndex);
            }
            public Model(int modelIndex, System.Numerics.Vector3 position, System.Numerics.Vector3 rotation, int materialIndex = -1)
            {
                Create(modelIndex, new Vector3D(position.X, position.Y, position.Z), new Vector3D(rotation.X, rotation.Y, rotation.Z), materialIndex);
            }
            public Model(int modelIndex, cTransform transform, int materialIndex = -1)
            {
                Create(modelIndex, new Vector3D(transform.position.X, transform.position.Y, transform.position.Z), new Vector3D(transform.rotation.X, transform.rotation.Y, transform.rotation.Z), materialIndex);
            }

            private void Create(int modelIndex, int materialIndex = -1)
            {
                Create(modelIndex, new Vector3D(0,0,0), new Vector3D(0,0,0), materialIndex);
            }
            private void Create(int modelIndex, System.Numerics.Vector3 position, System.Numerics.Vector3 rotation, int materialIndex = -1)
            {
                Create(modelIndex, new Vector3D(position.X, position.Y, position.Z), new Vector3D(rotation.X, rotation.Y, rotation.Z), materialIndex);
            }
            private void Create(int modelIndex, Vector3D position, Vector3D rotation, int materialIndex = -1)
            {
                this.modelIndex = modelIndex;
                this.materialIndex = materialIndex;
                this.position = position;
                this.rotation = rotation;
            }

            public int modelIndex;
            public int materialIndex;
            public Vector3D position;
            public Vector3D rotation;
        }
    }
}
