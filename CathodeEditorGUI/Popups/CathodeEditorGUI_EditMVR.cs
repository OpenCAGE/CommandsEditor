using CathodeEditorGUI.Popups.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_EditMVR : Form
    {
        private GUI_Resource_RenderableInstance renderable;

        public CathodeEditorGUI_EditMVR()
        {
            InitializeComponent();

            listBox1.BeginUpdate();
            for (int i = 0; i < CurrentInstance.moverDB.Movers.Count; i++)
                listBox1.Items.Add(i.ToString());
            listBox1.EndUpdate();

            renderable = new GUI_Resource_RenderableInstance();
            renderable.Location = new Point(458, 12);
            renderable.Size = new Size(838, 186);
            Controls.Add(renderable);

            LoadMVR(0);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            LoadMVR(listBox1.SelectedIndex);
        }

        private void LoadMVR(int mvrIndex)
        {
            //Convert model BIN index from REDs to PAK index
            int pakModelIndex = -1;
            for (int y = 0; y < CurrentInstance.modelDB.Models.Count; y++)
            {
                for (int z = 0; z < CurrentInstance.modelDB.Models[y].Submeshes.Count; z++)
                {
                    if (CurrentInstance.modelDB.Models[y].Submeshes[z].binIndex == CurrentInstance.redsDB.RenderableElements[(int)CurrentInstance.moverDB.Movers[mvrIndex].REDSIndex].ModelIndex)
                    {
                        pakModelIndex = y;
                        break;
                    }
                }
                if (pakModelIndex != -1) break;
            }

            //Get all remapped materials from REDs
            List<int> modelMaterialIndexes = new List<int>();
            for (int y = 0; y < CurrentInstance.moverDB.Movers[mvrIndex].ModelCount; y++)
                modelMaterialIndexes.Add(CurrentInstance.redsDB.RenderableElements[(int)CurrentInstance.moverDB.Movers[mvrIndex].REDSIndex + y].MaterialLibraryIndex);
            renderable.PopulateUI(pakModelIndex, modelMaterialIndexes);
        }
    }
}
