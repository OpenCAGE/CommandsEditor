namespace CommandsEditor.Popups
{
    partial class EditCharacterAssets
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCharacterAssets));
            this.assetSetList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.removeSelectedDecal = new System.Windows.Forms.Button();
            this.addNewDecal = new System.Windows.Forms.Button();
            this.decalList = new System.Windows.Forms.ListView();
            this.decalImageList = new System.Windows.Forms.ImageList(this.components);
            this.saveBtn = new System.Windows.Forms.Button();
            this.primaryColourList = new System.Windows.Forms.ListView();
            this.secondaryColourList = new System.Windows.Forms.ListView();
            this.tertiaryColourList = new System.Windows.Forms.ListView();
            this.addNewTertiary = new System.Windows.Forms.Button();
            this.removeSelectedTertiary = new System.Windows.Forms.Button();
            this.removeSelectedSecondary = new System.Windows.Forms.Button();
            this.addNewSecondary = new System.Windows.Forms.Button();
            this.removeSelectedPrimary = new System.Windows.Forms.Button();
            this.addNewPrimary = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // assetSetList
            // 
            this.assetSetList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.assetSetList.FormattingEnabled = true;
            this.assetSetList.Location = new System.Drawing.Point(12, 12);
            this.assetSetList.Name = "assetSetList";
            this.assetSetList.Size = new System.Drawing.Size(645, 21);
            this.assetSetList.TabIndex = 0;
            this.assetSetList.SelectedIndexChanged += new System.EventHandler(this.assetSetList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 413);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Colours";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.removeSelectedTertiary);
            this.groupBox3.Controls.Add(this.addNewTertiary);
            this.groupBox3.Controls.Add(this.tertiaryColourList);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(6, 278);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(219, 125);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tertiary";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.removeSelectedSecondary);
            this.groupBox2.Controls.Add(this.secondaryColourList);
            this.groupBox2.Controls.Add(this.addNewSecondary);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(219, 125);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Secondary";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.removeSelectedPrimary);
            this.groupBox4.Controls.Add(this.primaryColourList);
            this.groupBox4.Controls.Add(this.addNewPrimary);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(7, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(219, 125);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Primary";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.removeSelectedDecal);
            this.groupBox5.Controls.Add(this.addNewDecal);
            this.groupBox5.Controls.Add(this.decalList);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(250, 39);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(407, 413);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Decals";
            // 
            // removeSelectedDecal
            // 
            this.removeSelectedDecal.Location = new System.Drawing.Point(123, 379);
            this.removeSelectedDecal.Name = "removeSelectedDecal";
            this.removeSelectedDecal.Size = new System.Drawing.Size(111, 23);
            this.removeSelectedDecal.TabIndex = 7;
            this.removeSelectedDecal.Text = "Remove Selected";
            this.removeSelectedDecal.UseVisualStyleBackColor = true;
            this.removeSelectedDecal.Click += new System.EventHandler(this.removeSelectedDecal_Click);
            // 
            // addNewDecal
            // 
            this.addNewDecal.Location = new System.Drawing.Point(6, 379);
            this.addNewDecal.Name = "addNewDecal";
            this.addNewDecal.Size = new System.Drawing.Size(111, 23);
            this.addNewDecal.TabIndex = 6;
            this.addNewDecal.Text = "Add New";
            this.addNewDecal.UseVisualStyleBackColor = true;
            this.addNewDecal.Click += new System.EventHandler(this.addNewDecal_Click);
            // 
            // decalList
            // 
            this.decalList.HideSelection = false;
            this.decalList.LargeImageList = this.decalImageList;
            this.decalList.Location = new System.Drawing.Point(6, 19);
            this.decalList.Name = "decalList";
            this.decalList.Size = new System.Drawing.Size(395, 351);
            this.decalList.TabIndex = 0;
            this.decalList.UseCompatibleStateImageBehavior = false;
            this.decalList.View = System.Windows.Forms.View.LargeIcon;
            // 
            // decalImageList
            // 
            this.decalImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.decalImageList.ImageSize = new System.Drawing.Size(64, 64);
            this.decalImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(546, 458);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(111, 23);
            this.saveBtn.TabIndex = 5;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // primaryColourList
            // 
            this.primaryColourList.HideSelection = false;
            this.primaryColourList.Location = new System.Drawing.Point(6, 19);
            this.primaryColourList.Name = "primaryColourList";
            this.primaryColourList.Size = new System.Drawing.Size(204, 73);
            this.primaryColourList.TabIndex = 8;
            this.primaryColourList.UseCompatibleStateImageBehavior = false;
            // 
            // secondaryColourList
            // 
            this.secondaryColourList.HideSelection = false;
            this.secondaryColourList.Location = new System.Drawing.Point(6, 19);
            this.secondaryColourList.Name = "secondaryColourList";
            this.secondaryColourList.Size = new System.Drawing.Size(205, 73);
            this.secondaryColourList.TabIndex = 9;
            this.secondaryColourList.UseCompatibleStateImageBehavior = false;
            // 
            // tertiaryColourList
            // 
            this.tertiaryColourList.HideSelection = false;
            this.tertiaryColourList.Location = new System.Drawing.Point(6, 19);
            this.tertiaryColourList.Name = "tertiaryColourList";
            this.tertiaryColourList.Size = new System.Drawing.Size(206, 73);
            this.tertiaryColourList.TabIndex = 10;
            this.tertiaryColourList.UseCompatibleStateImageBehavior = false;
            // 
            // addNewTertiary
            // 
            this.addNewTertiary.Location = new System.Drawing.Point(6, 96);
            this.addNewTertiary.Name = "addNewTertiary";
            this.addNewTertiary.Size = new System.Drawing.Size(100, 23);
            this.addNewTertiary.TabIndex = 8;
            this.addNewTertiary.Text = "Add New";
            this.addNewTertiary.UseVisualStyleBackColor = true;
            this.addNewTertiary.Click += new System.EventHandler(this.addNewTertiary_Click);
            // 
            // removeSelectedTertiary
            // 
            this.removeSelectedTertiary.Location = new System.Drawing.Point(112, 96);
            this.removeSelectedTertiary.Name = "removeSelectedTertiary";
            this.removeSelectedTertiary.Size = new System.Drawing.Size(100, 23);
            this.removeSelectedTertiary.TabIndex = 8;
            this.removeSelectedTertiary.Text = "Remove Selected";
            this.removeSelectedTertiary.UseVisualStyleBackColor = true;
            this.removeSelectedTertiary.Click += new System.EventHandler(this.removeSelectedTertiary_Click);
            // 
            // removeSelectedSecondary
            // 
            this.removeSelectedSecondary.Location = new System.Drawing.Point(112, 96);
            this.removeSelectedSecondary.Name = "removeSelectedSecondary";
            this.removeSelectedSecondary.Size = new System.Drawing.Size(100, 23);
            this.removeSelectedSecondary.TabIndex = 11;
            this.removeSelectedSecondary.Text = "Remove Selected";
            this.removeSelectedSecondary.UseVisualStyleBackColor = true;
            this.removeSelectedSecondary.Click += new System.EventHandler(this.removeSelectedSecondary_Click);
            // 
            // addNewSecondary
            // 
            this.addNewSecondary.Location = new System.Drawing.Point(6, 96);
            this.addNewSecondary.Name = "addNewSecondary";
            this.addNewSecondary.Size = new System.Drawing.Size(100, 23);
            this.addNewSecondary.TabIndex = 12;
            this.addNewSecondary.Text = "Add New";
            this.addNewSecondary.UseVisualStyleBackColor = true;
            this.addNewSecondary.Click += new System.EventHandler(this.addNewSecondary_Click);
            // 
            // removeSelectedPrimary
            // 
            this.removeSelectedPrimary.Location = new System.Drawing.Point(111, 96);
            this.removeSelectedPrimary.Name = "removeSelectedPrimary";
            this.removeSelectedPrimary.Size = new System.Drawing.Size(100, 23);
            this.removeSelectedPrimary.TabIndex = 11;
            this.removeSelectedPrimary.Text = "Remove Selected";
            this.removeSelectedPrimary.UseVisualStyleBackColor = true;
            this.removeSelectedPrimary.Click += new System.EventHandler(this.removeSelectedPrimary_Click);
            // 
            // addNewPrimary
            // 
            this.addNewPrimary.Location = new System.Drawing.Point(5, 96);
            this.addNewPrimary.Name = "addNewPrimary";
            this.addNewPrimary.Size = new System.Drawing.Size(100, 23);
            this.addNewPrimary.TabIndex = 12;
            this.addNewPrimary.Text = "Add New";
            this.addNewPrimary.UseVisualStyleBackColor = true;
            this.addNewPrimary.Click += new System.EventHandler(this.addNewPrimary_Click);
            // 
            // EditCharacterAssets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 490);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.assetSetList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EditCharacterAssets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Character Asset Sets";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox assetSetList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.ListView decalList;
        private System.Windows.Forms.Button removeSelectedDecal;
        private System.Windows.Forms.Button addNewDecal;
        private System.Windows.Forms.ImageList decalImageList;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView primaryColourList;
        private System.Windows.Forms.ListView tertiaryColourList;
        private System.Windows.Forms.ListView secondaryColourList;
        private System.Windows.Forms.Button removeSelectedPrimary;
        private System.Windows.Forms.Button addNewPrimary;
        private System.Windows.Forms.Button removeSelectedTertiary;
        private System.Windows.Forms.Button addNewTertiary;
        private System.Windows.Forms.Button removeSelectedSecondary;
        private System.Windows.Forms.Button addNewSecondary;
    }
}