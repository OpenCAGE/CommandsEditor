namespace CommandsEditor
{
    partial class CommandsEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandsEditor));
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.loadLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.buildLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.levelViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLevelViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.connectToUnity = new System.Windows.Forms.ToolStripMenuItem();
            this.focusOnSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.compositeViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showExplorerViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoHideExplorerViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchOnlyCompositeNames = new System.Windows.Forms.ToolStripMenuItem();
            this.entityDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showEntityIDs = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeOpensEntity = new System.Windows.Forms.ToolStripMenuItem();
            this.createFlowgraphNodeWhenEntityCreatedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miscToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showConfirmationWhenSavingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useTexturedModelViewExperimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepFunctionUsesWindowOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetUILayoutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeInstancedResourcesExperimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setNumericStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpBtn = new System.Windows.Forms.ToolStripButton();
            this.ShowControls = new System.Windows.Forms.ToolStripButton();
            this.DEBUG_RunChecks = new System.Windows.Forms.ToolStripButton();
            this.DEBUG_DoorPhysEnt = new System.Windows.Forms.ToolStripButton();
            this.DEBUG_LaunchGame = new System.Windows.Forms.ToolStripButton();
            this.vS2015BlueTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.savePAKAndBINToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.helpBtn,
            this.ShowControls,
            this.DEBUG_RunChecks,
            this.DEBUG_DoorPhysEnt,
            this.DEBUG_LaunchGame});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1581, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadLevel,
            this.saveLevel,
            this.buildLevelToolStripMenuItem});
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripButton1.Text = "File";
            // 
            // loadLevel
            // 
            this.loadLevel.Name = "loadLevel";
            this.loadLevel.Size = new System.Drawing.Size(180, 22);
            this.loadLevel.Text = "Load Level";
            // 
            // saveLevel
            // 
            this.saveLevel.Name = "saveLevel";
            this.saveLevel.ShortcutKeyDisplayString = "Ctrl+S";
            this.saveLevel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveLevel.Size = new System.Drawing.Size(180, 22);
            this.saveLevel.Text = "Save Level";
            this.saveLevel.Click += new System.EventHandler(this.saveLevel_Click);
            // 
            // buildLevelToolStripMenuItem
            // 
            this.buildLevelToolStripMenuItem.Name = "buildLevelToolStripMenuItem";
            this.buildLevelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.buildLevelToolStripMenuItem.Text = "Build Level";
            this.buildLevelToolStripMenuItem.Click += new System.EventHandler(this.buildLevelToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.levelViewerToolStripMenuItem,
            this.toolStripSeparator2,
            this.compositeViewerToolStripMenuItem,
            this.entityDisplayToolStripMenuItem,
            this.miscToolStripMenuItem});
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(62, 22);
            this.toolStripButton2.Text = "Options";
            // 
            // levelViewerToolStripMenuItem
            // 
            this.levelViewerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setUpToolStripMenuItem,
            this.openLevelViewerToolStripMenuItem,
            this.toolStripSeparator1,
            this.connectToUnity,
            this.focusOnSelectedToolStripMenuItem});
            this.levelViewerToolStripMenuItem.Name = "levelViewerToolStripMenuItem";
            this.levelViewerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.levelViewerToolStripMenuItem.Text = "Level Viewer";
            // 
            // setUpToolStripMenuItem
            // 
            this.setUpToolStripMenuItem.Name = "setUpToolStripMenuItem";
            this.setUpToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.setUpToolStripMenuItem.Text = "Set Up Level Viewer";
            this.setUpToolStripMenuItem.Click += new System.EventHandler(this.setUpToolStripMenuItem_Click);
            // 
            // openLevelViewerToolStripMenuItem
            // 
            this.openLevelViewerToolStripMenuItem.Name = "openLevelViewerToolStripMenuItem";
            this.openLevelViewerToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.openLevelViewerToolStripMenuItem.Text = "Open Level Viewer";
            this.openLevelViewerToolStripMenuItem.Click += new System.EventHandler(this.openLevelViewerToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // connectToUnity
            // 
            this.connectToUnity.Checked = true;
            this.connectToUnity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.connectToUnity.Name = "connectToUnity";
            this.connectToUnity.Size = new System.Drawing.Size(201, 22);
            this.connectToUnity.Text = "Connect to Level Viewer";
            this.connectToUnity.ToolTipText = "Enable a websocket connection to the Unity Level Viewer.";
            this.connectToUnity.Click += new System.EventHandler(this.connectToUnity_Click);
            // 
            // focusOnSelectedToolStripMenuItem
            // 
            this.focusOnSelectedToolStripMenuItem.Name = "focusOnSelectedToolStripMenuItem";
            this.focusOnSelectedToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.focusOnSelectedToolStripMenuItem.Text = "Focus on Selected";
            this.focusOnSelectedToolStripMenuItem.ToolTipText = "Enable to focus the Unity camera on the object selected in the Commands Editor au" +
    "tomatically.";
            this.focusOnSelectedToolStripMenuItem.Click += new System.EventHandler(this.focusOnSelectedToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // compositeViewerToolStripMenuItem
            // 
            this.compositeViewerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showExplorerViewToolStripMenuItem,
            this.autoHideExplorerViewToolStripMenuItem,
            this.searchOnlyCompositeNames});
            this.compositeViewerToolStripMenuItem.Name = "compositeViewerToolStripMenuItem";
            this.compositeViewerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.compositeViewerToolStripMenuItem.Text = "Composite Display";
            // 
            // showExplorerViewToolStripMenuItem
            // 
            this.showExplorerViewToolStripMenuItem.Name = "showExplorerViewToolStripMenuItem";
            this.showExplorerViewToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.showExplorerViewToolStripMenuItem.Text = "Use File Browser Composite Viewer";
            this.showExplorerViewToolStripMenuItem.ToolTipText = "If enabled, the composite viewer will display a file browser style UI and dock to" +
    " the bottom of the window.";
            this.showExplorerViewToolStripMenuItem.Click += new System.EventHandler(this.showExplorerViewToolStripMenuItem_Click);
            // 
            // autoHideExplorerViewToolStripMenuItem
            // 
            this.autoHideExplorerViewToolStripMenuItem.Name = "autoHideExplorerViewToolStripMenuItem";
            this.autoHideExplorerViewToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.autoHideExplorerViewToolStripMenuItem.Text = "Auto Hide Composite Viewer";
            this.autoHideExplorerViewToolStripMenuItem.ToolTipText = "If enabled, the composite viewer will automatically hide when not interacted with" +
    ".";
            this.autoHideExplorerViewToolStripMenuItem.Click += new System.EventHandler(this.autoHideExplorerViewToolStripMenuItem_Click);
            // 
            // searchOnlyCompositeNames
            // 
            this.searchOnlyCompositeNames.Name = "searchOnlyCompositeNames";
            this.searchOnlyCompositeNames.Size = new System.Drawing.Size(258, 22);
            this.searchOnlyCompositeNames.Text = "Search Only Composite Names";
            this.searchOnlyCompositeNames.ToolTipText = "Enable this option to exclude folder names from the composite search.";
            this.searchOnlyCompositeNames.Click += new System.EventHandler(this.searchOnlyCompositeNames_Click);
            // 
            // entityDisplayToolStripMenuItem
            // 
            this.entityDisplayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showEntityIDs,
            this.nodeOpensEntity,
            this.createFlowgraphNodeWhenEntityCreatedToolStripMenuItem});
            this.entityDisplayToolStripMenuItem.Name = "entityDisplayToolStripMenuItem";
            this.entityDisplayToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.entityDisplayToolStripMenuItem.Text = "Entity Display";
            // 
            // showEntityIDs
            // 
            this.showEntityIDs.Name = "showEntityIDs";
            this.showEntityIDs.Size = new System.Drawing.Size(310, 22);
            this.showEntityIDs.Text = "Show Entity IDs";
            this.showEntityIDs.ToolTipText = "Show entity IDs within the editor UI.";
            this.showEntityIDs.Click += new System.EventHandler(this.showEntityIDs_Click);
            // 
            // nodeOpensEntity
            // 
            this.nodeOpensEntity.Name = "nodeOpensEntity";
            this.nodeOpensEntity.Size = new System.Drawing.Size(310, 22);
            this.nodeOpensEntity.Text = "Open Entity When Flowgraph Node Selected";
            this.nodeOpensEntity.Click += new System.EventHandler(this.nodeOpensEntity_Click);
            // 
            // createFlowgraphNodeWhenEntityCreatedToolStripMenuItem
            // 
            this.createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.Name = "createFlowgraphNodeWhenEntityCreatedToolStripMenuItem";
            this.createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.Size = new System.Drawing.Size(310, 22);
            this.createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.Text = "Create Flowgraph Node When Entity Created";
            this.createFlowgraphNodeWhenEntityCreatedToolStripMenuItem.Click += new System.EventHandler(this.createFlowgraphNodeWhenEntityCreatedToolStripMenuItem_Click);
            // 
            // miscToolStripMenuItem
            // 
            this.miscToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showConfirmationWhenSavingToolStripMenuItem,
            this.useTexturedModelViewExperimentalToolStripMenuItem,
            this.keepFunctionUsesWindowOpenToolStripMenuItem,
            this.resetUILayoutsToolStripMenuItem,
            this.writeInstancedResourcesExperimentalToolStripMenuItem,
            this.setNumericStepToolStripMenuItem,
            this.savePAKAndBINToolStripMenuItem});
            this.miscToolStripMenuItem.Name = "miscToolStripMenuItem";
            this.miscToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.miscToolStripMenuItem.Text = "Misc";
            // 
            // showConfirmationWhenSavingToolStripMenuItem
            // 
            this.showConfirmationWhenSavingToolStripMenuItem.Name = "showConfirmationWhenSavingToolStripMenuItem";
            this.showConfirmationWhenSavingToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.showConfirmationWhenSavingToolStripMenuItem.Text = "Show Confirmation When Saving";
            this.showConfirmationWhenSavingToolStripMenuItem.ToolTipText = "If enabled, a confirmation will show after a successful save.";
            this.showConfirmationWhenSavingToolStripMenuItem.Click += new System.EventHandler(this.showConfirmationWhenSavingToolStripMenuItem_Click);
            // 
            // useTexturedModelViewExperimentalToolStripMenuItem
            // 
            this.useTexturedModelViewExperimentalToolStripMenuItem.Name = "useTexturedModelViewExperimentalToolStripMenuItem";
            this.useTexturedModelViewExperimentalToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.useTexturedModelViewExperimentalToolStripMenuItem.Text = "Use Textured Model View (Experimental)";
            this.useTexturedModelViewExperimentalToolStripMenuItem.ToolTipText = "If enabled, the model previewer will try and find textures to render.";
            this.useTexturedModelViewExperimentalToolStripMenuItem.Click += new System.EventHandler(this.useTexturedModelViewExperimentalToolStripMenuItem_Click);
            // 
            // keepFunctionUsesWindowOpenToolStripMenuItem
            // 
            this.keepFunctionUsesWindowOpenToolStripMenuItem.Name = "keepFunctionUsesWindowOpenToolStripMenuItem";
            this.keepFunctionUsesWindowOpenToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.keepFunctionUsesWindowOpenToolStripMenuItem.Text = "Keep Function Uses Window Open";
            this.keepFunctionUsesWindowOpenToolStripMenuItem.Click += new System.EventHandler(this.keepFunctionUsesWindowOpenToolStripMenuItem_Click);
            // 
            // resetUILayoutsToolStripMenuItem
            // 
            this.resetUILayoutsToolStripMenuItem.Name = "resetUILayoutsToolStripMenuItem";
            this.resetUILayoutsToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.resetUILayoutsToolStripMenuItem.Text = "Reset UI Layouts";
            this.resetUILayoutsToolStripMenuItem.Click += new System.EventHandler(this.resetUILayoutsToolStripMenuItem_Click);
            // 
            // writeInstancedResourcesExperimentalToolStripMenuItem
            // 
            this.writeInstancedResourcesExperimentalToolStripMenuItem.Name = "writeInstancedResourcesExperimentalToolStripMenuItem";
            this.writeInstancedResourcesExperimentalToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.writeInstancedResourcesExperimentalToolStripMenuItem.Text = "Write Instanced Resources (Experimental)";
            this.writeInstancedResourcesExperimentalToolStripMenuItem.Click += new System.EventHandler(this.writeInstancedResourcesExperimentalToolStripMenuItem_Click);
            // 
            // setNumericStepToolStripMenuItem
            // 
            this.setNumericStepToolStripMenuItem.Name = "setNumericStepToolStripMenuItem";
            this.setNumericStepToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.setNumericStepToolStripMenuItem.Text = "Set Numeric Step";
            this.setNumericStepToolStripMenuItem.Click += new System.EventHandler(this.setNumericStepToolStripMenuItem_Click);
            // 
            // helpBtn
            // 
            this.helpBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpBtn.Image = ((System.Drawing.Image)(resources.GetObject("helpBtn.Image")));
            this.helpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(23, 22);
            this.helpBtn.Text = "Help";
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // ShowControls
            // 
            this.ShowControls.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ShowControls.Image = ((System.Drawing.Image)(resources.GetObject("ShowControls.Image")));
            this.ShowControls.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShowControls.Name = "ShowControls";
            this.ShowControls.Size = new System.Drawing.Size(56, 22);
            this.ShowControls.Text = "Controls";
            this.ShowControls.Click += new System.EventHandler(this.ShowControls_Click);
            // 
            // DEBUG_RunChecks
            // 
            this.DEBUG_RunChecks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DEBUG_RunChecks.Image = ((System.Drawing.Image)(resources.GetObject("DEBUG_RunChecks.Image")));
            this.DEBUG_RunChecks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DEBUG_RunChecks.Name = "DEBUG_RunChecks";
            this.DEBUG_RunChecks.Size = new System.Drawing.Size(146, 22);
            this.DEBUG_RunChecks.Text = "DEBUG: Check Flowgraph";
            this.DEBUG_RunChecks.Click += new System.EventHandler(this.DEBUG_RunChecks_Click);
            // 
            // DEBUG_DoorPhysEnt
            // 
            this.DEBUG_DoorPhysEnt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DEBUG_DoorPhysEnt.Image = ((System.Drawing.Image)(resources.GetObject("DEBUG_DoorPhysEnt.Image")));
            this.DEBUG_DoorPhysEnt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DEBUG_DoorPhysEnt.Name = "DEBUG_DoorPhysEnt";
            this.DEBUG_DoorPhysEnt.Size = new System.Drawing.Size(157, 22);
            this.DEBUG_DoorPhysEnt.Text = "DEBUG: Load Door Phys Ent";
            this.DEBUG_DoorPhysEnt.Click += new System.EventHandler(this.DEBUG_DoorPhysEnt_Click);
            // 
            // DEBUG_LaunchGame
            // 
            this.DEBUG_LaunchGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DEBUG_LaunchGame.Image = ((System.Drawing.Image)(resources.GetObject("DEBUG_LaunchGame.Image")));
            this.DEBUG_LaunchGame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DEBUG_LaunchGame.Name = "DEBUG_LaunchGame";
            this.DEBUG_LaunchGame.Size = new System.Drawing.Size(127, 22);
            this.DEBUG_LaunchGame.Text = "DEBUG: Launch Game";
            this.DEBUG_LaunchGame.Click += new System.EventHandler(this.DEBUG_LaunchGame_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Black;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText});
            this.statusStrip.Location = new System.Drawing.Point(0, 782);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1581, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusText
            // 
            this.statusText.ForeColor = System.Drawing.SystemColors.Control;
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 17);
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
            this.dockPanel.DockBottomPortion = 0.35D;
            this.dockPanel.DocumentTabStripLocation = WeifenLuo.WinFormsUI.Docking.DocumentTabStripLocation.Hidden;
            this.dockPanel.Location = new System.Drawing.Point(0, 25);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Padding = new System.Windows.Forms.Padding(6);
            this.dockPanel.ShowAutoHideContentOnHover = false;
            this.dockPanel.Size = new System.Drawing.Size(1581, 757);
            this.dockPanel.TabIndex = 5;
            this.dockPanel.Theme = this.vS2015BlueTheme1;
            // 
            // savePAKAndBINToolStripMenuItem
            // 
            this.savePAKAndBINToolStripMenuItem.Name = "savePAKAndBINToolStripMenuItem";
            this.savePAKAndBINToolStripMenuItem.Size = new System.Drawing.Size(291, 22);
            this.savePAKAndBINToolStripMenuItem.Text = "Save Commands PAK and BIN";
            this.savePAKAndBINToolStripMenuItem.Click += new System.EventHandler(this.savePAKAndBINToolStripMenuItem_Click);
            // 
            // CommandsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1581, 804);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "CommandsEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenCAGE Commands Editor";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
        private WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme vS2015BlueTheme1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem loadLevel;
        private System.Windows.Forms.ToolStripMenuItem saveLevel;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton2;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton helpBtn;
        private System.Windows.Forms.ToolStripMenuItem compositeViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showExplorerViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoHideExplorerViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchOnlyCompositeNames;
        private System.Windows.Forms.ToolStripMenuItem entityDisplayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showEntityIDs;
        private System.Windows.Forms.ToolStripMenuItem miscToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showConfirmationWhenSavingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useTexturedModelViewExperimentalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepFunctionUsesWindowOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nodeOpensEntity;
        private System.Windows.Forms.ToolStripMenuItem resetUILayoutsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton DEBUG_DoorPhysEnt;
        private System.Windows.Forms.ToolStripButton DEBUG_RunChecks;
        private System.Windows.Forms.ToolStripMenuItem writeInstancedResourcesExperimentalToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton DEBUG_LaunchGame;
        private System.Windows.Forms.ToolStripMenuItem buildLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFlowgraphNodeWhenEntityCreatedToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ShowControls;
        private System.Windows.Forms.ToolStripMenuItem levelViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToUnity;
        private System.Windows.Forms.ToolStripMenuItem focusOnSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLevelViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem setNumericStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePAKAndBINToolStripMenuItem;
    }
}