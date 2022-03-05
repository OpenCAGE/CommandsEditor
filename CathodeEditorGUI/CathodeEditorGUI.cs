using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
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

            //CathodeNavMesh navmesh = new CathodeNavMesh(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\STATE_0\NAV_MESH");
            //CathodeNavMesh navmesh_orig = new CathodeNavMesh(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\STATE_0\NAV_MESH5");

            //string bleh = "";

            //CathodeStringDB cathodeStringDB = new CathodeStringDB(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\GLOBAL\ANIM_STRING_DB_DEBUG.BIN");
            //CathodeStringDB cathodeStringDB2 = new CathodeStringDB(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\GLOBAL\ANIM_STRING_DB.BIN");

            /*
            string level = "TECH_RND_HZDLAB";
            string[] files = Directory.GetFiles(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\", "COMMANDS.PAK", SearchOption.AllDirectories);
            CommandsPAK newFlows = new CommandsPAK(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\" + level + @"\WORLD\COMMANDS.PAK");
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Contains(level)) continue;
                CommandsPAK pak = new CommandsPAK(files[i]);
                for (int x = 0;x < pak.Flowgraphs.Count; x++)
                {
                    //if (pak.Flowgraphs[x].name.Contains(":")) continue;
                    CathodeFlowgraph flow = newFlows.Flowgraphs.FirstOrDefault(o => o.nodeID == pak.Flowgraphs[x].nodeID);
                    if (flow == null)
                    {
                        newFlows.Flowgraphs.Add(pak.Flowgraphs[x]);
                    }
                }
                if (i == 2) break;
            }
            newFlows.Save();
            return;
            */

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
                FileTree.BeginUpdate();
                FileTree.Nodes.Clear();
                FileTree.EndUpdate();
                first_executed_flowgraph.Text = "Entry point: ";
                flowgraph_count.Text = "Flowgraph count: ";
            }
            if (clear_node_list)
            {
                node_search_box.Text = "";
                currentSearch = "";
                groupBox1.Text = "Selected Flowgraph Content";
                flowgraph_content.BeginUpdate();
                flowgraph_content.Items.Clear();
                flowgraph_content_RAW.Clear();
                flowgraph_content.EndUpdate();
                CurrentInstance.selectedFlowgraph = null;
                //editFlowgraphResources.Visible = false;
            }
            if (clear_parameter_list)
            {
                CurrentInstance.selectedEntity = null;
                selected_node_id.Text = "";
                selected_node_type.Text = "";
                selected_node_type_description.Text = "";
                selected_node_name.Text = "";
                for (int i = 0; i < NodeParams.Controls.Count; i++) 
                    NodeParams.Controls[i].Dispose();
                NodeParams.Controls.Clear();
                node_children.Items.Clear();
                currentlyShowingChildLinks = true;
                node_to_flowgraph_jump.Visible = false;
                editCAGEAnimationKeyframes.Visible = false;
                editNodeResources.Visible = false;
            }
        }

        /* Enable the option to load COMMANDS */
        public void EnableLoadingOfPaks(bool shouldEnable)
        {
            load_commands_pak.Invoke(new Action(() => { load_commands_pak.Enabled = shouldEnable; }));
            env_list.Invoke(new Action(() => { env_list.Enabled = shouldEnable; }));
        }

        /* Load a COMMANDS.PAK into the editor */
        Task currentBackgroundCacher = null;
        public void LoadCommandsPAK(string level)
        {
            //Reset UI
            ClearUI(true, true, true);
            CurrentInstance.commandsPAK = null;

            //Load
            string path_to_ENV = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + level;
            try
            {
                CurrentInstance.commandsPAK = new CommandsPAK(path_to_ENV + "/WORLD/COMMANDS.PAK");
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\n" + e.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentInstance.commandsPAK = null;
                return;
            }

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

            //Begin caching entity names so we don't have to keep generating them
            if (currentBackgroundCacher != null) currentBackgroundCacher.Dispose();
            currentBackgroundCacher = Task.Factory.StartNew(() => EditorUtils.GenerateEntityNameCache(this));

            //Populate file tree
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetFlowgraphNames().ToList());

            //Show info in UI
            RefreshStatsUI();
        }
        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            LoadCommandsPAK(env_list.SelectedItem.ToString());
        }
        private void RefreshStatsUI()
        {
            first_executed_flowgraph.Text = "Entry point: " + CurrentInstance.commandsPAK.EntryPoints[0].name;
            flowgraph_count.Text = "Flowgraph count: " + CurrentInstance.commandsPAK.Flowgraphs.Count;
        }

        /* Save the current edits */
        public void SaveCommandsPAK()
        {
            if (CurrentInstance.commandsPAK == null) return;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                CurrentInstance.commandsPAK.Save();
            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Failed to save COMMANDS.PAK!\n" + e.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            NodeDBEx.SaveNames();

            if (modifyMVR.Checked)
            {
                CathodeMovers mvr = new CathodeMovers(CurrentInstance.commandsPAK.Filepath.Replace("COMMANDS.PAK", "MODELS.MVR"));
                for (int i = 0; i < mvr.Movers.Count; i++)
                {
                    /*
                    if (mvr.Movers[i].IsThisTypeID == MoverType.STATIC_MODEL)
                    {
                        CathodeFlowgraph flowgraph = GetFlowgraphContainingNode(mvr.Movers[i].NodeID);
                        if (flowgraph == null) continue;
                        if (flowgraph.name.Contains("REQUIRED_ASSETS") && flowgraph.name.Contains("VFX")) continue;
                        MoverEntry mover = mvr.Movers[i];
                        mover.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                        mvr.Movers[i] = mover;
                    }
                    */
                    MOVER_DESCRIPTOR mover = mvr.Movers[i];
                    //This is a **TEMP** hack!
                    mover.Transform = 
                        System.Numerics.Matrix4x4.CreateScale(new System.Numerics.Vector3(0, 0, 0)) * 
                        System.Numerics.Matrix4x4.CreateFromQuaternion(System.Numerics.Quaternion.Identity) * 
                        System.Numerics.Matrix4x4.CreateTranslation(new System.Numerics.Vector3(-9999.0f, -9999.0f, -9999.0f));
                    mover.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                    mover.NodeID = new cGUID("00-00-00-00");
                    mvr.Movers[i] = mover;
                }
                if (mvr.FilePath != "") mvr.Save();
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

        /* Edit the loaded COMMANDS.PAK entry point */
        private void editEntryPoint_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.commandsPAK == null || !CurrentInstance.commandsPAK.Loaded) return;
            CathodeEditorGUI_EditEntryPoint edit_entrypoint = new CathodeEditorGUI_EditEntryPoint();
            edit_entrypoint.Show();
            edit_entrypoint.FormClosed += new FormClosedEventHandler(edit_entrypoint_closed);
        }
        private void edit_entrypoint_closed(Object sender, FormClosedEventArgs e)
        {
            RefreshStatsUI();
            this.BringToFront();
            this.Focus();
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
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    CathodeEntity entity = EditorUtils.ResolveHierarchy(((ProxyEntity)CurrentInstance.selectedEntity).hierarchy, out flow);
                    if (entity != null)
                    {
                        LoadFlowgraph(flow.name);
                        LoadEntity(entity);
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
            RefreshStatsUI();
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
            RefreshStatsUI();
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetFlowgraphNames().ToList());
        }

        /* Select node from loaded flowgraph */
        private void flowgraph_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flowgraph_content.SelectedIndex == -1 || CurrentInstance.selectedFlowgraph == null) return;
            try
            {
                cGUID entityID = new cGUID(flowgraph_content.SelectedItem.ToString().Substring(1, 11));
                CathodeEntity thisEntity = CurrentInstance.selectedFlowgraph.GetEntityByID(entityID);
                if (thisEntity != null) LoadEntity(thisEntity);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encountered an issue while looking up entity!\nPlease report this on GitHub!\n" + ex.Message, "Failed lookup!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (thisNodeInfo != null) LoadEntity(thisNodeInfo);
        }

        /* Flip the child link list to contain parents (this is an expensive search, which is why we only do it on request) */
        private void showLinkParents_Click(object sender, EventArgs e)
        {
            currentlyShowingChildLinks = !currentlyShowingChildLinks;
            RefreshNodeLinks();
        }

        /* Search node list */
        private string currentSearch = "";
        private void node_search_btn_Click(object sender, EventArgs e)
        {
            if (node_search_box.Text == currentSearch) return;
            List<string> matched = new List<string>();
            foreach (string item in flowgraph_content_RAW) if (item.ToUpper().Contains(node_search_box.Text.ToUpper())) matched.Add(item);
            flowgraph_content.BeginUpdate();
            flowgraph_content.Items.Clear();
            for (int i = 0; i < matched.Count; i++) flowgraph_content.Items.Add(matched[i]);
            flowgraph_content.EndUpdate();
            currentSearch = node_search_box.Text;
        }

        /* Load a flowgraph into the UI */
        List<string> flowgraph_content_RAW = new List<string>();
        private void LoadFlowgraph(string FileName)
        {
            ClearUI(false, true, true);
            CathodeFlowgraph entry = CurrentInstance.commandsPAK.Flowgraphs[CurrentInstance.commandsPAK.GetFileIndex(FileName)];
            CurrentInstance.selectedFlowgraph = entry;
            Cursor.Current = Cursors.WaitCursor;

            flowgraph_content.BeginUpdate();
            List<CathodeEntity> entities = entry.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                string desc = EditorUtils.GenerateNodeName(entities[i], CurrentInstance.selectedFlowgraph);
                flowgraph_content.Items.Add(desc);
                flowgraph_content_RAW.Add(desc);
            }
            flowgraph_content.EndUpdate();

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
            ReloadUIForNewEntity(((CathodeEditorGUI_AddNode)sender).NewEntity);
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected entity */
        private void removeSelectedNode_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            if (!ConfirmAction("Are you sure you want to remove this entity?")) return;

            string removedID = CurrentInstance.selectedEntity.nodeID.ToString();

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

            int indexToRemove = -1;
            for (int i = 0; i < flowgraph_content.Items.Count; i++)
            {
                if (flowgraph_content.Items[i].ToString().Substring(1, 11) == removedID)
                {
                    indexToRemove = i;
                    break;
                }
            }
            if (indexToRemove != -1) flowgraph_content.Items.RemoveAt(indexToRemove);
            indexToRemove = -1;
            for (int i = 0; i < flowgraph_content_RAW.Count; i++)
            {
                if (flowgraph_content_RAW[i].Substring(1, 11) == removedID)
                {
                    indexToRemove = i;
                    break;
                }
            }
            if (indexToRemove != -1) flowgraph_content_RAW.RemoveAt(indexToRemove);
            else LoadFlowgraph(CurrentInstance.selectedFlowgraph.name);

            ClearUI(false, false, true);
        }

        /* Duplicate selected entity */
        private void duplicateSelectedNode_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            if (!ConfirmAction("Are you sure you want to duplicate this entity?")) return;

            //Generate new node ID and name
            CathodeEntity newEnt = Utilities.CloneObject(CurrentInstance.selectedEntity);
            newEnt.nodeID = Utilities.GenerateGUID(DateTime.Now.ToString("G"));
            NodeDBEx.AddNewNodeName(newEnt.nodeID, NodeDBEx.GetEntityName(CurrentInstance.selectedEntity.nodeID) + "_clone");

            //Add parent links in to this node that linked in to the other node
            List<CathodeEntity> ents = CurrentInstance.selectedFlowgraph.GetEntities();
            List<CathodeNodeLink> newLinks = new List<CathodeNodeLink>();
            int num_of_new_things = 1;
            foreach (CathodeEntity entity in ents)
            {
                newLinks.Clear();
                foreach (CathodeNodeLink link in entity.childLinks)
                {
                    if (link.childID == CurrentInstance.selectedEntity.nodeID)
                    {
                        CathodeNodeLink newLink = new CathodeNodeLink();
                        newLink.connectionID = Utilities.GenerateGUID(DateTime.Now.ToString("G") + num_of_new_things.ToString()); num_of_new_things++;
                        newLink.childID = newEnt.nodeID;
                        newLink.childParamID = link.childParamID;
                        newLink.parentParamID = link.parentParamID;
                        newLinks.Add(newLink);
                    }
                }
                if (newLinks.Count != 0) entity.childLinks.AddRange(newLinks);
            }

            //Save back to flowgraph
            switch (newEnt.variant)
            {
                case EntityVariant.FUNCTION:
                    CurrentInstance.selectedFlowgraph.functions.Add((FunctionEntity)newEnt);
                    break;
                case EntityVariant.DATATYPE:
                    CurrentInstance.selectedFlowgraph.datatypes.Add((DatatypeEntity)newEnt);
                    break;
                case EntityVariant.PROXY:
                    CurrentInstance.selectedFlowgraph.proxies.Add((ProxyEntity)newEnt);
                    break;
                case EntityVariant.OVERRIDE:
                    CurrentInstance.selectedFlowgraph.overrides.Add((OverrideEntity)newEnt);
                    break;
                case EntityVariant.NOT_SETUP:
                    CurrentInstance.selectedFlowgraph.unknowns.Add(newEnt);
                    break;
            }

            //Load in to UI
            ReloadUIForNewEntity(newEnt);
        }

        /* Rename selected entity */
        private void renameSelectedNode_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            CathodeEditorGUI_RenameNode rename_node = new CathodeEditorGUI_RenameNode(CurrentInstance.selectedEntity.nodeID);
            rename_node.Show();
            rename_node.FormClosed += new FormClosedEventHandler(rename_node_closed);
        }
        private void rename_node_closed(Object sender, FormClosedEventArgs e)
        {
            if (((CathodeEditorGUI_RenameNode)sender).didSave &&
                ((CathodeEditorGUI_RenameNode)sender).NodeID == CurrentInstance.selectedEntity.nodeID)
            {
                NodeDBEx.AddNewNodeName(CurrentInstance.selectedEntity.nodeID, ((CathodeEditorGUI_RenameNode)sender).NodeName);
                string nodeID = CurrentInstance.selectedEntity.nodeID.ToString();
                string newNodeName = EditorUtils.GenerateNodeName(CurrentInstance.selectedEntity, CurrentInstance.selectedFlowgraph);
                for (int i = 0; i < flowgraph_content.Items.Count; i++)
                {
                    if (flowgraph_content.Items[i].ToString().Substring(1, 11) == nodeID)
                    {
                        flowgraph_content.Items[i] = newNodeName;
                        break;
                    }
                }
                for (int i = 0; i < flowgraph_content_RAW.Count; i++)
                {
                    if (flowgraph_content_RAW[i].Substring(1, 11) == nodeID)
                    {
                        flowgraph_content_RAW[i] = newNodeName;
                        break;
                    }
                }
                LoadEntity(CurrentInstance.selectedEntity);
            }
            this.BringToFront();
            this.Focus();
        }
        
        /* Perform a partial UI reload for a newly added entity */
        private void ReloadUIForNewEntity(CathodeEntity newEnt)
        {
            if (CurrentInstance.selectedFlowgraph == null || newEnt == null) return;
            if (currentSearch == "")
            {
                string newNodeName = EditorUtils.GenerateNodeName(newEnt, CurrentInstance.selectedFlowgraph);
                flowgraph_content.Items.Add(newNodeName);
                flowgraph_content_RAW.Add(newNodeName);
            }
            else
            {
                LoadFlowgraph(CurrentInstance.selectedFlowgraph.name);
            }
            LoadEntity(newEnt);
        }

        /* Load a entity into the UI */
        private void LoadEntity(CathodeEntity edit_node)
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

            //show resource editor button if this node has a resource reference
            cGUID resourceParamID = Utilities.GenerateGUID("resource");
            CathodeLoadedParameter resourceParam = CurrentInstance.selectedEntity.parameters.FirstOrDefault(o => o.paramID == resourceParamID);
            editNodeResources.Visible = ((resourceParam != null) || CurrentInstance.selectedEntity.resources.Count != 0);

            //populate parameter inputs
            int current_ui_offset = 7;
            for (int i = 0; i < edit_node.parameters.Count; i++)
            {
                if (edit_node.parameters[i].paramID == resourceParamID) continue; //We use the resource editor button (above) for resource parameters

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
            node_children.BeginUpdate();
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
            node_children.EndUpdate();
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
            LoadEntity(CurrentInstance.selectedEntity);
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
            LoadEntity(CurrentInstance.selectedEntity);
            this.BringToFront();
            this.Focus();
        }

        /* Edit CAGEAnimation keyframes */
        private void editCAGEAnimationKeyframes_Click(object sender, EventArgs e)
        {
            CAGEAnimationEditor keyframeEditor = new CAGEAnimationEditor((CAGEAnimation)CurrentInstance.selectedEntity);
            keyframeEditor.Show();
        }

        /* Edit resources referenced by the entity */
        private void editNodeResources_Click(object sender, EventArgs e)
        {
            //CurrentInstance.currentEntity - .parameters.FirstOrDefault("resources") - .resources
            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(CurrentInstance.selectedEntity);
            resourceEditor.Show();
        }

        /* Edit resources referenced by the flowgraph */
        private void editFlowgraphResources_Click(object sender, EventArgs e)
        {
            //CurrentInstance.currentFlowgraph.resources
            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(CurrentInstance.selectedFlowgraph);
            resourceEditor.Show();
        }

        /* Confirm an action */
        private bool ConfirmAction(string msg)
        {
            return (MessageBox.Show(msg, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void CathodeEditorGUI_Load(object sender, EventArgs e)
        {
            return;

            CathodeMovers mvr = new CathodeMovers(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\MODELS.MVR");
            //CathodeMovers.Movers = CathodeMovers.Movers.OrderBy(o => o.IsThisTypeID).ToList<alien_mvr_entry>();
            for (int i = 0; i < mvr.Movers.Count; i++)
            {
                MOVER_DESCRIPTOR thisMvr = mvr.Movers[i];

                //Transform
                //CollisionMapThingID
                //REDSIndex
                //ModelCount
                //ResourcesBINIndex
                //EnvironmentMapBINIndex
                //IsThisTypeID

                thisMvr.Transform = 
                    System.Numerics.Matrix4x4.CreateScale(new System.Numerics.Vector3(0,0,0)) * 
                    System.Numerics.Matrix4x4.CreateFromQuaternion(System.Numerics.Quaternion.Identity) * 
                    System.Numerics.Matrix4x4.CreateTranslation(new System.Numerics.Vector3(-9999.0f, -9999.0f, -9999.0f));
                //mvr.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                thisMvr.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                thisMvr.NodeID = new cGUID("00-00-00-00");

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

                mvr.Movers[i] = thisMvr;
            }
            mvr.Save();

            mvr = new CathodeMovers(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\TECH_HUB\WORLD\MODELS.MVR");
            mvr.Save();

            mvr = new CathodeMovers(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\MODELS.MVR");
            mvr.Movers.RemoveAll(o => o.NodeID != new cGUID(0));
            mvr.Save();

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
        public System.Numerics.Matrix4x4 transform;

        public void SetTransform(System.Numerics.Vector3 position, System.Numerics.Quaternion rotation, System.Numerics.Vector3 scale)
        {
            transform = System.Numerics.Matrix4x4.CreateScale(scale) * System.Numerics.Matrix4x4.CreateFromQuaternion(rotation) * System.Numerics.Matrix4x4.CreateTranslation(position);
        }
    }
}
