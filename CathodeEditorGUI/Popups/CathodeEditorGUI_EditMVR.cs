using CATHODE.Commands;
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
using System.Numerics;
using CATHODE.Misc;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_EditMVR : Form
    {
        public CathodeEditorGUI_EditMVR(ShortGuid nodeID = new ShortGuid())
        {
            InitializeComponent();

            listBox1.BeginUpdate();
            for (int i = 0; i < CurrentInstance.moverDB.Movers.Count; i++)
            {
                if (nodeID.val != null && CurrentInstance.moverDB.Movers[i].commandsNodeID != nodeID) continue;
                listBox1.Items.Add(i.ToString());
            }
            listBox1.EndUpdate();

            if (listBox1.Items.Count != 0)
                listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            LoadMVR(Convert.ToInt32(listBox1.SelectedItem.ToString()));
        }

        private void LoadMVR(int mvrIndex)
        {
            MOVER_DESCRIPTOR mvr = CurrentInstance.moverDB.Movers[mvrIndex];

            //Convert model BIN index from REDs to PAK index
            int pakModelIndex = -1;
            for (int y = 0; y < CurrentInstance.modelDB.Models.Count; y++)
            {
                for (int z = 0; z < CurrentInstance.modelDB.Models[y].Submeshes.Count; z++)
                {
                    if (CurrentInstance.modelDB.Models[y].Submeshes[z].binIndex == CurrentInstance.redsDB.RenderableElements[(int)mvr.renderableElementIndex].ModelIndex)
                    {
                        pakModelIndex = y;
                        break;
                    }
                }
                if (pakModelIndex != -1) break;
            }

            //Get all remapped materials from REDs
            List<int> modelMaterialIndexes = new List<int>();
            for (int y = 0; y < mvr.renderableElementCount; y++)
                modelMaterialIndexes.Add(CurrentInstance.redsDB.RenderableElements[(int)mvr.renderableElementIndex + y].MaterialLibraryIndex);
            renderable.PopulateUI(pakModelIndex, modelMaterialIndexes);

            //Load transform from matrix
            Vector3 scale;
            Quaternion rotation;
            Vector3 position;
            Matrix4x4.Decompose(mvr.transform, out scale, out rotation, out position);
            CathodeTransform transformData = new CathodeTransform();
            transformData.position = new CATHODE.Vector3(position.X, position.Y, position.Z);
            transform.PopulateUI(transformData, ShortGuidUtils.Generate("position"));

            visible.Checked = (mvr.visibility == 1);
            //mvr.
        }
    }
}
