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
using CathodeLib;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI : Form
    {
        private CommandsPAK commandsPAK = null;
        //private RenderableElementsBIN redsBIN = null;
        //private ModelPAK modelPAK = null;
        //private TexturePAK texturePAK = null;

        private Dictionary<CathodeDataType, GroupBox> precachedParameterGUI = new Dictionary<CathodeDataType, GroupBox>();
        private TreeUtility treeHelper;

        public CathodeEditorGUI()
        {
            InitializeComponent();
            treeHelper = new TreeUtility(FileTree);

            //New parameter UI
            precachedParameterGUI.Add(CathodeDataType.POSITION, POSITION_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.STRING, STRING_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.FILEPATH, STRING_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.DIRECTION, VECTOR_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.ENUM, ENUM_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.BOOL, BOOL_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.SHORT_GUID, GUID_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.INTEGER, NUMERIC_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.FLOAT, NUMERIC_VARIABLE_DUMMY);
            precachedParameterGUI.Add(CathodeDataType.SPLINE_DATA, UNIMPLEMENTED_VARIABLE_TYPE);

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
                node_to_flowgraph_jump.Visible = false;
            }
        }

        /* Load a COMMANDS.PAK into the editor */
        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            //Reset all UI here
            ClearUI(true, true, true);
            commandsPAK = null;

            //for (int i = 0; i < env_list.Items.Count; i++) commandsPAK = new CommandsPAK(Folders.GetPath(ToolPaths.Paths.FOLDER_ALIEN_ISOLATION) + "/DATA/ENV/PRODUCTION/" + env_list.Items[i].ToString() + "/WORLD/COMMANDS.PAK");
            string path_to_ENV = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + env_list.SelectedItem;

            //Sanity check
            if (!File.Exists(path_to_ENV + "/WORLD/COMMANDS.PAK"))
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\nPlease place this executable in your Alien: Isolation folder.", "Environment error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Load COMMANDS.PAK and populate file tree
            commandsPAK = new CommandsPAK(path_to_ENV + "/WORLD/COMMANDS.PAK");
            treeHelper.UpdateFileTree(commandsPAK.GetFlowgraphNames().ToList());

            //Load REDS.BIN, LEVEL_MODELS.PAK, and LEVEL_TEXTURES.ALL.PAK for resource assignment
            //redsBIN = new RenderableElementsBIN(path_to_ENV + "/WORLD/REDS.BIN");
            //modelPAK = new ModelPAK(path_to_ENV + "/RENDERABLE/LEVEL_MODELS.PAK"); modelPAK.Load();
            //texturePAK = new TexturePAK(path_to_ENV + "/RENDERABLE/LEVEL_TEXTURES.ALL.PAK"); texturePAK.Load();

            //Show info in UI
            first_executed_flowgraph.Text = "Entry point: " + commandsPAK.EntryPoints[0].name;
            flowgraph_count.Text = "Flowgraph count: " + commandsPAK.Flowgraphs.Count;
        }

        /* Save the current edits */
        private void save_commands_pak_Click(object sender, EventArgs e)
        {
            if (commandsPAK == null) return;
            commandsPAK.Save();
            //redsBIN.Save();
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

        /* Select node from loaded flowgraph */
        private void flowgraph_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flowgraph_content.SelectedIndex == -1 || selected_flowgraph == null) return;
            CathodeEntity thisNodeInfo = selected_flowgraph.GetEntityByID(new cGUID(flowgraph_content.SelectedItem.ToString().Substring(0, 11)));
            if (thisNodeInfo != null) LoadNode(thisNodeInfo);
        }

        /* Go to parent link when selected */
        private void node_parents_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (node_parents.SelectedIndex == -1 || selected_flowgraph == null) return;
            CathodeEntity thisNodeInfo = selected_flowgraph.GetEntityByID(selected_flowgraph.GetParentLinksByID(selected_node.nodeID)[node_parents.SelectedIndex].parentID);
            if (thisNodeInfo != null) LoadNode(thisNodeInfo);
            */
        }

        /* Go to selected pin out on button press */
        private void out_pin_goto_Click(object sender, EventArgs e)
        {
            /*
            if (node_children.SelectedIndex == -1 || selected_flowgraph == null) return;
            CathodeEntity thisNodeInfo = selected_flowgraph.GetEntityByID(selected_flowgraph.GetChildLinksByID(selected_node.nodeID)[node_children.SelectedIndex].childID);
            if (thisNodeInfo != null) LoadNode(thisNodeInfo);
            */
        }

        /* Edit selected pin out on button press */
        private void out_pin_edit_Click(object sender, EventArgs e)
        {
            /*
            if (node_children.SelectedIndex == -1 || selected_flowgraph == null) return;
            CathodeEditorGUI_EditPin pin_editor = new CathodeEditorGUI_EditPin(selected_flowgraph.GetChildLinksByID(selected_node.nodeID)[node_children.SelectedIndex], selected_flowgraph);
            pin_editor.Show();
            pin_editor.FormClosed += new FormClosedEventHandler(pin_editor_closed);
        }
        private void pin_editor_closed(Object sender, FormClosedEventArgs e)
        {
            RefreshNodeLinks();
            */
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
                string desc = "";
                switch (entities[i].variant)
                {
                    case EntityVariant.DATATYPE:
                        desc = NodeDB.GetName(entities[i].nodeID) + " (DataType " + ((DatatypeEntity)entities[i]).type.ToString() + ")";
                        break;
                    case EntityVariant.FUNCTION:
                        desc = NodeDB.GetFriendlyName(entities[i].nodeID) + " (" + NodeDB.GetNodeTypeName(((FunctionEntity)entities[i]).function, commandsPAK) + ")";
                        break;
                    case EntityVariant.OVERRIDE:
                        desc = "OVERRIDE!"; //TODO
                        break;
                    case EntityVariant.PROXY:
                        desc = "PROXY!"; //TODO
                        break;
                }

                string thisentrytext = entities[i].nodeID.ToString() + " " + desc;
                flowgraph_content.Items.Add(thisentrytext);
                flowgraph_content_RAW.Add(thisentrytext);
            }
            groupBox1.Text = "Selected Flowgraph Content - (" + entry.nodeID.ToString() + " - " + entry.name + ")";
            Cursor.Current = Cursors.Default;
        }

        /* Load a node into the UI */
        CathodeEntity selected_node = null;
        private void LoadNode(CathodeEntity edit_node)
        {
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
                    nodetypedesc = NodeDB.GetNodeTypeName(((FunctionEntity)edit_node).function, commandsPAK);
                    node_to_flowgraph_jump.Visible = (commandsPAK.GetFlowgraph(((FunctionEntity)edit_node).function) != null);
                    break;
                case EntityVariant.DATATYPE:
                    nodetypedesc = "DataType " + ((DatatypeEntity)edit_node).type.ToString();
                    break;
            }
            selected_node_type_description.Text = nodetypedesc;
            selected_node_name.Text = NodeDB.GetFriendlyName(edit_node.nodeID);

            //populate parameter inputs
            int current_ui_offset = 11;
            for (int i = 0; i < edit_node.parameters.Count; i++)
            {
                CathodeParameter this_param = edit_node.parameters[i].content;
                GroupBox parameterGUI = precachedParameterGUI[this_param.dataType];

                switch (this_param.dataType)
                {
                    case CathodeDataType.POSITION:
                        CathodeTransform cTrans = (CathodeTransform)this_param;
                        ((NumericUpDown)parameterGUI.Controls[13]).Value = (decimal)cTrans.position.X;
                        ((NumericUpDown)parameterGUI.Controls[11]).Value = (decimal)cTrans.position.Y;
                        ((NumericUpDown)parameterGUI.Controls[9]).Value = (decimal)cTrans.position.Z;
                        ((NumericUpDown)parameterGUI.Controls[6]).Value = (decimal)cTrans.rotation.X;
                        ((NumericUpDown)parameterGUI.Controls[4]).Value = (decimal)cTrans.rotation.Y;
                        ((NumericUpDown)parameterGUI.Controls[2]).Value = (decimal)cTrans.rotation.Z;
                        break;
                    case CathodeDataType.INTEGER:
                        CathodeInteger cInt = (CathodeInteger)this_param;
                        ((NumericUpDown)parameterGUI.Controls[0]).Value = (decimal)cInt.value;
                        break;
                    case CathodeDataType.STRING:
                        CathodeString cString = (CathodeString)this_param;
                        ((TextBox)parameterGUI.Controls[0]).Text = cString.value;
                        break;
                    case CathodeDataType.BOOL:
                        CathodeBool cBool = (CathodeBool)this_param;
                        ((CheckBox)parameterGUI.Controls[0]).Checked = cBool.value;
                        break;
                    case CathodeDataType.FLOAT:
                        CathodeFloat cFloat = (CathodeFloat)this_param;
                        ((NumericUpDown)parameterGUI.Controls[0]).Value = (decimal)cFloat.value;
                        //((NumericUpDown)parameterGUI.Controls[0]).   - Set float type not int
                        break;
                    case CathodeDataType.DIRECTION:
                        CathodeVector3 cVec3 = (CathodeVector3)this_param;
                        ((NumericUpDown)parameterGUI.Controls[6]).Value = (decimal)cVec3.value.X;
                        ((NumericUpDown)parameterGUI.Controls[4]).Value = (decimal)cVec3.value.Y;
                        ((NumericUpDown)parameterGUI.Controls[2]).Value = (decimal)cVec3.value.Z;
                        break;
                    case CathodeDataType.ENUM:
                        CathodeEnum cEnum = (CathodeEnum)this_param;
                        ((ComboBox)parameterGUI.Controls[1]).SelectedText = NodeDB.GetEnum(cEnum.enumID).Name;
                        ((NumericUpDown)parameterGUI.Controls[0]).Value = cEnum.enumIndex;
                        break;
                    case CathodeDataType.SHORT_GUID:
                        CathodeResource cResource = (CathodeResource)this_param;
                        ((TextBox)parameterGUI.Controls[0]).Text = BitConverter.ToString(new byte[] { cResource.resourceID.val[0] });
                        ((TextBox)parameterGUI.Controls[1]).Text = BitConverter.ToString(new byte[] { cResource.resourceID.val[1] });
                        ((TextBox)parameterGUI.Controls[2]).Text = BitConverter.ToString(new byte[] { cResource.resourceID.val[2] });
                        ((TextBox)parameterGUI.Controls[3]).Text = BitConverter.ToString(new byte[] { cResource.resourceID.val[3] });
                        break;
                    default:

                        break;
                }

                parameterGUI.Location = new Point(19, current_ui_offset);
                current_ui_offset += parameterGUI.Height + 6;
                parameterGUI.Text = NodeDB.GetName(edit_node.parameters[i].paramID) + " (" + edit_node.parameters[i].paramID.ToString() + ")";
                NodeParams.Controls.Add(parameterGUI);
            }

            RefreshNodeLinks();

            Cursor.Current = Cursors.Default;
        }

        /* Refresh child/parent node links for selected node */
        private void RefreshNodeLinks()
        {
            //Child links (pins out of this node)
            node_children.Items.Clear();
            foreach (CathodeNodeLink id in selected_node.childLinks)
            {
                string desc = "";
                switch (selected_node.variant)
                {
                    case EntityVariant.DATATYPE:
                        desc = NodeDB.GetName(selected_node.nodeID) + " (DataType " + ((DatatypeEntity)selected_node).type.ToString() + ")";
                        break;
                    case EntityVariant.FUNCTION:
                        desc = NodeDB.GetFriendlyName(selected_node.nodeID) + " (" + NodeDB.GetNodeTypeName(((FunctionEntity)selected_node).function, commandsPAK) + ")";
                        break;
                    case EntityVariant.OVERRIDE:
                        desc = "OVERRIDE!"; //TODO
                        break;
                    case EntityVariant.PROXY:
                        desc = "PROXY!"; //TODO
                        break;
                }
                node_children.Items.Add("[" + id.connectionID.ToString() + "] Pin out " + id.parentParamID.ToString() + " (" + NodeDB.GetName(id.parentParamID) + "), goes to " + id.childParamID.ToString() + " (" + NodeDB.GetName(id.childParamID) + ") on node " + id.childID.ToString() + " (" + NodeDB.GetFriendlyName(id.childID) + desc + ")");
            }
        }

        /* User selected a new parameter to use, update it in CommandsPAK */
        private void param_selector_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DEPRECATED
        }

        /* User selected parameter to edit, show edit UI & refresh when closed */
        CathodeResourceReference selected_reds_ref = null;
        private void param_edit_btn_Click(object sender, EventArgs e)
        {
            /*
            if (commandsPAK.GetParameter(Convert.ToInt32(((Button)sender).Name)).dataType == CathodeDataType.SHORT_GUID)
            {
                List<RenderableElement> redsList = new List<RenderableElement>();
                CathodeResource cResource = (CathodeResource)commandsPAK.GetParameter(Convert.ToInt32(((Button)sender).Name));
                CathodeResourceReference resRef = selected_flowgraph.GetResourceReferenceByID(cResource.resourceID);
                if (resRef == null || resRef.entryType != CathodeResourceReferenceType.RENDERABLE_INSTANCE) return;
                for (int p = 0; p < resRef.entryCountREDS; p++) redsList.Add(redsBIN.GetRenderableElement(resRef.entryIndexREDS + p));
                if (resRef.entryCountREDS != redsList.Count || redsList.Count == 0) return; //TODO: handle this nicer
                selected_reds_ref = resRef;
                CathodeEditorGUI_EditResource res_editor = new CathodeEditorGUI_EditResource(modelPAK.GetCS2s(), redsList);
                res_editor.Show();
                res_editor.EditComplete += new FinishedEditingIndexes(res_editor_submitted);
                return;
            }

            CathodeEditorGUI_EditParam param_editor = new CathodeEditorGUI_EditParam(commandsPAK.GetParameter(Convert.ToInt32(((Button)sender).Name)));
            param_editor.Show();
            param_editor.FormClosed += new FormClosedEventHandler(param_editor_closed);
        }
        private void res_editor_submitted(List<RenderableElement> updated_indexes, bool did_update)
        {
            if (did_update)
            {
                //TODO: Cannot save like this. We're only allowed one model ref (potentially only one material ref also) per REDS.BIN.
                //      Need to find existing ref of model and save that way - but then will block how many we can have sequentially.
                selected_reds_ref.entryIndexREDS = redsBIN.GetRenderableElementsCount();
                selected_reds_ref.entryCountREDS = updated_indexes.Count;
                foreach (RenderableElement redEl in updated_indexes) redsBIN.AddRenderableElement(redEl);
            }
            this.Focus();
            */
        }
        private void param_editor_closed(Object sender, FormClosedEventArgs e)
        {
            LoadNode(selected_node);
        }
    }
}
