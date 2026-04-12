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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.max_damage_distance_scale_to_set_1_normal = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.max_hearing_distance_set_1_normal = new System.Windows.Forms.TextBox();
            this.viewcone_set_set_1_normal = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(250, 224);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(650, 327);
            this.tabControl1.TabIndex = 412;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(642, 301);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(642, 301);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(678, 161);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(169, 13);
            this.label20.TabIndex = 519;
            this.label20.Text = "Maximum Damage Distance Scale";
            // 
            // max_damage_distance_scale_to_set_1_normal
            // 
            this.max_damage_distance_scale_to_set_1_normal.Enabled = false;
            this.max_damage_distance_scale_to_set_1_normal.Location = new System.Drawing.Point(681, 177);
            this.max_damage_distance_scale_to_set_1_normal.Name = "max_damage_distance_scale_to_set_1_normal";
            this.max_damage_distance_scale_to_set_1_normal.Size = new System.Drawing.Size(187, 20);
            this.max_damage_distance_scale_to_set_1_normal.TabIndex = 520;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(476, 161);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(136, 13);
            this.label19.TabIndex = 517;
            this.label19.Text = "Maximum Hearing Distance";
            // 
            // max_hearing_distance_set_1_normal
            // 
            this.max_hearing_distance_set_1_normal.Enabled = false;
            this.max_hearing_distance_set_1_normal.Location = new System.Drawing.Point(479, 177);
            this.max_hearing_distance_set_1_normal.Name = "max_hearing_distance_set_1_normal";
            this.max_hearing_distance_set_1_normal.Size = new System.Drawing.Size(187, 20);
            this.max_hearing_distance_set_1_normal.TabIndex = 518;
            // 
            // viewcone_set_set_1_normal
            // 
            this.viewcone_set_set_1_normal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.viewcone_set_set_1_normal.Enabled = false;
            this.viewcone_set_set_1_normal.FormattingEnabled = true;
            this.viewcone_set_set_1_normal.Items.AddRange(new object[] {
            "VIEWCONESET_STANDARD",
            "VIEWCONESET_HUMAN",
            "VIEWCONESET_SLEEPING",
            "VIEWCONESET_ANDROID",
            "VIEWCONESET_HUMAN_HEIGHTENED"});
            this.viewcone_set_set_1_normal.Location = new System.Drawing.Point(277, 176);
            this.viewcone_set_set_1_normal.Name = "viewcone_set_set_1_normal";
            this.viewcone_set_set_1_normal.Size = new System.Drawing.Size(187, 21);
            this.viewcone_set_set_1_normal.TabIndex = 516;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(274, 161);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 13);
            this.label17.TabIndex = 515;
            this.label17.Text = "Viewcone Set";
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
            this.Size = new System.Drawing.Size(1150, 774);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox max_damage_distance_scale_to_set_1_normal;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox max_hearing_distance_set_1_normal;
        private System.Windows.Forms.ComboBox viewcone_set_set_1_normal;
        private System.Windows.Forms.Label label17;
    }
}
