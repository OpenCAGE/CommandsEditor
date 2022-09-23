namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_EditMVR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_EditMVR));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.transform = new UserControls.GUI_TransformDataType();
            this.renderable = new Popups.UserControls.GUI_Resource_RenderableInstance();
            this.visible = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(437, 680);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // transform
            // 
            this.transform.Location = new System.Drawing.Point(458, 204);
            this.transform.Name = "transform";
            this.transform.Size = new System.Drawing.Size(340, 113);
            this.transform.TabIndex = 1;
            // 
            // renderable
            // 
            this.renderable.Location = new System.Drawing.Point(458, 12);
            this.renderable.Name = "renderable";
            this.renderable.Size = new System.Drawing.Size(838, 186);
            this.renderable.TabIndex = 2;
            // 
            // visible
            // 
            this.visible.AutoSize = true;
            this.visible.Location = new System.Drawing.Point(823, 227);
            this.visible.Name = "visible";
            this.visible.Size = new System.Drawing.Size(67, 17);
            this.visible.TabIndex = 3;
            this.visible.Text = "Is Visible";
            this.visible.UseVisualStyleBackColor = true;
            // 
            // CathodeEditorGUI_EditMVR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1308, 704);
            this.Controls.Add(this.visible);
            this.Controls.Add(this.renderable);
            this.Controls.Add(this.transform);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CathodeEditorGUI_EditMVR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Mover Descriptors";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private UserControls.GUI_TransformDataType transform;
        private Popups.UserControls.GUI_Resource_RenderableInstance renderable;
        private System.Windows.Forms.CheckBox visible;
    }
}