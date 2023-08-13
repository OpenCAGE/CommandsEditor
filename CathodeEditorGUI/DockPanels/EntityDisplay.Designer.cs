namespace CommandsEditor.DockPanels
{
    partial class EntityDisplay
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
            this.goToZone = new System.Windows.Forms.Button();
            this.showOverridesAndProxies = new System.Windows.Forms.Button();
            this.editEntityMovers = new System.Windows.Forms.Button();
            this.editEntityResources = new System.Windows.Forms.Button();
            this.entityInfoGroup = new System.Windows.Forms.GroupBox();
            this.hierarchyDisplay = new System.Windows.Forms.TextBox();
            this.jumpToComposite = new System.Windows.Forms.Button();
            this.selected_entity_name = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.selected_entity_type_description = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.editFunction = new System.Windows.Forms.Button();
            this.entityParamGroup = new System.Windows.Forms.GroupBox();
            this.addLinkOut = new System.Windows.Forms.Button();
            this.removeParameter = new System.Windows.Forms.Button();
            this.addNewParameter = new System.Windows.Forms.Button();
            this.entity_params = new System.Windows.Forms.Panel();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.entityInfoGroup.SuspendLayout();
            this.entityParamGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // goToZone
            // 
            this.goToZone.Location = new System.Drawing.Point(245, 87);
            this.goToZone.Name = "goToZone";
            this.goToZone.Size = new System.Drawing.Size(73, 23);
            this.goToZone.TabIndex = 188;
            this.goToZone.Text = "Zone";
            this.goToZone.UseVisualStyleBackColor = true;
            // 
            // showOverridesAndProxies
            // 
            this.showOverridesAndProxies.Location = new System.Drawing.Point(89, 87);
            this.showOverridesAndProxies.Name = "showOverridesAndProxies";
            this.showOverridesAndProxies.Size = new System.Drawing.Size(73, 23);
            this.showOverridesAndProxies.TabIndex = 187;
            this.showOverridesAndProxies.Text = "References";
            this.showOverridesAndProxies.UseVisualStyleBackColor = true;
            // 
            // editEntityMovers
            // 
            this.editEntityMovers.Location = new System.Drawing.Point(11, 87);
            this.editEntityMovers.Name = "editEntityMovers";
            this.editEntityMovers.Size = new System.Drawing.Size(73, 23);
            this.editEntityMovers.TabIndex = 186;
            this.editEntityMovers.Text = "Movers";
            this.editEntityMovers.UseVisualStyleBackColor = true;
            // 
            // editEntityResources
            // 
            this.editEntityResources.Location = new System.Drawing.Point(167, 87);
            this.editEntityResources.Name = "editEntityResources";
            this.editEntityResources.Size = new System.Drawing.Size(73, 23);
            this.editEntityResources.TabIndex = 184;
            this.editEntityResources.Text = "Resources";
            this.editEntityResources.UseVisualStyleBackColor = true;
            // 
            // entityInfoGroup
            // 
            this.entityInfoGroup.Controls.Add(this.hierarchyDisplay);
            this.entityInfoGroup.Controls.Add(this.jumpToComposite);
            this.entityInfoGroup.Controls.Add(this.selected_entity_name);
            this.entityInfoGroup.Controls.Add(this.label9);
            this.entityInfoGroup.Controls.Add(this.selected_entity_type_description);
            this.entityInfoGroup.Controls.Add(this.label6);
            this.entityInfoGroup.Location = new System.Drawing.Point(12, 12);
            this.entityInfoGroup.Name = "entityInfoGroup";
            this.entityInfoGroup.Size = new System.Drawing.Size(382, 69);
            this.entityInfoGroup.TabIndex = 183;
            this.entityInfoGroup.TabStop = false;
            this.entityInfoGroup.Text = "Selected Entity Info";
            // 
            // hierarchyDisplay
            // 
            this.hierarchyDisplay.Location = new System.Drawing.Point(6, 37);
            this.hierarchyDisplay.Name = "hierarchyDisplay";
            this.hierarchyDisplay.ReadOnly = true;
            this.hierarchyDisplay.Size = new System.Drawing.Size(323, 20);
            this.hierarchyDisplay.TabIndex = 9;
            // 
            // jumpToComposite
            // 
            this.jumpToComposite.Location = new System.Drawing.Point(335, 15);
            this.jumpToComposite.Name = "jumpToComposite";
            this.jumpToComposite.Size = new System.Drawing.Size(39, 45);
            this.jumpToComposite.TabIndex = 8;
            this.jumpToComposite.Text = "Go To";
            this.jumpToComposite.UseVisualStyleBackColor = true;
            this.jumpToComposite.Visible = false;
            // 
            // selected_entity_name
            // 
            this.selected_entity_name.AutoSize = true;
            this.selected_entity_name.Location = new System.Drawing.Point(54, 22);
            this.selected_entity_name.Name = "selected_entity_name";
            this.selected_entity_name.Size = new System.Drawing.Size(0, 13);
            this.selected_entity_name.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Name: ";
            // 
            // selected_entity_type_description
            // 
            this.selected_entity_type_description.AutoSize = true;
            this.selected_entity_type_description.Location = new System.Drawing.Point(54, 41);
            this.selected_entity_type_description.Name = "selected_entity_type_description";
            this.selected_entity_type_description.Size = new System.Drawing.Size(0, 13);
            this.selected_entity_type_description.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Type:";
            // 
            // editFunction
            // 
            this.editFunction.Location = new System.Drawing.Point(323, 87);
            this.editFunction.Name = "editFunction";
            this.editFunction.Size = new System.Drawing.Size(73, 23);
            this.editFunction.TabIndex = 185;
            this.editFunction.Text = "Function";
            this.editFunction.UseVisualStyleBackColor = true;
            // 
            // entityParamGroup
            // 
            this.entityParamGroup.Controls.Add(this.addLinkOut);
            this.entityParamGroup.Controls.Add(this.removeParameter);
            this.entityParamGroup.Controls.Add(this.addNewParameter);
            this.entityParamGroup.Controls.Add(this.entity_params);
            this.entityParamGroup.Location = new System.Drawing.Point(12, 116);
            this.entityParamGroup.Name = "entityParamGroup";
            this.entityParamGroup.Size = new System.Drawing.Size(382, 616);
            this.entityParamGroup.TabIndex = 182;
            this.entityParamGroup.TabStop = false;
            this.entityParamGroup.Text = "Selected Entity Parameters";
            // 
            // addLinkOut
            // 
            this.addLinkOut.Location = new System.Drawing.Point(131, 585);
            this.addLinkOut.Name = "addLinkOut";
            this.addLinkOut.Size = new System.Drawing.Size(125, 23);
            this.addLinkOut.TabIndex = 151;
            this.addLinkOut.Text = "Add Link Out";
            this.addLinkOut.UseVisualStyleBackColor = true;
            this.addLinkOut.Click += new System.EventHandler(this.addLinkOut_Click);
            // 
            // removeParameter
            // 
            this.removeParameter.Location = new System.Drawing.Point(256, 585);
            this.removeParameter.Name = "removeParameter";
            this.removeParameter.Size = new System.Drawing.Size(125, 23);
            this.removeParameter.TabIndex = 150;
            this.removeParameter.Text = "Remove Param/Link";
            this.removeParameter.UseVisualStyleBackColor = true;
            this.removeParameter.Click += new System.EventHandler(this.removeParameter_Click);
            // 
            // addNewParameter
            // 
            this.addNewParameter.Location = new System.Drawing.Point(6, 585);
            this.addNewParameter.Name = "addNewParameter";
            this.addNewParameter.Size = new System.Drawing.Size(125, 23);
            this.addNewParameter.TabIndex = 149;
            this.addNewParameter.Text = "Add Parameter";
            this.addNewParameter.UseVisualStyleBackColor = true;
            this.addNewParameter.Click += new System.EventHandler(this.addNewParameter_Click);
            // 
            // entity_params
            // 
            this.entity_params.AutoScroll = true;
            this.entity_params.Location = new System.Drawing.Point(6, 20);
            this.entity_params.Name = "entity_params";
            this.entity_params.Size = new System.Drawing.Size(375, 559);
            this.entity_params.TabIndex = 0;
            // 
            // EntityDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 742);
            this.Controls.Add(this.goToZone);
            this.Controls.Add(this.showOverridesAndProxies);
            this.Controls.Add(this.editEntityMovers);
            this.Controls.Add(this.editEntityResources);
            this.Controls.Add(this.entityInfoGroup);
            this.Controls.Add(this.editFunction);
            this.Controls.Add(this.entityParamGroup);
            this.Name = "EntityDisplay";
            this.Text = "Selected Entity";
            this.entityInfoGroup.ResumeLayout(false);
            this.entityInfoGroup.PerformLayout();
            this.entityParamGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button goToZone;
        private System.Windows.Forms.Button showOverridesAndProxies;
        private System.Windows.Forms.Button editEntityMovers;
        private System.Windows.Forms.Button editEntityResources;
        private System.Windows.Forms.GroupBox entityInfoGroup;
        private System.Windows.Forms.TextBox hierarchyDisplay;
        private System.Windows.Forms.Button jumpToComposite;
        private System.Windows.Forms.Label selected_entity_name;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label selected_entity_type_description;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button editFunction;
        private System.Windows.Forms.GroupBox entityParamGroup;
        private System.Windows.Forms.Button addLinkOut;
        private System.Windows.Forms.Button removeParameter;
        private System.Windows.Forms.Button addNewParameter;
        private System.Windows.Forms.Panel entity_params;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
    }
}