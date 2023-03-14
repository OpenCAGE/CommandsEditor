namespace CommandsEditor
{
    partial class CAGEAnimationEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CAGEAnimationEditor));
            this.animKeyframeValue = new System.Windows.Forms.TextBox();
            this.animKeyframeData = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.endVelY = new System.Windows.Forms.TextBox();
            this.endVelX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.startVelY = new System.Windows.Forms.TextBox();
            this.startVelX = new System.Windows.Forms.TextBox();
            this.animHost = new System.Windows.Forms.Integration.ElementHost();
            this.SaveEntity = new System.Windows.Forms.Button();
            this.eventKeyframeData = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.eventParam2 = new System.Windows.Forms.TextBox();
            this.eventParam1 = new System.Windows.Forms.TextBox();
            this.displaySelection = new System.Windows.Forms.ComboBox();
            this.animKeyframeData.SuspendLayout();
            this.eventKeyframeData.SuspendLayout();
            this.SuspendLayout();
            // 
            // animKeyframeValue
            // 
            this.animKeyframeValue.Location = new System.Drawing.Point(9, 30);
            this.animKeyframeValue.Name = "animKeyframeValue";
            this.animKeyframeValue.Size = new System.Drawing.Size(181, 20);
            this.animKeyframeValue.TabIndex = 9;
            this.animKeyframeValue.TextChanged += new System.EventHandler(this.animKeyframeValue_TextChanged);
            // 
            // animKeyframeData
            // 
            this.animKeyframeData.Controls.Add(this.label7);
            this.animKeyframeData.Controls.Add(this.label6);
            this.animKeyframeData.Controls.Add(this.label5);
            this.animKeyframeData.Controls.Add(this.label3);
            this.animKeyframeData.Controls.Add(this.label4);
            this.animKeyframeData.Controls.Add(this.endVelY);
            this.animKeyframeData.Controls.Add(this.endVelX);
            this.animKeyframeData.Controls.Add(this.label2);
            this.animKeyframeData.Controls.Add(this.label1);
            this.animKeyframeData.Controls.Add(this.startVelY);
            this.animKeyframeData.Controls.Add(this.startVelX);
            this.animKeyframeData.Controls.Add(this.animKeyframeValue);
            this.animKeyframeData.Location = new System.Drawing.Point(661, 473);
            this.animKeyframeData.Name = "animKeyframeData";
            this.animKeyframeData.Size = new System.Drawing.Size(198, 136);
            this.animKeyframeData.TabIndex = 12;
            this.animKeyframeData.TabStop = false;
            this.animKeyframeData.Text = "Animation Keyframe Data";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Value:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "End velocity:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Start velocity:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "X:";
            // 
            // endVelY
            // 
            this.endVelY.Location = new System.Drawing.Point(118, 106);
            this.endVelY.Name = "endVelY";
            this.endVelY.Size = new System.Drawing.Size(72, 20);
            this.endVelY.TabIndex = 17;
            this.endVelY.TextChanged += new System.EventHandler(this.endVelY_TextChanged);
            // 
            // endVelX
            // 
            this.endVelX.Location = new System.Drawing.Point(23, 106);
            this.endVelX.Name = "endVelX";
            this.endVelX.Size = new System.Drawing.Size(72, 20);
            this.endVelX.TabIndex = 16;
            this.endVelX.TextChanged += new System.EventHandler(this.endVelX_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "X:";
            // 
            // startVelY
            // 
            this.startVelY.Location = new System.Drawing.Point(118, 69);
            this.startVelY.Name = "startVelY";
            this.startVelY.Size = new System.Drawing.Size(72, 20);
            this.startVelY.TabIndex = 11;
            this.startVelY.TextChanged += new System.EventHandler(this.startVelY_TextChanged);
            // 
            // startVelX
            // 
            this.startVelX.Location = new System.Drawing.Point(23, 69);
            this.startVelX.Name = "startVelX";
            this.startVelX.Size = new System.Drawing.Size(72, 20);
            this.startVelX.TabIndex = 10;
            this.startVelX.TextChanged += new System.EventHandler(this.startVelX_TextChanged);
            // 
            // animHost
            // 
            this.animHost.Location = new System.Drawing.Point(8, 8);
            this.animHost.Name = "animHost";
            this.animHost.Size = new System.Drawing.Size(851, 459);
            this.animHost.TabIndex = 13;
            this.animHost.Text = "elementHost1";
            this.animHost.Child = null;
            // 
            // SaveEntity
            // 
            this.SaveEntity.Location = new System.Drawing.Point(8, 622);
            this.SaveEntity.Name = "SaveEntity";
            this.SaveEntity.Size = new System.Drawing.Size(111, 23);
            this.SaveEntity.TabIndex = 12;
            this.SaveEntity.Text = "Save";
            this.SaveEntity.UseVisualStyleBackColor = true;
            this.SaveEntity.Click += new System.EventHandler(this.SaveEntity_Click);
            // 
            // eventKeyframeData
            // 
            this.eventKeyframeData.Controls.Add(this.label9);
            this.eventKeyframeData.Controls.Add(this.label8);
            this.eventKeyframeData.Controls.Add(this.eventParam2);
            this.eventKeyframeData.Controls.Add(this.eventParam1);
            this.eventKeyframeData.Location = new System.Drawing.Point(351, 473);
            this.eventKeyframeData.Name = "eventKeyframeData";
            this.eventKeyframeData.Size = new System.Drawing.Size(304, 95);
            this.eventKeyframeData.TabIndex = 14;
            this.eventKeyframeData.TabStop = false;
            this.eventKeyframeData.Text = "Event Keyframe Data";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "????:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Event to trigger:";
            // 
            // eventParam2
            // 
            this.eventParam2.Location = new System.Drawing.Point(6, 67);
            this.eventParam2.Name = "eventParam2";
            this.eventParam2.Size = new System.Drawing.Size(291, 20);
            this.eventParam2.TabIndex = 10;
            this.eventParam2.TextChanged += new System.EventHandler(this.eventParam2_TextChanged);
            // 
            // eventParam1
            // 
            this.eventParam1.Location = new System.Drawing.Point(6, 30);
            this.eventParam1.Name = "eventParam1";
            this.eventParam1.Size = new System.Drawing.Size(291, 20);
            this.eventParam1.TabIndex = 9;
            this.eventParam1.TextChanged += new System.EventHandler(this.eventParam1_TextChanged);
            // 
            // displaySelection
            // 
            this.displaySelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.displaySelection.FormattingEnabled = true;
            this.displaySelection.Items.AddRange(new object[] {
            "Animation Keyframes",
            "Event Keyframes"});
            this.displaySelection.Location = new System.Drawing.Point(12, 518);
            this.displaySelection.Name = "displaySelection";
            this.displaySelection.Size = new System.Drawing.Size(188, 21);
            this.displaySelection.TabIndex = 15;
            this.displaySelection.SelectedIndexChanged += new System.EventHandler(this.displaySelection_SelectedIndexChanged);
            // 
            // CAGEAnimationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 671);
            this.Controls.Add(this.displaySelection);
            this.Controls.Add(this.eventKeyframeData);
            this.Controls.Add(this.SaveEntity);
            this.Controls.Add(this.animHost);
            this.Controls.Add(this.animKeyframeData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CAGEAnimationEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keyframe Editor";
            this.animKeyframeData.ResumeLayout(false);
            this.animKeyframeData.PerformLayout();
            this.eventKeyframeData.ResumeLayout(false);
            this.eventKeyframeData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox animKeyframeValue;
        private System.Windows.Forms.GroupBox animKeyframeData;
        private System.Windows.Forms.Integration.ElementHost animHost;
        private System.Windows.Forms.Button SaveEntity;
        private System.Windows.Forms.TextBox startVelY;
        private System.Windows.Forms.TextBox startVelX;
        private System.Windows.Forms.GroupBox eventKeyframeData;
        private System.Windows.Forms.TextBox eventParam2;
        private System.Windows.Forms.TextBox eventParam1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox endVelY;
        private System.Windows.Forms.TextBox endVelX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox displaySelection;
    }
}