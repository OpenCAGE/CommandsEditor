namespace CommandsEditor
{
    partial class RemoveParameter
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Links In", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Parameters", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Links Out", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveParameter));
            this.delete_param = new System.Windows.Forms.Button();
            this.parameterToDelete = new System.Windows.Forms.ListView();
            this.funcHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inheritHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // delete_param
            // 
            this.delete_param.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delete_param.Location = new System.Drawing.Point(540, 371);
            this.delete_param.Name = "delete_param";
            this.delete_param.Size = new System.Drawing.Size(101, 23);
            this.delete_param.TabIndex = 4;
            this.delete_param.Text = "Delete";
            this.delete_param.UseVisualStyleBackColor = true;
            this.delete_param.Click += new System.EventHandler(this.delete_param_Click);
            // 
            // parameterToDelete
            // 
            this.parameterToDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterToDelete.CheckBoxes = true;
            this.parameterToDelete.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.funcHeader,
            this.inheritHeader});
            this.parameterToDelete.FullRowSelect = true;
            listViewGroup1.Header = "Links In";
            listViewGroup1.Name = "Links In";
            listViewGroup2.Header = "Parameters";
            listViewGroup2.Name = "Parameters";
            listViewGroup3.Header = "Links Out";
            listViewGroup3.Name = "Links Out";
            this.parameterToDelete.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.parameterToDelete.HideSelection = false;
            this.parameterToDelete.LargeImageList = this.imageList;
            this.parameterToDelete.Location = new System.Drawing.Point(12, 12);
            this.parameterToDelete.Name = "parameterToDelete";
            this.parameterToDelete.Size = new System.Drawing.Size(629, 353);
            this.parameterToDelete.SmallImageList = this.imageList;
            this.parameterToDelete.TabIndex = 191;
            this.parameterToDelete.UseCompatibleStateImageBehavior = false;
            this.parameterToDelete.View = System.Windows.Forms.View.Details;
            // 
            // funcHeader
            // 
            this.funcHeader.Text = "Parameter Name";
            this.funcHeader.Width = 295;
            // 
            // inheritHeader
            // 
            this.inheritHeader.Text = "Type Info";
            this.inheritHeader.Width = 314;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "ArrowLeft.png");
            this.imageList.Images.SetKeyName(1, "ArrowRight.png");
            this.imageList.Images.SetKeyName(2, "Database.ico");
            // 
            // RemoveParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 406);
            this.Controls.Add(this.parameterToDelete);
            this.Controls.Add(this.delete_param);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoveParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remove Parameter/Link";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button delete_param;
        private System.Windows.Forms.ListView parameterToDelete;
        private System.Windows.Forms.ColumnHeader funcHeader;
        private System.Windows.Forms.ColumnHeader inheritHeader;
        private System.Windows.Forms.ImageList imageList;
    }
}