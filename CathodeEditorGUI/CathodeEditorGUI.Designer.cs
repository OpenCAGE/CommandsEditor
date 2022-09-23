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
            this.editTriggerSequence = new System.Windows.Forms.Button();
            this.editCAGEAnimationKeyframes = new System.Windows.Forms.Button();
            this.editCompositeResources = new System.Windows.Forms.Button();
            this.editEntityResources = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.hierarchyDisplay = new System.Windows.Forms.TextBox();
            this.jumpToComposite = new System.Windows.Forms.Button();
            this.selected_entity_name = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.selected_entity_type_description = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.selected_entity_type = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.selected_entity_id = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.showLinkParents = new System.Windows.Forms.Button();
            this.removeSelectedLink = new System.Windows.Forms.Button();
            this.addNewLink = new System.Windows.Forms.Button();
            this.out_pin_goto = new System.Windows.Forms.Button();
            this.entity_children = new System.Windows.Forms.ListBox();
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
            this.button1 = new System.Windows.Forms.Button();
            this.load_commands_pak = new System.Windows.Forms.Button();
            this.env_list = new System.Windows.Forms.ComboBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.editEntryPoint = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.modifyMVR = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.editEntityMovers = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
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
            this.FileTree.Location = new System.Drawing.Point(6, 19);
            this.FileTree.Name = "FileTree";
            this.FileTree.Size = new System.Drawing.Size(366, 684);
            this.FileTree.TabIndex = 99;
            this.FileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FileTree_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.editEntityMovers);
            this.groupBox1.Controls.Add(this.editTriggerSequence);
            this.groupBox1.Controls.Add(this.editCAGEAnimationKeyframes);
            this.groupBox1.Controls.Add(this.editCompositeResources);
            this.groupBox1.Controls.Add(this.editEntityResources);
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(392, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(857, 744);
            this.groupBox1.TabIndex = 162;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Composite Content";
            // 
            // editTriggerSequence
            // 
            this.editTriggerSequence.Location = new System.Drawing.Point(729, 121);
            this.editTriggerSequence.Name = "editTriggerSequence";
            this.editTriggerSequence.Size = new System.Drawing.Size(121, 23);
            this.editTriggerSequence.TabIndex = 178;
            this.editTriggerSequence.Text = "Edit TriggerSequence";
            this.editTriggerSequence.UseVisualStyleBackColor = true;
            this.editTriggerSequence.Visible = false;
            this.editTriggerSequence.Click += new System.EventHandler(this.editTriggerSequence_Click);
            // 
            // editCAGEAnimationKeyframes
            // 
            this.editCAGEAnimationKeyframes.Location = new System.Drawing.Point(729, 121);
            this.editCAGEAnimationKeyframes.Name = "editCAGEAnimationKeyframes";
            this.editCAGEAnimationKeyframes.Size = new System.Drawing.Size(121, 23);
            this.editCAGEAnimationKeyframes.TabIndex = 175;
            this.editCAGEAnimationKeyframes.Text = "Edit CAGEAnimation";
            this.editCAGEAnimationKeyframes.UseVisualStyleBackColor = true;
            this.editCAGEAnimationKeyframes.Visible = false;
            this.editCAGEAnimationKeyframes.Click += new System.EventHandler(this.editCAGEAnimationKeyframes_Click);
            // 
            // editCompositeResources
            // 
            this.editCompositeResources.Location = new System.Drawing.Point(366, -1);
            this.editCompositeResources.Name = "editCompositeResources";
            this.editCompositeResources.Size = new System.Drawing.Size(99, 23);
            this.editCompositeResources.TabIndex = 177;
            this.editCompositeResources.Text = "Edit Resources";
            this.editCompositeResources.UseVisualStyleBackColor = true;
            this.editCompositeResources.Visible = false;
            this.editCompositeResources.Click += new System.EventHandler(this.editCompositeResources_Click);
            // 
            // editEntityResources
            // 
            this.editEntityResources.Location = new System.Drawing.Point(625, 121);
            this.editEntityResources.Name = "editEntityResources";
            this.editEntityResources.Size = new System.Drawing.Size(99, 23);
            this.editEntityResources.TabIndex = 176;
            this.editEntityResources.Text = "Edit Resources";
            this.editEntityResources.UseVisualStyleBackColor = true;
            this.editEntityResources.Visible = false;
            this.editEntityResources.Click += new System.EventHandler(this.editEntityResources_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.hierarchyDisplay);
            this.groupBox7.Controls.Add(this.jumpToComposite);
            this.groupBox7.Controls.Add(this.selected_entity_name);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Controls.Add(this.selected_entity_type_description);
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Controls.Add(this.selected_entity_type);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.selected_entity_id);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Location = new System.Drawing.Point(469, 19);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(382, 105);
            this.groupBox7.TabIndex = 151;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Selected Entity Info";
            // 
            // hierarchyDisplay
            // 
            this.hierarchyDisplay.Location = new System.Drawing.Point(10, 76);
            this.hierarchyDisplay.Name = "hierarchyDisplay";
            this.hierarchyDisplay.ReadOnly = true;
            this.hierarchyDisplay.Size = new System.Drawing.Size(364, 20);
            this.hierarchyDisplay.TabIndex = 9;
            // 
            // jumpToComposite
            // 
            this.jumpToComposite.Location = new System.Drawing.Point(339, 15);
            this.jumpToComposite.Name = "jumpToComposite";
            this.jumpToComposite.Size = new System.Drawing.Size(35, 47);
            this.jumpToComposite.TabIndex = 8;
            this.jumpToComposite.Text = "Go To";
            this.jumpToComposite.UseVisualStyleBackColor = true;
            this.jumpToComposite.Visible = false;
            this.jumpToComposite.Click += new System.EventHandler(this.jumpToComposite_Click);
            // 
            // selected_entity_name
            // 
            this.selected_entity_name.AutoSize = true;
            this.selected_entity_name.Location = new System.Drawing.Point(86, 63);
            this.selected_entity_name.Name = "selected_entity_name";
            this.selected_entity_name.Size = new System.Drawing.Size(0, 13);
            this.selected_entity_name.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(7, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Node Name: ";
            // 
            // selected_entity_type_description
            // 
            this.selected_entity_type_description.AutoSize = true;
            this.selected_entity_type_description.Location = new System.Drawing.Point(151, 80);
            this.selected_entity_type_description.Name = "selected_entity_type_description";
            this.selected_entity_type_description.Size = new System.Drawing.Size(0, 13);
            this.selected_entity_type_description.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(7, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Node Type Description:";
            // 
            // selected_entity_type
            // 
            this.selected_entity_type.AutoSize = true;
            this.selected_entity_type.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selected_entity_type.Location = new System.Drawing.Point(83, 38);
            this.selected_entity_type.Name = "selected_entity_type";
            this.selected_entity_type.Size = new System.Drawing.Size(0, 13);
            this.selected_entity_type.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Node Type:";
            // 
            // selected_entity_id
            // 
            this.selected_entity_id.AutoSize = true;
            this.selected_entity_id.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selected_entity_id.Location = new System.Drawing.Point(69, 22);
            this.selected_entity_id.Name = "selected_entity_id";
            this.selected_entity_id.Size = new System.Drawing.Size(0, 13);
            this.selected_entity_id.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Node ID: ";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.showLinkParents);
            this.groupBox5.Controls.Add(this.removeSelectedLink);
            this.groupBox5.Controls.Add(this.addNewLink);
            this.groupBox5.Controls.Add(this.out_pin_goto);
            this.groupBox5.Controls.Add(this.entity_children);
            this.groupBox5.Location = new System.Drawing.Point(469, 600);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(382, 139);
            this.groupBox5.TabIndex = 149;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Selected Entity Links";
            // 
            // showLinkParents
            // 
            this.showLinkParents.Location = new System.Drawing.Point(298, 109);
            this.showLinkParents.Name = "showLinkParents";
            this.showLinkParents.Size = new System.Drawing.Size(79, 23);
            this.showLinkParents.TabIndex = 150;
            this.showLinkParents.Text = "Parents";
            this.showLinkParents.UseVisualStyleBackColor = true;
            this.showLinkParents.Click += new System.EventHandler(this.showLinkParents_Click);
            // 
            // removeSelectedLink
            // 
            this.removeSelectedLink.Location = new System.Drawing.Point(100, 109);
            this.removeSelectedLink.Name = "removeSelectedLink";
            this.removeSelectedLink.Size = new System.Drawing.Size(94, 23);
            this.removeSelectedLink.TabIndex = 148;
            this.removeSelectedLink.Text = "Remove Link";
            this.removeSelectedLink.UseVisualStyleBackColor = true;
            this.removeSelectedLink.Click += new System.EventHandler(this.removeSelectedLink_Click);
            // 
            // addNewLink
            // 
            this.addNewLink.Location = new System.Drawing.Point(6, 109);
            this.addNewLink.Name = "addNewLink";
            this.addNewLink.Size = new System.Drawing.Size(94, 23);
            this.addNewLink.TabIndex = 149;
            this.addNewLink.Text = "Add New Link";
            this.addNewLink.UseVisualStyleBackColor = true;
            this.addNewLink.Click += new System.EventHandler(this.addNewLink_Click);
            // 
            // out_pin_goto
            // 
            this.out_pin_goto.Location = new System.Drawing.Point(194, 109);
            this.out_pin_goto.Name = "out_pin_goto";
            this.out_pin_goto.Size = new System.Drawing.Size(94, 23);
            this.out_pin_goto.TabIndex = 146;
            this.out_pin_goto.Text = "Go To Link";
            this.out_pin_goto.UseVisualStyleBackColor = true;
            this.out_pin_goto.Click += new System.EventHandler(this.out_pin_goto_Click);
            // 
            // entity_children
            // 
            this.entity_children.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entity_children.FormattingEnabled = true;
            this.entity_children.HorizontalScrollbar = true;
            this.entity_children.Location = new System.Drawing.Point(6, 21);
            this.entity_children.Name = "entity_children";
            this.entity_children.Size = new System.Drawing.Size(371, 82);
            this.entity_children.TabIndex = 145;
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
            this.groupBox2.Controls.Add(this.removeParameter);
            this.groupBox2.Controls.Add(this.addNewParameter);
            this.groupBox2.Controls.Add(this.entity_params);
            this.groupBox2.Location = new System.Drawing.Point(469, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 464);
            this.groupBox2.TabIndex = 147;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Entity Parameters";
            // 
            // removeParameter
            // 
            this.removeParameter.Location = new System.Drawing.Point(193, 434);
            this.removeParameter.Name = "removeParameter";
            this.removeParameter.Size = new System.Drawing.Size(184, 23);
            this.removeParameter.TabIndex = 150;
            this.removeParameter.Text = "Remove Parameter";
            this.removeParameter.UseVisualStyleBackColor = true;
            this.removeParameter.Click += new System.EventHandler(this.removeParameter_Click);
            // 
            // addNewParameter
            // 
            this.addNewParameter.Location = new System.Drawing.Point(6, 434);
            this.addNewParameter.Name = "addNewParameter";
            this.addNewParameter.Size = new System.Drawing.Size(184, 23);
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
            this.entity_params.Size = new System.Drawing.Size(371, 408);
            this.entity_params.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(424, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(192, 23);
            this.button1.TabIndex = 176;
            this.button1.Text = "DEBUG: Purge all dead hierarchies.";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
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
            this.groupBox8.Controls.Add(this.button6);
            this.groupBox8.Controls.Add(this.button5);
            this.groupBox8.Controls.Add(this.button4);
            this.groupBox8.Controls.Add(this.button2);
            this.groupBox8.Controls.Add(this.editEntryPoint);
            this.groupBox8.Controls.Add(this.button1);
            this.groupBox8.Controls.Add(this.root_composite_display);
            this.groupBox8.Controls.Add(this.composite_count_display);
            this.groupBox8.Location = new System.Drawing.Point(554, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(695, 49);
            this.groupBox8.TabIndex = 174;
            this.groupBox8.TabStop = false;
            // 
            // button6
            // 
            this.button6.ForeColor = System.Drawing.Color.Red;
            this.button6.Location = new System.Drawing.Point(96, 9);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(127, 23);
            this.button6.TabIndex = 181;
            this.button6.Text = "DEBUG: decomp dump";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.ForeColor = System.Drawing.Color.Red;
            this.button5.Location = new System.Drawing.Point(72, 34);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(182, 23);
            this.button5.TabIndex = 180;
            this.button5.Text = "DEBUG: PARSE DAN DUMP";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.ForeColor = System.Drawing.Color.Red;
            this.button4.Location = new System.Drawing.Point(260, 34);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(116, 23);
            this.button4.TabIndex = 180;
            this.button4.Text = "DEBUG: purge all";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.ForeColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(228, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(192, 23);
            this.button2.TabIndex = 178;
            this.button2.Text = "DEBUG: Clear unknown header vals";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // editEntryPoint
            // 
            this.editEntryPoint.Location = new System.Drawing.Point(619, 9);
            this.editEntryPoint.Name = "editEntryPoint";
            this.editEntryPoint.Size = new System.Drawing.Size(73, 23);
            this.editEntryPoint.TabIndex = 177;
            this.editEntryPoint.Text = "Edit Root";
            this.editEntryPoint.UseVisualStyleBackColor = true;
            this.editEntryPoint.Click += new System.EventHandler(this.editEntryPoint_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.load_commands_pak);
            this.groupBox10.Controls.Add(this.modifyMVR);
            this.groupBox10.Controls.Add(this.env_list);
            this.groupBox10.Controls.Add(this.save_commands_pak);
            this.groupBox10.Location = new System.Drawing.Point(8, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(540, 49);
            this.groupBox10.TabIndex = 175;
            this.groupBox10.TabStop = false;
            // 
            // modifyMVR
            // 
            this.modifyMVR.AutoSize = true;
            this.modifyMVR.Location = new System.Drawing.Point(483, 13);
            this.modifyMVR.Name = "modifyMVR";
            this.modifyMVR.Size = new System.Drawing.Size(50, 30);
            this.modifyMVR.TabIndex = 174;
            this.modifyMVR.Text = "Clear\r\nMVR";
            this.toolTip1.SetToolTip(this.modifyMVR, "HIGHLY EXPERIMENTAL! This option will clear the level\'s MVR file, which will remo" +
        "ve any lingering geometry, but will likely cause unwanted issues in base-game le" +
        "vels.");
            this.modifyMVR.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(936, 37);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(234, 23);
            this.button3.TabIndex = 179;
            this.button3.Text = "DEBUG: Dump override chains in this flow";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // editEntityMovers
            // 
            this.editEntityMovers.Location = new System.Drawing.Point(520, 121);
            this.editEntityMovers.Name = "editEntityMovers";
            this.editEntityMovers.Size = new System.Drawing.Size(99, 23);
            this.editEntityMovers.TabIndex = 179;
            this.editEntityMovers.Text = "Edit Movers";
            this.editEntityMovers.UseVisualStyleBackColor = true;
            this.editEntityMovers.Visible = false;
            this.editEntityMovers.Click += new System.EventHandler(this.editEntityMovers_Click);
            // 
            // CathodeEditorGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 804);
            this.Controls.Add(this.button3);
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
            this.Load += new System.EventHandler(this.CathodeEditorGUI_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label composite_count_display;
        private System.Windows.Forms.Label root_composite_display;
        private System.Windows.Forms.Button save_commands_pak;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label selected_entity_type_description;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label selected_entity_type;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label selected_entity_id;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListBox entity_children;
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
        private System.Windows.Forms.Button out_pin_goto;
        private System.Windows.Forms.Button jumpToComposite;
        private System.Windows.Forms.Button removeSelectedEntity;
        private System.Windows.Forms.Button addNewNode;
        private System.Windows.Forms.Button removeParameter;
        private System.Windows.Forms.Button addNewParameter;
        private System.Windows.Forms.Button removeSelectedLink;
        private System.Windows.Forms.Button addNewLink;
        private System.Windows.Forms.Button removeSelectedFlowgraph;
        private System.Windows.Forms.Button addNewFlowgraph;
        private System.Windows.Forms.Button showLinkParents;
        private System.Windows.Forms.CheckBox modifyMVR;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button editCAGEAnimationKeyframes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button duplicateSelectedNode;
        private System.Windows.Forms.Button renameSelectedNode;
        private System.Windows.Forms.Button editEntityResources;
        private System.Windows.Forms.Button editCompositeResources;
        private System.Windows.Forms.Button editEntryPoint;
        private System.Windows.Forms.Button editTriggerSequence;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox hierarchyDisplay;
        private System.Windows.Forms.Button editEntityMovers;
    }
}