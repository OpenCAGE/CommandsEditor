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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Xml;
using WebSocketSharp;
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

        private string _baseTitle = "";

        public CommandsEditor(string level = null)
        {
            //CollisionMaps col = new CollisionMaps("E:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\SOLACE\\WORLD\\COLLISION.MAP");
            //col.Save();
            //return;

#if DEBUG
            Directory.Delete("E:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\SOLACE", true);
            CopyFilesRecursively("F:\\Alien Isolation Versions\\Alien Isolation PC Final\\DATA\\ENV\\PRODUCTION\\SOLACE", "E:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\SOLACE");
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

#if !DEBUG
            DEBUG_DoorPhysEnt.Visible = false;
            DEBUG_RunChecks.Visible = false;
#endif

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
            bool saved = Save();
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

        ShortGuid GUID_ANIMATED_MODEL = ShortGuidUtils.Generate("AnimatedModel");
        ShortGuid GUID_DYNAMIC_PHYSICS_SYSTEM = ShortGuidUtils.Generate("DYNAMIC_PHYSICS_SYSTEM");
        ShortGuid GUID_resource = ShortGuidUtils.Generate("resource");

        private bool Save()
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

            if (SettingsManager.GetBool(Singleton.Settings.ExperimentalResourceStuff))
            {
                //Update additional resource stuff
                foreach (Composite composite in _commandsDisplay.Content.commands.Entries)
                {
                    foreach (FunctionEntity func in composite.functions)
                    {
                        List<ResourceReference> resources = func.resources;
                        Parameter resourceParam = func.GetParameter(GUID_resource);
                        if (resourceParam != null && resourceParam.content != null && resourceParam.content.dataType == DataType.RESOURCE)
                            resources.AddRange(((cResource)resourceParam.content).value);

                        int collisionMapCount = resources.FindAll(o => o.resource_type == ResourceType.COLLISION_MAPPING).Count;
                        int animatedModelCount = resources.FindAll(o => o.resource_type == ResourceType.ANIMATED_MODEL).Count;
                        int physSystemCount = resources.FindAll(o => o.resource_type == ResourceType.DYNAMIC_PHYSICS_SYSTEM).Count;

                        if (collisionMapCount + animatedModelCount + physSystemCount == 0)
                            continue;

                        ShortGuid nameHash = default;
                        if (collisionMapCount != 0)
                            nameHash = ShortGuidUtils.Generate(EntityUtils.GetName(composite, func));

                        List<EntityPath> hierarchies = _commandsDisplay.Content.editor_utils.GetHierarchiesForEntity(composite, func);
                        List<ShortGuid> instanceIDs = new List<ShortGuid>();
                        for (int i = 0; i < hierarchies.Count; i++)
                            instanceIDs.Add(hierarchies[i].GenerateInstance());

                        foreach (ResourceReference resRef in resources)
                        {
                            ShortGuid id;
                            switch (resRef.resource_type)
                            {
                                case ResourceType.COLLISION_MAPPING:
                                    if (resRef.entityID == ShortGuid.Max)
                                        continue;
                                    id = nameHash;
                                    resRef.entityID = func.shortGUID;
                                    WriteCollisionMap(id, null, composite, func);
                                    break;
                                case ResourceType.ANIMATED_MODEL:
                                    id = GUID_ANIMATED_MODEL;
                                    break;
                                case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                                    id = GUID_DYNAMIC_PHYSICS_SYSTEM;
                                    break;
                                default:
                                    continue;
                            }

                            //TODO: also validate the positional values are correct on resource

                            for (int i = 0; i < hierarchies.Count; i++)
                            {
                                switch (resRef.resource_type)
                                {
                                    case ResourceType.COLLISION_MAPPING:
                                        WriteCollisionMap(id, hierarchies[i], composite, func);
                                        break;
                                    case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                                        WritePhysicsMap(hierarchies[i], resRef.index);
                                        break;
                                    case ResourceType.ANIMATED_MODEL:
                                        break;
                                    default:
                                        continue;
                                }
                                WriteResourceBin(instanceIDs[i], id);
                            }
                        }
                    }
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
            if (_commandsDisplay.Content.resource.env_animations != null && _commandsDisplay.Content.resource.env_animations.Entries != null)
                _commandsDisplay.Content.resource.env_animations.Save();
            if (_commandsDisplay.Content.resource.collision_maps != null && _commandsDisplay.Content.resource.collision_maps.Entries != null)
                _commandsDisplay.Content.resource.collision_maps.Save();
            if (_commandsDisplay.Content.mvr != null && _commandsDisplay.Content.mvr.Entries != null)
                _commandsDisplay.Content.mvr.Save();

            return true;
        }

        private void WritePhysicsMap(EntityPath hierarchy, int physics_system_index)
        {
            //If a composite further up in the path contains a PhysicsSystem too we shouldn't write this one out (NOTE: We also shouldn't write static stuff out by the looks of it)
            Composite comp = _commandsDisplay.Content.commands.EntryPoints[0];
            for (int x = 0; x < hierarchy.path.Count; x++)
            {
                FunctionEntity compInst = comp.functions.FirstOrDefault(o => o.shortGUID == hierarchy.path[x]);
                if (compInst == null)
                    break;

                comp = _commandsDisplay.Content.commands.GetComposite(compInst.function);
                if (x < hierarchy.path.Count - 2 && comp.GetFunctionEntitiesOfType(FunctionType.PhysicsSystem).Count != 0)
                {
                    return;
                }
            }

            //Get instance info
            (Vector3 position, Quaternion rotation) = CommandsUtils.CalculateInstancedPosition(hierarchy);
            ShortGuid compositeInstanceID = hierarchy.GenerateInstance();
            hierarchy.path.RemoveAt(hierarchy.path.Count - 2);
            EntityHandle compositeInstanceReference = new EntityHandle()
            {
                entity_id = hierarchy.path[hierarchy.path.Count - 2],
                composite_instance_id = hierarchy.GenerateInstance()
            };

            //Remove all entries that already exist for this instance
            _commandsDisplay.Content.resource.physics_maps.Entries.RemoveAll(o => o.composite_instance_id == compositeInstanceID && o.entity == compositeInstanceReference);

            //Make a new entry for the instance
            _commandsDisplay.Content.resource.physics_maps.Entries.Add(new PhysicsMaps.Entry()
            {
                physics_system_index = physics_system_index,
                resource_type = GUID_DYNAMIC_PHYSICS_SYSTEM,
                composite_instance_id = compositeInstanceID,
                entity = compositeInstanceReference,
                Position = position,
                Rotation = rotation
            });
        }

        private void WriteCollisionMap(ShortGuid resourceID, EntityPath hierarchy, Composite composite, FunctionEntity func)
        {
            //Get instance info
            EntityHandle compositeInstanceReference = new EntityHandle()
            {
                entity_id = func.shortGUID,
                composite_instance_id = hierarchy == null ? ShortGuid.Invalid : hierarchy.GenerateInstance()
            };

            if (_commandsDisplay.Content.resource.collision_maps.Entries.FindAll(o => o.entity == compositeInstanceReference && o.id == resourceID).Count != 0)
                return;

            //TODO: similar to PHYSICS.MAP, do we only write out if there's not another collision further up the chain?

            //Get zone ID
            ShortGuid zoneID = hierarchy == null ? ShortGuid.Invalid : new ShortGuid(1);
            if (hierarchy != null)
            {
                //TODO: this needs speeding up
                CancellationToken ct = new CancellationToken();
                _commandsDisplay.Content.editor_utils.TryFindZoneForEntity(func, composite, out Composite zoneComp, out FunctionEntity zoneEnt, ct);
                if (zoneComp != null && zoneEnt != null)
                {
                    //TODO: need to figure out how we know which zone instance to point at!
                    List<EntityPath> zoneInstances = _commandsDisplay.Content.editor_utils.GetHierarchiesForEntity(zoneComp, zoneEnt);
                    if (zoneInstances.Count > 0)
                        zoneID = zoneInstances[0].GenerateZoneID();
                }
            }

            //Make a new entry for the instance
            _commandsDisplay.Content.resource.collision_maps.Entries.Add(new CollisionMaps.Entry()
            {
                id = resourceID,
                entity = compositeInstanceReference,
                zone_id = zoneID
            });
        }

        private void WriteResourceBin(ShortGuid compositeInstanceID, ShortGuid resourceID)
        {
            if (_commandsDisplay.Content.resource.resources.Entries.FindAll(res => res.composite_instance_id == compositeInstanceID && res.resource_id == resourceID).Count != 0)
                return;

            _commandsDisplay.Content.resource.resources.Entries.Add(new Resources.Resource()
            {
                composite_instance_id = compositeInstanceID,
                resource_id = resourceID
            });
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
            //_commandsDisplay.LoadComposite(new ShortGuid("30-2E-B7-25"));
            //_commandsDisplay.CompositeDisplay.Composite.GetEntityByID(new ShortGuid("88-2E-34-D5")).AddParameter("position", new cTransform(new Vector3(-0.4999240f, 0.0003948f, -45.0000000f), new Vector3(0,0,0)));

            //_commandsDisplay.LoadComposite(new ShortGuid("7A-40-D8-07"));
            //_commandsDisplay.CompositeDisplay.Composite.GetEntityByID(new ShortGuid("A4-94-3A-1F")).AddParameter("Animation", new cString(""));
            //_commandsDisplay.CompositeDisplay.DeleteEntity(_commandsDisplay.CompositeDisplay.Composite.GetEntityByID(new ShortGuid("62-05-5E-F3")), false);
            //_commandsDisplay.CompositeDisplay.Composite.RemoveAllFunctionEntitiesOfType(FunctionType.Zone);

            _commandsDisplay.LoadComposite(new ShortGuid("83-AD-A3-18"));
            Singleton.OnEntityAdded += DEBUG_EntAdded;
            _commandsDisplay.CompositeDisplay.AddCopyOfEntity(_commandsDisplay.CompositeDisplay.Composite.GetEntityByID(new ShortGuid("6C-05-BE-DF")));
        }
        private void DEBUG_EntAdded(Entity ent)
        {
            Singleton.OnEntityAdded -= DEBUG_EntAdded;
            ((cTransform)ent.GetParameter("position").content).position.Z = -35;
            _commandsDisplay.CompositeDisplay.LoadEntity(ent);

            Save();
            Process.Start("E:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\AI.exe");
        }

        private void DEBUG_RunChecks_Click(object sender, EventArgs e)
        {
            LocalDebug.checkresources();
            return;

            List<EntityPath> zoneHandles = _commandsDisplay.Content.editor_utils.GetHierarchiesForEntity(_commandsDisplay.CompositeDisplay.Composite, _commandsDisplay.CompositeDisplay.ActiveEntityDisplay.Entity);
            for (int i = 0; i < zoneHandles.Count; i++)
            {
                ShortGuid zoneID = zoneHandles[i].GenerateZoneID();
                ShortGuid instanceID = zoneHandles[i].GenerateInstance();

                string sdfsdf = "";
            }


            //LocalDebug.checkphysicssystempositions();
        }

        private void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
