namespace CommandsEditor
{
    partial class ShowCompositeUses
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowCompositeUses));
            this.referenceList = new System.Windows.Forms.ListBox();
            this.label = new System.Windows.Forms.Label();
            this.jumpToEntity = new System.Windows.Forms.Button();
            this.entityVariant = new System.Windows.Forms.ComboBox();
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
            this.referenceList.Location = new System.Drawing.Point(15, 42);
            this.referenceList.Name = "referenceList";
            this.referenceList.Size = new System.Drawing.Size(738, 407);
            this.referenceList.TabIndex = 146;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 15);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(111, 13);
            this.label.TabIndex = 148;
            this.label.Text = "Find uses of function: ";
            // 
            // jumpToEntity
            // 
            this.jumpToEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.jumpToEntity.Location = new System.Drawing.Point(606, 455);
            this.jumpToEntity.Name = "jumpToEntity";
            this.jumpToEntity.Size = new System.Drawing.Size(147, 23);
            this.jumpToEntity.TabIndex = 150;
            this.jumpToEntity.Text = "Jump To Selected";
            this.jumpToEntity.UseVisualStyleBackColor = true;
            this.jumpToEntity.Click += new System.EventHandler(this.jumpToEntity_Click);
            // 
            // entityVariant
            // 
            this.entityVariant.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entityVariant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.entityVariant.FormattingEnabled = true;
            this.entityVariant.Location = new System.Drawing.Point(129, 12);
            this.entityVariant.Name = "entityVariant";
            this.entityVariant.Size = new System.Drawing.Size(624, 21);
            this.entityVariant.TabIndex = 151;
            this.entityVariant.SelectedIndexChanged += new System.EventHandler(this.entityVariant_SelectedIndexChanged);
            // 
            // ShowCompositeUses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 490);
            this.Controls.Add(this.entityVariant);
            this.Controls.Add(this.jumpToEntity);
            this.Controls.Add(this.label);
            this.Controls.Add(this.referenceList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShowCompositeUses";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Function Uses";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox referenceList;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button jumpToEntity;
        private System.Windows.Forms.ComboBox entityVariant;
    }
}