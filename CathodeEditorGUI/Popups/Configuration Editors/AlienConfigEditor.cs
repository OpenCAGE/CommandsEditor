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

            SetNumber(_selectedConfig, decrease_sweep_duration, "AreaSweep", "decrease_sweep_duration");
            SetNumber(_selectedConfig, increase_sweep_duration, "AreaSweep", "increase_sweep_duration");
            SetNumber(_selectedConfig, near_target_exclusion_radius_first_stalk_min, "AreaSweep", "near_target_exclusion_radius_first_stalk_min");
            SetNumber(_selectedConfig, near_target_exclusion_radius_first_stalk_max, "AreaSweep", "near_target_exclusion_radius_first_stalk_max");
            SetNumber(_selectedConfig, near_target_exclusion_radius_subsequent_stalk_min, "AreaSweep", "near_target_exclusion_radius_subsequent_stalk_min");
            SetNumber(_selectedConfig, near_target_exclusion_radius_subsequent_stalk_max, "AreaSweep", "near_target_exclusion_radius_subsequent_stalk_max");
            SetNumber(_selectedConfig, near_objective_exclusion_radius_first_stalk_min, "AreaSweep", "near_objective_exclusion_radius_first_stalk_min");
            SetNumber(_selectedConfig, near_objective_exclusion_radius_first_stalk_max, "AreaSweep", "near_objective_exclusion_radius_first_stalk_max");
            SetNumber(_selectedConfig, near_objective_exclusion_radius_subsequent_stalk_min, "AreaSweep", "near_objective_exclusion_radius_subsequent_stalk_min");
            SetNumber(_selectedConfig, near_objective_exclusion_radius_subsequent_stalk_max, "AreaSweep", "near_objective_exclusion_radius_subsequent_stalk_max");
            SetNumber(_selectedConfig, menace_gauge_decrease_time, "AreaSweep", "menace_gauge_decrease_time");
            SetNumber(_selectedConfig, meance_deemed_time, "AreaSweep", "meance_deemed_time");
            SetNumber(_selectedConfig, max_menaces, "AreaSweep", "max_menaces");
            SetNumber(_selectedConfig, menace_gauge_seconds_to_fill, "AreaSweep", "menace_gauge_seconds_to_fill");
            SetNumber(_selectedConfig, sweep_box_half_width, "AreaSweep", "sweep_box_half_width");
            SetNumber(_selectedConfig, sweep_box_min_half_length, "AreaSweep", "sweep_box_min_half_length");
            SetNumber(_selectedConfig, Vent_Attract_Time_Min, "AreaSweep", "Vent_Attract_Time_Min");
            SetNumber(_selectedConfig, Vent_Attract_Time_Max, "AreaSweep", "Vent_Attract_Time_Max");

            SetNumber(_selectedConfig, role_timeout_min, "BackstageAreaSweep", "role_timeout_min");
            SetNumber(_selectedConfig, role_timeout_max, "BackstageAreaSweep", "role_timeout_max");
            SetNumber(_selectedConfig, min_distance, "BackstageAreaSweep", "min_distance");
            SetNumber(_selectedConfig, max_distance, "BackstageAreaSweep", "max_distance");
            SetNumber(_selectedConfig, min_idle_time, "BackstageAreaSweep", "min_idle_time");
            SetNumber(_selectedConfig, max_idle_time, "BackstageAreaSweep", "max_idle_time");
            SetNumber(_selectedConfig, killtrap_time, "BackstageAreaSweep", "killtrap_time");
            SetNumber(_selectedConfig, ambush_timeout, "BackstageAreaSweep", "ambush_timeout");
        }

        private void SetNumber(List<BML> configs, NumericUpDown updown, string parentVal, string val)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].Content["AlienConfig"][parentVal]?[val]?.InnerText == null)
                    continue;
                updown.Value = Convert.ToDecimal(configs[i].Content["AlienConfig"][parentVal][val].InnerText);

                if (i != 0)
                    Console.WriteLine("Inherited " + parentVal + " " + val + " value of " + updown.Value + " from " + configs[i].Filepath);
                break;
            }
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

            EnsureChildElements(ammo, "AreaSweep", "decrease_sweep_duration").InnerText = decrease_sweep_duration.Text;
            EnsureChildElements(ammo, "AreaSweep", "increase_sweep_duration").InnerText = increase_sweep_duration.Text;
            EnsureChildElements(ammo, "AreaSweep", "near_target_exclusion_radius_first_stalk_min").InnerText = near_target_exclusion_radius_first_stalk_min.Text;
            EnsureChildElements(ammo, "AreaSweep", "near_target_exclusion_radius_first_stalk_max").InnerText = near_target_exclusion_radius_first_stalk_max.Text;
            EnsureChildElements(ammo, "AreaSweep", "near_target_exclusion_radius_subsequent_stalk_min").InnerText = near_target_exclusion_radius_subsequent_stalk_min.Text;
            EnsureChildElements(ammo, "AreaSweep", "near_target_exclusion_radius_subsequent_stalk_max").InnerText = near_target_exclusion_radius_subsequent_stalk_max.Text;
            EnsureChildElements(ammo, "AreaSweep", "near_objective_exclusion_radius_first_stalk_min").InnerText = near_objective_exclusion_radius_first_stalk_min.Text;
            EnsureChildElements(ammo, "AreaSweep", "near_objective_exclusion_radius_first_stalk_max").InnerText = near_objective_exclusion_radius_first_stalk_max.Text;
            EnsureChildElements(ammo, "AreaSweep", "near_objective_exclusion_radius_subsequent_stalk_min").InnerText = near_objective_exclusion_radius_subsequent_stalk_min.Text;
            EnsureChildElements(ammo, "AreaSweep", "near_objective_exclusion_radius_subsequent_stalk_max").InnerText = near_objective_exclusion_radius_subsequent_stalk_max.Text;
            EnsureChildElements(ammo, "AreaSweep", "menace_gauge_decrease_time").InnerText = menace_gauge_decrease_time.Text;
            EnsureChildElements(ammo, "AreaSweep", "meance_deemed_time").InnerText = meance_deemed_time.Text;
            EnsureChildElements(ammo, "AreaSweep", "max_menaces").InnerText = max_menaces.Text;
            EnsureChildElements(ammo, "AreaSweep", "menace_gauge_seconds_to_fill").InnerText = menace_gauge_seconds_to_fill.Text;
            EnsureChildElements(ammo, "AreaSweep", "sweep_box_half_width").InnerText = sweep_box_half_width.Text;
            EnsureChildElements(ammo, "AreaSweep", "sweep_box_min_half_length").InnerText = sweep_box_min_half_length.Text;
            EnsureChildElements(ammo, "AreaSweep", "Vent_Attract_Time_Min").InnerText = Vent_Attract_Time_Min.Text;
            EnsureChildElements(ammo, "AreaSweep", "Vent_Attract_Time_Max").InnerText = Vent_Attract_Time_Max.Text;

            EnsureChildElements(ammo, "BackstageAreaSweep", "role_timeout_min").InnerText = role_timeout_min.Text;
            EnsureChildElements(ammo, "BackstageAreaSweep", "role_timeout_max").InnerText = role_timeout_max.Text;
            EnsureChildElements(ammo, "BackstageAreaSweep", "min_distance").InnerText = min_distance.Text;
            EnsureChildElements(ammo, "BackstageAreaSweep", "max_distance").InnerText = max_distance.Text;
            EnsureChildElements(ammo, "BackstageAreaSweep", "min_idle_time").InnerText = min_idle_time.Text;
            EnsureChildElements(ammo, "BackstageAreaSweep", "max_idle_time").InnerText = max_idle_time.Text;
            EnsureChildElements(ammo, "BackstageAreaSweep", "killtrap_time").InnerText = killtrap_time.Text;
            EnsureChildElements(ammo, "BackstageAreaSweep", "ambush_timeout").InnerText = ambush_timeout.Text;

            _selectedConfig[0].Content = doc;
            _selectedConfig[0].Save();
        }

        private XmlElement EnsureChildElements(XmlNode parent, params string[] localNames)
        {
            XmlNode current = parent;
            XmlDocument document = parent as XmlDocument ?? parent.OwnerDocument;
            foreach (string name in localNames)
            {
                XmlElement match = null;
                foreach (XmlNode child in current.ChildNodes)
                {
                    if (child is XmlElement el && el.LocalName == name)
                    {
                        match = el;
                        break;
                    }
                }
                if (match == null)
                {
                    match = document.CreateElement(name);
                    current.AppendChild(match);
                }
                current = match;
            }
            return (XmlElement)current;
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/configs/alien-configs");
        }
    }
}
