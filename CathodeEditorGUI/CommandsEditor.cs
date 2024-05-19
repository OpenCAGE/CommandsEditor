using CATHODE;
using CATHODE.LEGACY;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups;
using CommandsEditor.UserControls;
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
using System.Windows.Media.Imaging;
using System.Xml;
using WebSocketSharp.Server;
using WeifenLuo.WinFormsUI.Docking;
using static System.Net.Mime.MediaTypeNames;

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

        private string _baseTitle = "";

        public CommandsEditor(string level = null)
        {
#if !DEBUG
            DEBUG_DoorPhysEnt.Visible = false;
            DEBUG_RunChecks.Visible = false;
#endif

            /*
            string[] cmds = Directory.GetFiles("E:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\", "COMMANDS.PAK", SearchOption.AllDirectories);
            foreach (string cmd in cmds)
            {
                Commands cmd_o = new Commands(cmd);
                /*
                foreach (Composite comp in cmd_o.Entries)
                {
                    foreach (FunctionEntity func in comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.Character)))
                    {
                        Parameter param = func.GetParameter("is_player");
                        if (param == null) continue;
                        if (((cBool)param.content).value != true) continue;

                        func.AddParameter("display_model", new cString("E_RIPLEY"));
                        func.AddParameter("reference_skeleton", new cString("E_RIPLEY"));

                        foreach (EntityConnector conn in func.GetParentLinks(comp))
                        {
                            if (conn.linkedParamID == ShortGuidUtils.Generate("display_model"))
                            {
                                Entity ent = comp.GetEntityByID(conn.linkedEntityID);
                                if (ent != null)
                                    ent.childLinks.RemoveAll(o => o.ID == conn.ID);
                            }
                            if (conn.linkedParamID == ShortGuidUtils.Generate("reference_skeleton"))
                            {
                                Entity ent = comp.GetEntityByID(conn.linkedEntityID);
                                if (ent != null)
                                    ent.childLinks.RemoveAll(o => o.ID == conn.ID);
                            }
                        }

                        func.childLinks.RemoveAll(o => o.thisParamID == ShortGuidUtils.Generate("display_model"));
                        func.childLinks.RemoveAll(o => o.thisParamID == ShortGuidUtils.Generate("reference_skeleton"));
                    }
                }
                */
            /*
                foreach (Composite comp in cmd_o.Entries)
                {
                    if (comp.GetFunctionEntitiesOfType(FunctionType.Zone).Count == 0)
                        continue;

                    FunctionEntity checkpoint = comp.AddFunction(FunctionType.DebugCheckpoint);
                    checkpoint.AddParameter("section", new cString("Load Zones: " + comp.name));

                    FunctionEntity popup = comp.AddFunction(FunctionType.PopupMessage);
                    popup.AddParameter("main_text", new cString("Loading Zone"));
                    checkpoint.AddParameterLink("on_checkpoint", popup, "start");

                    foreach (FunctionEntity zone in comp.GetFunctionEntitiesOfType(FunctionType.Zone))
                    {
                        zone.AddParameter("suspend_on_unload", new cBool(true));
                        zone.AddParameter("force_visible_on_load", new cBool(true));

                        checkpoint.AddParameterLink("on_checkpoint", zone, "request_load");
                    }
                }
                cmd_o.Save();
            }
            return;

            */

            /*
            Commands cmd = new Commands("E:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\DLC/BSPNOSTROMO_TWOTEAMS_PATCH\\WORLD\\COMMANDS.PAK");

            foreach (Composite comp in cmd.Entries)
            {
                List<FunctionEntity> zonesToBin = new List<FunctionEntity>();
                List<FunctionEntity> zones = comp.functions.FindAll(o => o.function == ShortGuidUtils.Generate("Zone"));
                foreach (FunctionEntity zone in zones)
                {
                    EntityConnector composites = zone.childLinks.FirstOrDefault(o => o.thisParamID == ShortGuidUtils.Generate("composites"));
                    if (composites.ID.IsInvalid) continue;

                    Entity triggersequence_ent = comp.GetEntityByID(composites.linkedEntityID);
                    if (triggersequence_ent == null || !(triggersequence_ent is TriggerSequence)) continue;

                    zonesToBin.Add(zone);
                }
                comp.functions.RemoveAll(o => zonesToBin.Contains(o));
            }

            /*
            foreach (Composite comp in cmd.Entries)
            {
                TriggerSequence bigtrig = (TriggerSequence)comp.AddFunction(FunctionType.TriggerSequence);
                bigtrig.AddParameter("no_duplicates", new cBool(true));

                FunctionEntity bigzone = comp.AddFunction(FunctionType.Zone);

                foreach (FunctionEntity zone in comp.functions.FindAll(o => o.function == ShortGuidUtils.Generate("Zone")))
                {
                    EntityConnector composites = zone.childLinks.FirstOrDefault(o => o.thisParamID == ShortGuidUtils.Generate("composites"));
                    if (composites.ID.IsInvalid) continue;

                    Entity triggersequence_ent = comp.GetEntityByID(composites.linkedEntityID);
                    if (triggersequence_ent == null || !(triggersequence_ent is TriggerSequence)) continue;

                    TriggerSequence triggersequence = (TriggerSequence)triggersequence_ent;
                    foreach (TriggerSequence.Entity entity in triggersequence.entities)
                    {
                        bigtrig.entities.Add(entity);
                    }

                    zone.AddParameterLink("on_loaded", bigzone, "request_load");
                    zone.AddParameterLink("on_unloaded", bigzone, "request_load");
                }

                bigzone.AddParameterLink("composites", bigtrig, "reference");
            }
            */
            //cmd.Save();
            //return;

            Singleton.Editor = this;

            _discord = new DiscordRpcClient("1152999067207606392");
            _discord.Initialize();
            _discord.SetPresence(new RichPresence() { Assets = new Assets() { LargeImageKey = "icon" } });

            Singleton.OnCompositeSelected += delegate (Composite composite)
            {
                RichPresence newPresence = _discord.CurrentPresence.Copy();
                newPresence.Details = "Level: " + _commandsDisplay?.Content?.level;
                newPresence.State = "Composite: " + EditorUtils.GetCompositeName(composite);
                _discord.SetPresence(newPresence);
                _discord.UpdateStartTime();
            };

            InitializeComponent();
            dockPanel.DockLeftPortion = SettingsManager.GetFloat(Singleton.Settings.CommandsSplitWidth, _defaultSplitterDistance);
            dockPanel.DockBottomPortion = SettingsManager.GetFloat(Singleton.Settings.SplitWidthMainBottom, _defaultSplitterDistance);
            dockPanel.DockRightPortion = SettingsManager.GetFloat(Singleton.Settings.SplitWidthMainRight, _defaultSplitterDistance);

            dockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;
            dockPanel.ShowDocumentIcon = true;

            _defaultWidth = Width;
            _defaultHeight = Height;

            WindowState = SettingsManager.GetString(Singleton.Settings.WindowState, "Normal") == "Maximized" ? FormWindowState.Maximized : FormWindowState.Normal;
            Width = SettingsManager.GetInteger(Singleton.Settings.WindowWidth, _defaultWidth);
            Height = SettingsManager.GetInteger(Singleton.Settings.WindowHeight, _defaultHeight);
            Resize += CommandsEditor_Resize;
            FormClosing += CommandsEditor_FormClosing;
            
            Singleton.OnEntitySelected += RefreshWebsocket;
            Singleton.OnCompositeSelected += RefreshWebsocket;
            Singleton.OnLevelLoaded += RefreshWebsocket;

            connectToUnity.Checked = !SettingsManager.GetBool(Singleton.Settings.ServerOpt); connectToUnity.PerformClick();
            showEntityIDs.Checked = !SettingsManager.GetBool(Singleton.Settings.EntIdOpt); showEntityIDs.PerformClick();
            searchOnlyCompositeNames.Checked = !SettingsManager.GetBool(Singleton.Settings.CompNameOnlyOpt); searchOnlyCompositeNames.PerformClick();
            useTexturedModelViewExperimentalToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ShowTexOpt); useTexturedModelViewExperimentalToolStripMenuItem.PerformClick();
            keepFunctionUsesWindowOpenToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.KeepUsesWindowOpen); keepFunctionUsesWindowOpenToolStripMenuItem.PerformClick();
            nodeOpensEntity.Checked = !SettingsManager.GetBool(Singleton.Settings.OpenEntityFromNode); nodeOpensEntity.PerformClick();
            useLegacyParameterCreatorToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator); useLegacyParameterCreatorToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.NodeOpt)) SettingsManager.SetBool(Singleton.Settings.NodeOpt, true);
            showNodegraph.Checked = !SettingsManager.GetBool(Singleton.Settings.NodeOpt); showNodegraph.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.UseEntityTabs)) SettingsManager.SetBool(Singleton.Settings.UseEntityTabs, true);
            entitiesOpenTabs.Checked = !SettingsManager.GetBool(Singleton.Settings.UseEntityTabs); entitiesOpenTabs.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.ShowSavedMsgOpt)) SettingsManager.SetBool(Singleton.Settings.ShowSavedMsgOpt, true);
            showConfirmationWhenSavingToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ShowSavedMsgOpt); showConfirmationWhenSavingToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.EnableFileBrowser)) SettingsManager.SetBool(Singleton.Settings.EnableFileBrowser, true);
            showExplorerViewToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.EnableFileBrowser); showExplorerViewToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.AutoHideCompositeDisplay)) SettingsManager.SetBool(Singleton.Settings.AutoHideCompositeDisplay, true);
            autoHideExplorerViewToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.AutoHideCompositeDisplay); autoHideExplorerViewToolStripMenuItem.PerformClick();

            //Fixes for dodgy top dropdowns
            compositeViewerToolStripMenuItem.MouseHover += (sender, e) => { ((ToolStripMenuItem)sender).PerformClick(); };
            compositeViewerToolStripMenuItem.DropDown.Closing += DropDown_Closing;
            entityDisplayToolStripMenuItem.MouseHover += (sender, e) => { ((ToolStripMenuItem)sender).PerformClick(); };
            entityDisplayToolStripMenuItem.DropDown.Closing += DropDown_Closing;
            miscToolStripMenuItem.MouseHover += (sender, e) => { ((ToolStripMenuItem)sender).PerformClick(); };
            miscToolStripMenuItem.DropDown.Closing += DropDown_Closing;
            toolStripButton2.DropDown.Closing += DropDown_Closing;

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
            _baseTitle = this.Text;

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

        //keep dropdown open if cursor is inside it 
        private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            var dropdown = sender as ToolStripDropDown;
            if (dropdown != null)
            {
                Point cursorPosition = dropdown.PointToClient(Cursor.Position);
                if (dropdown.DisplayRectangle.Contains(cursorPosition))
                {
                    e.Cancel = true;
                }
            }
        }

        private void CommandsEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsManager.SetFloat(Singleton.Settings.SplitWidthMainBottom, (float)dockPanel.DockBottomPortion);
            SettingsManager.SetFloat(Singleton.Settings.SplitWidthMainRight, (float)dockPanel.DockRightPortion);
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
        public void LoadAnimData()
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

            Singleton.OnFinishedLazyLoadingStrings?.Invoke();
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

            //Singleton.OnCompositeSelected?.Invoke(_activeCompositeDisplay.Composite);
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
            this.Text = _baseTitle + " - " + level;

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
        }

        private void _commandsDisplay_Resize(object sender, EventArgs e)
        {
            SettingsManager.SetFloat(Singleton.Settings.CommandsSplitWidth, (float)dockPanel.DockLeftPortion);
        }

        private void _commandsDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
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
                        switch (resource.resource_type)
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

            //Populate PHYSICS.MAP
            ShortGuid DYNAMIC_PHYSICS_SYSTEM = ShortGuidUtils.Generate("DYNAMIC_PHYSICS_SYSTEM");
            foreach (Composite composite in _commandsDisplay.Content.commands.Entries)
            {
                List<FunctionEntity> physEnts = composite.GetFunctionEntitiesOfType(FunctionType.PhysicsSystem);
                if (physEnts.Count == 0)
                    continue;
                if (physEnts.Count > 1)
                    Console.WriteLine("WARNING: More than one PhysicsSystem entity in this composite - editor UI should've prevented this!");

                FunctionEntity physEnt = physEnts[0];
                ResourceReference physEntRes = physEnt.GetResource(ResourceType.DYNAMIC_PHYSICS_SYSTEM);

                if (physEntRes == null)
                {
                    Console.WriteLine("WARNING: PhysicsSystem entity does not have a DYNAMIC_PHYSICS_SYSTEM resource - CathodeLib should've added this.");
                    continue;
                }

                List<EntityPath> hierarchies = _commandsDisplay.Content.editor_utils.GetHierarchiesForEntity(composite, physEnt);
                if (hierarchies.Count == 0)
                    continue;

                for (int i = 0; i < hierarchies.Count; i++)
                {
                    //Calculate global transform from path (TODO: this logic should totally exist elsewhere to be reusable)
                    Vector3 position; Quaternion rotation;
                    {
                        cTransform globalTransform = new cTransform();
                        Composite comp = _commandsDisplay.Content.commands.EntryPoints[0];
                        for (int x = 0; x < hierarchies[i].path.Count; x++)
                        {
                            FunctionEntity compInst = comp.functions.FirstOrDefault(o => o.shortGUID == hierarchies[i].path[x]);
                            if (compInst == null)
                                break;

                            Parameter positionParam = compInst.GetParameter("position");
                            if (positionParam != null && positionParam.content != null && positionParam.content.dataType == DataType.TRANSFORM)
                                globalTransform += (cTransform)positionParam.content;

                            comp = _commandsDisplay.Content.commands.GetComposite(compInst.function);
                            if (comp == null)
                                break;
                        }
                        position = globalTransform.position;
                        rotation = Quaternion.CreateFromYawPitchRoll(globalTransform.rotation.Y * (float)Math.PI / 180.0f, globalTransform.rotation.X * (float)Math.PI / 180.0f, globalTransform.rotation.Z * (float)Math.PI / 180.0f);
                    }
                    
                    //Get instance info
                    ShortGuid compositeInstanceID = hierarchies[i].GenerateInstance();
                    hierarchies[i].path.RemoveAt(hierarchies[i].path.Count - 2);
                    CommandsEntityReference compositeInstanceReference = new CommandsEntityReference()
                    {
                        entity_id = hierarchies[i].path[hierarchies[i].path.Count - 2],
                        composite_instance_id = hierarchies[i].GenerateInstance()
                    };
                    
                    //TODO: why do some entries require PHYSICS.MAP and others not?
                    var test = _commandsDisplay.Content.resource.physics_maps.Entries.FindAll(o => o.composite_instance_id == compositeInstanceID && o.entity == compositeInstanceReference);
                    if (test.Count == 0)
                        continue;

                    //Remove all entries that already exist for this instance
                    _commandsDisplay.Content.resource.physics_maps.Entries.RemoveAll(o => o.composite_instance_id == compositeInstanceID && o.entity == compositeInstanceReference);

                    //Make a new entry for the instance
                    _commandsDisplay.Content.resource.physics_maps.Entries.Add(new PhysicsMaps.Entry()
                    {
                        physics_system_index = physEntRes.index,
                        resource_type = DYNAMIC_PHYSICS_SYSTEM,
                        composite_instance_id = compositeInstanceID,
                        entity = compositeInstanceReference,
                        Position = position,
                        Rotation = rotation
                    });
                }
            }

            if (_commandsDisplay.Content.resource.physics_maps != null && _commandsDisplay.Content.resource.physics_maps.Entries != null)
                _commandsDisplay.Content.resource.physics_maps.Save();
            if (_commandsDisplay.Content.resource.resources != null && _commandsDisplay.Content.resource.resources.Entries != null)
                _commandsDisplay.Content.resource.resources.Save();
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
                _nodeViewer = new NodeEditor();
                _nodeViewer.Show(dockPanel, (DockState)Enum.Parse(typeof(DockState), SettingsManager.GetString(Singleton.Settings.NodegraphState, "DockRightAutoHide")));
                _nodeViewer.FormClosed += NodeViewer_FormClosed;
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
            SettingsManager.SetBool(Singleton.Settings.UseEntityTabs, entitiesOpenTabs.Checked);

            _commandsDisplay?.CloseAllChildTabs();
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

        private void useLegacyParameterCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useLegacyParameterCreatorToolStripMenuItem.Checked = !useLegacyParameterCreatorToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.UseLegacyParamCreator, useLegacyParameterCreatorToolStripMenuItem.Checked);
        }

        private void resetUILayoutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Width = _defaultWidth;
            Height = _defaultHeight;

            dockPanel.DockLeftPortion = _defaultSplitterDistance;
            dockPanel.DockRightPortion = _defaultSplitterDistance;
            dockPanel.DockBottomPortion = _defaultSplitterDistance;

            _commandsDisplay?.ResetSplitter();
            _activeCompositeDisplay?.ResetSplitter();

            if (_nodeViewer != null)
            {
                _nodeViewer.DockState = DockState.DockRightAutoHide;
                _nodeViewer.ResetLayout();
            }
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

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/");
        }

        private void DEBUG_DoorPhysEnt_Click(object sender, EventArgs e)
        {
            _commandsDisplay.LoadCompositeAndEntity(new ShortGuid("05-2C-95-DF"), new ShortGuid("BB-3E-91-2E"));
        }

        private void DEBUG_RunChecks_Click(object sender, EventArgs e)
        {
            LocalDebug.checkphysicssystempositions();
        }
    }
}
