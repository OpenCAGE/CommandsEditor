using CATHODE;
using CATHODE.EXPERIMENTAL;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CathodeLib.ObjectExtensions;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups;
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

        private Thread _loadThread = null;
        private ProgressUI _progressUI = null;

        private float _defaultSplitterDistance = 0.25f;
        private int _defaultWidth;
        private int _defaultHeight;

        private string _baseTitle = "";

        public CommandsEditor(string level = null)
        {
            //LocalDebug.CheckWriteInstanced();

            InitializeComponent();

            Singleton.Editor = this;
            Singleton.LoadGlobals();

            //LocalDebug.GetExclusiveMasters("production/tech_comms");
            //LocalDebug.GetExclusiveMasters("production/hab_airport");

            _discord = new DiscordRpcClient("1152999067207606392");
            _discord.Initialize();
            _discord.SetPresence(new RichPresence() { Assets = new Assets() { LargeImageKey = "icon" } });

            Singleton.OnCompositeSelected += OnCompositeSelectedForDiscord;

            if (SettingsManager.GetFloat(Singleton.Settings.NumericStep, -1.0f) == -1.0f)
                SettingsManager.SetFloat(Singleton.Settings.NumericStep, 0.1f);
            if (SettingsManager.GetFloat(Singleton.Settings.NumericStepRot, -1.0f) == -1.0f)
                SettingsManager.SetFloat(Singleton.Settings.NumericStepRot, 1.0f);

            dockPanel.DockLeftPortion = SettingsManager.GetFloat(Singleton.Settings.CommandsSplitWidth, _defaultSplitterDistance);
            dockPanel.DockBottomPortion = SettingsManager.GetFloat(Singleton.Settings.SplitWidthMainBottom, _defaultSplitterDistance);
            dockPanel.DockRightPortion = SettingsManager.GetFloat(Singleton.Settings.SplitWidthMainRight, _defaultSplitterDistance);
            dockPanel.ShowDocumentIcon = true;

            _defaultWidth = Width;
            _defaultHeight = Height;

#if !DEBUG
            DEBUG_ReloadLevel.Visible = false;
            connectToRuntimeUtils.Visible = false;

            uIToolStripMenuItem.Enabled = false;
            animationsToolStripMenuItem.Enabled = false;
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
            writeInstancedResourcesExperimentalToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.CompileInstances); writeInstancedResourcesExperimentalToolStripMenuItem.PerformClick();
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

            if (!SettingsManager.IsSet(Singleton.Settings.AskBeforeDeletingNode)) SettingsManager.SetBool(Singleton.Settings.AskBeforeDeletingNode, true);
            showConfirmationWhenDeletingNodeToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.AskBeforeDeletingNode); showConfirmationWhenDeletingNodeToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_FunctionNode))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_FunctionNode, Color.FromArgb(30, 144, 255).ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_FunctionNodeBottom))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_FunctionNodeBottom, Color.FromArgb(10, 109, 157).ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_FunctionText))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_FunctionText, Color.White.ToArgb());

            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_AliasNode))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_AliasNode, Color.FromArgb(255, 114, 30).ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_AliasNodeBottom))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_AliasNodeBottom, Color.FromArgb(196, 76, 29).ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_AliasText))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_AliasText, Color.White.ToArgb());

            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_ProxyNode))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_ProxyNode, Color.FromArgb(35, 196, 22).ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_ProxyNodeBottom))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_ProxyNodeBottom, Color.FromArgb(9, 153, 72).ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_ProxyText))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_ProxyText, Color.White.ToArgb());

            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_InstanceNode))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_InstanceNode, Color.FromArgb(195, 30, 255).ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_InstanceNodeBottom))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_InstanceNodeBottom, Color.FromArgb(118, 10, 157).ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_InstanceText))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_InstanceText, Color.White.ToArgb());

            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_VariableNode))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_VariableNode, Color.Red.ToArgb());
            if (!SettingsManager.IsSet(Singleton.Settings.NodeColour_VariableText))
                SettingsManager.SetInteger(Singleton.Settings.NodeColour_VariableText, Color.White.ToArgb());

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
            List<string> levels = Level.GetLevels(SharedData.pathToAI);
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

        private void OnCompositeSelectedForDiscord(Composite composite)
        {
            RichPresence newPresence = _discord.CurrentPresence.Copy();
            newPresence.Details = "Level: " + (_commandsDisplay?.Content?.Level?.Name ?? "No Level");
            newPresence.State = "Composite: " + EditorUtils.GetCompositeName(composite);
            _discord.SetPresence(newPresence);
            _discord.UpdateStartTime();
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

            if (_commandsDisplay != null)
            {
                Singleton.Editor.DockPanel.ActiveAutoHideContent = null;
                string oldLevelName = _commandsDisplay.Content?.Level?.Name;
                if (oldLevelName != null)
                    _levelMenuItems[oldLevelName].Checked = false;

                _commandsDisplay.CloseAllChildTabs();
                _commandsDisplay.Close();
                _commandsDisplay = null;
                
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                GC.WaitForPendingFinalizers();
            }

#if DEBUG
            if (Directory.Exists(SharedData.pathToAI + "\\LatestBuiltData\\ENV"))
            {
                Directory.Delete(SharedData.pathToAI + "\\DATA\\ENV\\" + level, true);
                CopyFilesRecursively(SharedData.pathToAI + "\\LatestBuiltData\\ENV\\" + level, SharedData.pathToAI + "\\DATA\\ENV\\" + level);
            }
#endif

            _commandsDisplay = new CommandsDisplay(level);
            Singleton.OnLevelLoaded += ShowCommandsDisplayWhenLoaded;

            _progressUI = new ProgressUI();
            _progressUI.ShowLevelLoading(_commandsDisplay.Content.Level);
            _progressUI.BringToFront();
            this.BringToFront();
            this.Activate();
            EnableButtons(false, "Loading " + _commandsDisplay.Content.Level.Name + "...");

            _loadThread = new Thread(ThreadedLevelLoader);
            _loadThread.Start();

            _levelMenuItems[_commandsDisplay.Content.Level.Name].Checked = true;
            UpdateTitle();
        }

        private void ThreadedLevelLoader()
        {
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                _commandsDisplay.Content.Load();
#if !CATHODE_FAIL_HARD
            }
            catch
            {
                this.BeginInvoke(new Action(() =>
                {
                    CloseProgressUI();
                    EnableButtons(true, "");
                    //TODO: warn!
                }));
            }
