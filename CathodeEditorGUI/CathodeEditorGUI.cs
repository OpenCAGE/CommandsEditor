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
                Editor.resource.env_animations = new EnvironmentAnimationDatabase(baseLevelPath + "WORLD/ENVIRONMENT_ANIMATION.DAT");
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
                Editor.resource.env_animations = null;
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
                    Editor.selected.composite.variables.Remove((VariableEntity)Editor.selected.entity);
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
            newEnt.shortGUID = ShortGuidUtils.GenerateRandom();
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
                    Editor.selected.composite.variables.Add((VariableEntity)newEnt);
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
                    description = "DataType " + ((VariableEntity)entity).type.ToString();
                    selected_entity_name.Text = ShortGuidUtils.FindString(((VariableEntity)entity).parameter);
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
        private void BuildNodeParameterDatabase(object sender, EventArgs e)
        {
            //LocalDebug.DumpEnumList();
            LocalDebug.DumpCathodeEntities();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ShortGuid test = ShortGuidUtils.Generate("test");
            return;

            EnvironmentAnimationDatabase db = new EnvironmentAnimationDatabase(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\ENVIRONMENT_ANIMATION.DAT");

            //RenderableElementsDatabase db2 = new RenderableElementsDatabase("reds.bin");
            //db2.RenderableElements.Add(new RenderableElementsDatabase.RenderableElement(){ MaterialLibraryIndex = 5, ModelIndex = 4, ModelLODIndex = 0, ModelLODPrimitiveCount = 2 });
            //db2.Save();

            //LocalDebug.FindAllNodesInCommands();
        }
        #endregion
    }
}
