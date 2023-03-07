namespace CommandsEditor
{
    partial class RenameEntity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameEntity));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.entity_name = new System.Windows.Forms.TextBox();
            this.save_entity_name = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.entity_name);
            this.groupBox1.Controls.Add(this.save_entity_name);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 93);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rename Entity";
            // 
            // entity_name
            // 
            this.entity_name.Location = new System.Drawing.Point(81, 28);
            this.entity_name.Name = "entity_name";
            this.entity_name.Size = new System.Drawing.Size(664, 20);
            this.entity_name.TabIndex = 6;
            // 
            // save_entity_name
            // 
            this.save_entity_name.Location = new System.Drawing.Point(645, 52);
            this.save_entity_name.Name = "save_entity_name";
            this.save_entity_name.Size = new System.Drawing.Size(101, 23);
            this.save_entity_name.TabIndex = 4;
            this.save_entity_name.Text = "Save";
            this.save_entity_name.UseVisualStyleBackColor = true;
            this.save_entity_name.Click += new System.EventHandler(this.save_entity_name_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Entity Name";
            // 
            // CathodeEditorGUI_RenameEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 116);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CathodeEditorGUI_RenameEntity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rename Entity";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox entity_name;
        private System.Windows.Forms.Button save_entity_name;
        private System.Windows.Forms.Label label1;
    }
}