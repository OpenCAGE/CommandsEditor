namespace CommandsEditor
{
    partial class EditHierarchy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditHierarchy));
            this.compositeName = new System.Windows.Forms.Label();
            this.SelectEntity = new System.Windows.Forms.Button();
            this.FollowEntityThrough = new System.Windows.Forms.Button();
            this.compositeEntityList1 = new Popups.UserControls.CompositeEntityList();
            this.SuspendLayout();
            // 
            // compositeName
            // 
            this.compositeName.AutoSize = true;
            this.compositeName.Location = new System.Drawing.Point(9, 11);
            this.compositeName.Name = "compositeName";
            this.compositeName.Size = new System.Drawing.Size(104, 13);
            this.compositeName.TabIndex = 146;
            this.compositeName.Text = "COMPOSITE NAME";
            // 
            // SelectEntity
            // 
            this.SelectEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectEntity.Location = new System.Drawing.Point(580, 679);
            this.SelectEntity.Name = "SelectEntity";
            this.SelectEntity.Size = new System.Drawing.Size(171, 23);
            this.SelectEntity.TabIndex = 147;
            this.SelectEntity.Text = "Select Entity";
            this.SelectEntity.UseVisualStyleBackColor = true;
            this.SelectEntity.Click += new System.EventHandler(this.SelectEntity_Click);
            // 
            // FollowEntityThrough
            // 
            this.FollowEntityThrough.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FollowEntityThrough.Location = new System.Drawing.Point(12, 679);
            this.FollowEntityThrough.Name = "FollowEntityThrough";
            this.FollowEntityThrough.Size = new System.Drawing.Size(171, 23);
            this.FollowEntityThrough.TabIndex = 148;
            this.FollowEntityThrough.Text = "Follow Entity Through";
            this.FollowEntityThrough.UseVisualStyleBackColor = true;
            this.FollowEntityThrough.Click += new System.EventHandler(this.FollowEntityThrough_Click);
            // 
            // compositeEntityList1
            // 
            this.compositeEntityList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.compositeEntityList1.Location = new System.Drawing.Point(9, 27);
            this.compositeEntityList1.Name = "compositeEntityList1";
            this.compositeEntityList1.Size = new System.Drawing.Size(741, 646);
            this.compositeEntityList1.TabIndex = 149;
            // 
            // EditHierarchy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 714);
            this.Controls.Add(this.compositeEntityList1);
            this.Controls.Add(this.FollowEntityThrough);
            this.Controls.Add(this.SelectEntity);
            this.Controls.Add(this.compositeName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditHierarchy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Entity";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label compositeName;
        private System.Windows.Forms.Button SelectEntity;
        private System.Windows.Forms.Button FollowEntityThrough;
        private Popups.UserControls.CompositeEntityList compositeEntityList1;
    }
}