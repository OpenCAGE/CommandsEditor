using CATHODE;
using CATHODE.LEGACY;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups;
using DiscordRPC;
using Newtonsoft.Json;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WebSocketSharp.Server;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor
{
    public partial class CommandsEditor : Form
    {
        public DockPanel DockPanel => dockPanel;

        private CommandsDisplay _commandsDisplay = null;
        public CommandsDisplay CommandsDisplay => _commandsDisplay;

        private NodeEditor _nodeViewer = null;
        private SelectLevel _levelSelect = null;

        private CompositeDisplay _activeCompositeDisplay = null;
        public CompositeDisplay ActiveCompositeDisplay => _activeCompositeDisplay;

        private WebSocketServer _server;
        private WebsocketServer _serverLogic;
        private DiscordRpcClient _discord;

        private Dictionary<string, ToolStripMenuItem> _levelMenuItems = new Dictionary<string, ToolStripMenuItem>();

        private float _defaultSplitterDistance = 0.25f;
        private int _defaultWidth;
        private int _defaultHeight;

        public CommandsEditor(string level = null)
        {
            _discord = new DiscordRpcClient("1152999067207606392");
            _discord.Initialize();
            UpdateDiscordPresence("");

            Singleton.Editor = this;

            InitializeComponent();
            dockPanel.DockLeftPortion = SettingsManager.GetFloat(Singleton.Settings.CommandsSplitWidth, _defaultSplitterDistance);
            dockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;
            dockPanel.ShowDocumentIcon = true;

            _defaultWidth = Width;
            _defaultHeight = Height;

            WindowState = SettingsManager.GetString(Singleton.Settings.WindowState, "Normal") == "Maximized" ? FormWindowState.Maximized : FormWindowState.Normal;
            Width = SettingsManager.GetInteger(Singleton.Settings.WindowWidth, _defaultWidth);
            Height = SettingsManager.GetInteger(Singleton.Settings.WindowHeight, _defaultHeight);
            Resize += CommandsEditor_Resize;
            
            Singleton.OnEntitySelected += RefreshWebsocket;
            Singleton.OnCompositeSelected += RefreshWebsocket;
            Singleton.OnLevelLoaded += RefreshWebsocket;

            connectToUnity.Checked = !SettingsManager.GetBool(Singleton.Settings.ServerOpt); connectToUnity.PerformClick();
            showNodegraph.Checked = !SettingsManager.GetBool(Singleton.Settings.NodeOpt); showNodegraph.PerformClick();
            showEntityIDs.Checked = !SettingsManager.GetBool(Singleton.Settings.EntIdOpt); showEntityIDs.PerformClick();
            searchOnlyCompositeNames.Checked = !SettingsManager.GetBool(Singleton.Settings.CompNameOnlyOpt); searchOnlyCompositeNames.PerformClick();
            useTexturedModelViewExperimentalToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ShowTexOpt); useTexturedModelViewExperimentalToolStripMenuItem.PerformClick();
            keepFunctionUsesWindowOpenToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.KeepUsesWindowOpen); keepFunctionUsesWindowOpenToolStripMenuItem.PerformClick();
            nodeOpensEntity.Checked = !SettingsManager.GetBool(Singleton.Settings.OpenEntityFromNode); nodeOpensEntity.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.UseEntTabsOpt)) SettingsManager.SetBool(Singleton.Settings.UseEntTabsOpt, true);
            entitiesOpenTabs.Checked = !SettingsManager.GetBool(Singleton.Settings.UseEntTabsOpt); entitiesOpenTabs.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.ShowSavedMsgOpt)) SettingsManager.SetBool(Singleton.Settings.ShowSavedMsgOpt, true);
            showConfirmationWhenSavingToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ShowSavedMsgOpt); showConfirmationWhenSavingToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.EnableFileBrowser)) SettingsManager.SetBool(Singleton.Settings.EnableFileBrowser, true);
            showExplorerViewToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.EnableFileBrowser); showExplorerViewToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.AutoHideCompositeDisplay)) SettingsManager.SetBool(Singleton.Settings.AutoHideCompositeDisplay, true);
            autoHideExplorerViewToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.AutoHideCompositeDisplay); autoHideExplorerViewToolStripMenuItem.PerformClick();

            //Set title
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
            {
                Strings[] strings = new Strings[textList.Count];
                Parallel.For(0, textList.Count, (i) =>
                {
                    strings[i] = new Strings(textList[i]);
                });
                for (int i = 0; i < textList.Count; i++)
                    Singleton.Strings.Add(Path.GetFileNameWithoutExtension(textList[i]), strings[i]);
            }

            //Load animation data - this should be quick enough to not worry about waiting for the thread
            Task.Factory.StartNew(() => LoadAnimData());

            //Load global textures - same note as above
            Task.Factory.StartNew(() => LoadGlobalTex());

            //Populate level list
            List<string> levels = Level.GetLevels(SharedData.pathToAI, true);
            for (int i = 0; i < levels.Count; i++)
            {
                ToolStripMenuItem levelItem = new ToolStripMenuItem(levels[i]);
                levelItem.Click += OnLevelSelected;
                loadLevel.DropDownItems.Add(levelItem);
                _levelMenuItems.Add(levels[i], levelItem);
            }

            //If we have been launched to a level, load that
            if (level != null)
                OnLevelSelected(level);
        }

        //UI: remember width/height of editor
        private void CommandsEditor_Resize(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case FormWindowState.Normal:
                    SettingsManager.SetInteger(Singleton.Settings.WindowWidth, Width);
                    SettingsManager.SetInteger(Singleton.Settings.WindowHeight, Height);
                    break;
                case FormWindowState.Maximized:

                    break;
            }
            SettingsManager.SetString(Singleton.Settings.WindowState, WindowState.ToString());
        }

        /* Load anim data */
        public static void LoadAnimData()
        {
            //Load animation data
            PAK2 animPAK = new PAK2(SharedData.pathToAI + "/DATA/GLOBAL/ANIMATION.PAK");

            //Load all male/female skeletons
            List<PAK2.File> skeletonDefs = animPAK.Entries.FindAll(o => o.Filename.Length > 17 && o.Filename.Substring(0, 17) == "DATA\\SKELETONDEFS");
            for (int i = 0; i < skeletonDefs.Count; i++)
            {
                string skeleton = Path.GetFileNameWithoutExtension(skeletonDefs[i].Filename);
                File.WriteAllBytes(skeleton, skeletonDefs[i].Content);
                XmlNode skeletonType = new BML(skeleton).Content.SelectSingleNode("//SkeletonDef/LoResReferenceSkeleton");
                if (skeletonType?.InnerText == "MALE" || skeletonType?.InnerText == "FEMALENPC")
                {
                    if (!Singleton.GenderedSkeletons.ContainsKey(skeletonType?.InnerText))
                        Singleton.GenderedSkeletons.Add(skeletonType?.InnerText, new List<string>());
                    Singleton.GenderedSkeletons[skeletonType?.InnerText].Add(skeleton);
                }
                File.Delete(skeleton);
            }

            //Anim string db
            File.WriteAllBytes("ANIM_STRING_DB.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB.BIN")).Content);
            Singleton.AnimationStrings = new AnimationStrings("ANIM_STRING_DB.BIN");
            File.Delete("ANIM_STRING_DB.BIN");

            //Debug anim string db
            File.WriteAllBytes("ANIM_STRING_DB_DEBUG.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB_DEBUG.BIN")).Content);
            Singleton.AnimationStrings_Debug = new AnimationStrings("ANIM_STRING_DB_DEBUG.BIN");
            File.Delete("ANIM_STRING_DB_DEBUG.BIN");

            //Anim clip db
            //File.WriteAllBytes("ANIM_CLIP_DB.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_CLIP_DB.BIN")).Content);
            //Singleton.AnimClipDB = new AnimClipDB("ANIM_CLIP_DB.BIN");
            //File.Delete("ANIM_CLIP_DB.BIN");

            //Skeleton db
            File.WriteAllBytes("DB.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("SKELE\\DB.BIN")).Content);
            Singleton.SkeletonDB = new SkeleDB("DB.BIN", Singleton.AnimationStrings_Debug);
            File.Delete("DB.BIN");

            //Load all skeleton names
            List<PAK2.File> skeletons = animPAK.Entries.FindAll(o => o.Filename.Length > 24 && o.Filename.Substring(0, 24) == "DATA\\ANIM_SYS\\SKELE\\DEFS");
            for (int i = 0; i < skeletons.Count; i++)
            {
                Singleton.AllSkeletons.Add(Singleton.AnimationStrings_Debug.Entries[Convert.ToUInt32(Path.GetFileNameWithoutExtension(skeletons[i].Filename))]);
            }
            Singleton.AllSkeletons.Sort();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /* Load global textures */
        private void LoadGlobalTex()
        {
            Singleton.GlobalTextures = new Textures(SharedData.pathToAI + "/DATA/ENV/GLOBAL/WORLD/GLOBAL_TEXTURES.ALL.PAK");
        }
        
        /* Monitor the currently active composite tab */
        private void DockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            CompositeDisplay prevActiveCompositeDisplay = _activeCompositeDisplay;
            object content = ((DockPanel)sender).ActiveContent;

            if (content is CompositeDisplay)
                _activeCompositeDisplay = (CompositeDisplay)content;
            else
                return;

            if (prevActiveCompositeDisplay == _activeCompositeDisplay) return;

            if (prevActiveCompositeDisplay != null)
                prevActiveCompositeDisplay.FormClosing -= OnActiveContentClosing;
            _activeCompositeDisplay.FormClosing += OnActiveContentClosing;

            Singleton.OnCompositeSelected?.Invoke(_activeCompositeDisplay.Composite);
        }
        private void OnActiveContentClosing(object sender, FormClosingEventArgs e)
        {
            CompositeDisplay prevActiveCompositeDisplay = _activeCompositeDisplay;
            _activeCompositeDisplay = null;

            if (prevActiveCompositeDisplay == _activeCompositeDisplay) return;

            Singleton.OnCompositeSelected?.Invoke(null);
        }

        private void loadLevel_Click(object sender, EventArgs e)
        {
            if (_levelSelect == null)
            {
                _levelSelect = new SelectLevel();
                _levelSelect.Show();
                _levelSelect.FormClosed += OnLevelSelectClosed;
                _levelSelect.OnLevelSelected += OnLevelSelected;
            }

            _levelSelect.BringToFront();
            _levelSelect.Focus();
        }
        private void OnLevelSelectClosed(object sender, FormClosedEventArgs e)
        {
            _levelSelect = null;
        }
        private void OnLevelSelected(object sender, EventArgs e)
        {
            OnLevelSelected(((ToolStripMenuItem)sender).Text);
        }
        private void OnLevelSelected(string level)
        {
            statusText.Text = "Loading " + level + "...";
            statusStrip.Update();

            _activeCompositeDisplay = null;
            _levelSelect = null;

            //Close all existing
            if (_commandsDisplay != null)
            {
                _levelMenuItems[_commandsDisplay.Content.level].Checked = false;

                _commandsDisplay.CloseAllChildTabs();
                _commandsDisplay.Close();
            }

            //Load new
            _commandsDisplay = new CommandsDisplay(level);
            _commandsDisplay.Resize += _commandsDisplay_Resize;
            _commandsDisplay.FormClosed += _commandsDisplay_FormClosed;
            UpdateCommandsDisplayDockState();

            _levelMenuItems[_commandsDisplay.Content.level].Checked = true;
            UpdateDiscordPresence("Editing " + level);
        }

        private void _commandsDisplay_Resize(object sender, EventArgs e)
        {
            SettingsManager.SetFloat(Singleton.Settings.CommandsSplitWidth, (float)dockPanel.DockLeftPortion);
        }

        private void _commandsDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateDiscordPresence("");
            _commandsDisplay?.Dispose();
            _commandsDisplay = null;
        }

        private void saveLevel_Click(object sender, EventArgs e)
        {
            if (_commandsDisplay == null) return;

            Cursor.Current = Cursors.WaitCursor;
            statusText.Text = "Saving...";
            statusStrip.Update();
            bool saved = LegacySave();
            statusText.Text = "";
            Cursor.Current = Cursors.Default;

            ShowSaveMsg(saved);
        }
        private void ShowSaveMsg(bool saved)
        {
            if (saved)
            {
                if (SettingsManager.GetBool(Singleton.Settings.ShowSavedMsgOpt))
                    MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Failed to save changes!", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool SaveNew()
        {
            bool saved = false;
#if !CATHODE_FAIL_HARD
            byte[] backup = null;
            try
            {
                backup = File.ReadAllBytes(_commandsDisplay.Content.commands.Filepath);
#endif
                saved = _commandsDisplay.Content.commands.Save();
#if !CATHODE_FAIL_HARD
            }
            catch (Exception ex)
            {
                try
                {
                    if (backup != null)
                        File.WriteAllBytes(_commandsDisplay.Content.commands.Filepath, backup);
                }
                catch { }

                return false;
            }

            if (!saved)
            {
                try
                {
                    if (backup != null)
                        File.WriteAllBytes(_commandsDisplay.Content.commands.Filepath, backup);
                }
                catch { }

                return false;
            }
#endif

            //Calculate instance specific stuff
            _commandsDisplay.Content.editor_utils.GenerateCompositeInstances(_commandsDisplay.Content.commands); //TODO: Do we need to do this? I don't think we should.
            CreateDataForInstance(_commandsDisplay.Content.commands.EntryPoints[0]);

            return saved;
        }
        private void CreateDataForInstance(Composite composite)
        {
            for (int i = 0; i < composite.functions.Count; i++) //todo: can we do this in parallel?
            {
                if (CommandsUtils.FunctionTypeExists(composite.functions[i].function))
                {
                    //This is a FunctionEntity which we may need to create data for the instance of

                    //Writing this code, I'm now realising that we would need to write these extra files to know the indexes to then write in Commands.
                    //It must all be done at the same time...
                    //Hmm...
                    for (int x = 0; x < composite.functions[i].resources.Count; x++)
                    {
                        ResourceReference resource = composite.functions[i].resources[x];
                        switch (resource.entryType)
                        {
                            case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                                //Write to PHYSICS.MAP
                                break;
                            case ResourceType.COLLISION_MAPPING:
                                //Write to COLLISION.MAP
                                break;
                            case ResourceType.RENDERABLE_INSTANCE:
                                //Write to MODELS.MVR
                                break;
                            case ResourceType.ANIMATED_MODEL:
                                //Write to ENVIRONMENT_ANIMATION.DAT
                                break;
                        }
                    }
                }
                else
                {
                    //This is a FunctionEntity which instances a child composite, so we should follow it through
                    Composite child = _commandsDisplay.Content.commands.GetComposite(composite.functions[i].function);
                    if (child != null) CreateDataForInstance(child);
                }
            }

            _commandsDisplay.Content.resource.physics_maps.Save();
            _commandsDisplay.Content.resource.collision_maps.Save();
            _commandsDisplay.Content.resource.character_accessories.Save();
            _commandsDisplay.Content.resource.reds.Save();
            _commandsDisplay.Content.resource.env_animations.Save();
            _commandsDisplay.Content.mvr.Save();
        }

        //To be deprecated: this does not work nicely for resources and other hierarchical things.
        private bool LegacySave()
        {
            bool saved = false;
#if !CATHODE_FAIL_HARD
            byte[] backup = null;
            try
            {
                backup = File.ReadAllBytes(_commandsDisplay.Content.commands.Filepath);
#endif
                saved = _commandsDisplay.Content.commands.Save();
#if !CATHODE_FAIL_HARD
            }
            catch (Exception ex)
            {
                try
                {
                    if (backup != null)
                        File.WriteAllBytes(_commandsDisplay.Content.commands.Filepath, backup);
                }
                catch { }

                return false;
            }

            if (!saved)
            {
                try
                {
                    if (backup != null)
                        File.WriteAllBytes(_commandsDisplay.Content.commands.Filepath, backup);
                }
                catch { }

                return false;
            }
#endif

            if (_commandsDisplay.Content.resource.physics_maps != null && _commandsDisplay.Content.resource.physics_maps.Entries != null)
                _commandsDisplay.Content.resource.physics_maps.Save();
            if (_commandsDisplay.Content.resource.character_accessories != null && _commandsDisplay.Content.resource.character_accessories.Entries != null)
                _commandsDisplay.Content.resource.character_accessories.Save();
            if (_commandsDisplay.Content.resource.reds != null && _commandsDisplay.Content.resource.reds.Entries != null)
                _commandsDisplay.Content.resource.reds.Save();
            if (_commandsDisplay.Content.mvr != null && _commandsDisplay.Content.mvr.Entries != null)
                _commandsDisplay.Content.mvr.Save();

            return true;
        }

        /* Enable the option to load */
        public void EnableLoadingOfPaks(bool shouldEnable, string text)
        {
            try
            {
                toolStrip.Invoke(new Action(() => { toolStrip.Enabled = shouldEnable; }));
                statusStrip.Invoke(new Action(() => { statusText.Text = text; }));
            }
            catch { }
        }

        /* Websocket to Unity */
        private void connectToUnity_Click(object sender, EventArgs e)
        {
            connectToUnity.Checked = !connectToUnity.Checked;
            SettingsManager.SetBool(Singleton.Settings.ServerOpt, connectToUnity.Checked);
            RefreshWebsocket();
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
                if (connectToUnity.Checked)
                    connectToUnity.PerformClick();

                MessageBox.Show("Failed to initialise Unity connection.\nIs another instance of the script editor running?", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void RefreshWebsocket() => RefreshWebsocket(null);
        private void RefreshWebsocket(object o)
        {
            if (!SettingsManager.GetBool(Singleton.Settings.ServerOpt))
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
            if (_commandsDisplay?.Content?.commands != null && _commandsDisplay.Content.commands.Loaded)
            {
                SendWebsocketData(new WebsocketServer.WSPacket { 
                    type = WebsocketServer.MessageType.LOAD_LEVEL, 
                    alien_path = SharedData.pathToAI, 
                    level_name = _commandsDisplay.Content.level 
                });
            }

            //Get active stuff
            Composite composite = ActiveCompositeDisplay?.Composite;
            Entity entity = ActiveCompositeDisplay?.ActiveEntityDisplay?.Entity;
            Parameter position = entity?.GetParameter("position");

            //Load composite
            if (composite != null)
            {
                SendWebsocketData(new WebsocketServer.WSPacket
                {
                    type = WebsocketServer.MessageType.LOAD_COMPOSITE,
                    composite_name = composite.shortGUID.ToByteString(),
                    alien_path = SharedData.pathToAI,
                    level_name = _commandsDisplay.Content.level
                });
            }

            //Point to position of selected entity
            if (position != null)
            {
                SendWebsocketData(new WebsocketServer.WSPacket
                {
                    type = WebsocketServer.MessageType.GO_TO_POSITION,
                    position = ((cTransform)position.content).position
                });
            }

            //Show name of entity
            if (entity != null && composite != null)
            {
                SendWebsocketData(new WebsocketServer.WSPacket
                {
                    type = WebsocketServer.MessageType.SHOW_ENTITY_NAME,
                    entity_name = EntityUtils.GetName(composite, entity)
                });
            }
        }

        private void SendWebsocketData(WebsocketServer.WSPacket content)
        {
            _server?.WebSocketServices["/commands_editor"].Sessions.Broadcast(JsonConvert.SerializeObject(content));
        }

        private void showNodegraph_Click(object sender, EventArgs e)
        {
            showNodegraph.Checked = !showNodegraph.Checked;
            SettingsManager.SetBool(Singleton.Settings.NodeOpt, showNodegraph.Checked);

            if (showNodegraph.Checked)
            {
                _nodeViewer = new NodeEditor(null);
                _nodeViewer.Show();
                _nodeViewer.FormClosed += NodeViewer_FormClosed;
                _nodeViewer.BringToFront();
                _nodeViewer.Focus();
            }
            else
            {
                if (_nodeViewer != null)
                {
                    _nodeViewer.FormClosed -= NodeViewer_FormClosed;
                    _nodeViewer.Close();
                    _nodeViewer = null;
                }
            }
        }
        private void NodeViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            _nodeViewer = null;
            showNodegraph.PerformClick();
        }

        private void showEntityIDs_Click(object sender, EventArgs e)
        {
            showEntityIDs.Checked = !showEntityIDs.Checked;
            SettingsManager.SetBool(Singleton.Settings.EntIdOpt, showEntityIDs.Checked);

            _commandsDisplay?.Reload(true);
            //TODO: also reload hierarchy cache
        }

        private void searchOnlyCompositeNames_Click(object sender, EventArgs e)
        {
            searchOnlyCompositeNames.Checked = !searchOnlyCompositeNames.Checked;
            SettingsManager.SetBool(Singleton.Settings.CompNameOnlyOpt, searchOnlyCompositeNames.Checked);
        }

        private void entitiesOpenTabs_Click(object sender, EventArgs e)
        {
            entitiesOpenTabs.Checked = !entitiesOpenTabs.Checked;
            SettingsManager.SetBool(Singleton.Settings.UseEntTabsOpt, entitiesOpenTabs.Checked);
        }

        private void showConfirmationWhenSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showConfirmationWhenSavingToolStripMenuItem.Checked = !showConfirmationWhenSavingToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.ShowSavedMsgOpt, showConfirmationWhenSavingToolStripMenuItem.Checked);
        }

        private void useTexturedModelViewExperimentalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useTexturedModelViewExperimentalToolStripMenuItem.Checked = !useTexturedModelViewExperimentalToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.ShowTexOpt, useTexturedModelViewExperimentalToolStripMenuItem.Checked);
        }

        private void showExplorerViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showExplorerViewToolStripMenuItem.Checked = !showExplorerViewToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.EnableFileBrowser, showExplorerViewToolStripMenuItem.Checked);
            UpdateCommandsDisplayDockState();
        }

        private void autoHideExplorerViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoHideExplorerViewToolStripMenuItem.Checked = !autoHideExplorerViewToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.AutoHideCompositeDisplay, autoHideExplorerViewToolStripMenuItem.Checked);
            UpdateCommandsDisplayDockState();
        }

        private void keepFunctionUsesWindowOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keepFunctionUsesWindowOpenToolStripMenuItem.Checked = !keepFunctionUsesWindowOpenToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.KeepUsesWindowOpen, keepFunctionUsesWindowOpenToolStripMenuItem.Checked);
        }

        private void nodeOpensEntity_Click(object sender, EventArgs e)
        {
            nodeOpensEntity.Checked = !nodeOpensEntity.Checked;
            SettingsManager.SetBool(Singleton.Settings.OpenEntityFromNode, nodeOpensEntity.Checked);
        }

        private void resetUILayoutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dockPanel.DockLeftPortion = _defaultSplitterDistance;
            _commandsDisplay?.ResetSplitter();
            _activeCompositeDisplay?.ResetSplitter();

            WindowState = FormWindowState.Normal;
            Width = _defaultWidth;
            Height = _defaultHeight;
        }

        private void UpdateCommandsDisplayDockState()
        {
            if (_commandsDisplay == null)
            {
                Singleton.Editor.DockPanel.ActiveAutoHideContent = null;
                return;
            }
            _commandsDisplay.UpdateDockState();
        }

        private void UpdateDiscordPresence(string text)
        {
            _discord.SetPresence(new RichPresence()
            {
                Details = "Commands Editor",
                State = text,
                Assets = new Assets() { LargeImageKey = "icon" }
            });
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/");
        }
    }
}
