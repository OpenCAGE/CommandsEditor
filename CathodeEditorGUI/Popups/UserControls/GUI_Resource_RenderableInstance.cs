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

namespace CathodeEditorGUI.Popups.UserControls
{
    public partial class GUI_Resource_RenderableInstance : ResourceUserControl
    {
        public Models.CS2 SelectedModel;
        public List<int> SelectedMaterialIndexes = new List<int>();

        public Action<int, int> OnMaterialSelected; //int = submesh index, int = level material index
        public Action<int> OnModelSelected; //int = model pak index

        public GUI_Resource_RenderableInstance()
        {
            InitializeComponent();
        }

        public void PopulateUI(int index, List<int> materialIndexes)
        {
            //TODO: does RENDERABLE_INSTANCE utilise position/rotation?

            SelectedModel = Editor.resource.models.FindModelForSubmesh(Editor.resource.models.GetAtWriteIndex(index));
            SelectedMaterialIndexes = materialIndexes;

            modelInfoTextbox.Text = SelectedModel.Name;
            if (SelectedModel.Submeshes[0]?.Name != "")
                modelInfoTextbox.Text += " -> [" + SelectedModel.Submeshes[0].Name + "]"; //TODO: we can have varying submesh names!

            materials.Items.Clear();
            for (int i = 0; i < materialIndexes.Count; i++)
            {
                materials.Items.Add(Editor.resource.materials.Entries[materialIndexes[i]].Name);
            }
        }

        private void editModel_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_SelectModel selectModel = new CathodeEditorGUI_SelectModel(Editor.resource.models.Entries.IndexOf(SelectedModel));
            selectModel.Show();
            selectModel.FormClosed += SelectModel_FormClosed;
        }
        private void SelectModel_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();

            CathodeEditorGUI_SelectModel selectModel = (CathodeEditorGUI_SelectModel)sender;
            if (selectModel.SelectedModelIndex == -1 || selectModel.SelectedModelMaterialIndexes.Count == 0) return;
            PopulateUI(selectModel.SelectedModelIndex, selectModel.SelectedModelMaterialIndexes);

            OnModelSelected?.Invoke(selectModel.SelectedModelIndex);
        }

        private void editMaterial_Click(object sender, EventArgs e)
        {
            if (materials.SelectedIndex == -1) return;

            CathodeEditorGUI_SelectMaterial selectMaterial = new CathodeEditorGUI_SelectMaterial(materials.SelectedIndex, SelectedMaterialIndexes[materials.SelectedIndex]);
            selectMaterial.Show();
            selectMaterial.FormClosed += SelectMaterial_FormClosed;
        }
        private void SelectMaterial_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();

            CathodeEditorGUI_SelectMaterial selectMaterial = (CathodeEditorGUI_SelectMaterial)sender;
            if (selectMaterial.SelectedMaterialIndex == -1) return;
            SelectedMaterialIndexes[selectMaterial.MaterialIndexToEdit] = selectMaterial.SelectedMaterialIndex;
            PopulateUI(Editor.resource.models.GetWriteIndex(SelectedModel), SelectedMaterialIndexes);
            materials.SelectedIndex = selectMaterial.MaterialIndexToEdit;

            OnMaterialSelected?.Invoke(selectMaterial.MaterialIndexToEdit, selectMaterial.SelectedMaterialIndex);
        }
    }
}
