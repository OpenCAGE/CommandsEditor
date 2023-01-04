namespace CathodeEditorGUI.Popups.UserControls
{
    partial class GUI_Resource_AnimatedModel
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
            this.label1 = new System.Windows.Forms.Label();
            this.animatedModelIndex = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.animatedModelIndex);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(832, 56);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ANIMATED_MODEL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Environment Animation:";
            // 
            // animatedModelIndex
            // 
            this.animatedModelIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.animatedModelIndex.FormattingEnabled = true;
            this.animatedModelIndex.Location = new System.Drawing.Point(142, 21);
            this.animatedModelIndex.Name = "animatedModelIndex";
            this.animatedModelIndex.Size = new System.Drawing.Size(253, 21);
            this.animatedModelIndex.TabIndex = 0;
            this.animatedModelIndex.SelectedIndexChanged += new System.EventHandler(this.animatedModelIndex_SelectedIndexChanged);
            // 
            // GUI_Resource_AnimatedModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "GUI_Resource_AnimatedModel";
            this.Size = new System.Drawing.Size(838, 62);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox animatedModelIndex;
        private System.Windows.Forms.Label label1;
    }
}
