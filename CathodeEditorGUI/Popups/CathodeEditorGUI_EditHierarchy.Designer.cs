namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_EditHierarchy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_EditHierarchy));
            this.composite_content = new System.Windows.Forms.ListBox();
            this.compositeName = new System.Windows.Forms.Label();
            this.SelectEntity = new System.Windows.Forms.Button();
            this.FollowEntityThrough = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // composite_content
            // 
            this.composite_content.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.composite_content.FormattingEnabled = true;
            this.composite_content.HorizontalScrollbar = true;
            this.composite_content.Location = new System.Drawing.Point(12, 35);
            this.composite_content.Name = "composite_content";
            this.composite_content.Size = new System.Drawing.Size(444, 641);
            this.composite_content.TabIndex = 145;
            this.composite_content.SelectedIndexChanged += new System.EventHandler(this.composite_content_SelectedIndexChanged);
            // 
            // compositeName
            // 
            this.compositeName.AutoSize = true;
            this.compositeName.Location = new System.Drawing.Point(12, 13);
            this.compositeName.Name = "compositeName";
            this.compositeName.Size = new System.Drawing.Size(104, 13);
            this.compositeName.TabIndex = 146;
            this.compositeName.Text = "COMPOSITE NAME";
            // 
            // SelectEntity
            // 
            this.SelectEntity.Location = new System.Drawing.Point(285, 682);
            this.SelectEntity.Name = "SelectEntity";
            this.SelectEntity.Size = new System.Drawing.Size(171, 23);
            this.SelectEntity.TabIndex = 147;
            this.SelectEntity.Text = "Select Entity";
            this.SelectEntity.UseVisualStyleBackColor = true;
            this.SelectEntity.Click += new System.EventHandler(this.SelectEntity_Click);
            // 
            // FollowEntityThrough
            // 
            this.FollowEntityThrough.Location = new System.Drawing.Point(12, 682);
            this.FollowEntityThrough.Name = "FollowEntityThrough";
            this.FollowEntityThrough.Size = new System.Drawing.Size(171, 23);
            this.FollowEntityThrough.TabIndex = 148;
            this.FollowEntityThrough.Text = "Follow Entity Through";
            this.FollowEntityThrough.UseVisualStyleBackColor = true;
            this.FollowEntityThrough.Click += new System.EventHandler(this.FollowEntityThrough_Click);
            // 
            // CathodeEditorGUI_EditHierarchy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 717);
            this.Controls.Add(this.FollowEntityThrough);
            this.Controls.Add(this.SelectEntity);
            this.Controls.Add(this.compositeName);
            this.Controls.Add(this.composite_content);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CathodeEditorGUI_EditHierarchy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Hierarchy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox composite_content;
        private System.Windows.Forms.Label compositeName;
        private System.Windows.Forms.Button SelectEntity;
        private System.Windows.Forms.Button FollowEntityThrough;
    }
}