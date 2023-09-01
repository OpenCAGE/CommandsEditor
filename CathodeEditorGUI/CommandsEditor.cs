using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private readonly string _serverOpt = "CE_ConnectToUnity";
        private readonly string _backupsOpt = "CS_EnableBackups";
        private readonly string _nodeOpt = "CS_NodeView";
        private readonly string _entIdOpt = "CS_ShowEntityIDs";
        private readonly string _instOpt = "CS_InstanceMode";

        public CommandsEditor(string level = null)
        {
            Singleton.Editor = this;

            InitializeComponent();
            dockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;

            Singleton.OnEntitySelected += RefreshWebsocket;
            Singleton.OnCompositeSelected += RefreshWebsocket;
            Singleton.OnLevelLoaded += RefreshWebsocket;

            enableBackups.Checked = !SettingsManager.GetBool(_backupsOpt); enableBackups.PerformClick();
            connectToUnity.Checked = !SettingsManager.GetBool(_serverOpt); connectToUnity.PerformClick();
            showNodegraph.Checked = !SettingsManager.GetBool(_nodeOpt); showNodegraph.PerformClick();
            showEntityIDs.Checked = !SettingsManager.GetBool(_entIdOpt); showEntityIDs.PerformClick();

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
            foreach (string text in textList)
                Singleton.Strings.Add(Path.GetFileNameWithoutExtension(text), new Strings(text));

            //Load animation data - this should be quick enough to not worry about waiting for the thread
            Task.Factory.StartNew(() => LoadAnimData());

            //If we have been launched to a level, load that
            if (level != null)
                OnLevelSelected(level);

            //Disable backups - we should now force people to use the extra backup tool
            //TODO: backup tool should backup the level at intervals like this tool did
            enableBackups.Checked = false;
            enableBackups.Visible = false;
        }

        /* Load anim data */
        public static void LoadAnimData()
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
                    if (!Singleton.Skeletons.ContainsKey(skeletonType?.InnerText))
                        Singleton.Skeletons.Add(skeletonType?.InnerText, new List<string>());
                    Singleton.Skeletons[skeletonType?.InnerText].Add(skeleton);
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

            animPAK = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
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
        private void OnLevelSelected(string level)
        {
            statusText.Text = "Loading " + level + "...";
            statusStrip.Update();

            _activeCompositeDisplay = null;
            _levelSelect = null;

            //Close all existing
            if (_commandsDisplay != null)
            {
                _commandsDisplay.CloseAllChildTabs();
                _commandsDisplay.Close();
            }

            //Load new
            _commandsDisplay = new CommandsDisplay(level);
            _commandsDisplay.Show(Singleton.Editor.DockPanel, DockState.DockLeft);
            _commandsDisplay.CloseButtonVisible = false;
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

            if (saved)
                MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /* Enable/disable backups */
        Task backgroundBackups = null;
        CancellationToken backupCancellationToken;
        private void enableBackups_Click(object sender, EventArgs e)
        {
            enableBackups.Checked = !enableBackups.Checked;
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

                if (_commandsDisplay.Content.commands == null) continue;
                mainInst.EnableLoadingOfPaks(false, "Performing automated backup...");

                string backupDirectory = _commandsDisplay.Content.commands.Filepath.Substring(0, _commandsDisplay.Content.commands.Filepath.Length - Path.GetFileName(_commandsDisplay.Content.commands.Filepath).Length) + "/COMMANDS_BACKUPS/";
                Directory.CreateDirectory(backupDirectory);

                //Make sure there are only 15 max backed up PAKs
                var files = new DirectoryInfo(backupDirectory).EnumerateFiles().OrderByDescending(f => f.CreationTime).Skip(15).ToList();
                files.ForEach(f => f.Delete());

                _commandsDisplay.Content.commands.Save(backupDirectory + "COMMANDS_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + ".PAK", false);
                mainInst.EnableLoadingOfPaks(true, "");
            }
        }

        /* Websocket to Unity */
        private void connectToUnity_Click(object sender, EventArgs e)
        {
            connectToUnity.Checked = !connectToUnity.Checked;
            SettingsManager.SetBool(_serverOpt, connectToUnity.Checked);
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
            if (_commandsDisplay?.Content?.commands != null && _commandsDisplay.Content.commands.Loaded)
            {
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)WebsocketServer.MessageType.LOAD_LEVEL) + _commandsDisplay.Content.level);
            }

            //Get active stuff
            Entity entity = ActiveCompositeDisplay?.ActiveEntityDisplay?.Entity;
            Composite composite = ActiveCompositeDisplay?.ActiveEntityDisplay?.Composite;
            Parameter position = entity?.GetParameter("position");

            //Point to position of selected entity
            if (position != null)
            {
                System.Numerics.Vector3 vec = ((cTransform)position.content).position;
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)WebsocketServer.MessageType.GO_TO_POSITION).ToString() + vec.X + ">" + vec.Y + ">" + vec.Z);
            }

            //Show name of entity
            if (entity != null && composite != null)
                _server.WebSocketServices["/commands_editor"].Sessions.Broadcast(((int)WebsocketServer.MessageType.SHOW_ENTITY_NAME).ToString() + EntityUtils.GetName(composite, entity));
        }

        private void showNodegraph_Click(object sender, EventArgs e)
        {
            showNodegraph.Checked = !showNodegraph.Checked;
            SettingsManager.SetBool(_nodeOpt, showNodegraph.Checked);

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
            SettingsManager.SetBool(_entIdOpt, showEntityIDs.Checked);

            _commandsDisplay?.Reload(true);
            //TODO: also reload hierarchy cache
        }

        private void enableInstanceMode_Click(object sender, EventArgs e)
        {
            SettingsManager.SetBool(_instOpt, enableInstanceMode.Checked);
        }
    }
}
