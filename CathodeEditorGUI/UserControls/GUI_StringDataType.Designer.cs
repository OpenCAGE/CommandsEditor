namespace CathodeEditorGUI.UserControls
{
    partial class GUI_StringDataType
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
            this.STRING_VARIABLE_DUMMY = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.STRING_VARIABLE_DUMMY.SuspendLayout();
            this.SuspendLayout();
            // 
            // STRING_VARIABLE_DUMMY
            // 
            this.STRING_VARIABLE_DUMMY.Controls.Add(this.textBox1);
            this.STRING_VARIABLE_DUMMY.Location = new System.Drawing.Point(3, 3);
            this.STRING_VARIABLE_DUMMY.Name = "STRING_VARIABLE_DUMMY";
            this.STRING_VARIABLE_DUMMY.Size = new System.Drawing.Size(334, 56);
            this.STRING_VARIABLE_DUMMY.TabIndex = 17;
            this.STRING_VARIABLE_DUMMY.TabStop = false;
            this.STRING_VARIABLE_DUMMY.Text = "Parameter Name (00-00-00-00)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(322, 20);
            this.textBox1.TabIndex = 8;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // GUI_StringDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.STRING_VARIABLE_DUMMY);
            this.Name = "GUI_StringDataType";
            this.Size = new System.Drawing.Size(340, 61);
            this.STRING_VARIABLE_DUMMY.ResumeLayout(false);
            this.STRING_VARIABLE_DUMMY.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox STRING_VARIABLE_DUMMY;
        private System.Windows.Forms.TextBox textBox1;
    }
}
