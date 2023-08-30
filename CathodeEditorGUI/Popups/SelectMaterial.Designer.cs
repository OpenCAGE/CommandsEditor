namespace CommandsEditor.Popups
{
    partial class SelectMaterial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectMaterial));
            this.modelPreviewArea = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectMaterialBtn = new System.Windows.Forms.Button();
            this.materialList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.modelPreviewArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelPreviewArea
            // 
            this.modelPreviewArea.Controls.Add(this.label2);
            this.modelPreviewArea.Controls.Add(this.label1);
            this.modelPreviewArea.Controls.Add(this.selectMaterialBtn);
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
            this.label1.Location = new System.Drawing.Point(42, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Previews are coming to the script editor soon.";
            // 
            // selectMaterialBtn
            // 
            this.selectMaterialBtn.Location = new System.Drawing.Point(158, 593);
            this.selectMaterialBtn.Name = "selectMaterialBtn";
            this.selectMaterialBtn.Size = new System.Drawing.Size(134, 35);
            this.selectMaterialBtn.TabIndex = 1;
            this.selectMaterialBtn.Text = "Select This Material";
            this.selectMaterialBtn.UseVisualStyleBackColor = true;
            this.selectMaterialBtn.Click += new System.EventHandler(this.selectMaterial_Click);
            // 
            // materialList
            // 
            this.materialList.FormattingEnabled = true;
            this.materialList.Location = new System.Drawing.Point(12, 12);
            this.materialList.Name = "materialList";
            this.materialList.Size = new System.Drawing.Size(366, 628);
            this.materialList.TabIndex = 106;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "You can preview and edit materials within the Asset Editor.";
            // 
            // SelectMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 652);
            this.Controls.Add(this.materialList);
            this.Controls.Add(this.modelPreviewArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SelectMaterial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Material";
            this.modelPreviewArea.ResumeLayout(false);
            this.modelPreviewArea.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox modelPreviewArea;
        private System.Windows.Forms.Button selectMaterialBtn;
        private System.Windows.Forms.ListBox materialList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}