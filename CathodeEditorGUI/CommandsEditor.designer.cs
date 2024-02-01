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
            this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.connectToUnity = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.compositeViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showExplorerViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoHideExplorerViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchOnlyCompositeNames = new System.Windows.Forms.ToolStripMenuItem();
            this.entityDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNodegraph = new System.Windows.Forms.ToolStripMenuItem();
            this.showEntityIDs = new System.Windows.Forms.ToolStripMenuItem();
            this.entitiesOpenTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.miscToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showConfirmationWhenSavingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useTexturedModelViewExperimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepFunctionUsesWindowOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpBtn = new System.Windows.Forms.ToolStripButton();
            this.vS2015BlueTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.helpBtn});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1257, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadLevel,
            this.saveLevel});
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripButton1.Text = "File";
            // 
            // loadLevel
            // 
            this.loadLevel.Name = "loadLevel";
            this.loadLevel.Size = new System.Drawing.Size(168, 22);
            this.loadLevel.Text = "Load Level";
            // 
            // saveLevel
            // 
            this.saveLevel.Name = "saveLevel";
            this.saveLevel.ShortcutKeyDisplayString = "Ctrl+S";
            this.saveLevel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveLevel.Size = new System.Drawing.Size(168, 22);
            this.saveLevel.Text = "Save Level";
            this.saveLevel.Click += new System.EventHandler(this.saveLevel_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToUnity,
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
            // connectToUnity
            // 
            this.connectToUnity.Checked = true;
            this.connectToUnity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.connectToUnity.Name = "connectToUnity";
            this.connectToUnity.Size = new System.Drawing.Size(173, 22);
            this.connectToUnity.Text = "Connect to Unity";
            this.connectToUnity.ToolTipText = "Enable a websocket connection to the Unity Level Viewer.";
            this.connectToUnity.Click += new System.EventHandler(this.connectToUnity_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(170, 6);
            // 
            // compositeViewerToolStripMenuItem
            // 
            this.compositeViewerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showExplorerViewToolStripMenuItem,
            this.autoHideExplorerViewToolStripMenuItem,
            this.searchOnlyCompositeNames});
            this.compositeViewerToolStripMenuItem.Name = "compositeViewerToolStripMenuItem";
            this.compositeViewerToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
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
            this.showNodegraph,
            this.showEntityIDs,
            this.entitiesOpenTabs});
            this.entityDisplayToolStripMenuItem.Name = "entityDisplayToolStripMenuItem";
            this.entityDisplayToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.entityDisplayToolStripMenuItem.Text = "Entity Display";
            // 
            // showNodegraph
            // 
            this.showNodegraph.Checked = true;
            this.showNodegraph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showNodegraph.Name = "showNodegraph";
            this.showNodegraph.Size = new System.Drawing.Size(210, 22);
            this.showNodegraph.Text = "Show Nodegraph";
            this.showNodegraph.ToolTipText = "Show the nodegraph view for the currently selected entity.";
            this.showNodegraph.Click += new System.EventHandler(this.showNodegraph_Click);
            // 
            // showEntityIDs
            // 
            this.showEntityIDs.Name = "showEntityIDs";
            this.showEntityIDs.Size = new System.Drawing.Size(210, 22);
            this.showEntityIDs.Text = "Show Entity IDs";
            this.showEntityIDs.ToolTipText = "Show entity IDs within the editor UI.";
            this.showEntityIDs.Click += new System.EventHandler(this.showEntityIDs_Click);
            // 
            // entitiesOpenTabs
            // 
            this.entitiesOpenTabs.Name = "entitiesOpenTabs";
            this.entitiesOpenTabs.Size = new System.Drawing.Size(210, 22);
            this.entitiesOpenTabs.Text = "Open Entities In New Tabs";
            this.entitiesOpenTabs.ToolTipText = "If enabled, entities will open in new tabs when selected, allowing multiple to be" +
    " visible at once.";
            this.entitiesOpenTabs.Click += new System.EventHandler(this.entitiesOpenTabs_Click);
            // 
            // miscToolStripMenuItem
            // 
            this.miscToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showConfirmationWhenSavingToolStripMenuItem,
            this.useTexturedModelViewExperimentalToolStripMenuItem,
            this.keepFunctionUsesWindowOpenToolStripMenuItem});
            this.miscToolStripMenuItem.Name = "miscToolStripMenuItem";
            this.miscToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.miscToolStripMenuItem.Text = "Misc";
            // 
            // showConfirmationWhenSavingToolStripMenuItem
            // 
            this.showConfirmationWhenSavingToolStripMenuItem.Name = "showConfirmationWhenSavingToolStripMenuItem";
            this.showConfirmationWhenSavingToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
            this.showConfirmationWhenSavingToolStripMenuItem.Text = "Show Confirmation When Saving";
            this.showConfirmationWhenSavingToolStripMenuItem.ToolTipText = "If enabled, a confirmation will show after a successful save.";
            this.showConfirmationWhenSavingToolStripMenuItem.Click += new System.EventHandler(this.showConfirmationWhenSavingToolStripMenuItem_Click);
            // 
            // useTexturedModelViewExperimentalToolStripMenuItem
            // 
            this.useTexturedModelViewExperimentalToolStripMenuItem.Name = "useTexturedModelViewExperimentalToolStripMenuItem";
            this.useTexturedModelViewExperimentalToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
            this.useTexturedModelViewExperimentalToolStripMenuItem.Text = "Use Textured Model View (Experimental)";
            this.useTexturedModelViewExperimentalToolStripMenuItem.ToolTipText = "If enabled, the model previewer will try and find textures to render.";
            this.useTexturedModelViewExperimentalToolStripMenuItem.Click += new System.EventHandler(this.useTexturedModelViewExperimentalToolStripMenuItem_Click);
            // 
            // keepFunctionUsesWindowOpenToolStripMenuItem
            // 
            this.keepFunctionUsesWindowOpenToolStripMenuItem.Name = "keepFunctionUsesWindowOpenToolStripMenuItem";
            this.keepFunctionUsesWindowOpenToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
            this.keepFunctionUsesWindowOpenToolStripMenuItem.Text = "Keep Function Uses Window Open";
            this.keepFunctionUsesWindowOpenToolStripMenuItem.Click += new System.EventHandler(this.keepFunctionUsesWindowOpenToolStripMenuItem_Click);
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
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Black;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText});
            this.statusStrip.Location = new System.Drawing.Point(0, 782);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1257, 22);
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
            this.dockPanel.Location = new System.Drawing.Point(0, 25);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Padding = new System.Windows.Forms.Padding(6);
            this.dockPanel.ShowAutoHideContentOnHover = false;
            this.dockPanel.Size = new System.Drawing.Size(1257, 757);
            this.dockPanel.TabIndex = 5;
            this.dockPanel.Theme = this.vS2015BlueTheme1;
            // 
            // CommandsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1257, 804);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "CommandsEditor";
            this.ShowIcon = false;
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
        private System.Windows.Forms.ToolStripMenuItem connectToUnity;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton helpBtn;
        private System.Windows.Forms.ToolStripMenuItem compositeViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showExplorerViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoHideExplorerViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchOnlyCompositeNames;
        private System.Windows.Forms.ToolStripMenuItem entityDisplayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showNodegraph;
        private System.Windows.Forms.ToolStripMenuItem showEntityIDs;
        private System.Windows.Forms.ToolStripMenuItem entitiesOpenTabs;
        private System.Windows.Forms.ToolStripMenuItem miscToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showConfirmationWhenSavingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useTexturedModelViewExperimentalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepFunctionUsesWindowOpenToolStripMenuItem;
    }
}