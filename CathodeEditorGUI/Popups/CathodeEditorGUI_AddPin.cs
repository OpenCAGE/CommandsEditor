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
        List<CathodeEntity> _entityList = null;

        public CathodeEditorGUI_AddPin(CathodeEntity entity, CathodeComposite flowgraph)
        {
            _entity = entity;
            InitializeComponent();

            _entityList = flowgraph.GetEntities();
            _entityList = _entityList.OrderBy(o => EditorUtils.GenerateEntityName(o, flowgraph).Substring(13)).ToList<CathodeEntity>();

            pin_in_node.BeginUpdate();
            for (int i = 0; i < _entityList.Count; i++)
            {
                string this_node_string = EditorUtils.GenerateEntityName(_entityList[i], flowgraph);
                pin_in_node.Items.Add(this_node_string);
            }
            pin_in_node.EndUpdate();

            pin_out_node.Text = EditorUtils.GenerateEntityName(_entity, flowgraph);
            pin_out_node.Enabled = false;

            RefreshPinInParams();
            RefreshPinOutParams();
        }

        private void save_pin_Click(object sender, EventArgs e)
        {
            if (pin_in_node.SelectedIndex == -1 || pin_out_param.Text == "" || pin_in_param.Text == "")
            {
                MessageBox.Show("Please complete all information for the link before saving!", "Incomplete information.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CathodeEntityLink newNodeLink = new CathodeEntityLink();
            newNodeLink.connectionID = ShortGuidUtils.Generate(DateTime.Now.ToString("G"));
            newNodeLink.parentParamID = ShortGuidUtils.Generate(pin_out_param.Text);
            newNodeLink.childID = _entityList[pin_in_node.SelectedIndex].shortGUID;
            newNodeLink.childParamID = ShortGuidUtils.Generate(pin_in_param.Text);
            _entity.childLinks.Add(newNodeLink);

            this.Close();
        }

        bool didPopulateInFromDB = false;
        private void pin_in_node_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPinInParams();
        }
        private void RefreshPinInParams()
        {
            pin_in_param.BeginUpdate();
            pin_in_param.Items.Clear();
            if (pin_in_node.SelectedIndex == -1) return;
            List<string> items = EditorUtils.GenerateParameterList(_entityList[pin_in_node.SelectedIndex], out didPopulateInFromDB);
            for (int i = 0; i < items.Count; i++) pin_in_param.Items.Add(items[i]);
            pin_in_param.EndUpdate();
        }

        bool didPopulateOutFromDB = false;
        private void pin_out_node_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPinOutParams();
        }
        private void RefreshPinOutParams()
        {
            pin_out_param.BeginUpdate();
            pin_out_param.Items.Clear();
            List<string> items = EditorUtils.GenerateParameterList(_entity, out didPopulateOutFromDB);
            for (int i = 0; i < items.Count; i++) pin_out_param.Items.Add(items[i]);
            pin_out_param.EndUpdate();
        }
    }
}
