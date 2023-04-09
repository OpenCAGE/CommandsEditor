using CATHODE;
using CATHODE.Scripting;
using CATHODE.LEGACY;
using CommandsEditor.UserControls;
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
using CommandsEditor.Popups.UserControls;
using WebSocketSharp.Server;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using static System.Net.Mime.MediaTypeNames;
using CATHODE.EXPERIMENTAL;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Security.Policy;
using Newtonsoft.Json;
using OpenCAGE;

namespace CommandsEditor
{
    public partial class CommandsEditor : Form
    {
        public Editor Loaded = new Editor();

        private TreeUtility _treeHelper;
        private Composite _previousComposite = null;

        private WebSocketServer _server;
        private WebsocketServer _serverLogic;
        private readonly string _serverOpt = "CE_ConnectToUnity";
        private readonly string _backupsOpt = "CS_EnableBackups";
        private readonly string _nodeOpt = "CS_NodeView";

        public CommandsEditor()
        {
            //LocalDebug.SyncEnumValuesAndDump();
            //return;

            //LocalDebug.SyncEnumValuesAndDump();
            //return;

            //ShortGuid guid = ShortGuidUtils.Generate("gravity_force");
            //Console.WriteLine(guid.ToByteString());
            //return;
            //
            //LocalDebug.CommandsTest();
            //string sdfd = "";

            //LocalDebug.CommandsTest();
            //return;

            /*
            LocalDebug.TestAllCmds();
            return;


            Commands commands = new Commands("G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\DLC\\BSPNOSTROMO_TWOTEAMS_PATCH\\WORLD\\COMMANDS.PAK");
            CAGEAnimation animNode = (CAGEAnimation)commands.GetComposite("DLC\\PREORDER\\PODLC_TWOTEAMS").functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation));
            File.WriteAllText("out.json", JsonConvert.SerializeObject(animNode, Formatting.Indented));
            /*
            foreach (CAGEAnimation.Animation key in animNode.animations)
            {
                foreach (CAGEAnimation.Animation.Keyframe keyD in key.keyframes)
                {
                    keyD.unk2 = 10;
                    keyD.unk3 = 0;
                    keyD.unk4 = -5;
                    keyD.unk5 = 0;
                }
            }
            *//*
            commands.Save();

            Environment.Exit(0);
            return;

            */

            EditorUtils.SetEditor(this);
            InitializeComponent();
            _treeHelper = new TreeUtility(FileTree);

            enableBackups.Checked = SettingsManager.GetBool(_backupsOpt);
            UnityConnection.Checked = SettingsManager.GetBool(_serverOpt);
            showNodeViewer.Checked = SettingsManager.GetBool(_nodeOpt);

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

            this.Text = "OpenCAGE Commands Editor";
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

            //Populate localised text string databases (in English)
            List<string> textList = Directory.GetFiles(SharedData.pathToAI + "/DATA/TEXT/ENGLISH/", "*.TXT", SearchOption.AllDirectories).ToList<string>();
            foreach (string text in textList)
            {
                Loaded.strings.Add(Path.GetFileNameWithoutExtension(text), new Strings(text));
            }

            //Populate animation strings
            string pathToStringDB = SharedData.pathToAI + "/DATA/GLOBAL/ANIM_STRING_DB.BIN";
            string pathToStringDB_Debug = SharedData.pathToAI + "/DATA/GLOBAL/ANIM_STRING_DB_DEBUG.BIN";
            if (!File.Exists(pathToStringDB) || !File.Exists(pathToStringDB_Debug))
            {
                PAK2 animPAK = new PAK2(SharedData.pathToAI + "/DATA/GLOBAL/ANIMATION.PAK");
                byte[] content = animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB.BIN")).Content;
                File.WriteAllBytes(pathToStringDB, content);
                content = animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB_DEBUG.BIN")).Content;
                File.WriteAllBytes(pathToStringDB_Debug, content);
            }
            Loaded.animstrings = new AnimationStrings(pathToStringDB);
            Loaded.animstrings_debug = new AnimationStrings(pathToStringDB_Debug);

            ClearUI(true, true, true);

            show3D.Visible = false;
#if DEBUG
            show3D.Visible = true;
#endif
        }

