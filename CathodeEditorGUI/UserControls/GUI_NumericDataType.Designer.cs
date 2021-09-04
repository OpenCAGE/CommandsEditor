namespace CathodeEditorGUI.UserControls
{
    partial class GUI_NumericDataType
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
            this.NUMERIC_VARIABLE_DUMMY = new System.Windows.Forms.GroupBox();
            this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.NUMERIC_VARIABLE_DUMMY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
            this.SuspendLayout();
            // 
            // NUMERIC_VARIABLE_DUMMY
            // 
            this.NUMERIC_VARIABLE_DUMMY.Controls.Add(this.textBox1);
            this.NUMERIC_VARIABLE_DUMMY.Controls.Add(this.numericUpDown7);
            this.NUMERIC_VARIABLE_DUMMY.Location = new System.Drawing.Point(3, 3);
            this.NUMERIC_VARIABLE_DUMMY.Name = "NUMERIC_VARIABLE_DUMMY";
            this.NUMERIC_VARIABLE_DUMMY.Size = new System.Drawing.Size(700, 56);
            this.NUMERIC_VARIABLE_DUMMY.TabIndex = 18;
            this.NUMERIC_VARIABLE_DUMMY.TabStop = false;
            this.NUMERIC_VARIABLE_DUMMY.Text = "Parameter Name (00-00-00-00)";
            // 
            // numericUpDown7
            // 
            this.numericUpDown7.Location = new System.Drawing.Point(15, 21);
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.Size = new System.Drawing.Size(664, 20);
            this.numericUpDown7.TabIndex = 9;
            this.numericUpDown7.ValueChanged += new System.EventHandler(this.numericUpDown7_ValueChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(664, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // GUI_NumericDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NUMERIC_VARIABLE_DUMMY);
            this.Name = "GUI_NumericDataType";
            this.Size = new System.Drawing.Size(707, 61);
            this.NUMERIC_VARIABLE_DUMMY.ResumeLayout(false);
            this.NUMERIC_VARIABLE_DUMMY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox NUMERIC_VARIABLE_DUMMY;
        private System.Windows.Forms.NumericUpDown numericUpDown7;
        private System.Windows.Forms.TextBox textBox1;
    }
}
