using AlienPAK;
using CATHODE;
using CATHODE.LEGACY;
using CATHODE.Scripting;
using CATHODE.ShaderTypes;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Scripts;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CommandsEditor.Popups.UserControls
{
    /// <summary>
    /// Interaction logic for GUI_ModelViewer.xaml
    /// </summary>
    public partial class GUI_ModelViewer : UserControl
    {
        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        public GUI_ModelViewer()
        {
            InitializeComponent();
        }

        public void ShowModel(List<Model> models)
        {
            ShowModel(models, true);
        }

        public void ShowModel(List<Model> models, bool zoomExtents)
        {
            Model3DGroup group = new Model3DGroup();
            for (int i = 0; i < models.Count; i++)
                group.Children.Add(OffsetModel(models[i].modelIndex, models[i].position, models[i].rotation, models[i].materialIndex));
            modelPreview.Content = group;

            if (zoomExtents)
            {
                myView.ModelUpDirection = new Vector3D(0, 1, 0);
                myView.Camera.UpDirection = new Vector3D(0, 1, 0);
                myView.Camera.LookDirection = new Vector3D(-0.5, -0.5, 1);
                myView.ZoomExtents();
            }
        }
        
        private Model3DGroup OffsetModel(int modelIndex, Vector3D position, Vector3D rotation, int materialIndex)
        {
            //Get mesh data
            Models.CS2.Component.LOD.Submesh submesh = Content.resource.models.GetAtWriteIndex(modelIndex);
            GeometryModel3D submeshGeo = submesh.ToGeometryModel3D();

            //Get material & texture data
            if (SettingsManager.GetBool(Singleton.Settings.ShowTexOpt))
            {
                Materials.Material material = Content.resource.materials.GetAtWriteIndex(materialIndex == -1 ? submesh.MaterialIndex : materialIndex);
                Shaders.Shader shader = Content.resource.shaders.Entries[material.ShaderIndex];
                MaterialApplier.ApplyMaterial(submeshGeo, material, shader, Content.resource.textures);
            }

            //Get transform data
            Transform3DGroup transform = new Transform3DGroup();
            transform.Children.Add(new ScaleTransform3D(submesh.VertexScale, submesh.VertexScale, submesh.VertexScale));
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
                Create(modelIndex, new Vector3D(0, 0, 0), new Vector3D(0, 0, 0), materialIndex);
            }
            public Model(int modelIndex, cTransform transform, int materialIndex = -1)
            {
                Create(modelIndex, new Vector3D(transform.position.X, transform.position.Y, transform.position.Z), new Vector3D(transform.rotation.X, transform.rotation.Y, transform.rotation.Z), materialIndex);
            }

            private void Create(int modelIndex, Vector3D position, Vector3D rotation, int materialIndex)
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
