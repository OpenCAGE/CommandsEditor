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
            createDatatypeEntity.Checked = true;
            createFunctionEntity.Checked = true;
        }

        //Repopulate UI
        private void selectedDatatypeEntity(object sender, EventArgs e)
        {
            //Datatype
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            entityVariant.Items.AddRange(new object[] {
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
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
        }
        private void selectedFunctionEntity(object sender, EventArgs e)
        {
            //Function
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < availableEntities.Count; i++) entityVariant.Items.Add(availableEntities[i].className);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
        }
        private void selectedFlowgraphEntity(object sender, EventArgs e)
        {
            //Flowgraph
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < availableFlows.Count; i++) entityVariant.Items.Add(availableFlows[i].name);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cGUID thisID = Utilities.GenerateGUID(DateTime.Now.ToString("G"));

            if (createDatatypeEntity.Checked)
            {
                DatatypeEntity newEntity = new DatatypeEntity(thisID);
                newEntity.type = (CathodeDataType)entityVariant.SelectedIndex;
                newEntity.parameter = Utilities.GenerateGUID(textBox1.Text);
                flow.datatypes.Add(newEntity);
                if (NodeDB.GetCathodeName(newEntity.parameter) == newEntity.parameter.ToString())
                    NodeDBEx.AddNewParameterName(newEntity.parameter, textBox1.Text);
                NewEntity = newEntity;
            }
            else if (createFunctionEntity.Checked)
            {
                FunctionEntity newEntity = new FunctionEntity(thisID);
                //Todo: find a nicer way of instancing functionentity types
                switch (entityVariant.Text)
                {
                    case "CAGEAnimation":
                        newEntity = new CAGEAnimation(thisID);
                        break;
                    case "TriggerSequence":
                        newEntity = new TriggerSequence(thisID);
                        break;
                }
                newEntity.function = CathodeEntityDatabase.GetEntityAtIndex(entityVariant.SelectedIndex).guid;
                //Todo: auto populate params here
                flow.functions.Add(newEntity);
                NodeDBEx.AddNewNodeName(thisID, textBox1.Text);
                NewEntity = newEntity;
            }
            else if (createFlowgraphEntity.Checked)
            {
                FunctionEntity newEntity = new FunctionEntity(thisID);
                CathodeFlowgraph selectedFlowgraph = availableFlows.FirstOrDefault(o => o.name == entityVariant.Text);
                if (selectedFlowgraph == null)
                {
                    //throw new Exception("Failed to look up flowgraph.");
                    MessageBox.Show("Failed to look up flowgraph!\nPlease report this issue on GitHub.\n\n" + entityVariant.Text, "Could not find flowgraph!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
