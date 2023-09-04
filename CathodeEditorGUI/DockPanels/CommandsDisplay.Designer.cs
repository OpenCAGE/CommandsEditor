namespace CommandsEditor.DockPanels
{
    partial class CommandsDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandsDisplay));
            this.FileTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.createComposite = new System.Windows.Forms.ToolStripButton();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.entity_search_box = new System.Windows.Forms.TextBox();
            this.entity_search_btn = new System.Windows.Forms.Button();
            this.createFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileTree
            // 
            this.FileTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileTree.ImageIndex = 0;
            this.FileTree.ImageList = this.imageList;
            this.FileTree.Location = new System.Drawing.Point(0, 54);
            this.FileTree.Name = "FileTree";
            this.FileTree.SelectedImageIndex = 0;
            this.FileTree.Size = new System.Drawing.Size(457, 680);
            this.FileTree.TabIndex = 153;
            this.FileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTree_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "folder");
            this.imageList.Images.SetKeyName(1, "composite");
            this.imageList.Images.SetKeyName(2, "folder_open");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createComposite,
            this.createFolder});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(457, 25);
            this.toolStrip1.TabIndex = 157;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // createComposite
            // 
            this.createComposite.Image = ((System.Drawing.Image)(resources.GetObject("createComposite.Image")));
            this.createComposite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createComposite.Name = "createComposite";
            this.createComposite.Size = new System.Drawing.Size(122, 22);
            this.createComposite.Text = "Create Composite";
            this.createComposite.Click += new System.EventHandler(this.createComposite_Click);
            // 
            // entity_search_box
            // 
            this.entity_search_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entity_search_box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.entity_search_box.Location = new System.Drawing.Point(4, 28);
            this.entity_search_box.Name = "entity_search_box";
            this.entity_search_box.Size = new System.Drawing.Size(386, 20);
            this.entity_search_box.TabIndex = 159;
            // 
            // entity_search_btn
            // 
            this.entity_search_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.entity_search_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.entity_search_btn.Location = new System.Drawing.Point(389, 28);
            this.entity_search_btn.Name = "entity_search_btn";
            this.entity_search_btn.Size = new System.Drawing.Size(63, 20);
            this.entity_search_btn.TabIndex = 158;
            this.entity_search_btn.Text = "Search";
            this.entity_search_btn.UseVisualStyleBackColor = true;
            // 
            // createFolder
            // 
            this.createFolder.Image = ((System.Drawing.Image)(resources.GetObject("createFolder.Image")));
            this.createFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createFolder.Name = "createFolder";
            this.createFolder.Size = new System.Drawing.Size(97, 22);
            this.createFolder.Text = "Create Folder";
            this.createFolder.Click += new System.EventHandler(this.createFolder_Click);
            // 
            // CommandsDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 734);
            this.Controls.Add(this.entity_search_box);
            this.Controls.Add(this.entity_search_btn);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.FileTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommandsDisplay";
            this.Text = "Composites";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton createComposite;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TextBox entity_search_box;
        private System.Windows.Forms.Button entity_search_btn;
        private System.Windows.Forms.ToolStripButton createFolder;
    }
}