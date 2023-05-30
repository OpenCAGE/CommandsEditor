namespace CommandsEditor
{
    partial class CharacterEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharacterEditor));
            this.shirtComposite = new System.Windows.Forms.TextBox();
            this.selectNewShirt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selectNewTrousers = new System.Windows.Forms.Button();
            this.trousersComposite = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.selectNewShoes = new System.Windows.Forms.Button();
            this.shoesComposite = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.selectNewHead = new System.Windows.Forms.Button();
            this.headComposite = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.selectNewArms = new System.Windows.Forms.Button();
            this.armsComposite = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.selectNewCollision = new System.Windows.Forms.Button();
            this.collisionComposite = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.skeletons = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.bodyTypes = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.characterInstances = new System.Windows.Forms.ComboBox();
            this.modelRendererHost = new System.Windows.Forms.Integration.ElementHost();
            this.addNewCharacter = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // shirtComposite
            // 
            this.shirtComposite.Location = new System.Drawing.Point(19, 82);
            this.shirtComposite.Name = "shirtComposite";
            this.shirtComposite.ReadOnly = true;
            this.shirtComposite.Size = new System.Drawing.Size(378, 20);
            this.shirtComposite.TabIndex = 0;
            // 
            // selectNewShirt
            // 
            this.selectNewShirt.Location = new System.Drawing.Point(403, 80);
            this.selectNewShirt.Name = "selectNewShirt";
            this.selectNewShirt.Size = new System.Drawing.Size(94, 23);
            this.selectNewShirt.TabIndex = 1;
            this.selectNewShirt.Text = "Change";
            this.selectNewShirt.UseVisualStyleBackColor = true;
            this.selectNewShirt.Click += new System.EventHandler(this.selectNewShirt_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Shirt";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Trousers";
            // 
            // selectNewTrousers
            // 
            this.selectNewTrousers.Location = new System.Drawing.Point(403, 158);
            this.selectNewTrousers.Name = "selectNewTrousers";
            this.selectNewTrousers.Size = new System.Drawing.Size(94, 23);
            this.selectNewTrousers.TabIndex = 4;
            this.selectNewTrousers.Text = "Change";
            this.selectNewTrousers.UseVisualStyleBackColor = true;
            this.selectNewTrousers.Click += new System.EventHandler(this.selectNewTrousers_Click);
            // 
            // trousersComposite
            // 
            this.trousersComposite.Location = new System.Drawing.Point(19, 160);
            this.trousersComposite.Name = "trousersComposite";
            this.trousersComposite.ReadOnly = true;
            this.trousersComposite.Size = new System.Drawing.Size(378, 20);
            this.trousersComposite.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Shoes";
            // 
            // selectNewShoes
            // 
            this.selectNewShoes.Location = new System.Drawing.Point(403, 197);
            this.selectNewShoes.Name = "selectNewShoes";
            this.selectNewShoes.Size = new System.Drawing.Size(94, 23);
            this.selectNewShoes.TabIndex = 7;
            this.selectNewShoes.Text = "Change";
            this.selectNewShoes.UseVisualStyleBackColor = true;
            this.selectNewShoes.Click += new System.EventHandler(this.selectNewShoes_Click);
            // 
            // shoesComposite
            // 
            this.shoesComposite.Location = new System.Drawing.Point(19, 199);
            this.shoesComposite.Name = "shoesComposite";
            this.shoesComposite.ReadOnly = true;
            this.shoesComposite.Size = new System.Drawing.Size(378, 20);
            this.shoesComposite.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Head";
            // 
            // selectNewHead
            // 
            this.selectNewHead.Location = new System.Drawing.Point(403, 41);
            this.selectNewHead.Name = "selectNewHead";
            this.selectNewHead.Size = new System.Drawing.Size(94, 23);
            this.selectNewHead.TabIndex = 10;
            this.selectNewHead.Text = "Change";
            this.selectNewHead.UseVisualStyleBackColor = true;
            this.selectNewHead.Click += new System.EventHandler(this.selectNewHead_Click);
            // 
            // headComposite
            // 
            this.headComposite.Location = new System.Drawing.Point(19, 43);
            this.headComposite.Name = "headComposite";
            this.headComposite.ReadOnly = true;
            this.headComposite.Size = new System.Drawing.Size(378, 20);
            this.headComposite.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Arms";
            // 
            // selectNewArms
            // 
            this.selectNewArms.Location = new System.Drawing.Point(403, 119);
            this.selectNewArms.Name = "selectNewArms";
            this.selectNewArms.Size = new System.Drawing.Size(94, 23);
            this.selectNewArms.TabIndex = 13;
            this.selectNewArms.Text = "Change";
            this.selectNewArms.UseVisualStyleBackColor = true;
            this.selectNewArms.Click += new System.EventHandler(this.selectNewArms_Click);
            // 
            // armsComposite
            // 
            this.armsComposite.Location = new System.Drawing.Point(19, 121);
            this.armsComposite.Name = "armsComposite";
            this.armsComposite.ReadOnly = true;
            this.armsComposite.Size = new System.Drawing.Size(378, 20);
            this.armsComposite.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Collision";
            // 
            // selectNewCollision
            // 
            this.selectNewCollision.Location = new System.Drawing.Point(403, 236);
            this.selectNewCollision.Name = "selectNewCollision";
            this.selectNewCollision.Size = new System.Drawing.Size(94, 23);
            this.selectNewCollision.TabIndex = 16;
            this.selectNewCollision.Text = "Change";
            this.selectNewCollision.UseVisualStyleBackColor = true;
            this.selectNewCollision.Click += new System.EventHandler(this.selectNewCollision_Click);
            // 
            // collisionComposite
            // 
            this.collisionComposite.Location = new System.Drawing.Point(19, 238);
            this.collisionComposite.Name = "collisionComposite";
            this.collisionComposite.ReadOnly = true;
            this.collisionComposite.Size = new System.Drawing.Size(378, 20);
            this.collisionComposite.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.shirtComposite);
            this.groupBox1.Controls.Add(this.selectNewCollision);
            this.groupBox1.Controls.Add(this.selectNewShirt);
            this.groupBox1.Controls.Add(this.collisionComposite);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.trousersComposite);
            this.groupBox1.Controls.Add(this.selectNewArms);
            this.groupBox1.Controls.Add(this.selectNewTrousers);
            this.groupBox1.Controls.Add(this.armsComposite);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.shoesComposite);
            this.groupBox1.Controls.Add(this.selectNewHead);
            this.groupBox1.Controls.Add(this.selectNewShoes);
            this.groupBox1.Controls.Add(this.headComposite);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(521, 280);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Composites";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.skeletons);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.bodyTypes);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(12, 39);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(521, 126);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Skeletons";
            // 
            // skeletons
            // 
            this.skeletons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skeletons.FormattingEnabled = true;
            this.skeletons.Location = new System.Drawing.Point(22, 83);
            this.skeletons.Name = "skeletons";
            this.skeletons.Size = new System.Drawing.Size(475, 21);
            this.skeletons.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Body";
            // 
            // bodyTypes
            // 
            this.bodyTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bodyTypes.FormattingEnabled = true;
            this.bodyTypes.Location = new System.Drawing.Point(22, 43);
            this.bodyTypes.Name = "bodyTypes";
            this.bodyTypes.Size = new System.Drawing.Size(475, 21);
            this.bodyTypes.TabIndex = 12;
            this.bodyTypes.SelectedIndexChanged += new System.EventHandler(this.bodyTypes_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Face";
            // 
            // characterInstances
            // 
            this.characterInstances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.characterInstances.FormattingEnabled = true;
            this.characterInstances.Location = new System.Drawing.Point(12, 12);
            this.characterInstances.Name = "characterInstances";
            this.characterInstances.Size = new System.Drawing.Size(801, 21);
            this.characterInstances.TabIndex = 15;
            this.characterInstances.SelectedIndexChanged += new System.EventHandler(this.characterInstances_SelectedIndexChanged);
            // 
            // modelRendererHost
            // 
            this.modelRendererHost.Location = new System.Drawing.Point(539, 40);
            this.modelRendererHost.Name = "modelRendererHost";
            this.modelRendererHost.Size = new System.Drawing.Size(407, 411);
            this.modelRendererHost.TabIndex = 22;
            this.modelRendererHost.Text = "elementHost1";
            this.modelRendererHost.Child = null;
            // 
            // addNewCharacter
            // 
            this.addNewCharacter.Location = new System.Drawing.Point(819, 11);
            this.addNewCharacter.Name = "addNewCharacter";
            this.addNewCharacter.Size = new System.Drawing.Size(127, 23);
            this.addNewCharacter.TabIndex = 23;
            this.addNewCharacter.Text = "Add New";
            this.addNewCharacter.UseVisualStyleBackColor = true;
            this.addNewCharacter.Click += new System.EventHandler(this.addNewCharacter_Click);
            // 
            // CharacterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 462);
            this.Controls.Add(this.addNewCharacter);
            this.Controls.Add(this.modelRendererHost);
            this.Controls.Add(this.characterInstances);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CharacterEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Character Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox shirtComposite;
        private System.Windows.Forms.Button selectNewShirt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button selectNewTrousers;
        private System.Windows.Forms.TextBox trousersComposite;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button selectNewShoes;
        private System.Windows.Forms.TextBox shoesComposite;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button selectNewHead;
        private System.Windows.Forms.TextBox headComposite;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button selectNewArms;
        private System.Windows.Forms.TextBox armsComposite;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button selectNewCollision;
        private System.Windows.Forms.TextBox collisionComposite;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox skeletons;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox bodyTypes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox characterInstances;
        private System.Windows.Forms.Integration.ElementHost modelRendererHost;
        private System.Windows.Forms.Button addNewCharacter;
    }
}