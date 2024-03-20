namespace CommandsEditor
{
    partial class AddEntity_Function
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEntity_Function));
            this.addDefaultParams = new System.Windows.Forms.CheckBox();
            this.createEntity = new System.Windows.Forms.Button();
            this.entityName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.searchText = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.clearSearchBtn = new System.Windows.Forms.Button();
            this.typesCount = new System.Windows.Forms.Label();
            this.functionTypeList = new System.Windows.Forms.ListView();
            this.funcHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inheritHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.entityListIcons = new System.Windows.Forms.ImageList(this.components);
            this.helpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addDefaultParams
            // 
            this.addDefaultParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addDefaultParams.AutoSize = true;
            this.addDefaultParams.Location = new System.Drawing.Point(15, 364);
            this.addDefaultParams.Name = "addDefaultParams";
            this.addDefaultParams.Size = new System.Drawing.Size(138, 17);
            this.addDefaultParams.TabIndex = 15;
            this.addDefaultParams.Text = "Add Default Parameters";
            this.addDefaultParams.UseVisualStyleBackColor = true;
            // 
            // createEntity
            // 
            this.createEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createEntity.Location = new System.Drawing.Point(540, 360);
            this.createEntity.Name = "createEntity";
            this.createEntity.Size = new System.Drawing.Size(101, 23);
            this.createEntity.TabIndex = 6;
            this.createEntity.Text = "Create";
            this.createEntity.UseVisualStyleBackColor = true;
            this.createEntity.Click += new System.EventHandler(this.createEntity_Click);
            // 
            // entityName
            // 
            this.entityName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entityName.Location = new System.Drawing.Point(15, 34);
            this.entityName.Name = "entityName";
            this.entityName.Size = new System.Drawing.Size(626, 20);
            this.entityName.TabIndex = 1;
            this.entityName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CreateEntityOnEnterKey);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 18);
            this.label2.TabIndex = 12;
            this.label2.Text = "Function Type";
            // 
            // searchText
            // 
            this.searchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchText.Location = new System.Drawing.Point(15, 88);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(545, 20);
            this.searchText.TabIndex = 2;
            this.searchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchFuncTypeOnEnterKey);
            // 
            // searchBtn
            // 
            this.searchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.searchBtn.Location = new System.Drawing.Point(578, 88);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(63, 20);
            this.searchBtn.TabIndex = 4;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 18);
            this.label1.TabIndex = 147;
            this.label1.Text = "Entity Name";
            // 
            // clearSearchBtn
            // 
            this.clearSearchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearSearchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearSearchBtn.Image = ((System.Drawing.Image)(resources.GetObject("clearSearchBtn.Image")));
            this.clearSearchBtn.Location = new System.Drawing.Point(559, 88);
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
            this.typesCount.Location = new System.Drawing.Point(537, 72);
            this.typesCount.Name = "typesCount";
            this.typesCount.Size = new System.Drawing.Size(89, 13);
            this.typesCount.TabIndex = 179;
            this.typesCount.Text = "Showing 0 Types";
            this.typesCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // functionTypeList
            // 
            this.functionTypeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.functionTypeList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.funcHeader,
            this.inheritHeader});
            this.functionTypeList.FullRowSelect = true;
            this.functionTypeList.HideSelection = false;
            this.functionTypeList.LargeImageList = this.entityListIcons;
            this.functionTypeList.Location = new System.Drawing.Point(15, 107);
            this.functionTypeList.MultiSelect = false;
            this.functionTypeList.Name = "functionTypeList";
            this.functionTypeList.Size = new System.Drawing.Size(626, 238);
            this.functionTypeList.SmallImageList = this.entityListIcons;
            this.functionTypeList.TabIndex = 180;
            this.functionTypeList.UseCompatibleStateImageBehavior = false;
            this.functionTypeList.View = System.Windows.Forms.View.Details;
            this.functionTypeList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.FunctionTypeList_ColumnClick);
            // 
            // funcHeader
            // 
            this.funcHeader.Text = "Function";
            this.funcHeader.Width = 364;
            // 
            // inheritHeader
            // 
            this.inheritHeader.Text = "Inherits From";
            this.inheritHeader.Width = 232;
            // 
            // entityListIcons
            // 
            this.entityListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("entityListIcons.ImageStream")));
            this.entityListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.entityListIcons.Images.SetKeyName(0, "d_ScriptableObject Icon braces only.png");
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
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // AddEntity_Function
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 400);
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.functionTypeList);
            this.Controls.Add(this.typesCount);
            this.Controls.Add(this.clearSearchBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.addDefaultParams);
            this.Controls.Add(this.createEntity);
            this.Controls.Add(this.entityName);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddEntity_Function";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Function Entity";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox addDefaultParams;
        private System.Windows.Forms.Button createEntity;
        private System.Windows.Forms.TextBox entityName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox searchText;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button clearSearchBtn;
        private System.Windows.Forms.Label typesCount;
        private System.Windows.Forms.ListView functionTypeList;
        private System.Windows.Forms.ColumnHeader funcHeader;
        private System.Windows.Forms.ColumnHeader inheritHeader;
        private System.Windows.Forms.ImageList entityListIcons;
        private System.Windows.Forms.Button helpBtn;
    }
}