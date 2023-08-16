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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandsEditor));
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.loadLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.enableBackups = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToUnity = new System.Windows.Forms.ToolStripMenuItem();
            this.showNodegraph = new System.Windows.Forms.ToolStripMenuItem();
            this.showEntityIDs = new System.Windows.Forms.ToolStripMenuItem();
            this.vS2015BlueTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
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
            this.loadLevel.Size = new System.Drawing.Size(130, 22);
            this.loadLevel.Text = "Load Level";
            this.loadLevel.Click += new System.EventHandler(this.loadLevel_Click);
            // 
            // saveLevel
            // 
            this.saveLevel.Name = "saveLevel";
            this.saveLevel.Size = new System.Drawing.Size(130, 22);
            this.saveLevel.Text = "Save Level";
            this.saveLevel.Click += new System.EventHandler(this.saveLevel_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableBackups,
            this.connectToUnity,
            this.showNodegraph,
            this.showEntityIDs});
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(62, 22);
            this.toolStripButton2.Text = "Options";
            // 
            // enableBackups
            // 
            this.enableBackups.Checked = true;
            this.enableBackups.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableBackups.Name = "enableBackups";
            this.enableBackups.Size = new System.Drawing.Size(180, 22);
            this.enableBackups.Text = "Enable Backups";
            this.enableBackups.Click += new System.EventHandler(this.enableBackups_Click);
            // 
            // connectToUnity
            // 
            this.connectToUnity.Checked = true;
            this.connectToUnity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.connectToUnity.Name = "connectToUnity";
            this.connectToUnity.Size = new System.Drawing.Size(180, 22);
            this.connectToUnity.Text = "Connect to Unity";
            this.connectToUnity.Click += new System.EventHandler(this.connectToUnity_Click);
            // 
            // showNodegraph
            // 
            this.showNodegraph.Checked = true;
            this.showNodegraph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showNodegraph.Name = "showNodegraph";
            this.showNodegraph.Size = new System.Drawing.Size(180, 22);
            this.showNodegraph.Text = "Show Nodegraph";
            this.showNodegraph.Click += new System.EventHandler(this.showNodegraph_Click);
            // 
            // showEntityIDs
            // 
            this.showEntityIDs.Name = "showEntityIDs";
            this.showEntityIDs.Size = new System.Drawing.Size(180, 22);
            this.showEntityIDs.Text = "Show Entity IDs";
            this.showEntityIDs.Click += new System.EventHandler(this.showEntityIDs_Click);
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
            this.dockPanel.DockLeftPortion = 0.3D;
            this.dockPanel.DockRightPortion = 0.35D;
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
    }
}