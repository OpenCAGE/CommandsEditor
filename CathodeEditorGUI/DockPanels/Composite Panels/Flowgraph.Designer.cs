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
            this.modifyPinsIn = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyPinsOut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AutoCalc = new System.Windows.Forms.Button();
            this.RemoveEmpties = new System.Windows.Forms.Button();
            this.stNodeEditor1 = new ST.Library.UI.NodeEditor.STNodeEditor();
            this.SaveFlowgraphUnfinished = new System.Windows.Forms.Button();
            this.ResetFG = new System.Windows.Forms.Button();
            this.TabStripContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteFGToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.createNewFlowgraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeContextMenu.SuspendLayout();
            this.TabStripContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveFlowgraph
            // 
            this.SaveFlowgraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveFlowgraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveFlowgraph.ForeColor = System.Drawing.Color.IndianRed;
            this.SaveFlowgraph.Location = new System.Drawing.Point(1257, 1);
            this.SaveFlowgraph.Name = "SaveFlowgraph";
            this.SaveFlowgraph.Size = new System.Drawing.Size(254, 38);
            this.SaveFlowgraph.TabIndex = 2;
            this.SaveFlowgraph.Text = "Save to pre-defined DB and load next";
            this.SaveFlowgraph.UseVisualStyleBackColor = true;
            this.SaveFlowgraph.Click += new System.EventHandler(this.SaveFlowgraph_Click);
            // 
            // nodeContextMenu
            // 
            this.nodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyPinsIn,
            this.modifyPinsOut,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.addNodeToolStripMenuItem});
            this.nodeContextMenu.Name = "EntityListContextMenu";
            this.nodeContextMenu.Size = new System.Drawing.Size(161, 120);
            this.nodeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // modifyPinsIn
            // 
            this.modifyPinsIn.Image = ((System.Drawing.Image)(resources.GetObject("modifyPinsIn.Image")));
            this.modifyPinsIn.Name = "modifyPinsIn";
            this.modifyPinsIn.Size = new System.Drawing.Size(160, 22);
            this.modifyPinsIn.Text = "Modify Pins In";
            this.modifyPinsIn.Click += new System.EventHandler(this.modifyPinsIn_Click);
            // 
            // modifyPinsOut
            // 
            this.modifyPinsOut.Image = ((System.Drawing.Image)(resources.GetObject("modifyPinsOut.Image")));
            this.modifyPinsOut.Name = "modifyPinsOut";
            this.modifyPinsOut.Size = new System.Drawing.Size(160, 22);
            this.modifyPinsOut.Text = "Modify Pins Out";
            this.modifyPinsOut.Click += new System.EventHandler(this.modifyPinsOut_Click);
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
            this.addNodeToolStripMenuItem.Text = "Add Node(s)";
            this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.addNodeToolStripMenuItem_Click);
            // 
            // AutoCalc
            // 
            this.AutoCalc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AutoCalc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoCalc.ForeColor = System.Drawing.Color.IndianRed;
            this.AutoCalc.Location = new System.Drawing.Point(0, 661);
            this.AutoCalc.Name = "AutoCalc";
            this.AutoCalc.Size = new System.Drawing.Size(123, 38);
            this.AutoCalc.TabIndex = 3;
            this.AutoCalc.Text = "Auto Calc";
            this.AutoCalc.UseVisualStyleBackColor = true;
            this.AutoCalc.Click += new System.EventHandler(this.AutoCalc_Click);
            // 
            // RemoveEmpties
            // 
            this.RemoveEmpties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveEmpties.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveEmpties.ForeColor = System.Drawing.Color.IndianRed;
            this.RemoveEmpties.Location = new System.Drawing.Point(123, 661);
            this.RemoveEmpties.Name = "RemoveEmpties";
            this.RemoveEmpties.Size = new System.Drawing.Size(123, 38);
            this.RemoveEmpties.TabIndex = 4;
            this.RemoveEmpties.Text = "Remove Empties";
            this.RemoveEmpties.UseVisualStyleBackColor = true;
            this.RemoveEmpties.Click += new System.EventHandler(this.RemoveEmpties_Click);
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
            // SaveFlowgraphUnfinished
            // 
            this.SaveFlowgraphUnfinished.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveFlowgraphUnfinished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveFlowgraphUnfinished.ForeColor = System.Drawing.Color.IndianRed;
            this.SaveFlowgraphUnfinished.Location = new System.Drawing.Point(1097, 1);
            this.SaveFlowgraphUnfinished.Name = "SaveFlowgraphUnfinished";
            this.SaveFlowgraphUnfinished.Size = new System.Drawing.Size(158, 38);
            this.SaveFlowgraphUnfinished.TabIndex = 5;
            this.SaveFlowgraphUnfinished.Text = "Save as Unfinished";
            this.SaveFlowgraphUnfinished.UseVisualStyleBackColor = true;
            this.SaveFlowgraphUnfinished.Click += new System.EventHandler(this.SaveFlowgraphUnfinished_Click);
            // 
            // ResetFG
            // 
            this.ResetFG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ResetFG.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetFG.ForeColor = System.Drawing.Color.IndianRed;
            this.ResetFG.Location = new System.Drawing.Point(1354, 661);
            this.ResetFG.Name = "ResetFG";
            this.ResetFG.Size = new System.Drawing.Size(158, 38);
            this.ResetFG.TabIndex = 6;
            this.ResetFG.Text = "Reset";
            this.ResetFG.UseVisualStyleBackColor = true;
            this.ResetFG.Click += new System.EventHandler(this.ResetFG_Click);
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
            this.Controls.Add(this.ResetFG);
            this.Controls.Add(this.SaveFlowgraphUnfinished);
            this.Controls.Add(this.RemoveEmpties);
            this.Controls.Add(this.AutoCalc);
            this.Controls.Add(this.SaveFlowgraph);
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
        private System.Windows.Forms.Button SaveFlowgraph;
        private System.Windows.Forms.ContextMenuStrip nodeContextMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifyPinsIn;
        private System.Windows.Forms.ToolStripMenuItem modifyPinsOut;
        private System.Windows.Forms.Button AutoCalc;
        private System.Windows.Forms.Button RemoveEmpties;
        private System.Windows.Forms.Button SaveFlowgraphUnfinished;
        private System.Windows.Forms.Button ResetFG;
        private System.Windows.Forms.ContextMenuStrip TabStripContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteFGToolstripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameFGToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem createNewFlowgraphToolStripMenuItem;
    }
}

