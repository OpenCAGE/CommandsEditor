namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_SelectModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_SelectModel));
            this.modelRendererHost = new System.Windows.Forms.Integration.ElementHost();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // modelRendererHost
            // 
            this.modelRendererHost.Location = new System.Drawing.Point(444, 12);
            this.modelRendererHost.Name = "modelRendererHost";
            this.modelRendererHost.Size = new System.Drawing.Size(673, 580);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(444, 596);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 101;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(444, 615);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 102;
            this.label2.Text = "label2";
            // 
            // CathodeEditorGUI_SelectModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 640);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileTree);
            this.Controls.Add(this.modelRendererHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CathodeEditorGUI_SelectModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Model";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost modelRendererHost;
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}