namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_AddParameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_AddParameter));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.param_name = new System.Windows.Forms.ComboBox();
            this.create_param = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.param_datatype = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.param_name);
            this.groupBox1.Controls.Add(this.create_param);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.param_datatype);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 122);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New Parameter";
            // 
            // param_name
            // 
            this.param_name.FormattingEnabled = true;
            this.param_name.Location = new System.Drawing.Point(104, 27);
            this.param_name.Name = "param_name";
            this.param_name.Size = new System.Drawing.Size(641, 21);
            this.param_name.TabIndex = 5;
            this.param_name.SelectedIndexChanged += new System.EventHandler(this.param_name_SelectedIndexChanged);
            this.param_name.TextChanged += new System.EventHandler(this.param_name_TextChanged);
            // 
            // create_param
            // 
            this.create_param.Location = new System.Drawing.Point(644, 81);
            this.create_param.Name = "create_param";
            this.create_param.Size = new System.Drawing.Size(101, 23);
            this.create_param.TabIndex = 4;
            this.create_param.Text = "Create";
            this.create_param.UseVisualStyleBackColor = true;
            this.create_param.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Data Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Param Name";
            // 
            // param_datatype
            // 
            this.param_datatype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.param_datatype.FormattingEnabled = true;
            this.param_datatype.Items.AddRange(new object[] {
            "POSITION",
            "FLOAT",
            "STRING",
            "SPLINE_DATA",
            "ENUM",
            "RESOURCE",
            "FILEPATH",
            "BOOL",
            "DIRECTION",
            "INTEGER"});
            this.param_datatype.Location = new System.Drawing.Point(104, 54);
            this.param_datatype.Name = "param_datatype";
            this.param_datatype.Size = new System.Drawing.Size(641, 21);
            this.param_datatype.TabIndex = 0;
            // 
            // CathodeEditorGUI_AddParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 144);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CathodeEditorGUI_AddParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Parameter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button create_param;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox param_datatype;
        private System.Windows.Forms.ComboBox param_name;
    }
}