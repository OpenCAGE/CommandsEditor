using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Scripting;
using CathodeLib;
using CATHODE;
using CATHODE.LEGACY;
using System.Numerics;

namespace CommandsEditor.Popups.UserControls
{
    public partial class GUI_Resource_RenderableInstance : ResourceUserControl
    {
        public Vector3 Position { get { return new Vector3((float)POS_X.Value, (float)POS_Y.Value, (float)POS_Z.Value); } }
        public Vector3 Rotation { get { return new Vector3((float)ROT_X.Value, (float)ROT_Y.Value, (float)ROT_Z.Value); } }

        public int SelectedModelIndex;
        public List<int> SelectedMaterialIndexes = new List<int>();

        public Action<int, int> OnMaterialSelected; //int = submesh index, int = level material index
        public Action<int> OnModelSelected; //int = model pak index

        public GUI_Resource_RenderableInstance() : base()
        {
            InitializeComponent();
        }

        public void PopulateUI(Vector3 position, Vector3 rotation, int redsIndex, int redsCount)
        {
            POS_X.Value = (decimal)position.X;
            POS_Y.Value = (decimal)position.Y;
            POS_Z.Value = (decimal)position.Z;

            ROT_X.Value = (decimal)rotation.X;
            ROT_Y.Value = (decimal)rotation.Y;
            ROT_Z.Value = (decimal)rotation.Z;

            PopulateUI(redsIndex, redsCount);

            groupBox1.Size = new Size(832, 227);
            this.Size = new Size(838, 232);
        }
        public void PopulateUI(int redsIndex, int redsCount)
        {
            try
            {
                //Get all remapped materials from REDs
                List<int> materialIndexes = new List<int>();
                for (int y = 0; y < redsCount; y++)
                    materialIndexes.Add(Content.resource.reds.Entries[redsIndex + y].MaterialIndex);
                PopulateUI(Content.resource.reds.Entries[redsIndex].ModelIndex, materialIndexes);
            }
            catch 
            {
                PopulateUI(-1, new List<int>());
            }
        }
        public void PopulateUI(int modelIndex, List<int> materialIndexes)
        {
            SelectedModelIndex = modelIndex;
            SelectedMaterialIndexes = materialIndexes;

            if (SelectedModelIndex == -1) SelectedModelIndex = 0;

            Models.CS2.Component.LOD.Submesh submesh = Content.resource.models.GetAtWriteIndex(SelectedModelIndex);
            Models.CS2.Component.LOD lod = Content.resource.models.FindModelLODForSubmesh(submesh); 
            //Models.CS2.Component component = Editor.resource.models.FindModelComponentForSubmesh(submesh);
            Models.CS2 mesh = Content.resource.models.FindModelForSubmesh(submesh);

            modelInfoTextbox.Text = mesh?.Name;
            if (lod.Name != "")
                modelInfoTextbox.Text += " -> [" + lod.Name + "]"; 

            materials.Items.Clear();
            for (int i = 0; i < materialIndexes.Count; i++)
                materials.Items.Add(/*"[" + mesh.Submeshes[i].Name + "] " + */Content.resource.materials.Entries[materialIndexes[i]].Name);

            groupBox1.Size = new Size(832, 180);
            this.Size = new Size(838, 186);
        }

        private void editModel_Click(object sender, EventArgs e)
        {
            SelectModel selectModel = new SelectModel(SelectedModelIndex);
            selectModel.Show();
            selectModel.FormClosed += SelectModel_FormClosed;
        }
        private void SelectModel_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();

            SelectModel selectModel = (SelectModel)sender;
            if (selectModel.SelectedModelIndex == -1 || selectModel.SelectedModelMaterialIndexes.Count == 0) return;
            PopulateUI(selectModel.SelectedModelIndex, selectModel.SelectedModelMaterialIndexes);

            OnModelSelected?.Invoke(selectModel.SelectedModelIndex);
        }

        private void editMaterial_Click(object sender, EventArgs e)
        {
            if (materials.SelectedIndex == -1) return;

            SelectMaterial selectMaterial = new SelectMaterial(materials.SelectedIndex, SelectedMaterialIndexes[materials.SelectedIndex]);
            selectMaterial.Show();
            selectMaterial.FormClosed += SelectMaterial_FormClosed;
        }
        private void SelectMaterial_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();

            SelectMaterial selectMaterial = (SelectMaterial)sender;
            if (selectMaterial.SelectedMaterialIndex == -1) return;
            SelectedMaterialIndexes[selectMaterial.MaterialIndexToEdit] = selectMaterial.SelectedMaterialIndex;
            PopulateUI(SelectedModelIndex, SelectedMaterialIndexes);
            materials.SelectedIndex = selectMaterial.MaterialIndexToEdit;

            OnMaterialSelected?.Invoke(selectMaterial.MaterialIndexToEdit, selectMaterial.SelectedMaterialIndex);
        }
    }
}
