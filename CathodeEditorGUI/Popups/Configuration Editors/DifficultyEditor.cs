using CATHODE;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.ConfigEditors
{
    public partial class DifficultyEditor : BaseWindow
    {
        List<BML> _selectedDifficulty;

        public DifficultyEditor() : base()
        {
            InitializeComponent();

            BML difficultySettingsTypes = new BML(Singleton.PathToAI + "\\DATA\\DIFFICULTYSETTINGS\\DIFFICULTYSETTINGS.BML");
            var difficultySettings = difficultySettingsTypes.Content["DifficultySettings"];
            classSelection.BeginUpdate();
            foreach (XmlElement difficultySetting in difficultySettings)
            {
                classSelection.Items.Add(difficultySetting["Name"].InnerText);
            }
            classSelection.EndUpdate();

            this.FormClosing += DifficultyEditor_FormClosing;
        }

        private void DifficultyEditor_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < classSelection.Items.Count; i++)
                classSelection.SelectedIndex = i;
            classSelection.SelectedIndex = 0;
        }

        private void DifficultyEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigEditorUtils.Unsubscribe(this.Controls, Save);
            this.FormClosing -= DifficultyEditor_FormClosing;
        }

        private void classSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigEditorUtils.Unsubscribe(this.Controls, Save);

            _selectedDifficulty = new List<BML>();
            _selectedDifficulty.Add(new BML(Singleton.PathToAI + "\\DATA\\DIFFICULTYSETTINGS\\" + classSelection.Text + ".BML"));
            while (true)
            {
                string template = _selectedDifficulty[_selectedDifficulty.Count - 1].Content["DifficultySetting"]["Template_Name"]?.InnerText;
                if (template == null || template == "") break;
                _selectedDifficulty.Add(new BML(Singleton.PathToAI + "\\DATA\\DIFFICULTYSETTINGS\\" + template + ".BML"));
            }

            ConfigEditorUtils.SetNumber(_selectedDifficulty, decrease_sweep_duration_modifier, "DifficultySetting", "Alien", "AlienConfig", "decrease_sweep_duration_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, increase_sweep_duration_modifier, "DifficultySetting", "Alien", "AlienConfig", "increase_sweep_duration_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, near_target_exclusion_radius_first_stalk_min_modifier, "DifficultySetting", "Alien", "AlienConfig", "near_target_exclusion_radius_first_stalk_min_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, near_target_exclusion_radius_first_stalk_max_modifier, "DifficultySetting", "Alien", "AlienConfig", "near_target_exclusion_radius_first_stalk_max_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, near_target_exclusion_radius_subsequent_stalk_min_modifier, "DifficultySetting", "Alien", "AlienConfig", "near_target_exclusion_radius_subsequent_stalk_min_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, near_target_exclusion_radius_subsequent_stalk_max_modifier, "DifficultySetting", "Alien", "AlienConfig", "near_target_exclusion_radius_subsequent_stalk_max_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, near_objective_exclusion_radius_first_stalk_min_modifier, "DifficultySetting", "Alien", "AlienConfig", "near_objective_exclusion_radius_first_stalk_min_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, near_objective_exclusion_radius_first_stalk_max_modifier, "DifficultySetting", "Alien", "AlienConfig", "near_objective_exclusion_radius_first_stalk_max_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, near_objective_exclusion_radius_subsequent_stalk_min_modifier, "DifficultySetting", "Alien", "AlienConfig", "near_objective_exclusion_radius_subsequent_stalk_min_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, near_objective_exclusion_radius_subsequent_stalk_max_modifier, "DifficultySetting", "Alien", "AlienConfig", "near_objective_exclusion_radius_subsequent_stalk_max_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, menace_gauge_decrease_time_modifier, "DifficultySetting", "Alien", "AlienConfig", "menace_gauge_decrease_time_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, meance_deemed_time_modifier, "DifficultySetting", "Alien", "AlienConfig", "meance_deemed_time_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, max_menaces_modifier, "DifficultySetting", "Alien", "AlienConfig", "max_menaces_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, menace_gauge_seconds_to_fill_modifier, "DifficultySetting", "Alien", "AlienConfig", "menace_gauge_seconds_to_fill_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, backstage_area_sweep_role_timeout_modifier, "DifficultySetting", "Alien", "AlienConfig", "backstage_area_sweep_role_timeout_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, backstage_area_sweep_min_distance_modifier, "DifficultySetting", "Alien", "AlienConfig", "backstage_area_sweep_min_distance_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, backstage_area_sweep_max_distance_modifier, "DifficultySetting", "Alien", "AlienConfig", "backstage_area_sweep_max_distance_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, backstage_area_sweep_min_idle_time_modifier, "DifficultySetting", "Alien", "AlienConfig", "backstage_area_sweep_min_idle_time_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, backstage_area_sweep_max_idle_time_modifier, "DifficultySetting", "Alien", "AlienConfig", "backstage_area_sweep_max_idle_time_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, backstage_area_sweep_killtrap_time_modifier, "DifficultySetting", "Alien", "AlienConfig", "backstage_area_sweep_killtrap_time_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, backstage_area_sweep_ambush_timeout_modifier, "DifficultySetting", "Alien", "AlienConfig", "backstage_area_sweep_ambush_timeout_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, sweep_box_half_length_modifier, "DifficultySetting", "Alien", "AlienConfig", "sweep_box_half_length_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, sweep_box_half_width_modifier, "DifficultySetting", "Alien", "AlienConfig", "sweep_box_half_width_modifier");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, Vent_Attract_Time_Min, "DifficultySetting", "Alien", "AlienConfig", "Vent_Attract_Time_Min");
            ConfigEditorUtils.SetNumber(_selectedDifficulty, Vent_Attract_Time_Max, "DifficultySetting", "Alien", "AlienConfig", "Vent_Attract_Time_Max");

            ConfigEditorUtils.Subscribe(this.Controls, Save);
        }

        //Load character type
        private void loadNPC_Click(object sender, EventArgs e)
        {
            /*
            //Update cursor and begin
            Cursor.Current = Cursors.WaitCursor;

            //Save selected config name
            string selectedConfig = characterTypes.Text;

            if (selectedConfig == "")
            {
                //No config selected, can't load anything
                MessageBox.Show("Please select a character.");
            }
            else
            {
                //Load-in XML data
                var ChrAttributeXML = _loadedDifficultyXml;

                //Set NPC_Generic Senses Values
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "max_hearing_distance_modifier", ChrAttributeXML, max_hearing_distance_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "visual_sense_activation_modifier", ChrAttributeXML, visual_sense_activation_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "visual_combined_sense_activation_modifier", ChrAttributeXML, visual_combined_sense_activation_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "weapon_sound_sense_activation_modifier", ChrAttributeXML, weapon_sound_sense_activation_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "weapon_sound_combined_sense_activation_modifier", ChrAttributeXML, weapon_sound_combined_sense_activation_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "movement_sound_sense_activation_modifier", ChrAttributeXML, movement_sound_sense_activation_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "movement_sound_combined_sense_activation_modifier", ChrAttributeXML, movement_sound_combined_sense_activation_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "flash_light_sense_activation_modifier", ChrAttributeXML, flash_light_sense_activation_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/Senses", "flash_light_combined_sense_activation_modifier", ChrAttributeXML, flash_light_combined_sense_activation_modifier, null);

                //Set NPC_Generic General Values
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/General", "damage_dealt_scalar", ChrAttributeXML, damage_dealt_scalar, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/General", "damage_received_scalar", ChrAttributeXML, damage_received_scalar, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/General", "suspicious_item_loop_scalar", ChrAttributeXML, suspicious_item_loop_scalar, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/General", "attack_pace_modifier", ChrAttributeXML, attack_pace_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/General", "attack_pace_modifier_per_npc", ChrAttributeXML, attack_pace_modifier_per_npc, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/General", "attack_pace_modifier_max", ChrAttributeXML, attack_pace_modifier_max, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/General", "shooting_in_cover_duration_modifier", ChrAttributeXML, shooting_in_cover_duration_modifier, null);
                AlienAttribute.getNode("NPC_Generic/" + selectedConfig + "/General", "time_between_shots_scalar", ChrAttributeXML, time_between_shots_scalar, null);
            }

            //Update cursor and finish
            Cursor.Current = Cursors.Default;
            */
        }

        //Unlock viewcone type buttons
        private void loadViewconeSet_Click(object sender, EventArgs e)
        {
            /*
            viewconeType.Enabled = true;
            loadViewconeType.Enabled = true;

            AlienAttribute.disableInput(visual_sense_exposure_effect_lower_modifier, null);
            AlienAttribute.disableInput(visual_sense_exposure_effect_upper_modifier, null);
            AlienAttribute.disableInput(visual_sense_stance_effect_lower_modifier, null);
            AlienAttribute.disableInput(visual_sense_stance_effect_upper_modifier, null);
            */
        }

        //Load viewcone set/type
        private void button3_Click(object sender, EventArgs e)
        {
            /*
            //Update cursor and begin
            Cursor.Current = Cursors.WaitCursor;

            //Save selected config name
            string viewconeSetSaved = viewconeSet.Text;
            string viewconeTypeSaved = viewconeType.Text;

            if (viewconeTypeSaved == "")
            {
                //No config selected, can't load anything
                MessageBox.Show("Please select a viewcone type.");
            }
            else
            {
                //Load-in XML data
                var ChrAttributeXML = _loadedDifficultyXml;
                
                //Set viewcone Values
                string viewconeXmlPath = "ViewconeSets/" + viewconeSetSaved + "/" + viewconeTypeSaved;
                AlienAttribute.getNode(viewconeXmlPath, "visual_sense_exposure_effect_lower_modifier", ChrAttributeXML, visual_sense_exposure_effect_lower_modifier, null);
                AlienAttribute.getNode(viewconeXmlPath, "visual_sense_exposure_effect_upper_modifier", ChrAttributeXML, visual_sense_exposure_effect_upper_modifier, null);
                AlienAttribute.getNode(viewconeXmlPath, "visual_sense_stance_effect_lower_modifier", ChrAttributeXML, visual_sense_stance_effect_lower_modifier, null);
                AlienAttribute.getNode(viewconeXmlPath, "visual_sense_stance_effect_upper_modifier", ChrAttributeXML, visual_sense_stance_effect_upper_modifier, null);
            }

            //Update cursor and finish
            Cursor.Current = Cursors.Default;
            */
        }

        //Save
        private void Save(object sender, EventArgs e)
        {
            /*
            //Update cursor and begin
            Cursor.Current = Cursors.WaitCursor;

            //Save selected config name
            string selectedConfig = classSelection.Text;

            //Save selected config name NPC
            string selectedConfigNPC = characterTypes.Text;

            //Save selected config name
            string viewconeSetSaved = viewconeSet.Text;
            string viewconeTypeSaved = viewconeType.Text;

            if (selectedConfig == "")
            {
                //No config selected, can't load anything
                MessageBox.Show("Please select a difficulty.");
            }
            else
            {
                //Load-in XML data
                var ChrAttributeXML = _loadedDifficultyXml;

                //save AlienConfig Values
                AlienAttribute.setNode("Alien/AlienConfig", "decrease_sweep_duration_modifier", ChrAttributeXML, decrease_sweep_duration_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "increase_sweep_duration_modifier", ChrAttributeXML, increase_sweep_duration_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "near_target_exclusion_radius_first_stalk_min_modifier", ChrAttributeXML, near_target_exclusion_radius_first_stalk_min_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "near_target_exclusion_radius_first_stalk_max_modifier", ChrAttributeXML, near_target_exclusion_radius_first_stalk_max_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "near_target_exclusion_radius_subsequent_stalk_min_modifier", ChrAttributeXML, near_target_exclusion_radius_subsequent_stalk_min_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "near_target_exclusion_radius_subsequent_stalk_max_modifier", ChrAttributeXML, near_target_exclusion_radius_subsequent_stalk_max_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "near_objective_exclusion_radius_first_stalk_min_modifier", ChrAttributeXML, near_objective_exclusion_radius_first_stalk_min_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "near_objective_exclusion_radius_first_stalk_max_modifier", ChrAttributeXML, near_objective_exclusion_radius_first_stalk_max_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "near_objective_exclusion_radius_subsequent_stalk_min_modifier", ChrAttributeXML, near_objective_exclusion_radius_subsequent_stalk_min_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "near_objective_exclusion_radius_subsequent_stalk_max_modifier", ChrAttributeXML, near_objective_exclusion_radius_subsequent_stalk_max_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "menace_gauge_decrease_time_modifier", ChrAttributeXML, menace_gauge_decrease_time_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "menace_cool_down_time_modifier", ChrAttributeXML, menace_cool_down_time_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "meance_deemed_time_modifier", ChrAttributeXML, meance_deemed_time_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "max_menaces_modifier", ChrAttributeXML, max_menaces_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "menace_gauge_seconds_to_fill_modifier", ChrAttributeXML, menace_gauge_seconds_to_fill_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "backstage_area_sweep_role_timeout_modifier", ChrAttributeXML, backstage_area_sweep_role_timeout_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "backstage_area_sweep_min_distance_modifier", ChrAttributeXML, backstage_area_sweep_min_distance_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "backstage_area_sweep_max_distance_modifier", ChrAttributeXML, backstage_area_sweep_max_distance_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "backstage_area_sweep_min_idle_time_modifier", ChrAttributeXML, backstage_area_sweep_min_idle_time_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "backstage_area_sweep_max_idle_time_modifier", ChrAttributeXML, backstage_area_sweep_max_idle_time_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "backstage_area_sweep_killtrap_time_modifier", ChrAttributeXML, backstage_area_sweep_killtrap_time_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "sweep_box_half_length_modifier", ChrAttributeXML, sweep_box_half_length_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "sweep_box_half_width_modifier", ChrAttributeXML, sweep_box_half_width_modifier, null);
                AlienAttribute.setNode("Alien/AlienConfig", "Vent_Attract_Time_Min", ChrAttributeXML, Vent_Attract_Time_Min, null);
                AlienAttribute.setNode("Alien/AlienConfig", "Vent_Attract_Time_Max", ChrAttributeXML, Vent_Attract_Time_Max, null);

                if (selectedConfigNPC != "")
                {
                    //save NPC_Generic Senses Values
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "max_hearing_distance_modifier", ChrAttributeXML, max_hearing_distance_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "visual_sense_activation_modifier", ChrAttributeXML, visual_sense_activation_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "visual_combined_sense_activation_modifier", ChrAttributeXML, visual_combined_sense_activation_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "weapon_sound_sense_activation_modifier", ChrAttributeXML, weapon_sound_sense_activation_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "weapon_sound_combined_sense_activation_modifier", ChrAttributeXML, weapon_sound_combined_sense_activation_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "movement_sound_sense_activation_modifier", ChrAttributeXML, movement_sound_sense_activation_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "movement_sound_combined_sense_activation_modifier", ChrAttributeXML, movement_sound_combined_sense_activation_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "flash_light_sense_activation_modifier", ChrAttributeXML, flash_light_sense_activation_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/Senses", "flash_light_combined_sense_activation_modifier", ChrAttributeXML, flash_light_combined_sense_activation_modifier, null);

                    //save NPC_Generic General Values
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/General", "damage_dealt_scalar", ChrAttributeXML, damage_dealt_scalar, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/General", "damage_received_scalar", ChrAttributeXML, damage_received_scalar, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/General", "suspicious_item_loop_scalar", ChrAttributeXML, suspicious_item_loop_scalar, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/General", "attack_pace_modifier", ChrAttributeXML, attack_pace_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/General", "attack_pace_modifier_per_npc", ChrAttributeXML, attack_pace_modifier_per_npc, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/General", "attack_pace_modifier_max", ChrAttributeXML, attack_pace_modifier_max, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/General", "shooting_in_cover_duration_modifier", ChrAttributeXML, shooting_in_cover_duration_modifier, null);
                    AlienAttribute.setNode("NPC_Generic/" + selectedConfigNPC + "/General", "time_between_shots_scalar", ChrAttributeXML, time_between_shots_scalar, null);
                }

                if (viewconeSetSaved != "" && viewconeTypeSaved != "")
                {
                    //save viewcone Values
                    string viewconeXmlPath = "ViewconeSets/" + viewconeSetSaved + "/" + viewconeTypeSaved;
                    AlienAttribute.setNode(viewconeXmlPath, "visual_sense_exposure_effect_lower_modifier", ChrAttributeXML, visual_sense_exposure_effect_lower_modifier, null);
                    AlienAttribute.setNode(viewconeXmlPath, "visual_sense_exposure_effect_upper_modifier", ChrAttributeXML, visual_sense_exposure_effect_upper_modifier, null);
                    AlienAttribute.setNode(viewconeXmlPath, "visual_sense_stance_effect_lower_modifier", ChrAttributeXML, visual_sense_stance_effect_lower_modifier, null);
                    AlienAttribute.setNode(viewconeXmlPath, "visual_sense_stance_effect_upper_modifier", ChrAttributeXML, visual_sense_stance_effect_upper_modifier, null);
                }

                //Save values
                if (AlienAttribute.saveXML(selectedConfig, gameBmlDirectory, ChrAttributeXML))
                {
                    MessageBox.Show("Saved configuration changes.");
                }
                else
                {
                    MessageBox.Show("An error occured while saving.");
                }
            }

            //Update cursor and finish
            Cursor.Current = Cursors.Default;
            */
        }
    }
}
