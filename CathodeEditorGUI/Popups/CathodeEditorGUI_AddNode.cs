using CATHODE;
using CATHODE.Commands;
using CathodeLib;
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
    public partial class CathodeEditorGUI_AddNode : Form
    {
        public CathodeEntity NewEntity = null;

        CathodeFlowgraph flow = null;
        List<CathodeFlowgraph> availableFlows = null;
        List<CathodeEntityDatabase.EntityDefinition> availableEntities = null;
        public CathodeEditorGUI_AddNode(CathodeFlowgraph _flow, List<CathodeFlowgraph> _flows)
        {
            flow = _flow;
            availableFlows = _flows.OrderBy(o => o.name).ToList();
            availableEntities = CathodeEntityDatabase.GetEntities();
            InitializeComponent();

            //quick hack to reload dropdown
            radioButton1.Checked = true;
            radioButton2.Checked = true;
        }

        //Repopulate UI
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //Datatype
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] {
                                    "POSITION",
                                    "FLOAT",
                                    "STRING",
                                    "SPLINE_DATA",
                                    "ENUM",
                                    "SHORT_GUID",
                                    "FILEPATH",
                                    "BOOL",
                                    "DIRECTION",
                                    "INTEGER"
            });
            comboBox1.EndUpdate();
            comboBox1.SelectedIndex = 0;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //Function
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            for (int i = 0; i < availableEntities.Count; i++) comboBox1.Items.Add(availableEntities[i].className);
            comboBox1.EndUpdate();
            comboBox1.SelectedIndex = 0;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            //Flowgraph
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            for (int i = 0; i < availableFlows.Count; i++) comboBox1.Items.Add(availableFlows[i].name);
            comboBox1.EndUpdate();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cGUID thisID = Utilities.GenerateGUID(DateTime.Now.ToString("G"));

            if (radioButton1.Checked)
            {
                DatatypeEntity newEntity = new DatatypeEntity(thisID);
                newEntity.type = (CathodeDataType)comboBox1.SelectedIndex;
                newEntity.parameter = Utilities.GenerateGUID(textBox1.Text);
                flow.datatypes.Add(newEntity);
                if (NodeDB.GetCathodeName(newEntity.parameter) == newEntity.parameter.ToString())
                    NodeDBEx.AddNewParameterName(newEntity.parameter, textBox1.Text);
                NewEntity = newEntity;
            }
            else if (radioButton2.Checked)
            {
                FunctionEntity newEntity = new FunctionEntity(thisID);
                newEntity.function = CathodeEntityDatabase.GetEntityAtIndex(comboBox1.SelectedIndex).guid;
                //Todo: auto populate params here
                flow.functions.Add(newEntity);
                NodeDBEx.AddNewNodeName(thisID, textBox1.Text);
                NewEntity = newEntity;
            }
            else if (radioButton3.Checked)
            {
                FunctionEntity newEntity = new FunctionEntity(thisID);
                CathodeFlowgraph selectedFlowgraph = availableFlows.FirstOrDefault(o => o.name == comboBox1.Text);
                if (selectedFlowgraph == null)
                {
                    //throw new Exception("Failed to look up flowgraph.");
                    MessageBox.Show("Failed to look up flowgraph!\nPlease report this issue on GitHub.\n\n" + comboBox1.Text, "Could not find flowgraph!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newEntity.function = selectedFlowgraph.nodeID;
                flow.functions.Add(newEntity);
                NodeDBEx.AddNewNodeName(thisID, textBox1.Text);
                NewEntity = newEntity;
            }

            this.Close();
        }
    }
}
