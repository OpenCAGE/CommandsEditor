namespace CathodeEditorGUI.Popups
{
    partial class CathodeEditorGUI_SelectMaterial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_SelectMaterial));
            this.modelPreviewArea = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectMaterial = new System.Windows.Forms.Button();
            this.materialList = new System.Windows.Forms.ListBox();
            this.modelPreviewArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelPreviewArea
            // 
            this.modelPreviewArea.Controls.Add(this.label1);
            this.modelPreviewArea.Controls.Add(this.selectMaterial);
            this.modelPreviewArea.Location = new System.Drawing.Point(384, 6);
            this.modelPreviewArea.Name = "modelPreviewArea";
            this.modelPreviewArea.Size = new System.Drawing.Size(298, 634);
            this.modelPreviewArea.TabIndex = 105;
            this.modelPreviewArea.TabStop = false;
            this.modelPreviewArea.Text = "Preview";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(112, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Coming Soon!";
            // 
            // selectMaterial
            // 
            this.selectMaterial.Location = new System.Drawing.Point(158, 593);
            this.selectMaterial.Name = "selectMaterial";
            this.selectMaterial.Size = new System.Drawing.Size(134, 35);
            this.selectMaterial.TabIndex = 1;
            this.selectMaterial.Text = "Select This Material";
            this.selectMaterial.UseVisualStyleBackColor = true;
            this.selectMaterial.Click += new System.EventHandler(this.selectMaterial_Click);
            // 
            // materialList
            // 
            this.materialList.FormattingEnabled = true;
            this.materialList.Location = new System.Drawing.Point(12, 12);
            this.materialList.Name = "materialList";
            this.materialList.Size = new System.Drawing.Size(366, 628);
            this.materialList.TabIndex = 106;
            // 
            // CathodeEditorGUI_SelectMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 652);
            this.Controls.Add(this.materialList);
            this.Controls.Add(this.modelPreviewArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CathodeEditorGUI_SelectMaterial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Material";
            this.modelPreviewArea.ResumeLayout(false);
            this.modelPreviewArea.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox modelPreviewArea;
        private System.Windows.Forms.Button selectMaterial;
        private System.Windows.Forms.ListBox materialList;
        private System.Windows.Forms.Label label1;
    }
}