//#define DO_TEST_STUFF

using CATHODE;
using CATHODE.EXPERIMENTAL;
using CATHODE.LEGACY;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
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
            //LocalDebug.DefaultsUnitTest();
            //LocalDebug.ApplyAllDefaults();

#if DEBUG && DO_TEST_STUFF
            //List<string> lvls = Level.GetLevels(SharedData.pathToAI, true);
            //foreach (string lvl in lvls)
            //{
            //    if (Directory.Exists(SharedData.pathToAI + "\\DATA_orig"))
            //    {
            //        Directory.Delete(SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\" + lvl, true);
            //        CopyFilesRecursively(SharedData.pathToAI + "\\DATA_orig\\ENV\\PRODUCTION\\" + lvl, SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\" + lvl);
            //    }
            //
            //    InstancedResolver.Read(LevelContent.DEBUG_LoadUnthreadedAndPopulateShortGuids(lvl));
            //}


            level = "TECH_RND_HZDLAB";

            if (Directory.Exists(SharedData.pathToAI + "\\DATA_orig"))
            {
                Directory.Delete(SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\" + level, true);
                CopyFilesRecursively(SharedData.pathToAI + "\\DATA_orig\\ENV\\PRODUCTION\\" + level, SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\" + level);
            }


            //Level is populated by MVR
            //MVR looks up the resource in COMMANDS via RESOURCES
            //MVR properties are applied to renderables in COMMANDS via this link (applied properties are specified by the type enum, e.g. not all provide position etc)
            //If no link has been made, both populate


            //It gets more complicated: RESOURCES.BIN is populated based on the result of COMMANDS (e.g. if an entity has "deleted" applied, it won't be listed there)
            //No fucking clue how I'm gonna be able to rewrite the thing without building the entire scripting logic into the compiler -_-


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
                for (int i = 0; i < content.commands.Entries[x].functions.Count; i++)
                {
                    for (int z = 0; z < content.commands.Entries[x].functions[i].resources.Count; z++)
                    {
                        if (content.commands.Entries[x].functions[i].resources[z].resource_type != ResourceType.COLLISION_MAPPING)
                            continue;
                        if (content.commands.Entries[x].functions[i].resources[z].index != -1 && min_collision_index > content.commands.Entries[x].functions[i].resources[z].index)
                            min_collision_index = content.commands.Entries[x].functions[i].resources[z].index;
                    }

                    Parameter resourceParam = content.commands.Entries[x].functions[i].GetParameter("resource");
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
                newPresence.Details = "Level: " + _commandsDisplay?.Content?.level;
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
            DEBUG_RunChecks.Visible = false;
            DEBUG_LaunchGame.Visible = false;
            buildLevelToolStripMenuItem.Visible = false;
#endif

            WindowState = SettingsManager.GetString(Singleton.Settings.WindowState, "Normal") == "Maximized" ? FormWindowState.Maximized : FormWindowState.Normal;
            Width = SettingsManager.GetInteger(Singleton.Settings.WindowWidth, _defaultWidth);
            Height = SettingsManager.GetInteger(Singleton.Settings.WindowHeight, _defaultHeight);
            Resize += CommandsEditor_Resize;
            FormClosing += CommandsEditor_FormClosing;

            if (!SettingsManager.IsSet(Singleton.Settings.ServerOpt)) SettingsManager.SetBool(Singleton.Settings.ServerOpt, true);
            connectToUnity.Checked = !SettingsManager.GetBool(Singleton.Settings.ServerOpt); connectToUnity.PerformClick();
            focusOnSelectedToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.UNITY_FocusEntity); focusOnSelectedToolStripMenuItem.PerformClick();
            ShowLevelViewerButton();

            showEntityIDs.Checked = !SettingsManager.GetBool(Singleton.Settings.EntIdOpt); showEntityIDs.PerformClick();
            searchOnlyCompositeNames.Checked = !SettingsManager.GetBool(Singleton.Settings.CompNameOnlyOpt); searchOnlyCompositeNames.PerformClick();
            useTexturedModelViewExperimentalToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ShowTexOpt); useTexturedModelViewExperimentalToolStripMenuItem.PerformClick();
            keepFunctionUsesWindowOpenToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.KeepUsesWindowOpen); keepFunctionUsesWindowOpenToolStripMenuItem.PerformClick();
            writeInstancedResourcesExperimentalToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.ExperimentalResourceStuff); writeInstancedResourcesExperimentalToolStripMenuItem.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.OpenEntityFromNode)) SettingsManager.SetBool(Singleton.Settings.OpenEntityFromNode, true);
            nodeOpensEntity.Checked = !SettingsManager.GetBool(Singleton.Settings.OpenEntityFromNode); nodeOpensEntity.PerformClick();

            if (!SettingsManager.IsSet(Singleton.Settings.MakeNodeWhenMakeEntity)) SettingsManager.SetBool(Singleton.Settings.MakeNodeWhenMakeEntity, true);
            createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.Checked = !SettingsManager.GetBool(Singleton.Settings.MakeNodeWhenMakeEntity); createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.PerformClick();

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
                this.Text = _baseTitle;
            else
                this.Text = _baseTitle + " - " + _commandsDisplay.Content.level;

            if (DirtyTracker.IsDirty)
                this.Text += " [UNSAVED CHANGES]";
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
                Directory.Delete(SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\" + level, true);
                CopyFilesRecursively(SharedData.pathToAI + "\\DATA_orig\\ENV\\PRODUCTION\\" + level, SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\" + level);
            }
#endif

            statusText.Text = "Loading " + level + "...";
            statusStrip.Update();

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

            //Sometimes get an error here which appears to be thread related (?) -> investigate next time
            _levelMenuItems[_commandsDisplay.Content.level].Checked = true;

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
                //...
            }


