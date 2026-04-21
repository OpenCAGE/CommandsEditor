namespace CommandsEditor.ConfigEditors
{
    partial class SenseEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SenseEditor));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.senseSet2 = new ConfigEditors.SenseSet();
            this.senseSet3 = new ConfigEditors.SenseSet();
            this.senseSet4 = new ConfigEditors.SenseSet();
            this.senseSet1 = new ConfigEditors.SenseSet();
            this.characterType = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(11, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(460, 419);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.senseSet1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(452, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Set 1 Normal";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.senseSet2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(452, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Set 1 Heightened";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.senseSet3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(452, 393);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Set 2";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.senseSet4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(452, 393);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Set 3";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // senseSet2
            // 
            this.senseSet2.Location = new System.Drawing.Point(6, 4);
            this.senseSet2.Name = "senseSet2";
            this.senseSet2.Size = new System.Drawing.Size(441, 384);
            this.senseSet2.TabIndex = 2;
            // 
            // senseSet3
            // 
            this.senseSet3.Location = new System.Drawing.Point(6, 4);
            this.senseSet3.Name = "senseSet3";
            this.senseSet3.Size = new System.Drawing.Size(441, 384);
            this.senseSet3.TabIndex = 2;
            // 
            // senseSet4
            // 
            this.senseSet4.Location = new System.Drawing.Point(6, 4);
            this.senseSet4.Name = "senseSet4";
            this.senseSet4.Size = new System.Drawing.Size(441, 384);
            this.senseSet4.TabIndex = 2;
            // 
            // senseSet1
            // 
            this.senseSet1.Location = new System.Drawing.Point(6, 4);
            this.senseSet1.Name = "senseSet1";
            this.senseSet1.Size = new System.Drawing.Size(441, 384);
            this.senseSet1.TabIndex = 2;
            // 
            // characterType
            // 
            this.characterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.characterType.FormattingEnabled = true;
            this.characterType.Location = new System.Drawing.Point(12, 12);
            this.characterType.Name = "characterType";
            this.characterType.Size = new System.Drawing.Size(456, 21);
            this.characterType.TabIndex = 1;
            // 
            // SenseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 466);
            this.Controls.Add(this.characterType);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CommandsEditor.SharedFormIcon.Icon;
            this.MaximizeBox = false;
            this.Name = "SenseEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sense Editor";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ConfigEditors.SenseSet senseSet1;
        private ConfigEditors.SenseSet senseSet2;
        private System.Windows.Forms.TabPage tabPage3;
        private ConfigEditors.SenseSet senseSet3;
        private System.Windows.Forms.TabPage tabPage4;
        private ConfigEditors.SenseSet senseSet4;
        private System.Windows.Forms.ComboBox characterType;
    }
}
