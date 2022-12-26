using CATHODE;
using CATHODE.Assets;
using CATHODE.Commands;
using CATHODE.LEGACY;
using CATHODE.Misc;
using CathodeEditorGUI.UserControls;
using CathodeLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            button1.Visible = true;
            button2.Visible = true;

            env_list.SelectedItem = "BSP_TORRENS";
            //LoadCommandsPAK(env_list.SelectedItem.ToString());
            //LoadComposite("DisplayModel:ALIEN");
            //LoadEntity(CurrentInstance.selectedComposite.GetEntityByID(new ShortGuid("")));
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

                //The game has two hard-coded _PATCH overrides which change the CommandsPAK but not the assets
                string levelName = env_list.Items[env_list.SelectedIndex].ToString();
                switch (levelName)
                {
                    case @"DLC\BSPNOSTROMO_RIPLEY_PATCH":
                    case @"DLC\BSPNOSTROMO_TWOTEAMS_PATCH":
                        baseLevelPath = baseLevelPath.Replace(levelName, levelName.Substring(0, levelName.Length - ("_PATCH").Length));
                        break;
                }

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
                MessageBox.Show("Failed to load asset PAKs!\nAre you opening a Commands PAK outside of a map directory?\nIf not, please try again.", "Resource editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        private void save_commands_pak_Click(object sender, EventArgs e)
        {
            if (CurrentInstance.commandsPAK == null) return;
            Cursor.Current = Cursors.WaitCursor;

            byte[] backup = null;
            try
            {
                backup = File.ReadAllBytes(CurrentInstance.commandsPAK.Filepath);
                CurrentInstance.commandsPAK.Save();
            }
            catch (Exception ex)
            {
                try
                {
                    if (backup != null)
                        File.WriteAllBytes(CurrentInstance.commandsPAK.Filepath, backup);
                }
                catch { }
            
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Failed to save changes!\n" + ex.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CurrentInstance.redsDB != null && CurrentInstance.redsDB.RenderableElements != null)
                CurrentInstance.redsDB.Save();

            if (CurrentInstance.moverDB != null && CurrentInstance.moverDB.Movers != null)
                CurrentInstance.moverDB.Save();

            Cursor.Current = Cursors.Default;
            MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            Composite flow;
            switch (CurrentInstance.selectedEntity.variant)
            {
                case EntityVariant.OVERRIDE:
                {
                    Entity entity = EditorUtils.ResolveHierarchy(((OverrideEntity)CurrentInstance.selectedEntity).hierarchy, out flow, out string hierarchy);
                    if (entity != null)
                    {
                        LoadComposite(flow.name);
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    Entity entity = EditorUtils.ResolveHierarchy(((ProxyEntity)CurrentInstance.selectedEntity).hierarchy, out flow, out string hierarchy);
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
                    List<EntityLink> prunedEntityLinks = new List<EntityLink>();
                    for (int l = 0; l < CurrentInstance.commandsPAK.Composites[i].functions[x].childLinks.Count; l++)
                    {
                        Entity linkedEntity = CurrentInstance.commandsPAK.Composites[i].GetEntityByID(CurrentInstance.commandsPAK.Composites[i].functions[x].childLinks[l].childID);
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
                Entity thisEntity = CurrentInstance.selectedComposite.GetEntityByID(entityID);
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
                List<Entity> ents = CurrentInstance.selectedComposite.GetEntities();
                int deleteIndex = -1;
                foreach (Entity entity in ents)
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

            Entity thisEntity = null;
            if (currentlyShowingChildLinks)
            {
                thisEntity = CurrentInstance.selectedComposite.GetEntityByID(CurrentInstance.selectedEntity.childLinks[entity_children.SelectedIndex].childID);
            }
            else
            {
                List<Entity> ents = CurrentInstance.selectedComposite.GetEntities();
                foreach (Entity entity in ents)
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
            Composite entry = CurrentInstance.commandsPAK.Composites[CurrentInstance.commandsPAK.GetFileIndex(FileName)];
            CurrentInstance.selectedComposite = entry;
            EditorUtils.PurgeDeadHierarchiesInActiveComposite(); //TODO: We should really just skip this info when parsing, can remove "unknown" on composite then
            Cursor.Current = Cursors.WaitCursor;

            composite_content.BeginUpdate();
            List<Entity> entities = entry.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                string desc = EditorUtils.GenerateEntityName(entities[i], CurrentInstance.selectedComposite);
                composite_content.Items.Add(desc);
                composite_content_RAW.Add(desc);
            }
            composite_content.EndUpdate();

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
        private void OnAddNewEntity(Entity entity)
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
            }

            List<Entity> entities = CurrentInstance.selectedComposite.GetEntities();
            for (int i = 0; i < entities.Count; i++) //We should actually query every entity in the PAK, since we might be ref'd by a proxy or override
            {
                List<EntityLink> entLinks = new List<EntityLink>();
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
            Entity newEnt = Utilities.CloneObject(CurrentInstance.selectedEntity);
            newEnt.shortGUID = ShortGuidUtils.Generate(DateTime.Now.ToString("G"));
            CurrentInstance.compositeLookup.SetEntityName(
                CurrentInstance.selectedComposite.shortGUID,
                newEnt.shortGUID,
                CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, CurrentInstance.selectedEntity.shortGUID) + "_clone");

            //Add parent links in to this entity that linked in to the other entity
            List<Entity> ents = CurrentInstance.selectedComposite.GetEntities();
            List<EntityLink> newLinks = new List<EntityLink>();
            int num_of_new_things = 1;
            foreach (Entity entity in ents)
            {
                newLinks.Clear();
                foreach (EntityLink link in entity.childLinks)
                {
                    if (link.childID == CurrentInstance.selectedEntity.shortGUID)
                    {
                        EntityLink newLink = new EntityLink();
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
        private void ReloadUIForNewEntity(Entity newEnt)
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
        private void LoadEntity(Entity entity)
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
                    Composite funcComposite = CurrentInstance.commandsPAK.GetComposite(thisFunction);
                    jumpToComposite.Visible = funcComposite != null;
                    if (funcComposite != null)
                        description = funcComposite.name;
                    else
                        description = ShortGuidUtils.FindString(thisFunction);
                    selected_entity_name.Text = CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, entity.shortGUID);
                    if (funcComposite == null)
                    {
                        FunctionType function = CommandsUtils.GetFunctionType(thisFunction);
                        editFunction.Enabled = function == FunctionType.CAGEAnimation || function == FunctionType.TriggerSequence;
                    }
                    editEntityResources.Enabled = (CurrentInstance.textureDB != null);
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
                    if (entity.variant == EntityVariant.PROXY) EditorUtils.ResolveHierarchy(((ProxyEntity)entity).hierarchy, out Composite comp, out hierarchy);
                    else EditorUtils.ResolveHierarchy(((OverrideEntity)entity).hierarchy, out Composite comp, out hierarchy);
                    hierarchyDisplay.Text = hierarchy;
                    jumpToComposite.Visible = true;
                    selected_entity_name.Text = CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, entity.shortGUID);
                    break;
                default:
                    selected_entity_name.Text = CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, entity.shortGUID);
                    break;
            }
            selected_entity_type_description.Text = description;

            //show mvr editor button if this entity has a mvr link
            if (CurrentInstance.moverDB != null && CurrentInstance.moverDB.Movers.FindAll(o => o.commandsNodeID == CurrentInstance.selectedEntity.shortGUID).Count != 0)
                editEntityMovers.Enabled = true;

            //sort parameters by type, to improve visibility in UI
            //entity.parameters = entity.parameters.OrderBy(o => o.content?.dataType).ToList();

            //populate parameter inputs
            int current_ui_offset = 7;
            for (int i = 0; i < entity.parameters.Count; i++)
            {
                ParameterData this_param = entity.parameters[i].content;
                UserControl parameterGUI = null;

                switch (this_param.dataType)
                {
                    case DataType.TRANSFORM:
                        parameterGUI = new GUI_TransformDataType();
                        ((GUI_TransformDataType)parameterGUI).PopulateUI((cTransform)this_param, entity.parameters[i].shortGUID);
                        break;
                    case DataType.INTEGER:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Int((cInteger)this_param, entity.parameters[i].shortGUID);
                        break;
                    case DataType.STRING:
                        parameterGUI = new GUI_StringDataType();
                        ((GUI_StringDataType)parameterGUI).PopulateUI((CATHODE.Commands.cString)this_param, entity.parameters[i].shortGUID);
                        break;
                    case DataType.BOOL:
                        parameterGUI = new GUI_BoolDataType();
                        ((GUI_BoolDataType)parameterGUI).PopulateUI((cBool)this_param, entity.parameters[i].shortGUID);
                        break;
                    case DataType.FLOAT:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Float((cFloat)this_param, entity.parameters[i].shortGUID);
                        break;
                    case DataType.VECTOR:
                        parameterGUI = new GUI_VectorDataType();
                        ((GUI_VectorDataType)parameterGUI).PopulateUI((cVector3)this_param, entity.parameters[i].shortGUID);
                        break;
                    case DataType.ENUM:
                        parameterGUI = new GUI_EnumDataType();
                        ((GUI_EnumDataType)parameterGUI).PopulateUI((cEnum)this_param, entity.parameters[i].shortGUID);
                        break;
                    case DataType.RESOURCE:
                        parameterGUI = new GUI_ResourceDataType();
                        ((GUI_ResourceDataType)parameterGUI).PopulateUI((cResource)this_param, entity.parameters[i].shortGUID);
                        break;
                    case DataType.SPLINE:
                        parameterGUI = new GUI_SplineDataType();
                        ((GUI_SplineDataType)parameterGUI).PopulateUI((cSpline)this_param, entity.parameters[i].shortGUID);
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
                foreach (EntityLink link in CurrentInstance.selectedEntity.childLinks)
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
                List<Entity> ents = CurrentInstance.selectedComposite.GetEntities();
                foreach (Entity entity in ents)
                {
                    foreach (EntityLink link in entity.childLinks)
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
        private FunctionEntity resourceFunctionToEdit = null;
        private void editEntityResources_Click(object sender, EventArgs e)
        {
            resourceFunctionToEdit = ((FunctionEntity)CurrentInstance.selectedEntity);

            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(((FunctionEntity)CurrentInstance.selectedEntity).resources, CurrentInstance.selectedEntity.shortGUID, EditorUtils.GenerateEntityName(CurrentInstance.selectedEntity, CurrentInstance.selectedComposite));
            resourceEditor.Show();
            resourceEditor.OnSaved += OnResourceEditorSaved;
            resourceEditor.FormClosed += ResourceEditor_FormClosed;
        }
        private void OnResourceEditorSaved(List<ResourceReference> resources)
        {
            if (resourceFunctionToEdit != null)
                resourceFunctionToEdit.resources = resources;
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

        /* Confirm an action */
        private bool ConfirmAction(string msg)
        {
            return (MessageBox.Show(msg, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        #region LOCAL_TESTS
        private void button1_Click_1(object sender, EventArgs e)
        {
#if DEBUG
            for (int mm = 0; mm < env_list.Items.Count; mm++)
            {
                //if (env_list.Items[mm].ToString() != "BSP_TORRENS") continue;

                CurrentInstance.commandsPAK = new CommandsPAK(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + env_list.Items[mm].ToString() + "/WORLD/COMMANDS.PAK");
                Console.WriteLine("Loading: " + CurrentInstance.commandsPAK.Filepath + "...");
                //CurrentInstance.compositeLookup = new EntityNameLookup(CurrentInstance.commandsPAK);

                //EnvironmentAnimationDatabase db = new EnvironmentAnimationDatabase(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + env_list.Items[mm].ToString() + "/WORLD/ENVIRONMENT_ANIMATION.DAT");
                //Console.WriteLine(db.Header.MatrixCount0);
                //Console.WriteLine(db.Header.MatrixCount1);
                //Console.WriteLine(db.Header.EntryCount1);
                //Console.WriteLine(db.Header.EntryCount0);
                //Console.WriteLine(db.Header.IDCount0);
                //Console.WriteLine(db.Header.IDCount1);
                //Console.WriteLine(db.Header.Unknown1_);

                string[] towrite = new string[200];
                for (int i = 0; i < CurrentInstance.commandsPAK.Composites.Count; i++)
                {
                    for (int x = 0; x < CurrentInstance.commandsPAK.Composites[i].functions.Count; x++)
                    {
                        if (!CommandsUtils.FunctionTypeExists(CurrentInstance.commandsPAK.Composites[i].functions[x].function)) continue;
                        FunctionType type = CommandsUtils.GetFunctionType(CurrentInstance.commandsPAK.Composites[i].functions[x].function);
                        switch (type)
                        {
                            case FunctionType.CollisionBarrier:
                                //ResourceReference rr = CurrentInstance.commandsPAK.Composites[i].functions[x].resources.FirstOrDefault(o => o.entryType == ResourceType.COLLISION_MAPPING);
                                //if (rr == null)
                                //{
                                //    Console.WriteLine("NULL");
                                //}
                                //else
                                //{
                                //    Console.WriteLine(rr.startIndex + " / " + rr.count + " / " + rr.entityID);
                                //    //Console.WriteLine(JsonConvert.SerializeObject(rr));
                                //}

                                if (CurrentInstance.commandsPAK.Composites[i].functions[x].resources.Count != 0)
                                {
                                    string breasdfdf = "";
                                    if (CurrentInstance.commandsPAK.Composites[i].functions[x].GetResource(ResourceType.COLLISION_MAPPING) == null)
                                    {
                                        string sdfsd = "";
                                    }
                                }
                                

                                //Console.WriteLine(CurrentInstance.commandsPAK.Composites[i].name + " -> " + CurrentInstance.commandsPAK.Composites[i].functions[x].shortGUID + " -> " +  type);
                                //for (int y = 0; y < CurrentInstance.commandsPAK.Composites[i].functions[x].resources.Count; y++)
                                //{
                                //    Console.WriteLine("\t" + CurrentInstance.commandsPAK.Composites[i].functions[x].resources[y].entryType);
                                //}


                                //Console.WriteLine(type);

                                //Console.WriteLine("");

                                //Composite comp = CurrentInstance.commandsPAK.Composites[i];
                                //List<ResourceReference> rr = ((cResource)comp.functions[x].GetParameter("resource").content).value;
                                //towrite[rr[0].startIndex] = rr[0].startIndex  + "\n\t" + comp.name + "\n\t" + CurrentInstance.compositeLookup.GetEntityName(comp.shortGUID, comp.functions[x].shortGUID);

                                //Console.WriteLine(rr.Count);


                                //Console.WriteLine(CurrentInstance.commandsPAK.Composites[i].name + " -> " + CurrentInstance.commandsPAK.Composites[i].functions[x].shortGUID + " -> " +  type);

                                //for (int y = 0; y < CurrentInstance.commandsPAK.Composites[i].functions[x].resources.Count; y++)
                                //{
                                //    ResourceReference rr = CurrentInstance.commandsPAK.Composites[i].functions[x].resources[y];
                                //    if (rr.entryType != ResourceType.RENDERABLE_INSTANCE)
                                //    {
                                //        Console.WriteLine(" !!!!!!!  FOUND " + rr.entryType);
                                //    }
                                //    Console.WriteLine(JsonConvert.SerializeObject(rr));
                                //}
                                break;
                        }
                    }
                }
                //foreach (string line in towrite)
                //{
                //    if (line == null)
                //    {
                //        Console.WriteLine("--");
                //    }
                //    else
                //    {
                //        Console.WriteLine(line);
                //    }
                //}

                //CurrentInstance.commandsPAK.Save();
            }
#endif
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int mm = 0; mm < env_list.Items.Count; mm++)
            {
                CurrentInstance.commandsPAK = new CommandsPAK(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + env_list.Items[mm].ToString() + "/WORLD/COMMANDS.PAK");
                CurrentInstance.commandsPAK.Save();
            }
        }
        #endregion
    }
}
