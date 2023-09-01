namespace CommandsEditor.UserControls
{
    partial class GUI_Link
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.group = new System.Windows.Forms.GroupBox();
            this.DeleteLink = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.EditLink = new System.Windows.Forms.Button();
            this.GoTo = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.group.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // group
            // 
            this.group.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.group.Controls.Add(this.tableLayoutPanel2);
            this.group.Controls.Add(this.label1);
            this.group.Controls.Add(this.textBox1);
            this.group.Location = new System.Drawing.Point(3, 2);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(334, 91);
            this.group.TabIndex = 19;
            this.group.TabStop = false;
            this.group.Text = " ";
            // 
            // DeleteLink
            // 
            this.DeleteLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeleteLink.Location = new System.Drawing.Point(221, 3);
            this.DeleteLink.Name = "DeleteLink";
            this.DeleteLink.Size = new System.Drawing.Size(104, 22);
            this.DeleteLink.TabIndex = 5;
            this.DeleteLink.Text = "Delete Link";
            this.DeleteLink.UseVisualStyleBackColor = true;
            this.DeleteLink.Click += new System.EventHandler(this.DeleteLink_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(6, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(322, 20);
            this.textBox1.TabIndex = 3;
            // 
            // EditLink
            // 
            this.EditLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditLink.Location = new System.Drawing.Point(112, 3);
            this.EditLink.Name = "EditLink";
            this.EditLink.Size = new System.Drawing.Size(103, 22);
            this.EditLink.TabIndex = 2;
            this.EditLink.Text = "Edit Link";
            this.EditLink.UseVisualStyleBackColor = true;
            this.EditLink.Click += new System.EventHandler(this.EditLink_Click);
            // 
            // GoTo
            // 
            this.GoTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GoTo.Location = new System.Drawing.Point(3, 3);
            this.GoTo.Name = "GoTo";
            this.GoTo.Size = new System.Drawing.Size(103, 22);
            this.GoTo.TabIndex = 1;
            this.GoTo.Text = "Go To Link";
            this.GoTo.UseVisualStyleBackColor = true;
            this.GoTo.Click += new System.EventHandler(this.GoTo_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.DeleteLink, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.EditLink, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.GoTo, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 58);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(328, 28);
            this.tableLayoutPanel2.TabIndex = 20;
            // 
            // GUI_Link
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.group);
            this.Name = "GUI_Link";
            this.Size = new System.Drawing.Size(340, 94);
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.Button GoTo;
        private System.Windows.Forms.Button EditLink;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DeleteLink;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
