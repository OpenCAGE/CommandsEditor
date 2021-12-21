using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
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

#if DEBUG
            button1.Visible = true;
#endif
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
                CurrentInstance.selectedFlowgraph = null;
            }
            if (clear_parameter_list)
            {
                CurrentInstance.selectedEntity = null;
                selected_node_id.Text = "";
                selected_node_type.Text = "";
                selected_node_type_description.Text = "";
                selected_node_name.Text = "";
                NodeParams.Controls.Clear();
                node_children.Items.Clear();
                currentlyShowingChildLinks = true;
                node_to_flowgraph_jump.Visible = false;
                editCAGEAnimationKeyframes.Visible = false;
            }
        }

        /* Load a COMMANDS.PAK into the editor */
        public void LoadCommandsPAK(string level)
        {
            //Reset UI
            ClearUI(true, true, true);
            CurrentInstance.commandsPAK = null;

            //Load
            string path_to_ENV = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + level;
            CurrentInstance.commandsPAK = new CommandsPAK(path_to_ENV + "/WORLD/COMMANDS.PAK");

            //Sanity check
            if (!CurrentInstance.commandsPAK.Loaded)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\nPlease place this executable in your Alien: Isolation folder.", "Environment error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (CurrentInstance.commandsPAK.EntryPoints[0] == null)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\nPlease reset your game files.", "COMMANDS.PAK corrupted!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Load in any custom param/node names
            NodeDBEx.LoadNames();

            //Populate file tree
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetFlowgraphNames().ToList());

            //Show info in UI
            first_executed_flowgraph.Text = "Entry point: " + CurrentInstance.commandsPAK.EntryPoints[0].name;
            flowgraph_count.Text = "Flowgraph count: " + CurrentInstance.commandsPAK.Flowgraphs.Count;
        }
        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            LoadCommandsPAK(env_list.SelectedItem.ToString());
        }

        /* Save the current edits */
        public void SaveCommandsPAK()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (CurrentInstance.commandsPAK == null) return;
            CurrentInstance.commandsPAK.Save();
            NodeDBEx.SaveNames();

            if (modifyMVR.Checked)
            {
                ModelsMVR modelsMVR = new ModelsMVR(CurrentInstance.commandsPAK.Filepath.Replace("COMMANDS.PAK", "MODELS.MVR"));
                for (int i = 0; i < modelsMVR.Movers.Count; i++)
                {
                    /*
                    if (modelsMVR.Movers[i].IsThisTypeID == MoverType.STATIC_MODEL)
                    {
                        CathodeFlowgraph flowgraph = GetFlowgraphContainingNode(modelsMVR.Movers[i].NodeID);
                        if (flowgraph == null) continue;
                        if (flowgraph.name.Contains("REQUIRED_ASSETS") && flowgraph.name.Contains("VFX")) continue;
                        CathodeMover mover = modelsMVR.Movers[i];
                        mover.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                        modelsMVR.Movers[i] = mover;
                    }
                    */
                    CathodeMover mover = modelsMVR.Movers[i];
                    //This is a **TEMP** hack!
                    mover.Transform = Matrix4x4.CreateScale(new Vector3(0, 0, 0)) * Matrix4x4.CreateFromQuaternion(Quaternion.Identity) * Matrix4x4.CreateTranslation(new Vector3(-9999.0f, -9999.0f, -9999.0f));
                    mover.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                    mover.NodeID = new cGUID("00-00-00-00");
                    modelsMVR.Movers[i] = mover;
                }
                if (modelsMVR.FilePath != "") modelsMVR.Save();
            }

            Cursor.Current = Cursors.Default;
        }
        private void save_commands_pak_Click(object sender, EventArgs e)
        {
            SaveCommandsPAK();
            MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private CathodeFlowgraph GetFlowgraphContainingNode(cGUID nodeID)
        {
            for (int i = 0; i < CurrentInstance.commandsPAK.Flowgraphs.Count; i++)
            {
                List<CathodeEntity> entities = CurrentInstance.commandsPAK.Flowgraphs[i].GetEntities();
                for (int x = 0; x < entities.Count; x++)
                {
                    if (entities[x].nodeID == nodeID)
                    {
                        return CurrentInstance.commandsPAK.Flowgraphs[i];
                    }
                }
            }
            return null;
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
            CathodeFlowgraph flow;
            switch (CurrentInstance.selectedEntity.variant)
            {
                case EntityVariant.OVERRIDE:
                {
                    CathodeEntity entity = EditorUtils.ResolveHierarchy(((OverrideEntity)CurrentInstance.selectedEntity).hierarchy, out flow);
                    if (entity != null)
                    {
                        LoadFlowgraph(flow.name);
                        LoadNode(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    CathodeEntity entity = EditorUtils.ResolveHierarchy(((ProxyEntity)CurrentInstance.selectedEntity).hierarchy, out flow);
                    if (entity != null)
                    {
                        LoadFlowgraph(flow.name);
                        LoadNode(entity);
                    }
                    break;
                }
                case EntityVariant.FUNCTION:
                {
                    LoadFlowgraph(selected_node_type_description.Text);
                    break;
                }
            }
        }

        /* Add new flowgraph */
        private void addNewFlowgraph_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.commandsPAK == null) return;
            CathodeEditorGUI_AddFlowgraph add_flow = new CathodeEditorGUI_AddFlowgraph();
            add_flow.Show();
            add_flow.FormClosed += new FormClosedEventHandler(add_flow_closed);
        }
        private void add_flow_closed(Object sender, FormClosedEventArgs e)
        {
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetFlowgraphNames().ToList());
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected flowgraph */
        private void removeSelectedFlowgraph_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedFlowgraph == null) return;
            for (int i = 0; i < CurrentInstance.commandsPAK.EntryPoints.Count(); i++)
            {
                if (CurrentInstance.selectedFlowgraph.nodeID == CurrentInstance.commandsPAK.EntryPoints[i].nodeID)
                {
                    MessageBox.Show("Cannot delete a flowgraph which is set as an entry point!", "Cannot delete.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (!ConfirmAction("Are you sure you want to remove this flowgraph?")) return;

            //Remove any entities or links that reference this flowgraph
            for (int i = 0; i < CurrentInstance.commandsPAK.Flowgraphs.Count; i++)
            {
                List<FunctionEntity> prunedFunctionEntities = new List<FunctionEntity>();
                for (int x = 0; x < CurrentInstance.commandsPAK.Flowgraphs[i].functions.Count; x++)
                {
                    if (CurrentInstance.commandsPAK.Flowgraphs[i].functions[x].function == CurrentInstance.selectedFlowgraph.nodeID) continue;
                    List<CathodeNodeLink> prunedNodeLinks = new List<CathodeNodeLink>();
                    for (int l = 0; l < CurrentInstance.commandsPAK.Flowgraphs[i].functions[x].childLinks.Count; l++)
                    {
                        CathodeEntity linkedNode = CurrentInstance.commandsPAK.Flowgraphs[i].GetEntityByID(CurrentInstance.commandsPAK.Flowgraphs[i].functions[x].childLinks[l].childID);
                        if (linkedNode != null && linkedNode.variant == EntityVariant.FUNCTION) if (((FunctionEntity)linkedNode).function == CurrentInstance.selectedFlowgraph.nodeID) continue;
                        prunedNodeLinks.Add(CurrentInstance.commandsPAK.Flowgraphs[i].functions[x].childLinks[l]);
                    }
                    CurrentInstance.commandsPAK.Flowgraphs[i].functions[x].childLinks = prunedNodeLinks;
                    prunedFunctionEntities.Add(CurrentInstance.commandsPAK.Flowgraphs[i].functions[x]);
                }
                CurrentInstance.commandsPAK.Flowgraphs[i].functions = prunedFunctionEntities;
            }
            //TODO: remove proxies etc that also reference any of the removed nodes

            //Remove the flowgraph
            CurrentInstance.commandsPAK.Flowgraphs.Remove(CurrentInstance.selectedFlowgraph);

            //Refresh UI
            ClearUI(false, true, true);
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetFlowgraphNames().ToList());
        }

        /* Select node from loaded flowgraph */
        private void flowgraph_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flowgraph_content.SelectedIndex == -1 || CurrentInstance.selectedFlowgraph == null) return;
            CathodeEntity thisNodeInfo = CurrentInstance.selectedFlowgraph.GetEntityByID(new cGUID(flowgraph_content.SelectedItem.ToString().Substring(1, 11)));
            if (thisNodeInfo != null) LoadNode(thisNodeInfo);
        }

        /* Add new out pin */
        private void addNewLink_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedFlowgraph == null || CurrentInstance.selectedEntity == null) return;
            CathodeEditorGUI_AddPin add_pin = new CathodeEditorGUI_AddPin(CurrentInstance.selectedEntity, CurrentInstance.selectedFlowgraph);
            add_pin.Show();
            add_pin.FormClosed += new FormClosedEventHandler(add_pin_closed);
        }
        private void add_pin_closed(Object sender, FormClosedEventArgs e)
        {
            RefreshNodeLinks();
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected out pin */
        private void removeSelectedLink_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null || CurrentInstance.selectedFlowgraph == null) return;
            if (!ConfirmAction("Are you sure you want to remove this link?")) return;
            if (currentlyShowingChildLinks)
            {
                CurrentInstance.selectedEntity.childLinks.RemoveAt(node_children.SelectedIndex);
            }
            else
            {
                List<CathodeEntity> ents = CurrentInstance.selectedFlowgraph.GetEntities();
                int deleteIndex = -1;
                foreach (CathodeEntity entity in ents)
                {
                    for (int i = 0; i < entity.childLinks.Count; i++)
                    {
                        if (entity.childLinks[i].connectionID == linkedNodeListIDs[node_children.SelectedIndex])
                        {
                            deleteIndex = i;
                            break;
                        }
                    }
                    if (deleteIndex != -1)
                    {
                        entity.childLinks.RemoveAt(deleteIndex);
                        break;
                    }
                }
            }
            RefreshNodeLinks();
        }

        /* Go to selected pin out on button press */
        private void out_pin_goto_Click(object sender, EventArgs e)
        {
            if (node_children.SelectedIndex == -1 || CurrentInstance.selectedFlowgraph == null) return;

            CathodeEntity thisNodeInfo = null;
            if (currentlyShowingChildLinks)
            {
                thisNodeInfo = CurrentInstance.selectedFlowgraph.GetEntityByID(CurrentInstance.selectedEntity.childLinks[node_children.SelectedIndex].childID);
            }
            else
            {
                List<CathodeEntity> ents = CurrentInstance.selectedFlowgraph.GetEntities();
                foreach (CathodeEntity entity in ents)
                {
                    for (int i = 0; i < entity.childLinks.Count; i++)
                    {
                        if (entity.childLinks[i].connectionID == linkedNodeListIDs[node_children.SelectedIndex])
                        {
                            thisNodeInfo = entity;
                            break;
                        }
                    }
                    if (thisNodeInfo != null) break;
                }
            }
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
        List<string> flowgraph_content_RAW = new List<string>();
        private void LoadFlowgraph(string FileName)
        {
            ClearUI(false, true, true);
            CathodeFlowgraph entry = CurrentInstance.commandsPAK.Flowgraphs[CurrentInstance.commandsPAK.GetFileIndex(FileName)];
            CurrentInstance.selectedFlowgraph = entry;
            Cursor.Current = Cursors.WaitCursor;

            List<CathodeEntity> entities = entry.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                string desc = EditorUtils.GenerateNodeName(entities[i], CurrentInstance.selectedFlowgraph);
                flowgraph_content.Items.Add(desc);
                flowgraph_content_RAW.Add(desc);
            }
            groupBox1.Text = entry.name;
            Cursor.Current = Cursors.Default;
        }

        /* Add new entity */
        private void addNewNode_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedFlowgraph == null) return;
            CathodeEditorGUI_AddNode add_parameter = new CathodeEditorGUI_AddNode(CurrentInstance.selectedFlowgraph, CurrentInstance.commandsPAK.Flowgraphs);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(add_node_closed);
        }
        private void add_node_closed(Object sender, FormClosedEventArgs e)
        {
            LoadFlowgraph(CurrentInstance.selectedFlowgraph.name);
            LoadNode(CurrentInstance.selectedEntity); //TODO: load returned new node
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected entity */
        private void removeSelectedNode_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            if (!ConfirmAction("Are you sure you want to remove this entity?")) return;

            switch (CurrentInstance.selectedEntity.variant)
            {
                case EntityVariant.DATATYPE:
                    CurrentInstance.selectedFlowgraph.datatypes.Remove((DatatypeEntity)CurrentInstance.selectedEntity);
                    break;
                case EntityVariant.FUNCTION:
                    CurrentInstance.selectedFlowgraph.functions.Remove((FunctionEntity)CurrentInstance.selectedEntity);
                    break;
                case EntityVariant.OVERRIDE:
                    CurrentInstance.selectedFlowgraph.overrides.Remove((OverrideEntity)CurrentInstance.selectedEntity);
                    break;
                case EntityVariant.PROXY:
                    CurrentInstance.selectedFlowgraph.proxies.Remove((ProxyEntity)CurrentInstance.selectedEntity);
                    break;
                case EntityVariant.NOT_SETUP:
                    CurrentInstance.selectedFlowgraph.unknowns.Remove(CurrentInstance.selectedEntity);
                    break;
            }

            List<CathodeEntity> entities = CurrentInstance.selectedFlowgraph.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                List<CathodeNodeLink> nodeLinks = new List<CathodeNodeLink>();
                for (int x = 0; x < entities[i].childLinks.Count; x++)
                {
                    if (entities[i].childLinks[x].childID != CurrentInstance.selectedEntity.nodeID) nodeLinks.Add(entities[i].childLinks[x]);
                }
                entities[i].childLinks = nodeLinks;
            }

            LoadFlowgraph(CurrentInstance.selectedFlowgraph.name);
        }

        /* Load a entity into the UI */
        private void LoadNode(CathodeEntity edit_node)
        {
            if (edit_node == null) return;

            ClearUI(false, false, true);
            CurrentInstance.selectedEntity = edit_node;
            Cursor.Current = Cursors.WaitCursor;

            //populate info labels
            selected_node_id.Text = edit_node.nodeID.ToString();
            selected_node_type.Text = edit_node.variant.ToString();
            string nodetypedesc = "";
            switch (edit_node.variant)
            {
                case EntityVariant.FUNCTION:
                    nodetypedesc = NodeDBEx.GetParameterName(((FunctionEntity)edit_node).function);
                    node_to_flowgraph_jump.Visible = (CurrentInstance.commandsPAK.GetFlowgraph(((FunctionEntity)edit_node).function) != null);
                    selected_node_name.Text = NodeDBEx.GetEntityName(edit_node.nodeID);
                    editCAGEAnimationKeyframes.Visible = nodetypedesc == "CAGEAnimation";
                    break;
                case EntityVariant.DATATYPE:
                    nodetypedesc = "DataType " + ((DatatypeEntity)edit_node).type.ToString();
                    selected_node_name.Text = NodeDBEx.GetParameterName(((DatatypeEntity)edit_node).parameter);
                    break;
                case EntityVariant.PROXY:
                case EntityVariant.OVERRIDE:
                    node_to_flowgraph_jump.Visible = true;
                    selected_node_name.Text = NodeDBEx.GetEntityName(edit_node.nodeID);
                    break;
                default:
                    selected_node_name.Text = NodeDBEx.GetEntityName(edit_node.nodeID);
                    break;
            }
            selected_node_type_description.Text = nodetypedesc;

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
                        ((GUI_HexDataType)parameterGUI).PopulateUI((CathodeResource)this_param, edit_node.parameters[i].paramID, CurrentInstance.selectedFlowgraph); 
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
        List<cGUID> linkedNodeListIDs = new List<cGUID>();
        private void RefreshNodeLinks()
        {
            node_children.Items.Clear();
            linkedNodeListIDs.Clear();
            addNewLink.Enabled = currentlyShowingChildLinks;
            //removeSelectedLink.Enabled = currentlyShowingChildLinks;
            //out_pin_goto.Enabled = currentlyShowingChildLinks;
            showLinkParents.Text = (currentlyShowingChildLinks) ? "Parents" : "Children";

            if (CurrentInstance.selectedFlowgraph == null || CurrentInstance.selectedEntity == null) return;
            if (currentlyShowingChildLinks)
            {
                //Child links (pins out of this node)
                foreach (CathodeNodeLink link in CurrentInstance.selectedEntity.childLinks)
                {
                    node_children.Items.Add(
                        /*"[" + link.connectionID.ToString() + "] " +*/
                        "(" + NodeDBEx.GetParameterName(link.parentParamID) + ") => " +
                        NodeDBEx.GetEntityName(link.childID) + 
                        " (" + NodeDBEx.GetParameterName(link.childParamID) + ")");
                    linkedNodeListIDs.Add(link.connectionID);
                }
            }
            else
            {
                //Parent links (pins into this node)
                List<CathodeEntity> ents = CurrentInstance.selectedFlowgraph.GetEntities();
                foreach (CathodeEntity entity in ents)
                {
                    foreach (CathodeNodeLink link in entity.childLinks)
                    {
                        if (link.childID != CurrentInstance.selectedEntity.nodeID) continue;
                        node_children.Items.Add(
                            /*"[" + link.connectionID.ToString() + "] " +*/
                            NodeDBEx.GetEntityName(entity.nodeID) +
                            " (" + NodeDBEx.GetParameterName(link.parentParamID) + ") => " +
                            "(" + NodeDBEx.GetParameterName(link.childParamID) + ")");
                        linkedNodeListIDs.Add(link.connectionID);
                    }
                }
            }
        }

        /* Add a new parameter */
        private void addNewParameter_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            CathodeEditorGUI_AddParameter add_parameter = new CathodeEditorGUI_AddParameter(CurrentInstance.selectedEntity);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(param_add_closed);
        }
        private void param_add_closed(Object sender, FormClosedEventArgs e)
        {
            LoadNode(CurrentInstance.selectedEntity);
            this.BringToFront();
            this.Focus();
        }

        /* Remove a parameter */
        private void removeParameter_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            if (CurrentInstance.selectedEntity.parameters.Count == 0) return;
            CathodeEditorGUI_RemoveParameter remove_parameter = new CathodeEditorGUI_RemoveParameter(CurrentInstance.selectedEntity);
            remove_parameter.Show();
            remove_parameter.FormClosed += new FormClosedEventHandler(param_remove_closed);
        }
        private void param_remove_closed(Object sender, FormClosedEventArgs e)
        {
            LoadNode(CurrentInstance.selectedEntity);
            this.BringToFront();
            this.Focus();
        }

        /* Edit CAGEAnimation keyframes */
        private void editCAGEAnimationKeyframes_Click(object sender, EventArgs e)
        {
            CAGEAnimationEditor keyframeEditor = new CAGEAnimationEditor((CAGEAnimation)CurrentInstance.selectedEntity);
            keyframeEditor.Show();
        }

        /* Confirm an action */
        private bool ConfirmAction(string msg)
        {
            return (MessageBox.Show(msg, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void CathodeEditorGUI_Load(object sender, EventArgs e)
        {
            return;

            ModelsMVR modelsMVR = new ModelsMVR(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\MODELS.MVR");
            //modelsMVR.Movers = modelsMVR.Movers.OrderBy(o => o.IsThisTypeID).ToList<alien_mvr_entry>();
            for (int i = 0; i < modelsMVR.Movers.Count; i++)
            {
                CathodeMover mvr = modelsMVR.Movers[i];

                //Transform
                //CollisionMapThingID
                //REDSIndex
                //ModelCount
                //ResourcesBINIndex
                //EnvironmentMapBINIndex
                //IsThisTypeID

                mvr.Transform = Matrix4x4.CreateScale(new Vector3(0,0,0)) * Matrix4x4.CreateFromQuaternion(Quaternion.Identity) * Matrix4x4.CreateTranslation(new Vector3(-9999.0f, -9999.0f, -9999.0f));
                //mvr.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                mvr.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                mvr.NodeID = new cGUID("00-00-00-00");

                //mvr.Unknowns5_ = new Vector4(0, 0, 0, 0);


                //mvr.InstanceState[0].X = 1; //Light R
                //mvr.InstanceState[0].Y = 1; //Light G
                //mvr.InstanceState[0].Z = 1; //Light B

                //mvr.InstanceState[0].W = 0; //Unused?

                //mvr.InstanceState[1].X = 1; //Material tint R
                //mvr.InstanceState[1].Y = 1; //Material tint G
                //mvr.InstanceState[1].Z = 1; //Material tint B

                //mvr.InstanceState[1].W = 1; //Unused?

                // mvr.InstanceState[2].X = 10; //Brightness of volumetric light meshes
                //mvr.InstanceState[2].Y = 0.50f; //setting this to zero makes all particle sprites black, anything higher than one fucks with lighting
                //mvr.InstanceState[2].Z = 0;  //this seems to define some sort of offset for particle systems

                //mvr.InstanceState[2].W = 10; //unused?
                // mvr.InstanceState[3].X = 1; //unused?

                // mvr.InstanceState[3].Y = 100; //Light radius?

                //mvr.InstanceState[3].Z = 10; //Diffuse texture tile horizontal
                // mvr.InstanceState[3].W = 1; //Diffuse texture tile vertical

                //maybe these are normal/specular map scales if the above is diffuse?
                // mvr.InstanceState[4].X = 10; //unused?
                //mvr.InstanceState[4].Y = 500; //unsued?
                //mvr.InstanceState[4].Z = 100; //unused?
                //mvr.InstanceState[4].W = 50; //unused?

                //none of these seem to do anything??
                //mvr.UnknownID = new cGUID("00-00-00-00");
                //mvr.NodeID = new cGUID("00-00-00-00");
                //mvr.ResourcesBINID = new cGUID("00-00-00-00");
                //mvr.UnknownMinMax_[0] = new Vector3(0, 0, 0);
                //mvr.UnknownMinMax_[1] = new Vector3(0, 0, 0);
                //mvr.UnknownValue1 = 0;
                //mvr.UnknownValue = 0;
                //mvr.Unknown5_ = 0;
                //mvr.Unknowns61_ = 0;
                //mvr.Unknowns60_ = 0;
                //mvr.Unknown17_ = 0;
                //mvr.Unknowns70_ = 0;
                //mvr.Unknowns71_ = 0;
                //mvr.UnknownMinMax_[0] = new Vector3(0, 0, 0);
                //mvr.UnknownMinMax_[1] = new Vector3(0, 0, 0);

                //one of these contains some sort of lighting info
                //mvr.UnknownValue3_ = 0;
                //mvr.UnknownValue4_ = 0;
                //mvr.Unknown2_ = 0;
                //mvr.Unknowns2_[0] = 0;
                //mvr.Unknowns2_[1] = 0;
                //mvr.Unknown3_.X = 0;
                //mvr.Unknown3_.Y = 0;
                //mvr.Unknown3_.Z = 0;
                //mvr.Unknown3_.W = 0;

                modelsMVR.Movers[i] = mvr;
            }
            modelsMVR.Save();

            modelsMVR = new ModelsMVR(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\TECH_HUB\WORLD\MODELS.MVR");
            modelsMVR.Save();

            modelsMVR = new ModelsMVR(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\MODELS.MVR");
            modelsMVR.Movers.RemoveAll(o => o.NodeID != new cGUID(0));
            modelsMVR.Save();

            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /*
            LoadCommandsPAK(@"DLC\BSPNOSTROMO_TWOTEAMS_PATCH");
            CathodeFlowgraph flow = CurrentInstance.commandsPAK.Flowgraphs.FirstOrDefault(o => o.name == @"DLC\PREORDER\PODLC_TWOTEAMS");
            CathodeEntity ent = flow.GetEntities().FirstOrDefault(o => o.nodeID == new cGUID("03-2D-F4-38"));
            CAGEAnimationEditor edit = new CAGEAnimationEditor((CATHODE.Commands.CAGEAnimation)ent);
            edit.Show();
            */
            LoadCommandsPAK(@"HAB_ShoppingCentre");
            foreach (CathodeFlowgraph flow in CurrentInstance.commandsPAK.Flowgraphs)
            {
                flow.proxies.Clear();
            }
            modifyMVR.Checked = false;
            SaveCommandsPAK();
        }
    }

    public struct CathodeGloballyTransformedEntity
    {
        public MoverType type;
        public CathodeEntity entity;
        public Matrix4x4 transform;

        public void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform = Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(position);
        }
    }
}
