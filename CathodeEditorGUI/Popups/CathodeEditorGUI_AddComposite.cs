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
    public partial class CathodeEditorGUI_AddComposite : Form
    {
        public CathodeEditorGUI_AddComposite()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;
            for (int i = 0; i < CurrentInstance.commandsPAK.Composites.Count; i++)
            {
                if (CurrentInstance.commandsPAK.Composites[i].name == textBox1.Text)
                {
                    MessageBox.Show("Failed to create composite.\nA composite with this name already exists.", "Composite already exists.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            CathodeComposite newFlowgraph = new CathodeComposite();
            newFlowgraph.name = textBox1.Text;
            newFlowgraph.shortGUID = Utilities.GenerateGUID(DateTime.Now.ToString("G"));
            CurrentInstance.commandsPAK.Composites.Add(newFlowgraph);
            this.Close();
        }
    }
}
