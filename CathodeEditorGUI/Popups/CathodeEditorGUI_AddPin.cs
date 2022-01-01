﻿using System;
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

        public CathodeEditorGUI_AddPin(CathodeEntity entity, CathodeFlowgraph flowgraph)
        {
            _entity = entity;
            InitializeComponent();

            _entityList = flowgraph.GetEntities();
            _entityList = _entityList.OrderBy(o => NodeDBEx.GetEntityName(o.nodeID)).ToList<CathodeEntity>();
            for (int i = 0; i < _entityList.Count; i++)
            {
                string this_node_string = EditorUtils.GenerateNodeName(_entityList[i], flowgraph);
                pin_out_node.Items.Add(this_node_string);
                pin_in_node.Items.Add(this_node_string);

                if (pin_out_node.SelectedIndex == -1 && _entityList[i].nodeID == entity.nodeID) pin_out_node.SelectedIndex = i;
            }
            if (pin_out_node.SelectedIndex == -1) throw new Exception("Failed to fetch entity in flowgraph!");
            pin_out_node.Enabled = false;

            RefreshPinInParams();
            RefreshPinOutParams();
        }

        private void save_pin_Click(object sender, EventArgs e)
        {
            if (pin_out_node.SelectedIndex == -1 || pin_in_node.SelectedIndex == -1 || pin_out_param.Text == "" || pin_in_param.Text == "")
            {
                MessageBox.Show("Please complete all information for the link before saving!", "Incomplete information.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CathodeNodeLink newNodeLink = new CathodeNodeLink();
            newNodeLink.connectionID = Utilities.GenerateGUID(DateTime.Now.ToString("G"));
            newNodeLink.parentParamID = Utilities.GenerateGUID(pin_out_param.Text);
            newNodeLink.childID = _entityList[pin_in_node.SelectedIndex].nodeID;
            newNodeLink.childParamID = Utilities.GenerateGUID(pin_in_param.Text);
            _entity.childLinks.Add(newNodeLink);

            this.Close();
        }

        private void pin_in_node_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPinInParams();
        }
        private void RefreshPinInParams()
        {
            pin_in_param.Items.Clear();
            if (pin_in_node.SelectedIndex == -1) return;
            List<string> items = EditorUtils.GenerateParameterList(_entityList[pin_in_node.SelectedIndex]);
            for (int i = 0; i < items.Count; i++) pin_in_param.Items.Add(items[i]);
        }

        private void pin_out_node_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPinOutParams();
        }
        private void RefreshPinOutParams()
        {
            pin_out_param.Items.Clear();
            if (pin_out_node.SelectedIndex == -1) return;
            List<string> items = EditorUtils.GenerateParameterList(_entityList[pin_out_node.SelectedIndex]);
            for (int i = 0; i < items.Count; i++) pin_out_param.Items.Add(items[i]);
        }
    }
}