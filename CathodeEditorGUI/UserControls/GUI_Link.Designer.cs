namespace CathodeEditorGUI.UserControls
{
    partial class GUI_Link
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
            this.group = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.EditLink = new System.Windows.Forms.Button();
            this.GoTo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // group
            // 
            this.group.Controls.Add(this.label1);
            this.group.Controls.Add(this.textBox1);
            this.group.Controls.Add(this.EditLink);
            this.group.Controls.Add(this.GoTo);
            this.group.Location = new System.Drawing.Point(3, 2);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(334, 91);
            this.group.TabIndex = 19;
            this.group.TabStop = false;
            this.group.Text = "Parameter Name (00-00-00-00)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(322, 20);
            this.textBox1.TabIndex = 3;
            // 
            // EditLink
            // 
            this.EditLink.Location = new System.Drawing.Point(169, 59);
            this.EditLink.Name = "EditLink";
            this.EditLink.Size = new System.Drawing.Size(160, 23);
            this.EditLink.TabIndex = 2;
            this.EditLink.Text = "Edit Link";
            this.EditLink.UseVisualStyleBackColor = true;
            this.EditLink.Click += new System.EventHandler(this.EditLink_Click);
            // 
            // GoTo
            // 
            this.GoTo.Location = new System.Drawing.Point(6, 59);
            this.GoTo.Name = "GoTo";
            this.GoTo.Size = new System.Drawing.Size(160, 23);
            this.GoTo.TabIndex = 1;
            this.GoTo.Text = "Go To Link";
            this.GoTo.UseVisualStyleBackColor = true;
            this.GoTo.Click += new System.EventHandler(this.GoTo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // GUI_Link
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.group);
            this.Name = "GUI_Link";
            this.Size = new System.Drawing.Size(340, 94);
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.Button GoTo;
        private System.Windows.Forms.Button EditLink;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}
