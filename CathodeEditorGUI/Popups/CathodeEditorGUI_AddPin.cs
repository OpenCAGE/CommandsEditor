using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Commands;
using CathodeLib;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddPin : Form
    {
        CathodeEntity _entity = null;

        public CathodeEditorGUI_AddPin(CathodeEntity entity, CathodeFlowgraph flowgraph)
        {
            _entity = entity;
            InitializeComponent();

            List<CathodeEntity> ents = flowgraph.GetEntities();
            ents.Sort();
            for (int i = 0; i < ents.Count; i++)
            {
                string this_node_string = "[" + ents[i].nodeID.ToString() + "] " + NodeDB.GetEditorName(ents[i].nodeID);
                pin_out_node.Items.Add(this_node_string);
                if (pin_out_node.SelectedIndex == -1 && ents[i].nodeID == entity.nodeID)
                {
                    pin_out_node.SelectedIndex = i;
                    continue; //We don't want to be able to connect back into ourself
                }
                pin_in_node.Items.Add(this_node_string);
            }
            if (pin_out_node.SelectedIndex == -1) throw new Exception("Failed to fetch entity in flowgraph!");
            pin_out_node.Enabled = false;
        }

        private void save_pin_Click(object sender, EventArgs e)
        {
            if (pin_out_node.SelectedIndex == -1 || pin_in_node.SelectedIndex == -1 || parameter_out.Text == "" || parameter_in.Text == "")
            {
                MessageBox.Show("Please complete all information for the link before saving!", "Incomplete information.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CathodeNodeLink newNodeLink = new CathodeNodeLink();
            newNodeLink.connectionID = Utilities.GenerateGUID(DateTime.Now.ToString("G"));
            newNodeLink.parentParamID = Utilities.GenerateGUID(parameter_out.Text);
            newNodeLink.childID = new cGUID(pin_in_node.SelectedItem.ToString().Substring(1, 11)); //relies on the above formatting staying true
            newNodeLink.childParamID = Utilities.GenerateGUID(parameter_in.Text);
            _entity.childLinks.Add(newNodeLink);

            this.Close();
        }
    }
}
