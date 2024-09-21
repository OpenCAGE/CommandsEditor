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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFlowgraph = new System.Windows.Forms.Button();
            this.DEBUG_CalcPositions = new System.Windows.Forms.Button();
            this.DEBUG_NextUnfinished = new System.Windows.Forms.Button();
            this.DEBUG_DumpUnfinished = new System.Windows.Forms.Button();
            this.DEBUG_NextAndSave = new System.Windows.Forms.Button();
            this.stNodeEditor1 = new ST.Library.UI.NodeEditor.STNodeEditor();
            this.DEBUG_Duplicate = new System.Windows.Forms.Button();
            this.DEBUG_SaveAllNoLinks = new System.Windows.Forms.Button();
            this.DEBUG_Next1Link = new System.Windows.Forms.Button();
            this.DEBUG_LoadAll = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.lockLocationToolStripMenuItem,
            this.lockConnectionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(178, 70);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.removeToolStripMenuItem.Text = "&Remove";
            // 
            // lockLocationToolStripMenuItem
            // 
            this.lockLocationToolStripMenuItem.Name = "lockLocationToolStripMenuItem";
            this.lockLocationToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.lockLocationToolStripMenuItem.Text = "U/Lock &Location";
            // 
            // lockConnectionToolStripMenuItem
            // 
            this.lockConnectionToolStripMenuItem.Name = "lockConnectionToolStripMenuItem";
            this.lockConnectionToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.lockConnectionToolStripMenuItem.Text = "U/Lock &Connection";
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
            // stNodeEditor1
            // 
            this.stNodeEditor1.AllowDrop = true;
            this.stNodeEditor1.AllowNodeGraphLoops = true;
            this.stNodeEditor1.AllowSameOwnerConnections = false;
            this.stNodeEditor1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
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
            // EntityFlowgraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 699);
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
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ST.Library.UI.NodeEditor.STNodeEditor stNodeEditor1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockConnectionToolStripMenuItem;
        private System.Windows.Forms.Button SaveFlowgraph;
        private System.Windows.Forms.Button DEBUG_CalcPositions;
        private System.Windows.Forms.Button DEBUG_NextUnfinished;
        private System.Windows.Forms.Button DEBUG_DumpUnfinished;
        private System.Windows.Forms.Button DEBUG_NextAndSave;
        private System.Windows.Forms.Button DEBUG_Duplicate;
        private System.Windows.Forms.Button DEBUG_SaveAllNoLinks;
        private System.Windows.Forms.Button DEBUG_Next1Link;
        private System.Windows.Forms.Button DEBUG_LoadAll;
    }
}
