using CathodeLib;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Numerics;
using CATHODE;
using AlienPAK;
using CommandsEditor.DockPanels;
using CATHODE.Scripting;
using System;
using CATHODE.LEGACY;
using HelixToolkit.Wpf;
using System.Security.Cryptography;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Resources;
using OpenCAGE;

namespace CommandsEditor.Popups.UserControls
{
    /// <summary>
    /// Interaction logic for GUI_ModelViewer.xaml
    /// </summary>
    public partial class GUI_ModelViewer : UserControl
    {
        protected LevelContent _content;

        public GUI_ModelViewer(LevelContent content)
        {
            _content = content;

            InitializeComponent();
        }

        public void ShowModel(List<Model> models)
        {
            Model3DGroup group = new Model3DGroup();
            for (int i = 0; i < models.Count; i++)
                group.Children.Add(OffsetModel(models[i].modelIndex, models[i].position, models[i].rotation));
            modelPreview.Content = group;
            myView.ZoomExtents();
        }
        
        private Model3DGroup OffsetModel(int modelIndex, Vector3D position, Vector3D rotation, int materialIndex = -1)
        {
            //Get mesh data
            Models.CS2.Component.LOD.Submesh submesh = _content.resource.models.GetAtWriteIndex(modelIndex);
            GeometryModel3D submeshGeo = submesh.ToGeometryModel3D();

            //Get material & texture data
            if (SettingsManager.GetBool(Singleton.Settings.ShowTexOpt))
            {
                try
                {
                    ShadersPAK.ShaderMaterialMetadata mdlMeta = _content.resource.shaders_legacy.GetMaterialMetadataFromShader(_content.resource.materials.GetAtWriteIndex(materialIndex == -1 ? submesh.MaterialLibraryIndex : materialIndex));
                    ShadersPAK.MaterialTextureContext mdlMetaDiff = mdlMeta.textures.FirstOrDefault(o => o.Type == ShadersPAK.ShaderSlot.DIFFUSE_MAP);
                    if (mdlMetaDiff != null)
                    {
                        Textures tex = mdlMetaDiff.TextureInfo.Source == CATHODE.Materials.Material.Texture.TextureSource.GLOBAL ? _content.resource.textures_global : _content.resource.textures;
                        Textures.TEX4 diff = tex.GetAtWriteIndex(mdlMetaDiff.TextureInfo.BinIndex);
                        byte[] diffDDS = diff?.ToDDS();
                        DiffuseMaterial mat = new DiffuseMaterial(new ImageBrush(diffDDS?.ToBitmap()?.ToImageSource()));
                        submeshGeo.Material = mat;
                        //TODO: normals?
                    }
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.ToString());
                }
            }

            //Get transform data
            Transform3DGroup transform = new Transform3DGroup();
            transform.Children.Add(new ScaleTransform3D(submesh.ScaleFactor, submesh.ScaleFactor, submesh.ScaleFactor));
            System.Numerics.Quaternion q = System.Numerics.Quaternion.CreateFromYawPitchRoll((float)(rotation.Y * Math.PI / 180.0f), (float)(rotation.X * Math.PI / 180.0f), (float)(rotation.Z * Math.PI / 180.0f));
            transform.Children.Add(new RotateTransform3D(new QuaternionRotation3D(new System.Windows.Media.Media3D.Quaternion(q.X, q.Y, q.Z, q.W))));
            transform.Children.Add(new TranslateTransform3D(position.X, position.Y, position.Z));

            //Submit
            Model3DGroup model = new Model3DGroup();
            model.Transform = transform;
            model.Children.Add(submeshGeo);
            return model;
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
            public Model(int modelIndex, Vector3 position, Vector3 rotation, int materialIndex = -1)
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
            private void Create(int modelIndex, Vector3 position, Vector3 rotation, int materialIndex = -1)
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
