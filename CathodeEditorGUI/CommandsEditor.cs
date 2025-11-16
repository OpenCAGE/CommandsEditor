//#define DO_TEST_STUFF

using CATHODE;
using CATHODE.EXPERIMENTAL;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CathodeLib.ObjectExtensions;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups;
using CommandsEditor.Properties;
using CommandsEditor.Scripts;
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
using System.IO.Compression;
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
using Task = System.Threading.Tasks.Task;

namespace CommandsEditor
{
    public partial class CommandsEditor : Form
    {
        public DockPanel DockPanel => dockPanel;

        private CommandsDisplay _commandsDisplay = null;
        public CommandsDisplay CommandsDisplay => _commandsDisplay;

        private SelectLevel _levelSelect = null;

        private DiscordRpcClient _discord;

        private Dictionary<string, ToolStripMenuItem> _levelMenuItems = new Dictionary<string, ToolStripMenuItem>();

        private float _defaultSplitterDistance = 0.25f;
        private int _defaultWidth;
        private int _defaultHeight;

        private string _baseTitle = "";

        public CommandsEditor(string level = null)
        {
            //LocalDebug.CheckFlowgraphsNew();

            //string sfsdf = "";

            //LocalDebug.TestLights();

            //LocalDebug.DefaultsUnitTest();
            //LocalDebug.ApplyAllDefaults();

#if DEBUG && DO_TEST_STUFF
            //List<string> lvls = Level.GetLevels(SharedData.pathToAI, true);
            //foreach (string lvl in lvls)
            //{
            //    if (Directory.Exists(SharedData.pathToAI + "\\DATA_orig"))
            //    {
            //        Directory.Delete(SharedData.pathToAI + "\\DATA\\ENV\\" + lvl, true);
            //        CopyFilesRecursively(SharedData.pathToAI + "\\DATA_orig\\ENV\\" + lvl, SharedData.pathToAI + "\\DATA\\ENV\\" + lvl);
            //    }
            //
            //    InstancedResolver.Read(LevelContent.DEBUG_LoadUnthreadedAndPopulateShortGuids(lvl));
            //}


            level = "TECH_RND_HZDLAB";

            if (Directory.Exists(SharedData.pathToAI + "\\DATA_orig"))
            {
                Directory.Delete(SharedData.pathToAI + "\\DATA\\ENV\\" + level, true);
                CopyFilesRecursively(SharedData.pathToAI + "\\DATA_orig\\ENV\\" + level, SharedData.pathToAI + "\\DATA\\ENV\\" + level);
            }


            //Level is populated by MVR
            //MVR looks up the resource in COMMANDS via RESOURCES
            //MVR properties are applied to renderables in COMMANDS via this link (applied properties are specified by the type enum, e.g. not all provide position etc)
            //If no link has been made, both populate


            //It gets more complicated: RESOURCES.BIN is populated based on the result of COMMANDS (e.g. if an entity has "deleted" applied, it won't be listed there)
            //No clue how I'm gonna be able to rewrite the thing without building the entire scripting logic into the compiler -_-

            // ^ need to look into "offline_only" entities and ones implemented offline, and reconstruct similar logic here.


            LevelContent content = LevelContent.DEBUG_LoadUnthreadedAndPopulateShortGuids(level);

            //File.Copy(content.mvr.Filepath, content.mvr.Filepath + " - Copy.MVR");
            //content.mvr.Entries = content.mvr.Entries.OrderBy(o => o.resource_index).ToList();
            //content.mvr.Save();

            InstancedResolver.Read(content);
            InstancedResolver.Write(content);
            InstancedResolver.Read(content);

            int min_collision_index = 999999;
            for (int x = 0; x < content.commands.Entries.Count; x++)
            {
                foreach (var function in content.commands.Entries[x].functions_dictionary.Values)
                {
                    for (int z = 0; z < function.resources.Count; z++)
                    {
                        if (function.resources[z].resource_type != ResourceType.COLLISION_MAPPING)
                            continue;
                        if (function.resources[z].index != -1 && min_collision_index > function.resources[z].index)
                            min_collision_index = function.resources[z].index;
                    }

                    Parameter resourceParam = function.GetParameter("resource");
                    if (resourceParam != null && resourceParam.content != null && resourceParam.content.dataType == DataType.RESOURCE)
                    {
                        cResource resource = (cResource)resourceParam.content;
                        for (int z = 0; z < resource.value.Count; z++)
                        {
                            if (resource.value[z].resource_type != ResourceType.COLLISION_MAPPING)
                                continue;
                            if (resource.value[z].index != -1 && min_collision_index > resource.value[z].index)
                                min_collision_index = resource.value[z].index;
                        }
                    }
                }
            }
            if (min_collision_index != 18)
            {
                //There are always 18 entries at the start of COLLISION.MAP which are empty for some reason, so we'd expect the min index to be 18
                Console.WriteLine("Unexpected!");
            }

#endif
            //*/
            //return;

            Singleton.Editor = this;
            Singleton.LoadGlobals();

            _discord = new DiscordRpcClient("1152999067207606392");
            _discord.Initialize();
            _discord.SetPresence(new RichPresence() { Assets = new Assets() { LargeImageKey = "icon" } });

            Singleton.OnCompositeSelected += delegate (Composite composite)
            {
                RichPresence newPresence = _discord.CurrentPresence.Copy();
                newPresence.Details = "Level: " + _commandsDisplay?.Content?.Level?.Name;
                newPresence.State = "Composite: " + EditorUtils.GetCompositeName(composite);
                _discord.SetPresence(newPresence);
                _discord.UpdateStartTime();
            };

            if (SettingsManager.GetFloat(Singleton.Settings.NumericStep, -1.0f) == -1.0f)
                SettingsManager.SetFloat(Singleton.Settings.NumericStep, 0.1f);
            if (SettingsManager.GetFloat(Singleton.Settings.NumericStepRot, -1.0f) == -1.0f)
                SettingsManager.SetFloat(Singleton.Settings.NumericStepRot, 1.0f);

            InitializeComponent();
            dockPanel.DockLeftPortion = SettingsManager.GetFloat(Singleton.Settings.CommandsSplitWidth, _defaultSplitterDistance);
            dockPanel.DockBottomPortion = SettingsManager.GetFloat(Singleton.Settings.SplitWidthMainBottom, _defaultSplitterDistance);
            dockPanel.DockRightPortion = SettingsManager.GetFloat(Singleton.Settings.SplitWidthMainRight, _defaultSplitterDistance);
            dockPanel.ShowDocumentIcon = true;

            _defaultWidth = Width;
            _defaultHeight = Height;

#if !DEBUG
            DEBUG_DoorPhysEnt.Visible = false;
            buildLevelToolStripMenuItem.Visible = false;
            DEBUG_ReloadLevel.Visible = false;
            connectToRuntimeUtils.Visible = false;
#endif

            WindowState = SettingsManager.GetString(Singleton.Settings.WindowState, "Normal") == "Maximized" ? FormWindowState.Maximized : FormWindowState.Normal;
            Width = SettingsManager.GetInteger(Singleton.Settings.WindowWidth, _defaultWidth);
            Height = SettingsManager.GetInteger(Singleton.Settings.WindowHeight, _defaultHeight);
            Resize += CommandsEditor_Resize;
            FormClosing += CommandsEditor_FormClosing;

            if (!SettingsManager.IsSet(Singleton.Settings.ServerOpt)) SettingsManager.SetBool(Singleton.Settings.ServerOpt, true);
            connectToUnity.Checked = !SettingsManager.GetBool(Singleton.Settings.ServerOpt); connectToUnity.PerformClick();
            
            if (!SettingsManager.IsSet(Singleton.Settings.RuntimeUtilsOpt)) SettingsManager.SetBool(Singleton.Settings.RuntimeUtilsOpt, false);
            connectToRuntimeUtils.Checked = SettingsManager.GetBool(Singleton.Settings.RuntimeUtilsOpt);
            if (connectToRuntimeUtils.Checked)
            {
                if (!RuntimeUtilsConnection.Send.Start())
                {
                    connectToRuntimeUtils.Checked = false;
                    SettingsManager.SetBool(Singleton.Settings.RuntimeUtilsOpt, false);
                }
            }
            focusOnSelectedToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.UNITY_FocusEntity); focusOnSelectedToolStripMenuItem.PerformClick();
            ShowLevelViewerButton();

