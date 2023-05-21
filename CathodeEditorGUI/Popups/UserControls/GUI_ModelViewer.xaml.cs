using CathodeLib;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Numerics;
using CATHODE;
using AlienPAK;

namespace CommandsEditor.Popups.UserControls
{
    /// <summary>
    /// Interaction logic for GUI_ModelViewer.xaml
    /// </summary>
    public partial class GUI_ModelViewer : UserControl
    {
        protected CommandsEditor _editor;
        protected Editor Editor { get { return _editor.Loaded; } } //hotfix for old Editor. static

        public GUI_ModelViewer(CommandsEditor editor)
        {
            InitializeComponent();
            _editor = editor;
        }

        public void ShowModel(List<Model> models)
        {
            Model3DGroup group = new Model3DGroup();
            for (int i = 0; i < models.Count; i++)
                group.Children.Add(OffsetModel(models[i].modelIndex, models[i].position, models[i].rotation));
            modelPreview.Content = group;
            myView.ZoomExtents();
        }
        
        private Model3DGroup OffsetModel(int modelIndex, Vector3D position, Vector3D rotation)
        {
            Models.CS2.Component.LOD.Submesh submesh = Editor.resource.models.GetAtWriteIndex(modelIndex);
            Model3DGroup model = new Model3DGroup();
            model.Children.Add(submesh.ToGeometryModel3D());
            Transform3DGroup transform = new Transform3DGroup();
            transform.Children.Add(new ScaleTransform3D(submesh.ScaleFactor, submesh.ScaleFactor, submesh.ScaleFactor));
            //transform.Children.Add(new RotateTransform3D(new (rotation.X, rotation.Y, rotation.Z, 0.0f));
            transform.Children.Add(new TranslateTransform3D(position.X, position.Y, position.Z));
            model.Transform = transform;
            return model;
        }

        public class Model
        {
            public Model(int modelIndex)
            {
                this.modelIndex = modelIndex;
                position = new Vector3D(0, 0, 0);
                rotation = new Vector3D(0, 0, 0);
            }
            public Model(int modelIndex, Vector3D position, Vector3D rotation)
            {
                this.modelIndex = modelIndex;
                this.position = position;
                this.rotation = rotation;
            }
            public Model(int modelIndex, Vector3 position, Vector3 rotation)
            {
                this.modelIndex = modelIndex;
                this.position = new Vector3D(position.X, position.Y, position.Z);
                this.rotation = new Vector3D(rotation.X, rotation.Y, rotation.Z);
            }

            public int modelIndex;
            public Vector3D position;
            public Vector3D rotation;
        }
    }
}
