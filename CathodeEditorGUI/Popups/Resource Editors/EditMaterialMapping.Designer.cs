namespace CommandsEditor
{
    partial class EditMaterialMapping
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditMaterialMapping));
            this.materialMappingTreeView = new System.Windows.Forms.TreeView();
            this.mappingsListView = new System.Windows.Forms.ListView();
            this.columnFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.selectButton = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialMappingTreeView
            // 
            this.materialMappingTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialMappingTreeView.Location = new System.Drawing.Point(0, 0);
            this.materialMappingTreeView.Name = "materialMappingTreeView";
            this.materialMappingTreeView.Size = new System.Drawing.Size(277, 450);
            this.materialMappingTreeView.TabIndex = 0;
            this.materialMappingTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.materialMappingTreeView_AfterSelect);
            // 
            // mappingsListView
            // 
            this.mappingsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFrom,
            this.columnTo});
            this.mappingsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mappingsListView.FullRowSelect = true;
            this.mappingsListView.GridLines = true;
            this.mappingsListView.HideSelection = false;
            this.mappingsListView.Location = new System.Drawing.Point(0, 0);
            this.mappingsListView.Name = "mappingsListView";
            this.mappingsListView.Size = new System.Drawing.Size(704, 423);
            this.mappingsListView.TabIndex = 0;
            this.mappingsListView.UseCompatibleStateImageBehavior = false;
            this.mappingsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnFrom
            // 
            this.columnFrom.Text = "From";
            this.columnFrom.Width = 340;
            // 
            // columnTo
            // 
            this.columnTo.Text = "To";
            this.columnTo.Width = 340;
            // 
            // selectButton
            // 
            this.selectButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.selectButton.Location = new System.Drawing.Point(0, 423);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(704, 27);
            this.selectButton.TabIndex = 1;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.materialMappingTreeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.mappingsListView);
            this.splitContainer.Panel2.Controls.Add(this.selectButton);
            this.splitContainer.Size = new System.Drawing.Size(985, 450);
            this.splitContainer.SplitterDistance = 277;
            this.splitContainer.TabIndex = 0;
            // 
            // EditMaterialMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 450);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditMaterialMapping";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Material Mapping";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView materialMappingTreeView;
        private System.Windows.Forms.ListView mappingsListView;
        private System.Windows.Forms.ColumnHeader columnFrom;
        private System.Windows.Forms.ColumnHeader columnTo;
        private System.Windows.Forms.Button selectButton;
    }
}

