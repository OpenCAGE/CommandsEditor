namespace CommandsEditor
{
    partial class AddEntity_Variable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEntity_Variable));
            this.createNewEntity = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.entityVariant = new System.Windows.Forms.ComboBox();
            this.createNode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // createNewEntity
            // 
            this.createNewEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createNewEntity.Location = new System.Drawing.Point(619, 65);
            this.createNewEntity.Name = "createNewEntity";
            this.createNewEntity.Size = new System.Drawing.Size(101, 23);
            this.createNewEntity.TabIndex = 4;
            this.createNewEntity.Text = "Create";
            this.createNewEntity.UseVisualStyleBackColor = true;
            this.createNewEntity.Click += new System.EventHandler(this.createEntity);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Datatype";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(79, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(641, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandler);
            // 
            // entityVariant
            // 
            this.entityVariant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.entityVariant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.entityVariant.FormattingEnabled = true;
            this.entityVariant.Location = new System.Drawing.Point(79, 38);
            this.entityVariant.Name = "entityVariant";
            this.entityVariant.Size = new System.Drawing.Size(641, 21);
            this.entityVariant.TabIndex = 0;
            this.entityVariant.SelectedIndexChanged += new System.EventHandler(this.entityVariant_SelectedIndexChanged);
            // 
            // createNode
            // 
            this.createNode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.createNode.AutoSize = true;
            this.createNode.Location = new System.Drawing.Point(79, 71);
            this.createNode.Name = "createNode";
            this.createNode.Size = new System.Drawing.Size(86, 17);
            this.createNode.TabIndex = 183;
            this.createNode.Text = "Create Node";
            this.createNode.UseVisualStyleBackColor = true;
            this.createNode.CheckedChanged += new System.EventHandler(this.createNode_CheckedChanged);
            // 
            // AddEntity_Variable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 102);
            this.Controls.Add(this.createNode);
            this.Controls.Add(this.createNewEntity);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.entityVariant);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 141);
            this.MinimumSize = new System.Drawing.Size(400, 141);
            this.Name = "AddEntity_Variable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Composite Parameter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox entityVariant;
        private System.Windows.Forms.Button createNewEntity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox createNode;
    }
}