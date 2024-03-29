namespace CommandsEditor
{
    partial class AddParameter_PreDefined
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddParameter_PreDefined));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Target", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("State", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Input", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Output", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Parameter", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Internal", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Reference", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Method", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("Finished", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup("Relay", System.Windows.Forms.HorizontalAlignment.Left);
            this.createParams = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.searchText = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.clearSearchBtn = new System.Windows.Forms.Button();
            this.typesCount = new System.Windows.Forms.Label();
            this.param_name = new System.Windows.Forms.ListView();
            this.funcHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inheritHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listIcons = new System.Windows.Forms.ImageList(this.components);
            this.helpBtn = new System.Windows.Forms.Button();
            this.selectAll = new System.Windows.Forms.Button();
            this.deSelectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createParams
            // 
            this.createParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createParams.Location = new System.Drawing.Point(472, 377);
            this.createParams.Name = "createParams";
            this.createParams.Size = new System.Drawing.Size(169, 23);
            this.createParams.TabIndex = 6;
            this.createParams.Text = "Create Selected";
            this.createParams.UseVisualStyleBackColor = true;
            this.createParams.Click += new System.EventHandler(this.createParams_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 18);
            this.label2.TabIndex = 12;
            this.label2.Text = "Parameters";
            // 
            // searchText
            // 
            this.searchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchText.Location = new System.Drawing.Point(15, 35);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(545, 20);
            this.searchText.TabIndex = 2;
            // 
            // searchBtn
            // 
            this.searchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.searchBtn.Location = new System.Drawing.Point(578, 35);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(63, 20);
            this.searchBtn.TabIndex = 4;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // clearSearchBtn
            // 
            this.clearSearchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearSearchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearSearchBtn.Image = ((System.Drawing.Image)(resources.GetObject("clearSearchBtn.Image")));
            this.clearSearchBtn.Location = new System.Drawing.Point(559, 35);
            this.clearSearchBtn.Name = "clearSearchBtn";
            this.clearSearchBtn.Size = new System.Drawing.Size(20, 20);
            this.clearSearchBtn.TabIndex = 3;
            this.clearSearchBtn.UseVisualStyleBackColor = true;
            this.clearSearchBtn.Click += new System.EventHandler(this.clearSearchBtn_Click);
            // 
            // typesCount
            // 
            this.typesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.typesCount.AutoSize = true;
            this.typesCount.Location = new System.Drawing.Point(537, 15);
            this.typesCount.Name = "typesCount";
            this.typesCount.Size = new System.Drawing.Size(89, 13);
            this.typesCount.TabIndex = 179;
            this.typesCount.Text = "Showing 0 Types";
            this.typesCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // param_name
            // 
            this.param_name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.param_name.CheckBoxes = true;
            this.param_name.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.funcHeader,
            this.inheritHeader});
            this.param_name.FullRowSelect = true;
            listViewGroup1.Header = "Target";
            listViewGroup1.Name = "Target";
            listViewGroup2.Header = "State";
            listViewGroup2.Name = "State";
            listViewGroup3.Header = "Input";
            listViewGroup3.Name = "Input";
            listViewGroup4.Header = "Output";
            listViewGroup4.Name = "Output";
            listViewGroup5.Header = "Parameter";
            listViewGroup5.Name = "Parameter";
            listViewGroup6.Header = "Internal";
            listViewGroup6.Name = "Internal";
            listViewGroup7.Header = "Reference";
            listViewGroup7.Name = "Reference";
            listViewGroup8.Header = "Method";
            listViewGroup8.Name = "Method";
            listViewGroup9.Header = "Finished";
            listViewGroup9.Name = "Finished";
            listViewGroup10.Header = "Relay";
            listViewGroup10.Name = "Relay";
            this.param_name.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6,
            listViewGroup7,
            listViewGroup8,
            listViewGroup9,
            listViewGroup10});
            this.param_name.HideSelection = false;
            this.param_name.LargeImageList = this.listIcons;
            this.param_name.Location = new System.Drawing.Point(15, 54);
            this.param_name.MultiSelect = false;
            this.param_name.Name = "param_name";
            this.param_name.Size = new System.Drawing.Size(626, 317);
            this.param_name.SmallImageList = this.listIcons;
            this.param_name.TabIndex = 180;
            this.param_name.UseCompatibleStateImageBehavior = false;
            this.param_name.View = System.Windows.Forms.View.Details;
            // 
            // funcHeader
            // 
            this.funcHeader.Text = "Parameter";
            this.funcHeader.Width = 364;
            // 
            // inheritHeader
            // 
            this.inheritHeader.Text = "Datatype";
            this.inheritHeader.Width = 232;
            // 
            // listIcons
            // 
            this.listIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listIcons.ImageStream")));
            this.listIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.listIcons.Images.SetKeyName(0, "Database.ico");
            // 
            // helpBtn
            // 
            this.helpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.helpBtn.Image = ((System.Drawing.Image)(resources.GetObject("helpBtn.Image")));
            this.helpBtn.Location = new System.Drawing.Point(633, 0);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(20, 20);
            this.helpBtn.TabIndex = 181;
            this.helpBtn.UseVisualStyleBackColor = true;
            // 
            // selectAll
            // 
            this.selectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectAll.Location = new System.Drawing.Point(15, 377);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(94, 23);
            this.selectAll.TabIndex = 183;
            this.selectAll.Text = "Select All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // deSelectAll
            // 
            this.deSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deSelectAll.Location = new System.Drawing.Point(115, 377);
            this.deSelectAll.Name = "deSelectAll";
            this.deSelectAll.Size = new System.Drawing.Size(94, 23);
            this.deSelectAll.TabIndex = 184;
            this.deSelectAll.Text = "De-select All";
            this.deSelectAll.UseVisualStyleBackColor = true;
            this.deSelectAll.Click += new System.EventHandler(this.deSelectAll_Click);
            // 
            // AddParameter_PreDefined
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 412);
            this.Controls.Add(this.deSelectAll);
            this.Controls.Add(this.selectAll);
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.param_name);
            this.Controls.Add(this.typesCount);
            this.Controls.Add(this.clearSearchBtn);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.createParams);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddParameter_PreDefined";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Parameter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button createParams;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox searchText;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Button clearSearchBtn;
        private System.Windows.Forms.Label typesCount;
        private System.Windows.Forms.ListView param_name;
        private System.Windows.Forms.ColumnHeader funcHeader;
        private System.Windows.Forms.ColumnHeader inheritHeader;
        private System.Windows.Forms.ImageList listIcons;
        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.Button selectAll;
        private System.Windows.Forms.Button deSelectAll;
    }
}