        private void CommandsEditor_Load(object sender, EventArgs e)
        {
            return;
#if DEBUG
            env_list.SelectedItem = "DLC\\BSPNOSTROMO_TWOTEAMS_PATCH";
            LoadCommandsPAK(env_list.SelectedItem.ToString());
            LoadComposite("DLC\\PREORDER\\PODLC_TWOTEAMS");
            LoadEntity(Loaded.selected.composite.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation)));
            editFunction.PerformClick();
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
                _previousComposite = null;
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
                Loaded.selected.composite = null;
            }
            if (clear_parameter_list)
            {
                Loaded.selected.entity = null;
                entityInfoGroup.Text = "Selected Entity Info";
                entityParamGroup.Text = "Selected Entity Parameters";
                selected_entity_type_description.Text = "";
                selected_entity_name.Text = "";
                for (int i = 0; i < entity_params.Controls.Count; i++) 
                    entity_params.Controls[i].Dispose();
                entity_params.Controls.Clear();
                jumpToComposite.Visible = false;
                editFunction.Enabled = false;
                editEntityResources.Enabled = false;
                editEntityMovers.Enabled = false;
                showOverridesAndProxies.Enabled = false;
                goToZone.Enabled = false;
                renameSelectedNode.Enabled = true;
                duplicateSelectedNode.Enabled = true;
                hierarchyDisplay.Visible = false;
                addNewParameter.Enabled = true;
                removeParameter.Enabled = true;
            }
            goBackToPrevComp.Enabled = _previousComposite != null;
            toolTip1.SetToolTip(goBackToPrevComp, "Go back to: " + ((_previousComposite != null) ? _previousComposite.name : ""));
            if (nodeViewer != null) nodeViewer.AddEntities(null, null);
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
            EntityUtils.LinkCommands(Loaded.commands);
            ShortGuidUtils.LinkCommands(Loaded.commands);
            if (currentBackgroundCacher != null) currentBackgroundCacher.Dispose();
            currentBackgroundCacher = Task.Factory.StartNew(() => EditorUtils.GenerateEntityNameCache(this));

            //Populate file tree
            _treeHelper.UpdateFileTree(Loaded.commands.GetCompositeNames().ToList());

            //Show info in UI
            RefreshStatsUI();

            //Load root composite
            _treeHelper.SelectNode(Loaded.commands.EntryPoints[0].name);

            //Force collect
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            //TEMP: Testing out new brute-forced ShortGuids
            ShortGuidUtils.Generate("Win");
            ShortGuidUtils.Generate("End");
            ShortGuidUtils.Generate("Cut");
            for (int i = 0; i < 26; i++)
                ShortGuidUtils.Generate("cut" + i);
            //ShortGuidUtils.Generate("temp");
            //ShortGuidUtils.Generate("test");
            //ShortGuidUtils.Generate("M01");
            //ShortGuidUtils.Generate("M02");
            //ShortGuidUtils.Generate("M03");
            //ShortGuidUtils.Generate("TEST");
            //ShortGuidUtils.Generate("CORE");
            //ShortGuidUtils.Generate("left");
            //ShortGuidUtils.Generate("back");
            //ShortGuidUtils.Generate("bind");
            //ShortGuidUtils.Generate("cam");
        }
        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            LoadCommandsPAK(env_list.SelectedItem.ToString());
        }
        private void RefreshStatsUI()
        {
            root_composite_display.Text = "Root composite: " + Loaded.commands.EntryPoints[0].name;
        }

        /* Load commands */
        private void LoadCommands(string level)
        {
            if (Loaded.commands != null) Loaded.commands.Entries.Clear();
            Loaded.level = level;

            string path_to_ENV = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + level;
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                Loaded.commands = new Commands(path_to_ENV + "/WORLD/COMMANDS.PAK");
                Loaded.OnCommandsSelected?.Invoke(Loaded.commands);
#if !CATHODE_FAIL_HARD
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\n" + e.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Loaded.commands = null;
                return;
            }
