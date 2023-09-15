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
            this.enableInstanceMode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.enableBackups = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToUnity = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showNodegraph = new System.Windows.Forms.ToolStripMenuItem();
            this.showEntityIDs = new System.Windows.Forms.ToolStripMenuItem();
            this.showConfirmationWhenSavingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchOnlyCompositeNames = new System.Windows.Forms.ToolStripMenuItem();
            this.entitiesOpenTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.useTexturedModelViewExperimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripButton2});
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
            this.enableInstanceMode,
            this.toolStripSeparator1,
            this.enableBackups,
            this.connectToUnity,
            this.toolStripSeparator2,
            this.showNodegraph,
            this.showEntityIDs,
            this.showConfirmationWhenSavingToolStripMenuItem,
            this.searchOnlyCompositeNames,
            this.entitiesOpenTabs,
            this.useTexturedModelViewExperimentalToolStripMenuItem});
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(62, 22);
            this.toolStripButton2.Text = "Options";
            // 
            // enableInstanceMode
            // 
            this.enableInstanceMode.Name = "enableInstanceMode";
            this.enableInstanceMode.Size = new System.Drawing.Size(286, 22);
            this.enableInstanceMode.Text = "Instance Mode";
            this.enableInstanceMode.ToolTipText = "Enable instance mode to modify entity instances.";
            this.enableInstanceMode.Click += new System.EventHandler(this.enableInstanceMode_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(283, 6);
            // 
            // enableBackups
            // 
            this.enableBackups.Checked = true;
            this.enableBackups.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableBackups.Name = "enableBackups";
            this.enableBackups.Size = new System.Drawing.Size(286, 22);
            this.enableBackups.Text = "Autosave (every 5 mins)";
            this.enableBackups.ToolTipText = "If checked, the editor will automatically save the level every 5 minutes.";
            this.enableBackups.Visible = false;
            this.enableBackups.Click += new System.EventHandler(this.enableBackups_Click);
            // 
            // connectToUnity
            // 
            this.connectToUnity.Checked = true;
            this.connectToUnity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.connectToUnity.Name = "connectToUnity";
            this.connectToUnity.Size = new System.Drawing.Size(286, 22);
            this.connectToUnity.Text = "Connect to Unity";
            this.connectToUnity.ToolTipText = "Enable a websocket connection to the Unity Level Viewer.";
            this.connectToUnity.Click += new System.EventHandler(this.connectToUnity_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(283, 6);
            // 
            // showNodegraph
            // 
            this.showNodegraph.Checked = true;
            this.showNodegraph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showNodegraph.Name = "showNodegraph";
            this.showNodegraph.Size = new System.Drawing.Size(286, 22);
            this.showNodegraph.Text = "Show Nodegraph";
            this.showNodegraph.ToolTipText = "Show the nodegraph view for the currently selected entity.";
            this.showNodegraph.Click += new System.EventHandler(this.showNodegraph_Click);
            // 
            // showEntityIDs
            // 
            this.showEntityIDs.Name = "showEntityIDs";
            this.showEntityIDs.Size = new System.Drawing.Size(286, 22);
            this.showEntityIDs.Text = "Show Entity IDs";
            this.showEntityIDs.ToolTipText = "Show entity IDs within the editor UI.";
            this.showEntityIDs.Click += new System.EventHandler(this.showEntityIDs_Click);
            // 
            // showConfirmationWhenSavingToolStripMenuItem
            // 
            this.showConfirmationWhenSavingToolStripMenuItem.Name = "showConfirmationWhenSavingToolStripMenuItem";
            this.showConfirmationWhenSavingToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
            this.showConfirmationWhenSavingToolStripMenuItem.Text = "Show Confirmation When Saving";
            this.showConfirmationWhenSavingToolStripMenuItem.ToolTipText = "If enabled, a confirmation will show after a successful save.";
            this.showConfirmationWhenSavingToolStripMenuItem.Click += new System.EventHandler(this.showConfirmationWhenSavingToolStripMenuItem_Click);
            // 
            // searchOnlyCompositeNames
            // 
            this.searchOnlyCompositeNames.Name = "searchOnlyCompositeNames";
            this.searchOnlyCompositeNames.Size = new System.Drawing.Size(286, 22);
            this.searchOnlyCompositeNames.Text = "Search Only Composite Names";
            this.searchOnlyCompositeNames.ToolTipText = "Enable this option to exclude folder names from the composite search.";
            this.searchOnlyCompositeNames.Click += new System.EventHandler(this.searchOnlyCompositeNames_Click);
            // 
            // entitiesOpenTabs
            // 
            this.entitiesOpenTabs.Name = "entitiesOpenTabs";
            this.entitiesOpenTabs.Size = new System.Drawing.Size(286, 22);
            this.entitiesOpenTabs.Text = "Open Entities In New Tabs";
            this.entitiesOpenTabs.Click += new System.EventHandler(this.entitiesOpenTabs_Click);
            // 
            // useTexturedModelViewExperimentalToolStripMenuItem
            // 
            this.useTexturedModelViewExperimentalToolStripMenuItem.Name = "useTexturedModelViewExperimentalToolStripMenuItem";
            this.useTexturedModelViewExperimentalToolStripMenuItem.Size = new System.Drawing.Size(286, 22);
            this.useTexturedModelViewExperimentalToolStripMenuItem.Text = "Use Textured Model View (Experimental)";
            this.useTexturedModelViewExperimentalToolStripMenuItem.Click += new System.EventHandler(this.useTexturedModelViewExperimentalToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem enableBackups;
        private System.Windows.Forms.ToolStripMenuItem connectToUnity;
        private System.Windows.Forms.ToolStripMenuItem showNodegraph;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripMenuItem showEntityIDs;
        private System.Windows.Forms.ToolStripMenuItem enableInstanceMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem searchOnlyCompositeNames;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem entitiesOpenTabs;
        private System.Windows.Forms.ToolStripMenuItem showConfirmationWhenSavingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useTexturedModelViewExperimentalToolStripMenuItem;
    }
}