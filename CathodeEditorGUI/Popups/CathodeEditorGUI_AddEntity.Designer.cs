namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_AddEntity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_AddEntity));
            this.createDatatypeEntity = new System.Windows.Forms.RadioButton();
            this.createFunctionEntity = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.createOverrideEntity = new System.Windows.Forms.RadioButton();
            this.createProxyEntity = new System.Windows.Forms.RadioButton();
            this.createCompositeEntity = new System.Windows.Forms.RadioButton();
            this.createNewEntity = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.entityVariant = new System.Windows.Forms.ComboBox();
            this.generateHierarchy = new System.Windows.Forms.Button();
            this.addDefaultParams = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // createDatatypeEntity
            // 
            this.createDatatypeEntity.AutoSize = true;
            this.createDatatypeEntity.Location = new System.Drawing.Point(18, 19);
            this.createDatatypeEntity.Name = "createDatatypeEntity";
            this.createDatatypeEntity.Size = new System.Drawing.Size(101, 17);
            this.createDatatypeEntity.TabIndex = 0;
            this.createDatatypeEntity.Text = "DataType Entity";
            this.createDatatypeEntity.UseVisualStyleBackColor = true;
            this.createDatatypeEntity.CheckedChanged += new System.EventHandler(this.selectedDatatypeEntity);
            // 
            // createFunctionEntity
            // 
            this.createFunctionEntity.AutoSize = true;
            this.createFunctionEntity.Checked = true;
            this.createFunctionEntity.Location = new System.Drawing.Point(125, 19);
            this.createFunctionEntity.Name = "createFunctionEntity";
            this.createFunctionEntity.Size = new System.Drawing.Size(95, 17);
            this.createFunctionEntity.TabIndex = 1;
            this.createFunctionEntity.TabStop = true;
            this.createFunctionEntity.Text = "Function Entity";
            this.createFunctionEntity.UseVisualStyleBackColor = true;
            this.createFunctionEntity.CheckedChanged += new System.EventHandler(this.selectedFunctionEntity);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.addDefaultParams);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.createNewEntity);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.entityVariant);
            this.groupBox1.Controls.Add(this.generateHierarchy);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 179);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New Entity";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Entity Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.createOverrideEntity);
            this.groupBox2.Controls.Add(this.createProxyEntity);
            this.groupBox2.Controls.Add(this.createDatatypeEntity);
            this.groupBox2.Controls.Add(this.createCompositeEntity);
            this.groupBox2.Controls.Add(this.createFunctionEntity);
            this.groupBox2.Location = new System.Drawing.Point(104, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(641, 50);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // createOverrideEntity
            // 
            this.createOverrideEntity.AutoSize = true;
            this.createOverrideEntity.Location = new System.Drawing.Point(421, 19);
            this.createOverrideEntity.Name = "createOverrideEntity";
            this.createOverrideEntity.Size = new System.Drawing.Size(94, 17);
            this.createOverrideEntity.TabIndex = 5;
            this.createOverrideEntity.Text = "Override Entity";
            this.createOverrideEntity.UseVisualStyleBackColor = true;
            this.createOverrideEntity.CheckedChanged += new System.EventHandler(this.selectedOverrideEntity);
            // 
            // createProxyEntity
            // 
            this.createProxyEntity.AutoSize = true;
            this.createProxyEntity.Location = new System.Drawing.Point(335, 19);
            this.createProxyEntity.Name = "createProxyEntity";
            this.createProxyEntity.Size = new System.Drawing.Size(80, 17);
            this.createProxyEntity.TabIndex = 4;
            this.createProxyEntity.Text = "Proxy Entity";
            this.createProxyEntity.UseVisualStyleBackColor = true;
            this.createProxyEntity.CheckedChanged += new System.EventHandler(this.selectedProxyEntity);
            // 
            // createCompositeEntity
            // 
            this.createCompositeEntity.AutoSize = true;
            this.createCompositeEntity.Location = new System.Drawing.Point(226, 19);
            this.createCompositeEntity.Name = "createCompositeEntity";
            this.createCompositeEntity.Size = new System.Drawing.Size(103, 17);
            this.createCompositeEntity.TabIndex = 3;
            this.createCompositeEntity.Text = "Composite Entity";
            this.createCompositeEntity.UseVisualStyleBackColor = true;
            this.createCompositeEntity.CheckedChanged += new System.EventHandler(this.selectedCompositeEntity);
            // 
            // createNewEntity
            // 
            this.createNewEntity.Location = new System.Drawing.Point(644, 137);
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
            this.label2.Location = new System.Drawing.Point(40, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Entity Class";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Entity Name";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(104, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(641, 20);
            this.textBox1.TabIndex = 1;
            // 
            // entityVariant
            // 
            this.entityVariant.FormattingEnabled = true;
            this.entityVariant.Items.AddRange(new object[] {
            "POSITION",
            "FLOAT",
            "STRING",
            "SPLINE_DATA",
            "ENUM",
            "SHORT_GUID",
            "FILEPATH",
            "BOOL",
            "DIRECTION",
            "INTEGER"});
            this.entityVariant.Location = new System.Drawing.Point(104, 110);
            this.entityVariant.Name = "entityVariant";
            this.entityVariant.Size = new System.Drawing.Size(641, 21);
            this.entityVariant.TabIndex = 0;
            // 
            // generateHierarchy
            // 
            this.generateHierarchy.Location = new System.Drawing.Point(104, 110);
            this.generateHierarchy.Name = "generateHierarchy";
            this.generateHierarchy.Size = new System.Drawing.Size(641, 23);
            this.generateHierarchy.TabIndex = 6;
            this.generateHierarchy.Text = "Select Entity To Point To";
            this.generateHierarchy.UseVisualStyleBackColor = true;
            this.generateHierarchy.Click += new System.EventHandler(this.generateHierarchy_Click);
            // 
            // addDefaultParams
            // 
            this.addDefaultParams.AutoSize = true;
            this.addDefaultParams.Location = new System.Drawing.Point(44, 143);
            this.addDefaultParams.Name = "addDefaultParams";
            this.addDefaultParams.Size = new System.Drawing.Size(138, 17);
            this.addDefaultParams.TabIndex = 7;
            this.addDefaultParams.Text = "Add Default Parameters";
            this.addDefaultParams.UseVisualStyleBackColor = true;
            // 
            // CathodeEditorGUI_AddEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 200);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CathodeEditorGUI_AddEntity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Entity";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton createDatatypeEntity;
        private System.Windows.Forms.RadioButton createFunctionEntity;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox entityVariant;
        private System.Windows.Forms.RadioButton createCompositeEntity;
        private System.Windows.Forms.Button createNewEntity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton createProxyEntity;
        private System.Windows.Forms.Button generateHierarchy;
        private System.Windows.Forms.RadioButton createOverrideEntity;
        private System.Windows.Forms.CheckBox addDefaultParams;
    }
}