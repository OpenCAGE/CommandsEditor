namespace CathodeEditorGUI.UserControls
{
    partial class GUI_BoolDataType
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BOOL_VARIABLE_DUMMY = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.BOOL_VARIABLE_DUMMY.SuspendLayout();
            this.SuspendLayout();
            // 
            // BOOL_VARIABLE_DUMMY
            // 
            this.BOOL_VARIABLE_DUMMY.Controls.Add(this.checkBox1);
            this.BOOL_VARIABLE_DUMMY.Location = new System.Drawing.Point(3, 3);
            this.BOOL_VARIABLE_DUMMY.Name = "BOOL_VARIABLE_DUMMY";
            this.BOOL_VARIABLE_DUMMY.Size = new System.Drawing.Size(334, 56);
            this.BOOL_VARIABLE_DUMMY.TabIndex = 18;
            this.BOOL_VARIABLE_DUMMY.TabStop = false;
            this.BOOL_VARIABLE_DUMMY.Text = "Parameter Name (00-00-00-00)";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 24);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Enabled";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // GUI_BoolDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BOOL_VARIABLE_DUMMY);
            this.Name = "GUI_BoolDataType";
            this.Size = new System.Drawing.Size(340, 61);
            this.BOOL_VARIABLE_DUMMY.ResumeLayout(false);
            this.BOOL_VARIABLE_DUMMY.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox BOOL_VARIABLE_DUMMY;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
