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
            this.label2 = new System.Windows.Forms.Label();
            this.materialInfoTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modelInfoTextbox = new System.Windows.Forms.TextBox();
            this.editModel = new System.Windows.Forms.Button();
            this.editMaterial = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.editMaterial);
            this.groupBox1.Controls.Add(this.editModel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.materialInfoTextbox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.modelInfoTextbox);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(832, 82);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Element";
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
            // materialInfoTextbox
            // 
            this.materialInfoTextbox.Location = new System.Drawing.Point(68, 47);
            this.materialInfoTextbox.Name = "materialInfoTextbox";
            this.materialInfoTextbox.ReadOnly = true;
            this.materialInfoTextbox.Size = new System.Drawing.Size(639, 20);
            this.materialInfoTextbox.TabIndex = 2;
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
            // modelInfoTextbox
            // 
            this.modelInfoTextbox.Location = new System.Drawing.Point(68, 21);
            this.modelInfoTextbox.Name = "modelInfoTextbox";
            this.modelInfoTextbox.ReadOnly = true;
            this.modelInfoTextbox.Size = new System.Drawing.Size(639, 20);
            this.modelInfoTextbox.TabIndex = 0;
            // 
            // editModel
            // 
            this.editModel.Location = new System.Drawing.Point(713, 19);
            this.editModel.Name = "editModel";
            this.editModel.Size = new System.Drawing.Size(108, 23);
            this.editModel.TabIndex = 8;
            this.editModel.Text = "Edit";
            this.editModel.UseVisualStyleBackColor = true;
            this.editModel.Click += new System.EventHandler(this.editModel_Click);
            // 
            // editMaterial
            // 
            this.editMaterial.Location = new System.Drawing.Point(713, 47);
            this.editMaterial.Name = "editMaterial";
            this.editMaterial.Size = new System.Drawing.Size(108, 23);
            this.editMaterial.TabIndex = 9;
            this.editMaterial.Text = "Edit";
            this.editMaterial.UseVisualStyleBackColor = true;
            this.editMaterial.Click += new System.EventHandler(this.editMaterial_Click);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox materialInfoTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox modelInfoTextbox;
        private System.Windows.Forms.Button editMaterial;
        private System.Windows.Forms.Button editModel;
    }
}
