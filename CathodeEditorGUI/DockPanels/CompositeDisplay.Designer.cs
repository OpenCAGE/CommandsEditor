namespace CommandsEditor.DockPanels
{
    partial class CompositeDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeDisplay));
            this.entityListIcons = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.createEntity = new System.Windows.Forms.ToolStripDropDownButton();
            this.createVariableEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFunctionEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createCompositeEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createProxyEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAliasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportComposite = new System.Windows.Forms.ToolStripButton();
            this.findUses = new System.Windows.Forms.ToolStripButton();
            this.deleteCheckedEntities = new System.Windows.Forms.ToolStripButton();
            this.deleteComposite = new System.Windows.Forms.ToolStripButton();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.vS2015BlueTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.panel1 = new System.Windows.Forms.Panel();
            this.compositeEntityList1 = new Popups.UserControls.CompositeEntityList();
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.goBackOnPath = new System.Windows.Forms.Button();
            this.pathDisplay = new System.Windows.Forms.TextBox();
            this.instanceInfo = new System.Windows.Forms.Button();
            this.EntityListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createParameterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFunctionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createInstanceOfCompositeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAliasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameComposite = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.EntityListContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // entityListIcons
            // 
            this.entityListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("entityListIcons.ImageStream")));
            this.entityListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.entityListIcons.Images.SetKeyName(0, "AnimatorController Icon.png");
            this.entityListIcons.Images.SetKeyName(1, "d_ScriptableObject Icon braces only.png");
            this.entityListIcons.Images.SetKeyName(2, "d_PrefabVariant Icon.png");
            this.entityListIcons.Images.SetKeyName(3, "d_ScriptableObject Icon.png");
            this.entityListIcons.Images.SetKeyName(4, "AreaEffector2D Icon.ico");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createEntity,
            this.exportComposite,
            this.findUses,
            this.deleteCheckedEntities,
            this.renameComposite,
            this.deleteComposite});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1615, 25);
            this.toolStrip1.TabIndex = 177;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // createEntity
            // 
            this.createEntity.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createVariableEntityToolStripMenuItem,
            this.createFunctionEntityToolStripMenuItem,
            this.createCompositeEntityToolStripMenuItem,
            this.createProxyEntityToolStripMenuItem,
            this.createAliasToolStripMenuItem});
            this.createEntity.Image = ((System.Drawing.Image)(resources.GetObject("createEntity.Image")));
            this.createEntity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createEntity.Name = "createEntity";
            this.createEntity.Size = new System.Drawing.Size(103, 22);
            this.createEntity.Text = "Create Entity";
            // 
            // createVariableEntityToolStripMenuItem
            // 
            this.createVariableEntityToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createVariableEntityToolStripMenuItem.Image")));
            this.createVariableEntityToolStripMenuItem.Name = "createVariableEntityToolStripMenuItem";
            this.createVariableEntityToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createVariableEntityToolStripMenuItem.Text = "Create Parameter";
            this.createVariableEntityToolStripMenuItem.ToolTipText = "Creates an entity which acts as a parameter that can be accessed when instancing " +
    "this composite.";
            this.createVariableEntityToolStripMenuItem.Click += new System.EventHandler(this.createVariableEntityToolStripMenuItem_Click);
            // 
            // createFunctionEntityToolStripMenuItem
            // 
            this.createFunctionEntityToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createFunctionEntityToolStripMenuItem.Image")));
            this.createFunctionEntityToolStripMenuItem.Name = "createFunctionEntityToolStripMenuItem";
            this.createFunctionEntityToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createFunctionEntityToolStripMenuItem.Text = "Create Function";
            this.createFunctionEntityToolStripMenuItem.ToolTipText = "Create an entity that can execute a Cathode function.";
            this.createFunctionEntityToolStripMenuItem.Click += new System.EventHandler(this.createFunctionEntityToolStripMenuItem_Click);
            // 
            // createCompositeEntityToolStripMenuItem
            // 
            this.createCompositeEntityToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createCompositeEntityToolStripMenuItem.Image")));
            this.createCompositeEntityToolStripMenuItem.Name = "createCompositeEntityToolStripMenuItem";
            this.createCompositeEntityToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createCompositeEntityToolStripMenuItem.Text = "Create Instance of Composite";
            this.createCompositeEntityToolStripMenuItem.ToolTipText = "Create an entity that instances another composite.";
            this.createCompositeEntityToolStripMenuItem.Click += new System.EventHandler(this.createCompositeEntityToolStripMenuItem_Click);
            // 
            // createProxyEntityToolStripMenuItem
            // 
            this.createProxyEntityToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createProxyEntityToolStripMenuItem.Image")));
            this.createProxyEntityToolStripMenuItem.Name = "createProxyEntityToolStripMenuItem";
            this.createProxyEntityToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createProxyEntityToolStripMenuItem.Text = "Create Proxy";
            this.createProxyEntityToolStripMenuItem.ToolTipText = "Create an entity that acts as a proxy to an entity in another composite.";
            this.createProxyEntityToolStripMenuItem.Click += new System.EventHandler(this.createProxyEntityToolStripMenuItem_Click);
            // 
            // createAliasToolStripMenuItem
            // 
            this.createAliasToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createAliasToolStripMenuItem.Image")));
            this.createAliasToolStripMenuItem.Name = "createAliasToolStripMenuItem";
            this.createAliasToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createAliasToolStripMenuItem.Text = "Create Alias";
            this.createAliasToolStripMenuItem.Click += new System.EventHandler(this.createAliasToolStripMenuItem_Click);
            // 
            // exportComposite
            // 
            this.exportComposite.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.exportComposite.Image = ((System.Drawing.Image)(resources.GetObject("exportComposite.Image")));
            this.exportComposite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportComposite.Name = "exportComposite";
            this.exportComposite.Size = new System.Drawing.Size(110, 22);
            this.exportComposite.Text = "Port Composite";
            this.exportComposite.Click += new System.EventHandler(this.exportComposite_Click);
            // 
            // findUses
            // 
            this.findUses.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.findUses.Image = ((System.Drawing.Image)(resources.GetObject("findUses.Image")));
            this.findUses.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findUses.Name = "findUses";
            this.findUses.Size = new System.Drawing.Size(177, 22);
            this.findUses.Text = "Find Instances of Composite";
            this.findUses.Click += new System.EventHandler(this.findUses_Click);
            // 
            // deleteCheckedEntities
            // 
            this.deleteCheckedEntities.Image = ((System.Drawing.Image)(resources.GetObject("deleteCheckedEntities.Image")));
            this.deleteCheckedEntities.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteCheckedEntities.Name = "deleteCheckedEntities";
            this.deleteCheckedEntities.Size = new System.Drawing.Size(150, 22);
            this.deleteCheckedEntities.Text = "Delete Checked Entities";
            this.deleteCheckedEntities.Click += new System.EventHandler(this.deleteCheckedEntities_Click);
            // 
            // deleteComposite
            // 
            this.deleteComposite.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.deleteComposite.Image = ((System.Drawing.Image)(resources.GetObject("deleteComposite.Image")));
            this.deleteComposite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteComposite.Name = "deleteComposite";
            this.deleteComposite.Size = new System.Drawing.Size(121, 22);
            this.deleteComposite.Text = "Delete Composite";
            this.deleteComposite.Click += new System.EventHandler(this.deleteComposite_Click);
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
            this.dockPanel.Location = new System.Drawing.Point(0, 0);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Padding = new System.Windows.Forms.Padding(6);
            this.dockPanel.ShowAutoHideContentOnHover = false;
            this.dockPanel.Size = new System.Drawing.Size(781, 773);
            this.dockPanel.TabIndex = 178;
            this.dockPanel.Theme = this.vS2015BlueTheme1;
            this.dockPanel.Resize += new System.EventHandler(this.dockPanel_Resize);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.compositeEntityList1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 773);
            this.panel1.TabIndex = 180;
            // 
            // compositeEntityList1
            // 
            this.compositeEntityList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compositeEntityList1.Location = new System.Drawing.Point(0, 0);
            this.compositeEntityList1.Name = "compositeEntityList1";
            this.compositeEntityList1.Size = new System.Drawing.Size(830, 773);
            this.compositeEntityList1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dockPanel);
            this.splitContainer1.Size = new System.Drawing.Size(1615, 773);
            this.splitContainer1.SplitterDistance = 830;
            this.splitContainer1.TabIndex = 182;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "flag_blue");
            this.imageList.Images.SetKeyName(1, "flag_green");
            this.imageList.Images.SetKeyName(2, "flag_red");
            this.imageList.Images.SetKeyName(3, "behavior");
            this.imageList.Images.SetKeyName(4, "behavior_loaded");
            this.imageList.Images.SetKeyName(5, "behavior_modified");
            this.imageList.Images.SetKeyName(6, "condition");
            this.imageList.Images.SetKeyName(7, "impulse");
            this.imageList.Images.SetKeyName(8, "action");
            this.imageList.Images.SetKeyName(9, "decorator");
            this.imageList.Images.SetKeyName(10, "sequence");
            this.imageList.Images.SetKeyName(11, "selector");
            this.imageList.Images.SetKeyName(12, "parallel");
            this.imageList.Images.SetKeyName(13, "folder_closed");
            this.imageList.Images.SetKeyName(14, "folder_open");
            this.imageList.Images.SetKeyName(15, "event");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeSelected});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 26);
            // 
            // closeSelected
            // 
            this.closeSelected.Image = ((System.Drawing.Image)(resources.GetObject("closeSelected.Image")));
            this.closeSelected.Name = "closeSelected";
            this.closeSelected.Size = new System.Drawing.Size(164, 22);
            this.closeSelected.Text = "Close Composite";
            this.closeSelected.Click += new System.EventHandler(this.closeSelected_Click);
            // 
            // goBackOnPath
            // 
            this.goBackOnPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.goBackOnPath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.goBackOnPath.Location = new System.Drawing.Point(3, 801);
            this.goBackOnPath.Name = "goBackOnPath";
            this.goBackOnPath.Size = new System.Drawing.Size(62, 20);
            this.goBackOnPath.TabIndex = 177;
            this.goBackOnPath.Text = "< Back";
            this.goBackOnPath.UseVisualStyleBackColor = true;
            this.goBackOnPath.Click += new System.EventHandler(this.goBackOnPath_Click);
            // 
            // pathDisplay
            // 
            this.pathDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pathDisplay.Enabled = false;
            this.pathDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathDisplay.Location = new System.Drawing.Point(64, 801);
            this.pathDisplay.Name = "pathDisplay";
            this.pathDisplay.ReadOnly = true;
            this.pathDisplay.Size = new System.Drawing.Size(1406, 20);
            this.pathDisplay.TabIndex = 177;
            // 
            // instanceInfo
            // 
            this.instanceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.instanceInfo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.instanceInfo.Location = new System.Drawing.Point(1469, 801);
            this.instanceInfo.Name = "instanceInfo";
            this.instanceInfo.Size = new System.Drawing.Size(146, 20);
            this.instanceInfo.TabIndex = 183;
            this.instanceInfo.Text = "Composite Instance Info";
            this.instanceInfo.UseVisualStyleBackColor = true;
            this.instanceInfo.Click += new System.EventHandler(this.instanceInfo_Click);
            // 
            // EntityListContextMenu
            // 
            this.EntityListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.duplicateToolStripMenuItem});
            this.EntityListContextMenu.Name = "EntityListContextMenu";
            this.EntityListContextMenu.Size = new System.Drawing.Size(125, 98);
            this.EntityListContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.EntityListContextMenu_Opening);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createParameterToolStripMenuItem,
            this.createFunctionToolStripMenuItem,
            this.createInstanceOfCompositeToolStripMenuItem,
            this.createProxyToolStripMenuItem,
            this.createAliasToolStripMenuItem1});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // createParameterToolStripMenuItem
            // 
            this.createParameterToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createParameterToolStripMenuItem.Image")));
            this.createParameterToolStripMenuItem.Name = "createParameterToolStripMenuItem";
            this.createParameterToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createParameterToolStripMenuItem.Text = "Create Parameter";
            this.createParameterToolStripMenuItem.Click += new System.EventHandler(this.createParameterToolStripMenuItem_Click);
            // 
            // createFunctionToolStripMenuItem
            // 
            this.createFunctionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createFunctionToolStripMenuItem.Image")));
            this.createFunctionToolStripMenuItem.Name = "createFunctionToolStripMenuItem";
            this.createFunctionToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createFunctionToolStripMenuItem.Text = "Create Function";
            this.createFunctionToolStripMenuItem.Click += new System.EventHandler(this.createFunctionToolStripMenuItem_Click);
            // 
            // createInstanceOfCompositeToolStripMenuItem
            // 
            this.createInstanceOfCompositeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createInstanceOfCompositeToolStripMenuItem.Image")));
            this.createInstanceOfCompositeToolStripMenuItem.Name = "createInstanceOfCompositeToolStripMenuItem";
            this.createInstanceOfCompositeToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createInstanceOfCompositeToolStripMenuItem.Text = "Create Instance of Composite";
            this.createInstanceOfCompositeToolStripMenuItem.Click += new System.EventHandler(this.createInstanceOfCompositeToolStripMenuItem_Click);
            // 
            // createProxyToolStripMenuItem
            // 
            this.createProxyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createProxyToolStripMenuItem.Image")));
            this.createProxyToolStripMenuItem.Name = "createProxyToolStripMenuItem";
            this.createProxyToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createProxyToolStripMenuItem.Text = "Create Proxy";
            this.createProxyToolStripMenuItem.Click += new System.EventHandler(this.createProxyToolStripMenuItem_Click);
            // 
            // createAliasToolStripMenuItem1
            // 
            this.createAliasToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("createAliasToolStripMenuItem1.Image")));
            this.createAliasToolStripMenuItem1.Name = "createAliasToolStripMenuItem1";
            this.createAliasToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.createAliasToolStripMenuItem1.Text = "Create Alias";
            this.createAliasToolStripMenuItem1.Click += new System.EventHandler(this.createAliasToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameToolStripMenuItem.Image")));
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("duplicateToolStripMenuItem.Image")));
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.duplicateToolStripMenuItem.Text = "Duplicate";
            this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
            // 
            // renameComposite
            // 
            this.renameComposite.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.renameComposite.Image = ((System.Drawing.Image)(resources.GetObject("renameComposite.Image")));
            this.renameComposite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.renameComposite.Name = "renameComposite";
            this.renameComposite.Size = new System.Drawing.Size(131, 22);
            this.renameComposite.Text = "Rename Composite";
            this.renameComposite.Click += new System.EventHandler(this.renameComposite_Click);
            // 
            // CompositeDisplay
            // 
            this.AllowEndUserDocking = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1615, 821);
            this.Controls.Add(this.instanceInfo);
            this.Controls.Add(this.pathDisplay);
            this.Controls.Add(this.goBackOnPath);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompositeDisplay";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.TabPageContextMenuStrip = this.contextMenuStrip1;
            this.Text = "Selected Composite";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.EntityListContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        private System.Windows.Forms.ToolStripButton findUses;
        private System.Windows.Forms.ToolStripButton deleteComposite;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.Panel panel1;
        private WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme vS2015BlueTheme1;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton deleteCheckedEntities;
        private System.Windows.Forms.ToolStripButton exportComposite;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripDropDownButton createEntity;
        private System.Windows.Forms.ToolStripMenuItem createVariableEntityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFunctionEntityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createCompositeEntityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createProxyEntityToolStripMenuItem;
        private System.Windows.Forms.Button goBackOnPath;
        private System.Windows.Forms.TextBox pathDisplay;
        private System.Windows.Forms.ToolStripMenuItem closeSelected;
        private System.Windows.Forms.ImageList entityListIcons;
        private System.Windows.Forms.ToolStripMenuItem createAliasToolStripMenuItem;
        private System.Windows.Forms.Button instanceInfo;
        private System.Windows.Forms.ContextMenuStrip EntityListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createParameterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFunctionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createInstanceOfCompositeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createProxyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createAliasToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private Popups.UserControls.CompositeEntityList compositeEntityList1;
        private System.Windows.Forms.ToolStripButton renameComposite;
    }
}