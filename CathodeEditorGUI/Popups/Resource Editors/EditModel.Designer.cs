namespace CommandsEditor
{
    partial class EditModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditModel));
            this.modelRendererHost = new System.Windows.Forms.Integration.ElementHost();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.modelPreviewArea = new System.Windows.Forms.GroupBox();
            this.useMaterials = new System.Windows.Forms.CheckBox();
            this.selectModelBtn = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.submeshFilterGroup = new System.Windows.Forms.GroupBox();
            this.submeshFilterPanel = new System.Windows.Forms.Panel();
            this.modelPreviewArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.submeshFilterGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelRendererHost
            // 
            this.modelRendererHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelRendererHost.Location = new System.Drawing.Point(3, 16);
            this.modelRendererHost.Name = "modelRendererHost";
            this.modelRendererHost.Size = new System.Drawing.Size(483, 610);
            this.modelRendererHost.TabIndex = 0;
            this.modelRendererHost.Text = "elementHost1";
            this.modelRendererHost.Child = null;
            // 
            // FileTree
            // 
            this.FileTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileTree.FullRowSelect = true;
            this.FileTree.HideSelection = false;
            this.FileTree.Location = new System.Drawing.Point(0, 0);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(352, 632);
            this.FileTree.TabIndex = 100;
            this.FileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTree_AfterSelect);
            // 
            // modelPreviewArea
            // 
            this.modelPreviewArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modelPreviewArea.Controls.Add(this.useMaterials);
            this.modelPreviewArea.Controls.Add(this.selectModelBtn);
            this.modelPreviewArea.Controls.Add(this.modelRendererHost);
            this.modelPreviewArea.Location = new System.Drawing.Point(3, 3);
            this.modelPreviewArea.Name = "modelPreviewArea";
            this.modelPreviewArea.Size = new System.Drawing.Size(489, 629);
            this.modelPreviewArea.TabIndex = 103;
            this.modelPreviewArea.TabStop = false;
            // 
            // useMaterials
            // 
            this.useMaterials.AutoSize = true;
            this.useMaterials.Location = new System.Drawing.Point(6, 606);
            this.useMaterials.Name = "useMaterials";
            this.useMaterials.Size = new System.Drawing.Size(111, 17);
            this.useMaterials.TabIndex = 2;
            this.useMaterials.Text = "Use Material Data";
            this.useMaterials.UseVisualStyleBackColor = true;
            this.useMaterials.CheckedChanged += new System.EventHandler(this.useMaterials_CheckedChanged);
            // 
            // selectModelBtn
            // 
            this.selectModelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectModelBtn.Enabled = false;
            this.selectModelBtn.Location = new System.Drawing.Point(340, 579);
            this.selectModelBtn.Name = "selectModelBtn";
            this.selectModelBtn.Size = new System.Drawing.Size(134, 35);
            this.selectModelBtn.TabIndex = 1;
            this.selectModelBtn.Text = "Select This Model";
            this.selectModelBtn.UseVisualStyleBackColor = true;
            this.selectModelBtn.Click += new System.EventHandler(this.selectModel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(7, 8);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FileTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1057, 632);
            this.splitContainer1.SplitterDistance = 352;
            this.splitContainer1.TabIndex = 104;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.modelPreviewArea);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.submeshFilterGroup);
            this.splitContainer2.Size = new System.Drawing.Size(701, 632);
            this.splitContainer2.SplitterDistance = 495;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // submeshFilterGroup
            // 
            this.submeshFilterGroup.Controls.Add(this.submeshFilterPanel);
            this.submeshFilterGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.submeshFilterGroup.Location = new System.Drawing.Point(0, 0);
            this.submeshFilterGroup.Name = "submeshFilterGroup";
            this.submeshFilterGroup.Size = new System.Drawing.Size(201, 632);
            this.submeshFilterGroup.TabIndex = 0;
            this.submeshFilterGroup.TabStop = false;
            this.submeshFilterGroup.Text = "Render Filter";
            // 
            // submeshFilterPanel
            // 
            this.submeshFilterPanel.AutoScroll = true;
            this.submeshFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.submeshFilterPanel.Location = new System.Drawing.Point(3, 16);
            this.submeshFilterPanel.Name = "submeshFilterPanel";
            this.submeshFilterPanel.Size = new System.Drawing.Size(195, 613);
            this.submeshFilterPanel.TabIndex = 0;
            // 
            // EditModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 647);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Model Editor";
            this.modelPreviewArea.ResumeLayout(false);
            this.modelPreviewArea.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.submeshFilterGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost modelRendererHost;
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.GroupBox modelPreviewArea;
        private System.Windows.Forms.Button selectModelBtn;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox useMaterials;
        private System.Windows.Forms.GroupBox submeshFilterGroup;
        private System.Windows.Forms.Panel submeshFilterPanel;
    }
}