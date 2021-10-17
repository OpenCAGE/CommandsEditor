using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Commands;
using CATHODE.Misc;
using CATHODE.Models;
using CATHODE.Textures;
using CathodeEditorGUI.UserControls;
using CathodeLib;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI : Form
    {
        private CommandsPAK commandsPAK = null;
        private TreeUtility treeHelper;
        private bool currentlyShowingChildLinks = true;

        public CathodeEditorGUI()
        {
            InitializeComponent();
            treeHelper = new TreeUtility(FileTree);

            //Populate available maps
            List<string> all_map_dirs = MapDirectories.GetAvailable();
            env_list.Items.Clear();
            foreach (string map in all_map_dirs) env_list.Items.Add(map);
            env_list.SelectedIndex = 0;
        }

        /* Clear the UI */
        private void ClearUI(bool clear_flowgraph_list, bool clear_node_list, bool clear_parameter_list)
        {
            if (clear_flowgraph_list)
            {
                FileTree.Nodes.Clear();
                first_executed_flowgraph.Text = "Entry point: ";
                flowgraph_count.Text = "Flowgraph count: ";
            }
            if (clear_node_list)
            {
                node_search_box.Text = "";
                groupBox1.Text = "Selected Flowgraph Content";
                flowgraph_content.Items.Clear();
                flowgraph_content_RAW.Clear();
                selected_flowgraph = null;
            }
            if (clear_parameter_list)
            {
                selected_node = null;
                selected_node_id.Text = "";
                selected_node_type.Text = "";
                selected_node_type_description.Text = "";
                selected_node_name.Text = "";
                NodeParams.Controls.Clear();
                node_children.Items.Clear();
                currentlyShowingChildLinks = true;
                node_to_flowgraph_jump.Visible = false;
            }
        }

        /* Load a COMMANDS.PAK into the editor */
        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            //Reset UI
            ClearUI(true, true, true);
            commandsPAK = null;

            //Load
            string path_to_ENV = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + env_list.SelectedItem;
            commandsPAK = new CommandsPAK(path_to_ENV + "/WORLD/COMMANDS.PAK");

            //Sanity check
            if (!commandsPAK.Loaded)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\nPlease place this executable in your Alien: Isolation folder.", "Environment error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (commandsPAK.EntryPoints[0] == null)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\nPlease reset your game files.", "COMMANDS.PAK corrupted!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Populate file tree
            treeHelper.UpdateFileTree(commandsPAK.GetFlowgraphNames().ToList());

            //Show info in UI
            first_executed_flowgraph.Text = "Entry point: " + commandsPAK.EntryPoints[0].name;
            flowgraph_count.Text = "Flowgraph count: " + commandsPAK.Flowgraphs.Count;
        }

        /* Save the current edits */
        private void save_commands_pak_Click(object sender, EventArgs e)
        {
            if (commandsPAK == null) return;
            commandsPAK.Save();
            MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /* Load nodes for selected script */
        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (FileTree.SelectedNode == null) return;
            if (((TreeItem)FileTree.SelectedNode.Tag).Item_Type != TreeItemType.EXPORTABLE_FILE) return;
            LoadFlowgraph(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
        }

        /* If selected node is a flowgraph instance, allow jump to it */
        private void node_to_flowgraph_jump_Click(object sender, EventArgs e)
        {
            LoadFlowgraph(selected_node_type_description.Text);
        }

        /* Add new flowgraph */
        private void addNewFlowgraph_Click(object sender, EventArgs e)
        {
            MessageBox.Show("wip");
        }

        /* Remove selected flowgraph */
        private void removeSelectedFlowgraph_Click(object sender, EventArgs e)
        {
            MessageBox.Show("wip");
        }

        /* Select node from loaded flowgraph */
        private void flowgraph_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flowgraph_content.SelectedIndex == -1 || selected_flowgraph == null) return;
            CathodeEntity thisNodeInfo = selected_flowgraph.GetEntityByID(new cGUID(flowgraph_content.SelectedItem.ToString().Substring(0, 11)));
            if (thisNodeInfo != null) LoadNode(thisNodeInfo);
        }

        /* Add new out pin */
        private void addNewLink_Click(object sender, EventArgs e)
        {
            if (selected_flowgraph == null || selected_node == null) return;
            CathodeEditorGUI_AddPin add_pin = new CathodeEditorGUI_AddPin(selected_node, selected_flowgraph);
            add_pin.Show();
            add_pin.FormClosed += new FormClosedEventHandler(add_pin_closed);
        }
        private void add_pin_closed(Object sender, FormClosedEventArgs e)
        {
            RefreshNodeLinks();
        }

        /* Remove selected out pin */
        private void removeSelectedLink_Click(object sender, EventArgs e)
        {
            selected_node.childLinks.RemoveAt(node_children.SelectedIndex);
            RefreshNodeLinks();
        }

        /* Go to selected pin out on button press */
        private void out_pin_goto_Click(object sender, EventArgs e)
        {
            if (node_children.SelectedIndex == -1 || selected_flowgraph == null) return;
            CathodeEntity thisNodeInfo = selected_flowgraph.GetEntityByID(selected_node.childLinks[node_children.SelectedIndex].childID);
            if (thisNodeInfo != null) LoadNode(thisNodeInfo);
        }

        /* Flip the child link list to contain parents (this is an expensive search, which is why we only do it on request) */
        private void showLinkParents_Click(object sender, EventArgs e)
        {
            currentlyShowingChildLinks = !currentlyShowingChildLinks;
            RefreshNodeLinks();
        }

        /* Search node list */
        private void node_search_btn_Click(object sender, EventArgs e)
        {
            List<string> matched = new List<string>();
            foreach (string item in flowgraph_content_RAW) if (item.ToUpper().Contains(node_search_box.Text.ToUpper())) matched.Add(item);
            flowgraph_content.Items.Clear();
            for (int i = 0; i < matched.Count; i++) flowgraph_content.Items.Add(matched[i]);
        }

        /* Load a flowgraph into the UI */
        CathodeFlowgraph selected_flowgraph = null;
        List<string> flowgraph_content_RAW = new List<string>();
        private void LoadFlowgraph(string FileName)
        {
            ClearUI(false, true, true);
            CathodeFlowgraph entry = commandsPAK.Flowgraphs[commandsPAK.GetFileIndex(FileName)];
            selected_flowgraph = entry;
            Cursor.Current = Cursors.WaitCursor;

            List<CathodeEntity> entities = entry.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                string desc = GenerateNodeName(entities[i]);
                flowgraph_content.Items.Add(desc);
                flowgraph_content_RAW.Add(desc);
            }
            groupBox1.Text = entry.name;
            Cursor.Current = Cursors.Default;
        }

        /* Add new entity */
        private void addNewNode_Click(object sender, EventArgs e)
        {
            if (selected_flowgraph == null) return;
            CathodeEditorGUI_AddNode add_parameter = new CathodeEditorGUI_AddNode(selected_flowgraph, commandsPAK.Flowgraphs);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(add_node_closed);
        }
        private void add_node_closed(Object sender, FormClosedEventArgs e)
        {
            LoadFlowgraph(selected_flowgraph.name);
            LoadNode(selected_node); //TODO: load returned new node
        }

        /* Remove selected entity */
        private void removeSelectedNode_Click(object sender, EventArgs e)
        {
            if (selected_node == null) return;

            switch (selected_node.variant)
            {
                case EntityVariant.DATATYPE:
                    selected_flowgraph.datatypes.Remove((DatatypeEntity)selected_node);
                    break;
                case EntityVariant.FUNCTION:
                    selected_flowgraph.functions.Remove((FunctionEntity)selected_node);
                    break;
                case EntityVariant.OVERRIDE:
                    selected_flowgraph.overrides.Remove((OverrideEntity)selected_node);
                    break;
                case EntityVariant.PROXY:
                    selected_flowgraph.proxies.Remove((ProxyEntity)selected_node);
                    break;
                case EntityVariant.NOT_SETUP:
                    selected_flowgraph.unknowns.Remove(selected_node);
                    break;
            }

            List<CathodeEntity> entities = selected_flowgraph.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                List<CathodeNodeLink> nodeLinks = new List<CathodeNodeLink>();
                for (int x = 0; x < entities[i].childLinks.Count; x++)
                {
                    if (entities[i].childLinks[x].childID != selected_node.nodeID) nodeLinks.Add(entities[i].childLinks[x]);
                }
                entities[i].childLinks = nodeLinks;
            }

            LoadFlowgraph(selected_flowgraph.name);
        }

        /* Utility: generate nice entity name to display in UI */
        private string GenerateNodeName(CathodeEntity entity)
        {
            string desc = "";
            switch (entity.variant)
            {
                case EntityVariant.DATATYPE:
                    desc = NodeDB.GetCathodeName(((DatatypeEntity)entity).parameter) + " (DataType " + ((DatatypeEntity)entity).type.ToString() + ")";
                    break;
                case EntityVariant.FUNCTION:
                    desc = NodeDB.GetEditorName(entity.nodeID) + " (" + NodeDB.GetCathodeName(((FunctionEntity)entity).function, commandsPAK) + ")";
                    break;
                case EntityVariant.OVERRIDE:
                    desc = "OVERRIDE!"; //TODO
                    break;
                case EntityVariant.PROXY:
                    desc = "PROXY!"; //TODO
                    break;
                case EntityVariant.NOT_SETUP:
                    desc = "NOT SETUP!"; //Huh?
                    break;
            }
            return entity.nodeID.ToString() + " " + desc;
        }

        /* Load a entity into the UI */
        CathodeEntity selected_node = null;
        private void LoadNode(CathodeEntity edit_node)
        {
            if (edit_node == null) return;

            ClearUI(false, false, true);
            selected_node = edit_node;
            Cursor.Current = Cursors.WaitCursor;

            //populate info labels
            selected_node_id.Text = edit_node.nodeID.ToString();
            selected_node_type.Text = edit_node.variant.ToString();
            string nodetypedesc = "";
            switch (edit_node.variant)
            {
                case EntityVariant.FUNCTION:
                    nodetypedesc = NodeDB.GetCathodeName(((FunctionEntity)edit_node).function, commandsPAK);
                    node_to_flowgraph_jump.Visible = (commandsPAK.GetFlowgraph(((FunctionEntity)edit_node).function) != null);
                    break;
                case EntityVariant.DATATYPE:
                    nodetypedesc = "DataType " + ((DatatypeEntity)edit_node).type.ToString();
                    break;
            }
            selected_node_type_description.Text = nodetypedesc;
            selected_node_name.Text = NodeDB.GetEditorName(edit_node.nodeID);

            //populate parameter inputs
            int current_ui_offset = 7;
            for (int i = 0; i < edit_node.parameters.Count; i++)
            {
                CathodeParameter this_param = edit_node.parameters[i].content;
                UserControl parameterGUI = null;

                switch (this_param.dataType)
                {
                    case CathodeDataType.POSITION:
                        parameterGUI = new GUI_TransformDataType();
                        ((GUI_TransformDataType)parameterGUI).PopulateUI((CathodeTransform)this_param, edit_node.parameters[i].paramID);
                        break;
                    case CathodeDataType.INTEGER:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Int((CathodeInteger)this_param, edit_node.parameters[i].paramID);
                        break;
                    case CathodeDataType.STRING:
                        parameterGUI = new GUI_StringDataType();
                        ((GUI_StringDataType)parameterGUI).PopulateUI((CathodeString)this_param, edit_node.parameters[i].paramID);
                        break;
                    case CathodeDataType.BOOL:
                        parameterGUI = new GUI_BoolDataType();
                        ((GUI_BoolDataType)parameterGUI).PopulateUI((CathodeBool)this_param, edit_node.parameters[i].paramID);
                        break;
                    case CathodeDataType.FLOAT:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Float((CathodeFloat)this_param, edit_node.parameters[i].paramID);
                        break;
                    case CathodeDataType.DIRECTION:
                        parameterGUI = new GUI_VectorDataType();
                        ((GUI_VectorDataType)parameterGUI).PopulateUI((CathodeVector3)this_param, edit_node.parameters[i].paramID);
                        break;
                    case CathodeDataType.ENUM:
                        parameterGUI = new GUI_EnumDataType();
                        ((GUI_EnumDataType)parameterGUI).PopulateUI((CathodeEnum)this_param, edit_node.parameters[i].paramID);
                        break;
                    case CathodeDataType.SHORT_GUID:
                        parameterGUI = new GUI_HexDataType();
                        ((GUI_HexDataType)parameterGUI).PopulateUI((CathodeResource)this_param, edit_node.parameters[i].paramID); 
                        break;
                    case CathodeDataType.SPLINE_DATA:
                        parameterGUI = new GUI_SplineDataType();
                        ((GUI_SplineDataType)parameterGUI).PopulateUI((CathodeSpline)this_param, edit_node.parameters[i].paramID);
                        break;
                }

                parameterGUI.Location = new Point(15, current_ui_offset);
                current_ui_offset += parameterGUI.Height + 6;
                NodeParams.Controls.Add(parameterGUI);
            }

            RefreshNodeLinks();

            Cursor.Current = Cursors.Default;
        }

        /* Refresh child/parent node links for selected node */
        private void RefreshNodeLinks()
        {
            node_children.Items.Clear();
            addNewLink.Enabled = currentlyShowingChildLinks;
            removeSelectedLink.Enabled = currentlyShowingChildLinks;
            out_pin_goto.Enabled = currentlyShowingChildLinks;
            showLinkParents.Text = (currentlyShowingChildLinks) ? "Parents" : "Children";

            if (selected_flowgraph == null || selected_node == null) return;
            if (currentlyShowingChildLinks)
            {
                //Child links (pins out of this node)
                foreach (CathodeNodeLink link in selected_node.childLinks)
                {
                    node_children.Items.Add(
                        /*"[" + link.connectionID.ToString() + "] " +*/
                        "(" + NodeDB.GetCathodeName(link.parentParamID) + ") => " +
                        NodeDB.GetEditorName(link.childID) + 
                        " (" + NodeDB.GetCathodeName(link.childParamID) + ")");
                }
            }
            else
            {
                //Parent links (pins into this node)
                List<CathodeEntity> ents = selected_flowgraph.GetEntities();
                foreach (CathodeEntity entity in ents)
                {
                    foreach (CathodeNodeLink link in entity.childLinks)
                    {
                        if (link.childID != selected_node.nodeID) continue;
                        node_children.Items.Add(
                            /*"[" + link.connectionID.ToString() + "] " +*/
                            NodeDB.GetEditorName(entity.nodeID) +
                            " (" + NodeDB.GetCathodeName(link.parentParamID) + ") => " +
                            "(" + NodeDB.GetCathodeName(link.childParamID) + ")");
                    }
                }
            }
        }

        /* Add a new parameter */
        private void addNewParameter_Click(object sender, EventArgs e)
        {
            if (selected_node == null) return;
            CathodeEditorGUI_AddParameter add_parameter = new CathodeEditorGUI_AddParameter(selected_node);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(param_add_closed);
        }
        private void param_add_closed(Object sender, FormClosedEventArgs e)
        {
            LoadNode(selected_node);
        }

        /* Remove a parameter */
        private void removeParameter_Click(object sender, EventArgs e)
        {
            MessageBox.Show("wip");
        }
    }
}