#endif
            RefreshWebsocket();

            if (Loaded.commands.EntryPoints == null)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\nPlease place this executable in your Alien: Isolation folder.", "Environment error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (Loaded.commands.EntryPoints[0] == null)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\nPlease reset your game files.", "COMMANDS.PAK corrupted!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /* Load assets */
        private void LoadAssets()
        {
            if (Loaded.resource.models != null) Loaded.resource.models.Entries.Clear();
            if (Loaded.resource.reds != null) Loaded.resource.reds.Entries.Clear();
            if (Loaded.resource.materials != null) Loaded.resource.materials.Entries.Clear();
            if (Loaded.resource.textures != null) Loaded.resource.textures.Entries.Clear();
            if (Loaded.resource.textures_global != null) Loaded.resource.textures_global.Entries.Clear();
            if (Loaded.resource.env_animations != null) Loaded.resource.env_animations.Entries.Clear();
            if (Loaded.resource.collision_maps != null) Loaded.resource.collision_maps.Entries.Clear();
            if (Loaded.resource.sound_bankdata != null) Loaded.resource.sound_bankdata.Entries.Clear();
            if (Loaded.resource.sound_dialoguelookups != null) Loaded.resource.sound_dialoguelookups.Entries.Clear();
            if (Loaded.resource.sound_eventdata != null) Loaded.resource.sound_eventdata.Entries.Clear();
            if (Loaded.resource.sound_environmentdata != null) Loaded.resource.sound_environmentdata.Entries.Clear();

#if !CATHODE_FAIL_HARD
            try
            {
#endif
                string baseLevelPath = Loaded.commands.Filepath.Substring(0, Loaded.commands.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);

                //The game has two hard-coded _PATCH overrides which change the CommandsPAK but not the assets
                string levelName = env_list.Items[env_list.SelectedIndex].ToString();
                switch (levelName)
                {
                    case @"DLC\BSPNOSTROMO_RIPLEY_PATCH":
                    case @"DLC\BSPNOSTROMO_TWOTEAMS_PATCH":
                        baseLevelPath = baseLevelPath.Replace(levelName, levelName.Substring(0, levelName.Length - ("_PATCH").Length));
                        break;
                }

                Loaded.resource.models = new Models(baseLevelPath + "RENDERABLE/LEVEL_MODELS.PAK");
                Loaded.resource.reds = new RenderableElements(baseLevelPath + "WORLD/REDS.BIN");
                Loaded.resource.materials = new Materials(baseLevelPath + "RENDERABLE/LEVEL_MODELS.MTL");
                //Editor.resource.textures = new Textures(baseLevelPath + "RENDERABLE/LEVEL_TEXTURES.ALL.PAK");
                //Editor.resource.textures_Global = new Textures(SharedData.pathToAI + "/DATA/ENV/GLOBAL/WORLD/GLOBAL_TEXTURES.ALL.PAK");
                Loaded.resource.env_animations = new EnvironmentAnimations(baseLevelPath + "WORLD/ENVIRONMENT_ANIMATION.DAT");
                Loaded.resource.collision_maps = new CollisionMaps(baseLevelPath + "WORLD/COLLISION.MAP");
                Loaded.resource.sound_bankdata = new SoundBankData(baseLevelPath + "WORLD/SOUNDBANKDATA.DAT");
                Loaded.resource.sound_dialoguelookups = new SoundDialogueLookups(baseLevelPath + "WORLD/SOUNDDIALOGUELOOKUPS.DAT");
                Loaded.resource.sound_eventdata = new SoundEventData(baseLevelPath + "WORLD/SOUNDEVENTDATA.DAT");
                Loaded.resource.sound_environmentdata = new SoundEnvironmentData(baseLevelPath + "WORLD/SOUNDENVIRONMENTDATA.DAT");
#if !CATHODE_FAIL_HARD
            }
            catch
            {
                //Can fail if we're loading a PAK outside the game structure
                MessageBox.Show("Failed to load asset PAKs!\nAre you opening a Commands PAK outside of a map directory?\nIf not, please try and load again.", "Resource editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Loaded.resource.models = null;
                Loaded.resource.reds = null;
                Loaded.resource.materials = null;
                Loaded.resource.textures = null;
                Loaded.resource.textures_global = null;
                Loaded.resource.env_animations = null;
                Loaded.resource.collision_maps = null;
                Loaded.resource.sound_bankdata = null;
                Loaded.resource.sound_dialoguelookups = null;
                Loaded.resource.sound_eventdata = null;
                Loaded.resource.sound_environmentdata = null;
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
                string baseLevelPath = Loaded.commands.Filepath.Substring(0, Loaded.commands.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);
                Loaded.mvr = new Movers(baseLevelPath + "WORLD/MODELS.MVR");
#if !CATHODE_FAIL_HARD
            }
            catch
            {
                //Can fail if we're loading a MVR outside the game structure
                MessageBox.Show("Failed to load mover descriptor database!\nAre you opening a Commands PAK outside of a map directory?\nMVR editing disabled.", "MVR editing disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Loaded.mvr = null;
            }
#endif
        }

        /* Save the current edits */
        private void save_commands_pak_Click(object sender, EventArgs e)
        {
            if (Loaded.commands == null) return;
            Cursor.Current = Cursors.WaitCursor;

#if !CATHODE_FAIL_HARD
            byte[] backup = null;
            try
            {
                backup = File.ReadAllBytes(Loaded.commands.Filepath);
#endif
                Loaded.commands.Save();
#if !CATHODE_FAIL_HARD
            }
            catch (Exception ex)
            {
                try
                {
                    if (backup != null)
                        File.WriteAllBytes(Loaded.commands.Filepath, backup);
                }
                catch { }
            
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Failed to save changes!\n" + ex.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#endif

            if (Loaded.resource.reds != null && Loaded.resource.reds.Entries != null)
                Loaded.resource.reds.Save();

            if (Loaded.mvr != null && Loaded.mvr.Entries != null)
                Loaded.mvr.Save();

            Cursor.Current = Cursors.Default;
            MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /* Edit the loaded COMMANDS.PAK's root composite */
        private void editEntryPoint_Click(object sender, EventArgs e)
        {
            if (Loaded.commands == null || Loaded.commands.EntryPoints == null) return;
            EditRootComposite edit_entrypoint = new EditRootComposite(this);
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
            switch (Loaded.selected.entity.variant)
            {
                case EntityVariant.OVERRIDE:
                {
                    Entity entity = CommandsUtils.ResolveHierarchy(Loaded.commands, Loaded.selected.composite, ((OverrideEntity)Loaded.selected.entity).connectedEntity.hierarchy, out flow, out string hierarchy);
                    if (entity != null)
                    {
                        LoadComposite(flow.name);
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    Entity entity = CommandsUtils.ResolveHierarchy(Loaded.commands, Loaded.selected.composite, ((ProxyEntity)Loaded.selected.entity).connectedEntity.hierarchy, out flow, out string hierarchy);
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
            if (Loaded.commands == null || Loaded.commands.EntryPoints == null) return;
            LoadComposite(Loaded.commands.EntryPoints[0].name);
        }

        /* Add new composite (really we should be able to do this OFF OF entities, like making a prefab) */
        private void addNewComposite_Click(object sender, EventArgs e)
        {
            if (Loaded.commands == null) return;
            AddComposite add_flow = new AddComposite(this);
            add_flow.Show();
            add_flow.FormClosed += new FormClosedEventHandler(add_flow_closed);
        }
        private void add_flow_closed(Object sender, FormClosedEventArgs e)
        {
            _treeHelper.UpdateFileTree(Loaded.commands.GetCompositeNames().ToList());
            RefreshStatsUI();
            this.BringToFront();
            this.Focus();
        }

        /* Remove selected composite */
        private void removeSelectedComposite_Click(object sender, EventArgs e)
        {
            if (Loaded.selected.composite == null) return;
            for (int i = 0; i < Loaded.commands.EntryPoints.Count(); i++)
            {
                if (Loaded.selected.composite.shortGUID == Loaded.commands.EntryPoints[i].shortGUID)
                {
                    MessageBox.Show("Cannot delete a composite which is the root, global, or pause menu!", "Cannot delete.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (!ConfirmAction("Are you sure you want to remove this composite?")) return;

            //Remove any entities or links that reference this composite
            for (int i = 0; i < Loaded.commands.Entries.Count; i++)
            {
                List<FunctionEntity> prunedFunctionEntities = new List<FunctionEntity>();
                for (int x = 0; x < Loaded.commands.Entries[i].functions.Count; x++)
                {
                    if (Loaded.commands.Entries[i].functions[x].function == Loaded.selected.composite.shortGUID) continue;
                    List<EntityLink> prunedEntityLinks = new List<EntityLink>();
                    for (int l = 0; l < Loaded.commands.Entries[i].functions[x].childLinks.Count; l++)
                    {
                        Entity linkedEntity = Loaded.commands.Entries[i].GetEntityByID(Loaded.commands.Entries[i].functions[x].childLinks[l].childID);
                        if (linkedEntity != null && linkedEntity.variant == EntityVariant.FUNCTION) if (((FunctionEntity)linkedEntity).function == Loaded.selected.composite.shortGUID) continue;
                        prunedEntityLinks.Add(Loaded.commands.Entries[i].functions[x].childLinks[l]);
                    }
                    Loaded.commands.Entries[i].functions[x].childLinks = prunedEntityLinks;
                    prunedFunctionEntities.Add(Loaded.commands.Entries[i].functions[x]);
                }
                Loaded.commands.Entries[i].functions = prunedFunctionEntities;
            }
            //TODO: remove proxies etc that also reference any of the removed entities

            //Remove the composite
            Loaded.commands.Entries.Remove(Loaded.selected.composite);

            //Refresh UI
            ClearUI(false, true, true);
            RefreshStatsUI();
            _treeHelper.UpdateFileTree(Loaded.commands.GetCompositeNames().ToList());
        }

        /* Select entity from loaded composite */
        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (composite_content.SelectedIndex == -1 || Loaded.selected.composite == null) return;
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                ShortGuid entityID = new ShortGuid(composite_content.SelectedItem.ToString().Substring(1, 11));
                Entity thisEntity = Loaded.selected.composite.GetEntityByID(entityID);
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
        private void LoadComposite(string filename)
        {
            LoadComposite(Loaded.commands.GetComposite(filename));
        }
        private void LoadComposite(Composite comp, Entity ent = null)
        {
            _previousComposite = Loaded.selected.composite;
            ClearUI(false, true, true);
            Loaded.selected.composite = comp;
            Loaded.OnCompositeSelected?.Invoke(Loaded.selected.composite);

            Cursor.Current = Cursors.WaitCursor;
            CommandsUtils.PurgeDeadLinks(Loaded.commands, comp);

            composite_content.BeginUpdate();
            List<Entity> entities = comp.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                string desc = EditorUtils.GenerateEntityName(entities[i], Loaded.selected.composite);
                composite_content.Items.Add(desc);
                composite_content_RAW.Add(desc);
            }
            composite_content.EndUpdate();

            groupBox1.Text = comp.name;
            Cursor.Current = Cursors.Default;

            if (ent != null) LoadEntity(ent);
        }

        /* Add new entity */
        private void addNewEntity_Click(object sender, EventArgs e)
        {
            if (Loaded.selected.composite == null) return;
            AddEntity add_parameter = new AddEntity(this, Loaded.selected.composite, Loaded.commands.Entries);
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
            if (Loaded.selected.entity == null) return;
            if (!ConfirmAction("Are you sure you want to remove this entity?")) return;

            string removedID = Loaded.selected.entity.shortGUID.ToByteString();

            switch (Loaded.selected.entity.variant)
            {
                case EntityVariant.VARIABLE:
                    Loaded.selected.composite.variables.Remove((VariableEntity)Loaded.selected.entity);
                    break;
                case EntityVariant.FUNCTION:
                    Loaded.selected.composite.functions.Remove((FunctionEntity)Loaded.selected.entity);
                    break;
                case EntityVariant.OVERRIDE:
                    Loaded.selected.composite.overrides.Remove((OverrideEntity)Loaded.selected.entity);
                    break;
                case EntityVariant.PROXY:
                    Loaded.selected.composite.proxies.Remove((ProxyEntity)Loaded.selected.entity);
                    break;
            }

            List<Entity> entities = Loaded.selected.composite.GetEntities();
            for (int i = 0; i < entities.Count; i++) //We should actually query every entity in the PAK, since we might be ref'd by a proxy or override
            {
                List<EntityLink> entLinks = new List<EntityLink>();
                for (int x = 0; x < entities[i].childLinks.Count; x++)
                {
                    if (entities[i].childLinks[x].childID != Loaded.selected.entity.shortGUID) entLinks.Add(entities[i].childLinks[x]);
                }
                entities[i].childLinks = entLinks;

                if (entities[i].variant == EntityVariant.FUNCTION)
                {
                    string entType = ShortGuidUtils.FindString(((FunctionEntity)entities[i]).function);
                    switch (entType)
                    {
                        case "TriggerSequence":
                            TriggerSequence triggerSequence = (TriggerSequence)entities[i];
                            List<TriggerSequence.Entity> triggers = new List<TriggerSequence.Entity>();
                            for (int x = 0; x < triggerSequence.entities.Count; x++)
                            {
                                if (triggerSequence.entities[x].connectedEntity.hierarchy.Count < 2 ||
                                    triggerSequence.entities[x].connectedEntity.hierarchy[triggerSequence.entities[x].connectedEntity.hierarchy.Count - 2] != Loaded.selected.entity.shortGUID)
                                {
                                    triggers.Add(triggerSequence.entities[x]);
                                }
                            }
                            triggerSequence.entities = triggers;
                            break;
                        case "CAGEAnimation":
                            CAGEAnimation cageAnim = (CAGEAnimation)entities[i];
                            List<CAGEAnimation.Connection> headers = new List<CAGEAnimation.Connection>();
                            for (int x = 0; x < cageAnim.connections.Count; x++)
                            {
                                if (cageAnim.connections[x].connectedEntity.hierarchy.Count < 2 ||
                                    cageAnim.connections[x].connectedEntity.hierarchy[cageAnim.connections[x].connectedEntity.hierarchy.Count - 2] != Loaded.selected.entity.shortGUID)
                                {
                                    headers.Add(cageAnim.connections[x]);
                                }
                            }
                            cageAnim.connections = headers;
                            break;
                    }
                }
            }

            LoadComposite(Loaded.selected.composite.name);

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
            if (Loaded.selected.entity == null) return;
            if (!ConfirmAction("Are you sure you want to duplicate this entity?")) return;

            //Generate new entity ID and name
            Entity newEnt = Loaded.selected.entity.Copy();
            newEnt.shortGUID = ShortGuidUtils.GenerateRandom();
            if (newEnt.variant != EntityVariant.VARIABLE)
                EntityUtils.SetName(
                    Loaded.selected.composite.shortGUID,
                    newEnt.shortGUID,
                    EntityUtils.GetName(Loaded.selected.composite.shortGUID, Loaded.selected.entity.shortGUID) + "_clone");

            //Add parent links in to this entity that linked in to the other entity
            List<Entity> ents = Loaded.selected.composite.GetEntities();
            List<EntityLink> newLinks = new List<EntityLink>();
            int num_of_new_things = 1;
            foreach (Entity entity in ents)
            {
                newLinks.Clear();
                foreach (EntityLink link in entity.childLinks)
                {
                    if (link.childID == Loaded.selected.entity.shortGUID)
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
                    Loaded.selected.composite.functions.Add((FunctionEntity)newEnt);
                    break;
                case EntityVariant.VARIABLE:
                    Loaded.selected.composite.variables.Add((VariableEntity)newEnt);
                    break;
                case EntityVariant.PROXY:
                    Loaded.selected.composite.proxies.Add((ProxyEntity)newEnt);
                    break;
                case EntityVariant.OVERRIDE:
                    Loaded.selected.composite.overrides.Add((OverrideEntity)newEnt);
                    break;
            }

            //Load in to UI
            ReloadUIForNewEntity(newEnt);
        }

        /* Rename selected entity */
        private void renameSelectedEntity_Click(object sender, EventArgs e)
        {
            if (Loaded.selected.entity == null) return;
            RenameEntity rename_entity = new RenameEntity(this, Loaded.selected.composite, Loaded.selected.entity);
            rename_entity.Show();
            rename_entity.OnSaved += OnEntityRenamed;
        }
        private void OnEntityRenamed(Composite composite, Entity entity)
        {
            string entityID = entity.shortGUID.ToByteString();
            string newEntityName = EditorUtils.GenerateEntityName(entity, composite, true);

            if (composite == Loaded.selected.composite)
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
            if (Loaded.selected.composite == null || newEnt == null) return;
            if (currentSearch == "")
            {
                string newEntityName = EditorUtils.GenerateEntityName(newEnt, Loaded.selected.composite);
                composite_content.Items.Add(newEntityName);
                composite_content_RAW.Add(newEntityName);
            }
            else
            {
                LoadComposite(Loaded.selected.composite.name);
            }
            LoadEntity(newEnt);
        }

        /* Load a entity into the UI */
        private List<Entity> parentEntities = new List<Entity>();
        private List<Entity> childEntities = new List<Entity>();
        private void LoadEntity(Entity entity)
        {
            ClearUI(false, false, true);
            Loaded.selected.entity = entity;
            Loaded.OnEntitySelected?.Invoke(Loaded.selected.entity);
            RefreshWebsocket();

            //Correct the UI, and return early if we have to change index, so we don't trigger twice
            int correctSelectedIndex = composite_content.Items.IndexOf(EditorUtils.GenerateEntityName(entity, Loaded.selected.composite));
            if (correctSelectedIndex == -1 && entity_search_box.Text != "")
            {
                entity_search_box.Text = "";
                DoSearch();
                correctSelectedIndex = composite_content.Items.IndexOf(EditorUtils.GenerateEntityName(entity, Loaded.selected.composite));
            }
            if (composite_content.SelectedIndex != correctSelectedIndex)
            {
                composite_content.SelectedIndex = correctSelectedIndex;
                return;
            }

            if (entity == null) return;

            Cursor.Current = Cursors.WaitCursor;
            entity_params.SuspendLayout();
            Task.Factory.StartNew(() => BackgroundEntityLoader(entity, this));

            //populate info labels
            entityInfoGroup.Text = "Selected " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.variant.ToString().ToLower().Replace('_', ' ')) + " Entity Info";
            entityParamGroup.Text = "Selected " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.variant.ToString().ToLower().Replace('_', ' ')) + " Entity Parameters";
            string description = "";
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    ShortGuid thisFunction = ((FunctionEntity)entity).function;
                    Composite funcComposite = Loaded.commands.GetComposite(thisFunction);
                    jumpToComposite.Visible = funcComposite != null;
                    if (funcComposite != null)
                        description = funcComposite.name;
                    else
                        description = ShortGuidUtils.FindString(thisFunction);
                    selected_entity_name.Text = EntityUtils.GetName(Loaded.selected.composite.shortGUID, entity.shortGUID);
                    if (funcComposite == null)
                    {
                        FunctionType function = CommandsUtils.GetFunctionType(thisFunction);
                        editFunction.Enabled = function == FunctionType.CAGEAnimation || function == FunctionType.TriggerSequence;
                    }
                    editEntityResources.Enabled = (Loaded.resource.models != null);
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
                    if (entity.variant == EntityVariant.PROXY) CommandsUtils.ResolveHierarchy(Loaded.commands, Loaded.selected.composite, ((ProxyEntity)entity).connectedEntity.hierarchy, out Composite comp, out hierarchy);
                    else CommandsUtils.ResolveHierarchy(Loaded.commands, Loaded.selected.composite, ((OverrideEntity)entity).connectedEntity.hierarchy, out Composite comp, out hierarchy);
                    hierarchyDisplay.Text = hierarchy;
                    jumpToComposite.Visible = true;
                    selected_entity_name.Text = EntityUtils.GetName(Loaded.selected.composite.shortGUID, entity.shortGUID);
                    break;
                default:
                    selected_entity_name.Text = EntityUtils.GetName(Loaded.selected.composite.shortGUID, entity.shortGUID);
                    break;
            }
            selected_entity_type_description.Text = description;

            //show mvr editor button if this entity has a mvr link
            if (Loaded.mvr != null && Loaded.mvr.Entries.FindAll(o => o.commandsNodeID == Loaded.selected.entity.shortGUID).Count != 0)
                editEntityMovers.Enabled = true;

            //populate linked params IN
            parentEntities.Clear();
            int current_ui_offset = 7;
            List<Entity> ents = Loaded.selected.composite.GetEntities();
            foreach (Entity ent in ents)
            {
                foreach (EntityLink link in ent.childLinks)
                {
                    if (link.childID != entity.shortGUID) continue;
                    GUI_Link parameterGUI = new GUI_Link(this);
                    parameterGUI.PopulateUI(link, false, ent.shortGUID);
                    parameterGUI.GoToEntity += LoadEntity;
                    parameterGUI.Location = new Point(15, current_ui_offset);
                    current_ui_offset += parameterGUI.Height + 6;
                    entity_params.Controls.Add(parameterGUI);
                    parentEntities.Add(ent);
                }
            }

            //populate parameter inputs
            entity.parameters = entity.parameters.OrderBy(o => o.name.ToString()).ToList();
            for (int i = 0; i < entity.parameters.Count; i++)
            {
                ParameterData this_param = entity.parameters[i].content;
                UserControl parameterGUI = null;
                string paramName = entity.parameters[i].name.ToString();
                switch (this_param.dataType)
                {
                    case DataType.TRANSFORM:
                        parameterGUI = new GUI_TransformDataType();
                        ((GUI_TransformDataType)parameterGUI).PopulateUI((cTransform)this_param, paramName);
                        break;
                    case DataType.INTEGER:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Int((cInteger)this_param, paramName);
                        break;
                    case DataType.STRING:
                        AssetList.Type asset = AssetList.Type.NONE;
                        string asset_arg = "";
                        //TODO: We can figure out a lot of these from the iOS dump.
                        //      For example - SoundEnvironmentMarker shows reverb_name as DataType SOUND_REVERB!
                        switch (paramName)
                        {
                            //case "Animation":
                            //    asset = AssetList.Type.ANIMATION;
                            //    break;
                            case "material":
                                asset = AssetList.Type.MATERIAL;
                                break;
                            case "title":
                            case "map_description":
                            case "content_title":
                            case "folder_title":
                            case "additional_info": //TODO: this is a good example of why we should handle this per-entity
                                asset = AssetList.Type.LOCALISED_STRING;
                                if (entity.variant == EntityVariant.FUNCTION && CommandsUtils.GetFunctionType(((FunctionEntity)entity).function).ToString().Contains("Objective"))
                                    asset_arg = "OBJECTIVES";
                                else if (entity.variant == EntityVariant.FUNCTION && CommandsUtils.GetFunctionType(((FunctionEntity)entity).function).ToString().Contains("Terminal"))
                                    asset_arg = "T0001/UI"; //TODO: we should also support TEXT dbs in the level folder for DLC stuff
                                else
                                    asset_arg = "UI";
                                break;
                            case "title_id":
                            case "message_id":
                            case "unlocked_text":
                            case "locked_text":
                            case "action_text":
                                asset = AssetList.Type.LOCALISED_STRING;
                                asset_arg = "UI";
                                break;
                            case "sound_event":
                            case "music_event":
                            case "stop_event":
                            case "line_01_event":
                            case "line_02_event":
                            case "line_03_event":
                            case "line_04_event":
                            case "line_05_event":
                            case "line_06_event":
                            case "line_07_event":
                            case "line_08_event":
                            case "line_09_event":
                            case "line_10_event":
                            case "on_enter_event":
                            case "on_exit_event":
                            case "music_start_event":
                            case "music_end_event":
                            case "music_restart_event":
                                asset = AssetList.Type.SOUND_EVENT;
                                break;
                            case "reverb_name":
                                asset = AssetList.Type.SOUND_REVERB;
                                break;
                            case "sound_bank":
                                asset = AssetList.Type.SOUND_BANK;
                                break;
                        }
                        if (asset != AssetList.Type.NONE)
                        {
                            parameterGUI = new GUI_StringVariant_AssetDropdown(this);
                            ((GUI_StringVariant_AssetDropdown)parameterGUI).PopulateUI((cString)this_param, paramName, asset, asset_arg);
                        }
                        else
                        {
                            parameterGUI = new GUI_StringDataType();
                            ((GUI_StringDataType)parameterGUI).PopulateUI((cString)this_param, paramName);
                        }
                        break;
                    case DataType.BOOL:
                        parameterGUI = new GUI_BoolDataType();
                        ((GUI_BoolDataType)parameterGUI).PopulateUI((cBool)this_param, paramName);
                        break;
                    case DataType.FLOAT:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Float((cFloat)this_param, paramName);
                        break;
                    case DataType.VECTOR:
                        switch (paramName)
                        {
                            case "AMBIENT_LIGHTING_COLOUR":
                            case "COLOUR_TINT_START":
                            case "COLOUR_TINT_MID":
                            case "COLOUR_TINT_END":
                            case "COLOUR_TINT":
                            case "COLOUR_TINT_OUTER":
                            case "DEPTH_INTERSECT_COLOUR_VALUE":
                            case "DEPTH_INTERSECT_INITIAL_COLOUR":
                            case "DEPTH_INTERSECT_MIDPOINT_COLOUR":
                            case "DEPTH_INTERSECT_END_COLOUR":
                            case "DEPTH_FOG_INITIAL_COLOUR":
                            case "DEPTH_FOG_MIDPOINT_COLOUR":
                            case "DEPTH_FOG_END_COLOUR":
                            case "ColourFactor":
                            case "lens_flare_colour":
                            case "light_shaft_colour":
                            case "initial_colour":
                            case "near_colour":
                            case "far_colour":
                            case "colour":
                            case "Colour":
                                parameterGUI = new GUI_VectorVariant_Colour();
                                ((GUI_VectorVariant_Colour)parameterGUI).PopulateUI((cVector3)this_param, paramName);
                                break;
                            default:
                                parameterGUI = new GUI_VectorDataType();
                                ((GUI_VectorDataType)parameterGUI).PopulateUI((cVector3)this_param, paramName);
                                break;
                        }
                        break;
                    case DataType.ENUM:
                        parameterGUI = new GUI_EnumDataType();
                        ((GUI_EnumDataType)parameterGUI).PopulateUI((cEnum)this_param, paramName);
                        break;
                    case DataType.RESOURCE:
                        parameterGUI = new GUI_ResourceDataType(this);
                        ((GUI_ResourceDataType)parameterGUI).PopulateUI((cResource)this_param, paramName);
                        break;
                    case DataType.SPLINE:
                        parameterGUI = new GUI_SplineDataType(this);
                        ((GUI_SplineDataType)parameterGUI).PopulateUI((cSpline)this_param, paramName);
                        break;
                }
                parameterGUI.Location = new Point(15, current_ui_offset);
                current_ui_offset += parameterGUI.Height + 6;
                entity_params.Controls.Add(parameterGUI);
            }

            //populate linked params OUT
            childEntities.Clear();
            for (int i = 0; i < entity.childLinks.Count; i++)
            {
                GUI_Link parameterGUI = new GUI_Link(this);
                parameterGUI.PopulateUI(entity.childLinks[i], true);
                parameterGUI.GoToEntity += LoadEntity;
                parameterGUI.Location = new Point(15, current_ui_offset);
                current_ui_offset += parameterGUI.Height + 6;
                entity_params.Controls.Add(parameterGUI);
                childEntities.Add(Loaded.selected.composite.GetEntityByID(entity.childLinks[i].childID));
            }

            //Update node viewer if it's open
            if (nodeViewer != null)
            {
                nodeViewer.AddEntities(Loaded.selected.composite, Loaded.selected.entity);
            }

            entity_params.ResumeLayout();
            Cursor.Current = Cursors.Default;
        }
        private void BackgroundEntityLoader(Entity ent, CommandsEditor mainInst)
        {
            bool isPointedTo = EditorUtils.IsSelectedEntityReferencedExternally();
            EditorUtils.TryFindZoneForSelectedEntity(out Composite zoneComp, out FunctionEntity zoneEnt);
            mainInst.ThreadedEntityUIUpdate(ent, isPointedTo, zoneComp, zoneEnt);
        }
        private Composite zoneCompositeForSelectedEntity = null;
        private FunctionEntity zoneEntityForSelectedEntity = null;
        public void ThreadedEntityUIUpdate(Entity ent, bool isPointedTo, Composite zoneComp, FunctionEntity zoneEnt)
        {
            if (ent != Loaded.selected.entity) return;
            showOverridesAndProxies.Invoke(new Action(() => { showOverridesAndProxies.Enabled = isPointedTo; }));
            zoneCompositeForSelectedEntity = zoneComp;
            zoneEntityForSelectedEntity = zoneEnt;
            string zoneText = "Zone";
            if (zoneEnt != null)
            {
                Parameter name = zoneEnt.GetParameter("name");
                if (name != null) zoneText += " (" + ((cString)name.content).value + ")";
            }
            goToZone.Invoke(new Action(() => { goToZone.Enabled = zoneEnt != null; goToZone.Text = zoneText; }));
        }

        /* Add a new parameter */
        private void addNewParameter_Click(object sender, EventArgs e)
        {
            if (Loaded.selected.entity == null) return;
            AddParameter add_parameter = new AddParameter(this, Loaded.selected.entity);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }
        private void refresh_entity_event(Object sender, FormClosedEventArgs e)
        {
            LoadEntity(Loaded.selected.entity);
            this.BringToFront();
            this.Focus();
        }

        /* Add a new link out */
        private void addLinkOut_Click(object sender, EventArgs e)
        {
            if (Loaded.selected.entity == null) return;
            AddOrEditLink add_link = new AddOrEditLink(this, Loaded.selected.composite, Loaded.selected.entity);
            add_link.Show();
            add_link.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }

        /* Remove a parameter */
        private void removeParameter_Click(object sender, EventArgs e)
        {
            if (Loaded.selected.entity == null) return;
            if (entity_params.Controls.Count == 0) return;
            if (Loaded.selected.entity.childLinks.Count + Loaded.selected.entity.parameters.Count == 0) return;
            RemoveParameter remove_parameter = new RemoveParameter(this, Loaded.selected.entity);
            remove_parameter.Show();
            remove_parameter.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }

        /* Edit function entity (CAGEAnimation/TriggerSequence) */
        private void editFunction_Click(object sender, EventArgs e)
        {
            if (Loaded.selected.entity.variant != EntityVariant.FUNCTION) return;
            string function = ShortGuidUtils.FindString(((FunctionEntity)Loaded.selected.entity).function);
            switch (function.ToUpper())
            {
                case "CAGEANIMATION":
                    Loaded.OnCAGEAnimationEditorOpened?.Invoke();
                    CAGEAnimationEditor cageAnimationEditor = new CAGEAnimationEditor(this, (CAGEAnimation)Loaded.selected.entity);
                    cageAnimationEditor.Show();
                    cageAnimationEditor.OnSaved += CAGEAnimationEditor_OnSaved;
                    break;
                case "TRIGGERSEQUENCE":
                    TriggerSequenceEditor triggerSequenceEditor = new TriggerSequenceEditor(this, (TriggerSequence)Loaded.selected.entity);
                    triggerSequenceEditor.Show();
                    triggerSequenceEditor.FormClosed += FunctionEditor_FormClosed;
                    break;
            }
        }
        private void CAGEAnimationEditor_OnSaved(CAGEAnimation newEntity)
        {
            CAGEAnimation entity = (CAGEAnimation)Loaded.selected.entity;
            entity.connections = newEntity.connections;
            entity.events = newEntity.events;
            entity.animations = newEntity.animations;
            entity.parameters = newEntity.parameters;
            LoadEntity(Loaded.selected.entity);
            this.BringToFront();
            this.Focus();
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
            resourceFunctionToEdit = ((FunctionEntity)Loaded.selected.entity);

            AddOrEditResource resourceEditor = new AddOrEditResource(this, ((FunctionEntity)Loaded.selected.entity).resources, Loaded.selected.entity.shortGUID, EditorUtils.GenerateEntityName(Loaded.selected.entity, Loaded.selected.composite));
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
            EditMVR moverEditor = new EditMVR(this, Loaded.selected.entity.shortGUID);
            moverEditor.Show();
            moverEditor.FormClosed += MoverEditor_FormClosed;
        }
        private void MoverEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();
        }

        /* Show overrides and proxies that point to this entity */
        private void showOverridesAndProxies_Click(object sender, EventArgs e)
        {
            ShowCrossRefs crossRefs = new ShowCrossRefs(this);
            crossRefs.Show();
            crossRefs.OnEntitySelected += OnCrossRefsEntitySelected;
        }
        private void OnCrossRefsEntitySelected(ShortGuid entity, Composite composite)
        {
            LoadComposite(composite, composite.GetEntityByID(entity));
        }

        /* Jump to the zone that this entity is in */
        private void goToZone_Click(object sender, EventArgs e)
        {
            if (Loaded.selected.composite != zoneCompositeForSelectedEntity)
                LoadComposite(zoneCompositeForSelectedEntity);
            LoadEntity(zoneEntityForSelectedEntity);
        }

        /* Enable/disable backups */
        Task backgroundBackups = null;
        CancellationToken backupCancellationToken;
        private void enableBackups_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool(_backupsOpt, enableBackups.Checked);

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            backupCancellationToken = tokenSource.Token;
            if (backgroundBackups != null) tokenSource.Cancel();
            while (backgroundBackups != null) Thread.Sleep(100);

            if (enableBackups.Checked)
                backgroundBackups = Task.Factory.StartNew(() => BackupCommands(this));
        }
        private void BackupCommands(CommandsEditor mainInst)
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

                if (Loaded.commands == null) continue;

                mainInst.EnableLoadingOfPaks(false, "Backup...");
                Loaded.commands.Save(Loaded.commands.Filepath + ".bak", false);
                mainInst.EnableLoadingOfPaks(true);
            }
        }

        /* Go back to the previous composite */
        private void goBackToPrevComp_Click(object sender, EventArgs e)
        {
            Loaded.selected.composite = null;
            LoadComposite(_previousComposite);
        }

        /* Confirm an action */
        private bool ConfirmAction(string msg)
        {
            return (MessageBox.Show(msg, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void show3D_Click(object sender, EventArgs e)
        {
            Composite3D viewer = new Composite3D(this, Loaded.selected.composite);
            viewer.Show();
        }

        private void StartWebsocket()
        {
            _server = new WebSocketServer("ws://localhost:1702");
            _server.AddWebSocketService<WebsocketServer>("/commands_editor", (server) =>
            {
                _serverLogic = server;
                _serverLogic.OnClientConnect += RefreshWebsocket;
            });
            _server.Start();
        }

        private void RefreshWebsocket()
        {
            if (!SettingsManager.GetBool(_serverOpt))
            {
                if (_server != null)
                    _server.Stop();
                _server = null;
                return;
            }
            else
            {
                if (_server == null)
                    StartWebsocket();
            }

            //Request the correct level
            if (Loaded.commands != null && Loaded.commands.Loaded)
            {
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)MessageType.LOAD_LEVEL) + Loaded.level);
            }

            //Point to position of selected entity
            if (Loaded.selected.entity != null && Loaded.selected.entity.GetParameter("position") != null)
            {
                System.Numerics.Vector3 vec = ((cTransform)Loaded.selected.entity.GetParameter("position").content).position;
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)MessageType.GO_TO_POSITION).ToString() + vec.X + ">" + vec.Y + ">" + vec.Z);
            }

            //Show name of entity
            if (Loaded.selected.entity != null && Loaded.selected.composite != null)
            {
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)MessageType.SHOW_ENTITY_NAME).ToString() + EntityUtils.GetName(Loaded.selected.composite, Loaded.selected.entity));
            }

            /*
            string str = "";
            for (int i = 0; i < Loaded.mvr.Entries.Count; i++)
            {
                if (Loaded.mvr.Entries[i].commandsNodeID != Loaded.selected.entity.shortGUID) continue;
                str += i + ">";
            }
            _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)MessageType.GO_TO_REDS).ToString() + str);
            */
        }

        private void UnityConnection_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool(_serverOpt, UnityConnection.Checked);
            RefreshWebsocket();
        }

        NodeEditor nodeViewer = null;
        private void showNodeViewer_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool(_nodeOpt, showNodeViewer.Checked);
            if (showNodeViewer.Checked)
            {
                nodeViewer = new NodeEditor(this);
                nodeViewer.Show();
                nodeViewer.FormClosed += NodeViewer_FormClosed;
            }
            else
            {
                if (nodeViewer != null)
                {
                    nodeViewer.FormClosed -= NodeViewer_FormClosed;
                    nodeViewer = null;
                }
            }
        }
        private void NodeViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            nodeViewer = null;
        }
    }
}
