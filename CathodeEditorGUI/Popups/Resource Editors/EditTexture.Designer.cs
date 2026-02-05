namespace CommandsEditor
{
    partial class EditTexture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTexture));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textureResolutionPersistent = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textureResolutionStreamed = new System.Windows.Forms.Label();
            this.textureName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.texturePreview = new System.Windows.Forms.PictureBox();
            this.selectTextureBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 7);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FileTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.selectTextureBtn);
            this.splitContainer1.Size = new System.Drawing.Size(719, 632);
            this.splitContainer1.SplitterDistance = 355;
            this.splitContainer1.TabIndex = 105;
            // 
            // FileTree
            // 
            this.FileTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileTree.FullRowSelect = true;
            this.FileTree.HideSelection = false;
            this.FileTree.Location = new System.Drawing.Point(0, 0);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(355, 632);
            this.FileTree.TabIndex = 100;
            this.FileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTree_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textureResolutionPersistent);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textureResolutionStreamed);
            this.groupBox1.Controls.Add(this.textureName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.texturePreview);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 565);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Texture Preview";
            // 
            // textureResolutionPersistent
            // 
            this.textureResolutionPersistent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textureResolutionPersistent.AutoSize = true;
            this.textureResolutionPersistent.Location = new System.Drawing.Point(149, 531);
            this.textureResolutionPersistent.Name = "textureResolutionPersistent";
            this.textureResolutionPersistent.Size = new System.Drawing.Size(10, 13);
            this.textureResolutionPersistent.TabIndex = 8;
            this.textureResolutionPersistent.Text = " ";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 531);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Resolution Persistent: ";
            // 
            // textureResolutionStreamed
            // 
            this.textureResolutionStreamed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textureResolutionStreamed.AutoSize = true;
            this.textureResolutionStreamed.Location = new System.Drawing.Point(149, 513);
            this.textureResolutionStreamed.Name = "textureResolutionStreamed";
            this.textureResolutionStreamed.Size = new System.Drawing.Size(10, 13);
            this.textureResolutionStreamed.TabIndex = 6;
            this.textureResolutionStreamed.Text = " ";
            // 
            // textureName
            // 
            this.textureName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textureName.AutoSize = true;
            this.textureName.Location = new System.Drawing.Point(149, 496);
            this.textureName.Name = "textureName";
            this.textureName.Size = new System.Drawing.Size(10, 13);
            this.textureName.TabIndex = 5;
            this.textureName.Text = " ";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 513);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Resolution Streamed:  ";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 496);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Texture Name: ";
            // 
            // texturePreview
            // 
            this.texturePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.texturePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.texturePreview.Location = new System.Drawing.Point(6, 19);
            this.texturePreview.Name = "texturePreview";
            this.texturePreview.Size = new System.Drawing.Size(339, 463);
            this.texturePreview.TabIndex = 2;
            this.texturePreview.TabStop = false;
            // 
            // selectTextureBtn
            // 
            this.selectTextureBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectTextureBtn.Enabled = false;
            this.selectTextureBtn.Location = new System.Drawing.Point(220, 593);
            this.selectTextureBtn.Name = "selectTextureBtn";
            this.selectTextureBtn.Size = new System.Drawing.Size(134, 35);
            this.selectTextureBtn.TabIndex = 1;
            this.selectTextureBtn.Text = "Select This Texture";
            this.selectTextureBtn.UseVisualStyleBackColor = true;
            this.selectTextureBtn.Click += new System.EventHandler(this.selectTextureBtn_Click);
            // 
            // EditTexture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 647);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditTexture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Texture";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.PictureBox texturePreview;
        private System.Windows.Forms.Button selectTextureBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label textureResolutionStreamed;
        private System.Windows.Forms.Label textureName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label textureResolutionPersistent;
        private System.Windows.Forms.Label label4;
    }
}