#if DEBUG
            DebugSaveFunc();
#endif


            if (_commandsDisplay.Content.resource.lights != null)
                _commandsDisplay.Content.resource.lights.Save();
            if (_commandsDisplay.Content.resource.env_maps != null && _commandsDisplay.Content.resource.env_maps.Entries != null)
                _commandsDisplay.Content.resource.env_maps.Save();
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
            if (openLevelViewerToolStripMenuItem.GetCurrentParent().InvokeRequired)
            {
                openLevelViewerToolStripMenuItem.GetCurrentParent().Invoke(new Action(() =>
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
            SettingsManager.SetBool(Singleton.Settings.EntIdOpt, showEntityIDs.Checked);

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

        private void nodeOpensEntity_Click(object sender, EventArgs e)
        {
            nodeOpensEntity.Checked = !nodeOpensEntity.Checked;
            SettingsManager.SetBool(Singleton.Settings.OpenEntityFromNode, nodeOpensEntity.Checked);
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

        public void ToggleMakeNodeWhenMakeEntity()
        {
            createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.PerformClick();
        }
        private void createFlowgraphNodeWhenEntityCreatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.Checked = !createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.Checked;
            SettingsManager.SetBool(Singleton.Settings.MakeNodeWhenMakeEntity, createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.Checked);
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

        private void DEBUG_DoorPhysEnt_Click(object sender, EventArgs e)
        {
            _commandsDisplay.LoadComposite(new ShortGuid("30-2E-B7-25"));
            _commandsDisplay.CompositeDisplay.Composite.GetEntityByID(new ShortGuid("88-2E-34-D5")).AddParameter("position", new cTransform(new Vector3(-0.4999240f, 0.0003948f, -40.0000000f), new Vector3(0,0,0)));

            _commandsDisplay.LoadComposite(new ShortGuid("7A-40-D8-07"));
            _commandsDisplay.CompositeDisplay.Composite.GetEntityByID(new ShortGuid("A4-94-3A-1F")).AddParameter("Animation", new cString(""));
            _commandsDisplay.CompositeDisplay.DeleteEntity(_commandsDisplay.CompositeDisplay.Composite.GetEntityByID(new ShortGuid("62-05-5E-F3")), false);
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
            Process.Start(SharedData.pathToAI + "\\AI.exe");
        }

        List<CollisionMaps.Entry> entries = new List<CollisionMaps.Entry>();
        private void SaveCollisionMaps(Composite composite)
        {
            if (composite == null)
                return;

            for (int i = 0; i < composite.functions.Count; i++)
            {
                if (CommandsUtils.FunctionTypeExists(composite.functions[i].function))
                {
                    //TODO: this assumes that entities don't have collision mappings in the resoure parameter and entity resources
                    ResourceReference collision = composite.functions[i].GetResource(ResourceType.COLLISION_MAPPING, true);
                    if (collision == null)
                        continue;

                    //TODO: need to make sure "collision_index" is the same for every entry with the same entity, then we can show it

                    CollisionMaps.Entry collisionMap = new CollisionMaps.Entry()
                    {
                        id = collision.resource_id,
                        collision_index = collision.index,
                        entity = new EntityHandle()
                        {
                            entity_id = composite.functions[i].shortGUID
                            //TODO: generate composite guid
                        }
                    };

                    //collision.resource_id

                    //switch (CommandsUtils.GetFunctionType(composite.functions[i].function))
                    //{
                    //    case FunctionType.
                    //}
                }
                else
                {
                    SaveCollisionMaps(_commandsDisplay.Content.commands.GetComposite(composite.functions[i].function));
                }
            }
        }

        private void DEBUG_RunChecks_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Singleton.Editor.CommandsDisplay.Content.commands.Entries.Count; i++)
            {
                CommandsUtils.PurgeDeadLinks(Singleton.Editor.CommandsDisplay.Content.commands, Singleton.Editor.CommandsDisplay.Content.commands.Entries[i], true);
            }
            for (int i = 0; i < Singleton.Editor.CommandsDisplay.Content.commands.Entries.Count; i++)
            {
                CommandsUtils.PurgeDeadLinks(Singleton.Editor.CommandsDisplay.Content.commands, Singleton.Editor.CommandsDisplay.Content.commands.Entries[i], true);
            }
            for (int i = 0; i < Singleton.Editor.CommandsDisplay.Content.commands.Entries.Count; i++)
            {
                CommandsUtils.PurgeDeadLinks(Singleton.Editor.CommandsDisplay.Content.commands, Singleton.Editor.CommandsDisplay.Content.commands.Entries[i], true);
            }

            return;
            Flowgraph window = new Flowgraph();
            window.Show();
            //window.DEBUG_LoadAll_Test(CommandsDisplay.Content.commands);
            window.Close();


            return;

            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff(_commandsDisplay.Content.level, "FINAL_ORDERED");
            return;

            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("DLC/SALVAGEMODE2", "FINAL");
            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_COMMS", "FINAL");
            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_HUB", "FINAL");
            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_MUTHRCORE", "FINAL");
            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_RND", "FINAL");
            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_RND_HZDLAB", "FINAL");
            return;

            List<string> levels = Level.GetLevels(SharedData.pathToAI, true);
            //levels.Clear();
            //levels.Add("DLC/CHALLENGEMAP3");
            foreach (string level in levels)
            {
                Console.WriteLine("LOADING: " + level);

                LevelContent content = LevelContent.DEBUG_LoadUnthreadedAndPopulateShortGuids(level);

                Dictionary<ShortGuid, Tuple<List<int>, List<CollisionMaps.Entry>>> collisionmapindexes = new Dictionary<ShortGuid, Tuple<List<int>, List<CollisionMaps.Entry>>>();
                foreach (var collisionmap in content.resource.collision_maps.Entries)
                {
                    if (!collisionmapindexes.ContainsKey(collisionmap.entity.entity_id))
                        collisionmapindexes.Add(collisionmap.entity.entity_id, new Tuple<List<int>, List<CollisionMaps.Entry>>(new List<int>(), new List<CollisionMaps.Entry>()));

                    if (!collisionmapindexes[collisionmap.entity.entity_id].Item1.Contains(collisionmap.collision_index))
                    {
                        collisionmapindexes[collisionmap.entity.entity_id].Item1.Add(collisionmap.collision_index);
                        collisionmapindexes[collisionmap.entity.entity_id].Item2.Add(collisionmap);
                    }
                }

                foreach (KeyValuePair<ShortGuid, Tuple<List<int>, List<CollisionMaps.Entry>>> indexes in collisionmapindexes)
                {
                    if (indexes.Value.Item1.Count != 1)
                    {
                        Console.WriteLine("---");
                        Console.WriteLine(indexes.Value.Item1.Count + " -> " + indexes.Value.Item1.Contains(-1));

                        foreach (var item in indexes.Value.Item2)
                        {
                            (Composite entComp, EntityPath entPath) = content.editor_utils.GetCompositeFromInstanceID(content.commands, item.entity.composite_instance_id);
                            Entity entEnt = entComp?.GetEntityByID(item.entity.entity_id);
                            (Composite zoneComp1, EntityPath zonePath1, Entity zoneEnt1) = content.editor_utils.GetZoneFromInstanceID(content.commands, item.zone_id);

                            string convertedResoureName = "[" + content.resource.collision_maps.Entries.IndexOf(item) + "] " + item.id.ToString() + " -> " + item.id.ToByteString() + " [" + item.collision_index + "]";

                            foreach (Composite comp in content.commands.Entries)
                            {
                                Entity ent = comp.GetEntityByID(item.entity.entity_id);
                                if (ent != null)
                                {
                                    convertedResoureName += "\n\t Entity LOOKUP found in " + comp.name + " [" + comp.shortGUID.ToByteString() + "] -> " + ShortGuidUtils.Generate(EntityUtils.GetName(comp, ent) + " [" + ent.shortGUID.ToByteString() + "]");
                                }
                            }
                            convertedResoureName += "\n\t Entity INSTANCEID: " + item.entity.composite_instance_id.ToByteString();

                            if (entComp != null)
                                convertedResoureName += "\n\t Entity Composite: " + entComp.name;
                            if (entPath != null)
                                convertedResoureName += "\n\t Entity Instance: " + entPath.GetAsString(content.commands, content.commands.EntryPoints[0], true);
                            if (entEnt != null && entComp == null)
                                convertedResoureName += "\n\t Entity Entity: " + entEnt.shortGUID + " -> can't resolve name";
                            if (entEnt != null && entComp != null)
                                convertedResoureName += "\n\t Entity Entity: " + entEnt.shortGUID + " -> " + EntityUtils.GetName(entComp, entEnt);

                            if (zonePath1 != null && zonePath1.path.Length == 2 && zonePath1.path[0] == new ShortGuid("01-00-00-00"))
                            {
                                convertedResoureName += "\n\t Primary Zone: GLOBAL ZONE";
                            }
                            else if (zonePath1 != null && zonePath1.path.Length == 1 && zonePath1.path[0] == new ShortGuid("00-00-00-00"))
                            {
                                convertedResoureName += "\n\t Primary Zone: ZERO ZONE";
                            }
                            else
                            {
                                convertedResoureName += "\n\t Primary Zone: " + item.zone_id.ToByteString() + " -> " + item.zone_id.ToString();
                                if (zoneComp1 != null)
                                    convertedResoureName += "\n\t Primary Zone Composite: " + zoneComp1.name;
                                if (zonePath1 != null)
                                    convertedResoureName += "\n\t Primary Zone Instance: " + zonePath1.GetAsString(content.commands, content.commands.EntryPoints[0], true);
                                if (zoneEnt1 != null && zoneComp1 == null)
                                    convertedResoureName += "\n\t Primary Zone Entity: " + zoneEnt1.shortGUID + " -> can't resolve name";
                                if (zoneEnt1 != null && zoneComp1 != null)
                                    convertedResoureName += "\n\t Primary Zone Entity: " + zoneEnt1.shortGUID + " -> " + EntityUtils.GetName(zoneComp1, zoneEnt1);
                            }


                            Console.WriteLine(convertedResoureName);
                            continue;
                        }
                        
                        
                       // string fscfsdf = "";
                    }
                }
            }



            return;

            SaveCollisionMaps(_commandsDisplay.Content.commands.EntryPoints[0]);



            return;

            ShortGuid aa = ShortGuidUtils.Generate("AnimatedModel");
            ShortGuid bb = ShortGuidUtils.Generate("DYNAMIC_PHYSICS_SYSTEM");
            ShortGuid cc = ShortGuidUtils.Generate("resource");


            //tex names?


            /*
            var ccc = _commandsDisplay.Content.commands.Entries.FirstOrDefault(o => o.name == "AYZ\\FX_LIBRARY\\WALL_DECALS\\HOSPITAL\\FX_WALL_DECAL_CORNER_INT_TYPE1");
            var cccc = ccc.functions.FirstOrDefault(o => o.shortGUID == new ShortGuid("BB-DA-BE-62")); //new ShortGuid("5E-0C-AC-7A")
            List<EntityPath> pathscccc = _commandsDisplay.Content.editor_utils.GetHierarchiesForEntity(ccc, cccc);
            var fgdfg = pathscccc[0].GenerateInstance();
            var fgdf1g = pathscccc[0].GeneratePathHash();




            foreach (var entry in _commandsDisplay.Content.resource.resources.Entries)
            {
                (Composite comp, EntityPath path) = _commandsDisplay.Content.editor_utils.GetCompositeFromInstanceID(_commandsDisplay.Content.commands, entry.composite_instance_id);

                if (comp == null && path == null)
                {
                    List<Tuple<Entity, Composite>> foundEntities = new List<Tuple<Entity, Composite>>();
                    foreach (Composite comp2 in _commandsDisplay.Content.commands.Entries)
                    {
                        Entity ent2 = comp2.GetEntityByID(entry.resource_id);
                        if (ent2 == null) continue;
                        foundEntities.Add(new Tuple<Entity, Composite>(ent2, comp2));
                    }


                    if (foundEntities.Count == 1)
                    {
                        List<EntityPath> paths = _commandsDisplay.Content.editor_utils.GetHierarchiesForEntity(foundEntities[0].Item2, foundEntities[0].Item1);
                        if (paths.Count == 1)
                        {
                            List<Tuple<Entity, Composite, ResourceReference>> foundResources = new List<Tuple<Entity, Composite, ResourceReference>>();
                            foreach (Composite comp2 in _commandsDisplay.Content.commands.Entries)
                            {
                                foreach (FunctionEntity funcEnt in comp2.functions)
                                {
                                    List<ResourceReference> resRefs = funcEnt.resources;
                                    Parameter resParam = funcEnt.GetParameter("resource");
                                    if (resParam != null && resParam.content != null && resParam.content.dataType == DataType.RESOURCE)
                                    {
                                        resRefs.AddRange(((cResource)resParam.content).value);
                                    }

                                    foreach (ResourceReference resRef in resRefs)
                                    {
                                        if (resRef.resource_id == entry.resource_id)
                                        {
                                            foundResources.Add(new Tuple<Entity, Composite, ResourceReference>(funcEnt, comp2, resRef));
                                        }
                                    }
                                }
                            }

                            string breakhere = "";
                        }
                    }
                }
            }



            var test1 = _commandsDisplay.Content.mvr.Entries.FirstOrDefault(o => o.entity.composite_instance_id == new ShortGuid("EE-61-70-F2"));

            for (int i = 0; i < test1.renderable_element_count; i++) 
            {
                var test5 = _commandsDisplay.Content.resource.reds.Entries[(int)test1.renderable_element_index + i];

                var test6 = _commandsDisplay.Content.resource.models.GetAtWriteIndex(test5.ModelIndex);
                var test61 = _commandsDisplay.Content.resource.models.FindModelForSubmesh(test6);
                var test62 = _commandsDisplay.Content.resource.models.FindModelComponentForSubmesh(test6);
                var test63 = _commandsDisplay.Content.resource.models.FindModelLODForSubmesh(test6);
                var test7 = _commandsDisplay.Content.resource.materials.GetAtWriteIndex(test5.MaterialIndex);

                string sdfsdf = "";
            }

            var test2 = _commandsDisplay.Content.resource.collision_maps.Entries.FirstOrDefault(o => o.entity.composite_instance_id == new ShortGuid("EE-61-70-F2"));
            var test3 = _commandsDisplay.Content.resource.resources.Entries.FirstOrDefault(o => o.composite_instance_id == new ShortGuid("EE-61-70-F2"));

            return;
            */

            //LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("BSP_LV426_PT01");
            //return;

            //LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("SOLACE");
            //return;

            //try
            //{
            //    SharedData.pathToAI = "F:\\Alien Isolation Versions\\Alien Isolation PC DVD\\Converted\\214491";
            //    List<string> levels = Level.GetLevels(SharedData.pathToAI, true);
            //    foreach (string level in levels)
            //    {
            //        try
            //        {
            //            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff(level, "PRELOAD");
            //        }
            //        catch { }
            //    }
            //}
            //catch { }

            SharedData.pathToAI = "F:\\Alien Isolation Versions\\Alien Isolation PC Final";
            Parallel.For(0, 2, i =>
            {
                switch (i)
                {
                    case 0:
                        try
                        {
                            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("DLC/CHALLENGEMAP11", "FINAL");
                        }
                        catch { }
                        break;
                    case 1:
                        try
                        {
                            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("DLC/CHALLENGEMAP14", "FINAL");
                        }
                        catch { }
                        break;
                }
            });
            Parallel.For(0, 2, i =>
            {
                switch (i)
                {
                    case 0:
                        try
                        {
                            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("DLC/SALVAGEMODE2", "FINAL");
                        }
                        catch { }
                        break;
                    case 1:
                        try
                        {
                            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_COMMS", "FINAL");
                        }
                        catch { }
                        break;
                }
            });
            Parallel.For(0, 2, i =>
            {
                switch (i)
                {
                    case 0:
                        try
                        {
                            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_HUB", "FINAL");
                        }
                        catch { }
                        break;
                    case 1:
                        try
                        {
                            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_MUTHRCORE", "FINAL");
                        }
                        catch { }
                        break;
                }
            });
            Parallel.For(0, 2, i =>
            {
                switch (i)
                {
                    case 0:
                        try
                        {
                            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_RND", "FINAL");
                        }
                        catch { }
                        break;
                    case 1:
                        try
                        {
                            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff("TECH_RND_HZDLAB", "FINAL");
                        }
                        catch { }
                        break;
                }
            });


            return;

            //try
            //{
            //    //SharedData.pathToAI = "F:\\Alien Isolation Versions\\Alien Isolation PC Final";
            //    List<string> levels = Level.GetLevels(SharedData.pathToAI, true);
            //    foreach (string level in levels)
            //    {
            //        Directory.Delete("E:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\" + level, true);
            //        CopyFilesRecursively("F:\\Alien Isolation Versions\\Alien Isolation PC Final\\DATA\\ENV\\PRODUCTION\\" + level, "E:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\" + level);
            //
            //        try
            //        {
            //            LocalDebug_NEW.DEBUG_DumpAllInstancedStuff(level, "FINAL");
            //        }
            //        catch { }
            //    }
            //}
            //catch { }
            //return;


            //InstanceWriter test = new InstanceWriter();
            //test.WriteInstances(_commandsDisplay.Content);

            //LocalDebug.checkprefabinstances();
            return;

            List<EntityPath> zoneHandles = _commandsDisplay.Content.editor_utils.GetHierarchiesForEntity(_commandsDisplay.CompositeDisplay.Composite, _commandsDisplay.CompositeDisplay.EntityDisplay.Entity);
            for (int i = 0; i < zoneHandles.Count; i++)
            {
                ShortGuid zoneID = zoneHandles[i].GenerateZoneID();
                ShortGuid instanceID = zoneHandles[i].GenerateCompositeInstanceID();

                string sdfsdf = "";
            }


            //LocalDebug.checkphysicssystempositions();
        }
        

        void DebugSaveFunc()
        {
            return;

            _commandsDisplay.Content.resource.lights.Indexes.Clear();
            _commandsDisplay.Content.resource.lights.Values.Clear();
            for (int i = 0; i < _commandsDisplay.Content.mvr.Entries.Count; i++)
            {
                _commandsDisplay.Content.mvr.Entries[i].environment_map_index = -1;
                //_commandsDisplay.Content.mvr.Entries[i].resource_index = -1;
            }
            _commandsDisplay.Content.resource.env_maps.Entries.Clear();

            //_commandsDisplay.Content.resource.collision_maps.Entries.Clear();
            //_commandsDisplay.Content.resource.physics_maps.Entries.Clear();
            //_commandsDisplay.Content.resource.resources.Entries.Clear();

            //note - can remove 130 entries from the end before it crashes on SOLACE
            //note - can remove 11 entries from the end before it crashes on BSP_TORRENS
            int removeCount = 11;
            _commandsDisplay.Content.mvr.Entries.RemoveRange(_commandsDisplay.Content.mvr.Entries.Count - removeCount, removeCount);

            //note - i can do this on BSP_LV426_PT01 - why doesn't it work on solace?
            //_commandsDisplay.Content.mvr.Entries.Clear();
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

        private void DEBUG_LaunchGame_Click(object sender, EventArgs e)
        {
            EditorUtils.PatchLaunchMode(_commandsDisplay.Content.level);
            Process.Start(SettingsManager.GetString("PATH_GameRoot") + "AI.exe");
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
    }
}
