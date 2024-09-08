namespace CommandsEditor.DockPanels
{
    partial class EntityList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityList));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.compositeEntityList1 = new Popups.UserControls.CompositeEntityList();
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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EntityListContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "GenericEditor.ico");
            // 
            // compositeEntityList1
            // 
            this.compositeEntityList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compositeEntityList1.Location = new System.Drawing.Point(0, 0);
            this.compositeEntityList1.Name = "compositeEntityList1";
            this.compositeEntityList1.Size = new System.Drawing.Size(479, 780);
            this.compositeEntityList1.TabIndex = 1;
            // 
            // EntityListContextMenu
            // 
            this.EntityListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.toolStripSeparator2,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.EntityListContextMenu.Name = "EntityListContextMenu";
            this.EntityListContextMenu.Size = new System.Drawing.Size(181, 170);
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
            this.createToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // createParameterToolStripMenuItem
            // 
            this.createParameterToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createParameterToolStripMenuItem.Image")));
            this.createParameterToolStripMenuItem.Name = "createParameterToolStripMenuItem";
            this.createParameterToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createParameterToolStripMenuItem.Text = "Create Parameter";
            // 
            // createFunctionToolStripMenuItem
            // 
            this.createFunctionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createFunctionToolStripMenuItem.Image")));
            this.createFunctionToolStripMenuItem.Name = "createFunctionToolStripMenuItem";
            this.createFunctionToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createFunctionToolStripMenuItem.Text = "Create Function";
            // 
            // createInstanceOfCompositeToolStripMenuItem
            // 
            this.createInstanceOfCompositeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createInstanceOfCompositeToolStripMenuItem.Image")));
            this.createInstanceOfCompositeToolStripMenuItem.Name = "createInstanceOfCompositeToolStripMenuItem";
            this.createInstanceOfCompositeToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createInstanceOfCompositeToolStripMenuItem.Text = "Create Instance of Composite";
            // 
            // createProxyToolStripMenuItem
            // 
            this.createProxyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("createProxyToolStripMenuItem.Image")));
            this.createProxyToolStripMenuItem.Name = "createProxyToolStripMenuItem";
            this.createProxyToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.createProxyToolStripMenuItem.Text = "Create Proxy";
            // 
            // createAliasToolStripMenuItem1
            // 
            this.createAliasToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("createAliasToolStripMenuItem1.Image")));
            this.createAliasToolStripMenuItem1.Name = "createAliasToolStripMenuItem1";
            this.createAliasToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.createAliasToolStripMenuItem1.Text = "Create Alias";
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
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameToolStripMenuItem.Image")));
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("duplicateToolStripMenuItem.Image")));
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.duplicateToolStripMenuItem.Text = "Duplicate";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Visible = false;
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Visible = false;
            // 
            // EntityList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 780);
            this.Controls.Add(this.compositeEntityList1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EntityList";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.Text = "Entities";
            this.EntityListContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private Popups.UserControls.CompositeEntityList compositeEntityList1;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    }
}