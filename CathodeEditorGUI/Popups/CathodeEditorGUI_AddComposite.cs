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
            for (int i = 0; i < Editor.commands.Composites.Count; i++)
            {
                if (Editor.commands.Composites[i].name == textBox1.Text)
                {
                    MessageBox.Show("Failed to create composite.\nA composite with this name already exists.", "Composite already exists.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            
            Composite newFlowgraph = new Composite();
            newFlowgraph.name = textBox1.Text;
            newFlowgraph.shortGUID = ShortGuidUtils.Generate(DateTime.Now.ToString("G"));
            Editor.commands.Composites.Add(newFlowgraph);
            this.Close();
        }
    }
}
