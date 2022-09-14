namespace CathodeEditorGUI.Popups.UserControls
{
    partial class GUI_Resource_RenderableInstance
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.showModelInfo = new System.Windows.Forms.Button();
            this.modelInfoTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.materialInfoTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.selectNewModel = new System.Windows.Forms.Button();
            this.showMaterialInfo = new System.Windows.Forms.Button();
            this.selectNewMaterial = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.selectNewMaterial);
            this.groupBox1.Controls.Add(this.showMaterialInfo);
            this.groupBox1.Controls.Add(this.selectNewModel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.materialInfoTextbox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.modelInfoTextbox);
            this.groupBox1.Controls.Add(this.showModelInfo);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(832, 82);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Element";
            // 
            // showModelInfo
            // 
            this.showModelInfo.Location = new System.Drawing.Point(642, 20);
            this.showModelInfo.Name = "showModelInfo";
            this.showModelInfo.Size = new System.Drawing.Size(65, 23);
            this.showModelInfo.TabIndex = 0;
            this.showModelInfo.Text = "Info";
            this.showModelInfo.UseVisualStyleBackColor = true;
            this.showModelInfo.Click += new System.EventHandler(this.showModelInfo_Click);
            // 
            // modelInfoTextbox
            // 
            this.modelInfoTextbox.Location = new System.Drawing.Point(68, 21);
            this.modelInfoTextbox.Name = "modelInfoTextbox";
            this.modelInfoTextbox.ReadOnly = true;
            this.modelInfoTextbox.Size = new System.Drawing.Size(568, 20);
            this.modelInfoTextbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Model";
            // 
            // materialInfoTextbox
            // 
            this.materialInfoTextbox.Location = new System.Drawing.Point(68, 47);
            this.materialInfoTextbox.Name = "materialInfoTextbox";
            this.materialInfoTextbox.ReadOnly = true;
            this.materialInfoTextbox.Size = new System.Drawing.Size(568, 20);
            this.materialInfoTextbox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Material";
            // 
            // selectNewModel
            // 
            this.selectNewModel.Location = new System.Drawing.Point(713, 20);
            this.selectNewModel.Name = "selectNewModel";
            this.selectNewModel.Size = new System.Drawing.Size(108, 23);
            this.selectNewModel.TabIndex = 5;
            this.selectNewModel.Text = "Select New";
            this.selectNewModel.UseVisualStyleBackColor = true;
            this.selectNewModel.Click += new System.EventHandler(this.selectNewModel_Click);
            // 
            // showMaterialInfo
            // 
            this.showMaterialInfo.Location = new System.Drawing.Point(642, 46);
            this.showMaterialInfo.Name = "showMaterialInfo";
            this.showMaterialInfo.Size = new System.Drawing.Size(65, 23);
            this.showMaterialInfo.TabIndex = 6;
            this.showMaterialInfo.Text = "Info";
            this.showMaterialInfo.UseVisualStyleBackColor = true;
            this.showMaterialInfo.Click += new System.EventHandler(this.showMaterialInfo_Click);
            // 
            // selectNewMaterial
            // 
            this.selectNewMaterial.Location = new System.Drawing.Point(713, 46);
            this.selectNewMaterial.Name = "selectNewMaterial";
            this.selectNewMaterial.Size = new System.Drawing.Size(108, 23);
            this.selectNewMaterial.TabIndex = 7;
            this.selectNewMaterial.Text = "Select New";
            this.selectNewMaterial.UseVisualStyleBackColor = true;
            this.selectNewMaterial.Click += new System.EventHandler(this.selectNewMaterial_Click);
            // 
            // GUI_Resource_RenderableInstance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "GUI_Resource_RenderableInstance";
            this.Size = new System.Drawing.Size(838, 87);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button selectNewMaterial;
        private System.Windows.Forms.Button showMaterialInfo;
        private System.Windows.Forms.Button selectNewModel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox materialInfoTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox modelInfoTextbox;
        private System.Windows.Forms.Button showModelInfo;
    }
}
