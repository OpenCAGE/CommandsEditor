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
            this.SuspendLayout();
            // 
            // referenceList
            // 
            this.referenceList.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.referenceList.FormattingEnabled = true;
            this.referenceList.HorizontalScrollbar = true;
            this.referenceList.Location = new System.Drawing.Point(15, 27);
            this.referenceList.Name = "referenceList";
            this.referenceList.Size = new System.Drawing.Size(738, 381);
            this.referenceList.TabIndex = 146;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(178, 13);
            this.label.TabIndex = 148;
            this.label.Text = "Entities that instance this composite:";
            // 
            // jumpToEntity
            // 
            this.jumpToEntity.Location = new System.Drawing.Point(606, 414);
            this.jumpToEntity.Name = "jumpToEntity";
            this.jumpToEntity.Size = new System.Drawing.Size(147, 23);
            this.jumpToEntity.TabIndex = 150;
            this.jumpToEntity.Text = "Jump To Selected";
            this.jumpToEntity.UseVisualStyleBackColor = true;
            this.jumpToEntity.Click += new System.EventHandler(this.jumpToEntity_Click);
            // 
            // ShowCompositeUses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 448);
            this.Controls.Add(this.jumpToEntity);
            this.Controls.Add(this.label);
            this.Controls.Add(this.referenceList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ShowCompositeUses";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Composite Uses";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox referenceList;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button jumpToEntity;
    }
}