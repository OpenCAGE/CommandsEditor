namespace CommandsEditor
{
    partial class SelectModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectModel));
            this.modelRendererHost = new System.Windows.Forms.Integration.ElementHost();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.modelPreviewArea = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectModelBtn = new System.Windows.Forms.Button();
            this.modelPreviewArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelRendererHost
            // 
            this.modelRendererHost.Location = new System.Drawing.Point(6, 19);
            this.modelRendererHost.Name = "modelRendererHost";
            this.modelRendererHost.Size = new System.Drawing.Size(673, 597);
            this.modelRendererHost.TabIndex = 0;
            this.modelRendererHost.Text = "elementHost1";
            this.modelRendererHost.Child = null;
            // 
            // FileTree
            // 
            this.FileTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileTree.Location = new System.Drawing.Point(12, 12);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(366, 616);
            this.FileTree.TabIndex = 100;
            this.FileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTree_AfterSelect);
            // 
            // modelPreviewArea
            // 
            this.modelPreviewArea.Controls.Add(this.label1);
            this.modelPreviewArea.Controls.Add(this.selectModelBtn);
            this.modelPreviewArea.Controls.Add(this.modelRendererHost);
            this.modelPreviewArea.Location = new System.Drawing.Point(384, 6);
            this.modelPreviewArea.Name = "modelPreviewArea";
            this.modelPreviewArea.Size = new System.Drawing.Size(685, 622);
            this.modelPreviewArea.TabIndex = 103;
            this.modelPreviewArea.TabStop = false;
            this.modelPreviewArea.Text = "groupBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 564);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 52);
            this.label1.TabIndex = 2;
            this.label1.Text = "[Zoom] = Scrollwheel\r\n[Rotate] = Right Mouse\r\n[Drag] = Middle Mouse\r\n[Recenter] =" +
    " Home";
            // 
            // selectModelBtn
            // 
            this.selectModelBtn.Location = new System.Drawing.Point(536, 572);
            this.selectModelBtn.Name = "selectModelBtn";
            this.selectModelBtn.Size = new System.Drawing.Size(134, 35);
            this.selectModelBtn.TabIndex = 1;
            this.selectModelBtn.Text = "Select This Model";
            this.selectModelBtn.UseVisualStyleBackColor = true;
            this.selectModelBtn.Click += new System.EventHandler(this.selectModel_Click);
            // 
            // SelectModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 635);
            this.Controls.Add(this.modelPreviewArea);
            this.Controls.Add(this.FileTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SelectModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Model";
            this.modelPreviewArea.ResumeLayout(false);
            this.modelPreviewArea.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost modelRendererHost;
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.GroupBox modelPreviewArea;
        private System.Windows.Forms.Button selectModelBtn;
        private System.Windows.Forms.Label label1;
    }
}