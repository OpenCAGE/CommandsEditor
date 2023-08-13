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
            System.Windows.Forms.ListViewGroup listViewGroup13 = new System.Windows.Forms.ListViewGroup("Variables", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup14 = new System.Windows.Forms.ListViewGroup("Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup15 = new System.Windows.Forms.ListViewGroup("Proxies", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup16 = new System.Windows.Forms.ListViewGroup("Overrides", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeDisplay));
            this.composite_content = new System.Windows.Forms.ListView();
            this.EntityName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EntityType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.entity_search_box = new System.Windows.Forms.TextBox();
            this.entity_search_btn = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.createEntity = new System.Windows.Forms.ToolStripButton();
            this.findUses = new System.Windows.Forms.ToolStripButton();
            this.deleteComposite = new System.Windows.Forms.ToolStripButton();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.vS2015BlueTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            listViewGroup13.Header = "Variables";
            listViewGroup13.Name = "Variables";
            listViewGroup14.Header = "Functions";
            listViewGroup14.Name = "Functions";
            listViewGroup15.Header = "Proxies";
            listViewGroup15.Name = "Proxies";
            listViewGroup16.Header = "Overrides";
            listViewGroup16.Name = "Overrides";
            this.composite_content.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup13,
            listViewGroup14,
            listViewGroup15,
            listViewGroup16});
            this.composite_content.HideSelection = false;
            this.composite_content.LabelWrap = false;
            this.composite_content.Location = new System.Drawing.Point(0, 30);
            this.composite_content.MultiSelect = false;
            this.composite_content.Name = "composite_content";
            this.composite_content.Size = new System.Drawing.Size(640, 760);
            this.composite_content.TabIndex = 176;
            this.composite_content.UseCompatibleStateImageBehavior = false;
            this.composite_content.View = System.Windows.Forms.View.Details;
            this.composite_content.SelectedIndexChanged += new System.EventHandler(this.composite_content_SelectedIndexChanged);
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
            this.entity_search_box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.entity_search_box.Location = new System.Drawing.Point(0, 4);
            this.entity_search_box.Name = "entity_search_box";
            this.entity_search_box.Size = new System.Drawing.Size(579, 20);
            this.entity_search_box.TabIndex = 146;
            // 
            // entity_search_btn
            // 
            this.entity_search_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.entity_search_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.entity_search_btn.Location = new System.Drawing.Point(577, 4);
            this.entity_search_btn.Name = "entity_search_btn";
            this.entity_search_btn.Size = new System.Drawing.Size(63, 20);
            this.entity_search_btn.TabIndex = 145;
            this.entity_search_btn.Text = "Search";
            this.entity_search_btn.UseVisualStyleBackColor = true;
            this.entity_search_btn.Click += new System.EventHandler(this.entity_search_btn_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createEntity,
            this.findUses,
            this.deleteComposite});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1615, 25);
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
            // findUses
            // 
            this.findUses.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.findUses.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.findUses.Image = ((System.Drawing.Image)(resources.GetObject("findUses.Image")));
            this.findUses.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findUses.Name = "findUses";
            this.findUses.Size = new System.Drawing.Size(136, 22);
            this.findUses.Text = "Find Uses of Composite";
            this.findUses.Click += new System.EventHandler(this.findUses_Click);
            // 
            // deleteComposite
            // 
            this.deleteComposite.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.deleteComposite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteComposite.Image = ((System.Drawing.Image)(resources.GetObject("deleteComposite.Image")));
            this.deleteComposite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteComposite.Name = "deleteComposite";
            this.deleteComposite.Size = new System.Drawing.Size(105, 22);
            this.deleteComposite.Text = "Delete Composite";
            this.deleteComposite.Click += new System.EventHandler(this.deleteComposite_Click);
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
            this.dockPanel.Location = new System.Drawing.Point(649, 3);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Padding = new System.Windows.Forms.Padding(6);
            this.dockPanel.ShowAutoHideContentOnHover = false;
            this.dockPanel.Size = new System.Drawing.Size(963, 790);
            this.dockPanel.TabIndex = 178;
            this.dockPanel.Theme = this.vS2015BlueTheme1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.composite_content);
            this.panel1.Controls.Add(this.entity_search_box);
            this.panel1.Controls.Add(this.entity_search_btn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 790);
            this.panel1.TabIndex = 180;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dockPanel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1615, 796);
            this.tableLayoutPanel1.TabIndex = 181;
            // 
            // CompositeDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1615, 821);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CompositeDisplay";
            this.Text = "Selected Composite";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
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
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        private System.Windows.Forms.ToolStripButton findUses;
        private System.Windows.Forms.ToolStripButton deleteComposite;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme vS2015BlueTheme1;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
    }
}