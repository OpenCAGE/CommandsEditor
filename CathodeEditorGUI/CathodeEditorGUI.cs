using CATHODE;
using CATHODE.Assets;
using CATHODE.Commands;
using CATHODE.LEGACY;
using CATHODE.Misc;
using CathodeEditorGUI.UserControls;
using CathodeLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Animation;

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
            DBG_CompileParamList.Visible = true;
            DBG_LoadAllCommands.Visible = true;

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
                Editor.selected.composite = null;
            }
            if (clear_parameter_list)
            {
                Editor.selected.entity = null;
                entityInfoGroup.Text = "Selected Entity Info";
                selected_entity_type_description.Text = "";
                selected_entity_name.Text = "";
                for (int i = 0; i < entity_params.Controls.Count; i++) 
                    entity_params.Controls[i].Dispose();
                entity_params.Controls.Clear();
                currentlyShowingChildLinks = true;
                jumpToComposite.Visible = false;
                editFunction.Enabled = false;
                editEntityResources.Enabled = false;
                editEntityMovers.Enabled = false;
                renameSelectedNode.Enabled = true;
                duplicateSelectedNode.Enabled = true;
                hierarchyDisplay.Visible = false;
                addNewParameter.Enabled = true;
                removeParameter.Enabled = true;
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
            Editor.util.entity = new EntityUtils(Editor.commands);
            if (currentBackgroundCacher != null) currentBackgroundCacher.Dispose();
            currentBackgroundCacher = Task.Factory.StartNew(() => EditorUtils.GenerateEntityNameCache(this));

            //Populate file tree
            treeHelper.UpdateFileTree(Editor.commands.GetCompositeNames().ToList());

            //Show info in UI
            RefreshStatsUI();

            //Load root composite
            treeHelper.SelectNode(Editor.commands.EntryPoints[0].name);
        }
        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            LoadCommandsPAK(env_list.SelectedItem.ToString());
        }
        private void RefreshStatsUI()
        {
            root_composite_display.Text = "Root composite: " + Editor.commands.EntryPoints[0].name;
            composite_count_display.Text = "Composite count: " + Editor.commands.Composites.Count;
        }

        /* Load commands */
        private void LoadCommands(string level)
        {
            Editor.commands = null;

            string path_to_ENV = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + level;
            try
            {
                Editor.commands = new CommandsPAK(path_to_ENV + "/WORLD/COMMANDS.PAK");
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\n" + e.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Editor.commands = null;
                return;
            }

            if (!Editor.commands.Loaded)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\nPlease place this executable in your Alien: Isolation folder.", "Environment error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (Editor.commands.EntryPoints[0] == null)
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
                string baseLevelPath = Editor.commands.Filepath.Substring(0, Editor.commands.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);

                //The game has two hard-coded _PATCH overrides which change the CommandsPAK but not the assets
                string levelName = env_list.Items[env_list.SelectedIndex].ToString();
                switch (levelName)
                {
                    case @"DLC\BSPNOSTROMO_RIPLEY_PATCH":
                    case @"DLC\BSPNOSTROMO_TWOTEAMS_PATCH":
                        baseLevelPath = baseLevelPath.Replace(levelName, levelName.Substring(0, levelName.Length - ("_PATCH").Length));
                        break;
                }

                Editor.resource.models = new CathodeModels(baseLevelPath + "RENDERABLE/MODELS_LEVEL.BIN",
                                                            baseLevelPath + "RENDERABLE/LEVEL_MODELS.PAK");
                Editor.resource.reds = new RenderableElementsDatabase(baseLevelPath + "WORLD/REDS.BIN");
                Editor.resource.materials = new MaterialDatabase(baseLevelPath + "RENDERABLE/LEVEL_MODELS.MTL");
                Editor.resource.textures = new Textures(baseLevelPath + "RENDERABLE/LEVEL_TEXTURES.ALL.PAK");
                Editor.resource.textures.Load();
                Editor.resource.textures_Global = new Textures(SharedData.pathToAI + "/DATA/ENV/GLOBAL/WORLD/GLOBAL_TEXTURES.ALL.PAK");
                Editor.resource.textures_Global.Load();
            }
            catch
            {
                //Can fail if we're loading a PAK outside the game structure
                MessageBox.Show("Failed to load asset PAKs!\nAre you opening a Commands PAK outside of a map directory?\nIf not, please try again.", "Resource editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Editor.resource.models = null;
                Editor.resource.reds = null;
                Editor.resource.materials = null;
                Editor.resource.textures = null;
                Editor.resource.textures_Global = null;
            }
        }

        /* Load mover descriptors */
        private void LoadMovers()
        {
            try
            {
                string baseLevelPath = Editor.commands.Filepath.Substring(0, Editor.commands.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);
                Editor.mvr = new MoverDatabase(baseLevelPath + "WORLD/MODELS.MVR");
            }
            catch
            {
                //Can fail if we're loading a MVR outside the game structure
                MessageBox.Show("Failed to load mover descriptor database!\nAre you opening a Commands PAK outside of a map directory?\nMVR editing disabled.", "MVR editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Editor.mvr = null;
            }
        }

        /* Save the current edits */
        private void save_commands_pak_Click(object sender, EventArgs e)
        {
            if (Editor.commands == null) return;
            Cursor.Current = Cursors.WaitCursor;

            byte[] backup = null;
            try
            {
                backup = File.ReadAllBytes(Editor.commands.Filepath);
                Editor.commands.Save();
            }
            catch (Exception ex)
            {
                try
                {
                    if (backup != null)
                        File.WriteAllBytes(Editor.commands.Filepath, backup);
                }
                catch { }
            
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Failed to save changes!\n" + ex.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Editor.resource.reds != null && Editor.resource.reds.RenderableElements != null)
                Editor.resource.reds.Save();

            if (Editor.mvr != null && Editor.mvr.Movers != null)
                Editor.mvr.Save();

            Cursor.Current = Cursors.Default;
            MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /* Edit the loaded COMMANDS.PAK's root composite */
        private void editEntryPoint_Click(object sender, EventArgs e)
        {
            if (Editor.commands == null || !Editor.commands.Loaded) return;
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
            switch (Editor.selected.entity.variant)
            {
                case EntityVariant.OVERRIDE:
                {
                    Entity entity = EditorUtils.ResolveHierarchy(((OverrideEntity)Editor.selected.entity).hierarchy, out flow, out string hierarchy);
                    if (entity != null)
                    {
                        LoadComposite(flow.name);
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    Entity entity = EditorUtils.ResolveHierarchy(((ProxyEntity)Editor.selected.entity).hierarchy, out flow, out string hierarchy);
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
            if (Editor.commands == null || !Editor.commands.Loaded) return;
            LoadComposite(Editor.commands.EntryPoints[0].name);
        }

        /* Add new composite (really we should be able to do this OFF OF entities, like making a prefab) */
        private void addNewComposite_Click(object sender, EventArgs e)
        {
            if (Editor.commands == null) return;
            CathodeEditorGUI_AddComposite add_flow = new CathodeEditorGUI_AddComposite();
            add_flow.Show();
            add_flow.FormClosed += new FormClosedEventHandler(add_flow_closed);
        }
        private void add_flow_closed(Object sender, FormClosedEventArgs e)
        {
            treeHelper.UpdateFileTree(Editor.commands.GetCompositeNames().ToList());
            RefreshStatsUI();
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected composite */
        private void removeSelectedComposite_Click(object sender, EventArgs e)
        {
            if (Editor.selected.composite == null) return;
            for (int i = 0; i < Editor.commands.EntryPoints.Count(); i++)
            {
                if (Editor.selected.composite.shortGUID == Editor.commands.EntryPoints[i].shortGUID)
                {
                    MessageBox.Show("Cannot delete a composite which is the root, global, or pause menu!", "Cannot delete.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (!ConfirmAction("Are you sure you want to remove this composite?")) return;

            //Remove any entities or links that reference this composite
            for (int i = 0; i < Editor.commands.Composites.Count; i++)
            {
                List<FunctionEntity> prunedFunctionEntities = new List<FunctionEntity>();
                for (int x = 0; x < Editor.commands.Composites[i].functions.Count; x++)
                {
                    if (Editor.commands.Composites[i].functions[x].function == Editor.selected.composite.shortGUID) continue;
                    List<EntityLink> prunedEntityLinks = new List<EntityLink>();
                    for (int l = 0; l < Editor.commands.Composites[i].functions[x].childLinks.Count; l++)
                    {
                        Entity linkedEntity = Editor.commands.Composites[i].GetEntityByID(Editor.commands.Composites[i].functions[x].childLinks[l].childID);
                        if (linkedEntity != null && linkedEntity.variant == EntityVariant.FUNCTION) if (((FunctionEntity)linkedEntity).function == Editor.selected.composite.shortGUID) continue;
                        prunedEntityLinks.Add(Editor.commands.Composites[i].functions[x].childLinks[l]);
                    }
                    Editor.commands.Composites[i].functions[x].childLinks = prunedEntityLinks;
                    prunedFunctionEntities.Add(Editor.commands.Composites[i].functions[x]);
                }
                Editor.commands.Composites[i].functions = prunedFunctionEntities;
            }
            //TODO: remove proxies etc that also reference any of the removed entities

            //Remove the composite
            Editor.commands.Composites.Remove(Editor.selected.composite);

            //Refresh UI
            ClearUI(false, true, true);
            RefreshStatsUI();
            treeHelper.UpdateFileTree(Editor.commands.GetCompositeNames().ToList());
        }

        /* Select entity from loaded composite */
        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (composite_content.SelectedIndex == -1 || Editor.selected.composite == null) return;
            try
            {
                ShortGuid entityID = new ShortGuid(composite_content.SelectedItem.ToString().Substring(1, 11));
                Entity thisEntity = Editor.selected.composite.GetEntityByID(entityID);
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
            if (Editor.selected.composite == null || Editor.selected.entity == null) return;
            CathodeEditorGUI_AddPin add_pin = new CathodeEditorGUI_AddPin(Editor.selected.entity, Editor.selected.composite);
            add_pin.Show();
            add_pin.FormClosed += new FormClosedEventHandler(add_pin_closed);
        }
        private void add_pin_closed(Object sender, FormClosedEventArgs e)
        {
            LoadEntity(Editor.selected.entity); //TODO: TEMP
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected out pin */
        private void removeSelectedLink_Click(object sender, EventArgs e)
        {
            /*
            if (Editor.selected.entity == null || Editor.selected.composite == null) return;
            if (!ConfirmAction("Are you sure you want to remove this link?")) return;
            if (currentlyShowingChildLinks)
            {
                Editor.selected.entity.childLinks.RemoveAt(entity_children.SelectedIndex);
            }
            else
            {
                List<Entity> ents = Editor.selected.composite.GetEntities();
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
            */
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
            Composite entry = Editor.commands.Composites[Editor.commands.GetFileIndex(FileName)];
            Editor.selected.composite = entry;
            EditorUtils.PurgeDeadHierarchiesInActiveComposite(); //TODO: We should really just skip this info when parsing, can remove "unknown" on composite then
            Cursor.Current = Cursors.WaitCursor;

            composite_content.BeginUpdate();
            List<Entity> entities = entry.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                string desc = EditorUtils.GenerateEntityName(entities[i], Editor.selected.composite);
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
            if (Editor.selected.composite == null) return;
            CathodeEditorGUI_AddEntity add_parameter = new CathodeEditorGUI_AddEntity(Editor.selected.composite, Editor.commands.Composites);
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
            if (Editor.selected.entity == null) return;
            if (!ConfirmAction("Are you sure you want to remove this entity?")) return;

            string removedID = Editor.selected.entity.shortGUID.ToString();

            switch (Editor.selected.entity.variant)
            {
                case EntityVariant.DATATYPE:
                    Editor.selected.composite.datatypes.Remove((DatatypeEntity)Editor.selected.entity);
                    break;
                case EntityVariant.FUNCTION:
                    Editor.selected.composite.functions.Remove((FunctionEntity)Editor.selected.entity);
                    break;
                case EntityVariant.OVERRIDE:
                    Editor.selected.composite.overrides.Remove((OverrideEntity)Editor.selected.entity);
                    break;
                case EntityVariant.PROXY:
                    Editor.selected.composite.proxies.Remove((ProxyEntity)Editor.selected.entity);
                    break;
            }

            List<Entity> entities = Editor.selected.composite.GetEntities();
            for (int i = 0; i < entities.Count; i++) //We should actually query every entity in the PAK, since we might be ref'd by a proxy or override
            {
                List<EntityLink> entLinks = new List<EntityLink>();
                for (int x = 0; x < entities[i].childLinks.Count; x++)
                {
                    if (entities[i].childLinks[x].childID != Editor.selected.entity.shortGUID) entLinks.Add(entities[i].childLinks[x]);
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
                                    triggerSequence.triggers[x].hierarchy[triggerSequence.triggers[x].hierarchy.Count - 2] != Editor.selected.entity.shortGUID)
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
                                    cageAnim.keyframeHeaders[x].connectedEntity[cageAnim.keyframeHeaders[x].connectedEntity.Count - 2] != Editor.selected.entity.shortGUID)
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
            else LoadComposite(Editor.selected.composite.name);

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
            if (Editor.selected.entity == null) return;
            if (!ConfirmAction("Are you sure you want to duplicate this entity?")) return;

            //Generate new entity ID and name
            Entity newEnt = Utilities.CloneObject(Editor.selected.entity);
            newEnt.shortGUID = ShortGuidUtils.Generate(DateTime.Now.ToString("G"));
            Editor.util.entity.SetName(
                Editor.selected.composite.shortGUID,
                newEnt.shortGUID,
                Editor.util.entity.GetName(Editor.selected.composite.shortGUID, Editor.selected.entity.shortGUID) + "_clone");

            //Add parent links in to this entity that linked in to the other entity
            List<Entity> ents = Editor.selected.composite.GetEntities();
            List<EntityLink> newLinks = new List<EntityLink>();
            int num_of_new_things = 1;
            foreach (Entity entity in ents)
            {
                newLinks.Clear();
                foreach (EntityLink link in entity.childLinks)
                {
                    if (link.childID == Editor.selected.entity.shortGUID)
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
                    Editor.selected.composite.functions.Add((FunctionEntity)newEnt);
                    break;
                case EntityVariant.DATATYPE:
                    Editor.selected.composite.datatypes.Add((DatatypeEntity)newEnt);
                    break;
                case EntityVariant.PROXY:
                    Editor.selected.composite.proxies.Add((ProxyEntity)newEnt);
                    break;
                case EntityVariant.OVERRIDE:
                    Editor.selected.composite.overrides.Add((OverrideEntity)newEnt);
                    break;
            }

            //Load in to UI
            ReloadUIForNewEntity(newEnt);
        }

        /* Rename selected entity */
        private void renameSelectedEntity_Click(object sender, EventArgs e)
        {
            if (Editor.selected.entity == null) return;
            CathodeEditorGUI_RenameEntity rename_entity = new CathodeEditorGUI_RenameEntity(Editor.selected.entity.shortGUID);
            rename_entity.Show();
            rename_entity.FormClosed += new FormClosedEventHandler(rename_entity_closed);
        }
        private void rename_entity_closed(Object sender, FormClosedEventArgs e)
        {
            if (((CathodeEditorGUI_RenameEntity)sender).didSave &&
                ((CathodeEditorGUI_RenameEntity)sender).EntityID == Editor.selected.entity.shortGUID)
            {
                Editor.util.entity.SetName(
                    Editor.selected.composite.shortGUID,
                    Editor.selected.entity.shortGUID,
                    ((CathodeEditorGUI_RenameEntity)sender).EntityName);

                string entityID = Editor.selected.entity.shortGUID.ToString();
                string newEntityName = EditorUtils.GenerateEntityName(Editor.selected.entity, Editor.selected.composite, true);
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
                LoadEntity(Editor.selected.entity);
            }
            this.BringToFront();
            this.Focus();
        }
        
        /* Perform a partial UI reload for a newly added entity */
        private void ReloadUIForNewEntity(Entity newEnt)
        {
            if (Editor.selected.composite == null || newEnt == null) return;
            if (currentSearch == "")
            {
                string newEntityName = EditorUtils.GenerateEntityName(newEnt, Editor.selected.composite);
                composite_content.Items.Add(newEntityName);
                composite_content_RAW.Add(newEntityName);
            }
            else
            {
                LoadComposite(Editor.selected.composite.name);
            }
            LoadEntity(newEnt);
        }

        /* Load a entity into the UI */
        private void LoadEntity(Entity entity)
        {
            ClearUI(false, false, true);
            Editor.selected.entity = entity;

            //Correct the UI, and return early if we have to change index, so we don't trigger twice
            int correctSelectedIndex = composite_content.Items.IndexOf(EditorUtils.GenerateEntityName(entity, Editor.selected.composite));
            if (composite_content.SelectedIndex != correctSelectedIndex)
            {
                composite_content.SelectedIndex = correctSelectedIndex;
                return;
            }

            if (entity == null) return;

            Cursor.Current = Cursors.WaitCursor;

            //populate info labels
            entityInfoGroup.Text = "Selected " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.variant.ToString().ToLower().Replace('_', ' ')) + " Info";
            string description = "";
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    ShortGuid thisFunction = ((FunctionEntity)entity).function;
                    Composite funcComposite = Editor.commands.GetComposite(thisFunction);
                    jumpToComposite.Visible = funcComposite != null;
                    if (funcComposite != null)
                        description = funcComposite.name;
                    else
                        description = ShortGuidUtils.FindString(thisFunction);
                    selected_entity_name.Text = Editor.util.entity.GetName(Editor.selected.composite.shortGUID, entity.shortGUID);
                    if (funcComposite == null)
                    {
                        FunctionType function = CommandsUtils.GetFunctionType(thisFunction);
                        editFunction.Enabled = function == FunctionType.CAGEAnimation || function == FunctionType.TriggerSequence;
                    }
                    editEntityResources.Enabled = (Editor.resource.textures != null);
                    break;
                case EntityVariant.DATATYPE:
                    description = "DataType " + ((DatatypeEntity)entity).type.ToString();
                    selected_entity_name.Text = ShortGuidUtils.FindString(((DatatypeEntity)entity).parameter);
                    renameSelectedNode.Enabled = false;
                    duplicateSelectedNode.Enabled = false;
                    addNewParameter.Enabled = false;
                    removeParameter.Enabled = false;
                    break;
                case EntityVariant.PROXY:
                case EntityVariant.OVERRIDE:
                    hierarchyDisplay.Visible = true;
                    string hierarchy = "";
                    if (entity.variant == EntityVariant.PROXY) EditorUtils.ResolveHierarchy(((ProxyEntity)entity).hierarchy, out Composite comp, out hierarchy);
                    else EditorUtils.ResolveHierarchy(((OverrideEntity)entity).hierarchy, out Composite comp, out hierarchy);
                    hierarchyDisplay.Text = hierarchy;
                    jumpToComposite.Visible = true;
                    selected_entity_name.Text = Editor.util.entity.GetName(Editor.selected.composite.shortGUID, entity.shortGUID);
                    break;
                default:
                    selected_entity_name.Text = Editor.util.entity.GetName(Editor.selected.composite.shortGUID, entity.shortGUID);
                    break;
            }
            selected_entity_type_description.Text = description;

            //show mvr editor button if this entity has a mvr link
            if (Editor.mvr != null && Editor.mvr.Movers.FindAll(o => o.commandsNodeID == Editor.selected.entity.shortGUID).Count != 0)
                editEntityMovers.Enabled = true;

            //populate linked params IN
            int current_ui_offset = 7;
            List<Entity> ents = Editor.selected.composite.GetEntities();
            foreach (Entity ent in ents)
            {
                foreach (EntityLink link in ent.childLinks)
                {
                    if (link.childID != entity.shortGUID) continue;
                    GUI_Link parameterGUI = new GUI_Link();
                    parameterGUI.PopulateUI(link, false, ent.shortGUID);
                    parameterGUI.GoToEntity += LoadEntity;
                    parameterGUI.Location = new Point(15, current_ui_offset);
                    current_ui_offset += parameterGUI.Height + 6;
                    entity_params.Controls.Add(parameterGUI);
                }
            }

            //populate parameter inputs
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
                        ((GUI_StringDataType)parameterGUI).PopulateUI((cString)this_param, entity.parameters[i].shortGUID);
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

            //populate linked params OUT
            for (int i = 0; i < entity.childLinks.Count; i++)
            {
                GUI_Link parameterGUI = new GUI_Link();
                parameterGUI.PopulateUI(entity.childLinks[i], true);
                parameterGUI.GoToEntity += LoadEntity;
                parameterGUI.Location = new Point(15, current_ui_offset);
                current_ui_offset += parameterGUI.Height + 6;
                entity_params.Controls.Add(parameterGUI);
            }

            Cursor.Current = Cursors.Default;
        }

        /* Add a new parameter */
        private void addNewParameter_Click(object sender, EventArgs e)
        {
            if (Editor.selected.entity == null) return;
            CathodeEditorGUI_AddParameter add_parameter = new CathodeEditorGUI_AddParameter(Editor.selected.entity);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }
        private void refresh_entity_event(Object sender, FormClosedEventArgs e)
        {
            LoadEntity(Editor.selected.entity);
            this.BringToFront();
            this.Focus();
        }

        /* Add a new link out */
        private void addLinkOut_Click(object sender, EventArgs e)
        {
            if (Editor.selected.entity == null) return;
            CathodeEditorGUI_AddPin add_link = new CathodeEditorGUI_AddPin(Editor.selected.entity, Editor.selected.composite);
            add_link.Show();
            add_link.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }

        /* Remove a parameter */
        private void removeParameter_Click(object sender, EventArgs e)
        {
            if (Editor.selected.entity == null) return;
            if (entity_params.Controls.Count == 0) return;
            CathodeEditorGUI_RemoveParameter remove_parameter = new CathodeEditorGUI_RemoveParameter(Editor.selected.entity);
            remove_parameter.Show();
            remove_parameter.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }

        /* Edit function entity (CAGEAnimation/TriggerSequence) */
        private void editFunction_Click(object sender, EventArgs e)
        {
            if (Editor.selected.entity.variant != EntityVariant.FUNCTION) return;
            string function = ShortGuidUtils.FindString(((FunctionEntity)Editor.selected.entity).function);
            switch (function.ToUpper())
            {
                case "CAGEANIMATION":
                    CAGEAnimationEditor cageAnimationEditor = new CAGEAnimationEditor((CAGEAnimation)Editor.selected.entity);
                    cageAnimationEditor.Show();
                    cageAnimationEditor.FormClosed += FunctionEditor_FormClosed;
                    break;
                case "TRIGGERSEQUENCE":
                    TriggerSequenceEditor triggerSequenceEditor = new TriggerSequenceEditor((TriggerSequence)Editor.selected.entity);
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
            resourceFunctionToEdit = ((FunctionEntity)Editor.selected.entity);

            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(((FunctionEntity)Editor.selected.entity).resources, Editor.selected.entity.shortGUID, EditorUtils.GenerateEntityName(Editor.selected.entity, Editor.selected.composite));
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
            CathodeEditorGUI_EditMVR moverEditor = new CathodeEditorGUI_EditMVR(Editor.selected.entity.shortGUID);
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

        private class EntityDef
        {
            public string title = "";
            public Dictionary<ParameterVariant, List<ParameterDef>> stuff = new Dictionary<ParameterVariant, List<ParameterDef>>();
        }
        private class ParameterDef
        {
            public string name;
            public string datatype;
            public string defaultval;
        }

        private void BuildNodeParameterDatabase(object sender, EventArgs e)
        {
            /*
            List<string> bruhmoment = File.ReadAllLines(@"C:\Users\mattf\Downloads\enums.txt").ToList<string>();
            Dictionary<string, List<string>> outputtt = new Dictionary<string, List<string>>();
            string currenteunm = "";
            foreach (string str in bruhmoment)
            {
                if (str.Length > 2 && str.Substring(0, 2) == "##")
                {
                    currenteunm = str.Substring(3).Trim();
                    outputtt.Add(currenteunm, new List<string>());
                }
                if (str.Length > 2 && str.Substring(0, 2) == " *")
                {
                    outputtt[currenteunm].Add(str.Substring(3).Trim());
                }
            }
            File.WriteAllText("enums.json", JsonConvert.SerializeObject(outputtt, Formatting.Indented));

            List<string> enums = new List<string>();
            foreach (var val in outputtt)
            {
                enums.Add(val.Key);
            }
            File.WriteAllLines("enums.txt", enums);

            BinaryWriter write = new BinaryWriter(File.OpenWrite("enums.bin"));
            write.BaseStream.SetLength(0);
            foreach (var str in outputtt)
            {
                write.Write(ShortGuidUtils.Generate(str.Key).val);
                write.Write(str.Key);
                write.Write(str.Value.Count);
                foreach (string str_ in str.Value)
                {
                    write.Write(str_);
                }
            }
            write.Close();
            return;
            */

            List<string> all_datatypes = new List<string>();
            List<string> all_types = new List<string>();

            List<string> content = File.ReadAllLines(@"C:\Users\mattf\Downloads\all_nodes_with_params.html").ToList<string>(); //https://myfiles.mattfiler.co.uk/all_nodes_with_params.html
            List<string> content2 = File.ReadAllLines(@"C:\Users\mattf\Downloads\all_nodes_with_params1.html").ToList<string>(); //https://myfiles.mattfiler.co.uk/all_nodes_with_params1.html
            List<EntityDef> ents = new List<EntityDef>();
            EntityDef currentEnt = null;
            for (int i = 0; i < content.Count; i++)
            {
                if (content[i].Length > 3 && content[i].Substring(0, 3) == "<h3")
                {
                    if (currentEnt != null)
                    {
                        ents.Add(currentEnt);
                    }
                    currentEnt = new EntityDef();
                    string split = content[i].Split('>')[1];
                    currentEnt.title = split.Substring(0, split.Length - 4);
                }
                else if (content[i].Length > 3 && content[i].Substring(0, 3) == "<li")
                {
                    string type_s = (content[i].Split('[')[1].Split(']')[0]).ToUpper();
                    if (type_s == "RESOURCE") continue;

                    Enum.TryParse(type_s, out ParameterVariant type);
                    if (!currentEnt.stuff.ContainsKey(type)) currentEnt.stuff.Add(type, new List<ParameterDef>());

                    string split = content[i].Split(']')[1];
                    if (split.Contains("["))
                    {
                        split = split.Split('[')[0];
                        split = split.Substring(1, split.Length - 2);
                    }
                    else
                    {
                        split = split.Substring(1, split.Length - 6);
                    }
                    string[] split2_l = content[i].Split(new string[] { "DataType: " }, StringSplitOptions.None);
                    string[] split_3 = content2[i].Split(new string[] { "DefaultVal: " }, StringSplitOptions.None);
                    string datatype = (split2_l.Length > 1) ? split2_l[1].Substring(0, split2_l[1].Length - 6) : "";
                    string defaultval = (split_3.Length > 1) ? split_3[1].Substring(0, split_3[1].Length - 6) : "";

                    currentEnt.stuff[type].Add(new ParameterDef() { datatype = datatype, name = split, defaultval = defaultval });

                    if (!all_types.Contains(type.ToString()))
                        all_types.Add(type.ToString());
                    if (!all_datatypes.Contains(datatype))
                        all_datatypes.Add(datatype);

                }
            }
            ents.Add(currentEnt);

            List<string> type_dump = new List<string>();
            type_dump.Add("{\"data\":[");
            foreach (EntityDef def in ents)
            {
                if (def.title == @"n:\\content\\build\\library\\archetypes\\gameplay\\gcip_worldpickup")
                    def.title = "GCIP_WorldPickup";
                if (def.title == @"n:\\content\\build\\library\\ayz\\animation\\logichelpers\\playforminduration")
                    def.title = "PlayForMinDuration";
                if (def.title == @"n:\\content\\build\\library\\archetypes\\script\\gameplay\\torch_control")
                    def.title = "Torch_Control";

                type_dump.Add("{\"type\": \"" + def.title + "\", \"data\": {");
                foreach (var val in def.stuff)
                {
                    type_dump.Add("\"" + val.Key + "\": [");
                    foreach (ParameterDef valu in val.Value)
                    {
                        type_dump.Add("\"" + valu.name + "\", ");
                    }
                    if (val.Value.Count != 0) type_dump[type_dump.Count - 1] = type_dump[type_dump.Count - 1].Substring(0, type_dump[type_dump.Count - 1].Length - 2);
                    type_dump.Add("], ");
                }
                if (def.stuff.Count != 0) type_dump[type_dump.Count - 1] = "]";
                type_dump.Add("}},");
            }
            if (ents.Count != 0) type_dump[type_dump.Count - 1] = "}}";
            type_dump.Add("]}");
            string ffff = "";
            for (int i = 0; i < type_dump.Count; i++)
                ffff += type_dump[i];
            File.WriteAllText("types.json", JObject.Parse(ffff).ToString(Formatting.Indented));

            List<string> resource_types = new List<string>();

            List<string> scripting = new List<string>();
            foreach (EntityDef def in ents)
            {
                if (def.title == @"n:\\content\\build\\library\\archetypes\\gameplay\\gcip_worldpickup")
                    def.title = "GCIP_WorldPickup";
                if (def.title == @"n:\\content\\build\\library\\ayz\\animation\\logichelpers\\playforminduration")
                    def.title = "PlayForMinDuration";
                if (def.title == @"n:\\content\\build\\library\\archetypes\\script\\gameplay\\torch_control")
                    def.title = "Torch_Control";

                if (def.stuff.Count != 0)
                {
                    scripting.Add("case FunctionType." + def.title + ":");
                    if (def.title == "CAGEAnimation") scripting.Add("\tnewEntity = new CAGEAnimation(thisID);");
                    if (def.title == "TriggerSequence") scripting.Add("\tnewEntity = new TriggerSequence(thisID);");
                }
                foreach (var val in def.stuff)
                {
                    foreach (ParameterDef valu in val.Value)
                    {
                        if (valu.name == "resource")
                        {
                            if (def.title != "ModelReference" && def.title != "EnvironmentModelReference")
                                scripting.Add("\tnewEntity.AddResource(ResourceType." + valu.datatype + ");");
                        }
                        else
                        {
                            string defaults = "";
                            string type = "";
                            defaults = valu.defaultval;
                            switch (valu.datatype)
                            {
                                case "":
                                case "Object":
                                case "ZonePtr":
                                case "ZoneLinkPtr":
                                case "ResourceID":
                                case "ReferenceFramePtr":
                                case "AnimationInfoPtr":
                                    type = "ParameterData"; //TODO
                                    defaults = "";
                                    break;

                                case "int":
                                    type = "cInteger";
                                    break;
                                case "bool":
                                    type = "cBool";
                                    break;
                                case "float":
                                    type = "cFloat";
                                    if (defaults != "") defaults += "f";
                                    break;
                                case "String":
                                    type = "cString";
                                    defaults = "\"" + defaults + "\"";
                                    break;
                                case "Position":
                                    type = "cTransform";
                                    break;
                                case "FilePath":
                                    type = "cString";
                                    break;
                                case "SPLINE":
                                case "SplineData":
                                    type = "cSpline";
                                    if (defaults == "0") defaults = "";
                                    break;
                                case "Direction":
                                    type = "cVector3";
                                    if (defaults == "0") defaults = "";
                                    if (defaults == "default_Direction") defaults = "";
                                    break;
                                case "Enum":
                                    type = "cEnum";
                                    if (defaults == "0xffffffff00000000") defaults = "";
                                    break;
                                default:
                                    type = "cEnum";
                                    string[] spl = valu.defaultval.Split('(');
                                    if (spl.Length > 1) defaults = "\"" + spl[0].Substring(0, spl[0].Length - 1) + "\", " + spl[1].Substring(0, spl[1].Length - 1);
                                    else
                                    {
                                        if (EnumUtils.GetEnum(ShortGuidUtils.Generate(valu.datatype)) == null)
                                        {
                                            type = "cResource";
                                            defaults = "new ResourceReference[]{ new ResourceReference(ResourceType." + valu.datatype + ") }.ToList<ResourceReference>(), newEntity.shortGUID";
                                            if (!resource_types.Contains(valu.datatype.ToString()))
                                                resource_types.Add(valu.datatype.ToString());
                                        }
                                        else
                                        {
                                            defaults = "\"" + valu.datatype + "\", 0";
                                        }
                                    }
                                    break;
                            }
                            if (defaults.Length > ("NEON_fmov").Length && defaults.Substring(0, ("NEON_fmov").Length) == "NEON_fmov") defaults = "";
                            scripting.Add("\tnewEntity.AddParameter(\"" + valu.name + "\", new " + type + "(" + defaults + "), ParameterVariant." + val.Key + "); //" + valu.datatype);
                        }
                    }
                }
                if (def.stuff.Count != 0)
                {
                    if (def.title == "ModelReference")
                    {
                        scripting.Add("\tcResource resourceData = new cResource(newEntity.shortGUID);");
                        scripting.Add("\tresourceData.AddResource(ResourceType.RENDERABLE_INSTANCE);");
                        scripting.Add("\tnewEntity.parameters.Add(new Parameter(\"resource\", resourceData));");
                    }
                    if (def.title == "EnvironmentModelReference")
                    {
                        scripting.Add("\tcResource resourceData2 = new cResource(newEntity.shortGUID);");
                        scripting.Add("\tresourceData2.AddResource(ResourceType.ANIMATED_MODEL); //TODO: need to figure out what startIndex links to, so we can set that!");
                        scripting.Add("\tnewEntity.parameters.Add(new Parameter(\"resource\", resourceData2));");
                    }
                    if (def.title == "PhysicsSystem") scripting.Add("\tnewEntity.AddResource(ResourceType.DYNAMIC_PHYSICS_SYSTEM).startIndex = 0;");
                    scripting.Add("break;");
                }
            }
            File.WriteAllLines("out.cs", scripting);

            Console.WriteLine(JsonConvert.SerializeObject(all_types, Formatting.Indented));
            Console.WriteLine(JsonConvert.SerializeObject(all_datatypes, Formatting.Indented));
            Console.WriteLine(JsonConvert.SerializeObject(resource_types, Formatting.Indented));

            string bleh = JsonConvert.SerializeObject(ents, Formatting.Indented);
            File.WriteAllText("out.json" , bleh);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int mm = 0; mm < env_list.Items.Count; mm++)
            {
                //if (env_list.Items[mm].ToString() != "BSP_TORRENS") continue;

                Editor.commands = new CommandsPAK(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + env_list.Items[mm].ToString() + "/WORLD/COMMANDS.PAK");
                Console.WriteLine("Loading: " + Editor.commands.Filepath + "...");
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
                for (int i = 0; i < Editor.commands.Composites.Count; i++)
                {
                    for (int x = 0; x < Editor.commands.Composites[i].functions.Count; x++)
                    {
                        if (!CommandsUtils.FunctionTypeExists(Editor.commands.Composites[i].functions[x].function)) continue;
                        FunctionType type = CommandsUtils.GetFunctionType(Editor.commands.Composites[i].functions[x].function);
                        switch (type)
                        {
                            case FunctionType.CameraShake:
                            case FunctionType.BoneAttachedCamera:
                            case FunctionType.CamPeek:
                            case FunctionType.ClipPlanesController:
                            case FunctionType.ControllableRange:
                            case FunctionType.FixedCamera:
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

                                //if (CurrentInstance.commandsPAK.Composites[i].functions[x].resources.Count != 0)
                                //{
                                //    string breasdfdf = "";
                                //    if (CurrentInstance.commandsPAK.Composites[i].functions[x].GetResource(ResourceType.COLLISION_MAPPING) == null)
                                //    {
                                //        string sdfsd = "";
                                //    }
                                //}


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


                                Console.WriteLine(Editor.commands.Composites[i].name + " -> " + Editor.commands.Composites[i].functions[x].shortGUID + " -> " + type);

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
        }
        #endregion
    }
}
