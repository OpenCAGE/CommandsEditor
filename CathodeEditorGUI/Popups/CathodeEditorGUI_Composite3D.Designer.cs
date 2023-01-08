namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_Composite3D
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
            this.modelRendererHost = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // modelRendererHost
            // 
            this.modelRendererHost.Location = new System.Drawing.Point(12, 12);
            this.modelRendererHost.Name = "modelRendererHost";
            this.modelRendererHost.Size = new System.Drawing.Size(673, 597);
            this.modelRendererHost.TabIndex = 1;
            this.modelRendererHost.Text = "elementHost1";
            this.modelRendererHost.Child = null;
            // 
            // CathodeEditorGUI_Composite3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 619);
            this.Controls.Add(this.modelRendererHost);
            this.Name = "CathodeEditorGUI_Composite3D";
            this.Text = "CathodeEditorGUI_Composite3D";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost modelRendererHost;
    }
}