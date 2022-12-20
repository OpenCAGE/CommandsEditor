namespace CathodeEditorGUI
{
    partial class TriggerSequenceEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriggerSequenceEditor));
            this.trigger_list = new System.Windows.Forms.ListBox();
            this.event_list = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deleteSelectedTrigger = new System.Windows.Forms.Button();
            this.addNewTrigger = new System.Windows.Forms.Button();
            this.selectedTriggerDetails = new System.Windows.Forms.GroupBox();
            this.saveTriggerTime = new System.Windows.Forms.Button();
            this.triggerDelay = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.triggerHierarchy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectEntToPointTo = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.selectedTriggerDetails.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // trigger_list
            // 
            this.trigger_list.FormattingEnabled = true;
            this.trigger_list.Location = new System.Drawing.Point(6, 19);
            this.trigger_list.Name = "trigger_list";
            this.trigger_list.Size = new System.Drawing.Size(695, 290);
            this.trigger_list.TabIndex = 0;
            this.trigger_list.SelectedIndexChanged += new System.EventHandler(this.trigger_list_SelectedIndexChanged);
            // 
            // event_list
            // 
            this.event_list.FormattingEnabled = true;
            this.event_list.Location = new System.Drawing.Point(6, 19);
            this.event_list.Name = "event_list";
            this.event_list.Size = new System.Drawing.Size(695, 290);
            this.event_list.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deleteSelectedTrigger);
            this.groupBox1.Controls.Add(this.addNewTrigger);
            this.groupBox1.Controls.Add(this.selectedTriggerDetails);
            this.groupBox1.Controls.Add(this.trigger_list);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1194, 315);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Entities";
            // 
            // deleteSelectedTrigger
            // 
            this.deleteSelectedTrigger.Location = new System.Drawing.Point(707, 276);
            this.deleteSelectedTrigger.Name = "deleteSelectedTrigger";
            this.deleteSelectedTrigger.Size = new System.Drawing.Size(170, 26);
            this.deleteSelectedTrigger.TabIndex = 3;
            this.deleteSelectedTrigger.Text = "Delete Selected Trigger";
            this.deleteSelectedTrigger.UseVisualStyleBackColor = true;
            this.deleteSelectedTrigger.Click += new System.EventHandler(this.deleteSelectedTrigger_Click);
            // 
            // addNewTrigger
            // 
            this.addNewTrigger.Location = new System.Drawing.Point(707, 244);
            this.addNewTrigger.Name = "addNewTrigger";
            this.addNewTrigger.Size = new System.Drawing.Size(170, 26);
            this.addNewTrigger.TabIndex = 2;
            this.addNewTrigger.Text = "Add New Trigger";
            this.addNewTrigger.UseVisualStyleBackColor = true;
            this.addNewTrigger.Click += new System.EventHandler(this.addNewTrigger_Click);
            // 
            // selectedTriggerDetails
            // 
            this.selectedTriggerDetails.Controls.Add(this.saveTriggerTime);
            this.selectedTriggerDetails.Controls.Add(this.triggerDelay);
            this.selectedTriggerDetails.Controls.Add(this.label2);
            this.selectedTriggerDetails.Controls.Add(this.triggerHierarchy);
            this.selectedTriggerDetails.Controls.Add(this.label1);
            this.selectedTriggerDetails.Controls.Add(this.selectEntToPointTo);
            this.selectedTriggerDetails.Location = new System.Drawing.Point(707, 46);
            this.selectedTriggerDetails.Name = "selectedTriggerDetails";
            this.selectedTriggerDetails.Size = new System.Drawing.Size(481, 161);
            this.selectedTriggerDetails.TabIndex = 1;
            this.selectedTriggerDetails.TabStop = false;
            this.selectedTriggerDetails.Text = "Selected Trigger Details";
            // 
            // saveTriggerTime
            // 
            this.saveTriggerTime.Location = new System.Drawing.Point(340, 114);
            this.saveTriggerTime.Name = "saveTriggerTime";
            this.saveTriggerTime.Size = new System.Drawing.Size(121, 23);
            this.saveTriggerTime.TabIndex = 7;
            this.saveTriggerTime.Text = "Save Time";
            this.saveTriggerTime.UseVisualStyleBackColor = true;
            this.saveTriggerTime.Click += new System.EventHandler(this.saveTriggerTime_Click);
            // 
            // triggerDelay
            // 
            this.triggerDelay.Location = new System.Drawing.Point(17, 116);
            this.triggerDelay.Name = "triggerDelay";
            this.triggerDelay.Size = new System.Drawing.Size(317, 20);
            this.triggerDelay.TabIndex = 6;
            this.triggerDelay.TextChanged += new System.EventHandler(this.triggerDelay_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Time To Wait Before Triggering (In Seconds)";
            // 
            // triggerHierarchy
            // 
            this.triggerHierarchy.Location = new System.Drawing.Point(17, 42);
            this.triggerHierarchy.Name = "triggerHierarchy";
            this.triggerHierarchy.ReadOnly = true;
            this.triggerHierarchy.Size = new System.Drawing.Size(444, 20);
            this.triggerHierarchy.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Entity To Trigger";
            // 
            // selectEntToPointTo
            // 
            this.selectEntToPointTo.Location = new System.Drawing.Point(17, 66);
            this.selectEntToPointTo.Name = "selectEntToPointTo";
            this.selectEntToPointTo.Size = new System.Drawing.Size(444, 23);
            this.selectEntToPointTo.TabIndex = 1;
            this.selectEntToPointTo.Text = "Select Entity To Point To";
            this.selectEntToPointTo.UseVisualStyleBackColor = true;
            this.selectEntToPointTo.Click += new System.EventHandler(this.selectEntToPointTo_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.event_list);
            this.groupBox2.Location = new System.Drawing.Point(12, 333);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1194, 315);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Triggers";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1063, 654);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 32);
            this.button1.TabIndex = 6;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TriggerSequenceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 695);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TriggerSequenceEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TriggerSequence Editor";
            this.groupBox1.ResumeLayout(false);
            this.selectedTriggerDetails.ResumeLayout(false);
            this.selectedTriggerDetails.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox trigger_list;
        private System.Windows.Forms.ListBox event_list;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox selectedTriggerDetails;
        private System.Windows.Forms.TextBox triggerHierarchy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectEntToPointTo;
        private System.Windows.Forms.Button deleteSelectedTrigger;
        private System.Windows.Forms.Button addNewTrigger;
        private System.Windows.Forms.TextBox triggerDelay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveTriggerTime;
    }
}