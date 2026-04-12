using CATHODE;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.ConfigEditors
{
    public partial class AlienConfigEditor : BaseWindow
    {
        List<BML> _selectedConfig;

        public AlienConfigEditor() : base()
        {
            InitializeComponent();

            BML ammoTypes = new BML(Singleton.PathToAI + "\\DATA\\ALIENCONFIGS\\ALIENCONFIGS.BML");
            var configs = ammoTypes.Content["AlienConfigs"];
            classSelection.BeginUpdate();
            foreach (XmlElement config in configs)
            {
                classSelection.Items.Add(config["Name"].InnerText);
            }
            classSelection.EndUpdate();
            classSelection.SelectedIndex = 0;
        }

        private void classSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (classSelection.Text == "")
            {
                MessageBox.Show("Please select a configuration.");
                return;
            }

            _selectedConfig = new List<BML>();
            _selectedConfig.Add(new BML(Singleton.PathToAI + "\\DATA\\ALIENCONFIGS\\" + classSelection.Text + ".BML"));
            while (true)
            {
                string template = _selectedConfig[_selectedConfig.Count - 1].Content["AlienConfig"]["Template_Name"]?.InnerText;
                if (template == null || template == "") break;
                _selectedConfig.Add(new BML(Singleton.PathToAI + "\\DATA\\ALIENCONFIGS\\" + template + ".BML"));
            }

            ConfigEditorUtils.SetNumber(_selectedConfig, decrease_sweep_duration, "AreaSweep", "decrease_sweep_duration");
            ConfigEditorUtils.SetNumber(_selectedConfig, increase_sweep_duration, "AreaSweep", "increase_sweep_duration");
            ConfigEditorUtils.SetNumber(_selectedConfig, near_target_exclusion_radius_first_stalk_min, "AreaSweep", "near_target_exclusion_radius_first_stalk_min");
            ConfigEditorUtils.SetNumber(_selectedConfig, near_target_exclusion_radius_first_stalk_max, "AreaSweep", "near_target_exclusion_radius_first_stalk_max");
            ConfigEditorUtils.SetNumber(_selectedConfig, near_target_exclusion_radius_subsequent_stalk_min, "AreaSweep", "near_target_exclusion_radius_subsequent_stalk_min");
            ConfigEditorUtils.SetNumber(_selectedConfig, near_target_exclusion_radius_subsequent_stalk_max, "AreaSweep", "near_target_exclusion_radius_subsequent_stalk_max");
            ConfigEditorUtils.SetNumber(_selectedConfig, near_objective_exclusion_radius_first_stalk_min, "AreaSweep", "near_objective_exclusion_radius_first_stalk_min");
            ConfigEditorUtils.SetNumber(_selectedConfig, near_objective_exclusion_radius_first_stalk_max, "AreaSweep", "near_objective_exclusion_radius_first_stalk_max");
            ConfigEditorUtils.SetNumber(_selectedConfig, near_objective_exclusion_radius_subsequent_stalk_min, "AreaSweep", "near_objective_exclusion_radius_subsequent_stalk_min");
            ConfigEditorUtils.SetNumber(_selectedConfig, near_objective_exclusion_radius_subsequent_stalk_max, "AreaSweep", "near_objective_exclusion_radius_subsequent_stalk_max");
            ConfigEditorUtils.SetNumber(_selectedConfig, menace_gauge_decrease_time, "AreaSweep", "menace_gauge_decrease_time");
            ConfigEditorUtils.SetNumber(_selectedConfig, meance_deemed_time, "AreaSweep", "meance_deemed_time");
            ConfigEditorUtils.SetNumber(_selectedConfig, max_menaces, "AreaSweep", "max_menaces");
            ConfigEditorUtils.SetNumber(_selectedConfig, menace_gauge_seconds_to_fill, "AreaSweep", "menace_gauge_seconds_to_fill");
            ConfigEditorUtils.SetNumber(_selectedConfig, sweep_box_half_width, "AreaSweep", "sweep_box_half_width");
            ConfigEditorUtils.SetNumber(_selectedConfig, sweep_box_min_half_length, "AreaSweep", "sweep_box_min_half_length");
            ConfigEditorUtils.SetNumber(_selectedConfig, Vent_Attract_Time_Min, "AreaSweep", "Vent_Attract_Time_Min");
            ConfigEditorUtils.SetNumber(_selectedConfig, Vent_Attract_Time_Max, "AreaSweep", "Vent_Attract_Time_Max");

            ConfigEditorUtils.SetNumber(_selectedConfig, role_timeout_min, "BackstageAreaSweep", "role_timeout_min");
            ConfigEditorUtils.SetNumber(_selectedConfig, role_timeout_max, "BackstageAreaSweep", "role_timeout_max");
            ConfigEditorUtils.SetNumber(_selectedConfig, min_distance, "BackstageAreaSweep", "min_distance");
            ConfigEditorUtils.SetNumber(_selectedConfig, max_distance, "BackstageAreaSweep", "max_distance");
            ConfigEditorUtils.SetNumber(_selectedConfig, min_idle_time, "BackstageAreaSweep", "min_idle_time");
            ConfigEditorUtils.SetNumber(_selectedConfig, max_idle_time, "BackstageAreaSweep", "max_idle_time");
            ConfigEditorUtils.SetNumber(_selectedConfig, killtrap_time, "BackstageAreaSweep", "killtrap_time");
            ConfigEditorUtils.SetNumber(_selectedConfig, ambush_timeout, "BackstageAreaSweep", "ambush_timeout");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (classSelection.Text == "")
            {
                MessageBox.Show("Please select an ammo type first.");
                return;
            }

            var doc = _selectedConfig[0].Content;
            XmlElement ammo = doc["AlienConfig"];

            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "decrease_sweep_duration").InnerText = decrease_sweep_duration.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "increase_sweep_duration").InnerText = increase_sweep_duration.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "near_target_exclusion_radius_first_stalk_min").InnerText = near_target_exclusion_radius_first_stalk_min.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "near_target_exclusion_radius_first_stalk_max").InnerText = near_target_exclusion_radius_first_stalk_max.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "near_target_exclusion_radius_subsequent_stalk_min").InnerText = near_target_exclusion_radius_subsequent_stalk_min.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "near_target_exclusion_radius_subsequent_stalk_max").InnerText = near_target_exclusion_radius_subsequent_stalk_max.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "near_objective_exclusion_radius_first_stalk_min").InnerText = near_objective_exclusion_radius_first_stalk_min.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "near_objective_exclusion_radius_first_stalk_max").InnerText = near_objective_exclusion_radius_first_stalk_max.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "near_objective_exclusion_radius_subsequent_stalk_min").InnerText = near_objective_exclusion_radius_subsequent_stalk_min.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "near_objective_exclusion_radius_subsequent_stalk_max").InnerText = near_objective_exclusion_radius_subsequent_stalk_max.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "menace_gauge_decrease_time").InnerText = menace_gauge_decrease_time.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "meance_deemed_time").InnerText = meance_deemed_time.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "max_menaces").InnerText = max_menaces.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "menace_gauge_seconds_to_fill").InnerText = menace_gauge_seconds_to_fill.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "sweep_box_half_width").InnerText = sweep_box_half_width.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "sweep_box_min_half_length").InnerText = sweep_box_min_half_length.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "Vent_Attract_Time_Min").InnerText = Vent_Attract_Time_Min.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "AreaSweep", "Vent_Attract_Time_Max").InnerText = Vent_Attract_Time_Max.Text;

            ConfigEditorUtils.EnsureChildElements(ammo, "BackstageAreaSweep", "role_timeout_min").InnerText = role_timeout_min.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "BackstageAreaSweep", "role_timeout_max").InnerText = role_timeout_max.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "BackstageAreaSweep", "min_distance").InnerText = min_distance.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "BackstageAreaSweep", "max_distance").InnerText = max_distance.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "BackstageAreaSweep", "min_idle_time").InnerText = min_idle_time.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "BackstageAreaSweep", "max_idle_time").InnerText = max_idle_time.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "BackstageAreaSweep", "killtrap_time").InnerText = killtrap_time.Text;
            ConfigEditorUtils.EnsureChildElements(ammo, "BackstageAreaSweep", "ambush_timeout").InnerText = ambush_timeout.Text;

            _selectedConfig[0].Content = doc;
            _selectedConfig[0].Save();
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/configs/alien-configs");
        }
    }
}
