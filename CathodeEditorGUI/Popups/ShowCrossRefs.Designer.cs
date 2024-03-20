namespace CommandsEditor
{
    partial class ShowCrossRefs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowCrossRefs));
            this.referenceList = new System.Windows.Forms.ListBox();
            this.label = new System.Windows.Forms.Label();
            this.jumpToEntity = new System.Windows.Forms.Button();
            this.showLinkedProxies = new System.Windows.Forms.Button();
            this.showLinkedOverrides = new System.Windows.Forms.Button();
            this.showLinkedTriggerSequences = new System.Windows.Forms.Button();
            this.showLinkedCageAnimations = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // referenceList
            // 
            this.referenceList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.referenceList.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.referenceList.FormattingEnabled = true;
            this.referenceList.HorizontalScrollbar = true;
            this.referenceList.Location = new System.Drawing.Point(132, 29);
            this.referenceList.Name = "referenceList";
            this.referenceList.Size = new System.Drawing.Size(738, 381);
            this.referenceList.TabIndex = 146;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(129, 11);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(143, 13);
            this.label.TabIndex = 148;
            this.label.Text = "Proxies pointing to this entity:";
            // 
            // jumpToEntity
            // 
            this.jumpToEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.jumpToEntity.Location = new System.Drawing.Point(723, 416);
            this.jumpToEntity.Name = "jumpToEntity";
            this.jumpToEntity.Size = new System.Drawing.Size(147, 23);
            this.jumpToEntity.TabIndex = 150;
            this.jumpToEntity.Text = "Jump To Selected";
            this.jumpToEntity.UseVisualStyleBackColor = true;
            this.jumpToEntity.Click += new System.EventHandler(this.jumpToEntity_Click);
            // 
            // showLinkedProxies
            // 
            this.showLinkedProxies.Location = new System.Drawing.Point(10, 29);
            this.showLinkedProxies.Name = "showLinkedProxies";
            this.showLinkedProxies.Size = new System.Drawing.Size(114, 41);
            this.showLinkedProxies.TabIndex = 151;
            this.showLinkedProxies.Text = "Proxies";
            this.showLinkedProxies.UseVisualStyleBackColor = true;
            this.showLinkedProxies.Click += new System.EventHandler(this.showLinkedProxies_Click);
            // 
            // showLinkedOverrides
            // 
            this.showLinkedOverrides.Location = new System.Drawing.Point(10, 76);
            this.showLinkedOverrides.Name = "showLinkedOverrides";
            this.showLinkedOverrides.Size = new System.Drawing.Size(114, 41);
            this.showLinkedOverrides.TabIndex = 152;
            this.showLinkedOverrides.Text = "Aliases";
            this.showLinkedOverrides.UseVisualStyleBackColor = true;
            this.showLinkedOverrides.Click += new System.EventHandler(this.showLinkedOverrides_Click);
            // 
            // showLinkedTriggerSequences
            // 
            this.showLinkedTriggerSequences.Location = new System.Drawing.Point(10, 123);
            this.showLinkedTriggerSequences.Name = "showLinkedTriggerSequences";
            this.showLinkedTriggerSequences.Size = new System.Drawing.Size(114, 41);
            this.showLinkedTriggerSequences.TabIndex = 153;
            this.showLinkedTriggerSequences.Text = "TriggerSequences";
            this.showLinkedTriggerSequences.UseVisualStyleBackColor = true;
            this.showLinkedTriggerSequences.Click += new System.EventHandler(this.showLinkedTriggerSequences_Click);
            // 
            // showLinkedCageAnimations
            // 
            this.showLinkedCageAnimations.Location = new System.Drawing.Point(10, 170);
            this.showLinkedCageAnimations.Name = "showLinkedCageAnimations";
            this.showLinkedCageAnimations.Size = new System.Drawing.Size(114, 41);
            this.showLinkedCageAnimations.TabIndex = 154;
            this.showLinkedCageAnimations.Text = "CAGEAnimations";
            this.showLinkedCageAnimations.UseVisualStyleBackColor = true;
            this.showLinkedCageAnimations.Click += new System.EventHandler(this.showLinkedCageAnimations_Click);
            // 
            // ShowCrossRefs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 448);
            this.Controls.Add(this.showLinkedCageAnimations);
            this.Controls.Add(this.showLinkedTriggerSequences);
            this.Controls.Add(this.showLinkedOverrides);
            this.Controls.Add(this.showLinkedProxies);
            this.Controls.Add(this.jumpToEntity);
            this.Controls.Add(this.label);
            this.Controls.Add(this.referenceList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShowCrossRefs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "External References";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox referenceList;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button jumpToEntity;
        private System.Windows.Forms.Button showLinkedProxies;
        private System.Windows.Forms.Button showLinkedOverrides;
        private System.Windows.Forms.Button showLinkedTriggerSequences;
        private System.Windows.Forms.Button showLinkedCageAnimations;
    }
}