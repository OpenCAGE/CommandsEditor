using CATHODE.Commands;
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
    public partial class CathodeEditorGUI_EditEntryPoint : Form
    {
        public CathodeEditorGUI_EditEntryPoint()
        {
            InitializeComponent();
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            comboBox1.SelectedIndex = -1;
            List<CathodeFlowgraph> flows = CurrentInstance.commandsPAK.Flowgraphs;
            for (int i = 0; i < flows.Count; i++)
            {
                comboBox1.Items.Add(flows[i].name);
                if (comboBox1.SelectedIndex == -1 && 
                    flows[i].nodeID == CurrentInstance.commandsPAK.EntryPoints[0].nodeID)
                {
                    comboBox1.SelectedIndex = i;
                }
            }
            comboBox1.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            CurrentInstance.commandsPAK.SetEntryPoint(CurrentInstance.commandsPAK.Flowgraphs[comboBox1.SelectedIndex].nodeID);
            this.Close();
        }
    }
}
