namespace CommandsEditor.UserControls
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
            this.POS_Z_1 = new System.Windows.Forms.NumericUpDown();
            this.POS_Y_1 = new System.Windows.Forms.NumericUpDown();
            this.POS_X_1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X_1)).BeginInit();
            this.tableLayoutPanel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // POS_Z_1
            // 
            this.POS_Z_1.DecimalPlaces = 7;
            this.POS_Z_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.POS_Z_1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_Z_1.Location = new System.Drawing.Point(225, 3);
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
            this.POS_Z_1.Size = new System.Drawing.Size(106, 20);
            this.POS_Z_1.TabIndex = 5;
            this.POS_Z_1.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Z_1.ValueChanged += new System.EventHandler(this.POS_Z_1_ValueChanged);
            // 
            // POS_Y_1
            // 
            this.POS_Y_1.DecimalPlaces = 7;
            this.POS_Y_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.POS_Y_1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_Y_1.Location = new System.Drawing.Point(114, 3);
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
            this.POS_Y_1.Size = new System.Drawing.Size(105, 20);
            this.POS_Y_1.TabIndex = 3;
            this.POS_Y_1.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_Y_1.ValueChanged += new System.EventHandler(this.POS_Y_1_ValueChanged);
            // 
            // POS_X_1
            // 
            this.POS_X_1.DecimalPlaces = 7;
            this.POS_X_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.POS_X_1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            851968});
            this.POS_X_1.Location = new System.Drawing.Point(3, 3);
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
            this.POS_X_1.Size = new System.Drawing.Size(105, 20);
            this.POS_X_1.TabIndex = 1;
            this.POS_X_1.Value = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            -2147483648});
            this.POS_X_1.ValueChanged += new System.EventHandler(this.POS_X_1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Parameter Name (00-00-00-00)";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel9.Controls.Add(this.POS_Z_1, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.POS_Y_1, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.POS_X_1, 0, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(334, 27);
            this.tableLayoutPanel9.TabIndex = 22;
            // 
            // GUI_VectorDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel9);
            this.Controls.Add(this.label1);
            this.Name = "GUI_VectorDataType";
            this.Size = new System.Drawing.Size(340, 51);
            ((System.ComponentModel.ISupportInitialize)(this.POS_Z_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_Y_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.POS_X_1)).EndInit();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown POS_Z_1;
        private System.Windows.Forms.NumericUpDown POS_Y_1;
        private System.Windows.Forms.NumericUpDown POS_X_1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
    }
}
