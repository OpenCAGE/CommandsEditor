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
    public partial class CathodeEditorGUI_AddFlowgraph : Form
    {
        public CathodeEditorGUI_AddFlowgraph()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;
            for (int i = 0; i < EditorUtils.Commands.Flowgraphs.Count; i++)
            {
                if (EditorUtils.Commands.Flowgraphs[i].name == textBox1.Text)
                {
                    MessageBox.Show("Failed to create flowgraph.\nA flowgraph with this name already exists.", "Flowgraph already exists.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            CathodeFlowgraph newFlowgraph = new CathodeFlowgraph();
            newFlowgraph.name = textBox1.Text;
            EditorUtils.Commands.Flowgraphs.Add(newFlowgraph);
            this.Close();
        }
    }
}
