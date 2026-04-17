namespace CommandsEditor.ConfigEditors
{
    partial class InventoryItemEditor
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Objects", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Weapons", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Ammo Types", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Medikits", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Explosives", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Light Emitters", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryItemEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.cancellable_duration_in_seconds = new System.Windows.Forms.TextBox();
            this.drop_when_consume = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.upgraded_health_increase_percentage = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.health_increase_percentage = new System.Windows.Forms.TextBox();
            this.droppable_when_held = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.display_ammo_as_percentage = new System.Windows.Forms.ComboBox();
            this.vanish_when_collected = new System.Windows.Forms.ComboBox();
            this.consume_when = new System.Windows.Forms.ComboBox();
            this.activated_by = new System.Windows.Forms.ComboBox();
            this.keyframe = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.target_weapon = new System.Windows.Forms.ComboBox();
            this.held_object_name = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.crafting_resource = new System.Windows.Forms.ComboBox();
            this.thrown_object_name = new System.Windows.Forms.ComboBox();
            this.special_slot = new System.Windows.Forms.ComboBox();
            this.display_quantity = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.localisation_tag = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.radial_menu_order_index = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.composite = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.default_quantity = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.ammo_type = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.stack_limit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.listView = new System.Windows.Forms.ListView();
            this.ItemName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.cancellable_duration_in_seconds);
            this.groupBox1.Controls.Add(this.drop_when_consume);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.upgraded_health_increase_percentage);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.health_increase_percentage);
            this.groupBox1.Controls.Add(this.droppable_when_held);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.display_ammo_as_percentage);
            this.groupBox1.Controls.Add(this.vanish_when_collected);
            this.groupBox1.Controls.Add(this.consume_when);
            this.groupBox1.Controls.Add(this.activated_by);
            this.groupBox1.Controls.Add(this.keyframe);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.target_weapon);
            this.groupBox1.Controls.Add(this.held_object_name);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.crafting_resource);
            this.groupBox1.Controls.Add(this.thrown_object_name);
            this.groupBox1.Controls.Add(this.special_slot);
            this.groupBox1.Controls.Add(this.display_quantity);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.localisation_tag);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.radial_menu_order_index);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.composite);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.default_quantity);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.ammo_type);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.stack_limit);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Location = new System.Drawing.Point(401, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(519, 330);
            this.groupBox1.TabIndex = 352;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Item Attributes";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(393, 256);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(114, 13);
            this.label27.TabIndex = 426;
            this.label27.Text = "Cancel Duration (secs)";
            // 
            // cancellable_duration_in_seconds
            // 
            this.cancellable_duration_in_seconds.Enabled = false;
            this.cancellable_duration_in_seconds.Location = new System.Drawing.Point(396, 272);
            this.cancellable_duration_in_seconds.Name = "cancellable_duration_in_seconds";
            this.cancellable_duration_in_seconds.Size = new System.Drawing.Size(114, 20);
            this.cancellable_duration_in_seconds.TabIndex = 427;
            this.toolTip1.SetToolTip(this.cancellable_duration_in_seconds, "The seconds before you can no longer cancel this.");
            // 
            // drop_when_consume
            // 
            this.drop_when_consume.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drop_when_consume.Enabled = false;
            this.drop_when_consume.FormattingEnabled = true;
            this.drop_when_consume.Items.AddRange(new object[] {
            "true",
            "false"});
            this.drop_when_consume.Location = new System.Drawing.Point(265, 271);
            this.drop_when_consume.Name = "drop_when_consume";
            this.drop_when_consume.Size = new System.Drawing.Size(114, 21);
            this.drop_when_consume.TabIndex = 425;
            this.toolTip1.SetToolTip(this.drop_when_consume, "Should this item be dropped once consumed?");
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(261, 255);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(115, 13);
            this.label21.TabIndex = 424;
            this.label21.Text = "Drop When Consumed";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(142, 255);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(114, 13);
            this.label17.TabIndex = 422;
            this.label17.Text = "Upgraded Health + (%)";
            // 
            // upgraded_health_increase_percentage
            // 
            this.upgraded_health_increase_percentage.Enabled = false;
            this.upgraded_health_increase_percentage.Location = new System.Drawing.Point(145, 271);
            this.upgraded_health_increase_percentage.Name = "upgraded_health_increase_percentage";
            this.upgraded_health_increase_percentage.Size = new System.Drawing.Size(114, 20);
            this.upgraded_health_increase_percentage.TabIndex = 423;
            this.toolTip1.SetToolTip(this.upgraded_health_increase_percentage, "The percentage health to increase when used (after being upgraded).");
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 254);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(97, 13);
            this.label15.TabIndex = 420;
            this.label15.Text = "Heath Increase (%)";
            // 
            // health_increase_percentage
            // 
            this.health_increase_percentage.Enabled = false;
            this.health_increase_percentage.Location = new System.Drawing.Point(14, 270);
            this.health_increase_percentage.Name = "health_increase_percentage";
            this.health_increase_percentage.Size = new System.Drawing.Size(114, 20);
            this.health_increase_percentage.TabIndex = 421;
            this.toolTip1.SetToolTip(this.health_increase_percentage, "The percentage of health to increase the player by when used.");
            // 
            // droppable_when_held
            // 
            this.droppable_when_held.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.droppable_when_held.Enabled = false;
            this.droppable_when_held.FormattingEnabled = true;
            this.droppable_when_held.Items.AddRange(new object[] {
            "true",
            "false"});
            this.droppable_when_held.Location = new System.Drawing.Point(396, 75);
            this.droppable_when_held.Name = "droppable_when_held";
            this.droppable_when_held.Size = new System.Drawing.Size(114, 21);
            this.droppable_when_held.TabIndex = 419;
            this.toolTip1.SetToolTip(this.droppable_when_held, "Should this item be able to be dropped when in the held state?");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(391, 59);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 13);
            this.label13.TabIndex = 417;
            this.label13.Text = "Droppable When Held";
            // 
            // display_ammo_as_percentage
            // 
            this.display_ammo_as_percentage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.display_ammo_as_percentage.Enabled = false;
            this.display_ammo_as_percentage.FormattingEnabled = true;
            this.display_ammo_as_percentage.Items.AddRange(new object[] {
            "true",
            "false"});
            this.display_ammo_as_percentage.Location = new System.Drawing.Point(14, 192);
            this.display_ammo_as_percentage.Name = "display_ammo_as_percentage";
            this.display_ammo_as_percentage.Size = new System.Drawing.Size(114, 21);
            this.display_ammo_as_percentage.TabIndex = 416;
            this.toolTip1.SetToolTip(this.display_ammo_as_percentage, "Display the weapon ammo as a percent? (Weapon only.)");
            // 
            // vanish_when_collected
            // 
            this.vanish_when_collected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vanish_when_collected.Enabled = false;
            this.vanish_when_collected.FormattingEnabled = true;
            this.vanish_when_collected.Items.AddRange(new object[] {
            "true",
            "false"});
            this.vanish_when_collected.Location = new System.Drawing.Point(145, 192);
            this.vanish_when_collected.Name = "vanish_when_collected";
            this.vanish_when_collected.Size = new System.Drawing.Size(114, 21);
            this.vanish_when_collected.TabIndex = 415;
            this.toolTip1.SetToolTip(this.vanish_when_collected, "When picked up, this item should disappear instead of being added to the inventor" +
        "y (ie. script logic dictates what should happen next)");
            // 
            // consume_when
            // 
            this.consume_when.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.consume_when.Enabled = false;
            this.consume_when.FormattingEnabled = true;
            this.consume_when.Items.AddRange(new object[] {
            "OnActivate",
            "OnThrowOrPlace"});
            this.consume_when.Location = new System.Drawing.Point(396, 192);
            this.consume_when.Name = "consume_when";
            this.consume_when.Size = new System.Drawing.Size(114, 21);
            this.consume_when.TabIndex = 414;
            this.toolTip1.SetToolTip(this.consume_when, "When should this item be counted as used? When thrown/placed, or when activated?");
            // 
            // activated_by
            // 
            this.activated_by.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.activated_by.Enabled = false;
            this.activated_by.FormattingEnabled = true;
            this.activated_by.Items.AddRange(new object[] {
            "PressToActivate",
            "HoldToActivate"});
            this.activated_by.Location = new System.Drawing.Point(145, 231);
            this.activated_by.Name = "activated_by";
            this.activated_by.Size = new System.Drawing.Size(114, 21);
            this.activated_by.TabIndex = 413;
            this.toolTip1.SetToolTip(this.activated_by, "How is this item activated? Does the user have to press or hold the trigger key?");
            // 
            // keyframe
            // 
            this.keyframe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyframe.Enabled = false;
            this.keyframe.FormattingEnabled = true;
            this.keyframe.Location = new System.Drawing.Point(14, 230);
            this.keyframe.Name = "keyframe";
            this.keyframe.Size = new System.Drawing.Size(114, 21);
            this.keyframe.TabIndex = 412;
            this.toolTip1.SetToolTip(this.keyframe, "Keyframe used for icon in inventory, defaults to item name");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(262, 176);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 363;
            this.label7.Text = "Target Weapon";
            // 
            // target_weapon
            // 
            this.target_weapon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.target_weapon.Enabled = false;
            this.target_weapon.FormattingEnabled = true;
            this.target_weapon.Items.AddRange(new object[] {
            "PLACEHOLDER"});
            this.target_weapon.Location = new System.Drawing.Point(265, 192);
            this.target_weapon.Name = "target_weapon";
            this.target_weapon.Size = new System.Drawing.Size(114, 21);
            this.target_weapon.TabIndex = 408;
            this.toolTip1.SetToolTip(this.target_weapon, "The name of the inventory item for this ammo\'s weapon");
            // 
            // held_object_name
            // 
            this.held_object_name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.held_object_name.Enabled = false;
            this.held_object_name.FormattingEnabled = true;
            this.held_object_name.Location = new System.Drawing.Point(396, 231);
            this.held_object_name.Name = "held_object_name";
            this.held_object_name.Size = new System.Drawing.Size(114, 21);
            this.held_object_name.TabIndex = 411;
            this.toolTip1.SetToolTip(this.held_object_name, "The object name for this item when held.");
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(262, 137);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(92, 13);
            this.label19.TabIndex = 397;
            this.label19.Text = "Crafting Resource";
            // 
            // crafting_resource
            // 
            this.crafting_resource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.crafting_resource.Enabled = false;
            this.crafting_resource.FormattingEnabled = true;
            this.crafting_resource.Items.AddRange(new object[] {
            "true",
            "false"});
            this.crafting_resource.Location = new System.Drawing.Point(265, 153);
            this.crafting_resource.Name = "crafting_resource";
            this.crafting_resource.Size = new System.Drawing.Size(114, 21);
            this.crafting_resource.TabIndex = 407;
            this.toolTip1.SetToolTip(this.crafting_resource, "Is this item a crafting resource?");
            // 
            // thrown_object_name
            // 
            this.thrown_object_name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thrown_object_name.Enabled = false;
            this.thrown_object_name.FormattingEnabled = true;
            this.thrown_object_name.Location = new System.Drawing.Point(265, 231);
            this.thrown_object_name.Name = "thrown_object_name";
            this.thrown_object_name.Size = new System.Drawing.Size(114, 21);
            this.thrown_object_name.TabIndex = 410;
            this.toolTip1.SetToolTip(this.thrown_object_name, "The object name when this item is thrown.");
            // 
            // special_slot
            // 
            this.special_slot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.special_slot.Enabled = false;
            this.special_slot.FormattingEnabled = true;
            this.special_slot.Items.AddRange(new object[] {
            "PLACEHOLDER"});
            this.special_slot.Location = new System.Drawing.Point(14, 153);
            this.special_slot.Name = "special_slot";
            this.special_slot.Size = new System.Drawing.Size(245, 21);
            this.special_slot.TabIndex = 409;
            this.toolTip1.SetToolTip(this.special_slot, "Dictates that the item should go into a special slot");
            // 
            // display_quantity
            // 
            this.display_quantity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.display_quantity.Enabled = false;
            this.display_quantity.FormattingEnabled = true;
            this.display_quantity.Items.AddRange(new object[] {
            "true",
            "false"});
            this.display_quantity.Location = new System.Drawing.Point(396, 153);
            this.display_quantity.Name = "display_quantity";
            this.display_quantity.Size = new System.Drawing.Size(114, 21);
            this.display_quantity.TabIndex = 355;
            this.toolTip1.SetToolTip(this.display_quantity, "Should we display the quantity information on the pickup prompt?");
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(14, 297);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(496, 26);
            this.btnSave.TabIndex = 354;
            this.btnSave.Text = "Save Inventory Item";
            this.toolTip1.SetToolTip(this.btnSave, "Save currently loaded inventory item.");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(391, 176);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(83, 13);
            this.label28.TabIndex = 405;
            this.label28.Text = "Consume When";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(391, 215);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 13);
            this.label11.TabIndex = 403;
            this.label11.Text = "Held Object Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(261, 215);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 401;
            this.label8.Text = "Thrown Object Name";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(142, 215);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 13);
            this.label18.TabIndex = 399;
            this.label18.Text = "Activated By";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(391, 137);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(83, 13);
            this.label20.TabIndex = 395;
            this.label20.Text = "Display Quantity";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(11, 176);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(127, 13);
            this.label22.TabIndex = 393;
            this.label22.Text = "Display Ammo as Percent";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(11, 59);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 13);
            this.label23.TabIndex = 391;
            this.label23.Text = "Localisation Tag";
            // 
            // localisation_tag
            // 
            this.localisation_tag.Enabled = false;
            this.localisation_tag.Location = new System.Drawing.Point(14, 75);
            this.localisation_tag.Name = "localisation_tag";
            this.localisation_tag.Size = new System.Drawing.Size(365, 20);
            this.localisation_tag.TabIndex = 392;
            this.toolTip1.SetToolTip(this.localisation_tag, "The text used to represent the item in-game. Defaults to a localised value in the" +
        " Alien: Isolation subtitle files, although can be edited to a plain-text string." +
        "");
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(391, 98);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(96, 13);
            this.label24.TabIndex = 389;
            this.label24.Text = "Radial Menu Index";
            // 
            // radial_menu_order_index
            // 
            this.radial_menu_order_index.Enabled = false;
            this.radial_menu_order_index.Location = new System.Drawing.Point(396, 114);
            this.radial_menu_order_index.Name = "radial_menu_order_index";
            this.radial_menu_order_index.Size = new System.Drawing.Size(114, 20);
            this.radial_menu_order_index.TabIndex = 390;
            this.toolTip1.SetToolTip(this.radial_menu_order_index, "What is the index of this item in the radial menu, e.g. the weapons should be gro" +
        "uped together, the medikits should be grouped together");
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(142, 177);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(118, 13);
            this.label25.TabIndex = 387;
            this.label25.Text = "Vanish When Collected";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(11, 137);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(72, 13);
            this.label26.TabIndex = 385;
            this.label26.Text = "Inventory Slot";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(262, 20);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(56, 13);
            this.label16.TabIndex = 383;
            this.label16.Text = "Composite";
            // 
            // composite
            // 
            this.composite.Enabled = false;
            this.composite.Location = new System.Drawing.Point(265, 36);
            this.composite.Name = "composite";
            this.composite.Size = new System.Drawing.Size(243, 20);
            this.composite.TabIndex = 384;
            this.toolTip1.SetToolTip(this.composite, "Name of required asset to spawn when dropped, defaults to Pickups\\item_name");
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 13);
            this.label12.TabIndex = 377;
            this.label12.Text = "Default Quantity";
            // 
            // default_quantity
            // 
            this.default_quantity.Enabled = false;
            this.default_quantity.Location = new System.Drawing.Point(14, 114);
            this.default_quantity.Name = "default_quantity";
            this.default_quantity.Size = new System.Drawing.Size(114, 20);
            this.default_quantity.TabIndex = 378;
            this.toolTip1.SetToolTip(this.default_quantity, "Quantity that the item is usually found in, defaults to 1");
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(134, 98);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 13);
            this.label14.TabIndex = 373;
            this.label14.Text = "Ammo Type";
            // 
            // ammo_type
            // 
            this.ammo_type.Enabled = false;
            this.ammo_type.Location = new System.Drawing.Point(137, 114);
            this.ammo_type.Name = "ammo_type";
            this.ammo_type.Size = new System.Drawing.Size(114, 20);
            this.ammo_type.TabIndex = 374;
            this.toolTip1.SetToolTip(this.ammo_type, "The enum value as matched against the AMMO_TYPE namespace within the game\'s code." +
        "");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(262, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 369;
            this.label10.Text = "Stack Limit";
            // 
            // stack_limit
            // 
            this.stack_limit.Enabled = false;
            this.stack_limit.Location = new System.Drawing.Point(265, 114);
            this.stack_limit.Name = "stack_limit";
            this.stack_limit.Size = new System.Drawing.Size(114, 20);
            this.stack_limit.TabIndex = 370;
            this.toolTip1.SetToolTip(this.stack_limit, "Maximum quantity that can be held in a single inventory stack, defaults to 1");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 215);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 367;
            this.label9.Text = "Keyframe";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(11, 20);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(35, 13);
            this.label29.TabIndex = 361;
            this.label29.Text = "Name";
            // 
            // name
            // 
            this.name.Enabled = false;
            this.name.Location = new System.Drawing.Point(14, 36);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(245, 20);
            this.name.TabIndex = 362;
            this.toolTip1.SetToolTip(this.name, "The inventory item codename.");
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemName});
            this.listView.FullRowSelect = true;
            listViewGroup1.Header = "Objects";
            listViewGroup1.Name = "object";
            listViewGroup1.Tag = "object";
            listViewGroup2.Header = "Weapons";
            listViewGroup2.Name = "weapon";
            listViewGroup2.Tag = "weapon";
            listViewGroup3.Header = "Ammo Types";
            listViewGroup3.Name = "ammo";
            listViewGroup3.Tag = "ammo";
            listViewGroup4.Header = "Medikits";
            listViewGroup4.Name = "medikit";
            listViewGroup4.Tag = "medikit";
            listViewGroup5.Header = "Explosives";
            listViewGroup5.Name = "ied";
            listViewGroup5.Tag = "ied";
            listViewGroup6.Header = "Light Emitters";
            listViewGroup6.Name = "light";
            listViewGroup6.Tag = "light";
            this.listView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 12);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(316, 707);
            this.listView.TabIndex = 354;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // ItemName
            // 
            this.ItemName.Text = "Item Name";
            this.ItemName.Width = 239;
            // 
            // InventoryItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 731);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CommandsEditor.SharedFormIcon.Icon;
            this.MaximizeBox = false;
            this.Name = "InventoryItemEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item and Inventory Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox default_quantity;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox ammo_type;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox stack_limit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox localisation_tag;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox radial_menu_order_index;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox composite;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox crafting_resource;
        private System.Windows.Forms.ComboBox display_quantity;
        private System.Windows.Forms.ComboBox target_weapon;
        private System.Windows.Forms.ComboBox special_slot;
        private System.Windows.Forms.ComboBox held_object_name;
        private System.Windows.Forms.ComboBox thrown_object_name;
        private System.Windows.Forms.ComboBox keyframe;
        private System.Windows.Forms.ComboBox display_ammo_as_percentage;
        private System.Windows.Forms.ComboBox vanish_when_collected;
        private System.Windows.Forms.ComboBox consume_when;
        private System.Windows.Forms.ComboBox activated_by;
        private System.Windows.Forms.ComboBox droppable_when_held;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox cancellable_duration_in_seconds;
        private System.Windows.Forms.ComboBox drop_when_consume;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox upgraded_health_increase_percentage;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox health_increase_percentage;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader ItemName;
    }
}
