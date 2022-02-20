namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_AddNode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_AddNode));
            this.createDatatypeEntity = new System.Windows.Forms.RadioButton();
            this.createFunctionEntity = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.createFlowgraphEntity = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.entityVariant = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.createDatatypeEntity.AutoSize = true;
            this.createDatatypeEntity.Location = new System.Drawing.Point(18, 19);
            this.createDatatypeEntity.Name = "radioButton1";
            this.createDatatypeEntity.Size = new System.Drawing.Size(101, 17);
            this.createDatatypeEntity.TabIndex = 0;
            this.createDatatypeEntity.Text = "DataType Entity";
            this.createDatatypeEntity.UseVisualStyleBackColor = true;
            this.createDatatypeEntity.CheckedChanged += new System.EventHandler(this.selectedDatatypeEntity);
            // 
            // radioButton2
            // 
            this.createFunctionEntity.AutoSize = true;
            this.createFunctionEntity.Checked = true;
            this.createFunctionEntity.Location = new System.Drawing.Point(125, 19);
            this.createFunctionEntity.Name = "radioButton2";
            this.createFunctionEntity.Size = new System.Drawing.Size(95, 17);
            this.createFunctionEntity.TabIndex = 1;
            this.createFunctionEntity.TabStop = true;
            this.createFunctionEntity.Text = "Function Entity";
            this.createFunctionEntity.UseVisualStyleBackColor = true;
            this.createFunctionEntity.CheckedChanged += new System.EventHandler(this.selectedFunctionEntity);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.entityVariant);
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
            this.groupBox2.Controls.Add(this.createDatatypeEntity);
            this.groupBox2.Controls.Add(this.createFlowgraphEntity);
            this.groupBox2.Controls.Add(this.createFunctionEntity);
            this.groupBox2.Location = new System.Drawing.Point(104, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(641, 50);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // radioButton3
            // 
            this.createFlowgraphEntity.AutoSize = true;
            this.createFlowgraphEntity.Location = new System.Drawing.Point(226, 19);
            this.createFlowgraphEntity.Name = "radioButton3";
            this.createFlowgraphEntity.Size = new System.Drawing.Size(103, 17);
            this.createFlowgraphEntity.TabIndex = 3;
            this.createFlowgraphEntity.Text = "Flowgraph Entity";
            this.createFlowgraphEntity.UseVisualStyleBackColor = true;
            this.createFlowgraphEntity.CheckedChanged += new System.EventHandler(this.selectedFlowgraphEntity);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(644, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // comboBox1
            // 
            this.entityVariant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.entityVariant.Name = "comboBox1";
            this.entityVariant.Size = new System.Drawing.Size(641, 21);
            this.entityVariant.TabIndex = 0;
            // 
            // CathodeEditorGUI_AddNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 200);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CathodeEditorGUI_AddNode";
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
        private System.Windows.Forms.RadioButton createFlowgraphEntity;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}