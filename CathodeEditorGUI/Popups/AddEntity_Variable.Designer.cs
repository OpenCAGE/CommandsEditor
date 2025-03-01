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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEntity_Variable));
            this.createVariable = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.variableName = new System.Windows.Forms.TextBox();
            this.variableType = new System.Windows.Forms.ComboBox();
            this.createNode = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.variableDirection = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // createVariable
            // 
            this.createVariable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createVariable.Location = new System.Drawing.Point(619, 96);
            this.createVariable.Name = "createVariable";
            this.createVariable.Size = new System.Drawing.Size(101, 23);
            this.createVariable.TabIndex = 5;
            this.createVariable.Text = "Create";
            this.createVariable.UseVisualStyleBackColor = true;
            this.createVariable.Click += new System.EventHandler(this.createEntity);
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
            // variableName
            // 
            this.variableName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.variableName.Location = new System.Drawing.Point(79, 12);
            this.variableName.Name = "variableName";
            this.variableName.Size = new System.Drawing.Size(641, 20);
            this.variableName.TabIndex = 1;
            this.variableName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandler);
            // 
            // variableType
            // 
            this.variableType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.variableType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.variableType.FormattingEnabled = true;
            this.variableType.Location = new System.Drawing.Point(79, 38);
            this.variableType.Name = "variableType";
            this.variableType.Size = new System.Drawing.Size(641, 21);
            this.variableType.TabIndex = 2;
            this.variableType.SelectedIndexChanged += new System.EventHandler(this.entityVariant_SelectedIndexChanged);
            // 
            // createNode
            // 
            this.createNode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.createNode.AutoSize = true;
            this.createNode.Location = new System.Drawing.Point(79, 100);
            this.createNode.Name = "createNode";
            this.createNode.Size = new System.Drawing.Size(86, 17);
            this.createNode.TabIndex = 4;
            this.createNode.Text = "Create Node";
            this.createNode.UseVisualStyleBackColor = true;
            this.createNode.CheckedChanged += new System.EventHandler(this.createNode_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 185;
            this.label3.Text = "Direction";
            // 
            // variableDirection
            // 
            this.variableDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.variableDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.variableDirection.FormattingEnabled = true;
            this.variableDirection.Items.AddRange(new object[] {
            "IN",
            "OUT"});
            this.variableDirection.Location = new System.Drawing.Point(79, 65);
            this.variableDirection.Name = "variableDirection";
            this.variableDirection.Size = new System.Drawing.Size(641, 21);
            this.variableDirection.TabIndex = 3;
            this.toolTip1.SetToolTip(this.variableDirection, "Specifies which way the data flows for this parameter. If out, it\'ll appear as a " +
        "pin on the right of any instances of the composite, and vice versa.");
            // 
            // AddEntity_Variable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 131);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.variableDirection);
            this.Controls.Add(this.createNode);
            this.Controls.Add(this.createVariable);
            this.Controls.Add(this.variableName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.variableType);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 170);
            this.MinimumSize = new System.Drawing.Size(400, 141);
            this.Name = "AddEntity_Variable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Composite Parameter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox variableType;
        private System.Windows.Forms.Button createVariable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox variableName;
        private System.Windows.Forms.CheckBox createNode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox variableDirection;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}