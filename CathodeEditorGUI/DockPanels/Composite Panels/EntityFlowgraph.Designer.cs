namespace CommandsEditor
{
    partial class EntityFlowgraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityFlowgraph));
            this.SaveFlowgraph = new System.Windows.Forms.Button();
            this.DEBUG_CalcPositions = new System.Windows.Forms.Button();
            this.DEBUG_NextUnfinished = new System.Windows.Forms.Button();
            this.DEBUG_DumpUnfinished = new System.Windows.Forms.Button();
            this.DEBUG_NextAndSave = new System.Windows.Forms.Button();
            this.DEBUG_Duplicate = new System.Windows.Forms.Button();
            this.DEBUG_SaveAllNoLinks = new System.Windows.Forms.Button();
            this.DEBUG_Next1Link = new System.Windows.Forms.Button();
            this.DEBUG_LoadAll = new System.Windows.Forms.Button();
            this.DEBUG_AddPinIn = new System.Windows.Forms.Button();
            this.DEBUG_AddPinOut = new System.Windows.Forms.Button();
            this.DEBUG_AddNode = new System.Windows.Forms.Button();
            this.DEBUG_Compile = new System.Windows.Forms.Button();
            this.nodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPinInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPinOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.removePinInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePinOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stNodeEditor1 = new ST.Library.UI.NodeEditor.STNodeEditor();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveFlowgraph
            // 
            this.SaveFlowgraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveFlowgraph.Location = new System.Drawing.Point(1436, 1);
            this.SaveFlowgraph.Name = "SaveFlowgraph";
            this.SaveFlowgraph.Size = new System.Drawing.Size(75, 23);
            this.SaveFlowgraph.TabIndex = 2;
            this.SaveFlowgraph.Text = "Save Pos";
            this.SaveFlowgraph.UseVisualStyleBackColor = true;
            this.SaveFlowgraph.Click += new System.EventHandler(this.SaveFlowgraph_Click);
            // 
            // DEBUG_CalcPositions
            // 
            this.DEBUG_CalcPositions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DEBUG_CalcPositions.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_CalcPositions.Location = new System.Drawing.Point(1290, 1);
            this.DEBUG_CalcPositions.Name = "DEBUG_CalcPositions";
            this.DEBUG_CalcPositions.Size = new System.Drawing.Size(121, 23);
            this.DEBUG_CalcPositions.TabIndex = 3;
            this.DEBUG_CalcPositions.Text = "Calc Selected Pos";
            this.DEBUG_CalcPositions.UseVisualStyleBackColor = true;
            this.DEBUG_CalcPositions.Click += new System.EventHandler(this.DEBUG_CalcPositions_Click);
            // 
            // DEBUG_NextUnfinished
            // 
            this.DEBUG_NextUnfinished.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DEBUG_NextUnfinished.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_NextUnfinished.Location = new System.Drawing.Point(1165, 1);
            this.DEBUG_NextUnfinished.Name = "DEBUG_NextUnfinished";
            this.DEBUG_NextUnfinished.Size = new System.Drawing.Size(121, 23);
            this.DEBUG_NextUnfinished.TabIndex = 4;
            this.DEBUG_NextUnfinished.Text = "Next Incomplete";
            this.DEBUG_NextUnfinished.UseVisualStyleBackColor = true;
            this.DEBUG_NextUnfinished.Click += new System.EventHandler(this.DEBUG_NextUnfinished_Click);
            // 
            // DEBUG_DumpUnfinished
            // 
            this.DEBUG_DumpUnfinished.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DEBUG_DumpUnfinished.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_DumpUnfinished.Location = new System.Drawing.Point(1040, 1);
            this.DEBUG_DumpUnfinished.Name = "DEBUG_DumpUnfinished";
            this.DEBUG_DumpUnfinished.Size = new System.Drawing.Size(121, 23);
            this.DEBUG_DumpUnfinished.TabIndex = 5;
            this.DEBUG_DumpUnfinished.Text = "Dump Incomplete";
            this.DEBUG_DumpUnfinished.UseVisualStyleBackColor = true;
            this.DEBUG_DumpUnfinished.Click += new System.EventHandler(this.DEBUG_DumpUnfinished_Click);
            // 
            // DEBUG_NextAndSave
            // 
            this.DEBUG_NextAndSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DEBUG_NextAndSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DEBUG_NextAndSave.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_NextAndSave.Location = new System.Drawing.Point(1165, 56);
            this.DEBUG_NextAndSave.Name = "DEBUG_NextAndSave";
            this.DEBUG_NextAndSave.Size = new System.Drawing.Size(121, 37);
            this.DEBUG_NextAndSave.TabIndex = 6;
            this.DEBUG_NextAndSave.Text = "Next Incomplete And Save";
            this.DEBUG_NextAndSave.UseVisualStyleBackColor = true;
            this.DEBUG_NextAndSave.Click += new System.EventHandler(this.DEBUG_NextAndSave_Click);
            // 
            // DEBUG_Duplicate
            // 
            this.DEBUG_Duplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DEBUG_Duplicate.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_Duplicate.Location = new System.Drawing.Point(1290, 28);
            this.DEBUG_Duplicate.Name = "DEBUG_Duplicate";
            this.DEBUG_Duplicate.Size = new System.Drawing.Size(121, 23);
            this.DEBUG_Duplicate.TabIndex = 7;
            this.DEBUG_Duplicate.Text = "Duplicate Selected";
            this.DEBUG_Duplicate.UseVisualStyleBackColor = true;
            this.DEBUG_Duplicate.Click += new System.EventHandler(this.DEBUG_Duplicate_Click);
            // 
            // DEBUG_SaveAllNoLinks
            // 
            this.DEBUG_SaveAllNoLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DEBUG_SaveAllNoLinks.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_SaveAllNoLinks.Location = new System.Drawing.Point(1040, 28);
            this.DEBUG_SaveAllNoLinks.Name = "DEBUG_SaveAllNoLinks";
            this.DEBUG_SaveAllNoLinks.Size = new System.Drawing.Size(121, 23);
            this.DEBUG_SaveAllNoLinks.TabIndex = 8;
            this.DEBUG_SaveAllNoLinks.Text = "Save All No Links";
            this.DEBUG_SaveAllNoLinks.UseVisualStyleBackColor = true;
            this.DEBUG_SaveAllNoLinks.Click += new System.EventHandler(this.DEBUG_SaveAllNoLinks_Click);
            // 
            // DEBUG_Next1Link
            // 
            this.DEBUG_Next1Link.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DEBUG_Next1Link.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_Next1Link.Location = new System.Drawing.Point(1165, 28);
            this.DEBUG_Next1Link.Name = "DEBUG_Next1Link";
            this.DEBUG_Next1Link.Size = new System.Drawing.Size(121, 23);
            this.DEBUG_Next1Link.TabIndex = 9;
            this.DEBUG_Next1Link.Text = "Next With 1 Link Ent";
            this.DEBUG_Next1Link.UseVisualStyleBackColor = true;
            this.DEBUG_Next1Link.Click += new System.EventHandler(this.DEBUG_Next1Link_Click);
            // 
            // DEBUG_LoadAll
            // 
            this.DEBUG_LoadAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DEBUG_LoadAll.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_LoadAll.Location = new System.Drawing.Point(1040, 56);
            this.DEBUG_LoadAll.Name = "DEBUG_LoadAll";
            this.DEBUG_LoadAll.Size = new System.Drawing.Size(121, 23);
            this.DEBUG_LoadAll.TabIndex = 10;
            this.DEBUG_LoadAll.Text = "Load All";
            this.DEBUG_LoadAll.UseVisualStyleBackColor = true;
            this.DEBUG_LoadAll.Click += new System.EventHandler(this.DEBUG_LoadAll_Click);
            // 
            // DEBUG_AddPinIn
            // 
            this.DEBUG_AddPinIn.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_AddPinIn.Location = new System.Drawing.Point(0, 0);
            this.DEBUG_AddPinIn.Name = "DEBUG_AddPinIn";
            this.DEBUG_AddPinIn.Size = new System.Drawing.Size(85, 23);
            this.DEBUG_AddPinIn.TabIndex = 11;
            this.DEBUG_AddPinIn.Text = "Add Pin In";
            this.DEBUG_AddPinIn.UseVisualStyleBackColor = true;
            // 
            // DEBUG_AddPinOut
            // 
            this.DEBUG_AddPinOut.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_AddPinOut.Location = new System.Drawing.Point(0, 23);
            this.DEBUG_AddPinOut.Name = "DEBUG_AddPinOut";
            this.DEBUG_AddPinOut.Size = new System.Drawing.Size(85, 23);
            this.DEBUG_AddPinOut.TabIndex = 12;
            this.DEBUG_AddPinOut.Text = "Add Pin Out";
            this.DEBUG_AddPinOut.UseVisualStyleBackColor = true;
            // 
            // DEBUG_AddNode
            // 
            this.DEBUG_AddNode.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_AddNode.Location = new System.Drawing.Point(0, 46);
            this.DEBUG_AddNode.Name = "DEBUG_AddNode";
            this.DEBUG_AddNode.Size = new System.Drawing.Size(85, 23);
            this.DEBUG_AddNode.TabIndex = 13;
            this.DEBUG_AddNode.Text = "Add Node";
            this.DEBUG_AddNode.UseVisualStyleBackColor = true;
            // 
            // DEBUG_Compile
            // 
            this.DEBUG_Compile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DEBUG_Compile.ForeColor = System.Drawing.Color.IndianRed;
            this.DEBUG_Compile.Location = new System.Drawing.Point(110, 0);
            this.DEBUG_Compile.Name = "DEBUG_Compile";
            this.DEBUG_Compile.Size = new System.Drawing.Size(85, 23);
            this.DEBUG_Compile.TabIndex = 14;
            this.DEBUG_Compile.Text = "Compile";
            this.DEBUG_Compile.UseVisualStyleBackColor = true;
            this.DEBUG_Compile.Click += new System.EventHandler(this.DEBUG_Compile_Click);
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
            this.nodeContextMenu.Size = new System.Drawing.Size(181, 192);
            this.nodeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // addPinInToolStripMenuItem
            // 
            this.addPinInToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addPinInToolStripMenuItem.Image")));
            this.addPinInToolStripMenuItem.Name = "addPinInToolStripMenuItem";
            this.addPinInToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addPinInToolStripMenuItem.Text = "Add Pin In";
            this.addPinInToolStripMenuItem.Click += new System.EventHandler(this.addPinInToolStripMenuItem_Click);
            // 
            // addPinOutToolStripMenuItem
            // 
            this.addPinOutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addPinOutToolStripMenuItem.Image")));
            this.addPinOutToolStripMenuItem.Name = "addPinOutToolStripMenuItem";
            this.addPinOutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addPinOutToolStripMenuItem.Text = "Add Pin Out";
            this.addPinOutToolStripMenuItem.Click += new System.EventHandler(this.addPinOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // removePinInToolStripMenuItem
            // 
            this.removePinInToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removePinInToolStripMenuItem.Image")));
            this.removePinInToolStripMenuItem.Name = "removePinInToolStripMenuItem";
            this.removePinInToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removePinInToolStripMenuItem.Text = "Remove Pin In";
            this.removePinInToolStripMenuItem.Click += new System.EventHandler(this.removePinInToolStripMenuItem_Click);
            // 
            // removePinOutToolStripMenuItem
            // 
            this.removePinOutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removePinOutToolStripMenuItem.Image")));
            this.removePinOutToolStripMenuItem.Name = "removePinOutToolStripMenuItem";
            this.removePinOutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removePinOutToolStripMenuItem.Text = "Remove Pin Out";
            this.removePinOutToolStripMenuItem.Click += new System.EventHandler(this.removePinOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "Delete Node";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("duplicateToolStripMenuItem.Image")));
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.duplicateToolStripMenuItem.Text = "Duplicate Node";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
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
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addNodeToolStripMenuItem.Image")));
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addNodeToolStripMenuItem.Text = "Add Node";
            this.addNodeToolStripMenuItem.Click += new System.EventHandler(this.addNodeToolStripMenuItem_Click);
            // 
            // EntityFlowgraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 699);
            this.Controls.Add(this.DEBUG_Compile);
            this.Controls.Add(this.DEBUG_AddNode);
            this.Controls.Add(this.DEBUG_AddPinOut);
            this.Controls.Add(this.DEBUG_AddPinIn);
            this.Controls.Add(this.DEBUG_LoadAll);
            this.Controls.Add(this.DEBUG_Next1Link);
            this.Controls.Add(this.DEBUG_SaveAllNoLinks);
            this.Controls.Add(this.DEBUG_Duplicate);
            this.Controls.Add(this.DEBUG_NextAndSave);
            this.Controls.Add(this.DEBUG_DumpUnfinished);
            this.Controls.Add(this.DEBUG_NextUnfinished);
            this.Controls.Add(this.DEBUG_CalcPositions);
            this.Controls.Add(this.SaveFlowgraph);
            this.Controls.Add(this.stNodeEditor1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EntityFlowgraph";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flowgraph";
            this.nodeContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ST.Library.UI.NodeEditor.STNodeEditor stNodeEditor1;
        private System.Windows.Forms.Button SaveFlowgraph;
        private System.Windows.Forms.Button DEBUG_CalcPositions;
        private System.Windows.Forms.Button DEBUG_NextUnfinished;
        private System.Windows.Forms.Button DEBUG_DumpUnfinished;
        private System.Windows.Forms.Button DEBUG_NextAndSave;
        private System.Windows.Forms.Button DEBUG_Duplicate;
        private System.Windows.Forms.Button DEBUG_SaveAllNoLinks;
        private System.Windows.Forms.Button DEBUG_Next1Link;
        private System.Windows.Forms.Button DEBUG_LoadAll;
        private System.Windows.Forms.Button DEBUG_AddPinIn;
        private System.Windows.Forms.Button DEBUG_AddPinOut;
        private System.Windows.Forms.Button DEBUG_AddNode;
        private System.Windows.Forms.Button DEBUG_Compile;
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

