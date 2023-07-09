namespace CommandsEditor
{
    partial class EditMVR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditMVR));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.renderable = new Popups.UserControls.GUI_Resource_RenderableInstance(null);
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SCALE_Z = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.SCALE_Y = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.SCALE_X = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ROT_Z = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.ROT_Y = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.ROT_X = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.POS_Z = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.POS_Y = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.POS_X = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.ROT_W = new System.Windows.Forms.NumericUpDown();
            this.type_dropdown = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.SCALE_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SCALE_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SCALE_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_W)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 19);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(837, 303);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // renderable
            // 
            this.renderable.Location = new System.Drawing.Point(5, 19);
            this.renderable.Name = "renderable";
            this.renderable.Size = new System.Drawing.Size(838, 186);
            this.renderable.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(555, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 285);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Scale";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(216, 304);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Z:";
            // 
            // SCALE_Z
            // 
            this.SCALE_Z.DecimalPlaces = 8;
            this.SCALE_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.SCALE_Z.Location = new System.Drawing.Point(236, 301);
            this.SCALE_Z.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.SCALE_Z.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.SCALE_Z.Name = "SCALE_Z";
            this.SCALE_Z.Size = new System.Drawing.Size(80, 20);
            this.SCALE_Z.TabIndex = 26;
            this.SCALE_Z.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.SCALE_Z.ValueChanged += new System.EventHandler(this.SCALE_Z_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(113, 304);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Y:";
            // 
            // SCALE_Y
            // 
            this.SCALE_Y.DecimalPlaces = 8;
            this.SCALE_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.SCALE_Y.Location = new System.Drawing.Point(132, 301);
            this.SCALE_Y.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.SCALE_Y.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.SCALE_Y.Name = "SCALE_Y";
            this.SCALE_Y.Size = new System.Drawing.Size(80, 20);
            this.SCALE_Y.TabIndex = 24;
            this.SCALE_Y.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.SCALE_Y.ValueChanged += new System.EventHandler(this.SCALE_Y_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 304);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "X:";
            // 
            // SCALE_X
            // 
            this.SCALE_X.DecimalPlaces = 8;
            this.SCALE_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.SCALE_X.Location = new System.Drawing.Point(29, 301);
            this.SCALE_X.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.SCALE_X.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.SCALE_X.Name = "SCALE_X";
            this.SCALE_X.Size = new System.Drawing.Size(80, 20);
            this.SCALE_X.TabIndex = 22;
            this.SCALE_X.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.SCALE_X.ValueChanged += new System.EventHandler(this.SCALE_X_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 42;
            this.label3.Text = "Rotation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Z:";
            // 
            // ROT_Z
            // 
            this.ROT_Z.DecimalPlaces = 8;
            this.ROT_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.ROT_Z.Location = new System.Drawing.Point(236, 263);
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
            this.ROT_Z.TabIndex = 40;
            this.ROT_Z.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.ROT_Z.ValueChanged += new System.EventHandler(this.ROT_Z_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(113, 266);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Y:";
            // 
            // ROT_Y
            // 
            this.ROT_Y.DecimalPlaces = 8;
            this.ROT_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.ROT_Y.Location = new System.Drawing.Point(132, 263);
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
            this.ROT_Y.TabIndex = 38;
            this.ROT_Y.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.ROT_Y.ValueChanged += new System.EventHandler(this.ROT_Y_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 266);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "X:";
            // 
            // ROT_X
            // 
            this.ROT_X.DecimalPlaces = 8;
            this.ROT_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.ROT_X.Location = new System.Drawing.Point(29, 263);
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
            this.ROT_X.TabIndex = 36;
            this.ROT_X.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.ROT_X.ValueChanged += new System.EventHandler(this.ROT_X_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 208);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Position";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(216, 227);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Z:";
            // 
            // POS_Z
            // 
            this.POS_Z.DecimalPlaces = 8;
            this.POS_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_Z.Location = new System.Drawing.Point(236, 224);
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
            this.POS_Z.TabIndex = 33;
            this.POS_Z.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Z.ValueChanged += new System.EventHandler(this.POS_Z_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(113, 227);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Y:";
            // 
            // POS_Y
            // 
            this.POS_Y.DecimalPlaces = 8;
            this.POS_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_Y.Location = new System.Drawing.Point(132, 224);
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
            this.POS_Y.TabIndex = 31;
            this.POS_Y.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Y.ValueChanged += new System.EventHandler(this.POS_Y_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 227);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 13);
            this.label14.TabIndex = 30;
            this.label14.Text = "X:";
            // 
            // POS_X
            // 
            this.POS_X.DecimalPlaces = 8;
            this.POS_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_X.Location = new System.Drawing.Point(29, 224);
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
            this.POS_X.TabIndex = 29;
            this.POS_X.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_X.ValueChanged += new System.EventHandler(this.POS_X_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(319, 266);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(21, 13);
            this.label15.TabIndex = 44;
            this.label15.Text = "W:";
            // 
            // ROT_W
            // 
            this.ROT_W.DecimalPlaces = 8;
            this.ROT_W.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.ROT_W.Location = new System.Drawing.Point(340, 263);
            this.ROT_W.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.ROT_W.Minimum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.ROT_W.Name = "ROT_W";
            this.ROT_W.Size = new System.Drawing.Size(80, 20);
            this.ROT_W.TabIndex = 43;
            this.ROT_W.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.ROT_W.ValueChanged += new System.EventHandler(this.ROT_W_ValueChanged);
            // 
            // type_dropdown
            // 
            this.type_dropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.type_dropdown.FormattingEnabled = true;
            this.type_dropdown.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.type_dropdown.Location = new System.Drawing.Point(558, 224);
            this.type_dropdown.Name = "type_dropdown";
            this.type_dropdown.Size = new System.Drawing.Size(284, 21);
            this.type_dropdown.TabIndex = 174;
            this.type_dropdown.SelectedIndexChanged += new System.EventHandler(this.type_dropdown_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(555, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(284, 52);
            this.label2.TabIndex = 175;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(849, 329);
            this.groupBox1.TabIndex = 176;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Movers";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.renderable);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.SCALE_X);
            this.groupBox2.Controls.Add(this.type_dropdown);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.SCALE_Y);
            this.groupBox2.Controls.Add(this.ROT_W);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.SCALE_Z);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.ROT_Z);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.POS_X);
            this.groupBox2.Controls.Add(this.ROT_Y);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.POS_Y);
            this.groupBox2.Controls.Add(this.ROT_X);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.POS_Z);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(12, 347);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(849, 337);
            this.groupBox2.TabIndex = 177;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Mover Descriptor";
            // 
            // EditMVR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 693);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EditMVR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Mover Descriptors";
            ((System.ComponentModel.ISupportInitialize)(this.SCALE_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SCALE_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SCALE_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_W)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private Popups.UserControls.GUI_Resource_RenderableInstance renderable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown SCALE_Z;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown SCALE_Y;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown SCALE_X;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ROT_Z;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown ROT_Y;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown ROT_X;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown POS_Z;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown POS_Y;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown POS_X;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown ROT_W;
        private System.Windows.Forms.ComboBox type_dropdown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}