namespace CathodeEditorGUI.UserControls
{
    partial class GUI_EnumDataType
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(239, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(80, 20);
            this.textBox1.TabIndex = 11;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "AGGRESSION_GAIN",
            "ALERTNESS_STATE",
            "ALIEN_CONFIGURATION_TYPE",
            "ALIEN_DEVELOPMENT_MANAGER_ABILITIES",
            "ALIEN_DEVELOPMENT_MANAGER_ABILITY_MASKS",
            "ALIEN_DEVELOPMENT_MANAGER_STAGES",
            "ALLIANCE_GROUP",
            "AMBUSH_TYPE",
            "AMMO_TYPE",
            "ANIM_CALLBACK_ENUM",
            "ANIM_MODE",
            "ANIM_TRACK_TYPE",
            "ANIM_TREE_ENUM",
            "ANIMATION_EFFECT_TYPE",
            "AREA_SWEEP_TYPE",
            "AREA_SWEEP_TYPE_CODE",
            "BEHAVIOR_TREE_BRANCH_TYPE",
            "BEHAVIOUR_MOOD_SET",
            "BEHAVIOUR_TREE_FLAGS",
            "BLEND_MODE",
            "BLUEPRINT_LEVEL",
            "BUTTON_TYPE",
            "CAMERA_PATH_CLASS",
            "CAMERA_PATH_TYPE",
            "CHARACTER_BB_ENTRY_TYPE",
            "CHARACTER_CLASS",
            "CHARACTER_CLASS_COMBINATION",
            "CHARACTER_FOLEY_SOUND",
            "CHARACTER_NODE",
            "CHARACTER_STANCE",
            "CHECKPOINT_TYPE",
            "CI_MESSAGE_TYPE",
            "CLIPPING_PLANES_PRESETS",
            "COLLISION_TYPE",
            "COMBAT_BEHAVIOUR",
            "CROUCH_MODE",
            "CUSTOM_CHARACTER_ACCESSORY_OVERRIDE",
            "CUSTOM_CHARACTER_ASSETS",
            "CUSTOM_CHARACTER_BUILD",
            "CUSTOM_CHARACTER_COMPONENT",
            "CUSTOM_CHARACTER_ETHNICITY",
            "CUSTOM_CHARACTER_GENDER",
            "CUSTOM_CHARACTER_MODEL",
            "CUSTOM_CHARACTER_POPULATION",
            "CUSTOM_CHARACTER_SLEEVETYPE",
            "CUSTOM_CHARACTER_TYPE",
            "DAMAGE_EFFECT_TYPE_FLAGS",
            "DAMAGE_EFFECTS",
            "DAMAGE_MODE",
            "DEATH_STYLE",
            "DEVICE_INTERACTION_MODE",
            "DIALOGUE_ACTION",
            "DIALOGUE_ARGUMENT",
            "DIALOGUE_NPC_COMBAT_MODE",
            "DIALOGUE_NPC_CONTEXT",
            "DIALOGUE_NPC_EVENT",
            "DIALOGUE_VOICE_ACTOR",
            "DIFFICULTY_SETTING_TYPE",
            "DOOR_MECHANISM",
            "DUCK_HEIGHT",
            "ENEMY_TYPE",
            "ENVIRONMENT_ARCHETYPE",
            "EQUIPMENT_SLOT",
            "EVENT_OCCURED_TYPE",
            "EXIT_WAYPOINT",
            "FLAG_CHANGE_SOURCE_TYPE",
            "FLASH_INVOKE_TYPE",
            "FLASH_SCRIPT_RENDER_TYPE",
            "FOG_BOX_TYPE",
            "FOLDER_LOCK_TYPE",
            "FOLLOW_CAMERA_MODIFIERS",
            "FOLLOW_TYPE",
            "FRAME_FLAGS",
            "FRONTEND_STATE",
            "GAME_CLIP",
            "GATING_TOOL_TYPE",
            "IDLE",
            "IDLE_STYLE",
            "IMPACT_CHARACTER_BODY_LOCATION_TYPE",
            "INPUT_DEVICE_TYPE",
            "JOB_TYPE",
            "LEVEL_HEAP_TAG",
            "LEVER_TYPE",
            "LIGHT_ADAPTATION_MECHANISM",
            "LIGHT_ANIM",
            "LIGHT_FADE_TYPE",
            "LIGHT_TRANSITION",
            "LIGHT_TYPE",
            "LOCOMOTION_STATE",
            "LOCOMOTION_TARGET_SPEED",
            "LOGIC_CHARACTER_FLAGS",
            "LOGIC_CHARACTER_GAUGE_TYPE",
            "LOGIC_CHARACTER_TIMER_TYPE",
            "LOOK_SPEED",
            "MAP_ICON_TYPE",
            "MELEE_ATTACK_TYPE",
            "MELEE_CONTEXT_TYPE",
            "MOOD",
            "MOOD_INTENSITY",
            "MOVE",
            "MUSIC_RTPC_MODE",
            "NAV_MESH_AREA_TYPE",
            "NAVIGATION_CHARACTER_CLASS",
            "NAVIGATION_CHARACTER_CLASS_COMBINATION",
            "NOISE_TYPE",
            "NPC_AGGRO_LEVEL",
            "NPC_COMBAT_STATE",
            "NPC_COVER_REQUEST_TYPE",
            "NPC_GUN_AIM_MODE",
            "ORIENTATION_AXIS",
            "PATH_DRIVEN_TYPE",
            "PAUSE_SENSES_TYPE",
            "PICKUP_CATEGORY",
            "PLATFORM_TYPE",
            "PLAYER_INVENTORY_SET",
            "POPUP_MESSAGE_ICON",
            "POPUP_MESSAGE_SOUND",
            "PRIORITY",
            "RANGE_TEST_SHAPE",
            "RAYCAST_PRIORITY",
            "RESPAWN_MODE",
            "REWIRE_SYSTEM_NAME",
            "REWIRE_SYSTEM_TYPE",
            "SECONDARY_ANIMATION_LAYER",
            "SENSE_SET",
            "SENSE_SET_SYSTEM",
            "SENSORY_TYPE",
            "SHAKE_TYPE",
            "SIDE",
            "SOUND_POOL",
            "SPEECH_PRIORITY",
            "STEAL_CAMERA_TYPE",
            "SUB_OBJECTIVE_TYPE",
            "SUSPECT_RESPONSE_PHASE",
            "SUSPICIOUS_ITEM",
            "SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY",
            "SUSPICIOUS_ITEM_CLOSE_REACTION_DETAIL",
            "SUSPICIOUS_ITEM_REACTION",
            "SUSPICIOUS_ITEM_STAGE",
            "SUSPICIOUS_ITEM_TRIGGER",
            "TASK_CHARACTER_CLASS_FILTER",
            "TASK_OPERATION_MODE",
            "TASK_PRIORITY",
            "TERMINAL_LOCATION",
            "TEXT_ALIGNMENT",
            "THRESHOLD_QUALIFIER",
            "TRANSITION_DIRECTION",
            "TRAVERSAL_ANIMS",
            "TRAVERSAL_TYPE",
            "UI_ICON_ICON",
            "UI_KEYGATE_TYPE",
            "VENT_LOCK_REASON",
            "VIEWCONE_TYPE",
            "VISIBILITY_SETTINGS_TYPE",
            "WAVE_SHAPE",
            "WEAPON_HANDEDNESS",
            "WEAPON_IMPACT_EFFECT_ORIENTATION",
            "WEAPON_IMPACT_EFFECT_TYPE",
            "WEAPON_IMPACT_FILTER_ORIENTATION",
            "WEAPON_TYPE"});
            this.comboBox1.Location = new System.Drawing.Point(6, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(227, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(152, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "Parameter Name (00-00-00-00)";
            // 
            // GUI_EnumDataType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label13);
            this.Name = "GUI_EnumDataType";
            this.Size = new System.Drawing.Size(340, 45);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox1;
    }
}
