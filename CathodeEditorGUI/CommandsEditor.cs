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
using System.Xml;
using System.Runtime.InteropServices;
using System.Numerics;

namespace CommandsEditor
{
    public partial class CommandsEditor : Form
    {
        public EditorData Editor = new EditorData();

        private TreeUtility _treeHelper;
        private Composite _previousComposite = null;

        private WebSocketServer _server;
        private WebsocketServer _serverLogic;
        private readonly string _serverOpt = "CE_ConnectToUnity";
        private readonly string _backupsOpt = "CS_EnableBackups";
        private readonly string _nodeOpt = "CS_NodeView";

        Task currentEntityNameCacher = null;
        Task currentHierarchyCacher = null;

        public CommandsEditor()
        {
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
                Editor.strings.Add(Path.GetFileNameWithoutExtension(text), new Strings(text));
            }

            //Load animation data - this should be quick enough to not worry about waiting for the thread
            Task.Factory.StartNew(() => LoadAnimData(this));

            ClearUI(true, true, true);

            show3D.Visible = false;
#if DEBUG
            show3D.Visible = true;
#endif
        }

        /* Load anim data */
        public static void LoadAnimData(CommandsEditor editor)
        {
            //Load animation data
            PAK2 animPAK = new PAK2(SharedData.pathToAI + "/DATA/GLOBAL/ANIMATION.PAK");

            //Load all male/female skeletons
            List<PAK2.File> skeletons = animPAK.Entries.FindAll(o => o.Filename.Length > 17 && o.Filename.Substring(0, 17) == "DATA\\SKELETONDEFS");
            for (int i = 0; i < skeletons.Count; i++)
            {
                string skeleton = Path.GetFileNameWithoutExtension(skeletons[i].Filename);
                File.WriteAllBytes(skeleton, skeletons[i].Content);
                XmlNode skeletonType = new BML(skeleton).Content.SelectSingleNode("//SkeletonDef/LoResReferenceSkeleton");
                if (skeletonType?.InnerText == "MALE" || skeletonType?.InnerText == "FEMALENPC")
                {
                    if (!editor.Editor.skeletons.ContainsKey(skeletonType?.InnerText))
                        editor.Editor.skeletons.Add(skeletonType?.InnerText, new List<string>());
                    editor.Editor.skeletons[skeletonType?.InnerText].Add(skeleton);
                }
                File.Delete(skeleton);
            }

            //Anim string db
            File.WriteAllBytes("ANIM_STRING_DB.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB.BIN")).Content);
            editor.Editor.animstrings = new AnimationStrings("ANIM_STRING_DB.BIN");
            File.Delete("ANIM_STRING_DB.BIN");

            //Debug anim string db
            File.WriteAllBytes("ANIM_STRING_DB_DEBUG.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB_DEBUG.BIN")).Content);
            editor.Editor.animstrings_debug = new AnimationStrings("ANIM_STRING_DB_DEBUG.BIN");
            File.Delete("ANIM_STRING_DB_DEBUG.BIN");

            animPAK = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void CommandsEditor_Load(object sender, EventArgs e)
        {
            if (nodeViewer != null)
            {
                nodeViewer.BringToFront();
                nodeViewer.Focus();
            }
            return;
#if DEBUG
            env_list.SelectedItem = "DLC\\BSPNOSTROMO_TWOTEAMS_PATCH";
            LoadCommandsPAK(env_list.SelectedItem.ToString());
            LoadComposite("DLC\\PREORDER\\PODLC_TWOTEAMS");
            LoadEntity(Editor.selected.composite.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation)));
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
                Editor.selected.composite = null;
            }
            if (clear_parameter_list)
            {
                Editor.selected.entity = null;
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
        public void LoadCommandsPAK(string level)
        {
            //Reset UI
            ClearUI(true, true, true);

            //Load everything
            LoadCommands(level);
            /*Task.Factory.StartNew(() => */LoadAssets()/*)*/;
            /*Task.Factory.StartNew(() => */LoadMovers()/*)*/;

            //Link up commands to utils and cache some things
            EntityUtils.LinkCommands(Editor.commands);
            ShortGuidUtils.LinkCommands(Editor.commands);
            if (currentEntityNameCacher != null) currentEntityNameCacher.Dispose();
            currentEntityNameCacher = Task.Factory.StartNew(() => EditorUtils.GenerateEntityNameCache(this));
            CacheHierarchies();

            //Populate file tree
            _treeHelper.UpdateFileTree(Editor.commands.GetCompositeNames().ToList());

            //Show info in UI
            RefreshStatsUI();

            //Load root composite
            _treeHelper.SelectNode(Editor.commands.EntryPoints[0].name);

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
            root_composite_display.Text = "Root composite: " + Editor.commands.EntryPoints[0].name;
        }

        /* Cache entity hierarchies */
        private void CacheHierarchies()
        {
            if (currentHierarchyCacher != null) currentHierarchyCacher.Dispose();
            currentHierarchyCacher = Task.Factory.StartNew(() => EditorUtils.GenerateCompositeInstances(Editor.commands));
        }

        /* Load commands */
        private void LoadCommands(string level)
        {
            if (Editor.commands != null) Editor.commands.Entries.Clear();
            Editor.level = level;

            string path_to_ENV = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + level;
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                Editor.commands = new Commands(path_to_ENV + "/WORLD/COMMANDS.PAK");
                Editor.OnCommandsSelected?.Invoke(Editor.commands);
#if !CATHODE_FAIL_HARD
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load COMMANDS.PAK!\n" + e.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Editor.commands = null;
                return;
            }
#endif
            RefreshWebsocket();

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
            if (Editor.resource.models != null) Editor.resource.models.Entries.Clear();
            if (Editor.resource.reds != null) Editor.resource.reds.Entries.Clear();
            if (Editor.resource.materials != null) Editor.resource.materials.Entries.Clear();
            if (Editor.resource.textures != null) Editor.resource.textures.Entries.Clear();
            if (Editor.resource.textures_global != null) Editor.resource.textures_global.Entries.Clear();
            if (Editor.resource.env_animations != null) Editor.resource.env_animations.Entries.Clear();
            if (Editor.resource.collision_maps != null) Editor.resource.collision_maps.Entries.Clear();
            if (Editor.resource.sound_bankdata != null) Editor.resource.sound_bankdata.Entries.Clear();
            if (Editor.resource.sound_dialoguelookups != null) Editor.resource.sound_dialoguelookups.Entries.Clear();
            if (Editor.resource.sound_eventdata != null) Editor.resource.sound_eventdata.Entries.Clear();
            if (Editor.resource.sound_environmentdata != null) Editor.resource.sound_environmentdata.Entries.Clear();
            if (Editor.resource.character_accessories != null) Editor.resource.character_accessories.Entries.Clear();

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

                Editor.resource.models = new Models(baseLevelPath + "RENDERABLE/LEVEL_MODELS.PAK");
                Editor.resource.reds = new RenderableElements(baseLevelPath + "WORLD/REDS.BIN");
                Editor.resource.materials = new Materials(baseLevelPath + "RENDERABLE/LEVEL_MODELS.MTL");
                //Editor.resource.textures = new Textures(baseLevelPath + "RENDERABLE/LEVEL_TEXTURES.ALL.PAK");
                //Editor.resource.textures_Global = new Textures(SharedData.pathToAI + "/DATA/ENV/GLOBAL/WORLD/GLOBAL_TEXTURES.ALL.PAK");
                Editor.resource.env_animations = new EnvironmentAnimations(baseLevelPath + "WORLD/ENVIRONMENT_ANIMATION.DAT");
                Editor.resource.collision_maps = new CollisionMaps(baseLevelPath + "WORLD/COLLISION.MAP");
                Editor.resource.sound_bankdata = new SoundBankData(baseLevelPath + "WORLD/SOUNDBANKDATA.DAT");
                Editor.resource.sound_dialoguelookups = new SoundDialogueLookups(baseLevelPath + "WORLD/SOUNDDIALOGUELOOKUPS.DAT");
                Editor.resource.sound_eventdata = new SoundEventData(baseLevelPath + "WORLD/SOUNDEVENTDATA.DAT");
                Editor.resource.sound_environmentdata = new SoundEnvironmentData(baseLevelPath + "WORLD/SOUNDENVIRONMENTDATA.DAT");
                Editor.resource.character_accessories = new CharacterAccessorySets(baseLevelPath + "WORLD/CHARACTERACCESSORYSETS.BIN");
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
                Editor.resource.textures_global = null;
                Editor.resource.env_animations = null;
                Editor.resource.collision_maps = null;
                Editor.resource.sound_bankdata = null;
                Editor.resource.sound_dialoguelookups = null;
                Editor.resource.sound_eventdata = null;
                Editor.resource.sound_environmentdata = null;
                Editor.resource.character_accessories = null;
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
                Editor.mvr = new Movers(baseLevelPath + "WORLD/MODELS.MVR");
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

            if (Editor.resource.character_accessories != null && Editor.resource.character_accessories.Entries != null)
                Editor.resource.character_accessories.Save();

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
            switch (Editor.selected.entity.variant)
            {
                case EntityVariant.OVERRIDE:
                {
                    Entity entity = CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((OverrideEntity)Editor.selected.entity).connectedEntity.hierarchy, out flow, out string hierarchy);
                    if (entity != null)
                    {
                        LoadComposite(flow.name);
                        LoadEntity(entity);
                    }
                    break;
                }
                case EntityVariant.PROXY:
                {
                    Entity entity = CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((ProxyEntity)Editor.selected.entity).connectedEntity.hierarchy, out flow, out string hierarchy);
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
            AddComposite add_flow = new AddComposite(this);
            add_flow.Show();
            add_flow.FormClosed += new FormClosedEventHandler(add_flow_closed);
        }
        private void add_flow_closed(Object sender, FormClosedEventArgs e)
        {
            _treeHelper.UpdateFileTree(Editor.commands.GetCompositeNames().ToList());
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
            _treeHelper.UpdateFileTree(Editor.commands.GetCompositeNames().ToList());

            CacheHierarchies();
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
        public void LoadComposite(string filename)
        {
            LoadComposite(Editor.commands.GetComposite(filename));
        }
        public void LoadComposite(Composite comp, Entity ent = null)
        {
            _previousComposite = Editor.selected.composite;
            ClearUI(false, true, true);

            Editor.selected.composite = comp;
            Editor.OnCompositeSelected?.Invoke(Editor.selected.composite);

            Cursor.Current = Cursors.WaitCursor;
            CommandsUtils.PurgeDeadLinks(Editor.commands, comp);

            composite_content.BeginUpdate();
            List<Entity> entities = comp.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                string desc = EditorUtils.GenerateEntityName(entities[i], Editor.selected.composite);
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
            if (Editor.selected.composite == null) return;
            AddEntity add_parameter = new AddEntity(this, Editor.selected.composite, Editor.commands.Entries);
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
                            List<TriggerSequence.Entity> triggers = new List<TriggerSequence.Entity>();
                            for (int x = 0; x < triggerSequence.entities.Count; x++)
                            {
                                if (triggerSequence.entities[x].connectedEntity.hierarchy.Count < 2 ||
                                    triggerSequence.entities[x].connectedEntity.hierarchy[triggerSequence.entities[x].connectedEntity.hierarchy.Count - 2] != Editor.selected.entity.shortGUID)
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
                                    cageAnim.connections[x].connectedEntity.hierarchy[cageAnim.connections[x].connectedEntity.hierarchy.Count - 2] != Editor.selected.entity.shortGUID)
                                {
                                    headers.Add(cageAnim.connections[x]);
                                }
                            }
                            cageAnim.connections = headers;
                            break;
                    }
                }
            }

            LoadComposite(Editor.selected.composite.name);
            ClearUI(false, false, true);

            CacheHierarchies();
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
            Entity newEnt = null;
            switch (Editor.selected.entity.variant)
            {
                case EntityVariant.FUNCTION:
                    newEnt = ((FunctionEntity)Editor.selected.entity).Copy();
                    break;
                case EntityVariant.VARIABLE:
                    newEnt = ((VariableEntity)Editor.selected.entity).Copy();
                    break;
                case EntityVariant.OVERRIDE:
                    newEnt = ((OverrideEntity)Editor.selected.entity).Copy();
                    break;
                case EntityVariant.PROXY:
                    newEnt = ((ProxyEntity)Editor.selected.entity).Copy();
                    break;
            }
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

            CacheHierarchies();
        }

        /* Rename selected entity */
        private void renameSelectedEntity_Click(object sender, EventArgs e)
        {
            if (Editor.selected.entity == null) return;
            RenameEntity rename_entity = new RenameEntity(this, Editor.selected.composite, Editor.selected.entity);
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
        private List<Entity> parentEntities = new List<Entity>();
        private List<Entity> childEntities = new List<Entity>();
        public void LoadEntity(ShortGuid guid)
        {
            LoadEntity(Editor.selected.composite.GetEntityByID(guid));
        }
        public void LoadEntity(Entity entity)
        {
            ClearUI(false, false, true);
            Editor.selected.entity = entity;
            Editor.OnEntitySelected?.Invoke(Editor.selected.entity);
            RefreshWebsocket();

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
                    Composite funcComposite = Editor.commands.GetComposite(thisFunction);
                    jumpToComposite.Visible = funcComposite != null;
                    if (funcComposite != null)
                        description = funcComposite.name;
                    else
                        description = CathodeEntityDatabase.GetEntity(thisFunction).className;
                    selected_entity_name.Text = EntityUtils.GetName(Editor.selected.composite.shortGUID, entity.shortGUID);
                    if (funcComposite == null)
                    {
                        FunctionType function = CommandsUtils.GetFunctionType(thisFunction);
                        editFunction.Enabled = function == FunctionType.CAGEAnimation || function == FunctionType.TriggerSequence || function == FunctionType.Character;
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
                    if (entity.variant == EntityVariant.PROXY) CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((ProxyEntity)entity).connectedEntity.hierarchy, out Composite comp, out hierarchy);
                    else CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((OverrideEntity)entity).connectedEntity.hierarchy, out Composite comp, out hierarchy);
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
            if (Editor.mvr != null && Editor.mvr.Entries.FindAll(o => o.entity.entity_id == Editor.selected.entity.shortGUID).Count != 0)
                editEntityMovers.Enabled = true;

            //populate linked params IN
            parentEntities.Clear();
            int current_ui_offset = 7;
            List<Entity> ents = Editor.selected.composite.GetEntities();
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
                childEntities.Add(Editor.selected.composite.GetEntityByID(entity.childLinks[i].childID));
            }

            //Update node viewer if it's open
            if (nodeViewer != null)
            {
                nodeViewer.AddEntities(Editor.selected.composite, Editor.selected.entity);
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
            if (ent != Editor.selected.entity) return;
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
            if (Editor.selected.entity == null) return;
            AddParameter add_parameter = new AddParameter(this, Editor.selected.entity);
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
            AddOrEditLink add_link = new AddOrEditLink(this, Editor.selected.composite, Editor.selected.entity);
            add_link.Show();
            add_link.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }

        /* Remove a parameter */
        private void removeParameter_Click(object sender, EventArgs e)
        {
            if (Editor.selected.entity == null) return;
            if (entity_params.Controls.Count == 0) return;
            if (Editor.selected.entity.childLinks.Count + Editor.selected.entity.parameters.Count == 0) return;
            RemoveParameter remove_parameter = new RemoveParameter(this, Editor.selected.entity);
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
                    Editor.OnCAGEAnimationEditorOpened?.Invoke();
                    CAGEAnimationEditor cageAnimationEditor = new CAGEAnimationEditor(this, (CAGEAnimation)Editor.selected.entity);
                    cageAnimationEditor.Show();
                    cageAnimationEditor.OnSaved += CAGEAnimationEditor_OnSaved;
                    break;
                case "TRIGGERSEQUENCE":
                    TriggerSequenceEditor triggerSequenceEditor = new TriggerSequenceEditor(this, (TriggerSequence)Editor.selected.entity);
                    triggerSequenceEditor.Show();
                    triggerSequenceEditor.FormClosed += FunctionEditor_FormClosed;
                    break;
                case "CHARACTER":
                    //TODO: I think this is only valid for entities with "custom_character_type" set - but working that out requires a complex parse of connected entities. So ignoring for now.
                    CharacterEditor characterEditor = new CharacterEditor(this);
                    characterEditor.Show();
                    characterEditor.FormClosed += FunctionEditor_FormClosed;
                    break;
            }
        }
        private void CAGEAnimationEditor_OnSaved(CAGEAnimation newEntity)
        {
            CAGEAnimation entity = (CAGEAnimation)Editor.selected.entity;
            entity.connections = newEntity.connections;
            entity.events = newEntity.events;
            entity.animations = newEntity.animations;
            entity.parameters = newEntity.parameters;
            LoadEntity(Editor.selected.entity);
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
            resourceFunctionToEdit = ((FunctionEntity)Editor.selected.entity);

            AddOrEditResource resourceEditor = new AddOrEditResource(this, ((FunctionEntity)Editor.selected.entity).resources, Editor.selected.entity.shortGUID, EditorUtils.GenerateEntityName(Editor.selected.entity, Editor.selected.composite));
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
            EditMVR moverEditor = new EditMVR(this, Editor.selected.entity.shortGUID);
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
            if (Editor.selected.composite != zoneCompositeForSelectedEntity)
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

                if (Editor.commands == null) continue;

                mainInst.EnableLoadingOfPaks(false, "Backup...");
                Editor.commands.Save(Editor.commands.Filepath + ".bak", false);
                mainInst.EnableLoadingOfPaks(true);
            }
        }

        /* Go back to the previous composite */
        private void goBackToPrevComp_Click(object sender, EventArgs e)
        {
            Editor.selected.composite = null;
            LoadComposite(_previousComposite);
        }

        /* Confirm an action */
        private bool ConfirmAction(string msg)
        {
            return (MessageBox.Show(msg, "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void show3D_Click(object sender, EventArgs e)
        {
            Composite3D viewer = new Composite3D(this, Editor.selected.composite);
            viewer.Show();
        }

        private bool StartWebsocket()
        {
            try
            {
                _server = new WebSocketServer("ws://localhost:1702");
                _server.AddWebSocketService<WebsocketServer>("/commands_editor", (server) =>
                {
                    _serverLogic = server;
                    _serverLogic.OnClientConnect += RefreshWebsocket;
                });
                _server.Start();
                return true;
            }
            catch
            {
                UnityConnection.Checked = false;
                MessageBox.Show("Failed to initialise Unity connection.\nIs another instance of the script editor running?", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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
            if (Editor.commands != null && Editor.commands.Loaded)
            {
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)WebsocketServer.MessageType.LOAD_LEVEL) + Editor.level);
            }

            //Point to position of selected entity
            if (Editor.selected.entity != null && Editor.selected.entity.GetParameter("position") != null)
            {
                System.Numerics.Vector3 vec = ((cTransform)Editor.selected.entity.GetParameter("position").content).position;
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)WebsocketServer.MessageType.GO_TO_POSITION).ToString() + vec.X + ">" + vec.Y + ">" + vec.Z);
            }

            //Show name of entity
            if (Editor.selected.entity != null && Editor.selected.composite != null)
            {
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)WebsocketServer.MessageType.SHOW_ENTITY_NAME).ToString() + EntityUtils.GetName(Editor.selected.composite, Editor.selected.entity));
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
                nodeViewer.BringToFront();
                nodeViewer.Focus();
            }
            else
            {
                if (nodeViewer != null)
                {
                    nodeViewer.FormClosed -= NodeViewer_FormClosed;
                    nodeViewer.Close();
                    nodeViewer = null;
                }
            }
        }
        private void NodeViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            nodeViewer = null;
            showNodeViewer.Checked = false;
        }
    }
}
