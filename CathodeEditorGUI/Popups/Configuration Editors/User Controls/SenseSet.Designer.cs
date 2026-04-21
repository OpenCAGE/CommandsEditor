namespace CommandsEditor.ConfigEditors
{
    partial class SenseSet
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
            this.label20 = new System.Windows.Forms.Label();
            this.max_damage_distance_scale_to_set_1_normal = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.max_hearing_distance_set_1_normal = new System.Windows.Forms.TextBox();
            this.viewcone_set_set_1_normal = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.senseType1 = new ConfigEditors.SenseType();
            this.senseType2 = new ConfigEditors.SenseType();
            this.senseType3 = new ConfigEditors.SenseType();
            this.senseType4 = new ConfigEditors.SenseType();
            this.senseType5 = new ConfigEditors.SenseType();
            this.senseType6 = new ConfigEditors.SenseType();
            this.senseType7 = new ConfigEditors.SenseType();
            this.senseType8 = new ConfigEditors.SenseType();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.SuspendLayout();
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(13, 49);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(169, 13);
            this.label20.TabIndex = 533;
            this.label20.Text = "Maximum Damage Distance Scale";
            // 
            // max_damage_distance_scale_to_set_1_normal
            // 
            this.max_damage_distance_scale_to_set_1_normal.Location = new System.Drawing.Point(16, 65);
            this.max_damage_distance_scale_to_set_1_normal.Name = "max_damage_distance_scale_to_set_1_normal";
            this.max_damage_distance_scale_to_set_1_normal.Size = new System.Drawing.Size(187, 20);
            this.max_damage_distance_scale_to_set_1_normal.TabIndex = 534;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(227, 10);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(136, 13);
            this.label19.TabIndex = 531;
            this.label19.Text = "Maximum Hearing Distance";
            // 
            // max_hearing_distance_set_1_normal
            // 
            this.max_hearing_distance_set_1_normal.Location = new System.Drawing.Point(230, 26);
            this.max_hearing_distance_set_1_normal.Name = "max_hearing_distance_set_1_normal";
            this.max_hearing_distance_set_1_normal.Size = new System.Drawing.Size(187, 20);
            this.max_hearing_distance_set_1_normal.TabIndex = 532;
            // 
            // viewcone_set_set_1_normal
            // 
            this.viewcone_set_set_1_normal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewcone_set_set_1_normal.FormattingEnabled = true;
            this.viewcone_set_set_1_normal.Items.AddRange(new object[] {
            "VIEWCONESET_STANDARD",
            "VIEWCONESET_HUMAN",
            "VIEWCONESET_SLEEPING",
            "VIEWCONESET_ANDROID",
            "VIEWCONESET_HUMAN_HEIGHTENED"});
            this.viewcone_set_set_1_normal.Location = new System.Drawing.Point(16, 25);
            this.viewcone_set_set_1_normal.Name = "viewcone_set_set_1_normal";
            this.viewcone_set_set_1_normal.Size = new System.Drawing.Size(187, 21);
            this.viewcone_set_set_1_normal.TabIndex = 530;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 10);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 13);
            this.label17.TabIndex = 529;
            this.label17.Text = "Viewcone Set";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Location = new System.Drawing.Point(3, 99);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(435, 280);
            this.tabControl1.TabIndex = 528;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.senseType1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(427, 254);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Visual Sense";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.senseType2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(427, 254);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Weapon Sound Sense";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.senseType3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(427, 254);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Movement Sound Sense";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.senseType4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(427, 254);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Damage Caused Sense";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.senseType5);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(427, 254);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Touched Sense";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.senseType6);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(427, 254);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "See Flashlight Sense";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.senseType7);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(427, 254);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Affected by Flamethrower Sense";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.senseType8);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(427, 254);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "Combined Sense";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // senseType1
            // 
            this.senseType1.Location = new System.Drawing.Point(6, 7);
            this.senseType1.Name = "senseType1";
            this.senseType1.Size = new System.Drawing.Size(414, 241);
            this.senseType1.TabIndex = 1;
            // 
            // senseType2
            // 
            this.senseType2.Location = new System.Drawing.Point(6, 7);
            this.senseType2.Name = "senseType2";
            this.senseType2.Size = new System.Drawing.Size(414, 241);
            this.senseType2.TabIndex = 1;
            // 
            // senseType3
            // 
            this.senseType3.Location = new System.Drawing.Point(6, 7);
            this.senseType3.Name = "senseType3";
            this.senseType3.Size = new System.Drawing.Size(414, 241);
            this.senseType3.TabIndex = 1;
            // 
            // senseType4
            // 
            this.senseType4.Location = new System.Drawing.Point(6, 7);
            this.senseType4.Name = "senseType4";
            this.senseType4.Size = new System.Drawing.Size(414, 241);
            this.senseType4.TabIndex = 1;
            // 
            // senseType5
            // 
            this.senseType5.Location = new System.Drawing.Point(6, 7);
            this.senseType5.Name = "senseType5";
            this.senseType5.Size = new System.Drawing.Size(414, 241);
            this.senseType5.TabIndex = 1;
            // 
            // senseType6
            // 
            this.senseType6.Location = new System.Drawing.Point(6, 7);
            this.senseType6.Name = "senseType6";
            this.senseType6.Size = new System.Drawing.Size(414, 241);
            this.senseType6.TabIndex = 1;
            // 
            // senseType7
            // 
            this.senseType7.Location = new System.Drawing.Point(6, 7);
            this.senseType7.Name = "senseType7";
            this.senseType7.Size = new System.Drawing.Size(414, 241);
            this.senseType7.TabIndex = 1;
            // 
            // senseType8
            // 
            this.senseType8.Location = new System.Drawing.Point(6, 7);
            this.senseType8.Name = "senseType8";
            this.senseType8.Size = new System.Drawing.Size(414, 241);
            this.senseType8.TabIndex = 1;
            // 
            // SenseSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label20);
            this.Controls.Add(this.max_damage_distance_scale_to_set_1_normal);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.max_hearing_distance_set_1_normal);
            this.Controls.Add(this.viewcone_set_set_1_normal);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.tabControl1);
            this.Name = "SenseSet";
            this.Size = new System.Drawing.Size(441, 384);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox max_damage_distance_scale_to_set_1_normal;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox max_hearing_distance_set_1_normal;
        private System.Windows.Forms.ComboBox viewcone_set_set_1_normal;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private ConfigEditors.SenseType senseType1;
        private ConfigEditors.SenseType senseType2;
        private ConfigEditors.SenseType senseType3;
        private ConfigEditors.SenseType senseType4;
        private ConfigEditors.SenseType senseType5;
        private ConfigEditors.SenseType senseType6;
        private ConfigEditors.SenseType senseType7;
        private ConfigEditors.SenseType senseType8;
    }
}
