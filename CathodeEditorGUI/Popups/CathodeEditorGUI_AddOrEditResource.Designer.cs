namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_AddOrEditResource
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_AddOrEditResource));
            this.resource_panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // resource_panel
            // 
            this.resource_panel.AutoScroll = true;
            this.resource_panel.Location = new System.Drawing.Point(12, 12);
            this.resource_panel.Name = "resource_panel";
            this.resource_panel.Size = new System.Drawing.Size(915, 572);
            this.resource_panel.TabIndex = 1;
            // 
            // CathodeEditorGUI_AddOrEditResource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 596);
            this.Controls.Add(this.resource_panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CathodeEditorGUI_AddOrEditResource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resource Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel resource_panel;
    }
}