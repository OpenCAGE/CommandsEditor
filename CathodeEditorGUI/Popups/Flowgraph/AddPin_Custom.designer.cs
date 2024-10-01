namespace CommandsEditor
{
    partial class AddPin_Custom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPin_Custom));
            this.label3 = new System.Windows.Forms.Label();
            this.parameterList = new System.Windows.Forms.ComboBox();
            this.save_pin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Parameter on this entity";
            // 
            // parameterList
            // 
            this.parameterList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterList.FormattingEnabled = true;
            this.parameterList.Location = new System.Drawing.Point(12, 25);
            this.parameterList.Name = "parameterList";
            this.parameterList.Size = new System.Drawing.Size(875, 21);
            this.parameterList.TabIndex = 8;
            // 
            // save_pin
            // 
            this.save_pin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.save_pin.Location = new System.Drawing.Point(782, 52);
            this.save_pin.Name = "save_pin";
            this.save_pin.Size = new System.Drawing.Size(105, 23);
            this.save_pin.TabIndex = 11;
            this.save_pin.Text = "Add";
            this.save_pin.UseVisualStyleBackColor = true;
            this.save_pin.Click += new System.EventHandler(this.save_pin_Click);
            // 
            // AddPin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 89);
            this.Controls.Add(this.parameterList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.save_pin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1500, 128);
            this.MinimumSize = new System.Drawing.Size(500, 128);
            this.Name = "AddPin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Pin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button save_pin;
        private System.Windows.Forms.ComboBox parameterList;
    }
}