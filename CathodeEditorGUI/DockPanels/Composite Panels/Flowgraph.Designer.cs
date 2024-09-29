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
            this.SaveFlowgraph = new System.Windows.Forms.Button();
            this.nodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPinInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPinOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.removePinInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePinOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stNodeEditor1 = new ST.Library.UI.NodeEditor.STNodeEditor();
            this.nodeContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveFlowgraph
            // 
            this.SaveFlowgraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveFlowgraph.Location = new System.Drawing.Point(1372, 1);
            this.SaveFlowgraph.Name = "SaveFlowgraph";
            this.SaveFlowgraph.Size = new System.Drawing.Size(139, 23);
            this.SaveFlowgraph.TabIndex = 2;
            this.SaveFlowgraph.Text = "Save To Pre-Defined DB";
            this.SaveFlowgraph.UseVisualStyleBackColor = true;
            this.SaveFlowgraph.Click += new System.EventHandler(this.SaveFlowgraph_Click);
            // 
            // nodeContextMenu
            // 
            this.nodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPinInToolStripMenuItem,
            this.addPinOutToolStripMenuItem,
            this.toolStripSeparator2,
            this.removePinInToolStripMenuItem,
            this.removePinOutToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.addNodeToolStripMenuItem});
            this.nodeContextMenu.Name = "EntityListContextMenu";
            this.nodeContextMenu.Size = new System.Drawing.Size(161, 170);
            this.nodeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // addPinInToolStripMenuItem
            // 
            this.addPinInToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addPinInToolStripMenuItem.Image")));
            this.addPinInToolStripMenuItem.Name = "addPinInToolStripMenuItem";
            this.addPinInToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.addPinInToolStripMenuItem.Text = "Add Pin In";
            this.addPinInToolStripMenuItem.Click += new System.EventHandler(this.addPinInToolStripMenuItem_Click);
            // 
            // addPinOutToolStripMenuItem
            // 
            this.addPinOutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addPinOutToolStripMenuItem.Image")));
            this.addPinOutToolStripMenuItem.Name = "addPinOutToolStripMenuItem";
            this.addPinOutToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.addPinOutToolStripMenuItem.Text = "Add Pin Out";
            this.addPinOutToolStripMenuItem.Click += new System.EventHandler(this.addPinOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // removePinInToolStripMenuItem
            // 
            this.removePinInToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removePinInToolStripMenuItem.Image")));
            this.removePinInToolStripMenuItem.Name = "removePinInToolStripMenuItem";
            this.removePinInToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.removePinInToolStripMenuItem.Text = "Remove Pin In";
            this.removePinInToolStripMenuItem.Click += new System.EventHandler(this.removePinInToolStripMenuItem_Click);
            // 
            // removePinOutToolStripMenuItem
            // 
            this.removePinOutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removePinOutToolStripMenuItem.Image")));
            this.removePinOutToolStripMenuItem.Name = "removePinOutToolStripMenuItem";
            this.removePinOutToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.removePinOutToolStripMenuItem.Text = "Remove Pin Out";
            this.removePinOutToolStripMenuItem.Click += new System.EventHandler(this.removePinOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.deleteToolStripMenuItem.Text = "Delete Node";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("duplicateToolStripMenuItem.Image")));
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.duplicateToolStripMenuItem.Text = "Duplicate Node";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addNodeToolStripMenuItem.Image")));
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.addNodeToolStripMenuItem.Text = "Add Node";
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
            // Flowgraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 699);
            this.Controls.Add(this.SaveFlowgraph);
            this.Controls.Add(this.stNodeEditor1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Flowgraph";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flowgraph";
            this.nodeContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ST.Library.UI.NodeEditor.STNodeEditor stNodeEditor1;
        private System.Windows.Forms.Button SaveFlowgraph;
        private System.Windows.Forms.ContextMenuStrip nodeContextMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem addPinInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPinOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePinInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePinOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
    }
}

