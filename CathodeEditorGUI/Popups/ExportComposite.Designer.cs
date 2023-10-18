namespace CommandsEditor
{
    partial class ExportComposite
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportComposite));
            this.export = new System.Windows.Forms.Button();
            this.levelList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.overwrite = new System.Windows.Forms.CheckBox();
            this.recurse = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(324, 71);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(113, 23);
            this.export.TabIndex = 5;
            this.export.Text = "Export";
            this.toolTip1.SetToolTip(this.export, "Export composite to selected level.");
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // levelList
            // 
            this.levelList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.levelList.FormattingEnabled = true;
            this.levelList.Location = new System.Drawing.Point(15, 25);
            this.levelList.Name = "levelList";
            this.levelList.Size = new System.Drawing.Size(422, 21);
            this.levelList.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Port composite to level:";
            // 
            // overwrite
            // 
            this.overwrite.AutoSize = true;
            this.overwrite.Checked = true;
            this.overwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overwrite.Location = new System.Drawing.Point(15, 54);
            this.overwrite.Name = "overwrite";
            this.overwrite.Size = new System.Drawing.Size(219, 17);
            this.overwrite.TabIndex = 8;
            this.overwrite.Text = "Overwrite existing destination composites";
            this.toolTip1.SetToolTip(this.overwrite, "If checked: when composites are copied they will overwrite any by the same ID in " +
        "the destination level.");
            this.overwrite.UseVisualStyleBackColor = true;
            // 
            // recurse
            // 
            this.recurse.AutoSize = true;
            this.recurse.Checked = true;
            this.recurse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.recurse.Location = new System.Drawing.Point(15, 77);
            this.recurse.Name = "recurse";
            this.recurse.Size = new System.Drawing.Size(257, 17);
            this.recurse.TabIndex = 9;
            this.recurse.Text = "Copy all composites referenced by this composite";
            this.toolTip1.SetToolTip(this.recurse, "If checked: composites that are instanced within the exported composite will also" +
        " be copied.");
            this.recurse.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 104);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(422, 56);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "";
            // 
            // ExportComposite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 172);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.recurse);
            this.Controls.Add(this.overwrite);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.levelList);
            this.Controls.Add(this.export);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ExportComposite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Port Composite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button export;
        private System.Windows.Forms.ComboBox levelList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox overwrite;
        private System.Windows.Forms.CheckBox recurse;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}