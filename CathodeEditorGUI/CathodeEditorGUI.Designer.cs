namespace CathodeEditorGUI
{
    partial class CathodeEditorGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CathodeEditorGUI));
            this.composite_count_display = new System.Windows.Forms.Label();
            this.root_composite_display = new System.Windows.Forms.Label();
            this.save_commands_pak = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.removeSelectedFlowgraph = new System.Windows.Forms.Button();
            this.addNewFlowgraph = new System.Windows.Forms.Button();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.editEntityMovers = new System.Windows.Forms.Button();
            this.editEntityResources = new System.Windows.Forms.Button();
            this.entityInfoGroup = new System.Windows.Forms.GroupBox();
            this.hierarchyDisplay = new System.Windows.Forms.TextBox();
            this.jumpToComposite = new System.Windows.Forms.Button();
            this.selected_entity_name = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.selected_entity_type_description = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.renameSelectedNode = new System.Windows.Forms.Button();
            this.duplicateSelectedNode = new System.Windows.Forms.Button();
            this.removeSelectedEntity = new System.Windows.Forms.Button();
            this.addNewNode = new System.Windows.Forms.Button();
            this.composite_content = new System.Windows.Forms.ListBox();
            this.entity_search_box = new System.Windows.Forms.TextBox();
            this.node_search_btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.removeParameter = new System.Windows.Forms.Button();
            this.addNewParameter = new System.Windows.Forms.Button();
            this.entity_params = new System.Windows.Forms.Panel();
            this.editFunction = new System.Windows.Forms.Button();
            this.load_commands_pak = new System.Windows.Forms.Button();
            this.env_list = new System.Windows.Forms.ComboBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.DBG_LoadAllCommands = new System.Windows.Forms.Button();
            this.editEntryPoint = new System.Windows.Forms.Button();
            this.DBG_CompileParamList = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.addLinkOut = new System.Windows.Forms.Button();
            this.composite_search = new System.Windows.Forms.TextBox();
            this.composite_search_btn = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.entityInfoGroup.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // composite_count_display
            // 
            this.composite_count_display.AutoSize = true;
            this.composite_count_display.Location = new System.Drawing.Point(6, 28);
            this.composite_count_display.Name = "composite_count_display";
            this.composite_count_display.Size = new System.Drawing.Size(95, 13);
            this.composite_count_display.TabIndex = 172;
            this.composite_count_display.Text = "Composite count:  ";
            // 
            // root_composite_display
            // 
            this.root_composite_display.AutoSize = true;
            this.root_composite_display.Location = new System.Drawing.Point(6, 12);
            this.root_composite_display.Name = "root_composite_display";
            this.root_composite_display.Size = new System.Drawing.Size(84, 13);
            this.root_composite_display.TabIndex = 170;
            this.root_composite_display.Text = "Root composite:";
            // 
            // save_commands_pak
            // 
            this.save_commands_pak.Location = new System.Drawing.Point(391, 16);
            this.save_commands_pak.Name = "save_commands_pak";
            this.save_commands_pak.Size = new System.Drawing.Size(86, 23);
            this.save_commands_pak.TabIndex = 164;
            this.save_commands_pak.Text = "Save";
            this.save_commands_pak.UseVisualStyleBackColor = true;
            this.save_commands_pak.Click += new System.EventHandler(this.save_commands_pak_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.composite_search);
            this.groupBox3.Controls.Add(this.composite_search_btn);
            this.groupBox3.Controls.Add(this.removeSelectedFlowgraph);
            this.groupBox3.Controls.Add(this.addNewFlowgraph);
            this.groupBox3.Controls.Add(this.FileTree);
            this.groupBox3.Location = new System.Drawing.Point(8, 55);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(378, 744);
            this.groupBox3.TabIndex = 163;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Composites";
            // 
            // removeSelectedFlowgraph
            // 
            this.removeSelectedFlowgraph.Location = new System.Drawing.Point(191, 710);
            this.removeSelectedFlowgraph.Name = "removeSelectedFlowgraph";
            this.removeSelectedFlowgraph.Size = new System.Drawing.Size(181, 23);
            this.removeSelectedFlowgraph.TabIndex = 150;
            this.removeSelectedFlowgraph.Text = "Remove Selected";
            this.removeSelectedFlowgraph.UseVisualStyleBackColor = true;
            this.removeSelectedFlowgraph.Click += new System.EventHandler(this.removeSelectedComposite_Click);
            // 
            // addNewFlowgraph
            // 
            this.addNewFlowgraph.Location = new System.Drawing.Point(6, 710);
            this.addNewFlowgraph.Name = "addNewFlowgraph";
            this.addNewFlowgraph.Size = new System.Drawing.Size(181, 23);
            this.addNewFlowgraph.TabIndex = 149;
            this.addNewFlowgraph.Text = "Add Composite";
            this.addNewFlowgraph.UseVisualStyleBackColor = true;
            this.addNewFlowgraph.Click += new System.EventHandler(this.addNewComposite_Click);
            // 
            // FileTree
            // 
            this.FileTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FileTree.Location = new System.Drawing.Point(6, 45);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(366, 658);
            this.FileTree.TabIndex = 99;
            this.FileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTree_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.editEntityMovers);
            this.groupBox1.Controls.Add(this.editEntityResources);
            this.groupBox1.Controls.Add(this.entityInfoGroup);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.editFunction);
            this.groupBox1.Location = new System.Drawing.Point(392, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(857, 744);
            this.groupBox1.TabIndex = 162;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Composite Content";
            // 
            // editEntityMovers
            // 
            this.editEntityMovers.Location = new System.Drawing.Point(469, 94);
            this.editEntityMovers.Name = "editEntityMovers";
            this.editEntityMovers.Size = new System.Drawing.Size(121, 23);
            this.editEntityMovers.TabIndex = 179;
            this.editEntityMovers.Text = "Associated Movers";
            this.editEntityMovers.UseVisualStyleBackColor = true;
            this.editEntityMovers.Click += new System.EventHandler(this.editEntityMovers_Click);
            // 
            // editEntityResources
            // 
            this.editEntityResources.Location = new System.Drawing.Point(600, 94);
            this.editEntityResources.Name = "editEntityResources";
            this.editEntityResources.Size = new System.Drawing.Size(121, 23);
            this.editEntityResources.TabIndex = 176;
            this.editEntityResources.Text = "Attached Resources";
            this.editEntityResources.UseVisualStyleBackColor = true;
            this.editEntityResources.Click += new System.EventHandler(this.editEntityResources_Click);
            // 
            // entityInfoGroup
            // 
            this.entityInfoGroup.Controls.Add(this.hierarchyDisplay);
            this.entityInfoGroup.Controls.Add(this.jumpToComposite);
            this.entityInfoGroup.Controls.Add(this.selected_entity_name);
            this.entityInfoGroup.Controls.Add(this.label9);
            this.entityInfoGroup.Controls.Add(this.selected_entity_type_description);
            this.entityInfoGroup.Controls.Add(this.label6);
            this.entityInfoGroup.Location = new System.Drawing.Point(469, 19);
            this.entityInfoGroup.Name = "entityInfoGroup";
            this.entityInfoGroup.Size = new System.Drawing.Size(382, 69);
            this.entityInfoGroup.TabIndex = 151;
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
            this.jumpToComposite.Click += new System.EventHandler(this.jumpToComposite_Click);
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.renameSelectedNode);
            this.groupBox4.Controls.Add(this.duplicateSelectedNode);
            this.groupBox4.Controls.Add(this.removeSelectedEntity);
            this.groupBox4.Controls.Add(this.addNewNode);
            this.groupBox4.Controls.Add(this.composite_content);
            this.groupBox4.Controls.Add(this.entity_search_box);
            this.groupBox4.Controls.Add(this.node_search_btn);
            this.groupBox4.Location = new System.Drawing.Point(6, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(457, 720);
            this.groupBox4.TabIndex = 148;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Entities";
            // 
            // renameSelectedNode
            // 
            this.renameSelectedNode.Location = new System.Drawing.Point(339, 690);
            this.renameSelectedNode.Name = "renameSelectedNode";
            this.renameSelectedNode.Size = new System.Drawing.Size(111, 23);
            this.renameSelectedNode.TabIndex = 150;
            this.renameSelectedNode.Text = "Rename Selected";
            this.renameSelectedNode.UseVisualStyleBackColor = true;
            this.renameSelectedNode.Click += new System.EventHandler(this.renameSelectedEntity_Click);
            // 
            // duplicateSelectedNode
            // 
            this.duplicateSelectedNode.Location = new System.Drawing.Point(228, 690);
            this.duplicateSelectedNode.Name = "duplicateSelectedNode";
            this.duplicateSelectedNode.Size = new System.Drawing.Size(111, 23);
            this.duplicateSelectedNode.TabIndex = 149;
            this.duplicateSelectedNode.Text = "Duplicate Selected";
            this.duplicateSelectedNode.UseVisualStyleBackColor = true;
            this.duplicateSelectedNode.Click += new System.EventHandler(this.duplicateSelectedEntity_Click);
            // 
            // removeSelectedEntity
            // 
            this.removeSelectedEntity.Location = new System.Drawing.Point(117, 690);
            this.removeSelectedEntity.Name = "removeSelectedEntity";
            this.removeSelectedEntity.Size = new System.Drawing.Size(111, 23);
            this.removeSelectedEntity.TabIndex = 148;
            this.removeSelectedEntity.Text = "Remove Selected";
            this.removeSelectedEntity.UseVisualStyleBackColor = true;
            this.removeSelectedEntity.Click += new System.EventHandler(this.removeSelectedEntity_Click);
            // 
            // addNewNode
            // 
            this.addNewNode.Location = new System.Drawing.Point(6, 690);
            this.addNewNode.Name = "addNewNode";
            this.addNewNode.Size = new System.Drawing.Size(111, 23);
            this.addNewNode.TabIndex = 147;
            this.addNewNode.Text = "Add Entity";
            this.addNewNode.UseVisualStyleBackColor = true;
            this.addNewNode.Click += new System.EventHandler(this.addNewEntity_Click);
            // 
            // composite_content
            // 
            this.composite_content.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.composite_content.FormattingEnabled = true;
            this.composite_content.HorizontalScrollbar = true;
            this.composite_content.Location = new System.Drawing.Point(6, 43);
            this.composite_content.Name = "composite_content";
            this.composite_content.Size = new System.Drawing.Size(444, 641);
            this.composite_content.TabIndex = 144;
            this.composite_content.SelectedIndexChanged += new System.EventHandler(this.composite_content_SelectedIndexChanged);
            // 
            // entity_search_box
            // 
            this.entity_search_box.Location = new System.Drawing.Point(6, 17);
            this.entity_search_box.Name = "entity_search_box";
            this.entity_search_box.Size = new System.Drawing.Size(348, 20);
            this.entity_search_box.TabIndex = 146;
            // 
            // node_search_btn
            // 
            this.node_search_btn.Location = new System.Drawing.Point(360, 15);
            this.node_search_btn.Name = "node_search_btn";
            this.node_search_btn.Size = new System.Drawing.Size(90, 23);
            this.node_search_btn.TabIndex = 145;
            this.node_search_btn.Text = "Search";
            this.node_search_btn.UseVisualStyleBackColor = true;
            this.node_search_btn.Click += new System.EventHandler(this.entity_search_btn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.addLinkOut);
            this.groupBox2.Controls.Add(this.removeParameter);
            this.groupBox2.Controls.Add(this.addNewParameter);
            this.groupBox2.Controls.Add(this.entity_params);
            this.groupBox2.Location = new System.Drawing.Point(469, 123);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 616);
            this.groupBox2.TabIndex = 147;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Entity Parameters";
            // 
            // removeParameter
            // 
            this.removeParameter.Location = new System.Drawing.Point(256, 586);
            this.removeParameter.Name = "removeParameter";
            this.removeParameter.Size = new System.Drawing.Size(125, 23);
            this.removeParameter.TabIndex = 150;
            this.removeParameter.Text = "Remove Param/Link";
            this.removeParameter.UseVisualStyleBackColor = true;
            this.removeParameter.Click += new System.EventHandler(this.removeParameter_Click);
            // 
            // addNewParameter
            // 
            this.addNewParameter.Location = new System.Drawing.Point(6, 586);
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
            this.entity_params.Size = new System.Drawing.Size(375, 560);
            this.entity_params.TabIndex = 0;
            // 
            // editFunction
            // 
            this.editFunction.Location = new System.Drawing.Point(730, 94);
            this.editFunction.Name = "editFunction";
            this.editFunction.Size = new System.Drawing.Size(121, 23);
            this.editFunction.TabIndex = 178;
            this.editFunction.Text = "Edit Function";
            this.toolTip1.SetToolTip(this.editFunction, "Available on TriggerSequence and CAGEAnimation nodes");
            this.editFunction.UseVisualStyleBackColor = true;
            this.editFunction.Click += new System.EventHandler(this.editFunction_Click);
            // 
            // load_commands_pak
            // 
            this.load_commands_pak.Location = new System.Drawing.Point(299, 16);
            this.load_commands_pak.Name = "load_commands_pak";
            this.load_commands_pak.Size = new System.Drawing.Size(86, 23);
            this.load_commands_pak.TabIndex = 160;
            this.load_commands_pak.Text = "Load";
            this.load_commands_pak.UseVisualStyleBackColor = true;
            this.load_commands_pak.Click += new System.EventHandler(this.load_commands_pak_Click);
            // 
            // env_list
            // 
            this.env_list.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.env_list.FormattingEnabled = true;
            this.env_list.Location = new System.Drawing.Point(9, 17);
            this.env_list.Name = "env_list";
            this.env_list.Size = new System.Drawing.Size(284, 21);
            this.env_list.TabIndex = 173;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.DBG_LoadAllCommands);
            this.groupBox8.Controls.Add(this.editEntryPoint);
            this.groupBox8.Controls.Add(this.DBG_CompileParamList);
            this.groupBox8.Controls.Add(this.root_composite_display);
            this.groupBox8.Controls.Add(this.composite_count_display);
            this.groupBox8.Location = new System.Drawing.Point(501, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(748, 49);
            this.groupBox8.TabIndex = 174;
            this.groupBox8.TabStop = false;
            // 
            // DBG_LoadAllCommands
            // 
            this.DBG_LoadAllCommands.ForeColor = System.Drawing.Color.Red;
            this.DBG_LoadAllCommands.Location = new System.Drawing.Point(226, 9);
            this.DBG_LoadAllCommands.Name = "DBG_LoadAllCommands";
            this.DBG_LoadAllCommands.Size = new System.Drawing.Size(192, 23);
            this.DBG_LoadAllCommands.TabIndex = 178;
            this.DBG_LoadAllCommands.Text = "DEBUG: LOAD ALL COMMANDS";
            this.DBG_LoadAllCommands.UseVisualStyleBackColor = true;
            this.DBG_LoadAllCommands.Visible = false;
            this.DBG_LoadAllCommands.Click += new System.EventHandler(this.button2_Click);
            // 
            // editEntryPoint
            // 
            this.editEntryPoint.Location = new System.Drawing.Point(668, 14);
            this.editEntryPoint.Name = "editEntryPoint";
            this.editEntryPoint.Size = new System.Drawing.Size(73, 25);
            this.editEntryPoint.TabIndex = 177;
            this.editEntryPoint.Text = "Edit Root";
            this.editEntryPoint.UseVisualStyleBackColor = true;
            this.editEntryPoint.Click += new System.EventHandler(this.editEntryPoint_Click);
            // 
            // DBG_CompileParamList
            // 
            this.DBG_CompileParamList.ForeColor = System.Drawing.Color.Red;
            this.DBG_CompileParamList.Location = new System.Drawing.Point(424, 9);
            this.DBG_CompileParamList.Name = "DBG_CompileParamList";
            this.DBG_CompileParamList.Size = new System.Drawing.Size(192, 23);
            this.DBG_CompileParamList.TabIndex = 176;
            this.DBG_CompileParamList.Text = "DEBUG: COMPILE PARAMETERS";
            this.DBG_CompileParamList.UseVisualStyleBackColor = true;
            this.DBG_CompileParamList.Visible = false;
            this.DBG_CompileParamList.Click += new System.EventHandler(this.BuildNodeParameterDatabase);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.load_commands_pak);
            this.groupBox10.Controls.Add(this.env_list);
            this.groupBox10.Controls.Add(this.save_commands_pak);
            this.groupBox10.Location = new System.Drawing.Point(8, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(487, 49);
            this.groupBox10.TabIndex = 175;
            this.groupBox10.TabStop = false;
            // 
            // addLinkOut
            // 
            this.addLinkOut.Location = new System.Drawing.Point(131, 586);
            this.addLinkOut.Name = "addLinkOut";
            this.addLinkOut.Size = new System.Drawing.Size(125, 23);
            this.addLinkOut.TabIndex = 151;
            this.addLinkOut.Text = "Add Link Out";
            this.addLinkOut.UseVisualStyleBackColor = true;
            this.addLinkOut.Click += new System.EventHandler(this.addLinkOut_Click);
            // 
            // composite_search
            // 
            this.composite_search.Location = new System.Drawing.Point(6, 19);
            this.composite_search.Name = "composite_search";
            this.composite_search.Size = new System.Drawing.Size(270, 20);
            this.composite_search.TabIndex = 152;
            // 
            // composite_search_btn
            // 
            this.composite_search_btn.Location = new System.Drawing.Point(282, 17);
            this.composite_search_btn.Name = "composite_search_btn";
            this.composite_search_btn.Size = new System.Drawing.Size(90, 23);
            this.composite_search_btn.TabIndex = 151;
            this.composite_search_btn.Text = "Search";
            this.composite_search_btn.UseVisualStyleBackColor = true;
            this.composite_search_btn.Click += new System.EventHandler(this.composite_search_btn_Click);
            // 
            // CathodeEditorGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 804);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CathodeEditorGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenCAGE Cathode Editor (ALPHA)";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.entityInfoGroup.ResumeLayout(false);
            this.entityInfoGroup.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label composite_count_display;
        private System.Windows.Forms.Label root_composite_display;
        private System.Windows.Forms.Button save_commands_pak;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox entityInfoGroup;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label selected_entity_type_description;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox entity_search_box;
        private System.Windows.Forms.Button node_search_btn;
        private System.Windows.Forms.ListBox composite_content;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel entity_params;
        private System.Windows.Forms.Button load_commands_pak;
        private System.Windows.Forms.ComboBox env_list;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label selected_entity_name;
        private System.Windows.Forms.Button jumpToComposite;
        private System.Windows.Forms.Button removeSelectedEntity;
        private System.Windows.Forms.Button addNewNode;
        private System.Windows.Forms.Button removeParameter;
        private System.Windows.Forms.Button addNewParameter;
        private System.Windows.Forms.Button removeSelectedFlowgraph;
        private System.Windows.Forms.Button addNewFlowgraph;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button duplicateSelectedNode;
        private System.Windows.Forms.Button renameSelectedNode;
        private System.Windows.Forms.Button editEntityResources;
        private System.Windows.Forms.Button editEntryPoint;
        private System.Windows.Forms.Button editFunction;
        private System.Windows.Forms.TextBox hierarchyDisplay;
        private System.Windows.Forms.Button editEntityMovers;
        private System.Windows.Forms.Button DBG_CompileParamList;
        private System.Windows.Forms.Button DBG_LoadAllCommands;
        private System.Windows.Forms.Button addLinkOut;
        private System.Windows.Forms.TextBox composite_search;
        private System.Windows.Forms.Button composite_search_btn;
    }
}