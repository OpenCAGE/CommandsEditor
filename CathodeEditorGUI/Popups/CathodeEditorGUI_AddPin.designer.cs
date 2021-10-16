namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_AddPin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_AddPin));
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pin_in_node = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.save_pin = new System.Windows.Forms.Button();
            this.parameter_out = new System.Windows.Forms.TextBox();
            this.parameter_in = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pin_out_node = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Parameter on this entity";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pin_out_node);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.parameter_out);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 116);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pin Out";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.parameter_in);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.pin_in_node);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(491, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(397, 116);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pin In";
            // 
            // pin_in_node
            // 
            this.pin_in_node.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pin_in_node.FormattingEnabled = true;
            this.pin_in_node.Location = new System.Drawing.Point(16, 40);
            this.pin_in_node.Name = "pin_in_node";
            this.pin_in_node.Size = new System.Drawing.Size(367, 21);
            this.pin_in_node.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Connects in to entity";
            // 
            // save_pin
            // 
            this.save_pin.Location = new System.Drawing.Point(783, 134);
            this.save_pin.Name = "save_pin";
            this.save_pin.Size = new System.Drawing.Size(105, 23);
            this.save_pin.TabIndex = 11;
            this.save_pin.Text = "Save";
            this.save_pin.UseVisualStyleBackColor = true;
            this.save_pin.Click += new System.EventHandler(this.save_pin_Click);
            // 
            // parameter_out
            // 
            this.parameter_out.Location = new System.Drawing.Point(16, 80);
            this.parameter_out.Name = "parameter_out";
            this.parameter_out.Size = new System.Drawing.Size(367, 20);
            this.parameter_out.TabIndex = 5;
            // 
            // parameter_in
            // 
            this.parameter_in.Location = new System.Drawing.Point(16, 80);
            this.parameter_in.Name = "parameter_in";
            this.parameter_in.Size = new System.Drawing.Size(367, 20);
            this.parameter_in.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Parameter on this entity";
            // 
            // pin_out_node
            // 
            this.pin_out_node.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pin_out_node.FormattingEnabled = true;
            this.pin_out_node.Location = new System.Drawing.Point(16, 40);
            this.pin_out_node.Name = "pin_out_node";
            this.pin_out_node.Size = new System.Drawing.Size(367, 21);
            this.pin_out_node.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Connects out from entity";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::CathodeEditorGUI.Properties.Resources.arrow;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(415, 62);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 21);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // CathodeEditorGUI_AddPin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 166);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.save_pin);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CathodeEditorGUI_AddPin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Entity Pin Link";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox pin_in_node;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button save_pin;
        private System.Windows.Forms.TextBox parameter_out;
        private System.Windows.Forms.ComboBox pin_out_node;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox parameter_in;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}