            showEntityIDs.Checked = !SettingsManager.GetBool(Singleton.Settings.ShowShortGuids); showEntityIDs.PerformClick();
            searchOnlyCompositeNames.Checked = !SettingsManager.GetBool(Singleton.Settings.CompNameOnlyOpt); searchOnlyCompositeNames.PerformClick();
            useTexturedModelViewExperimentalToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ShowTexOpt); useTexturedModelViewExperimentalToolStripMenuItem.PerformClick();
            keepFunctionUsesWindowOpenToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.KeepUsesWindowOpen); keepFunctionUsesWindowOpenToolStripMenuItem.PerformClick();
            writeInstancedResourcesExperimentalToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ExperimentalResourceStuff); writeInstancedResourcesExperimentalToolStripMenuItem.PerformClick();
            openGameOnSaveToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.LaunchGameWhenSaved); openGameOnSaveToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.ShowSavedMsgOpt)) SettingsManager.SetBool(Singleton.Settings.ShowSavedMsgOpt, true);
            showConfirmationWhenSavingToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ShowSavedMsgOpt); showConfirmationWhenSavingToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.EnableFileBrowser)) SettingsManager.SetBool(Singleton.Settings.EnableFileBrowser, true);
            showExplorerViewToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.EnableFileBrowser); showExplorerViewToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.AutoHideCompositeDisplay)) SettingsManager.SetBool(Singleton.Settings.AutoHideCompositeDisplay, true);
            autoHideExplorerViewToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.AutoHideCompositeDisplay); autoHideExplorerViewToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.SavePakAndBin)) SettingsManager.SetBool(Singleton.Settings.SavePakAndBin, true);
            savePAKAndBINToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.SavePakAndBin); savePAKAndBINToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.PopulateAllPinsOnCreateNode)) SettingsManager.SetBool(Singleton.Settings.PopulateAllPinsOnCreateNode, true);
            populateAllNodePinsWhenCreatedToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.PopulateAllPinsOnCreateNode); populateAllNodePinsWhenCreatedToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.OptionToDeleteEntityWithNode)) SettingsManager.SetBool(Singleton.Settings.OptionToDeleteEntityWithNode, true);
            giveOptionToDeleteEntityWhenNoNodesToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.OptionToDeleteEntityWithNode); giveOptionToDeleteEntityWhenNoNodesToolStripMenuItem.PerformClick();

            //Fixes for dodgy top dropdowns
            compositeViewerToolStripMenuItem.MouseHover += (sender, e) => { ((ToolStripMenuItem)sender).PerformClick(); };
            compositeViewerToolStripMenuItem.DropDown.Closing += DropDown_Closing;
            entityDisplayToolStripMenuItem.MouseHover += (sender, e) => { ((ToolStripMenuItem)sender).PerformClick(); };
            entityDisplayToolStripMenuItem.DropDown.Closing += DropDown_Closing;
            miscToolStripMenuItem.MouseHover += (sender, e) => { ((ToolStripMenuItem)sender).PerformClick(); };
            miscToolStripMenuItem.DropDown.Closing += DropDown_Closing;
            toolStripButton2.DropDown.Closing += DropDown_Closing;

            //Set title
            _baseTitle = "OpenCAGE Commands Editor";
            if (OpenCAGE.SettingsManager.GetBool("CONFIG_ShowPlatform") &&
                OpenCAGE.SettingsManager.GetString("META_GameVersion") != "")
            {
                switch (OpenCAGE.SettingsManager.GetString("META_GameVersion"))
                {
                    case "STEAM":
                        _baseTitle += " - Steam";
                        break;
                    case "EPIC_GAMES_STORE":
                        _baseTitle += " - Epic Games Store";
                        break;
                    case "GOG":
                        _baseTitle += " - GoG";
                        break;
                }
            }
            DirtyTracker.OnChanged += OnDirtyChanged;
            UpdateTitle();

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
            KillLevelViewer();
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

        private void OnDirtyChanged(bool dirty) => UpdateTitle();
        private void UpdateTitle()
        {
            if (_commandsDisplay == null)
            {
                this.Text = _baseTitle;
            }
            else
            {
                string[] levelBits = _commandsDisplay.Content.Level.Name.Split('/');
                this.Text = _baseTitle + " - " + levelBits[levelBits.Length - 1] + " (" + _commandsDisplay.Content.Level.Name.Substring(0, _commandsDisplay.Content.Level.Name.Length - levelBits[levelBits.Length - 1].Length).TrimEnd('/') + ")";
            }

#if USE_DIRTY_TRACKER
            if (DirtyTracker.IsDirty)
                this.Text += " [UNSAVED CHANGES]";
#endif
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
            if (level == null)
                return;
            level = level.ToUpper();

#if DEBUG
            if (Directory.Exists(SharedData.pathToAI + "\\DATA_orig"))
            {
                Directory.Delete(SharedData.pathToAI + "\\DATA\\ENV\\" + level, true);
                CopyFilesRecursively(SharedData.pathToAI + "\\DATA_orig\\ENV\\" + level, SharedData.pathToAI + "\\DATA\\ENV\\" + level);
            }
#endif

            statusText.Text = "Loading " + level + "...";
            statusStrip.Update();

            _levelSelect = null;

            //Close all existing
            if (_commandsDisplay != null)
            {
                _levelMenuItems[_commandsDisplay.Content.Level.Name].Checked = false;

                _commandsDisplay.CloseAllChildTabs();
                _commandsDisplay.Close();
            }

            //Load new
            _commandsDisplay = new CommandsDisplay(level);
            _commandsDisplay.Resize += _commandsDisplay_Resize;
            _commandsDisplay.FormClosed += _commandsDisplay_FormClosed;
            UpdateCommandsDisplayDockState();

            //Sometimes get an error here which appears to be thread related (?) -> investigate next time
            _levelMenuItems[_commandsDisplay.Content.Level.Name].Checked = true;

            UpdateTitle();
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

            if (_commandsDisplay.CompositeDisplay != null)
                _commandsDisplay.CompositeDisplay.SaveAllFlowgraphs();
            
            bool saved = Save();
            statusText.Text = "";
            Cursor.Current = Cursors.Default;

            ShowSaveMsg(saved);
        }
        private void ShowSaveMsg(bool saved)
        {
            Singleton.OnSaved?.Invoke();
            if (saved)
            {
                if (SettingsManager.GetBool(Singleton.Settings.ShowSavedMsgOpt))
                    MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Failed to save changes!", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buildLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: save but save with all the instanced stuff
        }

        private bool Save()
        {
            //Close alien down if it's open, it conflicts with our write locks!
            List<Process> allProcesses = new List<Process>(Process.GetProcessesByName("AI"));
            for (int x = 0; x < allProcesses.Count; x++)
            {
                try
                {
                    allProcesses[x].Kill();
                    allProcesses[x].WaitForExit();
                }
                catch { }
            }

            //TODO: take a backup first
            _commandsDisplay.Content.Level.Save();

            if (SettingsManager.GetBool(Singleton.Settings.SavePakAndBin))
            {
                string ext = "BIN";
                if (Path.GetExtension(_commandsDisplay.Content.Level.Commands.Filepath).ToUpper() == ".BIN")
                    ext = "PAK";
                _commandsDisplay.Content.Level.Commands.Save(_commandsDisplay.Content.Level.Commands.Filepath.Substring(0, _commandsDisplay.Content.Level.Commands.Filepath.Length - 3) + ext, false);
            }

            if (SettingsManager.GetBool(Singleton.Settings.ExperimentalResourceStuff))
            {
                //Commands cmd = _commandsDisplay.Content.Level.Commands;
                //foreach (Composite comp in cmd.Entries)
                //{
                //    List<FunctionEntity> soundLoadBanks = comp.GetFunctionEntitiesOfType(FunctionType.SoundLoadBank);
                //    foreach (FunctionEntity soundLoadBank in soundLoadBanks)
                //    {
                //        string fdsdfsdf = "";
                //    }
                //}
            }

            if (SettingsManager.GetBool(Singleton.Settings.LaunchGameWhenSaved))
            {
                PatchManager.Platform platform;
                switch (OpenCAGE.SettingsManager.GetString("META_GameVersion"))
                {
                    case "STEAM":
                        platform = PatchManager.Platform.STEAM;
                        break;
                    case "EPIC_GAMES_STORE":
                        platform = PatchManager.Platform.EPIC_GAMES_STORE;
                        break;
                    case "GOG":
                        platform = PatchManager.Platform.GOG;
                        break;
                    default:
                        return true;
                }

                PatchManager.PatchLaunchMode(platform, SharedData.pathToAI, _commandsDisplay.Content.Level.Name);
                PatchManager.PatchFileIntegrityCheck(platform, SharedData.pathToAI);
                PatchManager.PatchPopupMessage(platform, SharedData.pathToAI);
                PatchManager.UpdateLevelListInPackages(platform, SharedData.pathToAI);

                PatchManager.PatchSkipFrontendFlag(platform, SharedData.pathToAI, SettingsManager.GetBool("OPT_SkipFE"));
                PatchManager.PatchNoUIFlag(platform, SharedData.pathToAI, SettingsManager.GetBool("OPT_HudDisabled"));
                PatchManager.PatchMemReplayLogFlag(platform, SharedData.pathToAI, SettingsManager.GetBool("OPT_Mem_Replay_Logs"));
                PatchManager.PatchUIPerfFlag(platform, SharedData.pathToAI, SettingsManager.GetBool("OPT_cUIEnabled_UIPerf"));

                if (platform == PatchManager.Platform.STEAM)
                {
                    Process.Start("steam://rungameid/214490");
                }
                else
                {
                    ProcessStartInfo alienProcess = new ProcessStartInfo();
                    alienProcess.WorkingDirectory = SettingsManager.GetString("PATH_GameRoot");
                    alienProcess.FileName = SettingsManager.GetString("PATH_GameRoot") + "/AI.exe";
                    Process.Start(alienProcess);
                }
            }

            return true;
        }

        /* Enable the option to load */
        public void EnableLoadingOfPaks(bool shouldEnable, string text)
        {
            try
            {
                toolStrip?.Invoke(new Action(() => { toolStrip.Enabled = shouldEnable; }));
                statusStrip?.Invoke(new Action(() => { statusText.Text = text; }));
            }
            catch { }
        }

        /* Websocket to Unity */
        private void openLevelViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KillLevelViewer();

            LevelViewerSetup.UnityProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = LevelViewerSetup.InstallationPath,
                    Arguments = $"-projectPath \"{SettingsManager.GetString("PATH_GameRoot")}/DATA/MODTOOLS/REMOTE_ASSETS/levelviewer\"",
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            }; 
            LevelViewerSetup.UnityProcess.Exited += UnityProcess_Exited;
            LevelViewerSetup.UnityProcess.Start();

            openLevelViewerToolStripMenuItem.Enabled = false;
        }
        private void UnityProcess_Exited(object sender, EventArgs e)
        {
            if (openLevelViewerToolStripMenuItem == null)
                return;

            var parent = openLevelViewerToolStripMenuItem.GetCurrentParent();
            if (parent != null && parent.InvokeRequired)
            {
                parent.Invoke(new Action(() =>
                {
                    openLevelViewerToolStripMenuItem.Enabled = true;
                }));
            }
            else
            {
                openLevelViewerToolStripMenuItem.Enabled = true;
            }
        }
        private void connectToUnity_Click(object sender, EventArgs e)
        {
            connectToUnity.Checked = !connectToUnity.Checked;
            SettingsManager.SetBool(Singleton.Settings.ServerOpt, connectToUnity.Checked);

            if (connectToUnity.Checked)
            {
                if (!UnityConnection.Send.Start())
                {
                    connectToUnity.PerformClick();
                    MessageBox.Show("Failed to initialise Unity connection.\nIs another instance of the script editor running?", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                UnityConnection.Send.Stop();
            }
        }
        private void focusOnSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            focusOnSelectedToolStripMenuItem.Checked = !focusOnSelectedToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.UNITY_FocusEntity, focusOnSelectedToolStripMenuItem.Checked);
            UnityConnection.Send.SendReSyncPacket();
        }
        private void connectToRuntimeUtils_Click(object sender, EventArgs e)
        {
            connectToRuntimeUtils.Checked = !connectToRuntimeUtils.Checked;
            SettingsManager.SetBool(Singleton.Settings.RuntimeUtilsOpt, connectToRuntimeUtils.Checked);

            if (connectToRuntimeUtils.Checked)
            {
                if (!RuntimeUtilsConnection.Send.Start())
                {
                    connectToRuntimeUtils.PerformClick();
                    MessageBox.Show("Failed to connect to RuntimeUtils server.\nIs the game running with the RuntimeUtils DLL loaded?", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                RuntimeUtilsConnection.Send.Stop();
            }
        }
        private void setUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setUpToolStripMenuItem.Enabled = false;
            LevelViewerSetup setup = new LevelViewerSetup();
            setup.FormClosed += Setup_FormClosed;
            setup.Show();
        }
        private void Setup_FormClosed(object sender, FormClosedEventArgs e)
        {
            setUpToolStripMenuItem.Enabled = true;
            ShowLevelViewerButton();
        }
        private static void KillLevelViewer()
        {
            if (LevelViewerSetup.UnityProcess != null && !LevelViewerSetup.UnityProcess.HasExited)
                LevelViewerSetup.UnityProcess.Kill();
        }
        private void ShowLevelViewerButton()
        {
            setUpToolStripMenuItem.Visible = !LevelViewerSetup.Installed;
            openLevelViewerToolStripMenuItem.Visible = LevelViewerSetup.Installed;
        }

        private void showEntityIDs_Click(object sender, EventArgs e)
        {
            showEntityIDs.Checked = !showEntityIDs.Checked;
            SettingsManager.SetBool(Singleton.Settings.ShowShortGuids, showEntityIDs.Checked);

            _commandsDisplay?.Reload(true);
            //TODO: also reload hierarchy cache
        }

        private void searchOnlyCompositeNames_Click(object sender, EventArgs e)
        {
            searchOnlyCompositeNames.Checked = !searchOnlyCompositeNames.Checked;
            SettingsManager.SetBool(Singleton.Settings.CompNameOnlyOpt, searchOnlyCompositeNames.Checked);
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

        SetNumericStep numericStepConfig = null;
        private void setNumericStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (numericStepConfig != null)
            {
                numericStepConfig.Close();
            }
            numericStepConfig = new SetNumericStep();
            numericStepConfig.Show();
        }

        private void savePAKAndBINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savePAKAndBINToolStripMenuItem.Checked = !savePAKAndBINToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.SavePakAndBin, savePAKAndBINToolStripMenuItem.Checked);
        }

        private void populateAllNodePinsWhenCreatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            populateAllNodePinsWhenCreatedToolStripMenuItem.Checked = !populateAllNodePinsWhenCreatedToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.PopulateAllPinsOnCreateNode, populateAllNodePinsWhenCreatedToolStripMenuItem.Checked);
        }

        private void giveOptionToDeleteEntityWhenNoNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            giveOptionToDeleteEntityWhenNoNodesToolStripMenuItem.Checked = !giveOptionToDeleteEntityWhenNoNodesToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.OptionToDeleteEntityWithNode, giveOptionToDeleteEntityWhenNoNodesToolStripMenuItem.Checked);
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
            _commandsDisplay?.CompositeDisplay?.ResetPortions();
        }

        private void writeInstancedResourcesExperimentalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeInstancedResourcesExperimentalToolStripMenuItem.Checked = !writeInstancedResourcesExperimentalToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.ExperimentalResourceStuff, writeInstancedResourcesExperimentalToolStripMenuItem.Checked);
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

        List<CollisionMaps.COLLISION_MAPPING> entries = new List<CollisionMaps.COLLISION_MAPPING>();
        private void SaveCollisionMaps(Composite composite)
        {
            if (composite == null)
                return;

            foreach (var function in composite.functions_dictionary.Values)
            {
                if (function.function.IsFunctionType)
                {
                    //TODO: this assumes that entities don't have collision mappings in the resoure parameter and entity resources
                    ResourceReference collision = function.GetResource(ResourceType.COLLISION_MAPPING, true);
                    if (collision == null)
                        continue;

                    //TODO: need to make sure "collision_index" is the same for every entry with the same entity, then we can show it

                    CollisionMaps.COLLISION_MAPPING collisionMap = new CollisionMaps.COLLISION_MAPPING()
                    {
                        ResourceGUID = collision.resource_id,
                        //CollisionProxyIndex = collision.index,
                        Entity = new EntityHandle()
                        {
                            entity_id = function.shortGUID
                            //TODO: generate composite guid
                        }
                    };

                    //collision.resource_id

                    //switch (CommandsUtils.GetFunctionType(function.function))
                    //{
                    //    case FunctionType.
                    //}
                }
                else
                {
                    SaveCollisionMaps(_commandsDisplay.Content.Level.Commands.GetComposite(function.function));
                }
            }
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

        ControlsWindow _controlsWindow = null;
        private void ShowControls_Click(object sender, EventArgs e)
        {
            if (_controlsWindow != null)
            {
                _controlsWindow.FormClosed -= _controlsWindow_FormClosed;
                _controlsWindow.Close();
            }

            _controlsWindow = new ControlsWindow();
            _controlsWindow.Show();
            _controlsWindow.FormClosed += _controlsWindow_FormClosed;
        }
        private void _controlsWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controlsWindow = null;
        }

        private void DEBUG_ReloadLevel_Click(object sender, EventArgs e)
        {
            if (!RuntimeUtilsConnection.Send.Connected)
            {
                MessageBox.Show("Cannot reload level - not connected to RuntimeUtils", "Not connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RuntimeUtilsConnection.Send.SendData(new RuntimeUtilsConnection.Packet() { load_level = "Production/HAB_Airport" });
        }

        private void openGameOnSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openGameOnSaveToolStripMenuItem.Checked = !openGameOnSaveToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.LaunchGameWhenSaved, openGameOnSaveToolStripMenuItem.Checked);
        }
    }
}
