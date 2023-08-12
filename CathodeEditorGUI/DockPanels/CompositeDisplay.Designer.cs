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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeDisplay));
            this.FileTree = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.createComposite = new System.Windows.Forms.ToolStripButton();
            this.removeSelected = new System.Windows.Forms.ToolStripButton();
            this.findUsesOfSelected = new System.Windows.Forms.ToolStripButton();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileTree
            // 
            this.FileTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileTree.Location = new System.Drawing.Point(0, 28);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(833, 706);
            this.FileTree.TabIndex = 153;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createComposite,
            this.findUsesOfSelected,
            this.removeSelected});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(833, 25);
            this.toolStrip1.TabIndex = 157;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // createComposite
            // 
            this.createComposite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.createComposite.Image = ((System.Drawing.Image)(resources.GetObject("createComposite.Image")));
            this.createComposite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createComposite.Name = "createComposite";
            this.createComposite.Size = new System.Drawing.Size(106, 22);
            this.createComposite.Text = "Create Composite";
            this.createComposite.Click += new System.EventHandler(this.createComposite_Click);
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
            // findUsesOfSelected
            // 
            this.findUsesOfSelected.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.findUsesOfSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.findUsesOfSelected.Image = ((System.Drawing.Image)(resources.GetObject("findUsesOfSelected.Image")));
            this.findUsesOfSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findUsesOfSelected.Name = "findUsesOfSelected";
            this.findUsesOfSelected.Size = new System.Drawing.Size(122, 22);
            this.findUsesOfSelected.Text = "Find Uses of Selected";
            this.findUsesOfSelected.Click += new System.EventHandler(this.findUsesOfSelected_Click);
            // 
            // CompositeDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 734);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.FileTree);
            this.Name = "CompositeDisplay";
            this.Text = "CompositeDisplay";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton createComposite;
        private System.Windows.Forms.ToolStripButton removeSelected;
        private System.Windows.Forms.ToolStripButton findUsesOfSelected;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
    }
}