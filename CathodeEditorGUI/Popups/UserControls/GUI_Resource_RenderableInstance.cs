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
    public partial class GUI_Resource_RenderableInstance : UserControl
    {
        int ModelIndex, MaterialIndex;

        public GUI_Resource_RenderableInstance()
        {
            InitializeComponent();
        }

        public void PopulateUI(int modelIndex, int materialIndex)
        {
            modelInfoTextbox.Text = CurrentInstance.modelDB.modelBIN.ModelFilePaths[modelIndex];
            if (CurrentInstance.modelDB.modelBIN.ModelLODPartNames[modelIndex] != "")
                modelInfoTextbox.Text += " -> [" + CurrentInstance.modelDB.modelBIN.ModelLODPartNames[modelIndex] + "]";
            materialInfoTextbox.Text = CurrentInstance.materialDB.MaterialNames[materialIndex];

            //TODO: IS THIS RENDERABLE INSTANCE JUST ALL THE SUBMESHES OF THE ONE PAK MODEL? IF SO, CAN'T WE SHOW IT AS SUCH?

            //We are passed the BIN INDEX here, so look it up in the PAK & get the model we're a submesh for so we can render the whole thing later!
            ModelIndex = -1;
            for (int i = 0; i < CurrentInstance.modelDB.Models.Count; i++)
            {
                for (int x = 0; x < CurrentInstance.modelDB.Models[i].Submeshes.Count; x++)
                {
                    if (CurrentInstance.modelDB.Models[i].Submeshes[x].binIndex == modelIndex)
                    {
                        ModelIndex = i;
                        break;
                    }
                }
                if (ModelIndex != -1) break;
            }
            MaterialIndex = materialIndex;
        }

        private void editModel_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_SelectModel modelthing = new CathodeEditorGUI_SelectModel(ModelIndex);
            modelthing.Show();
        }

        private void editMaterial_Click(object sender, EventArgs e)
        {

        }
    }
}
