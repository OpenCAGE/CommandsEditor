using System.Drawing;

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
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.createComposite = new System.Windows.Forms.ToolStripButton();
            this.createFolder = new System.Windows.Forms.ToolStripButton();
            this.findFuncs = new System.Windows.Forms.ToolStripButton();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.entity_search_box = new System.Windows.Forms.TextBox();
            this.entity_search_btn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.pathDisplay = new System.Windows.Forms.TextBox();
            this.goBackOnPath = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.createFolder,
            this.findFuncs});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1513, 25);
            this.toolStrip1.TabIndex = 157;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // createComposite
            // 
            this.createComposite.Image = ((System.Drawing.Image)(resources.GetObject("createComposite.Image")));
            this.createComposite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createComposite.Name = "createComposite";
            this.createComposite.Size = new System.Drawing.Size(125, 22);
            this.createComposite.Text = "Create New Prefab";
            this.createComposite.Click += new System.EventHandler(this.createComposite_Click);
            // 
            // createFolder
            // 
            this.createFolder.Image = ((System.Drawing.Image)(resources.GetObject("createFolder.Image")));
            this.createFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createFolder.Name = "createFolder";
            this.createFolder.Size = new System.Drawing.Size(124, 22);
            this.createFolder.Text = "Create New Folder";
            this.createFolder.Click += new System.EventHandler(this.createFolder_Click);
            // 
            // findFuncs
            // 
            this.findFuncs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.findFuncs.Image = ((System.Drawing.Image)(resources.GetObject("findFuncs.Image")));
            this.findFuncs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findFuncs.Name = "findFuncs";
            this.findFuncs.Size = new System.Drawing.Size(141, 22);
            this.findFuncs.Text = "Find Function Entities";
            this.findFuncs.Click += new System.EventHandler(this.findFuncs_Click);
            // 
            // entity_search_box
            // 
            this.entity_search_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entity_search_box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.entity_search_box.Location = new System.Drawing.Point(4, 28);
            this.entity_search_box.Name = "entity_search_box";
            this.entity_search_box.Size = new System.Drawing.Size(1442, 20);
            this.entity_search_box.TabIndex = 159;
            // 
            // entity_search_btn
            // 
            this.entity_search_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.entity_search_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.entity_search_btn.Location = new System.Drawing.Point(1445, 28);
            this.entity_search_btn.Name = "entity_search_btn";
            this.entity_search_btn.Size = new System.Drawing.Size(63, 20);
            this.entity_search_btn.TabIndex = 158;
            this.entity_search_btn.Text = "Search";
            this.entity_search_btn.UseVisualStyleBackColor = true;
            this.entity_search_btn.Click += new System.EventHandler(this.entity_search_btn_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 19);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1156, 661);
            this.listView1.TabIndex = 179;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "temp_prefab.png");
            this.imageList1.Images.SetKeyName(1, "temp_folder.png");
            this.imageList1.Images.SetKeyName(2, "temp_globe.png");
            this.imageList1.Images.SetKeyName(3, "temp_gamemanager.png");
            this.imageList1.Images.SetKeyName(4, "temp_displaymodel.png");
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(344, 680);
            this.treeView1.TabIndex = 180;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // pathDisplay
            // 
            this.pathDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pathDisplay.Enabled = false;
            this.pathDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathDisplay.Location = new System.Drawing.Point(62, 0);
            this.pathDisplay.Name = "pathDisplay";
            this.pathDisplay.ReadOnly = true;
            this.pathDisplay.Size = new System.Drawing.Size(1094, 20);
            this.pathDisplay.TabIndex = 181;
            // 
            // goBackOnPath
            // 
            this.goBackOnPath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.goBackOnPath.Location = new System.Drawing.Point(0, 0);
            this.goBackOnPath.Name = "goBackOnPath";
            this.goBackOnPath.Size = new System.Drawing.Size(63, 20);
            this.goBackOnPath.TabIndex = 182;
            this.goBackOnPath.Text = "< Back";
            this.goBackOnPath.UseVisualStyleBackColor = true;
            this.goBackOnPath.Click += new System.EventHandler(this.goBackOnPath_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(4, 54);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.goBackOnPath);
            this.splitContainer1.Panel2.Controls.Add(this.pathDisplay);
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(1504, 680);
            this.splitContainer1.SplitterDistance = 344;
            this.splitContainer1.TabIndex = 183;
            // 
            // CommandsDisplay
            // 
            this.AllowEndUserDocking = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1513, 734);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.entity_search_box);
            this.Controls.Add(this.entity_search_btn);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommandsDisplay";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
            this.Text = "Prefabs";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton createComposite;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TextBox entity_search_box;
        private System.Windows.Forms.Button entity_search_btn;
        private System.Windows.Forms.ToolStripButton createFolder;
        private System.Windows.Forms.ToolStripButton findFuncs;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox pathDisplay;
        private System.Windows.Forms.Button goBackOnPath;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}