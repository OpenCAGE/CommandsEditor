namespace CommandsEditor.UserControls
{
    partial class GUI_StringVariant_AssetDropdown
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dropdown = new System.Windows.Forms.Integration.ElementHost();
            this.wpF_Dropdown1 = new UserControls.Variants.WPF_Dropdown();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Parameter Name (00-00-00-00)";
            // 
            // dropdown
            // 
            this.dropdown.Location = new System.Drawing.Point(3, 20);
            this.dropdown.Name = "dropdown";
            this.dropdown.Size = new System.Drawing.Size(322, 21);
            this.dropdown.TabIndex = 22;
            this.dropdown.Child = this.wpF_Dropdown1;
            // 
            // GUI_StringVariant_AssetDropdown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dropdown);
            this.Name = "GUI_StringVariant_AssetDropdown";
            this.Size = new System.Drawing.Size(340, 45);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Integration.ElementHost dropdown;
        private UserControls.Variants.WPF_Dropdown wpF_Dropdown1;
    }
}
