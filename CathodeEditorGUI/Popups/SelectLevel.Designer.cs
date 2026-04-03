namespace CommandsEditor.Popups
{
    partial class SelectLevel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectLevel));
            this.load_commands_pak = new System.Windows.Forms.Button();
            this.env_list = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // load_commands_pak
            // 
            this.load_commands_pak.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.load_commands_pak.Location = new System.Drawing.Point(453, 17);
            this.load_commands_pak.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.load_commands_pak.Name = "load_commands_pak";
            this.load_commands_pak.Size = new System.Drawing.Size(129, 35);
            this.load_commands_pak.TabIndex = 174;
            this.load_commands_pak.Text = "Load";
            this.load_commands_pak.UseVisualStyleBackColor = true;
            this.load_commands_pak.Click += new System.EventHandler(this.load_commands_pak_Click);
            // 
            // env_list
            // 
            this.env_list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.env_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.env_list.FormattingEnabled = true;
            this.env_list.Location = new System.Drawing.Point(18, 18);
            this.env_list.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.env_list.Name = "env_list";
            this.env_list.Size = new System.Drawing.Size(424, 28);
            this.env_list.TabIndex = 175;
            // 
            // SelectLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 66);
            this.Controls.Add(this.load_commands_pak);
            this.Controls.Add(this.env_list);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(616, 105);
            this.Name = "SelectLevel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Level";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button load_commands_pak;
        private System.Windows.Forms.ComboBox env_list;
    }
}