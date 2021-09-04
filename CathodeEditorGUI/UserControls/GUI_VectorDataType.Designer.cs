namespace CathodeEditorGUI.UserControls
{
    partial class GUI_VectorDataType
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
            this.VECTOR_VARIABLE_DUMMY = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.POS_Z_1 = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.POS_Y_1 = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.POS_X_1 = new System.Windows.Forms.NumericUpDown();
            this.VECTOR_VARIABLE_DUMMY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X_1)).BeginInit();
            this.SuspendLayout();
            // 
            // VECTOR_VARIABLE_DUMMY
            // 
            this.VECTOR_VARIABLE_DUMMY.Controls.Add(this.label18);
            this.VECTOR_VARIABLE_DUMMY.Controls.Add(this.POS_Z_1);
            this.VECTOR_VARIABLE_DUMMY.Controls.Add(this.label19);
            this.VECTOR_VARIABLE_DUMMY.Controls.Add(this.POS_Y_1);
            this.VECTOR_VARIABLE_DUMMY.Controls.Add(this.label20);
            this.VECTOR_VARIABLE_DUMMY.Controls.Add(this.POS_X_1);
            this.VECTOR_VARIABLE_DUMMY.Location = new System.Drawing.Point(3, 3);
            this.VECTOR_VARIABLE_DUMMY.Name = "VECTOR_VARIABLE_DUMMY";
            this.VECTOR_VARIABLE_DUMMY.Size = new System.Drawing.Size(700, 56);
            this.VECTOR_VARIABLE_DUMMY.TabIndex = 16;
            this.VECTOR_VARIABLE_DUMMY.TabStop = false;
            this.VECTOR_VARIABLE_DUMMY.Text = "Parameter Name (00-00-00-00)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(218, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 13);
            this.label18.TabIndex = 6;
            this.label18.Text = "Z:";
            // 
            // POS_Z_1
            // 
            this.POS_Z_1.DecimalPlaces = 8;
            this.POS_Z_1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_Z_1.Location = new System.Drawing.Point(238, 22);
            this.POS_Z_1.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.POS_Z_1.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Z_1.Name = "POS_Z_1";
            this.POS_Z_1.Size = new System.Drawing.Size(80, 20);
            this.POS_Z_1.TabIndex = 5;
            this.POS_Z_1.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Z_1.ValueChanged += new System.EventHandler(this.POS_Z_1_ValueChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(115, 25);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 13);
            this.label19.TabIndex = 4;
            this.label19.Text = "Y:";
            // 
            // POS_Y_1
            // 
            this.POS_Y_1.DecimalPlaces = 8;
            this.POS_Y_1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_Y_1.Location = new System.Drawing.Point(134, 22);
            this.POS_Y_1.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.POS_Y_1.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Y_1.Name = "POS_Y_1";
            this.POS_Y_1.Size = new System.Drawing.Size(80, 20);
            this.POS_Y_1.TabIndex = 3;
            this.POS_Y_1.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Y_1.ValueChanged += new System.EventHandler(this.POS_Y_1_ValueChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(12, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(17, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "X:";
            // 
            // POS_X_1
            // 
            this.POS_X_1.DecimalPlaces = 8;
            this.POS_X_1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_X_1.Location = new System.Drawing.Point(31, 22);
            this.POS_X_1.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.POS_X_1.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_X_1.Name = "POS_X_1";
            this.POS_X_1.Size = new System.Drawing.Size(80, 20);
            this.POS_X_1.TabIndex = 1;
            this.POS_X_1.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_X_1.ValueChanged += new System.EventHandler(this.POS_X_1_ValueChanged);
            // 
            // GUI_VectorDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VECTOR_VARIABLE_DUMMY);
            this.Name = "GUI_VectorDataType";
            this.Size = new System.Drawing.Size(707, 61);
            this.VECTOR_VARIABLE_DUMMY.ResumeLayout(false);
            this.VECTOR_VARIABLE_DUMMY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X_1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox VECTOR_VARIABLE_DUMMY;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown POS_Z_1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown POS_Y_1;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown POS_X_1;
    }
}
