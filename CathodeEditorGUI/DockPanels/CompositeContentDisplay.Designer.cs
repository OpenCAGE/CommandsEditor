namespace CommandsEditor.DockPanels
{
    partial class CompositeContentDisplay
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Variables", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Proxies", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Overrides", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeContentDisplay));
            this.composite_content = new System.Windows.Forms.ListView();
            this.EntityName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EntityType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.entity_search_box = new System.Windows.Forms.TextBox();
            this.entity_search_btn = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.createEntity = new System.Windows.Forms.ToolStripButton();
            this.removeSelected = new System.Windows.Forms.ToolStripButton();
            this.duplicateSelected = new System.Windows.Forms.ToolStripButton();
            this.renameSelected = new System.Windows.Forms.ToolStripButton();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // composite_content
            // 
            this.composite_content.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.composite_content.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.EntityName,
            this.EntityType});
            this.composite_content.FullRowSelect = true;
            listViewGroup1.Header = "Variables";
            listViewGroup1.Name = "Variables";
            listViewGroup2.Header = "Functions";
            listViewGroup2.Name = "Functions";
            listViewGroup3.Header = "Proxies";
            listViewGroup3.Name = "Proxies";
            listViewGroup4.Header = "Overrides";
            listViewGroup4.Name = "Overrides";
            this.composite_content.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.composite_content.HideSelection = false;
            this.composite_content.LabelWrap = false;
            this.composite_content.Location = new System.Drawing.Point(0, 54);
            this.composite_content.MultiSelect = false;
            this.composite_content.Name = "composite_content";
            this.composite_content.Size = new System.Drawing.Size(630, 647);
            this.composite_content.TabIndex = 176;
            this.composite_content.UseCompatibleStateImageBehavior = false;
            this.composite_content.View = System.Windows.Forms.View.Details;
            // 
            // EntityName
            // 
            this.EntityName.Text = "Name";
            this.EntityName.Width = 279;
            // 
            // EntityType
            // 
            this.EntityType.Text = "Type";
            this.EntityType.Width = 163;
            // 
            // entity_search_box
            // 
            this.entity_search_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entity_search_box.Location = new System.Drawing.Point(0, 28);
            this.entity_search_box.Name = "entity_search_box";
            this.entity_search_box.Size = new System.Drawing.Size(570, 20);
            this.entity_search_box.TabIndex = 146;
            // 
            // entity_search_btn
            // 
            this.entity_search_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.entity_search_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.entity_search_btn.Location = new System.Drawing.Point(567, 28);
            this.entity_search_btn.Name = "entity_search_btn";
            this.entity_search_btn.Size = new System.Drawing.Size(63, 20);
            this.entity_search_btn.TabIndex = 145;
            this.entity_search_btn.Text = "Search";
            this.entity_search_btn.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createEntity,
            this.removeSelected,
            this.duplicateSelected,
            this.renameSelected});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(630, 25);
            this.toolStrip1.TabIndex = 177;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // createEntity
            // 
            this.createEntity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.createEntity.Image = ((System.Drawing.Image)(resources.GetObject("createEntity.Image")));
            this.createEntity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createEntity.Name = "createEntity";
            this.createEntity.Size = new System.Drawing.Size(78, 22);
            this.createEntity.Text = "Create Entity";
            this.createEntity.Click += new System.EventHandler(this.createEntity_Click);
            // 
            // removeSelected
            // 
            this.removeSelected.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.removeSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.removeSelected.Image = ((System.Drawing.Image)(resources.GetObject("removeSelected.Image")));
            this.removeSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeSelected.Name = "removeSelected";
            this.removeSelected.Size = new System.Drawing.Size(101, 22);
            this.removeSelected.Text = "Remove Selected";
            this.removeSelected.Click += new System.EventHandler(this.removeSelected_Click);
            // 
            // duplicateSelected
            // 
            this.duplicateSelected.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.duplicateSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.duplicateSelected.Image = ((System.Drawing.Image)(resources.GetObject("duplicateSelected.Image")));
            this.duplicateSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.duplicateSelected.Name = "duplicateSelected";
            this.duplicateSelected.Size = new System.Drawing.Size(108, 22);
            this.duplicateSelected.Text = "Duplicate Selected";
            this.duplicateSelected.Click += new System.EventHandler(this.duplicateSelected_Click);
            // 
            // renameSelected
            // 
            this.renameSelected.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.renameSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.renameSelected.Image = ((System.Drawing.Image)(resources.GetObject("renameSelected.Image")));
            this.renameSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.renameSelected.Name = "renameSelected";
            this.renameSelected.Size = new System.Drawing.Size(101, 22);
            this.renameSelected.Text = "Rename Selected";
            this.renameSelected.Click += new System.EventHandler(this.renameSelected_Click);
            // 
            // CompositeContentDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 699);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.composite_content);
            this.Controls.Add(this.entity_search_btn);
            this.Controls.Add(this.entity_search_box);
            this.Name = "CompositeContentDisplay";
            this.Text = "CompositeContentDisplay";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView composite_content;
        private System.Windows.Forms.ColumnHeader EntityName;
        private System.Windows.Forms.ColumnHeader EntityType;
        private System.Windows.Forms.TextBox entity_search_box;
        private System.Windows.Forms.Button entity_search_btn;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton createEntity;
        private System.Windows.Forms.ToolStripButton removeSelected;
        private System.Windows.Forms.ToolStripButton duplicateSelected;
        private System.Windows.Forms.ToolStripButton renameSelected;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
    }
}