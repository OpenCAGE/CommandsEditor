namespace CommandsEditor.Popups.UserControls
{
    partial class GUI_Resource_DynamicPhysicsSystem
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.addNewInstanceRef = new System.Windows.Forms.Button();
            this.instances = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ROT_Z = new SmoothNumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.POS_X = new SmoothNumericUpDown();
            this.ROT_Y = new SmoothNumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.POS_Y = new SmoothNumericUpDown();
            this.ROT_X = new SmoothNumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.POS_Z = new SmoothNumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ROT_Z);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.POS_X);
            this.groupBox1.Controls.Add(this.ROT_Y);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.POS_Y);
            this.groupBox1.Controls.Add(this.ROT_X);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.POS_Z);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.addNewInstanceRef);
            this.groupBox1.Controls.Add(this.instances);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(832, 135);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dynamic Physics System";
            // 
            // addNewInstanceRef
            // 
            this.addNewInstanceRef.Location = new System.Drawing.Point(699, 22);
            this.addNewInstanceRef.Name = "addNewInstanceRef";
            this.addNewInstanceRef.Size = new System.Drawing.Size(127, 23);
            this.addNewInstanceRef.TabIndex = 25;
            this.addNewInstanceRef.Text = "Add New";
            this.addNewInstanceRef.UseVisualStyleBackColor = true;
            this.addNewInstanceRef.Click += new System.EventHandler(this.addNewInstanceRef_Click);
            // 
            // instances
            // 
            this.instances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.instances.FormattingEnabled = true;
            this.instances.Location = new System.Drawing.Point(9, 23);
            this.instances.Name = "instances";
            this.instances.Size = new System.Drawing.Size(687, 21);
            this.instances.TabIndex = 24;
            this.instances.SelectedIndexChanged += new System.EventHandler(this.instances_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(258, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "Rotation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(464, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 55;
            this.label4.Text = "Z:";
            // 
            // ROT_Z
            // 
            this.ROT_Z.DecimalPlaces = 7;
            this.ROT_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ROT_Z.Location = new System.Drawing.Point(484, 107);
            this.ROT_Z.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.ROT_Z.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.ROT_Z.Name = "ROT_Z";
            this.ROT_Z.Size = new System.Drawing.Size(80, 20);
            this.ROT_Z.TabIndex = 54;
            this.ROT_Z.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(361, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 53;
            this.label5.Text = "Y:";
            // 
            // POS_X
            // 
            this.POS_X.DecimalPlaces = 7;
            this.POS_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.POS_X.Location = new System.Drawing.Point(277, 68);
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
            this.POS_X.TabIndex = 43;
            this.POS_X.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            // 
            // ROT_Y
            // 
            this.ROT_Y.DecimalPlaces = 7;
            this.ROT_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ROT_Y.Location = new System.Drawing.Point(380, 107);
            this.ROT_Y.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.ROT_Y.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.ROT_Y.Name = "ROT_Y";
            this.ROT_Y.Size = new System.Drawing.Size(80, 20);
            this.ROT_Y.TabIndex = 52;
            this.ROT_Y.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(258, 71);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 13);
            this.label14.TabIndex = 44;
            this.label14.Text = "X:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(258, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 51;
            this.label6.Text = "X:";
            // 
            // POS_Y
            // 
            this.POS_Y.DecimalPlaces = 7;
            this.POS_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.POS_Y.Location = new System.Drawing.Point(380, 68);
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
            this.POS_Y.TabIndex = 45;
            this.POS_Y.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            // 
            // ROT_X
            // 
            this.ROT_X.DecimalPlaces = 7;
            this.ROT_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ROT_X.Location = new System.Drawing.Point(277, 107);
            this.ROT_X.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.ROT_X.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.ROT_X.Name = "ROT_X";
            this.ROT_X.Size = new System.Drawing.Size(80, 20);
            this.ROT_X.TabIndex = 50;
            this.ROT_X.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(361, 71);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 13);
            this.label13.TabIndex = 46;
            this.label13.Text = "Y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(258, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 49;
            this.label7.Text = "Position";
            // 
            // POS_Z
            // 
            this.POS_Z.DecimalPlaces = 7;
            this.POS_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.POS_Z.Location = new System.Drawing.Point(484, 68);
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
            this.POS_Z.TabIndex = 47;
            this.POS_Z.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(464, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 48;
            this.label9.Text = "Z:";
            // 
            // GUI_Resource_DynamicPhysicsSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "GUI_Resource_DynamicPhysicsSystem";
            this.Size = new System.Drawing.Size(838, 142);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button addNewInstanceRef;
        private System.Windows.Forms.ComboBox instances;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private SmoothNumericUpDown ROT_Z;
        private System.Windows.Forms.Label label5;
        private SmoothNumericUpDown POS_X;
        private SmoothNumericUpDown ROT_Y;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        private SmoothNumericUpDown POS_Y;
        private SmoothNumericUpDown ROT_X;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
        private SmoothNumericUpDown POS_Z;
        private System.Windows.Forms.Label label9;
    }
}