#endif
        }

        private void CloseProgressUI()
        {
            if (_progressUI != null && !_progressUI.IsDisposed)
            {
                _progressUI.Close();
                _progressUI.Dispose();
                _progressUI = null;
            }
        }

        private void ShowCommandsDisplayWhenLoaded(LevelContent content)
        {
            Singleton.OnLevelLoaded -= ShowCommandsDisplayWhenLoaded;

            Singleton.Editor.BeginInvoke(new Action(() => 
            {
                CloseProgressUI();
                EnableButtons(true, "");

                _commandsDisplay.Resize += _commandsDisplay_Resize;
                _commandsDisplay.FormClosed += _commandsDisplay_FormClosed;
                _commandsDisplay.UpdateDockState();

                Singleton.Editor.Activate();
                Singleton.Editor.Focus();
            }));
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

            Cursor.Current = Cursors.WaitCursor;
            statusText.Text = "Saving...";
            statusStrip.Update();

            _progressUI = new ProgressUI();
            _progressUI.ShowLevelSaving(_commandsDisplay.Content.Level);

            if (_commandsDisplay.CompositeDisplay != null)
                _commandsDisplay.CompositeDisplay.SaveAllFlowgraphs();

#if DEBUG
            if (SettingsManager.GetBool(Singleton.Settings.CompileInstances))
            {
                _commandsDisplay.Content.Level.Resources.Entries.Clear();
                _commandsDisplay.Content.Level.PhysicsMaps.Entries.Clear();
                //todo - clear others when i write them

                Instancing inst = new Instancing(_commandsDisplay.Content.Level);
                inst.GenerateInstances();
                inst.ProcessInstances();
            }
#endif

            //TODO: take a backup first
            _commandsDisplay.Content.Level.Save();

            if (!_commandsDisplay.Content.Level.Commands.Compressed && SettingsManager.GetBool(Singleton.Settings.SavePakAndBin))
            {
                string ext = "BIN";
                if (Path.GetExtension(_commandsDisplay.Content.Level.Commands.Filepath).ToUpper() == ".BIN")
                    ext = "PAK";
                _commandsDisplay.Content.Level.Commands.Save(_commandsDisplay.Content.Level.Commands.Filepath.Substring(0, _commandsDisplay.Content.Level.Commands.Filepath.Length - 3) + ext, false);
            }

#if !DEBUG
            PatchManager.Platform? platform = null;
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
            }
            if (platform.HasValue)
            {
                PatchManager.PatchFileIntegrityCheck(platform.Value, SharedData.pathToAI);
                PatchManager.PatchPopupMessage(platform.Value, SharedData.pathToAI);
                PatchManager.UpdateLevelListInPackages(platform.Value, SharedData.pathToAI);

                PatchManager.PatchSkipFrontendFlag(platform.Value, SharedData.pathToAI, SettingsManager.GetBool("OPT_SkipFE"));
                PatchManager.PatchNoUIFlag(platform.Value, SharedData.pathToAI, SettingsManager.GetBool("OPT_HudDisabled"));
                PatchManager.PatchMemReplayLogFlag(platform.Value, SharedData.pathToAI, SettingsManager.GetBool("OPT_Mem_Replay_Logs"));
                PatchManager.PatchUIPerfFlag(platform.Value, SharedData.pathToAI, SettingsManager.GetBool("OPT_cUIEnabled_UIPerf"));

                if (SettingsManager.GetBool(Singleton.Settings.LaunchGameWhenSaved))
                {
                    PatchManager.PatchLaunchMode(platform.Value, SharedData.pathToAI, _commandsDisplay.Content.Level.Name);

                    if (platform.Value == PatchManager.Platform.STEAM)
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
            }
#endif

            statusText.Text = "";
            Cursor.Current = Cursors.Default;
            CloseProgressUI();

            Singleton.OnSaved?.Invoke();
            //if (saved)
            //{
                if (SettingsManager.GetBool(Singleton.Settings.ShowSavedMsgOpt))
                    MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //    MessageBox.Show("Failed to save changes!", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void EnableButtons(bool shouldEnable, string text)
        {
            try
            {
                if (toolStrip.InvokeRequired)
                    toolStrip.Invoke(new Action(() => { toolStrip.Enabled = shouldEnable; toolStrip.Refresh(); }));
                else
                    toolStrip.Enabled = shouldEnable; toolStrip.Refresh();

                if (statusStrip.InvokeRequired)
                    statusStrip.Invoke(new Action(() => { statusText.Text = text; statusStrip.Update(); }));
                else
                    statusText.Text = text; statusStrip.Update();
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
            SettingsManager.SetBool(Singleton.Settings.CompileInstances, writeInstancedResourcesExperimentalToolStripMenuItem.Checked);
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

        EditModel _modelEditor = null;
        private void modelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_modelEditor != null)
            {
                _modelEditor.FormClosed -= _modelEditor_FormClosed;
                _modelEditor.Close();
            }

            _modelEditor = new EditModel(null, false);
            _modelEditor.Show();
            _modelEditor.FormClosed += _modelEditor_FormClosed;
        }
        private void _modelEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            _modelEditor = null;
        }

        EditMaterial _materialEditor = null;
        private void materialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_materialEditor != null)
            {
                _materialEditor.FormClosed -= _materialEditor_FormClosed;
                _materialEditor.Close();
            }

            _materialEditor = new EditMaterial(null, false);
            _materialEditor.Show();
            _materialEditor.FormClosed += _materialEditor_FormClosed;
        }
        private void _materialEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            _materialEditor = null;
        }

        EditMaterialMapping _materialMappingEditor = null;
        private void materialMappingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_materialMappingEditor != null)
            {
                _materialMappingEditor.FormClosed -= _materialMappingEditor_FormClosed;
                _materialMappingEditor.Close();
            }

            _materialMappingEditor = new EditMaterialMapping(null, false);
            _materialMappingEditor.Show();
            _materialMappingEditor.FormClosed += _materialMappingEditor_FormClosed;
        }
        private void _materialMappingEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            _materialMappingEditor = null;
        }

        EditTexture _textureEditor = null;
        private void texturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_textureEditor != null)
            {
                _textureEditor.FormClosed -= _textureEditor_FormClosed;
                _textureEditor.Close();
            }

            _textureEditor = new EditTexture(null, false);
            _textureEditor.Show();
            _textureEditor.FormClosed += _textureEditor_FormClosed;
        }
        private void _textureEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            _textureEditor = null;
        }

        GalaxyEditor _galaxyEditor = null;
        private void galaxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_galaxyEditor != null)
            {
                _galaxyEditor.FormClosed -= _galaxyEditor_FormClosed;
                _galaxyEditor.Close();
            }

            _galaxyEditor = new GalaxyEditor();
            _galaxyEditor.Show();
            _galaxyEditor.FormClosed += _galaxyEditor_FormClosed;
        }
        private void _galaxyEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            _galaxyEditor = null;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            modelsToolStripMenuItem.Enabled = _commandsDisplay?.Content?.Level != null;
            materialsToolStripMenuItem.Enabled = _commandsDisplay?.Content?.Level != null;
            materialMappingsToolStripMenuItem.Enabled = _commandsDisplay?.Content?.Level != null;
            texturesToolStripMenuItem.Enabled = _commandsDisplay?.Content?.Level != null;
            galaxyToolStripMenuItem.Enabled = _commandsDisplay?.Content?.Level != null;
        }

        SetNodeColours _setNodeColours;
        private void setNodeColoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_setNodeColours != null)
                _setNodeColours.Close();

            _setNodeColours = new SetNodeColours();
            _setNodeColours.Show();
        }

        private void showConfirmationWhenDeletingNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showConfirmationWhenDeletingNodeToolStripMenuItem.Checked = !showConfirmationWhenDeletingNodeToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.AskBeforeDeletingNode, showConfirmationWhenDeletingNodeToolStripMenuItem.Checked);
        }

        private void miscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savePAKAndBINToolStripMenuItem.Enabled = _commandsDisplay?.Content?.Level?.Commands != null && _commandsDisplay.Content.Level.Commands.Compressed;
        }

        ControlsWindow _controlsWindow = null;
        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
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

        LaunchGame _launchGamePopup = null;
        private void launchGameBtn_Click(object sender, EventArgs e)
        {
            if (_launchGamePopup != null)
            {
                _launchGamePopup.FormClosed -= _launchGamePopup_FormClosed;
                _launchGamePopup.Close();
            }

            _launchGamePopup = new LaunchGame();
            _launchGamePopup.Show();
            _launchGamePopup.FormClosed += _launchGamePopup_FormClosed;
        }
        private void _launchGamePopup_FormClosed(object sender, FormClosedEventArgs e)
        {
            _launchGamePopup = null;
        }

        private void uIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo
        }

        private void animationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo
        }
    }
}
