using CATHODE;
using CATHODE.Enums;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.ConfigEditors
{
    public partial class AmmoEditor : BaseWindow
    {
        List<BML> _selectedAmmo;

        public AmmoEditor() : base()
        {
            InitializeComponent();

            Damage_1.BeginUpdate();
            Damage_2.BeginUpdate();
            Damage_3.BeginUpdate();
            foreach (DAMAGE_EFFECT_TYPE_FLAGS flag in Enum.GetValues(typeof(DAMAGE_EFFECT_TYPE_FLAGS)))
            {
                if ((int)flag == -1)
                    continue;

                Damage_1.Items.Add(flag.ToString());
                Damage_2.Items.Add(flag.ToString());
                Damage_3.Items.Add(flag.ToString());
            }
            Damage_1.EndUpdate();
            Damage_2.EndUpdate();
            Damage_3.EndUpdate();

            BML ammoTypes = new BML(Singleton.PathToAI + "\\DATA\\WEAPON_INFO\\AMMO\\AMMOTYPES.BML");
            var ammos = ammoTypes.Content["AmmoTypes"];
            classSelection.BeginUpdate();
            foreach (XmlElement ammo in ammos)
            {
                classSelection.Items.Add(ammo["Name"].InnerText);
            }
            classSelection.EndUpdate();
            classSelection.SelectedIndex = 0;
        }

        private void classSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (classSelection.Text == "")
            {
                MessageBox.Show("Please select an ammo type first.");
                return;
            }

            _selectedAmmo = new List<BML>();
            _selectedAmmo.Add(new BML(Singleton.PathToAI + "\\DATA\\WEAPON_INFO\\AMMO\\" + classSelection.Text + ".BML"));
            while (true)
            {
                string template = _selectedAmmo[_selectedAmmo.Count - 1].Content["Ammo"]["Template_Name"]?.InnerText;
                if (template == null || template == "") break;
                _selectedAmmo.Add(new BML(Singleton.PathToAI + "\\DATA\\WEAPON_INFO\\AMMO\\" + template + ".BML"));
            }

            SetCheckbox(_selectedAmmo, Projectile, "Hand_Weapon_Data", "Projectile");
            SetCheckbox(_selectedAmmo, Flamethrower, "Hand_Weapon_Data", "Flamethrower");
            SetNumber(_selectedAmmo, damage_rays_per_shot, "Hand_Weapon_Data", "damage_rays_per_shot");
            SetCheckbox(_selectedAmmo, damage_rays_blocked_by_characters, "Hand_Weapon_Data", "damage_rays_blocked_by_characters");
            SetCheckbox(_selectedAmmo, use_fixed_accuracy, "Hand_Weapon_Data", "use_fixed_accuracy");
            SetNumber(_selectedAmmo, fixed_accuracy, "Hand_Weapon_Data", "fixed_accuracy");
            SetNumber(_selectedAmmo, npc_accuracy_multiplier, "Hand_Weapon_Data", "npc_accuracy_multiplier");
            SetNumber(_selectedAmmo, min_accuracy_radius_at_10_metres, "Hand_Weapon_Data", "min_accuracy_radius_at_10_metres");
            SetNumber(_selectedAmmo, max_accuracy_radius_at_10_metres, "Hand_Weapon_Data", "max_accuracy_radius_at_10_metres");
            SetCheckbox(_selectedAmmo, is_fuel, "Hand_Weapon_Data", "is_fuel");
            SetNumber(_selectedAmmo, fuel_units_consumed_per_second_if_firing, "Hand_Weapon_Data", "fuel_units_consumed_per_second_if_firing");
            SetNumber(_selectedAmmo, fuel_units_consumed_per_second_if_switched_on, "Hand_Weapon_Data", "fuel_units_consumed_per_second_if_switched_on");
            SetNumber(_selectedAmmo, projectile_units_consumed_per_shot, "Hand_Weapon_Data", "projectile_units_consumed_per_shot");

            SetNumber(_selectedAmmo, min_distance, "damage_ranges", "min_distance");
            damageRanges.BeginUpdate();
            damageRanges.Items.Clear();
            foreach (XmlElement range_damage in _selectedAmmo[0].Content["Ammo"]["damage_ranges"]["range_damage_list"])
            {
                damageRanges.Items.Add(range_damage.GetAttribute("range"));
            }
            damageRanges.EndUpdate();
            damageRanges.SelectedIndex = 0;

            SetCheckbox(_selectedAmmo, has_physics_response, "Physics_response_at_impact_point", "has_physics_response");
            SetNumber(_selectedAmmo, impulse_radius, "Physics_response_at_impact_point", "impulse_radius");
            SetNumber(_selectedAmmo, impulse_at_centre_of_blast, "Physics_response_at_impact_point", "impulse_at_centre_of_blast");
            SetNumber(_selectedAmmo, impulse_fall_off_power, "Physics_response_at_impact_point", "impulse_fall_off_power");
            SetNumber(_selectedAmmo, character_wavefront_speed, "Physics_response_at_impact_point", "character_wavefront_speed");
        }

        private void SetCheckbox(List<BML> configs, CheckBox checkbox, string parentVal, string val)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].Content["Ammo"][parentVal]?[val]?.InnerText == null)
                    continue;
                checkbox.Checked = configs[i].Content["Ammo"][parentVal][val].InnerText.ToUpper() == "TRUE";

                if (i != 0)
                    Console.WriteLine("Inherited " + parentVal + " " + val + " value of " + checkbox.Checked + " from " + configs[i].Filepath);
                break;
            }
        }
        private void SetNumber(List<BML> configs, NumericUpDown updown, string parentVal, string val)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].Content["Ammo"][parentVal]?[val]?.InnerText == null)
                    continue;
                updown.Value = Convert.ToDecimal(configs[i].Content["Ammo"][parentVal][val].InnerText);

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

            var doc = _selectedAmmo[0].Content;
            XmlElement ammo = doc["Ammo"];

            EnsureChildElements(ammo, "Hand_Weapon_Data", "Projectile").InnerText = Projectile.Checked.ToString();
            EnsureChildElements(ammo, "Hand_Weapon_Data", "Flamethrower").InnerText = Flamethrower.Checked.ToString();
            EnsureChildElements(ammo, "Hand_Weapon_Data", "damage_rays_per_shot").InnerText = damage_rays_per_shot.Text;
            EnsureChildElements(ammo, "Hand_Weapon_Data", "damage_rays_blocked_by_characters").InnerText = damage_rays_blocked_by_characters.Checked.ToString();
            EnsureChildElements(ammo, "Hand_Weapon_Data", "use_fixed_accuracy").InnerText = use_fixed_accuracy.Checked.ToString();
            EnsureChildElements(ammo, "Hand_Weapon_Data", "fixed_accuracy").InnerText = fixed_accuracy.Text;
            EnsureChildElements(ammo, "Hand_Weapon_Data", "npc_accuracy_multiplier").InnerText = npc_accuracy_multiplier.Text;
            EnsureChildElements(ammo, "Hand_Weapon_Data", "min_accuracy_radius_at_10_metres").InnerText = min_accuracy_radius_at_10_metres.Text;
            EnsureChildElements(ammo, "Hand_Weapon_Data", "max_accuracy_radius_at_10_metres").InnerText = max_accuracy_radius_at_10_metres.Text;
            EnsureChildElements(ammo, "Hand_Weapon_Data", "is_fuel").InnerText = is_fuel.Checked.ToString();
            EnsureChildElements(ammo, "Hand_Weapon_Data", "fuel_units_consumed_per_second_if_firing").InnerText = fuel_units_consumed_per_second_if_firing.Text;
            EnsureChildElements(ammo, "Hand_Weapon_Data", "fuel_units_consumed_per_second_if_switched_on").InnerText = fuel_units_consumed_per_second_if_switched_on.Text;
            EnsureChildElements(ammo, "Hand_Weapon_Data", "projectile_units_consumed_per_shot").InnerText = projectile_units_consumed_per_shot.Text;

            EnsureChildElements(ammo, "damage_ranges", "min_distance").InnerText = min_distance.Text;

            EnsureChildElements(ammo, "Physics_response_at_impact_point", "has_physics_response").InnerText = has_physics_response.Checked.ToString();
            EnsureChildElements(ammo, "Physics_response_at_impact_point", "impulse_radius").InnerText = impulse_radius.Text;
            EnsureChildElements(ammo, "Physics_response_at_impact_point", "impulse_at_centre_of_blast").InnerText = impulse_at_centre_of_blast.Text;
            EnsureChildElements(ammo, "Physics_response_at_impact_point", "impulse_fall_off_power").InnerText = impulse_fall_off_power.Text;
            EnsureChildElements(ammo, "Physics_response_at_impact_point", "character_wavefront_speed").InnerText = character_wavefront_speed.Text;

            _selectedAmmo[0].Content = doc;
            _selectedAmmo[0].Save();
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

        private void damageRanges_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (damageRanges.Text == "")
                return;

            foreach (XmlElement range_damage in _selectedAmmo[0].Content["Ammo"]["damage_ranges"]["range_damage_list"]) 
            {
                if (range_damage.GetAttribute("range") != damageRanges.Text)
                    continue;

                vs_NPC.Text = range_damage.GetAttribute("vs_NPC");
                vsPlayer.Text = range_damage.GetAttribute("vsPlayer");
                vsAndroid.Text = range_damage.GetAttribute("vsAndroid");
                vsAndroidHeavy.Text = range_damage.GetAttribute("vsAndroidHeavy");
                vsFHugger.Text = range_damage.GetAttribute("vsFHugger");
                vsPhysics.Text = range_damage.GetAttribute("vsPhysics");
                headshot.Text = range_damage.GetAttribute("headshot");
                Damage_1.SelectedItem = range_damage.GetAttribute("Damage_1").ToUpper();
                Damage_2.SelectedItem = range_damage.GetAttribute("Damage_2").ToUpper();
                Damage_3.SelectedItem = range_damage.GetAttribute("Damage_3").ToUpper();
                Ragdoll.Text = range_damage.GetAttribute("Ragdoll");
                vsAlien.Text = range_damage.GetAttribute("vsAlien");
                AlienStun.Text = range_damage.GetAttribute("AlienStun");
                StunDuration.Text = range_damage.GetAttribute("StunDuration");
                EMPDuration.Text = range_damage.GetAttribute("EMPDuration");
                BlindDuration.Text = range_damage.GetAttribute("BlindDuration");
            }
        }

        private void saveRange_Click(object sender, EventArgs e)
        {
            if (damageRanges.Text == "")
                return;

            var doc = _selectedAmmo[0].Content;

            foreach (XmlElement range_damage in doc["Ammo"]["damage_ranges"]["range_damage_list"])
            {
                if (range_damage.GetAttribute("range") != damageRanges.Text)
                    continue;

                range_damage.SetAttribute("vs_NPC", vs_NPC.Text);
                range_damage.SetAttribute("vsPlayer", vsPlayer.Text);
                range_damage.SetAttribute("vsAndroid", vsAndroid.Text);
                range_damage.SetAttribute("vsAndroidHeavy", vsAndroidHeavy.Text);
                range_damage.SetAttribute("vsFHugger", vsFHugger.Text);
                range_damage.SetAttribute("vsPhysics", vsPhysics.Text);
                range_damage.SetAttribute("headshot", headshot.Text);
                range_damage.SetAttribute("Damage_1", Damage_1.Text);
                range_damage.SetAttribute("Damage_2", Damage_2.Text);
                range_damage.SetAttribute("Damage_3", Damage_3.Text);
                range_damage.SetAttribute("Ragdoll", Ragdoll.Text);
                range_damage.SetAttribute("vsAlien", vsAlien.Text);
                range_damage.SetAttribute("AlienStun", AlienStun.Text);
                range_damage.SetAttribute("StunDuration", StunDuration.Text);
                range_damage.SetAttribute("EMPDuration", EMPDuration.Text);
                range_damage.SetAttribute("BlindDuration", BlindDuration.Text);
            }

            _selectedAmmo[0].Content = doc;
            _selectedAmmo[0].Save();
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/configs/ammo");
        }
    }
}
