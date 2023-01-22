using CATHODE;
using CATHODE.LEGACY.Assets;
using CATHODE.Scripting;
using CATHODE.LEGACY;
using CathodeEditorGUI.UserControls;
using CathodeLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Scripting.Internal;
using System.Threading;
using CathodeEditorGUI.Popups.UserControls;
using WebSocketSharp.Server;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI : Form
    {
        private TreeUtility treeHelper;

#if DEBUG
        private WebSocketServer server;
#endif

        public CathodeEditorGUI()
        {
            //LocalDebug.TestAllPhysMap();
            //return;


            InitializeComponent();
            treeHelper = new TreeUtility(FileTree);

#if DEBUG
            server = new WebSocketServer("ws://localhost:1702");
            server.AddWebSocketService<WebsocketServer>("/commands_editor");
            server.Start();
#endif

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

            show3D.Visible = false;
            DBG_WebsocketTest.Visible = false;
#if DEBUG
            show3D.Visible = true;
            DBG_WebsocketTest.Visible = true;
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
        public void EnableLoadingOfPaks(bool shouldEnable, string text = "Caching...")
        {
            load_commands_pak.Invoke(new Action(() => { load_commands_pak.Enabled = shouldEnable; load_commands_pak.Text = (shouldEnable) ? "Load" : text; }));
            save_commands_pak.Invoke(new Action(() => { save_commands_pak.Enabled = shouldEnable; }));
            env_list.Invoke(new Action(() => { env_list.Enabled = shouldEnable; }));
            enableBackups.Invoke(new Action(() => { enableBackups.Enabled = shouldEnable; }));
        }

        /* Load a COMMANDS.PAK into the editor with additional stuff */
        Task currentBackgroundCacher = null;
        public void LoadCommandsPAK(string level)
        {
            //Reset UI
            ClearUI(true, true, true);

            //Load everything
            LoadCommands(level);
            /*Task.Factory.StartNew(() => */LoadAssets()/*)*/;
            /*Task.Factory.StartNew(() => */LoadMovers()/*)*/;

            //Begin caching entity names so we don't have to keep generating them
            EntityUtils.LinkCommands(Editor.commands);
            ShortGuidUtils.LinkCommands(Editor.commands);
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
        }

        /* Load commands */
        private void LoadCommands(string level)
        {
            Editor.commands = null;

            string path_to_ENV = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + level;
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                Editor.commands = new Commands(path_to_ENV + "/WORLD/COMMANDS.PAK");
#if !CATHODE_FAIL_HARD
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\n" + e.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Editor.commands = null;
                return;
            }
#endif

            if (Editor.commands.EntryPoints == null)
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
            Editor.resource.models = null;
            Editor.resource.reds = null;
            Editor.resource.materials = null;
            Editor.resource.textures = null;
            Editor.resource.textures_Global = null;
            Editor.resource.env_animations = null;

#if !CATHODE_FAIL_HARD
            try
            {
#endif
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

                Editor.resource.models = new CathodeModels(baseLevelPath + "RENDERABLE/MODELS_LEVEL.BIN", baseLevelPath + "RENDERABLE/LEVEL_MODELS.PAK");
                Editor.resource.reds = new RenderableElementsDatabase(baseLevelPath + "WORLD/REDS.BIN");
                Editor.resource.materials = new MaterialDatabase(baseLevelPath + "RENDERABLE/LEVEL_MODELS.MTL");
                //Editor.resource.textures = new Textures(baseLevelPath + "RENDERABLE/LEVEL_TEXTURES.ALL.PAK");
                //Editor.resource.textures_Global = new Textures(SharedData.pathToAI + "/DATA/ENV/GLOBAL/WORLD/GLOBAL_TEXTURES.ALL.PAK");
                Editor.resource.env_animations = new EnvironmentAnimationDatabase(baseLevelPath + "WORLD/ENVIRONMENT_ANIMATION.DAT");
#if !CATHODE_FAIL_HARD
            }
            catch
            {
                //Can fail if we're loading a PAK outside the game structure
                MessageBox.Show("Failed to load asset PAKs!\nAre you opening a Commands PAK outside of a map directory?\nIf not, please try and load again.", "Resource editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Editor.resource.models = null;
                Editor.resource.reds = null;
                Editor.resource.materials = null;
                Editor.resource.textures = null;
                Editor.resource.textures_Global = null;
                Editor.resource.env_animations = null;
            }
#endif
        }

        /* Load mover descriptors */
        private void LoadMovers()
        {
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                string baseLevelPath = Editor.commands.Filepath.Substring(0, Editor.commands.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);
                Editor.mvr = new MoverDatabase(baseLevelPath + "WORLD/MODELS.MVR");
#if !CATHODE_FAIL_HARD
            }
            catch
            {
                //Can fail if we're loading a MVR outside the game structure
                MessageBox.Show("Failed to load mover descriptor database!\nAre you opening a Commands PAK outside of a map directory?\nMVR editing disabled.", "MVR editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Editor.mvr = null;
            }
#endif
        }

        /* Save the current edits */
        private void save_commands_pak_Click(object sender, EventArgs e)
        {
            if (Editor.commands == null) return;
            Cursor.Current = Cursors.WaitCursor;

#if !CATHODE_FAIL_HARD
            byte[] backup = null;
            try
            {
                backup = File.ReadAllBytes(Editor.commands.Filepath);
#endif
                Editor.commands.Save();
#if !CATHODE_FAIL_HARD
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
#endif

            if (Editor.resource.reds != null && Editor.resource.reds.Entries != null)
                Editor.resource.reds.Save();

            if (Editor.mvr != null && Editor.mvr.Entries != null)
                Editor.mvr.Save();

            Cursor.Current = Cursors.Default;
            MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /* Edit the loaded COMMANDS.PAK's root composite */
        private void editEntryPoint_Click(object sender, EventArgs e)
        {
            if (Editor.commands == null || Editor.commands.EntryPoints == null) return;
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
                    Entity entity = CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((OverrideEntity)Editor.selected.entity).hierarchy, out flow, out string hierarchy);
                    if (entity != null)
                    {
                        LoadComposite(flow.name);
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    Entity entity = CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((ProxyEntity)Editor.selected.entity).hierarchy, out flow, out string hierarchy);
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
            if (Editor.commands == null || Editor.commands.EntryPoints == null) return;
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
            for (int i = 0; i < Editor.commands.Entries.Count; i++)
            {
                List<FunctionEntity> prunedFunctionEntities = new List<FunctionEntity>();
                for (int x = 0; x < Editor.commands.Entries[i].functions.Count; x++)
                {
                    if (Editor.commands.Entries[i].functions[x].function == Editor.selected.composite.shortGUID) continue;
                    List<EntityLink> prunedEntityLinks = new List<EntityLink>();
                    for (int l = 0; l < Editor.commands.Entries[i].functions[x].childLinks.Count; l++)
                    {
                        Entity linkedEntity = Editor.commands.Entries[i].GetEntityByID(Editor.commands.Entries[i].functions[x].childLinks[l].childID);
                        if (linkedEntity != null && linkedEntity.variant == EntityVariant.FUNCTION) if (((FunctionEntity)linkedEntity).function == Editor.selected.composite.shortGUID) continue;
                        prunedEntityLinks.Add(Editor.commands.Entries[i].functions[x].childLinks[l]);
                    }
                    Editor.commands.Entries[i].functions[x].childLinks = prunedEntityLinks;
                    prunedFunctionEntities.Add(Editor.commands.Entries[i].functions[x]);
                }
                Editor.commands.Entries[i].functions = prunedFunctionEntities;
            }
            //TODO: remove proxies etc that also reference any of the removed entities

            //Remove the composite
            Editor.commands.Entries.Remove(Editor.selected.composite);

            //Refresh UI
            ClearUI(false, true, true);
            RefreshStatsUI();
            treeHelper.UpdateFileTree(Editor.commands.GetCompositeNames().ToList());
        }

        /* Select entity from loaded composite */
        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (composite_content.SelectedIndex == -1 || Editor.selected.composite == null) return;
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                ShortGuid entityID = new ShortGuid(composite_content.SelectedItem.ToString().Substring(1, 11));
                Entity thisEntity = Editor.selected.composite.GetEntityByID(entityID);
                if (thisEntity != null) LoadEntity(thisEntity);
#if !CATHODE_FAIL_HARD
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encountered an issue while looking up entity!\nPlease report this on GitHub!\n" + ex.Message, "Failed lookup!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }

        /* Search entity list */
        private string currentSearch = "";
        private void entity_search_btn_Click(object sender, EventArgs e)
        {
            DoSearch();
        }
        private void DoSearch()
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
            Composite entry = Editor.commands.GetComposite(FileName);
            CommandsUtils.PurgeDeadLinks(Editor.commands, entry);
            Editor.selected.composite = entry;
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
            CathodeEditorGUI_AddEntity add_parameter = new CathodeEditorGUI_AddEntity(Editor.selected.composite, Editor.commands.Entries);
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

            string removedID = Editor.selected.entity.shortGUID.ToByteString();

            switch (Editor.selected.entity.variant)
            {
                case EntityVariant.VARIABLE:
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
                            List<TriggerSequence.Trigger> triggers = new List<TriggerSequence.Trigger>();
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
                            List<CAGEAnimation.Header> headers = new List<CAGEAnimation.Header>();
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

            LoadComposite(Editor.selected.composite.name);

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
            if (newEnt.variant != EntityVariant.VARIABLE)
                EntityUtils.SetName(
                    Editor.selected.composite.shortGUID,
                    newEnt.shortGUID,
                    EntityUtils.GetName(Editor.selected.composite.shortGUID, Editor.selected.entity.shortGUID) + "_clone");

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
                case EntityVariant.VARIABLE:
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
            CathodeEditorGUI_RenameEntity rename_entity = new CathodeEditorGUI_RenameEntity(Editor.selected.composite, Editor.selected.entity);
            rename_entity.Show();
            rename_entity.OnSaved += OnEntityRenamed;
        }
        private void OnEntityRenamed(Composite composite, Entity entity)
        {
            string entityID = entity.shortGUID.ToByteString();
            string newEntityName = EditorUtils.GenerateEntityName(entity, composite, true);

            if (composite == Editor.selected.composite)
            {
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
                LoadEntity(entity);
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
            if (correctSelectedIndex == -1 && entity_search_box.Text != "")
            {
                entity_search_box.Text = "";
                DoSearch();
                correctSelectedIndex = composite_content.Items.IndexOf(EditorUtils.GenerateEntityName(entity, Editor.selected.composite));
            }
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
                    selected_entity_name.Text = EntityUtils.GetName(Editor.selected.composite.shortGUID, entity.shortGUID);
                    if (funcComposite == null)
                    {
                        FunctionType function = CommandsUtils.GetFunctionType(thisFunction);
                        editFunction.Enabled = function == FunctionType.CAGEAnimation || function == FunctionType.TriggerSequence;
                    }
                    editEntityResources.Enabled = (Editor.resource.models != null);
                    break;
                case EntityVariant.VARIABLE:
                    description = "DataType " + ((VariableEntity)entity).type.ToString();
                    selected_entity_name.Text = ShortGuidUtils.FindString(((VariableEntity)entity).name);
                    //renameSelectedNode.Enabled = false;
                    break;
                case EntityVariant.PROXY:
                case EntityVariant.OVERRIDE:
                    hierarchyDisplay.Visible = true;
                    string hierarchy = "";
                    if (entity.variant == EntityVariant.PROXY) CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((ProxyEntity)entity).hierarchy, out Composite comp, out hierarchy);
                    else CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((OverrideEntity)entity).hierarchy, out Composite comp, out hierarchy);
                    hierarchyDisplay.Text = hierarchy;
                    jumpToComposite.Visible = true;
                    selected_entity_name.Text = EntityUtils.GetName(Editor.selected.composite.shortGUID, entity.shortGUID);
                    break;
                default:
                    selected_entity_name.Text = EntityUtils.GetName(Editor.selected.composite.shortGUID, entity.shortGUID);
                    break;
            }
            selected_entity_type_description.Text = description;

            //show mvr editor button if this entity has a mvr link
            if (Editor.mvr != null && Editor.mvr.Entries.FindAll(o => o.commandsNodeID == Editor.selected.entity.shortGUID).Count != 0)
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
                        ((GUI_TransformDataType)parameterGUI).PopulateUI((cTransform)this_param, entity.parameters[i].name);
                        break;
                    case DataType.INTEGER:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Int((cInteger)this_param, entity.parameters[i].name);
                        break;
                    case DataType.STRING:
                        parameterGUI = new GUI_StringDataType();
                        ((GUI_StringDataType)parameterGUI).PopulateUI((cString)this_param, entity.parameters[i].name);
                        break;
                    case DataType.BOOL:
                        parameterGUI = new GUI_BoolDataType();
                        ((GUI_BoolDataType)parameterGUI).PopulateUI((cBool)this_param, entity.parameters[i].name);
                        break;
                    case DataType.FLOAT:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Float((cFloat)this_param, entity.parameters[i].name);
                        break;
                    case DataType.VECTOR:
                        parameterGUI = new GUI_VectorDataType();
                        ((GUI_VectorDataType)parameterGUI).PopulateUI((cVector3)this_param, entity.parameters[i].name);
                        break;
                    case DataType.ENUM:
                        parameterGUI = new GUI_EnumDataType();
                        ((GUI_EnumDataType)parameterGUI).PopulateUI((cEnum)this_param, entity.parameters[i].name);
                        break;
                    case DataType.RESOURCE:
                        parameterGUI = new GUI_ResourceDataType();
                        ((GUI_ResourceDataType)parameterGUI).PopulateUI((cResource)this_param, entity.parameters[i].name);
                        break;
                    case DataType.SPLINE:
                        parameterGUI = new GUI_SplineDataType();
                        ((GUI_SplineDataType)parameterGUI).PopulateUI((cSpline)this_param, entity.parameters[i].name);
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
            CathodeEditorGUI_AddOrEditLink add_link = new CathodeEditorGUI_AddOrEditLink(Editor.selected.composite, Editor.selected.entity);
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

        /* Enable/disable backups */
        Task backgroundBackups = null;
        CancellationToken backupCancellationToken;
        private void enableBackups_CheckedChanged(object sender, EventArgs e)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            backupCancellationToken = tokenSource.Token;
            if (backgroundBackups != null) tokenSource.Cancel();
            while (backgroundBackups != null) Thread.Sleep(100);

            if (enableBackups.Checked)
                backgroundBackups = Task.Factory.StartNew(() => BackupCommands(this));
        }
        private void BackupCommands(CathodeEditorGUI mainInst)
        {
            int i = 0;
            while (true)
            {
                i = 0;
                while (i < 300000)
                {
                    if (backupCancellationToken.IsCancellationRequested)
                    {
                        backgroundBackups = null;
                        return;
                    }
                    Thread.Sleep(500);
                    i += 500;
                }

                if (Editor.commands == null) continue;

                mainInst.EnableLoadingOfPaks(false, "Backup...");
                Editor.commands.Save(Editor.commands.Filepath + ".bak", false);
                mainInst.EnableLoadingOfPaks(true);
            }
        }

        /* Confirm an action */
        private bool ConfirmAction(string msg)
        {
            return (MessageBox.Show(msg, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

#region LOCAL_TESTS
        private void BuildNodeParameterDatabase(object sender, EventArgs e)
        {
            //This time, we load an existing Commands file, from the game's ENG_ALIEN_NEST level
            Commands commands = new Commands("Alien Isolation/DATA/ENV/PRODUCTION/ENG_ALIEN_NEST/WORLD/COMMANDS.PAK");

            //Create our new composite and set it as the one that loads first in the level's Commands
            Composite composite = commands.AddComposite("My Cool Script 2", true);

            //Create our checkpoint just like last time to act on the level entry, and apply its parameters
            FunctionEntity checkpoint = composite.AddFunction(FunctionType.Checkpoint, true);
            checkpoint.AddParameter("is_first_checkpoint", new cBool(true));
            checkpoint.AddParameter("section", new cString("Entry"));

            //Since we loaded in our Commands file, we can grab composites that already exist within it 
            Composite spawnPositionSelect = commands.GetComposite("ARCHETYPES\\SCRIPT\\MISSION\\SPAWNPOSITIONSELECT");

            //We can then create function entities that instance these composites to execute their functionality
            FunctionEntity playerSpawn = composite.AddFunction(spawnPositionSelect, true);

            //This particular composite implements a public variable called SpawnPlayer which spawns the player
            //Lets link to that public variable off of our checkpoint when it finishes loading
            checkpoint.AddParameterLink("finished_loading", playerSpawn, "SpawnPlayer");

            //Save the Commands out back to ENG_ALIEN_NEST
            commands.Save();


            LocalDebug.LoadAndSaveAllCommands();

            //File.WriteAllLines("out.txt", LocalDebug.CommandsToScript(new Commands(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\WORLD\COMMANDS.PAK")));
            //LocalDebug.DumpEnumList();
            //LocalDebug.DumpCathodeEntities();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            /*
            string commandsPath = "G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\ENG_ALIEN_NEST\\WORLD\\COMMANDS.PAK";
            //if (File.Exists(commandsPath)) File.Delete(commandsPath);

            Commands commands = new Commands(commandsPath);
            ///Composite composite = commands.EntryPoints[0];
            ///composite.functions.Clear();
            Composite composite = commands.AddComposite("bruh_moment", true);

            FunctionEntity checkpoint = composite.AddFunction(FunctionType.Checkpoint);
            FunctionEntity playerSpawn = composite.AddFunction(commands.GetComposite("ARCHETYPES\\SCRIPT\\MISSION\\SPAWNPOSITIONSELECT"));
            checkpoint.AddParameter("is_first_checkpoint", new cBool(true));
            checkpoint.AddParameter("section", new cString("Entry"));
            checkpoint.AddParameterLink("on_checkpoint", playerSpawn, "SpawnPlayer");

            commands.Save();


            string fgdg = "";
            */


            Commands commands = new Commands("G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\ENG_ALIEN_NEST\\WORLD\\COMMANDS.PAK");

            Composite composite = commands.AddComposite("My Cool Script5ert5", true);

            FunctionEntity objective = composite.AddFunction(FunctionType.SetPrimaryObjective, true);
            FunctionEntity checkpoint = composite.AddFunction(FunctionType.Checkpoint, true);
            FunctionEntity playerSpawn = composite.AddFunction(commands.GetComposite("ARCHETYPES\\SCRIPT\\MISSION\\SPAWNPOSITIONSELECT"), true);

            checkpoint.AddParameter("is_first_checkpoint", new cBool(true));
            checkpoint.AddParameter("section", new cString("Entry"));

            checkpoint.AddParameterLink("finished_loading", playerSpawn, "SpawnPlayer");
            checkpoint.AddParameterLink("finished_loading", objective, "trigger");

            objective.AddParameter("title", new cString("Do Something!"));
            objective.AddParameter("additional_info", new cString("Hey, you should go and do something!"));

            for (int i = 0; i < 99; i++)
            {
                Composite composite2 = commands.AddComposite("My Cool Script " + i, false);
                for (int x = 0; x < 99; x++)
                {

                    FunctionEntity checkpointe55 = composite.AddFunction(FunctionType.WEAPON_DidHitSomethingFilter, true);
                    FunctionEntity checkpoint55 = composite.AddFunction(FunctionType.Checkpoint, true);
                    for (int y = 0;y < 9; y++)
                    {
                        checkpoint55.AddParameterLink("finished_loading", objective, "trigger");
                        checkpointe55.AddParameterLink("sfhgjsdh", objective, "trigger");
                        checkpoint55.AddParameterLink("sdfsdf", objective, "trigger");
                    }
                }
            }

            commands.Save();


            /*
            //Create our Commands file to contain our scripts
            //File.Delete("G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\ENG_ALIEN_NEST\\WORLD\\COMMANDS.PAK");
            Commands commands = new Commands("G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\ENG_ALIEN_NEST\\WORLD\\COMMANDS.PAK");

            Composite composite = commands.AddComposite("My Cool Script 2", true);

            FunctionEntity checkpoint = composite.AddFunction(FunctionType.Checkpoint, true);
            FunctionEntity objective = composite.AddFunction(FunctionType.SetPrimaryObjective, true);
            FunctionEntity playerSpawn = composite.AddFunction(commands.GetComposite("ARCHETYPES\\SCRIPT\\MISSION\\SPAWNPOSITIONSELECT"));

            checkpoint.AddParameter("is_first_checkpoint", new cBool(true));
            checkpoint.AddParameter("section", new cString("Entry"));

            checkpoint.AddParameterLink("finished_loading", objective, "trigger");
            checkpoint.AddParameterLink("finished_loading", playerSpawn, "SpawnPlayer");

            //Save the Commands file
            commands.Save();
            */




            /*
            Commands commands = new Commands(commandsPath);

            Composite composite = commands.AddComposite(@"My Cool Script", true);

            FunctionEntity checkpoint = composite.AddFunction(FunctionType.Checkpoint);
            checkpoint.AddParameter("is_first_checkpoint", new cBool(true));
            checkpoint.AddParameter("section", new cString("Entry"));

            FunctionEntity fadeIn = composite.AddFunction(FunctionType.ScreenFadeInTimed);
            fadeIn.AddParameter("time", new cFloat(2.0f));
            checkpoint.AddParameterLink("finished_loading", fadeIn, "start");

            FunctionEntity player = composite.AddFunction(FunctionType.Character);
            player.AddParameter("anim_set", new cString("1ST_PERSON"));
            player.AddParameter("spawn", new cFloat(0.1f));
            player.AddParameter("reference_skeleton", new cString("FEMALE_FP"));
            player.AddParameter("is_player", new cBool(true));
            player.AddParameter("activate_on_reset", new cString("true"));
            player.AddParameter("attribute_set", new cString("THE_PLAYER"));
            player.AddParameter("footwear_sound", new cString("Trainers"));
            player.AddParameter("alliance_group", new cEnum("ALLIANCE_GROUP", 1));
            player.AddParameter(new ShortGuid("CD-8E-31-AF"), new cString("THE_PLAYER"));
            player.AddParameter("position", new cTransform(new Vector3(12.05990000f, 7.60228000f, -12.62150000f), new Vector3(-0.00000149f, -135.00000000f, 0.00000042f)));
            player.AddParameter("display_model", new cString("PLAYER_FP"));
            player.AddParameter("character_class", new cEnum("CHARACTER_CLASS", 0));
            player.AddParameter("spawn_on_reset", new cBool(false));
            player.AddParameter("finished_spawning", new cFloat(0.1f));
            fadeIn.AddParameterLink("started", player, "spawn");

            FunctionEntity objective = composite.AddFunction(FunctionType.SetPrimaryObjective);
            objective.AddParameter("title", new cString("You Bruh!"));
            objective.AddParameter("additional_info", new cString("Hey, you should go and do something!"));
            player.AddParameterLink("spawned", objective, "trigger"); //why is finished_spawning not triggering?

            FunctionEntity giveMotionTracker = composite.AddFunction(FunctionType.WEAPON_GiveToPlayer);
            giveMotionTracker.AddParameter("weapon", new cEnum("EQUIPMENT_SLOT", 6));
            player.AddParameterLink("spawned", giveMotionTracker, "trigger");

            FunctionEntity globalSpawnEvent = composite.AddFunction(FunctionType.GlobalEvent);
            globalSpawnEvent.AddParameter("EventName", new cString("Player_Spawned"));
            globalSpawnEvent.AddParameter("EventValue", new cFloat(3.0f));
            player.AddParameterLink("spawned", globalSpawnEvent, "trigger");

            FunctionEntity alwaysReturnsTrue = composite.AddFunction(FunctionType.FloatLessThanOrEqual);
            alwaysReturnsTrue.AddParameter("LHS", new cFloat(1.0f));
            alwaysReturnsTrue.AddParameter("RHS", new cFloat(1.0f));
            player.AddParameterLink("spawned", alwaysReturnsTrue, "trigger");

            FunctionEntity enableFunctionality = composite.AddFunction(FunctionType.ToggleFunctionality);
            enableFunctionality.AddParameterLink("enable_radial", alwaysReturnsTrue, "on_true");
            enableFunctionality.AddParameterLink("enable_radial_hacking_info", alwaysReturnsTrue, "on_true");
            enableFunctionality.AddParameterLink("enable_radial_cutting_info", alwaysReturnsTrue, "on_true");
            enableFunctionality.AddParameterLink("enable_radial_battery_info", alwaysReturnsTrue, "on_true");
            enableFunctionality.AddParameterLink("enable_hud_battery_info", alwaysReturnsTrue, "on_true");

            //TODO do we need to TriggerBindCharacter to VariableThePlayer?


            FunctionEntity map = composite.AddFunction(FunctionType.MapAnchor);
            map.AddParameter("map_scale", new cFloat(2.7f));
            map.AddParameter("keyframe", new cString("Bsp_Torrens_1"));
            map.AddParameter("world_pos", new cTransform(new Vector3(-5.12215000f, 7.79390000f, -0.74270500f), new Vector3(0, 0, 0)));
            player.AddParameterLink("spawned", map, "trigger");


            commands.Save();
            */

            //LocalDebug.FindAllNodesInCommands();
        }


#endregion

        GUI_ModelViewer modelViewer = null;
        private void show3D_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_Composite3D viewer = new CathodeEditorGUI_Composite3D(Editor.selected.composite);
            viewer.Show();
        }

        private string levelLoaded = "";
        private void button1_Click(object sender, EventArgs e)
        {
#if DEBUG
            if (levelLoaded != env_list.SelectedItem.ToString())
            {
                levelLoaded = env_list.SelectedItem.ToString();
                server.WebSocketServices["/commands_editor"].Sessions.Broadcast("1" + levelLoaded);
                return;
            }

            try
            {
                //Vector3 vec = ((cTransform)Editor.selected.entity.GetParameter("position").content).position;
                //server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)MessageType.GO_TO_POSITION).ToString() + vec.X + ">" + vec.Y + ">" + vec.Z);

                string str = "";
                for (int i = 0; i < Editor.mvr.Entries.Count; i++)
                {
                    if (Editor.mvr.Entries[i].commandsNodeID != Editor.selected.entity.shortGUID) continue;
                    str += i + ">";
                }
                server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)MessageType.GO_TO_REDS).ToString() + str);
            }
            catch { }
#endif
        }
    }
}
