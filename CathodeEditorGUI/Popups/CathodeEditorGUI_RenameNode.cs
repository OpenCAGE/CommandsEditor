using CATHODE;
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
    public partial class CathodeEditorGUI_RenameNode : Form
    {
        public string NodeName { get { return entity_name.Text; } }
        public cGUID NodeID;
        public bool didSave = false;

        public CathodeEditorGUI_RenameNode(cGUID node)
        {
            InitializeComponent();
            entity_name.Text = NodeDBEx.GetEntityName(node);
            NodeID = node;
        }

        private void save_entity_name_Click(object sender, EventArgs e)
        {
            if (entity_name.Text == "") return;
            didSave = true;
            this.Close();
        }
    }
}
