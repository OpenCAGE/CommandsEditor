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
            CathodeEditorGUI_SelectModel modelthing = new CathodeEditorGUI_SelectModel(SelectedModelIndex);
            modelthing.Show();
            modelthing.FormClosed += Modelthing_FormClosed;
        }
        private void Modelthing_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();

            CathodeEditorGUI_SelectModel modelthing = (CathodeEditorGUI_SelectModel)sender;
            if (modelthing.SelectedIndelIndex == -1 || modelthing.SelectedModelMaterialIndexes.Count == 0) return;
            PopulateUI(modelthing.SelectedIndelIndex, modelthing.SelectedModelMaterialIndexes);
        }

        private void editMaterial_Click(object sender, EventArgs e)
        {

        }
    }
}
