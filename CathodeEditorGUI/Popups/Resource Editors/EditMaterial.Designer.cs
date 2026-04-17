namespace CommandsEditor
{
    partial class EditMaterial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditMaterial));
            this.materialList = new System.Windows.Forms.ListView();
            this.selectMaterialBtn = new System.Windows.Forms.Button();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.MaterialInfoWPF1 = new MaterialInfoWPF();
            this.SuspendLayout();
            // 
            // materialList
            // 
            this.materialList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.materialList.FullRowSelect = true;
            this.materialList.HideSelection = false;
            this.materialList.LabelWrap = false;
            this.materialList.Location = new System.Drawing.Point(1, 6);
            this.materialList.MultiSelect = false;
            this.materialList.Name = "materialList";
            this.materialList.Size = new System.Drawing.Size(384, 900);
            this.materialList.TabIndex = 21;
            this.materialList.UseCompatibleStateImageBehavior = false;
            this.materialList.View = System.Windows.Forms.View.Details;
            this.materialList.SelectedIndexChanged += new System.EventHandler(this.materialList_SelectedIndexChanged);
            // 
            // selectMaterialBtn
            // 
            this.selectMaterialBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectMaterialBtn.Location = new System.Drawing.Point(689, 912);
            this.selectMaterialBtn.Name = "selectMaterialBtn";
            this.selectMaterialBtn.Size = new System.Drawing.Size(140, 30);
            this.selectMaterialBtn.TabIndex = 22;
            this.selectMaterialBtn.Text = "Use This Material";
            this.selectMaterialBtn.UseVisualStyleBackColor = true;
            this.selectMaterialBtn.Click += new System.EventHandler(this.selectMaterial_Click);
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost1.Location = new System.Drawing.Point(391, 6);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(450, 904);
            this.elementHost1.TabIndex = 20;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.MaterialInfoWPF1;
            // 
            // EditMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 950);
            this.Controls.Add(this.selectMaterialBtn);
            this.Controls.Add(this.materialList);
            this.Controls.Add(this.elementHost1);
            this.Icon = global::CommandsEditor.SharedFormIcon.Icon;
            this.MinimumSize = new System.Drawing.Size(841, 600);
            this.Name = "EditMaterial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Material Editor";
            this.Load += new System.EventHandler(this.EditMaterial_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private MaterialInfoWPF MaterialInfoWPF1;
        private System.Windows.Forms.ListView materialList;
        private System.Windows.Forms.Button selectMaterialBtn;
    }
}
