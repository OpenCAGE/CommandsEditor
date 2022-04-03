using CATHODE;
using CATHODE.Commands;
using CATHODE.Models;
using CathodeEditorGUI.UserControls;
using CathodeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI : Form
    {
        private TreeUtility treeHelper;
        private bool currentlyShowingChildLinks = true;

        public CathodeEditorGUI()
        {
            //CathodeModels mdl = new CathodeModels(
            //    @"F:\Alien Isolation Modding\Isolation Mod Tools\Alien Isolation PC Final\DATA\ENV\PRODUCTION\BSP_LV426_PT01\RENDERABLE\MODELS_LEVEL.BIN",
            //    @"F:\Alien Isolation Modding\Isolation Mod Tools\Alien Isolation PC Final\DATA\ENV\PRODUCTION\BSP_LV426_PT01\RENDERABLE\LEVEL_MODELS.PAK");

            InitializeComponent();
            treeHelper = new TreeUtility(FileTree);

            //Populate available maps
            env_list.Items.Clear();
            List<string> mapList = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            for (int i = 0; i < mapList.Count; i++)
            {
                string[] fileSplit = mapList[i].Split(new[] { "PRODUCTION" }, StringSplitOptions.None);
                string mapName = fileSplit[fileSplit.Length - 1].Substring(1, fileSplit[fileSplit.Length - 1].Length - 20);
                mapList[i] = (mapName);
            }
            mapList.Remove("DLC\\BSPNOSTROMO_RIPLEY"); mapList.Remove("DLC\\BSPNOSTROMO_TWOTEAMS");
            env_list.Items.AddRange(mapList.ToArray());
            if (env_list.Items.Contains("FRONTEND")) env_list.SelectedItem = "FRONTEND";
            else env_list.SelectedIndex = 0;

//#if debug
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
//#endif
        }

        /* Clear the UI */
        private void ClearUI(bool clear_composite_list, bool clear_entity_list, bool clear_parameter_list)
        {
            if (clear_composite_list)
            {
                FileTree.BeginUpdate();
                FileTree.Nodes.Clear();
                FileTree.EndUpdate();
                root_composite_display.Text = "Entry point: ";
                composite_count_display.Text = "Composite count: ";
            }
            if (clear_entity_list)
            {
                entity_search_box.Text = "";
                currentSearch = "";
                groupBox1.Text = "Selected Composite Content";
                composite_content.BeginUpdate();
                composite_content.Items.Clear();
                composite_content_RAW.Clear();
                composite_content.EndUpdate();
                CurrentInstance.selectedComposite = null;
                editCompositeResources.Visible = false;
            }
            if (clear_parameter_list)
            {
                CurrentInstance.selectedEntity = null;
                selected_entity_id.Text = "";
                selected_entity_type.Text = "";
                selected_entity_type_description.Text = "";
                selected_entity_name.Text = "";
                for (int i = 0; i < entity_params.Controls.Count; i++) 
                    entity_params.Controls[i].Dispose();
                entity_params.Controls.Clear();
                entity_children.Items.Clear();
                currentlyShowingChildLinks = true;
                jumpToComposite.Visible = false;
                editTriggerSequence.Visible = false;
                editCAGEAnimationKeyframes.Visible = false;
                editEntityResources.Visible = false;
            }
        }

        /* Enable the option to load COMMANDS */
        public void EnableLoadingOfPaks(bool shouldEnable)
        {
            load_commands_pak.Invoke(new Action(() => { load_commands_pak.Enabled = shouldEnable; load_commands_pak.Text = (shouldEnable) ? "Load" : "Caching..."; }));
            env_list.Invoke(new Action(() => { env_list.Enabled = shouldEnable; }));
        }

        /* Load a COMMANDS.PAK into the editor */
        Task currentBackgroundCacher = null;
        public void LoadCommandsPAK(string level)
        {
            //Reset UI
            ClearUI(true, true, true);
            EditorUtils.ResetHierarchyPurgeCache();
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

            //Load in any custom param/entity names
            EntityDBEx.LoadNames();

            //Begin caching entity names so we don't have to keep generating them
            if (currentBackgroundCacher != null) currentBackgroundCacher.Dispose();
            currentBackgroundCacher = Task.Factory.StartNew(() => EditorUtils.GenerateEntityNameCache(this));

            //Populate file tree
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetCompositeNames().ToList());

            //Show info in UI
            RefreshStatsUI();

            //Select entry point composite
            LoadComposite(CurrentInstance.commandsPAK.EntryPoints[0].name);
        }
        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            LoadCommandsPAK(env_list.SelectedItem.ToString());
        }
        private void RefreshStatsUI()
        {
            root_composite_display.Text = "Entry point: " + CurrentInstance.commandsPAK.EntryPoints[0].name;
            composite_count_display.Text = "Composite count: " + CurrentInstance.commandsPAK.Composites.Count;
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
            EntityDBEx.SaveNames();

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
        private CathodeComposite GetCompositeContainingEntity(cGUID entityID)
        {
            for (int i = 0; i < CurrentInstance.commandsPAK.Composites.Count; i++)
            {
                List<CathodeEntity> entities = CurrentInstance.commandsPAK.Composites[i].GetEntities();
                for (int x = 0; x < entities.Count; x++)
                {
                    if (entities[x].shortGUID == entityID)
                    {
                        return CurrentInstance.commandsPAK.Composites[i];
                    }
                }
            }
            return null;
        }

        /* Edit the loaded COMMANDS.PAK entry point */
        private void editEntryPoint_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.commandsPAK == null || !CurrentInstance.commandsPAK.Loaded) return;
            CathodeEditorGUI_EditRootComposite edit_entrypoint = new CathodeEditorGUI_EditRootComposite();
            edit_entrypoint.Show();
            edit_entrypoint.FormClosed += new FormClosedEventHandler(edit_entrypoint_closed);
        }
        private void edit_entrypoint_closed(Object sender, FormClosedEventArgs e)
        {
            RefreshStatsUI();
            this.BringToFront();
            this.Focus();
        }

        /* Load entities for selected composite */
        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (FileTree.SelectedNode == null) return;
            if (((TreeItem)FileTree.SelectedNode.Tag).Item_Type != TreeItemType.EXPORTABLE_FILE) return;
            LoadComposite(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
        }

        /* If selected entity is a composite instance, allow jump to it */
        private void jumpToComposite_Click(object sender, EventArgs e)
        {
            CathodeComposite flow;
            switch (CurrentInstance.selectedEntity.variant)
            {
                case EntityVariant.OVERRIDE:
                {
                    CathodeEntity entity = EditorUtils.ResolveHierarchy(((OverrideEntity)CurrentInstance.selectedEntity).hierarchy, out flow);
                    if (entity != null)
                    {
                        LoadComposite(flow.name);
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    CathodeEntity entity = EditorUtils.ResolveHierarchy(((ProxyEntity)CurrentInstance.selectedEntity).hierarchy, out flow);
                    if (entity != null)
                    {
                        LoadComposite(flow.name);
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.FUNCTION:
                {
                    LoadComposite(selected_entity_type_description.Text);
                    break;
                }
            }
        }

        /* Select entry point from top of UI */
        private void SelectEntryPointUI(object sender, System.EventArgs e)
        {
            if (CurrentInstance.commandsPAK == null || !CurrentInstance.commandsPAK.Loaded) return;
            LoadComposite(CurrentInstance.commandsPAK.EntryPoints[0].name);
        }

        /* Add new composite (really we should be able to do this OFF OF entities, like making a prefab) */
        private void addNewComposite_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.commandsPAK == null) return;
            CathodeEditorGUI_AddComposite add_flow = new CathodeEditorGUI_AddComposite();
            add_flow.Show();
            add_flow.FormClosed += new FormClosedEventHandler(add_flow_closed);
        }
        private void add_flow_closed(Object sender, FormClosedEventArgs e)
        {
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetCompositeNames().ToList());
            RefreshStatsUI();
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected composite */
        private void removeSelectedComposite_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedComposite == null) return;
            for (int i = 0; i < CurrentInstance.commandsPAK.EntryPoints.Count(); i++)
            {
                if (CurrentInstance.selectedComposite.shortGUID == CurrentInstance.commandsPAK.EntryPoints[i].shortGUID)
                {
                    MessageBox.Show("Cannot delete a composite which is set as an entry point!", "Cannot delete.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (!ConfirmAction("Are you sure you want to remove this composite?")) return;

            //Remove any entities or links that reference this composite
            for (int i = 0; i < CurrentInstance.commandsPAK.Composites.Count; i++)
            {
                List<FunctionEntity> prunedFunctionEntities = new List<FunctionEntity>();
                for (int x = 0; x < CurrentInstance.commandsPAK.Composites[i].functions.Count; x++)
                {
                    if (CurrentInstance.commandsPAK.Composites[i].functions[x].function == CurrentInstance.selectedComposite.shortGUID) continue;
                    List<CathodeEntityLink> prunedEntityLinks = new List<CathodeEntityLink>();
                    for (int l = 0; l < CurrentInstance.commandsPAK.Composites[i].functions[x].childLinks.Count; l++)
                    {
                        CathodeEntity linkedEntity = CurrentInstance.commandsPAK.Composites[i].GetEntityByID(CurrentInstance.commandsPAK.Composites[i].functions[x].childLinks[l].childID);
                        if (linkedEntity != null && linkedEntity.variant == EntityVariant.FUNCTION) if (((FunctionEntity)linkedEntity).function == CurrentInstance.selectedComposite.shortGUID) continue;
                        prunedEntityLinks.Add(CurrentInstance.commandsPAK.Composites[i].functions[x].childLinks[l]);
                    }
                    CurrentInstance.commandsPAK.Composites[i].functions[x].childLinks = prunedEntityLinks;
                    prunedFunctionEntities.Add(CurrentInstance.commandsPAK.Composites[i].functions[x]);
                }
                CurrentInstance.commandsPAK.Composites[i].functions = prunedFunctionEntities;
            }
            //TODO: remove proxies etc that also reference any of the removed entities

            //Remove the composite
            CurrentInstance.commandsPAK.Composites.Remove(CurrentInstance.selectedComposite);

            //Refresh UI
            ClearUI(false, true, true);
            RefreshStatsUI();
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetCompositeNames().ToList());
        }

        /* Select entity from loaded composite */
        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (composite_content.SelectedIndex == -1 || CurrentInstance.selectedComposite == null) return;
            try
            {
                cGUID entityID = new cGUID(composite_content.SelectedItem.ToString().Substring(1, 11));
                CathodeEntity thisEntity = CurrentInstance.selectedComposite.GetEntityByID(entityID);
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
            if (CurrentInstance.selectedComposite == null || CurrentInstance.selectedEntity == null) return;
            CathodeEditorGUI_AddPin add_pin = new CathodeEditorGUI_AddPin(CurrentInstance.selectedEntity, CurrentInstance.selectedComposite);
            add_pin.Show();
            add_pin.FormClosed += new FormClosedEventHandler(add_pin_closed);
        }
        private void add_pin_closed(Object sender, FormClosedEventArgs e)
        {
            RefreshEntityLinks();
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected out pin */
        private void removeSelectedLink_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null || CurrentInstance.selectedComposite == null) return;
            if (!ConfirmAction("Are you sure you want to remove this link?")) return;
            if (currentlyShowingChildLinks)
            {
                CurrentInstance.selectedEntity.childLinks.RemoveAt(entity_children.SelectedIndex);
            }
            else
            {
                List<CathodeEntity> ents = CurrentInstance.selectedComposite.GetEntities();
                int deleteIndex = -1;
                foreach (CathodeEntity entity in ents)
                {
                    for (int i = 0; i < entity.childLinks.Count; i++)
                    {
                        if (entity.childLinks[i].connectionID == linkedEntityListIDs[entity_children.SelectedIndex])
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
            RefreshEntityLinks();
        }

        /* Go to selected pin out on button press */
        private void out_pin_goto_Click(object sender, EventArgs e)
        {
            if (entity_children.SelectedIndex == -1 || CurrentInstance.selectedComposite == null) return;

            CathodeEntity thisEntity = null;
            if (currentlyShowingChildLinks)
            {
                thisEntity = CurrentInstance.selectedComposite.GetEntityByID(CurrentInstance.selectedEntity.childLinks[entity_children.SelectedIndex].childID);
            }
            else
            {
                List<CathodeEntity> ents = CurrentInstance.selectedComposite.GetEntities();
                foreach (CathodeEntity entity in ents)
                {
                    for (int i = 0; i < entity.childLinks.Count; i++)
                    {
                        if (entity.childLinks[i].connectionID == linkedEntityListIDs[entity_children.SelectedIndex])
                        {
                            thisEntity = entity;
                            break;
                        }
                    }
                    if (thisEntity != null) break;
                }
            }
            if (thisEntity != null) LoadEntity(thisEntity);
        }

        /* Flip the child link list to contain parents (this is an expensive search, which is why we only do it on request) */
        private void showLinkParents_Click(object sender, EventArgs e)
        {
            currentlyShowingChildLinks = !currentlyShowingChildLinks;
            RefreshEntityLinks();
        }

        /* Search entity list */
        private string currentSearch = "";
        private void entity_search_btn_Click(object sender, EventArgs e)
        {
            if (entity_search_box.Text == currentSearch) return;
            List<string> matched = new List<string>();
            foreach (string item in composite_content_RAW) if (item.ToUpper().Contains(entity_search_box.Text.ToUpper())) matched.Add(item);
            composite_content.BeginUpdate();
            composite_content.Items.Clear();
            for (int i = 0; i < matched.Count; i++) composite_content.Items.Add(matched[i]);
            composite_content.EndUpdate();
            currentSearch = entity_search_box.Text;
        }

        /* Load a composite into the UI */
        List<string> composite_content_RAW = new List<string>();
        private void LoadComposite(string FileName)
        {
            ClearUI(false, true, true);
            CathodeComposite entry = CurrentInstance.commandsPAK.Composites[CurrentInstance.commandsPAK.GetFileIndex(FileName)];
            CurrentInstance.selectedComposite = entry;
            EditorUtils.PurgeDeadHierarchiesInActiveComposite();
            Cursor.Current = Cursors.WaitCursor;

            composite_content.BeginUpdate();
            List<CathodeEntity> entities = entry.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                string desc = EditorUtils.GenerateEntityName(entities[i], CurrentInstance.selectedComposite);
                composite_content.Items.Add(desc);
                composite_content_RAW.Add(desc);
            }
            composite_content.EndUpdate();

//#if debug //TODO: PULL THIS INTO STABLE
            editCompositeResources.Visible = true;
//#endif

            groupBox1.Text = entry.name;
            Cursor.Current = Cursors.Default;
        }

        /* Add new entity */
        private void addNewEntity_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedComposite == null) return;
            CathodeEditorGUI_AddEntity add_parameter = new CathodeEditorGUI_AddEntity(CurrentInstance.selectedComposite, CurrentInstance.commandsPAK.Composites);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(add_entity_closed);
        }
        private void add_entity_closed(Object sender, FormClosedEventArgs e)
        {
            ReloadUIForNewEntity(((CathodeEditorGUI_AddEntity)sender).NewEntity);
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected entity */
        private void removeSelectedEntity_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            if (!ConfirmAction("Are you sure you want to remove this entity?")) return;

            string removedID = CurrentInstance.selectedEntity.shortGUID.ToString();

            switch (CurrentInstance.selectedEntity.variant)
            {
                case EntityVariant.DATATYPE:
                    CurrentInstance.selectedComposite.datatypes.Remove((DatatypeEntity)CurrentInstance.selectedEntity);
                    break;
                case EntityVariant.FUNCTION:
                    CurrentInstance.selectedComposite.functions.Remove((FunctionEntity)CurrentInstance.selectedEntity);
                    break;
                case EntityVariant.OVERRIDE:
                    CurrentInstance.selectedComposite.overrides.Remove((OverrideEntity)CurrentInstance.selectedEntity);
                    break;
                case EntityVariant.PROXY:
                    CurrentInstance.selectedComposite.proxies.Remove((ProxyEntity)CurrentInstance.selectedEntity);
                    break;
                case EntityVariant.NOT_SETUP:
                    CurrentInstance.selectedComposite.unknowns.Remove(CurrentInstance.selectedEntity);
                    break;
            }

            List<CathodeEntity> entities = CurrentInstance.selectedComposite.GetEntities();
            for (int i = 0; i < entities.Count; i++) //We should actually query every entity in the PAK, since we might be ref'd by a proxy or override
            {
                List<CathodeEntityLink> entLinks = new List<CathodeEntityLink>();
                for (int x = 0; x < entities[i].childLinks.Count; x++)
                {
                    if (entities[i].childLinks[x].childID != CurrentInstance.selectedEntity.shortGUID) entLinks.Add(entities[i].childLinks[x]);
                }
                entities[i].childLinks = entLinks;

                if (entities[i].variant == EntityVariant.FUNCTION)
                {
                    string entType = EntityDB.GetCathodeName(((FunctionEntity)entities[i]).function);
                    switch (entType)
                    {
                        case "TriggerSequence":
                            TriggerSequence triggerSequence = (TriggerSequence)entities[i];
                            List<TEMP_TriggerSequenceExtraDataHolder1> triggers = new List<TEMP_TriggerSequenceExtraDataHolder1>();
                            for (int x = 0; x < triggerSequence.triggers.Count; x++)
                            {
                                if (triggerSequence.triggers[x].hierarchy.Count < 2 ||
                                    triggerSequence.triggers[x].hierarchy[triggerSequence.triggers[x].hierarchy.Count - 2] != CurrentInstance.selectedEntity.shortGUID)
                                {
                                    triggers.Add(triggerSequence.triggers[x]);
                                }
                            }
                            triggerSequence.triggers = triggers;
                            break;
                        case "CAGEAnimation":
                            CAGEAnimation cageAnim = (CAGEAnimation)entities[i];
                            List<CathodeParameterKeyframeHeader> headers = new List<CathodeParameterKeyframeHeader>();
                            for (int x = 0; x < cageAnim.keyframeHeaders.Count; x++)
                            {
                                if (cageAnim.keyframeHeaders[x].connectedEntity.Count < 2 ||
                                    cageAnim.keyframeHeaders[x].connectedEntity[cageAnim.keyframeHeaders[x].connectedEntity.Count - 2] != CurrentInstance.selectedEntity.shortGUID)
                                {
                                    headers.Add(cageAnim.keyframeHeaders[x]);
                                }
                            }
                            cageAnim.keyframeHeaders = headers;
                            break;
                    }
                }
            }

            int indexToRemove = -1;
            for (int i = 0; i < composite_content.Items.Count; i++)
            {
                if (composite_content.Items[i].ToString().Substring(1, 11) == removedID)
                {
                    indexToRemove = i;
                    break;
                }
            }
            if (indexToRemove != -1) composite_content.Items.RemoveAt(indexToRemove);
            indexToRemove = -1;
            for (int i = 0; i < composite_content_RAW.Count; i++)
            {
                if (composite_content_RAW[i].Substring(1, 11) == removedID)
                {
                    indexToRemove = i;
                    break;
                }
            }
            if (indexToRemove != -1) composite_content_RAW.RemoveAt(indexToRemove);
            else LoadComposite(CurrentInstance.selectedComposite.name);

            ClearUI(false, false, true);
        }

        /* Remove selected entity when DELETE key is pressed in composite */
        private void composite_content_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                removeSelectedEntity.PerformClick();
            }
        }

        /* Duplicate selected entity */
        private void duplicateSelectedEntity_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            if (!ConfirmAction("Are you sure you want to duplicate this entity?")) return;

            //Generate new entity ID and name
            CathodeEntity newEnt = Utilities.CloneObject(CurrentInstance.selectedEntity);
            newEnt.shortGUID = Utilities.GenerateGUID(DateTime.Now.ToString("G"));
            EntityDBEx.AddNewEntityName(newEnt.shortGUID, EntityDBEx.GetEntityName(CurrentInstance.selectedEntity.shortGUID) + "_clone");

            //Add parent links in to this entity that linked in to the other entity
            List<CathodeEntity> ents = CurrentInstance.selectedComposite.GetEntities();
            List<CathodeEntityLink> newLinks = new List<CathodeEntityLink>();
            int num_of_new_things = 1;
            foreach (CathodeEntity entity in ents)
            {
                newLinks.Clear();
                foreach (CathodeEntityLink link in entity.childLinks)
                {
                    if (link.childID == CurrentInstance.selectedEntity.shortGUID)
                    {
                        CathodeEntityLink newLink = new CathodeEntityLink();
                        newLink.connectionID = Utilities.GenerateGUID(DateTime.Now.ToString("G") + num_of_new_things.ToString()); num_of_new_things++;
                        newLink.childID = newEnt.shortGUID;
                        newLink.childParamID = link.childParamID;
                        newLink.parentParamID = link.parentParamID;
                        newLinks.Add(newLink);
                    }
                }
                if (newLinks.Count != 0) entity.childLinks.AddRange(newLinks);
            }

            //Save back to composite
            switch (newEnt.variant)
            {
                case EntityVariant.FUNCTION:
                    CurrentInstance.selectedComposite.functions.Add((FunctionEntity)newEnt);
                    break;
                case EntityVariant.DATATYPE:
                    CurrentInstance.selectedComposite.datatypes.Add((DatatypeEntity)newEnt);
                    break;
                case EntityVariant.PROXY:
                    CurrentInstance.selectedComposite.proxies.Add((ProxyEntity)newEnt);
                    break;
                case EntityVariant.OVERRIDE:
                    CurrentInstance.selectedComposite.overrides.Add((OverrideEntity)newEnt);
                    break;
                case EntityVariant.NOT_SETUP:
                    CurrentInstance.selectedComposite.unknowns.Add(newEnt);
                    break;
            }

            //Load in to UI
            ReloadUIForNewEntity(newEnt);
        }

        /* Rename selected entity */
        private void renameSelectedEntity_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity == null) return;
            CathodeEditorGUI_RenameEntity rename_entity = new CathodeEditorGUI_RenameEntity(CurrentInstance.selectedEntity.shortGUID);
            rename_entity.Show();
            rename_entity.FormClosed += new FormClosedEventHandler(rename_entity_closed);
        }
        private void rename_entity_closed(Object sender, FormClosedEventArgs e)
        {
            if (((CathodeEditorGUI_RenameEntity)sender).didSave &&
                ((CathodeEditorGUI_RenameEntity)sender).EntityID == CurrentInstance.selectedEntity.shortGUID)
            {
                EntityDBEx.AddNewEntityName(CurrentInstance.selectedEntity.shortGUID, ((CathodeEditorGUI_RenameEntity)sender).EntityName);
                string entityID = CurrentInstance.selectedEntity.shortGUID.ToString();
                string newEntityName = EditorUtils.GenerateEntityName(CurrentInstance.selectedEntity, CurrentInstance.selectedComposite);
                for (int i = 0; i < composite_content.Items.Count; i++)
                {
                    if (composite_content.Items[i].ToString().Substring(1, 11) == entityID)
                    {
                        composite_content.Items[i] = newEntityName;
                        break;
                    }
                }
                for (int i = 0; i < composite_content_RAW.Count; i++)
                {
                    if (composite_content_RAW[i].Substring(1, 11) == entityID)
                    {
                        composite_content_RAW[i] = newEntityName;
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
            if (CurrentInstance.selectedComposite == null || newEnt == null) return;
            if (currentSearch == "")
            {
                string newEntityName = EditorUtils.GenerateEntityName(newEnt, CurrentInstance.selectedComposite);
                composite_content.Items.Add(newEntityName);
                composite_content_RAW.Add(newEntityName);
            }
            else
            {
                LoadComposite(CurrentInstance.selectedComposite.name);
            }
            LoadEntity(newEnt);
        }

        /* Load a entity into the UI */
        private void LoadEntity(CathodeEntity entity)
        {
            if (entity == null) return;

            ClearUI(false, false, true);
            CurrentInstance.selectedEntity = entity;
            Cursor.Current = Cursors.WaitCursor;

            //populate info labels
            selected_entity_id.Text = entity.shortGUID.ToString();
            selected_entity_type.Text = entity.variant.ToString();
            string description = "";
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    description = EntityDBEx.GetParameterName(((FunctionEntity)entity).function);
                    jumpToComposite.Visible = (CurrentInstance.commandsPAK.GetComposite(((FunctionEntity)entity).function) != null);
                    selected_entity_name.Text = EntityDBEx.GetEntityName(entity.shortGUID);
//#if debug //TODO: PULL THIS INTO STABLE
                    editTriggerSequence.Visible = description == "TriggerSequence";
                    editCAGEAnimationKeyframes.Visible = description == "CAGEAnimation";
//#endif
                    break;
                case EntityVariant.DATATYPE:
                    description = "DataType " + ((DatatypeEntity)entity).type.ToString();
                    selected_entity_name.Text = EntityDBEx.GetParameterName(((DatatypeEntity)entity).parameter);
                    break;
                case EntityVariant.PROXY:
                case EntityVariant.OVERRIDE:
                    jumpToComposite.Visible = true;
                    selected_entity_name.Text = EntityDBEx.GetEntityName(entity.shortGUID);
                    break;
                default:
                    selected_entity_name.Text = EntityDBEx.GetEntityName(entity.shortGUID);
                    break;
            }
            selected_entity_type_description.Text = description;

            //show resource editor button if this entity has a resource reference
            cGUID resourceParamID = Utilities.GenerateGUID("resource");
            CathodeLoadedParameter resourceParam = CurrentInstance.selectedEntity.parameters.FirstOrDefault(o => o.shortGUID == resourceParamID);
//#if debug //TODO: PULL THIS INTO STABLE
            editEntityResources.Visible = ((resourceParam != null) || CurrentInstance.selectedEntity.resources.Count != 0);
//#endif

            //populate parameter inputs
            int current_ui_offset = 7;
            for (int i = 0; i < entity.parameters.Count; i++)
            {
                if (entity.parameters[i].shortGUID == resourceParamID) continue; //We use the resource editor button (above) for resource parameters

                CathodeParameter this_param = entity.parameters[i].content;
                UserControl parameterGUI = null;

                switch (this_param.dataType)
                {
                    case CathodeDataType.POSITION:
                        parameterGUI = new GUI_TransformDataType();
                        ((GUI_TransformDataType)parameterGUI).PopulateUI((CathodeTransform)this_param, entity.parameters[i].shortGUID);
                        break;
                    case CathodeDataType.INTEGER:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Int((CathodeInteger)this_param, entity.parameters[i].shortGUID);
                        break;
                    case CathodeDataType.STRING:
                        parameterGUI = new GUI_StringDataType();
                        ((GUI_StringDataType)parameterGUI).PopulateUI((CathodeString)this_param, entity.parameters[i].shortGUID);
                        break;
                    case CathodeDataType.BOOL:
                        parameterGUI = new GUI_BoolDataType();
                        ((GUI_BoolDataType)parameterGUI).PopulateUI((CathodeBool)this_param, entity.parameters[i].shortGUID);
                        break;
                    case CathodeDataType.FLOAT:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Float((CathodeFloat)this_param, entity.parameters[i].shortGUID);
                        break;
                    case CathodeDataType.DIRECTION:
                        parameterGUI = new GUI_VectorDataType();
                        ((GUI_VectorDataType)parameterGUI).PopulateUI((CathodeVector3)this_param, entity.parameters[i].shortGUID);
                        break;
                    case CathodeDataType.ENUM:
                        parameterGUI = new GUI_EnumDataType();
                        ((GUI_EnumDataType)parameterGUI).PopulateUI((CathodeEnum)this_param, entity.parameters[i].shortGUID);
                        break;
                    case CathodeDataType.SHORT_GUID:
                        parameterGUI = new GUI_HexDataType();
                        ((GUI_HexDataType)parameterGUI).PopulateUI((CathodeResource)this_param, entity.parameters[i].shortGUID, CurrentInstance.selectedComposite); 
                        break;
                    case CathodeDataType.SPLINE_DATA:
                        parameterGUI = new GUI_SplineDataType();
                        ((GUI_SplineDataType)parameterGUI).PopulateUI((CathodeSpline)this_param, entity.parameters[i].shortGUID);
                        break;
                }

                parameterGUI.Location = new Point(15, current_ui_offset);
                current_ui_offset += parameterGUI.Height + 6;
                entity_params.Controls.Add(parameterGUI);
            }

            RefreshEntityLinks();

            Cursor.Current = Cursors.Default;
        }

        /* Refresh child/parent links for selected entity */
        List<cGUID> linkedEntityListIDs = new List<cGUID>();
        private void RefreshEntityLinks()
        {
            entity_children.BeginUpdate();
            entity_children.Items.Clear();
            linkedEntityListIDs.Clear();
            addNewLink.Enabled = currentlyShowingChildLinks;
            //removeSelectedLink.Enabled = currentlyShowingChildLinks;
            //out_pin_goto.Enabled = currentlyShowingChildLinks;
            showLinkParents.Text = (currentlyShowingChildLinks) ? "Parents" : "Children";

            if (CurrentInstance.selectedComposite == null || CurrentInstance.selectedEntity == null) return;
            if (currentlyShowingChildLinks)
            {
                //Child links (pins out of this entity)
                foreach (CathodeEntityLink link in CurrentInstance.selectedEntity.childLinks)
                {
                    entity_children.Items.Add(
                        /*"[" + link.connectionID.ToString() + "] " +*/
                        "(" + EntityDBEx.GetParameterName(link.parentParamID) + ") => " +
                        EntityDBEx.GetEntityName(link.childID) + 
                        " (" + EntityDBEx.GetParameterName(link.childParamID) + ")");
                    linkedEntityListIDs.Add(link.connectionID);
                }
            }
            else
            {
                //Parent links (pins into this entity)
                List<CathodeEntity> ents = CurrentInstance.selectedComposite.GetEntities();
                foreach (CathodeEntity entity in ents)
                {
                    foreach (CathodeEntityLink link in entity.childLinks)
                    {
                        if (link.childID != CurrentInstance.selectedEntity.shortGUID) continue;
                        entity_children.Items.Add(
                            /*"[" + link.connectionID.ToString() + "] " +*/
                            EntityDBEx.GetEntityName(entity.shortGUID) +
                            " (" + EntityDBEx.GetParameterName(link.parentParamID) + ") => " +
                            "(" + EntityDBEx.GetParameterName(link.childParamID) + ")");
                        linkedEntityListIDs.Add(link.connectionID);
                    }
                }
            }
            entity_children.EndUpdate();
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

        /* Edit a TriggerSequence */
        private void editTriggerSequence_Click(object sender, EventArgs e)
        {
            TriggerSequenceEditor keyframeEditor = new TriggerSequenceEditor((TriggerSequence)CurrentInstance.selectedEntity);
            keyframeEditor.Show();
        }

        /* Edit CAGEAnimation keyframes */
        private void editCAGEAnimationKeyframes_Click(object sender, EventArgs e)
        {
            CAGEAnimationEditor keyframeEditor = new CAGEAnimationEditor((CAGEAnimation)CurrentInstance.selectedEntity);
            keyframeEditor.Show();
        }

        /* Edit resources referenced by the entity */
        private void editEntityResources_Click(object sender, EventArgs e)
        {
            //CurrentInstance.currentEntity - .parameters.FirstOrDefault("resources") - .resources
            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(CurrentInstance.selectedEntity);
            resourceEditor.Show();
        }

        /* Edit resources referenced by the composite */
        private void editCompositeResources_Click(object sender, EventArgs e)
        {
            //CurrentInstance.currentComposite.resources
            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(CurrentInstance.selectedComposite);
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
            if (CurrentInstance.selectedComposite == null)
            {
                for (int mm = 0; mm < env_list.Items.Count; mm++)
                {
                    CurrentInstance.commandsPAK = new CommandsPAK(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + env_list.Items[mm].ToString() + "/WORLD/COMMANDS.PAK");
                    EntityDBEx.LoadNames();
                    foreach (CathodeComposite flow in CurrentInstance.commandsPAK.Composites)
                    {
                        CurrentInstance.selectedComposite = flow;
                        EditorUtils.PurgeDeadHierarchiesInActiveComposite();
                    }
                    CurrentInstance.commandsPAK.Save();
                }
                return;
            }

            foreach (CathodeComposite flow in CurrentInstance.commandsPAK.Composites)
            {
                CurrentInstance.selectedComposite = flow;
                EditorUtils.PurgeDeadHierarchiesInActiveComposite();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (CathodeComposite flow in CurrentInstance.commandsPAK.Composites)
            {
                flow.unknownPair = new OffsetPair(0, 0);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedComposite == null) return;
            Console.WriteLine(CurrentInstance.selectedComposite.name);
            foreach (OverrideEntity overrider in CurrentInstance.selectedComposite.overrides)
            {
                Console.WriteLine(EntityDBEx.GetEntityName(overrider.shortGUID));
                Console.WriteLine(EditorUtils.HierarchyToString(overrider.hierarchy));
                Console.WriteLine("-----");
            }
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
