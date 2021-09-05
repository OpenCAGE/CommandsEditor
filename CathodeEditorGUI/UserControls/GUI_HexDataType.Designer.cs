namespace CathodeEditorGUI.UserControls
{
    partial class GUI_HexDataType
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
            this.GUID_VARIABLE_DUMMY = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.GUID_VARIABLE_DUMMY.SuspendLayout();
            this.SuspendLayout();
            // 
            // GUID_VARIABLE_DUMMY
            // 
            this.GUID_VARIABLE_DUMMY.Controls.Add(this.textBox4);
            this.GUID_VARIABLE_DUMMY.Controls.Add(this.textBox5);
            this.GUID_VARIABLE_DUMMY.Controls.Add(this.textBox3);
            this.GUID_VARIABLE_DUMMY.Controls.Add(this.textBox2);
            this.GUID_VARIABLE_DUMMY.Location = new System.Drawing.Point(3, 3);
            this.GUID_VARIABLE_DUMMY.Name = "GUID_VARIABLE_DUMMY";
            this.GUID_VARIABLE_DUMMY.Size = new System.Drawing.Size(700, 56);
            this.GUID_VARIABLE_DUMMY.TabIndex = 19;
            this.GUID_VARIABLE_DUMMY.TabStop = false;
            this.GUID_VARIABLE_DUMMY.Text = "Parameter Name (00-00-00-00)";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(126, 21);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(31, 20);
            this.textBox4.TabIndex = 12;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(89, 21);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(31, 20);
            this.textBox5.TabIndex = 11;
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(52, 21);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(31, 20);
            this.textBox3.TabIndex = 10;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(15, 21);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(31, 20);
            this.textBox2.TabIndex = 9;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // GUI_HexDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GUID_VARIABLE_DUMMY);
            this.Name = "GUI_HexDataType";
            this.Size = new System.Drawing.Size(707, 61);
            this.GUID_VARIABLE_DUMMY.ResumeLayout(false);
            this.GUID_VARIABLE_DUMMY.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GUID_VARIABLE_DUMMY;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
    }
}
