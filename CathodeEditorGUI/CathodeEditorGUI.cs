using CATHODE;
using CATHODE.Assets;
using CATHODE.Commands;
using CATHODE.LEGACY;
using CATHODE.Misc;
using CathodeEditorGUI.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

            this.Text = "OpenCAGE Cathode Editor";
            if (OpenCAGE.SettingsManager.GetBool("CONFIG_ShowPlatform") &&
                OpenCAGE.SettingsManager.GetString("META_GameVersion") != "")
            {
                switch (OpenCAGE.SettingsManager.GetString("META_GameVersion"))
                {
                    case "STEAM":
                        this.Text += " - Steam";
                        break;
                    case "EPIC_GAMES_STORE":
                        this.Text += " - Epic Games Store";
                        break;
                    case "GOG":
                        this.Text += " - GoG";
                        break;
                }
            }

            ClearUI(true, true, true);

#if DEBUG
            /*
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            */
#endif
        }

        /* Clear the UI */
        private void ClearUI(bool clear_composite_list, bool clear_entity_list, bool clear_parameter_list)
        {
            if (clear_composite_list)
            {
                FileTree.BeginUpdate();
                FileTree.Nodes.Clear();
                FileTree.EndUpdate();
                root_composite_display.Text = "Root composite: ";
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
                entityInfoGroup.Text = "Selected Entity Info";
                selected_entity_type_description.Text = "";
                selected_entity_name.Text = "";
                for (int i = 0; i < entity_params.Controls.Count; i++) 
                    entity_params.Controls[i].Dispose();
                entity_params.Controls.Clear();
                entity_children.Items.Clear();
                currentlyShowingChildLinks = true;
                jumpToComposite.Visible = false;
                editFunction.Enabled = false;
                editEntityResources.Enabled = false;
                editEntityMovers.Enabled = false;
                renameSelectedNode.Enabled = true;
                duplicateSelectedNode.Enabled = true;
                hierarchyDisplay.Visible = false;
            }
        }

        /* Enable the option to load COMMANDS */
        public void EnableLoadingOfPaks(bool shouldEnable)
        {
            load_commands_pak.Invoke(new Action(() => { load_commands_pak.Enabled = shouldEnable; load_commands_pak.Text = (shouldEnable) ? "Load" : "Caching..."; }));
            env_list.Invoke(new Action(() => { env_list.Enabled = shouldEnable; }));
        }

        /* Load a COMMANDS.PAK into the editor with additional stuff */
        Task currentBackgroundCacher = null;
        public void LoadCommandsPAK(string level)
        {
            //Reset UI
            ClearUI(true, true, true);
            EditorUtils.ResetHierarchyPurgeCache();

            //Load everything
            LoadCommands(level);
            /*Task.Factory.StartNew(() => */LoadAssets()/*)*/;
            /*Task.Factory.StartNew(() => */LoadMovers()/*)*/;

            //Begin caching entity names so we don't have to keep generating them
            CurrentInstance.compositeLookup = new EntityNameLookup(CurrentInstance.commandsPAK);
            if (currentBackgroundCacher != null) currentBackgroundCacher.Dispose();
            currentBackgroundCacher = Task.Factory.StartNew(() => EditorUtils.GenerateEntityNameCache(this));

            //Populate file tree
            treeHelper.UpdateFileTree(CurrentInstance.commandsPAK.GetCompositeNames().ToList());

            //Show info in UI
            RefreshStatsUI();

            //Load root composite
            treeHelper.SelectNode(CurrentInstance.commandsPAK.EntryPoints[0].name);
        }
        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            LoadCommandsPAK(env_list.SelectedItem.ToString());
        }
        private void RefreshStatsUI()
        {
            root_composite_display.Text = "Root composite: " + CurrentInstance.commandsPAK.EntryPoints[0].name;
            composite_count_display.Text = "Composite count: " + CurrentInstance.commandsPAK.Composites.Count;
        }

        /* Load commands */
        private void LoadCommands(string level)
        {
            CurrentInstance.commandsPAK = null;

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
        }

        /* Load assets */
        private void LoadAssets()
        {
            try
            {
                string baseLevelPath = CurrentInstance.commandsPAK.Filepath.Substring(0, CurrentInstance.commandsPAK.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);
                CurrentInstance.modelDB = new CathodeModels(baseLevelPath + "RENDERABLE/MODELS_LEVEL.BIN",
                                                            baseLevelPath + "RENDERABLE/LEVEL_MODELS.PAK");
                CurrentInstance.redsDB = new RenderableElementsDatabase(baseLevelPath + "WORLD/REDS.BIN");
                CurrentInstance.materialDB = new MaterialDatabase(baseLevelPath + "RENDERABLE/LEVEL_MODELS.MTL");
                CurrentInstance.textureDB = new Textures(baseLevelPath + "RENDERABLE/LEVEL_TEXTURES.ALL.PAK");
                CurrentInstance.textureDB.Load();
                CurrentInstance.textureDB_Global = new Textures(SharedData.pathToAI + "/DATA/ENV/GLOBAL/WORLD/GLOBAL_TEXTURES.ALL.PAK");
                CurrentInstance.textureDB_Global.Load();
            }
            catch
            {
                //Can fail if we're loading a PAK outside the game structure
                MessageBox.Show("Failed to load asset PAKs!\nAre you opening a Commands PAK outside of a map directory?\nResource editing disabled.", "Resource editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CurrentInstance.modelDB = null;
                CurrentInstance.redsDB = null;
                CurrentInstance.materialDB = null;
                CurrentInstance.textureDB = null;
                CurrentInstance.textureDB_Global = null;
            }
        }

        /* Load mover descriptors */
        private void LoadMovers()
        {
            try
            {
                string baseLevelPath = CurrentInstance.commandsPAK.Filepath.Substring(0, CurrentInstance.commandsPAK.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);
                CurrentInstance.moverDB = new MoverDatabase(baseLevelPath + "WORLD/MODELS.MVR");
            }
            catch
            {
                //Can fail if we're loading a MVR outside the game structure
                MessageBox.Show("Failed to load mover descriptor database!\nAre you opening a Commands PAK outside of a map directory?\nMVR editing disabled.", "MVR editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CurrentInstance.moverDB = null;
            }
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

            if (CurrentInstance.redsDB != null && CurrentInstance.redsDB.RenderableElements != null)
                CurrentInstance.redsDB.Save();

            if (CurrentInstance.moverDB != null && CurrentInstance.moverDB.Movers != null)
                CurrentInstance.moverDB.Save();

            Cursor.Current = Cursors.Default;
        }
        private void save_commands_pak_Click(object sender, EventArgs e)
        {
            SaveCommandsPAK();
            MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private CathodeComposite GetCompositeContainingEntity(ShortGuid entityID)
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

        /* Edit the loaded COMMANDS.PAK's root composite */
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
                    CathodeEntity entity = EditorUtils.ResolveHierarchy(((OverrideEntity)CurrentInstance.selectedEntity).hierarchy, out flow, out string hierarchy);
                    if (entity != null)
                    {
                        LoadComposite(flow.name);
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    CathodeEntity entity = EditorUtils.ResolveHierarchy(((ProxyEntity)CurrentInstance.selectedEntity).hierarchy, out flow, out string hierarchy);
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

        /* Select root composite from top of UI */
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
                    MessageBox.Show("Cannot delete a composite which is the root, global, or pause menu!", "Cannot delete.", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                ShortGuid entityID = new ShortGuid(composite_content.SelectedItem.ToString().Substring(1, 11));
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

            if (CurrentInstance.textureDB != null && entry.resources.Count != 0)
                editCompositeResources.Visible = true;

            groupBox1.Text = entry.name;
            Cursor.Current = Cursors.Default;
        }

        /* Add new entity */
        private void addNewEntity_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedComposite == null) return;
            CathodeEditorGUI_AddEntity add_parameter = new CathodeEditorGUI_AddEntity(CurrentInstance.selectedComposite, CurrentInstance.commandsPAK.Composites);
            add_parameter.Show();
            add_parameter.OnNewEntity += OnAddNewEntity;
        }
        private void OnAddNewEntity(CathodeEntity entity)
        {
            ReloadUIForNewEntity(entity);
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
                    string entType = ShortGuidUtils.FindString(((FunctionEntity)entities[i]).function);
                    switch (entType)
                    {
                        case "TriggerSequence":
                            TriggerSequence triggerSequence = (TriggerSequence)entities[i];
                            List<CathodeTriggerSequenceTrigger> triggers = new List<CathodeTriggerSequenceTrigger>();
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
            newEnt.shortGUID = ShortGuidUtils.Generate(DateTime.Now.ToString("G"));
            CurrentInstance.compositeLookup.SetEntityName(
                CurrentInstance.selectedComposite.shortGUID,
                newEnt.shortGUID,
                CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, CurrentInstance.selectedEntity.shortGUID) + "_clone");

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
                        newLink.connectionID = ShortGuidUtils.Generate(DateTime.Now.ToString("G") + num_of_new_things.ToString()); num_of_new_things++;
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
                CurrentInstance.compositeLookup.SetEntityName(
                    CurrentInstance.selectedComposite.shortGUID,
                    CurrentInstance.selectedEntity.shortGUID,
                    ((CathodeEditorGUI_RenameEntity)sender).EntityName);

                string entityID = CurrentInstance.selectedEntity.shortGUID.ToString();
                string newEntityName = EditorUtils.GenerateEntityName(CurrentInstance.selectedEntity, CurrentInstance.selectedComposite, true);
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
            entityInfoGroup.Text = "Selected " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.variant.ToString().ToLower().Replace('_', ' ')) + " Info";
            string description = "";
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    ShortGuid thisFunction = ((FunctionEntity)entity).function;
                    CathodeComposite funcComposite = CurrentInstance.commandsPAK.GetComposite(thisFunction);
                    jumpToComposite.Visible = funcComposite != null;
                    if (funcComposite != null)
                        description = funcComposite.name;
                    else
                        description = ShortGuidUtils.FindString(((FunctionEntity)entity).function);
                    selected_entity_name.Text = CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, entity.shortGUID);
                    if (funcComposite == null)
                    {
                        CathodeFunctionType function = CommandsUtils.GetFunctionType(thisFunction);
                        editFunction.Enabled = function == CathodeFunctionType.CAGEAnimation || function == CathodeFunctionType.TriggerSequence;
                    }
                    break;
                case EntityVariant.DATATYPE:
                    description = "DataType " + ((DatatypeEntity)entity).type.ToString();
                    selected_entity_name.Text = ShortGuidUtils.FindString(((DatatypeEntity)entity).parameter);
                    renameSelectedNode.Enabled = false;
                    duplicateSelectedNode.Enabled = false;
                    break;
                case EntityVariant.PROXY:
                case EntityVariant.OVERRIDE:
                    hierarchyDisplay.Visible = true;
                    string hierarchy = "";
                    if (entity.variant == EntityVariant.PROXY) EditorUtils.ResolveHierarchy(((ProxyEntity)entity).hierarchy, out CathodeComposite comp, out hierarchy);
                    else EditorUtils.ResolveHierarchy(((OverrideEntity)entity).hierarchy, out CathodeComposite comp, out hierarchy);
                    hierarchyDisplay.Text = hierarchy;
                    jumpToComposite.Visible = true;
                    selected_entity_name.Text = CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, entity.shortGUID);
                    break;
                default:
                    selected_entity_name.Text = CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, entity.shortGUID);
                    break;
            }
            selected_entity_type_description.Text = description;

            //show resource editor button if this entity has a resource reference
            ShortGuid resourceParamID = ShortGuidUtils.Generate("resource");
            CathodeLoadedParameter resourceParam = CurrentInstance.selectedEntity.parameters.FirstOrDefault(o => o.shortGUID == resourceParamID);
            if (CurrentInstance.textureDB != null)
                editEntityResources.Enabled = ((resourceParam != null) || CurrentInstance.selectedEntity.resources.Count != 0);
            if (CurrentInstance.moverDB != null && CurrentInstance.moverDB.Movers.FindAll(o => o.commandsNodeID == CurrentInstance.selectedEntity.shortGUID).Count != 0)
                editEntityMovers.Enabled = true;

            //sort parameters by type, to improve visibility in UI
            entity.parameters = entity.parameters.OrderBy(o => o.content?.dataType).ToList();

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
        List<ShortGuid> linkedEntityListIDs = new List<ShortGuid>();
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
                        "this [" + ShortGuidUtils.FindString(link.parentParamID) + "] => " +
                        EditorUtils.GenerateEntityNameWithoutID(CurrentInstance.selectedComposite.GetEntities().FirstOrDefault(o => o.shortGUID == link.childID), CurrentInstance.selectedComposite) +
                        " [" + ShortGuidUtils.FindString(link.childParamID) + "]");
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
                            EditorUtils.GenerateEntityNameWithoutID(entity, CurrentInstance.selectedComposite) +
                            " [" + ShortGuidUtils.FindString(link.parentParamID) + "] => " +
                            "this [" + ShortGuidUtils.FindString(link.childParamID) + "]");
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

        /* Edit function entity (CAGEAnimation/TriggerSequence) */
        private void editFunction_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.selectedEntity.variant != EntityVariant.FUNCTION) return;
            string function = ShortGuidUtils.FindString(((FunctionEntity)CurrentInstance.selectedEntity).function);
            switch (function.ToUpper())
            {
                case "CAGEANIMATION":
                    CAGEAnimationEditor cageAnimationEditor = new CAGEAnimationEditor((CAGEAnimation)CurrentInstance.selectedEntity);
                    cageAnimationEditor.Show();
                    cageAnimationEditor.FormClosed += FunctionEditor_FormClosed;
                    break;
                case "TRIGGERSEQUENCE":
                    TriggerSequenceEditor triggerSequenceEditor = new TriggerSequenceEditor((TriggerSequence)CurrentInstance.selectedEntity);
                    triggerSequenceEditor.Show();
                    triggerSequenceEditor.FormClosed += FunctionEditor_FormClosed;
                    break;
            }
        }
        private void FunctionEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();
        }

        /* Edit resources referenced by the entity */
        private void editEntityResources_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(CurrentInstance.selectedEntity, CurrentInstance.selectedComposite);
            resourceEditor.Show();
            resourceEditor.FormClosed += ResourceEditor_FormClosed;
        }
        private void ResourceEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();
        }

        /* Edit mover instances of this entity */
        private void editEntityMovers_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_EditMVR moverEditor = new CathodeEditorGUI_EditMVR(CurrentInstance.selectedEntity.shortGUID);
            moverEditor.Show();
            moverEditor.FormClosed += MoverEditor_FormClosed;
        }
        private void MoverEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();
        }

        /* Edit resources referenced by the composite */
        private void editCompositeResources_Click(object sender, EventArgs e)
        {
            //CurrentInstance.currentComposite.resources
            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(CurrentInstance.selectedComposite);
            resourceEditor.Show();
            resourceEditor.FormClosed += ResourceEditor_FormClosed1;
        }
        private void ResourceEditor_FormClosed1(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();
        }

        /* Confirm an action */
        private bool ConfirmAction(string msg)
        {
            return (MessageBox.Show(msg, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void CathodeEditorGUI_Load(object sender, EventArgs e)
        {
            return;

            MoverDatabase mvr = new MoverDatabase(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\MODELS.MVR");
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

                thisMvr.transform = 
                    System.Numerics.Matrix4x4.CreateScale(new System.Numerics.Vector3(0,0,0)) * 
                    System.Numerics.Matrix4x4.CreateFromQuaternion(System.Numerics.Quaternion.Identity) * 
                    System.Numerics.Matrix4x4.CreateTranslation(new System.Numerics.Vector3(-9999.0f, -9999.0f, -9999.0f));
                //mvr.IsThisTypeID = MoverType.DYNAMIC_MODEL;
                thisMvr.instanceTypeFlags = 6;
                thisMvr.commandsNodeID = new ShortGuid("00-00-00-00");

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

            mvr = new MoverDatabase(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\TECH_HUB\WORLD\MODELS.MVR");
            mvr.Save();

            mvr = new MoverDatabase(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\MODELS.MVR");
            mvr.Movers.RemoveAll(o => o.commandsNodeID != new ShortGuid(0));
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
            CathodeEditorGUI_EditMVR mvrEditor = new CathodeEditorGUI_EditMVR();
            mvrEditor.Show();
            return;

            for (int i = 0; i < CurrentInstance.redsDB.RenderableElements.Count; i++)
            {
                //CurrentInstance.redsDB.RenderableElements[i].ModelIndex = 100;
                CurrentInstance.redsDB.RenderableElements[i].MaterialLibraryIndex -= 1;

                //CurrentInstance.redsDB.RenderableElements[i].ModelLODIndex = -1;
                //CurrentInstance.redsDB.RenderableElements[i].ModelLODPrimitiveCount = 0;
            }
            CurrentInstance.redsDB.Save();
            return;

            CathodeEditorGUI_SelectModel modelthing = new CathodeEditorGUI_SelectModel();
            modelthing.Show();
            return;

            if (CurrentInstance.selectedComposite == null) return;
            Console.WriteLine(CurrentInstance.selectedComposite.name);
            foreach (OverrideEntity overrider in CurrentInstance.selectedComposite.overrides)
            {
                //Console.WriteLine(EntityDBEx.GetEntityName(overrider.shortGUID));
                //Console.WriteLine(EditorUtils.HierarchyToString(overrider.hierarchy));
                //Console.WriteLine("-----");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //CurrentInstance.commandsPAK.Composites.RemoveAll(o => o.name != "GLOBAL" && o.name != "PAUSEMENU" && o.name != CurrentInstance.commandsPAK.EntryPoints[0].name);
            CurrentInstance.commandsPAK.EntryPoints[0].name = "RootComposite";
            CurrentInstance.commandsPAK.Composites.RemoveAll(o => o.name.Length > 3 && o.name.Substring(0, 3) == "AYZ");
            CurrentInstance.commandsPAK.Save();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> output = new List<string>();
            
            BinaryReader reader = new BinaryReader(File.OpenRead(@"E:\OpenCAGE_2021\CathodeEditorGUI\CathodeLib\CathodeLib\Resources\NodeDBs\entity_parameter_names.bin"));
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                reader.BaseStream.Position += 4;
                string bleh = reader.ReadString();
                if (!output.Contains(bleh)) output.Add(bleh);
            }
            reader.Close();
            reader = new BinaryReader(File.OpenRead(@"E:\OpenCAGE_2021\CathodeLib\CathodeLib\Resources\NodeDBs\cathode_id_map.bin"));
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                reader.BaseStream.Position += 4;
                string bleh = reader.ReadString();
                if (!output.Contains(bleh)) output.Add(bleh);
            }
            reader.Close();
            reader = new BinaryReader(File.OpenRead(@"E:\OpenCAGE_2021\CathodeLib\CathodeLib\Resources\NodeDBs\cathode_id_map_DUMP_ONLY.bin"));
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                reader.BaseStream.Position += 4;
                string bleh = reader.ReadString();
                if (!output.Contains(bleh)) output.Add(bleh);
            }
            reader.Close();
            
            List<string> dump1 = Directory.GetFiles(@"E:\GitHub Repos\alien_assets_daniel\DEBUG\commands_bin_dumps", "*", SearchOption.AllDirectories).ToList<string>();
            foreach (string dump in dump1)
            {
                List<string> lines = File.ReadAllLines(dump).ToList<string>();
                foreach (string line in lines)
                {
                    if (line.Length == 0 || line.Substring(0, 1) != "8") continue;
                    if (line.Substring(line.Length - 1) == "'")
                    {
                        List<string> content = line.Split('\'').ToList<string>();
                        if (content.Count > 1)
                        {
                            string name = content[content.Count - 2];
                            if (!output.Contains(name)) output.Add(name);
                        }
                    }
                }
            }

            BinaryWriter writer = new BinaryWriter(File.OpenWrite("binout.bin"));
            foreach (string out1 in output) {
                Utilities.Write<ShortGuid>(writer, ShortGuidUtils.Generate(out1));
                writer.Write(out1);
            };
            writer.Close();
            return;

            /*
            List<string> dumps = Directory.GetFiles(@"E:\GitHub Repos\alien_assets_daniel\DEBUG", "alien_commands_bin_*", SearchOption.AllDirectories).ToList<string>();
            List<Task> tasks = new List<Task>();
            foreach (string dump in dumps)
            {
                tasks.Add(Task.Factory.StartNew(() => ParseCommandBinDump(dump)));
            }
            */

            List<string> allStrings = new List<string>();
            List<string> dumps = Directory.GetFiles(@"E:\OpenCAGE_2021\CathodeEditorGUI\Build", "OUT_STRINGS_*", SearchOption.AllDirectories).ToList<string>();
            foreach (string dump in dumps)
            {
                foreach (string line in File.ReadAllLines(dump))
                {
                    if (allStrings.Contains(line)) continue;
                    allStrings.Add(line);
                }
            }
            File.WriteAllLines("OUT_STRINGS.txt", allStrings);

            /*
            List<string> dumps = Directory.GetFiles(@"E:\OpenCAGE_2021\CathodeEditorGUI\Build\out2", "*.bin", SearchOption.AllDirectories).ToList<string>();
            List<BinComposite> composites = new List<BinComposite>();
            foreach (string dump in dumps)
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(dump));
                int compCount = reader.ReadInt32();
                for (int i = 0; i < compCount; i++)
                {
                    ShortGuid compositeID = Utilities.Consume<ShortGuid>(reader);
                    BinComposite composite = composites.FirstOrDefault(o => o.id == compositeID);
                    if (composite == null)
                    {
                        composite = new BinComposite();
                        composite.id = compositeID;
                        composites.Add(composite);
                    }
                    string thisPath = reader.ReadString();
                    if (composite.path != "" && thisPath != composite.path)
                    {
                        Console.WriteLine("WARNING: " + composite.id + " name mismatch!");
                        Console.WriteLine("         " + composite.path);
                        Console.WriteLine("         " + thisPath);
                    }
                    composite.path = thisPath;
                    int entityCount = reader.ReadInt32();
                    for (int x = 0; x < entityCount; x++)
                    {
                        ShortGuid entityID = Utilities.Consume<ShortGuid>(reader);
                        BinEntity entity = composite.entities.FirstOrDefault(o => o.id == entityID);
                        if (entity == null)
                        {
                            entity = new BinEntity();
                            entity.id = entityID;
                            composite.entities.Add(entity);
                        }
                        string thisName = reader.ReadString();
                        if (entity.name != "" && thisName != entity.name)
                        {
                            Console.WriteLine("WARNING: " + entity.id + " name mismatch!");
                            Console.WriteLine("         " + entity.name);
                            Console.WriteLine("         " + thisName);
                        }
                        entity.name = thisName;
                    }
                }
                reader.Close();
            }
            BinaryWriter writer = new BinaryWriter(File.OpenWrite("out2/COMBINED.bin"));
            writer.BaseStream.SetLength(0);
            writer.Write(composites.Count);
            for (int i = 0; i < composites.Count; i++)
            {
                Utilities.Write<ShortGuid>(writer, composites[i].id);
                //writer.Write(composites[i].path);
                writer.Write(composites[i].entities.Count);
                for (int x = 0; x < composites[i].entities.Count; x++)
                {
                    Utilities.Write<ShortGuid>(writer, composites[i].entities[x].id);
                    writer.Write(composites[i].entities[x].name);
                }
            }
            writer.Close();
            writer = new BinaryWriter(File.OpenWrite("out2/composites_only.bin"));
            writer.BaseStream.SetLength(0);
            writer.Write(composites.Count);
            for (int i = 0; i < composites.Count; i++)
            {
                Utilities.Write<ShortGuid>(writer, composites[i].id);
                writer.Write(composites[i].path);
            }
            writer.Close();
            */
        }
        private void ParseCommandBinDump(string dump)
        {
            List<BinComposite> composites = new List<BinComposite>();
            string[] content = File.ReadAllLines(dump);
            List<string> strings = new List<string>();
            for (int i = 0; i < content.Length; i++)
            {
                string[] split = content[i].Split(new char[] { '\'' }, 2);
                if (split.Length != 2) continue;
                if (split[0].Substring(split[0].Length - 2, 1) != "2") continue;
                string value = split[1].Substring(0, split[1].Length - 1);
                strings.Add(value);
                if (value == "") continue;

                continue;

                ShortGuid compositeID;
                try
                {
                    compositeID = new ShortGuid(content[i].Substring(13, 2) + "-" + content[i].Substring(11, 2) + "-" + content[i].Substring(9, 2) + "-" + content[i].Substring(7, 2));
                }
                catch
                {
                    continue;
                }
                BinComposite composite = composites.FirstOrDefault(o => o.id == compositeID);

                switch (content[i].Substring(0, 1))
                {
                    case "1":
                    {
                        //Composite name definition! First is name, second is path.
                        if (composite == null)
                        {
                            composite = new BinComposite();
                            composite.id = compositeID;
                            composite.name = value;
                            composites.Add(composite);
                        }
                        else
                        {
                            composite.path = value;
                        }
                        break;
                    }
                    case "2":
                    {
                        if (content[i].Length < 28) continue;
                        ShortGuid entityID;
                        try
                        {
                            entityID = new ShortGuid(content[i].Substring(26, 2) + "-" + content[i].Substring(24, 2) + "-" + content[i].Substring(22, 2) + "-" + content[i].Substring(20, 2));
                        }
                        catch
                        {
                            continue;
                        }
                        if (composite == null)
                        {
                            composite = new BinComposite();
                            composite.id = compositeID;
                            composites.Add(composite);
                        }
                        BinEntity entity = composite.entities.FirstOrDefault(o => o.id == entityID);
                        if (entity == null)
                        {
                            entity = new BinEntity();
                            entity.id = entityID;
                            composite.entities.Add(entity);
                        }
                        else if (entity.name != value)
                        {
                            Console.WriteLine("Renaming " + entityID + " from '" + entity.name + "' to '" + value + "'");
                        }
                        entity.name = value;
                        break;
                    }
                    case "5":
                    {
                        if (split[0].Substring(split[0].Length - 13) != "0x58EBAC79 2 ") continue;
                        if (content[i].Length < 28) continue;
                        ShortGuid entityID;
                        try
                        {
                            entityID = new ShortGuid(content[i].Substring(26, 2) + "-" + content[i].Substring(24, 2) + "-" + content[i].Substring(22, 2) + "-" + content[i].Substring(20, 2));
                        }
                        catch
                        {
                            continue;
                        }
                        if (composite == null)
                        {
                            composite = new BinComposite();
                            composite.id = compositeID;
                            composites.Add(composite);
                        }
                        BinEntity entity = composite.entities.FirstOrDefault(o => o.id == entityID);
                        if (entity == null)
                        {
                            entity = new BinEntity();
                            entity.id = entityID;
                            composite.entities.Add(entity);
                        }
                        else if (entity.name != value)
                        {
                            Console.WriteLine("Renaming " + entityID + " from '" + entity.name + "' to '" + value + "'");
                        }
                        entity.name = value;
                        break;
                    }
                    //CASE 8 would be parameter definition
                }
            }

            File.WriteAllLines("OUT_STRINGS_" + Path.GetFileNameWithoutExtension(dump) + ".txt", strings);
            return;

            if (!Directory.Exists("out2")) Directory.CreateDirectory("out2");

            BinaryWriter writer = new BinaryWriter(File.OpenWrite("out2/" + Path.GetFileNameWithoutExtension(dump) + ".bin"));
            writer.BaseStream.SetLength(0);
            writer.Write(composites.Count);
            for (int i = 0; i < composites.Count; i++)
            {
                Utilities.Write<ShortGuid>(writer, composites[i].id);
                writer.Write(composites[i].path);
                writer.Write(composites[i].entities.Count);
                for (int x = 0; x < composites[i].entities.Count; x++)
                {
                    Utilities.Write<ShortGuid>(writer, composites[i].entities[x].id);
                    writer.Write(composites[i].entities[x].name);
                }
            }
            writer.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string[] decomp = File.ReadAllLines(@"E:\GitHub Repos\isolation_testground\enum_dump.txt");
            Dictionary<string, List<string>> enums = new Dictionary<string, List<string>>();
            List<string> dumpForWiki = new List<string>();
            for (int i = 0; i < decomp.Length; i++)
            {
                string[] thisSplit = decomp[i].Split(new char[] { ' ' }, 2);
                string enum_name = thisSplit[0];
                string[] thisSplit2 = thisSplit[1].Split('"');
                string enum_value = thisSplit2[1];

                if (enum_name == enum_value)
                {
                    dumpForWiki.Add("## " + enum_name);
                    enums.Add(enum_name, new List<string>());
                }
                else
                {
                    dumpForWiki.Add(" * " + enum_value);
                    enums[enum_name].Add(enum_value);
                }
            }

            File.WriteAllLines("out.md", dumpForWiki);
            dumpForWiki.Clear();
            foreach (var item in enums.OrderBy(x => x.Key))
            {
                if (item.Value.Count == 0) continue;
                dumpForWiki.Add("## " + item.Key);
                foreach (string val in item.Value)
                {
                    dumpForWiki.Add(" * " + val);
                }
                dumpForWiki.Add("");
            }
            File.WriteAllLines("out_ordered.md", dumpForWiki);
            string breakhere = "";

            /*
            string decomp = File.ReadAllText(@"C:\Users\mattf_cr4e5zq\AI ios.c");
            string[] decompSplit = decomp.Split(new[] { "ShortGuid::ShortGuid" }, StringSplitOptions.None);
            List<string> test = new List<string>();
            for (int i = 0; i < decompSplit.Length; i++)
            {
                string[] thisSplit = decompSplit[i].Split(new char[] { ',' }, 2);
                if (thisSplit.Length < 2) continue;
                if (thisSplit[1][0] != '"') continue;
                string[] thisNewSplit = thisSplit[1].Split('"');
                test.Add(thisNewSplit[1]);
            }
            List<string> test2 = new List<string>();
            for (int i =0; i <test.Count; i++)
            {
                if (test2.Contains(test[i])) continue;
                test2.Add(test[i]);
            }
            File.WriteAllLines("bleh.txt", test2);

            JObject obj = JObject.Parse(File.ReadAllText(@"C:\Users\mattf_cr4e5zq\Downloads\out.json"));
            JArray objA = ((JArray)obj["nodes"]);
            for (int i = 0; i < objA.Count; i++)
            {
                JArray objAP = (JArray)objA[i]["parameters"];
                for (int x = 0; x < objAP.Count; x++)
                {
                    if (test2.Contains(objAP[x]["name"].ToString())) continue;
                    test2.Add(objAP[x]["name"].ToString());
                    Console.WriteLine("Added " + objAP[x]["name"].ToString());
                }
            }

            BinaryReader reader = new BinaryReader(File.OpenRead(@"E:\OpenCAGE_2021\CathodeEditorGUI\CathodeLib\CathodeLib\Resources\NodeDBs\entity_parameter_names.bin"));
            reader.BaseStream.Position += 1;
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                reader.BaseStream.Position += 4;
                string str = reader.ReadString();
                if (test2.Contains(str)) continue;
                test2.Add(str);
            }
            reader.Close();

            BinaryWriter writer = new BinaryWriter(File.OpenWrite("bleh4.bin"));
            for (int i = 0; i < test2.Count; i++)
            {
                Utilities.Write<ShortGuid>(writer, ShortGuidUtils.Generate(test2[i]));
                writer.Write(test2[i]);
            }
            writer.Close();

            */
        }
    }

    public class BinComposite
    {
        public ShortGuid id;
        public string name = "";
        public string path = "";
        public List<BinEntity> entities = new List<BinEntity>();
    }
    public class BinEntity
    {
        public ShortGuid id;
        public string name = "";
        public List<BinParameter> parameters = new List<BinParameter>();
    }
    public class BinParameter
    {
        public ShortGuid id;
        public string name;
    }
}
