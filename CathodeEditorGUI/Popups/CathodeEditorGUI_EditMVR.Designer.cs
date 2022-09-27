namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI_EditMVR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI_EditMVR));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.visible = new System.Windows.Forms.CheckBox();
            this.renderable = new Popups.UserControls.GUI_Resource_RenderableInstance();
            this.transform = new UserControls.GUI_TransformDataType();
            this.typeInfo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.visibleInfo = new System.Windows.Forms.TextBox();
            this.saveMover = new System.Windows.Forms.Button();
            this.deleteMover = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(205, 303);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // visible
            // 
            this.visible.AutoSize = true;
            this.visible.Location = new System.Drawing.Point(687, 250);
            this.visible.Name = "visible";
            this.visible.Size = new System.Drawing.Size(67, 17);
            this.visible.TabIndex = 3;
            this.visible.Text = "Is Visible";
            this.visible.UseVisualStyleBackColor = true;
            this.visible.Visible = false;
            // 
            // renderable
            // 
            this.renderable.Location = new System.Drawing.Point(223, 12);
            this.renderable.Name = "renderable";
            this.renderable.Size = new System.Drawing.Size(838, 186);
            this.renderable.TabIndex = 2;
            // 
            // transform
            // 
            this.transform.Location = new System.Drawing.Point(223, 204);
            this.transform.Name = "transform";
            this.transform.Size = new System.Drawing.Size(340, 113);
            this.transform.TabIndex = 1;
            this.transform.Visible = false;
            // 
            // typeInfo
            // 
            this.typeInfo.Location = new System.Drawing.Point(581, 224);
            this.typeInfo.Name = "typeInfo";
            this.typeInfo.Size = new System.Drawing.Size(100, 20);
            this.typeInfo.TabIndex = 4;
            this.typeInfo.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(578, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Type";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(684, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Is Visible";
            this.label2.Visible = false;
            // 
            // visibleInfo
            // 
            this.visibleInfo.Location = new System.Drawing.Point(687, 224);
            this.visibleInfo.Name = "visibleInfo";
            this.visibleInfo.Size = new System.Drawing.Size(100, 20);
            this.visibleInfo.TabIndex = 6;
            this.visibleInfo.Visible = false;
            // 
            // saveMover
            // 
            this.saveMover.Location = new System.Drawing.Point(639, 283);
            this.saveMover.Name = "saveMover";
            this.saveMover.Size = new System.Drawing.Size(208, 34);
            this.saveMover.TabIndex = 8;
            this.saveMover.Text = "Save This Mover";
            this.saveMover.UseVisualStyleBackColor = true;
            this.saveMover.Visible = false;
            this.saveMover.Click += new System.EventHandler(this.saveMover_Click);
            // 
            // deleteMover
            // 
            this.deleteMover.Enabled = false;
            this.deleteMover.Location = new System.Drawing.Point(853, 283);
            this.deleteMover.Name = "deleteMover";
            this.deleteMover.Size = new System.Drawing.Size(208, 34);
            this.deleteMover.TabIndex = 9;
            this.deleteMover.Text = "Delete This Mover";
            this.deleteMover.UseVisualStyleBackColor = true;
            this.deleteMover.Visible = false;
            this.deleteMover.Click += new System.EventHandler(this.deleteMover_Click);
            // 
            // CathodeEditorGUI_EditMVR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 328);
            this.Controls.Add(this.deleteMover);
            this.Controls.Add(this.saveMover);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.visibleInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.typeInfo);
            this.Controls.Add(this.visible);
            this.Controls.Add(this.renderable);
            this.Controls.Add(this.transform);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CathodeEditorGUI_EditMVR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Mover Descriptors";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private UserControls.GUI_TransformDataType transform;
        private Popups.UserControls.GUI_Resource_RenderableInstance renderable;
        private System.Windows.Forms.CheckBox visible;
        private System.Windows.Forms.TextBox typeInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox visibleInfo;
        private System.Windows.Forms.Button saveMover;
        private System.Windows.Forms.Button deleteMover;
    }
}