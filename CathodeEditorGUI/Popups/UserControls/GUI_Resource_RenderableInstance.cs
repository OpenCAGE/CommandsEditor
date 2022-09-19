using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Commands;
using CathodeLib;
using CATHODE;
using CATHODE.LEGACY;

namespace CathodeEditorGUI.Popups.UserControls
{
    public partial class GUI_Resource_RenderableInstance : ResourceUserControl
    {
        public int SelectedModelIndex;
        public List<int> SelectedMaterialIndexes = new List<int>();

        public GUI_Resource_RenderableInstance()
        {
            InitializeComponent();
        }

        public void PopulateUI(int modelIndex, List<int> materialIndexes)
        {
            SelectedModelIndex = modelIndex;
            SelectedMaterialIndexes = materialIndexes;

            int binIndex = CurrentInstance.modelDB.Models[modelIndex].Submeshes[0].binIndex;
            modelInfoTextbox.Text = CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex];
            if (CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex] != "")
                modelInfoTextbox.Text += " -> [" + CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex] + "]";

            materials.Items.Clear();
            for (int i = 0; i < materialIndexes.Count; i++)
            {
                materials.Items.Add(CurrentInstance.materialDB.MaterialNames[materialIndexes[i]]);
            }
        }

        private void editModel_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_SelectModel selectModel = new CathodeEditorGUI_SelectModel(SelectedModelIndex);
            selectModel.Show();
            selectModel.FormClosed += SelectModel_FormClosed;
        }
        private void SelectModel_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();

            CathodeEditorGUI_SelectModel selectModel = (CathodeEditorGUI_SelectModel)sender;
            if (selectModel.SelectedIndelIndex == -1 || selectModel.SelectedModelMaterialIndexes.Count == 0) return;
            PopulateUI(selectModel.SelectedIndelIndex, selectModel.SelectedModelMaterialIndexes);
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
            PopulateUI(SelectedModelIndex, SelectedMaterialIndexes);
            materials.SelectedIndex = selectMaterial.MaterialIndexToEdit;
        }
    }
}
