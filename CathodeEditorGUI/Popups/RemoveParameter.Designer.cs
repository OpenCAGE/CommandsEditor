namespace CommandsEditor
{
    partial class RemoveParameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveParameter));
            this.delete_param = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.parameterToDelete = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // delete_param
            // 
            this.delete_param.Location = new System.Drawing.Point(615, 39);
            this.delete_param.Name = "delete_param";
            this.delete_param.Size = new System.Drawing.Size(101, 23);
            this.delete_param.TabIndex = 4;
            this.delete_param.Text = "Delete";
            this.delete_param.UseVisualStyleBackColor = true;
            this.delete_param.Click += new System.EventHandler(this.delete_param_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Parameter";
            // 
            // parameterToDelete
            // 
            this.parameterToDelete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterToDelete.FormattingEnabled = true;
            this.parameterToDelete.Items.AddRange(new object[] {
            "POSITION",
            "FLOAT",
            "STRING",
            "SPLINE_DATA",
            "ENUM",
            "SHORT_GUID",
            "FILEPATH",
            "BOOL",
            "DIRECTION",
            "INTEGER"});
            this.parameterToDelete.Location = new System.Drawing.Point(75, 12);
            this.parameterToDelete.Name = "parameterToDelete";
            this.parameterToDelete.Size = new System.Drawing.Size(641, 21);
            this.parameterToDelete.TabIndex = 0;
            // 
            // RemoveParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 73);
            this.Controls.Add(this.delete_param);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.parameterToDelete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RemoveParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remove Parameter/Link";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button delete_param;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox parameterToDelete;
    }
}