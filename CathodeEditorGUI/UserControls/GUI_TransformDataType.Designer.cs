namespace CommandsEditor.UserControls
{
    partial class GUI_TransformDataType
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
            this.POSITION_VARIABLE_DUMMY = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ROT_Z = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.ROT_Y = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.ROT_X = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.POS_Z = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.POS_Y = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.POS_X = new System.Windows.Forms.NumericUpDown();
            this.POSITION_VARIABLE_DUMMY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X)).BeginInit();
            this.SuspendLayout();
            // 
            // POSITION_VARIABLE_DUMMY
            // 
            this.POSITION_VARIABLE_DUMMY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label8);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label10);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.ROT_Z);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label11);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.ROT_Y);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label12);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.ROT_X);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label7);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label5);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.POS_Z);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label3);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.POS_Y);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label2);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.POS_X);
            this.POSITION_VARIABLE_DUMMY.Location = new System.Drawing.Point(3, 3);
            this.POSITION_VARIABLE_DUMMY.Name = "POSITION_VARIABLE_DUMMY";
            this.POSITION_VARIABLE_DUMMY.Size = new System.Drawing.Size(334, 108);
            this.POSITION_VARIABLE_DUMMY.TabIndex = 1;
            this.POSITION_VARIABLE_DUMMY.TabStop = false;
            this.POSITION_VARIABLE_DUMMY.Text = "Parameter Name (00-00-00-00)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Rotation";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(218, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Z:";
            // 
            // ROT_Z
            // 
            this.ROT_Z.DecimalPlaces = 8;
            this.ROT_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.ROT_Z.Location = new System.Drawing.Point(238, 74);
            this.ROT_Z.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.ROT_Z.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.ROT_Z.Name = "ROT_Z";
            this.ROT_Z.Size = new System.Drawing.Size(80, 20);
            this.ROT_Z.TabIndex = 12;
            this.ROT_Z.ValueChanged += new System.EventHandler(this.ROT_Z_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(115, 77);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Y:";
            // 
            // ROT_Y
            // 
            this.ROT_Y.DecimalPlaces = 8;
            this.ROT_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.ROT_Y.Location = new System.Drawing.Point(134, 74);
            this.ROT_Y.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.ROT_Y.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.ROT_Y.Name = "ROT_Y";
            this.ROT_Y.Size = new System.Drawing.Size(80, 20);
            this.ROT_Y.TabIndex = 10;
            this.ROT_Y.ValueChanged += new System.EventHandler(this.ROT_Y_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 77);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "X:";
            // 
            // ROT_X
            // 
            this.ROT_X.DecimalPlaces = 8;
            this.ROT_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.ROT_X.Location = new System.Drawing.Point(31, 74);
            this.ROT_X.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.ROT_X.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.ROT_X.Name = "ROT_X";
            this.ROT_X.Size = new System.Drawing.Size(80, 20);
            this.ROT_X.TabIndex = 8;
            this.ROT_X.ValueChanged += new System.EventHandler(this.ROT_X_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Position";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(218, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Z:";
            // 
            // POS_Z
            // 
            this.POS_Z.DecimalPlaces = 8;
            this.POS_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_Z.Location = new System.Drawing.Point(238, 35);
            this.POS_Z.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.POS_Z.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Z.Name = "POS_Z";
            this.POS_Z.Size = new System.Drawing.Size(80, 20);
            this.POS_Z.TabIndex = 5;
            this.POS_Z.ValueChanged += new System.EventHandler(this.POS_Z_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Y:";
            // 
            // POS_Y
            // 
            this.POS_Y.DecimalPlaces = 8;
            this.POS_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_Y.Location = new System.Drawing.Point(134, 35);
            this.POS_Y.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.POS_Y.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Y.Name = "POS_Y";
            this.POS_Y.Size = new System.Drawing.Size(80, 20);
            this.POS_Y.TabIndex = 3;
            this.POS_Y.ValueChanged += new System.EventHandler(this.POS_Y_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "X:";
            // 
            // POS_X
            // 
            this.POS_X.DecimalPlaces = 8;
            this.POS_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_X.Location = new System.Drawing.Point(31, 35);
            this.POS_X.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.POS_X.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_X.Name = "POS_X";
            this.POS_X.Size = new System.Drawing.Size(80, 20);
            this.POS_X.TabIndex = 1;
            this.POS_X.ValueChanged += new System.EventHandler(this.POS_X_ValueChanged);
            // 
            // GUI_TransformDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.POSITION_VARIABLE_DUMMY);
            this.Name = "GUI_TransformDataType";
            this.Size = new System.Drawing.Size(340, 113);
            this.POSITION_VARIABLE_DUMMY.ResumeLayout(false);
            this.POSITION_VARIABLE_DUMMY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox POSITION_VARIABLE_DUMMY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown ROT_Z;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown ROT_Y;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown ROT_X;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown POS_Z;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown POS_Y;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown POS_X;
    }
}
