using CathodeLib;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Numerics;

namespace CathodeEditorGUI.Popups.UserControls
{
    /// <summary>
    /// Interaction logic for GUI_ModelViewer.xaml
    /// </summary>
    public partial class GUI_ModelViewer : UserControl
    {
        CS2Reader reader = null;

        public GUI_ModelViewer()
        {
            InitializeComponent();
            reader = new CS2Reader();
        }

        public void ShowModel(List<Model> models)
        {
            Model3DGroup group = new Model3DGroup();
            for (int i = 0; i < models.Count; i++)
                group.Children.Add(OffsetModel(models[i].modelIndex, models[i].position, models[i].rotation));
            modelPreview.Content = group;
        }
        
        private Model3DGroup OffsetModel(int modelIndex, Vector3D position, Vector3D rotation)
        {
            float scale = 100.0f;

            Model3DGroup model = reader.Read(modelIndex);
            Transform3DGroup transform = new Transform3DGroup();
            transform.Children.Add(new ScaleTransform3D(scale, scale, scale));
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
