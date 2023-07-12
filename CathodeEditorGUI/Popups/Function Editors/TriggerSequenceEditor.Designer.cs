namespace CommandsEditor
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
            this.entity_list = new System.Windows.Forms.ListBox();
            this.trigger_list = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deleteSelectedTrigger = new System.Windows.Forms.Button();
            this.addNewTrigger = new System.Windows.Forms.Button();
            this.selectedEntityDetails = new System.Windows.Forms.GroupBox();
            this.entityTriggerDelay = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.entityHierarchy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectEntToPointTo = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.selectedTriggerDetails = new System.Windows.Forms.GroupBox();
            this.saveTrigger = new System.Windows.Forms.Button();
            this.triggerEndParam = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.triggerStartParam = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.deleteParamTrigger = new System.Windows.Forms.Button();
            this.addNewParamTrigger = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.moveUp = new System.Windows.Forms.Button();
            this.moveDown = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.selectedEntityDetails.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.selectedTriggerDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // entity_list
            // 
            this.entity_list.FormattingEnabled = true;
            this.entity_list.Location = new System.Drawing.Point(6, 19);
            this.entity_list.Name = "entity_list";
            this.entity_list.Size = new System.Drawing.Size(695, 290);
            this.entity_list.TabIndex = 0;
            this.entity_list.SelectedIndexChanged += new System.EventHandler(this.trigger_list_SelectedIndexChanged);
            // 
            // trigger_list
            // 
            this.trigger_list.FormattingEnabled = true;
            this.trigger_list.Location = new System.Drawing.Point(6, 19);
            this.trigger_list.Name = "trigger_list";
            this.trigger_list.Size = new System.Drawing.Size(695, 290);
            this.trigger_list.TabIndex = 2;
            this.trigger_list.SelectedIndexChanged += new System.EventHandler(this.trigger_list_SelectedIndexChanged_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.moveDown);
            this.groupBox1.Controls.Add(this.moveUp);
            this.groupBox1.Controls.Add(this.deleteSelectedTrigger);
            this.groupBox1.Controls.Add(this.addNewTrigger);
            this.groupBox1.Controls.Add(this.selectedEntityDetails);
            this.groupBox1.Controls.Add(this.entity_list);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1194, 315);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Entities";
            // 
            // deleteSelectedTrigger
            // 
            this.deleteSelectedTrigger.Location = new System.Drawing.Point(707, 283);
            this.deleteSelectedTrigger.Name = "deleteSelectedTrigger";
            this.deleteSelectedTrigger.Size = new System.Drawing.Size(170, 26);
            this.deleteSelectedTrigger.TabIndex = 3;
            this.deleteSelectedTrigger.Text = "Delete Selected Entity";
            this.deleteSelectedTrigger.UseVisualStyleBackColor = true;
            this.deleteSelectedTrigger.Click += new System.EventHandler(this.deleteSelectedEntity_Click);
            // 
            // addNewTrigger
            // 
            this.addNewTrigger.Location = new System.Drawing.Point(707, 251);
            this.addNewTrigger.Name = "addNewTrigger";
            this.addNewTrigger.Size = new System.Drawing.Size(170, 26);
            this.addNewTrigger.TabIndex = 2;
            this.addNewTrigger.Text = "Add New Entity";
            this.addNewTrigger.UseVisualStyleBackColor = true;
            this.addNewTrigger.Click += new System.EventHandler(this.addNewEntity_Click);
            // 
            // selectedEntityDetails
            // 
            this.selectedEntityDetails.Controls.Add(this.entityTriggerDelay);
            this.selectedEntityDetails.Controls.Add(this.label2);
            this.selectedEntityDetails.Controls.Add(this.entityHierarchy);
            this.selectedEntityDetails.Controls.Add(this.label1);
            this.selectedEntityDetails.Controls.Add(this.selectEntToPointTo);
            this.selectedEntityDetails.Location = new System.Drawing.Point(707, 13);
            this.selectedEntityDetails.Name = "selectedEntityDetails";
            this.selectedEntityDetails.Size = new System.Drawing.Size(481, 136);
            this.selectedEntityDetails.TabIndex = 1;
            this.selectedEntityDetails.TabStop = false;
            this.selectedEntityDetails.Text = "Selected Entity Details";
            // 
            // entityTriggerDelay
            // 
            this.entityTriggerDelay.Location = new System.Drawing.Point(17, 96);
            this.entityTriggerDelay.Name = "entityTriggerDelay";
            this.entityTriggerDelay.Size = new System.Drawing.Size(444, 20);
            this.entityTriggerDelay.TabIndex = 6;
            this.entityTriggerDelay.TextChanged += new System.EventHandler(this.triggerDelay_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Time To Wait Before Triggering (In Seconds)";
            // 
            // entityHierarchy
            // 
            this.entityHierarchy.Location = new System.Drawing.Point(17, 42);
            this.entityHierarchy.Name = "entityHierarchy";
            this.entityHierarchy.ReadOnly = true;
            this.entityHierarchy.Size = new System.Drawing.Size(444, 20);
            this.entityHierarchy.TabIndex = 3;
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
            this.selectEntToPointTo.Location = new System.Drawing.Point(334, 68);
            this.selectEntToPointTo.Name = "selectEntToPointTo";
            this.selectEntToPointTo.Size = new System.Drawing.Size(127, 23);
            this.selectEntToPointTo.TabIndex = 1;
            this.selectEntToPointTo.Text = "Select Entity";
            this.selectEntToPointTo.UseVisualStyleBackColor = true;
            this.selectEntToPointTo.Click += new System.EventHandler(this.selectEntToPointTo_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.selectedTriggerDetails);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.deleteParamTrigger);
            this.groupBox2.Controls.Add(this.trigger_list);
            this.groupBox2.Controls.Add(this.addNewParamTrigger);
            this.groupBox2.Location = new System.Drawing.Point(12, 333);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1194, 315);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Triggers";
            // 
            // selectedTriggerDetails
            // 
            this.selectedTriggerDetails.Controls.Add(this.saveTrigger);
            this.selectedTriggerDetails.Controls.Add(this.triggerEndParam);
            this.selectedTriggerDetails.Controls.Add(this.label4);
            this.selectedTriggerDetails.Controls.Add(this.triggerStartParam);
            this.selectedTriggerDetails.Controls.Add(this.label5);
            this.selectedTriggerDetails.Location = new System.Drawing.Point(707, 13);
            this.selectedTriggerDetails.Name = "selectedTriggerDetails";
            this.selectedTriggerDetails.Size = new System.Drawing.Size(481, 147);
            this.selectedTriggerDetails.TabIndex = 7;
            this.selectedTriggerDetails.TabStop = false;
            this.selectedTriggerDetails.Text = "Selected Trigger Details";
            // 
            // saveTrigger
            // 
            this.saveTrigger.Location = new System.Drawing.Point(369, 108);
            this.saveTrigger.Name = "saveTrigger";
            this.saveTrigger.Size = new System.Drawing.Size(92, 23);
            this.saveTrigger.TabIndex = 7;
            this.saveTrigger.Text = "Save";
            this.saveTrigger.UseVisualStyleBackColor = true;
            this.saveTrigger.Click += new System.EventHandler(this.saveTrigger_Click);
            // 
            // triggerEndParam
            // 
            this.triggerEndParam.Location = new System.Drawing.Point(17, 82);
            this.triggerEndParam.Name = "triggerEndParam";
            this.triggerEndParam.Size = new System.Drawing.Size(444, 20);
            this.triggerEndParam.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Parameter To Trigger On End";
            // 
            // triggerStartParam
            // 
            this.triggerStartParam.Location = new System.Drawing.Point(17, 42);
            this.triggerStartParam.Name = "triggerStartParam";
            this.triggerStartParam.Size = new System.Drawing.Size(444, 20);
            this.triggerStartParam.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Parameter To Trigger On Start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(883, 251);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(239, 52);
            this.label3.TabIndex = 6;
            this.label3.Text = "This is a list of supported start/end parameters\r\nwhich can be triggered by this " +
    "TriggerSequence. \r\n\r\nWill apply to all entities in the list above.";
            // 
            // deleteParamTrigger
            // 
            this.deleteParamTrigger.Location = new System.Drawing.Point(707, 283);
            this.deleteParamTrigger.Name = "deleteParamTrigger";
            this.deleteParamTrigger.Size = new System.Drawing.Size(170, 26);
            this.deleteParamTrigger.TabIndex = 5;
            this.deleteParamTrigger.Text = "Delete Selected Trigger";
            this.deleteParamTrigger.UseVisualStyleBackColor = true;
            this.deleteParamTrigger.Click += new System.EventHandler(this.deleteParamTrigger_Click);
            // 
            // addNewParamTrigger
            // 
            this.addNewParamTrigger.Location = new System.Drawing.Point(707, 251);
            this.addNewParamTrigger.Name = "addNewParamTrigger";
            this.addNewParamTrigger.Size = new System.Drawing.Size(170, 26);
            this.addNewParamTrigger.TabIndex = 4;
            this.addNewParamTrigger.Text = "Add New Trigger";
            this.addNewParamTrigger.UseVisualStyleBackColor = true;
            this.addNewParamTrigger.Click += new System.EventHandler(this.addNewParamTrigger_Click);
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
            // moveUp
            // 
            this.moveUp.Location = new System.Drawing.Point(707, 171);
            this.moveUp.Name = "moveUp";
            this.moveUp.Size = new System.Drawing.Size(170, 26);
            this.moveUp.TabIndex = 4;
            this.moveUp.Text = "Move Selected Entity Up";
            this.moveUp.UseVisualStyleBackColor = true;
            this.moveUp.Click += new System.EventHandler(this.moveUp_Click);
            // 
            // moveDown
            // 
            this.moveDown.Location = new System.Drawing.Point(707, 203);
            this.moveDown.Name = "moveDown";
            this.moveDown.Size = new System.Drawing.Size(170, 26);
            this.moveDown.TabIndex = 5;
            this.moveDown.Text = "Move Selected Entity Down";
            this.moveDown.UseVisualStyleBackColor = true;
            this.moveDown.Click += new System.EventHandler(this.moveDown_Click);
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
            this.MaximizeBox = false;
            this.Name = "TriggerSequenceEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TriggerSequence Editor";
            this.groupBox1.ResumeLayout(false);
            this.selectedEntityDetails.ResumeLayout(false);
            this.selectedEntityDetails.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.selectedTriggerDetails.ResumeLayout(false);
            this.selectedTriggerDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox entity_list;
        private System.Windows.Forms.ListBox trigger_list;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox selectedEntityDetails;
        private System.Windows.Forms.TextBox entityHierarchy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectEntToPointTo;
        private System.Windows.Forms.Button deleteSelectedTrigger;
        private System.Windows.Forms.Button addNewTrigger;
        private System.Windows.Forms.TextBox entityTriggerDelay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox selectedTriggerDetails;
        private System.Windows.Forms.TextBox triggerEndParam;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox triggerStartParam;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button deleteParamTrigger;
        private System.Windows.Forms.Button addNewParamTrigger;
        private System.Windows.Forms.Button saveTrigger;
        private System.Windows.Forms.Button moveDown;
        private System.Windows.Forms.Button moveUp;
    }
}