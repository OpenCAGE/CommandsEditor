namespace CommandsEditor
{
    partial class Flowgraph
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Flowgraph));
            this.nodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modifyPinsIn = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyPinsOut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addAllPinsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeUnusedPinsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stNodeEditor1 = new ST.Library.UI.NodeEditor.STNodeEditor();
            this.TabStripContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteFGToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.createNewFlowgraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeContextMenu.SuspendLayout();
            this.TabStripContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // nodeContextMenu
            // 
            this.nodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyPinsIn,
            this.modifyPinsOut,
            this.toolStripSeparator2,
            this.addAllPinsToolStripMenuItem,
            this.removeUnusedPinsToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.addNodeToolStripMenuItem});
            this.nodeContextMenu.Name = "EntityListContextMenu";
            this.nodeContextMenu.Size = new System.Drawing.Size(186, 170);
            this.nodeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // modifyPinsIn
            // 
            this.modifyPinsIn.Image = ((System.Drawing.Image)(resources.GetObject("modifyPinsIn.Image")));
            this.modifyPinsIn.Name = "modifyPinsIn";
            this.modifyPinsIn.Size = new System.Drawing.Size(185, 22);
            this.modifyPinsIn.Text = "Modify Pins In";
            this.modifyPinsIn.Click += new System.EventHandler(this.modifyPinsIn_Click);
            // 
            // modifyPinsOut
            // 
            this.modifyPinsOut.Image = ((System.Drawing.Image)(resources.GetObject("modifyPinsOut.Image")));
            this.modifyPinsOut.Name = "modifyPinsOut";
            this.modifyPinsOut.Size = new System.Drawing.Size(185, 22);
            this.modifyPinsOut.Text = "Modify Pins Out";
            this.modifyPinsOut.Click += new System.EventHandler(this.modifyPinsOut_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
            // 
            // addAllPinsToolStripMenuItem
            // 
            this.addAllPinsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addAllPinsToolStripMenuItem.Image")));
            this.addAllPinsToolStripMenuItem.Name = "addAllPinsToolStripMenuItem";
            this.addAllPinsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.addAllPinsToolStripMenuItem.Text = "Add All Pins";
            this.addAllPinsToolStripMenuItem.Click += new System.EventHandler(this.addAllPinsToolStripMenuItem_Click);
            // 
            // removeUnusedPinsToolStripMenuItem
            // 
            this.removeUnusedPinsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeUnusedPinsToolStripMenuItem.Image")));
            this.removeUnusedPinsToolStripMenuItem.Name = "removeUnusedPinsToolStripMenuItem";
            this.removeUnusedPinsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.removeUnusedPinsToolStripMenuItem.Text = "Remove Unused Pins";
            this.removeUnusedPinsToolStripMenuItem.Click += new System.EventHandler(this.removeUnusedPinsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.deleteToolStripMenuItem.Text = "Delete Node";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("duplicateToolStripMenuItem.Image")));
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.duplicateToolStripMenuItem.Text = "Duplicate Node";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addNodeToolStripMenuItem.Image")));
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.addNodeToolStripMenuItem.Text = "Add Node(s)";
            this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.addNodeToolStripMenuItem_Click);
            // 
            // stNodeEditor1
            // 
            this.stNodeEditor1.AllowDrop = true;
            this.stNodeEditor1.AllowNodeGraphLoops = true;
            this.stNodeEditor1.AllowSameOwnerConnections = false;
            this.stNodeEditor1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.stNodeEditor1.ContextMenuStrip = this.nodeContextMenu;
            this.stNodeEditor1.Curvature = 0.3F;
            this.stNodeEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stNodeEditor1.Location = new System.Drawing.Point(0, 0);
            this.stNodeEditor1.LocationBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.stNodeEditor1.MarkBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.stNodeEditor1.MarkForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.stNodeEditor1.MinimumSize = new System.Drawing.Size(100, 100);
            this.stNodeEditor1.Name = "stNodeEditor1";
            this.stNodeEditor1.RequireCtrlForZooming = false;
            this.stNodeEditor1.RoundedCornerRadius = 10;
            this.stNodeEditor1.Size = new System.Drawing.Size(1512, 699);
            this.stNodeEditor1.TabIndex = 1;
            this.stNodeEditor1.Text = "stNodeEditor1";
            // 
            // TabStripContextMenu
            // 
            this.TabStripContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteFGToolstripMenuItem,
            this.renameFGToolStripMenuItem,
            this.toolStripSeparator3,
            this.createNewFlowgraphToolStripMenuItem});
            this.TabStripContextMenu.Name = "TabStripContextMenu";
            this.TabStripContextMenu.Size = new System.Drawing.Size(195, 76);
            this.TabStripContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TabStripContextMenu_Opening);
            // 
            // deleteFGToolstripMenuItem
            // 
            this.deleteFGToolstripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteFGToolstripMenuItem.Image")));
            this.deleteFGToolstripMenuItem.Name = "deleteFGToolstripMenuItem";
            this.deleteFGToolstripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.deleteFGToolstripMenuItem.Text = "Delete";
            this.deleteFGToolstripMenuItem.Click += new System.EventHandler(this.deleteFGToolstripMenuItem_Click);
            // 
            // renameFGToolStripMenuItem
            // 
            this.renameFGToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameFGToolStripMenuItem.Image")));
            this.renameFGToolStripMenuItem.Name = "renameFGToolStripMenuItem";
            this.renameFGToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.renameFGToolStripMenuItem.Text = "Rename ";
            this.renameFGToolStripMenuItem.Click += new System.EventHandler(this.renameFGToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(191, 6);
            // 
            // createNewFlowgraphToolStripMenuItem
            // 
            this.createNewFlowgraphToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createNewFlowgraphToolStripMenuItem.Image")));
            this.createNewFlowgraphToolStripMenuItem.Name = "createNewFlowgraphToolStripMenuItem";
            this.createNewFlowgraphToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.createNewFlowgraphToolStripMenuItem.Text = "Create New Flowgraph";
            this.createNewFlowgraphToolStripMenuItem.Click += new System.EventHandler(this.createNewFlowgraphToolStripMenuItem_Click);
            // 
            // Flowgraph
            // 
            this.AllowEndUserDocking = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 699);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.stNodeEditor1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Flowgraph";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TabPageContextMenuStrip = this.TabStripContextMenu;
            this.Text = "Flowgraph";
            this.nodeContextMenu.ResumeLayout(false);
            this.TabStripContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ST.Library.UI.NodeEditor.STNodeEditor stNodeEditor1;
        private System.Windows.Forms.ContextMenuStrip nodeContextMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifyPinsIn;
        private System.Windows.Forms.ToolStripMenuItem modifyPinsOut;
        private System.Windows.Forms.ContextMenuStrip TabStripContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteFGToolstripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameFGToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem createNewFlowgraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem addAllPinsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeUnusedPinsToolStripMenuItem;
    }
}

