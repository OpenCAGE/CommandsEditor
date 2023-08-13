﻿namespace CommandsEditor.UserControls
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
            this.ROT_Z = new System.Windows.Forms.NumericUpDown();
            this.ROT_Y = new System.Windows.Forms.NumericUpDown();
            this.ROT_X = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.POS_Z = new System.Windows.Forms.NumericUpDown();
            this.POS_Y = new System.Windows.Forms.NumericUpDown();
            this.POS_X = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.POSITION_VARIABLE_DUMMY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X)).BeginInit();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // POSITION_VARIABLE_DUMMY
            // 
            this.POSITION_VARIABLE_DUMMY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.tableLayoutPanel9);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.tableLayoutPanel10);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label8);
            this.POSITION_VARIABLE_DUMMY.Controls.Add(this.label7);
            this.POSITION_VARIABLE_DUMMY.Location = new System.Drawing.Point(3, 4);
            this.POSITION_VARIABLE_DUMMY.Name = "POSITION_VARIABLE_DUMMY";
            this.POSITION_VARIABLE_DUMMY.Size = new System.Drawing.Size(334, 108);
            this.POSITION_VARIABLE_DUMMY.TabIndex = 1;
            this.POSITION_VARIABLE_DUMMY.TabStop = false;
            this.POSITION_VARIABLE_DUMMY.Text = "Parameter Name (00-00-00-00)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Rotation";
            // 
            // ROT_Z
            // 
            this.ROT_Z.DecimalPlaces = 7;
            this.ROT_Z.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROT_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.ROT_Z.Location = new System.Drawing.Point(217, 3);
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
            this.ROT_Z.Size = new System.Drawing.Size(102, 20);
            this.ROT_Z.TabIndex = 12;
            this.ROT_Z.ValueChanged += new System.EventHandler(this.ROT_Z_ValueChanged);
            // 
            // ROT_Y
            // 
            this.ROT_Y.DecimalPlaces = 7;
            this.ROT_Y.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROT_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.ROT_Y.Location = new System.Drawing.Point(110, 3);
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
            this.ROT_Y.Size = new System.Drawing.Size(101, 20);
            this.ROT_Y.TabIndex = 10;
            this.ROT_Y.ValueChanged += new System.EventHandler(this.ROT_Y_ValueChanged);
            // 
            // ROT_X
            // 
            this.ROT_X.DecimalPlaces = 7;
            this.ROT_X.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROT_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.ROT_X.Location = new System.Drawing.Point(3, 3);
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
            this.ROT_X.Size = new System.Drawing.Size(101, 20);
            this.ROT_X.TabIndex = 8;
            this.ROT_X.ValueChanged += new System.EventHandler(this.ROT_X_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Position";
            // 
            // POS_Z
            // 
            this.POS_Z.DecimalPlaces = 7;
            this.POS_Z.Dock = System.Windows.Forms.DockStyle.Fill;
            this.POS_Z.Increment = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.POS_Z.Location = new System.Drawing.Point(217, 3);
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
            this.POS_Z.Size = new System.Drawing.Size(102, 20);
            this.POS_Z.TabIndex = 5;
            this.POS_Z.ValueChanged += new System.EventHandler(this.POS_Z_ValueChanged);
            // 
            // POS_Y
            // 
            this.POS_Y.DecimalPlaces = 7;
            this.POS_Y.Dock = System.Windows.Forms.DockStyle.Fill;
            this.POS_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.POS_Y.Location = new System.Drawing.Point(110, 3);
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
            this.POS_Y.Size = new System.Drawing.Size(101, 20);
            this.POS_Y.TabIndex = 3;
            this.POS_Y.ValueChanged += new System.EventHandler(this.POS_Y_ValueChanged);
            // 
            // POS_X
            // 
            this.POS_X.DecimalPlaces = 7;
            this.POS_X.Dock = System.Windows.Forms.DockStyle.Fill;
            this.POS_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.POS_X.Location = new System.Drawing.Point(3, 3);
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
            this.POS_X.Size = new System.Drawing.Size(101, 20);
            this.POS_X.TabIndex = 1;
            this.POS_X.ValueChanged += new System.EventHandler(this.POS_X_ValueChanged);
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.Controls.Add(this.POS_X, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.POS_Z, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.POS_Y, 1, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(6, 32);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(322, 27);
            this.tableLayoutPanel9.TabIndex = 2;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel10.ColumnCount = 3;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.Controls.Add(this.ROT_X, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.ROT_Z, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.ROT_Y, 1, 0);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(6, 75);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(322, 27);
            this.tableLayoutPanel10.TabIndex = 3;
            // 
            // GUI_TransformDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.POSITION_VARIABLE_DUMMY);
            this.Name = "GUI_TransformDataType";
            this.Size = new System.Drawing.Size(340, 114);
            this.POSITION_VARIABLE_DUMMY.ResumeLayout(false);
            this.POSITION_VARIABLE_DUMMY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROT_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X)).EndInit();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox POSITION_VARIABLE_DUMMY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown ROT_Z;
        private System.Windows.Forms.NumericUpDown ROT_Y;
        private System.Windows.Forms.NumericUpDown ROT_X;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown POS_Z;
        private System.Windows.Forms.NumericUpDown POS_Y;
        private System.Windows.Forms.NumericUpDown POS_X;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
    }
}
